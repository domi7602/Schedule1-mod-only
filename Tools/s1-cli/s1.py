"""Local Schedule I CLI. No third-party Python dependencies."""
import argparse
import json
import math
import zlib
import os
from pathlib import Path
import shutil
import secrets
import socket
import struct
import subprocess
from datetime import datetime
import sys
import time


def strict_json(value):
    def finite(number):
        parsed = float(number)
        if not math.isfinite(parsed):
            raise ValueError('Nonfinite JSON number')
        return parsed
    def reject(value):
        raise ValueError('Invalid JSON constant: ' + value)
    return json.loads(value, parse_constant=reject, parse_float=finite)


class Failure(Exception):
    def __init__(self, code, message, data=None):
        super().__init__(message)
        self.code, self.data = code, data


class Client:
    def __init__(self, port, timeout):
        self.port = port
        self.deadline = time.monotonic() + timeout
        self.trace = []

    def remaining(self):
        remaining = self.deadline - time.monotonic()
        if remaining <= 0:
            raise Failure(4, 'Command deadline exceeded; no retry.')
        return remaining

    def request(self, method, params=None):
        request = {'id': secrets.randbelow(999990000) + 10000, 'method': method, 'params': params or {}}
        record = {'request': request}
        self.trace.append(record)
        try:
            with socket.create_connection(('127.0.0.1', self.port), self.remaining()) as sock:
                def send(value):
                    payload = json.dumps(value, allow_nan=False, separators=(',', ':')).encode('utf-8')
                    sock.settimeout(self.remaining())
                    sock.sendall(struct.pack('<I', len(payload)) + payload)

                def read(size):
                    out = bytearray()
                    while len(out) < size:
                        sock.settimeout(self.remaining())
                        chunk = sock.recv(size - len(out))
                        if not chunk:
                            raise Failure(5, 'Stream closed before frame completed.')
                        out.extend(chunk)
                    return bytes(out)

                send(request)
                for _ in range(8):
                    length = struct.unpack('<I', read(4))[0]
                    if not 0 < length <= 1048576:
                        raise Failure(5, 'Invalid or excessive response length.')
                    response = strict_json(read(length))
                    if not isinstance(response, dict):
                        raise Failure(5, 'Response must be an object.')
                    rid = response.get('id')
                    result = response.get('result')
                    if (type(rid) is int and rid < 0 and isinstance(result, dict)
                            and result.get('type') == 'server_heartbeat' and result.get('status') == 'alive'):
                        continue
                    if type(rid) is not int or rid != request['id']:
                        raise Failure(5, 'Response ID mismatch; no retry.')
                    record['response'] = response
                    send({'id': rid, 'status': 'received'})
                    if response.get('error'):
                        raise Failure(6, 'Bridge rejected request.', response['error'])
                    if not isinstance(result, dict):
                        raise Failure(5, 'Response has no object result.')
                    return result
                raise Failure(5, 'Response frame budget exceeded.')
        except (TimeoutError, socket.timeout) as exc:
            raise Failure(4, 'Bridge deadline exceeded; outcome may be unknown, no retry.') from exc
        except OSError as exc:
            raise Failure(3, 'Bridge unavailable: ' + str(exc)) from exc
        except (ValueError, UnicodeError, RecursionError) as exc:
            raise Failure(5, 'Invalid response JSON: ' + str(exc)) from exc


def process_observation(client):
    pwsh = shutil.which('pwsh')
    if os.name != 'nt' or not pwsh:
        return {'available': False, 'error': 'Windows and PowerShell 7 required for local process identity.'}
    try:
        result = subprocess.run([pwsh, '-NoProfile', '-ExecutionPolicy', 'Bypass', '-File',
            str(Path(__file__).resolve().parent / 'process-info.ps1'), '-Port', str(client.port)],
            capture_output=True, text=True, encoding='utf-8', timeout=min(3, client.remaining()))
        value = strict_json(result.stdout)
        if not isinstance(value, dict) or type(value.get('available')) is not bool:
            raise ValueError('Invalid process observation')
        return value
    except (OSError, ValueError, subprocess.TimeoutExpired, Failure) as exc:
        return {'available': False, 'error': str(exc)}


def worldlook_set(args, client):
    if (args.port != 8765 or not args.save_name or not args.save_path or
            not Path(args.save_path).is_absolute() or not args.game_process_id or args.game_process_id < 1 or
            not args.game_started_utc or not args.evidence):
        raise Failure(2, 'WorldLook on/off require process/save identity: --save-name, absolute --save-path, --game-process-id, --game-started-utc, --evidence; port must be 8765.')
    try:
        started = datetime.fromisoformat(args.game_started_utc.replace('Z', '+00:00'))
        if started.tzinfo is None:
            raise ValueError('Timezone required')
    except ValueError as exc:
        raise Failure(2, 'Process start must be an ISO-8601 timestamp with timezone.') from exc
    pwsh = shutil.which('pwsh')
    if not pwsh:
        raise Failure(3, 'PowerShell 7 is required for the existing guarded WorldLook driver.')
    remaining = client.remaining()
    if remaining < 5:
        raise Failure(2, 'WorldLook setters require --timeout of at least 6 seconds.')
    script = Path(__file__).resolve().parent.parent / 'invoke-worldlook-mcp.ps1'
    command = [pwsh, '-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', str(script),
               '-Action', args.action.title(), '-SaveName', args.save_name, '-SavePath', args.save_path,
               '-GameDir', str(Path(args.game_dir).resolve()), '-GameProcessId', str(args.game_process_id),
               '-GameStartedUtc', args.game_started_utc, '-EvidencePath', str(Path(args.evidence).resolve()),
               '-TimeoutSeconds', str(int(remaining))]
    try:
        completed = subprocess.run(command, capture_output=True, text=True, encoding='utf-8', timeout=remaining)
    except subprocess.TimeoutExpired as exc:
        raise Failure(4, 'WorldLook driver timed out; effect unknown. Read status/evidence before any further action; no retry.') from exc
    except OSError as exc:
        raise Failure(3, 'Cannot start guarded WorldLook driver: ' + str(exc)) from exc
    if completed.returncode:
        raise Failure(7, 'Guarded WorldLook action failed; inspect evidence before retrying.', {'stderr': completed.stderr[-8192:], 'stdout': completed.stdout[-8192:]})
    try:
        result = strict_json(completed.stdout)
        if not isinstance(result, dict) or not isinstance(result.get('status'), dict) or result['status'].get('requestedEnabled') is not (args.action == 'on'):
            raise ValueError('No verified setter readback')
        return result
    except ValueError as exc:
        raise Failure(5, 'Invalid guarded driver result; effect unknown, no retry.') from exc


def worldlook_status(client):
    result = client.request('call_method', {'object_name': 'WorldLook.Automation',
        'component_type': 'WorldLook.WorldLookAutomation', 'method_name': 'GetStatus'})
    value = result.get('return_value')
    if result.get('method_name') != 'GetStatus' or not isinstance(value, dict) or value.get('type') != 'String' or not isinstance(value.get('value'), str):
        raise Failure(5, 'WorldLook endpoint did not return explicit JSON status.')
    try:
        status = strict_json(value['value'])
    except ValueError as exc:
        raise Failure(5, 'Invalid WorldLook JSON.') from exc
    booleans = ('accepted', 'requestedEnabled', 'ready', 'singleplayer', 'loading',
                'hostOrSingleplayer', 'gameLoaded', 'lightingFaulted', 'waterFaulted')
    if not isinstance(status, dict) or type(status.get('schemaVersion')) is not int or status['schemaVersion'] != 1 or any(type(status.get(k)) is not bool for k in booleans):
        raise Failure(5, 'Incomplete or unsupported WorldLook schema.')
    if not status['accepted'] or status.get('error'):
        raise Failure(6, 'WorldLook rejected status.', status)
    return status


class Parser(argparse.ArgumentParser):
    def error(self, message):
        raise Failure(2, message)


def main(argv=None):
    argv = list(sys.argv[1:] if argv is None else argv)
    parser = Parser(prog='s1', description='Local Schedule I bridge client (no launch/load/save)')
    parser.add_argument('--json', action='store_true', help='Stable JSON envelope')
    parser.add_argument('--port', type=int, default=8765, help='Loopback bridge port (default 8765)')
    parser.add_argument('--timeout', type=float, default=8, help='Whole-command deadline in seconds (0.1..120)')
    parser.add_argument('--game-dir', default=os.environ.get('SCHEDULE1_PATH', 'C:/Program Files (x86)/Steam/steamapps/common/Schedule I'))
    commands = parser.add_subparsers(dest='command', required=True)
    for name in ('status', 'doctor', 'capabilities'):
        commands.add_parser(name)
    commands.add_parser('time').add_argument('action', choices=['get'])
    commands.add_parser('capture').add_argument('--timeout-ms', type=int, default=5000, help='Server capture deadline (100..15000); also allow time for transport and PNG decoding')
    worldlook = commands.add_parser('worldlook')
    worldlook.add_argument('action', choices=['status', 'on', 'off'])
    worldlook.add_argument('--save-name')
    worldlook.add_argument('--save-path')
    worldlook.add_argument('--game-process-id', type=int)
    worldlook.add_argument('--game-started-utc')
    worldlook.add_argument('--evidence', help='New JSONL file (never overwritten)')
    lighting = commands.add_parser('lighting')
    lighting.add_argument('action', choices=['inspect'])
    lighting.add_argument('--object', required=True, help='Exact absolute Unity hierarchy path; no scene scan')
    client = None
    command = None
    data = None
    try:
        args = parser.parse_args(argv)
        command = args.command
        if not 1 <= args.port <= 65535 or not 0.1 <= args.timeout <= 120:
            raise Failure(2, 'Port or timeout out of range.')
        client = Client(args.port, args.timeout)
        if command == 'doctor':
            game = Path(args.game_dir).resolve()
            data = {'python': sys.version.split()[0], 'python_executable': sys.executable,
                    'pwsh': shutil.which('pwsh'), 'game_dir': str(game),
                    'game_executable_exists': (game / 'Schedule I.exe').is_file(),
                    'installed_bridge': (game / 'Mods/S1MCPServer-IL2CPP.dll').is_file(),
                    'bridge_reachable': False, 'endpoint': f'127.0.0.1:{args.port}'}
            try:
                data['capabilities'] = client.request('list_methods')
                data['bridge_reachable'] = True
            finally:
                data['process_observation'] = process_observation(client)
        elif command == 'capture':
            if not 100 <= args.timeout_ms <= 15000:
                raise Failure(2, 'Capture timeout must be 100..15000 ms.')
            capabilities = client.request('list_methods')
            methods = capabilities.get('available_methods')
            if not isinstance(methods, list) or not all(isinstance(m, str) for m in methods):
                raise Failure(5, 'Bridge returned no valid capability list.')
            if 'capture_screenshot' not in methods:
                raise Failure(6, 'Installed bridge does not expose capture_screenshot; no fallback capture performed.')
            data = client.request('capture_screenshot', {'timeout_ms': args.timeout_ms})
            try:
                from pngcheck import verify_png
                data = verify_png(data, args.game_dir, client.remaining)
            except (ValueError, OSError, zlib.error) as exc:
                raise Failure(8, 'PNG validation failed: ' + str(exc)) from exc
        elif command == 'worldlook':
            data = worldlook_status(client) if args.action == 'status' else worldlook_set(args, client)
        elif command == 'lighting':
            if not args.object.startswith('/') or len(args.object) > 512 or any(c in args.object for c in '*?\n\r'):
                raise Failure(2, 'Supply an exact absolute Unity hierarchy path, not a pattern.')
            data = {'object': args.object, 'component': 'UnityEngine.Light', 'properties': {}}
            for member in ('color', 'intensity', 'enabled'):
                value = client.request('get_component_property', {'object_name': args.object, 'component_type': 'UnityEngine.Light', 'property_name': member})
                if value.get('property_name') != member or 'value' not in value or value['value'] is None:
                    raise Failure(5, 'Light property readback is missing or mismatched.', value)
                data['properties'][member] = value['value']
        else:
            method = {'status': 'get_game_state', 'capabilities': 'list_methods', 'time': 'get_time_of_day'}[command]
            data = client.request(method)
        result = {'schema_version': 1, 'ok': True, 'command': command, 'data': data, 'error': None, 'requests': client.trace}
        code = 0
    except Failure as exc:
        code = exc.code
        result = {'schema_version': 1, 'ok': False, 'command': command, 'data': data,
                  'error': {'code': code, 'message': str(exc), 'details': exc.data}, 'requests': client.trace if client else []}
    print(json.dumps(result, ensure_ascii=True, allow_nan=False, indent=None if '--json' in argv else 2))
    return code


if __name__ == '__main__':
    sys.exit(main())
