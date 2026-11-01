"""Production subprocess tests. Loopback fixtures are NOT game evidence."""
import json
import os
from pathlib import Path
import subprocess
import sys
import unittest

ROOT = Path(__file__).resolve().parents[1]


def run_cli(*args):
    return subprocess.run([sys.executable, str(ROOT / 's1.py'), *args], capture_output=True, text=True, timeout=15)


class Bridge:
    """Real framed TCP fixture, checking the client's acknowledgement."""
    def __init__(self, replies):
        import socket
        self.socket = socket.socket()
        self.socket.bind(('127.0.0.1', 0))
        self.socket.listen()
        self.socket.settimeout(3)
        self.port = self.socket.getsockname()[1]
        self.replies = replies
        self.requests = []
        self.errors = []

    def __enter__(self):
        import threading
        self.thread = threading.Thread(target=self.serve, daemon=True)
        self.thread.start()
        return self

    def serve(self):
        import struct
        def read(sock):
            def exact(n):
                out = b''
                while len(out) < n:
                    chunk = sock.recv(n - len(out))
                    if not chunk:
                        raise EOFError('no acknowledgement')
                    out += chunk
                return out
            return json.loads(exact(struct.unpack('<I', exact(4))[0]))
        try:
            for method, result in self.replies:
                sock, _ = self.socket.accept()
                with sock:
                    sock.settimeout(3)
                    request = read(sock)
                    self.requests.append(request)
                    assert request['method'] == method, request
                    response = {'id': request['id'], 'result': result}
                    if callable(result):
                        response = result(request)
                    responses = response if isinstance(response, list) else [response]
                    for item in responses:
                        if isinstance(item, bytes):
                            sock.sendall(item)
                            continue
                        payload = json.dumps(item).encode()
                        # Fragmented writes exercise exact-read logic.
                        wire = struct.pack('<I', len(payload)) + payload
                        sock.sendall(wire[:2])
                        sock.sendall(wire[2:])
                    ack = read(sock)
                    assert ack == {'id': request['id'], 'status': 'received'}, ack
        except Exception as exc:
            self.errors.append(str(exc))

    def __exit__(self, *args):
        self.thread.join(4)
        self.socket.close()


class CliTests(unittest.TestCase):
    def test_status_correlated_tcp_and_ack(self):
        with Bridge([('get_game_state', {'scene_name': 'Menu'})]) as bridge:
            result = run_cli('--port', str(bridge.port), '--json', 'status')
        self.assertEqual(0, result.returncode, result.stderr + result.stdout)
        value = json.loads(result.stdout)
        self.assertEqual(1, value['schema_version'])
        self.assertTrue(value['ok'])
        self.assertEqual('Menu', value['data']['scene_name'])
        self.assertFalse(bridge.errors)

    def test_capabilities_and_time_read(self):
        for command, method, payload in [
            (['capabilities'], 'list_methods', {'server_name': 'S1MCPServer', 'version': 'fixture', 'available_methods': ['get_time_of_day']}),
            (['time', 'get'], 'get_time_of_day', {'available': False, 'reason': 'menu'})]:
            with self.subTest(command=command):
                with Bridge([(method, payload)]) as bridge:
                    result = run_cli('--json', '--port', str(bridge.port), *command)
                self.assertEqual(0, result.returncode, result.stdout + result.stderr)
                self.assertFalse(bridge.errors)
                self.assertEqual(payload, json.loads(result.stdout)['data'])

    def test_doctor_offline_retains_diagnostics(self):
        import tempfile
        import socket
        with tempfile.TemporaryDirectory(dir=os.environ.get('TMPDIR')) as folder:
            with socket.socket() as blocker:
                blocker.bind(('127.0.0.1', 0))
                result = run_cli('--json', '--timeout', '2', '--port', str(blocker.getsockname()[1]), '--game-dir', folder, 'doctor')
        self.assertIn(result.returncode, (3, 4), result.stdout + result.stderr)
        value = json.loads(result.stdout)
        self.assertFalse(value['data']['installed_bridge'])
        self.assertFalse(value['data']['bridge_reachable'])
        self.assertIn('python', value['data'])

    def test_worldlook_status(self):
        status = dict(schemaVersion=1, accepted=True, requestedEnabled=False, ready=False,
                      singleplayer=False, loading=False, hostOrSingleplayer=False,
                      gameLoaded=False, lightingFaulted=False, waterFaulted=False)
        reply = {'method_name': 'GetStatus', 'return_value': {'type': 'String', 'value': json.dumps(status)}}
        with Bridge([('call_method', reply)]) as bridge:
            result = run_cli('--json', '--port', str(bridge.port), 'worldlook', 'status')
        self.assertEqual(0, result.returncode, result.stdout + result.stderr)
        self.assertEqual(status, json.loads(result.stdout)['data'])
        self.assertEqual({'object_name': 'WorldLook.Automation', 'component_type': 'WorldLook.WorldLookAutomation', 'method_name': 'GetStatus'}, bridge.requests[0]['params'])
        self.assertFalse(bridge.errors)

    def test_lighting_only_selected_light_members(self):
        replies = [('get_component_property', {'property_name': name, 'value': 1}) for name in ('color', 'intensity', 'enabled')]
        with Bridge(replies) as bridge:
            result = run_cli('--json', '--port', str(bridge.port), 'lighting', 'inspect', '--object', '/Fixture/Sun')
        self.assertEqual(0, result.returncode, result.stdout + result.stderr)
        self.assertFalse(bridge.errors)
        self.assertEqual(3, len(bridge.requests))
        self.assertTrue(all(r['params']['object_name'] == '/Fixture/Sun' and r['params']['component_type'] == 'UnityEngine.Light' for r in bridge.requests))

    def test_setter_requires_identity_and_delegates_existing_guard(self):
        import importlib.util
        from types import SimpleNamespace
        from unittest.mock import patch
        spec = importlib.util.spec_from_file_location('s1_cli', ROOT / 's1.py')
        assert spec is not None and spec.loader is not None
        cli = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(cli)
        self.assertTrue(hasattr(cli, 'worldlook_set'), 'Missing guarded setter adapter')
        args = SimpleNamespace(action='on', port=8765, save_name='Fixture', save_path=str(ROOT.resolve()),
                               game_process_id=42, game_started_utc='2026-10-26T12:00:00Z',
                               evidence=str(ROOT / 'never-created.jsonl'), game_dir=str(ROOT))
        payload = {'status': {'requestedEnabled': True}, 'process': {'Id': 42}}
        completed = SimpleNamespace(returncode=0, stdout=json.dumps(payload), stderr='')
        with patch.object(cli.shutil, 'which', return_value='pwsh'), patch.object(cli.subprocess, 'run', return_value=completed) as run:
            self.assertEqual(payload, cli.worldlook_set(args, cli.Client(8765, 30)))
        command = run.call_args.args[0]
        self.assertIn(str(ROOT.parent / 'invoke-worldlook-mcp.ps1'), command)
        self.assertIn('-GameProcessId', command)
        self.assertIn('-GameStartedUtc', command)
        self.assertIn('-SavePath', command)
        self.assertIn('-EvidencePath', command)
        self.assertNotIn('shell', run.call_args.kwargs)
        result = run_cli('--json', 'worldlook', 'on')
        self.assertEqual(2, result.returncode)
        self.assertIn('identity', json.loads(result.stdout)['error']['message'])

    def test_capture_checks_capability_and_real_png(self):
        import os
        import tempfile
        import struct
        import zlib
        with tempfile.TemporaryDirectory(dir=os.environ.get('TMPDIR')) as folder:
            png = Path(folder) / 'UserData/S1MCP/Evidence/Screenshots/capture-fixture.png'
            png.parent.mkdir(parents=True)
            def chunk(kind, value):
                return struct.pack('>I', len(value)) + kind + value + struct.pack('>I', zlib.crc32(kind + value))
            content = b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', struct.pack('>IIBBBBB', 1, 1, 8, 6, 0, 0, 0)) + chunk(b'IDAT', zlib.compress(b'\x00\xff\x00\x00\xff')) + chunk(b'IEND', b'')
            png.write_bytes(content)
            payload = dict(schema_version=1, success=True, path=str(png.resolve()), width=1, height=1, bytes=len(content), includes_ui=True, capture_source='Unity.ScreenCapture/end-of-frame')
            capabilities = {'available_methods': ['capture_screenshot']}
            with Bridge([('list_methods', capabilities), ('capture_screenshot', payload)]) as bridge:
                result = run_cli('--json', '--port', str(bridge.port), '--game-dir', folder, 'capture')
            self.assertEqual(0, result.returncode, result.stdout + result.stderr)
            self.assertTrue(json.loads(result.stdout)['data']['png_verified'])
            self.assertFalse(bridge.errors)
            self.assertEqual({'timeout_ms': 5000}, bridge.requests[1]['params'])
            png.write_bytes(content[:-4] + b'bad!')
            with Bridge([('list_methods', capabilities), ('capture_screenshot', payload)]) as bridge:
                result = run_cli('--json', '--port', str(bridge.port), '--game-dir', folder, 'capture')
            self.assertEqual(8, result.returncode, result.stdout + result.stderr)
            self.assertFalse(json.loads(result.stdout)['ok'])

    def test_capture_unsupported_never_requests_capture(self):
        with Bridge([('list_methods', {'available_methods': []})]) as bridge:
            result = run_cli('--json', '--port', str(bridge.port), 'capture')
        self.assertEqual(6, result.returncode, result.stdout + result.stderr)
        self.assertEqual(1, len(bridge.requests))
        self.assertFalse(bridge.errors)

    def test_nonfinite_response_is_protocol_error(self):
        with Bridge([('get_game_state', {'value': float('nan')})]) as bridge:
            result = run_cli('--json', '--port', str(bridge.port), 'status')
        self.assertEqual(5, result.returncode, result.stdout + result.stderr)
        self.assertFalse(json.loads(result.stdout)['ok'])

    def test_doctor_reports_process_identity(self):
        with Bridge([('list_methods', {'available_methods': []})]) as bridge:
            result = run_cli('--json', '--port', str(bridge.port), 'doctor')
        self.assertEqual(0, result.returncode, result.stdout + result.stderr)
        local = json.loads(result.stdout)['data']
        self.assertIn('process_observation', local)
        self.assertIn('available', local['process_observation'])
        self.assertFalse(bridge.errors)

    def test_windows_s1_launcher(self):
        if os.name != 'nt':
            self.skipTest('Windows launcher')
        result = subprocess.run(['cmd.exe', '/d', '/c', str(ROOT / 's1.cmd'), '--help'], capture_output=True, text=True, timeout=10)
        self.assertEqual(0, result.returncode, result.stderr)
        self.assertIn('worldlook', result.stdout)

    def test_protocol_rejects_wrong_id_and_bad_lengths(self):
        import struct
        cases = [lambda r: {'id': str(r['id']), 'result': {}},
                 lambda r: {'id': r['id'] + 1, 'result': {}},
                 lambda r: {'id': True, 'result': {}},
                 lambda r: struct.pack('<I', 0),
                 lambda r: struct.pack('<I', 1048577),
                 lambda r: struct.pack('<I', 4) + b'nope']
        for response in cases:
            with self.subTest(response=response):
                with Bridge([('get_game_state', response)]) as bridge:
                    result = run_cli('--json', '--port', str(bridge.port), 'status')
                self.assertEqual(5, result.returncode, result.stdout + result.stderr)
                self.assertEqual(1, len(bridge.requests))

    def test_heartbeat_and_bridge_errors_are_acknowledged(self):
        def reply(r):
            return [{'id': -1, 'result': {'type': 'server_heartbeat', 'status': 'alive'}},
                    {'id': r['id'], 'error': {'code': -32601, 'message': 'Not supported'}}]
        with Bridge([('get_game_state', reply)]) as bridge:
            result = run_cli('--json', '--port', str(bridge.port), 'status')
        self.assertEqual(6, result.returncode, result.stdout + result.stderr)
        self.assertFalse(bridge.errors)

    def test_socket_deadline_never_retries(self):
        import time
        def stalled(r):
            time.sleep(0.5)
            return {'id': r['id'], 'result': {}}
        with Bridge([('get_game_state', stalled)]) as bridge:
            started = time.monotonic()
            result = run_cli('--json', '--timeout', '0.1', '--port', str(bridge.port), 'status')
            elapsed = time.monotonic() - started
        self.assertEqual(4, result.returncode, result.stdout + result.stderr)
        self.assertLess(elapsed, 1.5)
        self.assertEqual(1, len(bridge.requests))

    def test_input_errors_are_json_and_do_not_expose_exec(self):
        for command in [('exec', 'anything'), ('time', 'set'), ('load',), ('--timeout', 'nan', 'status'),
                        ('--port', '0', 'status'), ('capture', '--timeout-ms', '99'),
                        ('lighting', 'inspect', '--object', '*Sun*')]:
            with self.subTest(command=command):
                result = run_cli('--json', *command)
                self.assertEqual(2, result.returncode, result.stdout + result.stderr)
                self.assertEqual([], json.loads(result.stdout)['requests'])

    def test_help(self):
        result = run_cli('--help')
        self.assertEqual(0, result.returncode, result.stderr)
        self.assertIn('capabilities', result.stdout)


if __name__ == '__main__':
    unittest.main()
