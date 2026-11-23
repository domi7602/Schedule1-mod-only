"""Cut the local f12 static-batch wall and skirting slices. Requires UnityPy 1.25.4.

python -B extract-door-cut.py <Schedule I_Data/level1> [--check]
The generated file contains local game geometry and is for local use only.
"""

import argparse
import hashlib
import json
import math
from pathlib import Path

import UnityPy
from UnityPy.helpers.MeshHelper import MeshHandler


LEVEL_SHA256 = "8d17438080e7692895a8bb3ce63ca1a3f980bf0f2d5c8972f1ce67ea65cd047f"
OUTPUT = Path(__file__).resolve().parents[1] / "assets" / "main-cut.json"
Z_MIN, Z_MAX, Y_TOP = 21.86, 23.14, 2.32


def clip(poly, axis, plane, sign):
    """Keep one half-plane, interpolating the entire native vertex payload."""
    if not poly:
        return []
    result = []
    prev = poly[-1]
    for curr in poly:
        a = sign * (prev[axis] - plane)
        b = sign * (curr[axis] - plane)
        if a * b < 0:
            t = a / (a - b)
            result.append(tuple(x + t * (y - x) for x, y in zip(prev, curr)))
        if b >= 0:
            result.append(curr)
        prev = curr
    return result


def outside_door(triangle):
    # Only the portal wall is in this X interval. Adjacent walls stay intact.
    if not all(108.72 < v[0] < 108.77 for v in triangle):
        yield triangle
        return
    inside = triangle
    for axis, plane, sign in ((2, Z_MIN, 1), (2, Z_MAX, -1), (1, Y_TOP, -1)):
        if all(sign * (v[axis] - plane) >= 0 for v in inside):
            continue
        outside = clip(inside, axis, plane, -sign)
        for k in range(1, len(outside) - 1):
            yield (outside[0], outside[k], outside[k + 1])
        inside = clip(inside, axis, plane, sign)
        if len(inside) < 3:
            break


def area(triangle):
    a, b, c = triangle
    u = [b[k] - a[k] for k in range(3)]
    v = [c[k] - a[k] for k in range(3)]
    return math.hypot(u[1] * v[2] - u[2] * v[1], u[2] * v[0] - u[0] * v[2],
                      u[0] * v[1] - u[1] * v[0]) / 2


def extract(objects, renderer_id, filter_id, mesh_id, submesh, expected_removed):
    renderer = objects[renderer_id].read_typetree()
    mesh_filter = objects[filter_id].read_typetree()
    if (mesh_filter["m_GameObject"] != renderer["m_GameObject"]
            or mesh_filter["m_Mesh"] != {"m_FileID": 0, "m_PathID": mesh_id}
            or renderer["m_StaticBatchInfo"] != {"firstSubMesh": submesh, "subMeshCount": 1}):
        raise ValueError("Static-batch attribution changed")
    mesh = MeshHandler(objects[mesh_id].read())
    mesh.process()
    source = mesh.get_triangles()[submesh]
    vertices, indices, lookup = [], [], {}
    original_area = kept_area = 0.0
    for tri in source:
        payload = [tuple(mesh.m_Vertices[i]) + tuple(mesh.m_Normals[i])
                   + tuple(mesh.m_Tangents[i]) + tuple(mesh.m_UV0[i]) for i in tri]
        original_area += area(payload)
        for part in outside_door(payload):
            measure = area(part)
            if measure < 1e-10:
                continue
            kept_area += measure
            for v in part:
                if v not in lookup:
                    lookup[v] = len(vertices)
                    vertices.append(v)
                indices.append(lookup[v])
    if not math.isclose(original_area - kept_area, expected_removed, abs_tol=0.00002):
        raise ValueError(f"Unexpected area removed: {original_area - kept_area}")
    return {"v": [v[:3] for v in vertices], "uv": [v[10:] for v in vertices],
            "n": [v[3:6] for v in vertices], "tangents": [v[6:10] for v in vertices],
            "t": indices}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("level1", type=Path)
    parser.add_argument("--check", action="store_true", help="Compare the existing output")
    args = parser.parse_args()
    with args.level1.open("rb") as stream:
        digest = hashlib.sha256()
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
        if digest.hexdigest() != LEVEL_SHA256:
            raise SystemExit("Unknown level1 SHA-256")
        stream.seek(0)
        level = UnityPy.load(stream).file
        width = Z_MAX - Z_MIN
        cut = extract(level.objects, 319722, 372703, 143, 4, width * (Y_TOP - 0.1))
        # The 0.8 m skirting has a narrow horizontal top, also crossing the door.
        trim_depth = 108.75 - 108.73585510253906
        cut["skirting"] = extract(level.objects, 329676, 333301, 33, 102,
                                   width * (0.8 + trim_depth))
    data = (json.dumps(cut, separators=(",", ":"), allow_nan=False) + "\n").encode("ascii")
    if args.check:
        if OUTPUT.read_bytes() != data:
            raise SystemExit(f"Fixture differs: {OUTPUT}")
    else:
        OUTPUT.write_bytes(data)
    for name, body in (("Main", cut), ("Skirting", cut["skirting"])):
        print(f"{name}: {len(body['v'])} vertices, {len(body['t']) // 3} triangles")
    print(f"{OUTPUT.name}: SHA-256 {hashlib.sha256(data).hexdigest()}")


if __name__ == "__main__":
    main()
