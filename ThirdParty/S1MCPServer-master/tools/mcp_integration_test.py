#!/usr/bin/env python3
"""Integration test for the hardened S1MCPServer TCP transport (v1.0.3+) and the
quick-win feature endpoints (v1.0.4).

Run with the game running (server listening on 127.0.0.1:8765).

Verifies the robustness contract that the pre-1.0.2 server broke:
  1. baseline request/response works
  2. `status` reports live server state
  3. a client that dies with RST does not block the next client
  4. a half-open (zombie) connection is replaced, not honoured
  5. the server survives all of the above and still answers
  6. the fast path (handshake/list_methods/status) answers on the server thread even while
     the main-thread queue is stalled - the "log in from the main menu" contract
And the v1.0.4 feature endpoints (main-thread handlers; in the main menu they prove routing
  and the clean `available:false` degradation - the true feature paths need an in-game run
  as host):
  7. `list_mods` lists every registered melon including S1MCPServer
  8. `get_time_of_day` reports the in-game clock (or a clean not-available)
  9. `set_time_of_day` applies a clock change with a readback (or a clean not-available)
  10. `get_weather` reports the nine native weather weights (or a clean not-available)

Exit code 0 = all checks passed.
"""
import json
import socket
import struct
import sys
import time

HOST, PORT = "127.0.0.1", 8765
FRAME_TIMEOUT = 20.0


class Client:
    def __init__(self, timeout=FRAME_TIMEOUT):
        self.sock = socket.create_connection((HOST, PORT), timeout=timeout)
        self.sock.settimeout(timeout)
        self._id = 0

    def _send(self, method, params=None):
        self._id += 1
        payload = json.dumps({
            "jsonrpc": "2.0", "id": self._id, "method": method, "params": params or {}
        }).encode("utf-8")
        self.sock.sendall(struct.pack("<I", len(payload)) + payload)
        return self._id

    def _recv_exact(self, n):
        buf = b""
        while len(buf) < n:
            chunk = self.sock.recv(n - len(buf))
            if not chunk:
                raise EOFError("peer closed the connection")
            buf += chunk
        return buf

    def _read_frame(self):
        (length,) = struct.unpack("<I", self._recv_exact(4))
        return json.loads(self._recv_exact(length).decode("utf-8"))

    def request(self, method, params=None, timeout=FRAME_TIMEOUT):
        want = self._send(method, params)
        deadline = time.time() + timeout
        while time.time() < deadline:
            msg = self._read_frame()
            # Server heartbeats and client acks carry no "method"; skip them.
            if isinstance(msg, dict) and msg.get("id") == want and "method" not in msg:
                return msg
        raise TimeoutError(f"no response to '{method}' within {timeout}s")

    def close(self):
        try:
            self.sock.close()
        except Exception:
            pass

    def abort(self):
        """Kill the connection with RST, like a crashed client process."""
        try:
            self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_LINGER, struct.pack("ii", 1, 0))
        except Exception:
            pass
        self.close()


def main():
    results = []
    failed = []

    def check(name, ok, detail):
        results.append({"check": name, "ok": bool(ok), "detail": detail})
        if not ok:
            failed.append(name)
        print(f"[{'PASS' if ok else 'FAIL'}] {name}: {detail}")

    # 1 + 2: baseline and status
    try:
        c = Client()
        state = c.request("get_game_state")
        check("baseline request", "result" in state, f"get_game_state answered (id={state.get('id')})")
        st = c.request("status")["result"]
        check("status command",
              st.get("listener_running") is True and "connection_generation" in st,
              f"listener_running={st.get('listener_running')}, "
              f"client_connected={st.get('client_connected')}, "
              f"generation={st.get('connection_generation')}, "
              f"uptime={st.get('uptime_seconds')}s")
        c.close()
    except Exception as ex:
        check("baseline request", False, f"{type(ex).__name__}: {ex}")
        print(json.dumps({"checks": results, "failed": failed}, indent=2))
        return 1

    time.sleep(1.0)

    # 3: a client that dies hard must not block the next one
    try:
        dead = Client()
        dead.request("status")
        dead.abort()
        time.sleep(1.0)
        t0 = time.time()
        c = Client(timeout=8.0)
        c.request("status")
        elapsed = time.time() - t0
        c.close()
        check("recovery after RST", elapsed < 5.0, f"next client served after {elapsed:.2f}s")
    except Exception as ex:
        check("recovery after RST", False, f"{type(ex).__name__}: {ex}")

    time.sleep(1.0)

    # 4: a half-open socket must be replaced, not honoured
    try:
        zombie = Client()
        zombie.request("status")
        # keep zombie open on purpose - the server sees a connected client
        t0 = time.time()
        c = Client(timeout=8.0)
        st = c.request("status")["result"]
        elapsed = time.time() - t0
        c.close()
        zombie.close()
        replaced = st.get("stale_clients_replaced", 0)
        check("half-open client replaced", replaced >= 1 and elapsed < 5.0,
              f"stale_clients_replaced={replaced}, new client served after {elapsed:.2f}s")
    except Exception as ex:
        check("half-open client replaced", False, f"{type(ex).__name__}: {ex}")

    time.sleep(1.0)

    # 5: still healthy afterwards
    try:
        c = Client()
        st = c.request("status")["result"]
        c.close()
        check("server healthy after abuse",
              st.get("last_error") in (None, "none") or st.get("last_error_seconds_ago", -1) >= 0,
              f"status={st.get('status')}, last_error={st.get('last_error')}, "
              f"command_queue_depth={st.get('command_queue_depth')}")
    except Exception as ex:
        check("server healthy after abuse", False, f"{type(ex).__name__}: {ex}")

    time.sleep(1.0)

    # 6: fast path - connection commands must answer even when the main thread is not draining.
    # This is the contract that makes "log in from the main menu" work: with the game paused,
    # the main-thread queue stalls, but handshake/list_methods/status are served on the server
    # thread and must still come back promptly.
    try:
        c = Client(timeout=10.0)
        hs = c.request("handshake")["result"]
        available = hs.get("available_methods") or []
        check("handshake names the fast-path methods",
              "handshake" in available and "status" in available,
              f"version={hs.get('version')}, total_methods={hs.get('total_methods')}")

        inline_before = c.request("status")["result"].get("inline_requests", 0)

        timings = {}
        for method in ("handshake", "list_methods", "status"):
            t0 = time.time()
            c.request(method, timeout=6.0)
            timings[method] = time.time() - t0

        st = c.request("status")["result"]
        inline_after = st.get("inline_requests", 0)
        c.close()

        slowest = max(timings.values())
        grew = inline_after - inline_before
        check("fast path answers on the server thread",
              slowest < 5.0 and grew >= 3,
              f"handshake={timings['handshake']:.2f}s, list_methods={timings['list_methods']:.2f}s, "
              f"status={timings['status']:.2f}s, "
              f"inline_requests {inline_before}->{inline_after}, "
              f"last_inline_method={st.get('last_inline_method')}")
    except Exception as ex:
        check("fast path answers on the server thread", False, f"{type(ex).__name__}: {ex}")

    # Informational: a main-thread command may legitimately stall while the game is paused.
    try:
        t0 = time.time()
        c2 = Client(timeout=8.0)
        c2.request("get_game_state", timeout=8.0)
        c2.close()
        print(f"[INFO] main-thread command get_game_state answered in {time.time() - t0:.2f}s")
    except Exception as ex:
        print(f"[INFO] main-thread command get_game_state stalled ({type(ex).__name__}) "
              f"- expected while the game is paused or unfocused")

    # 7-10: quick-win feature endpoints (1.0.4). All main-thread handlers. In the main menu
    # they prove routing and the clean available:false degradation (TimeManager and
    # EnvironmentManager have not spawned there); the true feature paths need an in-game
    # run as host. A crash or timeout fails; a structured answer passes. Each check gets
    # its own connection and try/except so one failure does not mask the others.
    c3 = Client(timeout=10.0)
    try:
        mods = c3.request("list_mods", timeout=8.0)
        res = mods.get("result") or {}
        names = [m.get("name", "") for m in (res.get("mods") or [])]
        listed = ("result" in mods
                  and isinstance(res.get("total_count"), int)
                  and res.get("total_count") == len(res.get("mods") or [])
                  and res.get("total_count", 0) > 0
                  and any("S1MCPServer" in n for n in names))
        check("list_mods lists the melons", listed,
              f"total_count={res.get('total_count')}, s1mcp listed={any('S1MCPServer' in n for n in names)}")
    except Exception as ex:
        check("list_mods lists the melons", False, f"{type(ex).__name__}: {ex}")
    finally:
        c3.close()

    c4 = Client(timeout=10.0)
    try:
        tod = c4.request("get_time_of_day", timeout=8.0)
        r2 = tod.get("result") or {}
        if r2.get("available"):
            tm = r2.get("time_minutes")
            ok_tod = ("result" in tod and isinstance(tm, int) and 0 <= tm <= 1439
                      and isinstance(r2.get("display"), str) and ":" in r2.get("display", "")
                      and isinstance(r2.get("elapsed_days"), int) and r2.get("elapsed_days", -1) >= 0)
        else:
            ok_tod = "result" in tod and isinstance(r2.get("reason"), str)
        check("get_time_of_day answers", ok_tod,
              f"available={r2.get('available')}, time_minutes={r2.get('time_minutes')}, "
              f"elapsed_days={r2.get('elapsed_days')}, display={r2.get('display')}, reason={r2.get('reason')}")
    except Exception as ex:
        check("get_time_of_day answers", False, f"{type(ex).__name__}: {ex}")
    finally:
        c4.close()

    c5 = Client(timeout=10.0)
    try:
        stod = c5.request("set_time_of_day", {"time": 12.0}, timeout=8.0)
        r3 = stod.get("result") or {}
        if r3.get("available"):
            # Honest server contract: changed must equal (readback == applied); if changed is
            # false (e.g. a non-host client) the note must explain why.
            ok_stod = ("result" in stod
                       and isinstance(r3.get("changed"), bool)
                       and r3["changed"] == (r3.get("readback_minutes") == r3.get("applied_minutes"))
                       and r3.get("applied_minutes") == 720)
            if not r3.get("changed"):
                ok_stod = ok_stod and isinstance(r3.get("note"), str)
        else:
            ok_stod = "result" in stod and isinstance(r3.get("reason"), str)
        check("set_time_of_day answers", ok_stod,
              f"available={r3.get('available')}, applied={r3.get('applied_minutes')}, "
              f"readback={r3.get('readback_minutes')}, changed={r3.get('changed')}, reason={r3.get('reason')}")
    except Exception as ex:
        check("set_time_of_day answers", False, f"{type(ex).__name__}: {ex}")
    finally:
        c5.close()

    c6 = Client(timeout=10.0)
    try:
        wx = c6.request("get_weather", timeout=8.0)
        r4 = wx.get("result") or {}
        conds = r4.get("conditions") or {}
        keys = {"sunny", "cloudy", "rainy", "stormy", "snowy", "foggy", "windy", "hail", "sleet"}
        if r4.get("available"):
            ok_wx = ("result" in wx and set(conds) >= keys
                     and all(isinstance(v, (int, float)) and 0.0 <= v <= 1.0 for v in (conds.values())))
        else:
            ok_wx = "result" in wx and isinstance(r4.get("reason"), str)
        check("get_weather answers", ok_wx,
              f"available={r4.get('available')}, conditions={conds if r4.get('available') else 'n/a'}, "
              f"reason={r4.get('reason')}")
    except Exception as ex:
        check("get_weather answers", False, f"{type(ex).__name__}: {ex}")
    finally:
        c6.close()

    print(json.dumps({"checks": results, "failed": failed}, indent=2))
    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main())
