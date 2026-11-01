#!/usr/bin/env python3
# LOCAL PROPRIETARY GAME GEOMETRY. Do not upload/publish this fixture or include
# it in a redistributed mod. Reads only the owner's installed game data.
"""Extract seven decorative vanilla models and the exact TownHall door frame.

Run in the existing UnityPy 1.25.4 / numpy 2.5.4 environment, with -B:
  <python> -B build/extract-native-furniture.py --data-dir <Schedule I_Data>
Add --check for a byte-for-byte regeneration check without writing anything.

Only assets/native-furniture.json is written. No game process, bridge, Blender,
network, scripts, colliders, textures, materials or NetworkObjects are copied.
Materials are identified by the exact scene renderer path and original slot.

Each vertex has XYZ, normal XYZ, tangent XYZW, UV0 XY, UV1 XY (14 numbers).
Absent native UV1 is explicitly marked sourceHasUv1=false and padded with zero;
it is NOT fabricated lightmap UV data. Existing UVs are never replaced.
Static scene-root batches are already world-baked: use only the renderer's
firstSubMesh/subMeshCount range, never the entire combined mesh. Nonstatic
positions/normals/tangents are transformed through the original ancestry.
Every model is translated (NOT rotated) to a shared bottom/center pivot. Unity
Y remains up and world X/Z orientation is preserved. LOD alternatives, disabled
original clock hands and desk payment signage are deliberately not selected.
sourceBoundsMin/Max are the serialized renderer contribution AABBs in world
axes (submesh AABB union for static batches; transformed Mesh.bounds otherwise).
sourceWorldBoundsMin/Max separately record the tight indexed-geometry bounds.
These are offline source bounds, not a claimed live Renderer.bounds readback.
"""
from __future__ import annotations

import argparse
import hashlib
import importlib.metadata
import json

from pathlib import Path
import struct
import sys

SOURCE_SHA256 = {
    "level1": "8d17438080e7692895a8bb3ce63ca1a3f980bf0f2d5c8972f1ce67ea65cd047f",
    "sharedassets1.assets": "b4ccd4981fae098f41344a1451a2566bb985323dc265757088cb32fe30706d73",
    "sharedassets0.assets": "1ed15c8cde5fc8a205fc40a92c087498638d9711689bece9d950902814cc82fe",
}
# The only external byte range read; its descriptor is inside a hash-pinned mesh.
STREAM_SPANS = {
    ("sharedassets0.assets.resS", 1793869584, 47936):
        "7b9a06894a1973413f11aa0ab8e4fec4e46a72c7ba02d5729de60b5668c16e9c",
}
UNITY_VERSION = "2022.3.62f2"
RE = "Map/Hyland Point/Region_Downtown/RE Office/Interior/"
MED = "@Businesses/Hyland Medical/Interior/"
BIN = "Map/Hyland Point/Region_Docks/Barbershop/Interior/Small Bin/small bin/"
CLOCK = "@Businesses/PostOffice/Interior/Wall Clock/"
# renderer ID, GameObject ID, exact hierarchy path, attributed vertex count.
ASSETS = (
    ("desk", "Ornate Desk", "Drawer/working side -X; long axis Z.", (
        (291857, 54023, RE + "Ornate Desk/ornate desk", 704),)),
    ("chair", "Chair_Business", "Seat faces +X; high back is on -X.", (
        (288605, 71441, MED + "Chair_Business_Sittable/Chair_Business/Chair_Business", 1498),)),
    ("cabinet", "Filing Cabinet", "Drawer handles face +X.", (
        (280371, 58975, RE + "Filing Cabinet/filing cabinet/Draw", 292),
        (274280, 81261, RE + "Filing Cabinet/filing cabinet/Draw (1)", 292),
        (288404, 119476, RE + "Filing Cabinet/filing cabinet/Draw (2)", 292),
        (324953, 69968, RE + "Filing Cabinet/filing cabinet/Main", 172))),
    ("computer", "OldComputer", "Screen outward normal approximately (-0.930382,0,-0.366591); includes keyboard, mouse, pad.", (
        (300964, 3765, RE + "OldComputer/oldcomputer (1)/Computer", 1358),
        (272369, 131177, RE + "OldComputer/oldcomputer (1)/Keyboard", 166),
        (314025, 31029, RE + "OldComputer/oldcomputer (1)/Mouse", 104),
        (306236, 124259, RE + "OldComputer/oldcomputer (1)/MousePad", 48))),
    ("plant", "Plant2 in pot", "Small tabletop plant; pot plus LOD0 foliage; no prescribed front.", (
        (303433, 75146, MED + "Plant2 in pot", 375),
        (303431, 75147, MED + "Plant2 in pot/Plant2 (1)/Plant2_LOD0", 300))),
    ("bin", "Small Bin", "Body, head and lid in the serialized closed pose; no prescribed front.", (
        (290774, 3839, BIN + "Body", 104),
        (308387, 129857, BIN + "Head", 240),
        (323081, 69018, BIN + "Lid", 88))),
    ("clock", "Wall Clock", "Face +Z, wall behind -Z; static original hands, not a working clock.", (
        (330272, 7005, CLOCK + "Wall clock/Clock", 142),
        (289740, 112948, CLOCK + "Min/MinuteHand", 4),
        (272980, 59812, CLOCK + "Hour/HourHand", 4))),
    ("door-frame", "DoorFrame", "Thin axis X; opening width Z. Visual source only: do not change the accepted passage.", (
        (329674, 95282, "Map/Hyland Point/Region_Downtown/TownCenter/TownHall/townhall/DoorFrame", 76),)),
)
# Source LOD0 membership, not guessed from names; excludes duplicate LOD meshes.
LOD0_PROOFS = {
    454242: [291857],
    456092: [280371, 274280, 288404, 324953],
    457431: [300964, 272369, 314025, 306236],
    457291: [289740, 272980, 330272],
    452615: [303431],
}


def require(condition, message):
    if not condition:
        raise ValueError(message)


def file_sha256(path):
    h = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            h.update(block)
    return h.hexdigest()


def read_span(data_dir, descriptor):
    name, offset, size = descriptor
    require(descriptor in STREAM_SPANS, "Unpinned external vertex range")
    with (data_dir / name).open("rb") as stream:
        stream.seek(offset)
        raw = stream.read(size)
    require(len(raw) == size and hashlib.sha256(raw).hexdigest() == STREAM_SPANS[descriptor],
            f"External vertex span changed: {descriptor}")
    return raw


def verify_inputs(data_dir):
    # Validate level1 before UnityPy parses any scene data.
    for name, expected in SOURCE_SHA256.items():
        actual = file_sha256(data_dir / name)
        require(actual == expected, f"{name}: source SHA-256 mismatch: {actual}")
    for span in STREAM_SPANS:
        read_span(data_dir, span)


def pointer(path_id, file_id=0):
    return {"m_FileID": file_id, "m_PathID": path_id}


class Source:
    def __init__(self, data_dir):
        import numpy as np
        import UnityPy
        self.np, self.data_dir = np, data_dir
        paths = [data_dir / name for name in SOURCE_SHA256]
        self.env = UnityPy.load(*(str(path) for path in paths))
        self.files = {path.name: self.env.files[str(path)] for path in paths}
        self.level = self.files["level1"]
        self.trees, self.matrices, self.meshes = {}, {}, {}
        for name, source in self.files.items():
            require(source.unity_version == UNITY_VERSION and source.reader.endian == "<",
                    f"{name}: unexpected Unity version or endian")
        self.root_transforms = [o.path_id for o in self.level.objects.values()
                                if o.class_id == 4 and o.read_typetree()["m_Father"] == pointer(0)]

    def tree(self, path_id, class_id, name="level1"):
        key = (name, path_id)
        obj = self.files[name].objects[path_id]
        require(obj.class_id == class_id, f"{key}: incorrect object class")
        if key not in self.trees:
            self.trees[key] = obj.read_typetree()
        return self.trees[key]

    def external(self, ptr):
        fid = ptr["m_FileID"]
        name = ("level1" if fid == 0 else
                self.level.externals[fid - 1].path.replace("\\", "/").split("/")[-1])
        require(name in self.files, f"Unpinned external file: {name}")
        return name, ptr["m_PathID"]

    def transform_path(self, tid):
        names, seen = [], set()
        while tid:
            require(tid not in seen, "Transform ancestry cycle")
            seen.add(tid)
            t = self.tree(tid, 4)
            require(t["m_GameObject"]["m_FileID"] == t["m_Father"]["m_FileID"] == 0,
                    "External scene ancestry")
            go = self.tree(t["m_GameObject"]["m_PathID"], 1)
            require(pointer(tid) in [c["component"] for c in go["m_Component"]],
                    "Transform ownership mismatch")
            parent = t["m_Father"]["m_PathID"]
            siblings = ([p["m_PathID"] for p in self.tree(parent, 4)["m_Children"]]
                        if parent else self.root_transforms)
            names_at_level = [self.tree(self.tree(s, 4)["m_GameObject"]["m_PathID"], 1)["m_Name"]
                              for s in siblings]
            require(names_at_level.count(go["m_Name"]) == 1, "Ambiguous scene hierarchy path")
            names.append(go["m_Name"])
            tid = parent
        return "/".join(reversed(names))

    def world_matrix(self, tid):
        if tid not in self.matrices:
            np = self.np
            t = self.tree(tid, 4)
            q = np.array([t["m_LocalRotation"][k] for k in "xyzw"], dtype=float)
            require(abs(float(np.linalg.norm(q)) - 1) < 1e-5, "Non-unit source quaternion")
            q /= np.linalg.norm(q)
            x, y, z, w = q
            m = np.eye(4)
            rotation = np.array([
                [1-2*(y*y+z*z), 2*(x*y-z*w), 2*(x*z+y*w)],
                [2*(x*y+z*w), 1-2*(x*x+z*z), 2*(y*z-x*w)],
                [2*(x*z-y*w), 2*(y*z+x*w), 1-2*(x*x+y*y)]])
            m[:3, :3] = rotation @ np.diag([t["m_LocalScale"][k] for k in "xyz"])
            m[:3, 3] = [t["m_LocalPosition"][k] for k in "xyz"]
            parent = t["m_Father"]["m_PathID"]
            self.matrices[tid] = self.world_matrix(parent) @ m if parent else m
        return self.matrices[tid]

    def mesh_data(self, name, mid):
        key = (name, mid)
        if key not in self.meshes:
            mesh = self.tree(mid, 43, name)
            vd, stream = mesh["m_VertexData"], mesh["m_StreamData"]
            raw = (read_span(self.data_dir, (stream["path"], stream["offset"], stream["size"]))
                   if stream["size"] else bytes(vd["m_DataSize"]))
            channels, occupied, layout = vd["m_Channels"], set(), {}
            active = {i for i, c in enumerate(channels) if c["dimension"] & 15}
            require(active in ({0, 1, 2, 4}, {0, 1, 2, 4, 5}), "Unexpected native vertex channels")
            for slot in sorted(active):
                c = channels[slot]
                dim = c["dimension"] & 15  # Unity channel flags occupy high bits.
                expected = {0: (3,), 1: (3, 4), 2: (4,), 4: (2,), 5: (2,)}[slot]
                require(c["stream"] == 0 and c["format"] in (0, 1) and dim in expected,
                        f"Unsupported channel format at slot {slot}")
                width, fmt = (4, "f") if c["format"] == 0 else (2, "e")
                span = set(range(c["offset"], c["offset"] + dim * width))
                require(c["offset"] >= 0 and not occupied.intersection(span), "Overlapping channels")
                occupied.update(span)
                layout[slot] = (c["offset"], "<" + str(dim) + fmt)
            stride = max(occupied) + 1
            require(len(occupied) == stride and len(raw) == stride * vd["m_VertexCount"],
                    "Invalid vertex stream extent")
            require(mesh["m_MeshCompression"] == 0, "Compressed mesh not supported")
            self.meshes[key] = (mesh, raw, stride, layout)
        return self.meshes[key]


def source_indices(mesh, sub):
    require(mesh["m_IndexFormat"] in (0, 1), "Unsupported native index format")
    width, fmt = (2, "<H") if mesh["m_IndexFormat"] == 0 else (4, "<I")
    begin, count, base = (sub[k] for k in ("firstByte", "indexCount", "baseVertex"))
    raw = bytes(mesh["m_IndexBuffer"])
    require(sub["topology"] == 0 and count > 0 and count % 3 == 0, "Expected triangle list")
    require(begin >= 0 and begin % width == 0 and begin + count * width <= len(raw),
            "Invalid native index range")
    result = [v[0] + base for v in struct.iter_unpack(fmt, raw[begin:begin + count * width])]
    require(all(0 <= i < mesh["m_VertexData"]["m_VertexCount"] and
                sub["firstVertex"] <= i < sub["firstVertex"] + sub["vertexCount"] for i in result),
            "Indices exceed attributed submesh span")
    return result


def bounds(vertices):
    return ([min(v[j] for v in vertices) for j in range(3)],
            [max(v[j] for v in vertices) for j in range(3)])


def extract_part(source, target):
    np = source.np
    rid, gid, expected_path, expected_count = target
    renderer = source.tree(rid, 23)
    require(renderer["m_GameObject"] == pointer(gid), "Renderer GameObject changed")
    go = source.tree(gid, 1)
    components = [c["component"] for c in go["m_Component"]]
    require(pointer(rid) in components and all(p["m_FileID"] == 0 for p in components),
            "Invalid component ownership")
    transform_ids = [p["m_PathID"] for p in components if source.level.objects[p["m_PathID"]].class_id == 4]
    filter_ids = [p["m_PathID"] for p in components if source.level.objects[p["m_PathID"]].class_id == 33]
    require(len(transform_ids) == len(filter_ids) == 1, "Ambiguous renderer source components")
    tid, fid = transform_ids[0], filter_ids[0]
    path = source.transform_path(tid)
    require(path == expected_path and go["m_IsActive"] and renderer["m_Enabled"],
            f"Unexpected or disabled source: {path}")
    mesh_filter = source.tree(fid, 33)
    require(mesh_filter["m_GameObject"] == pointer(gid), "Filter owner mismatch")
    require(all(renderer[k] == pointer(0) for k in
                ("m_StaticBatchRoot", "m_AdditionalVertexStreams", "m_EnlightenVertexStream")),
            "Unexpected batch root or overriding vertex streams")
    name, mid = source.external(mesh_filter["m_Mesh"])
    mesh, raw, stride, layout = source.mesh_data(name, mid)
    batch = renderer["m_StaticBatchInfo"]
    static = batch["subMeshCount"] > 0
    first = batch["firstSubMesh"] if static else 0
    count = batch["subMeshCount"] if static else len(mesh["m_SubMeshes"])
    require(not static or name == "level1" and mesh["m_Name"].startswith("Combined Mesh (root: scene)"),
            "Static mesh is not a verified scene-root batch")
    require(0 <= first and 0 < count <= 8 and first + count <= len(mesh["m_SubMeshes"]), "Invalid batch range or slot budget")
    materials = []
    for p in renderer["m_Materials"]:
        mname, material_id = source.external(p)
        materials.append(source.tree(material_id, 21, mname)["m_Name"])
    require(count == len(materials), "Native material/submesh count mismatch")
    original_indices = [source_indices(mesh, sm) for sm in mesh["m_SubMeshes"][first:first+count]]
    original_ids = sorted({i for sub in original_indices for i in sub})
    require(len(original_ids) == expected_count, f"{path}: attributed vertex count changed")
    require(len(original_ids) <= 10000 and all(len(sub) <= 30000 for sub in original_indices),
            "Per-part vertex/index budget exceeded")
    vertices = []
    for index in original_ids:
        channels = {slot: struct.unpack_from(fmt, raw, index * stride + offset)
                    for slot, (offset, fmt) in layout.items()}
        vertices.append(list(channels[0]) + list(channels[1][:3]) + list(channels[2]) +
                        list(channels[4]) + list(channels.get(5, (0.0, 0.0))))
    native = np.array(vertices, dtype=float)
    require(np.isfinite(native).all(), "Non-finite source attributes")
    world = native.copy()
    if not static:
        m = source.world_matrix(tid)
        linear = m[:3, :3]
        require(float(np.linalg.det(linear)) > 0, "Unexpected mirrored/singular source transform")
        world[:, :3] = native[:, :3] @ linear.T + m[:3, 3]
        world[:, 3:6] = native[:, 3:6] @ np.linalg.inv(linear)
        world[:, 6:9] = native[:, 6:9] @ linear.T
        for begin, end in ((3, 6), (6, 9)):
            lengths = np.linalg.norm(world[:, begin:end], axis=1)
            require(bool((lengths > 1e-8).all()), "Zero native normal/tangent")
            world[:, begin:end] /= lengths[:, None]
        restored = (world[:, :3] - m[:3, 3]) @ np.linalg.inv(linear).T
        require(np.allclose(restored, native[:, :3], atol=1e-11, rtol=0), "Position transform round-trip failed")
    require(np.array_equal(world[:, 9:], native[:, 9:]), "Tangent handedness or original UVs changed")
    mapping = {old: new for new, old in enumerate(original_ids)}
    lo, hi = bounds(world.tolist())
    # Preserve Unity's serialized culling extents, not the full combined mesh box.
    boxes = ([sm["localAABB"] for sm in mesh["m_SubMeshes"][first:first+count]]
             if static else [mesh["m_LocalAABB"]])
    box_min, box_max = [], []
    for box in boxes:
        center = np.array([box["m_Center"][axis] for axis in "xyz"])
        extent = np.array([box["m_Extent"][axis] for axis in "xyz"])
        require(np.isfinite(center).all() and np.isfinite(extent).all() and (extent >= 0).all(),
                "Invalid native source bounds")
        if not static:
            matrix = source.world_matrix(tid)
            center = matrix[:3, :3] @ center + matrix[:3, 3]
            extent = np.abs(matrix[:3, :3]) @ extent
        box_min.append(center - extent)
        box_max.append(center + extent)
    source_min = np.min(box_min, axis=0).tolist()
    source_max = np.max(box_max, axis=0).tolist()
    require(all(source_min[j] <= lo[j] + 1e-4 and source_max[j] >= hi[j] - 1e-4 for j in range(3)),
            "Native bounds do not contain the renderer contribution")
    part = {"sourcePath": path, "meshName": mesh["m_Name"],
            "sourceVertexCount": mesh["m_VertexData"]["m_VertexCount"],
            "firstSubMesh": first, "materialNames": materials,
            "vertices": world.tolist(),
            "submeshes": [[mapping[i] for i in sub] for sub in original_indices],
            "sourceRendererId": rid, "sourceGameObjectId": gid,
            "sourceMeshFile": name, "sourceMeshId": mid,
            "sourceMeshSha256": hashlib.sha256(source.files[name].objects[mid].get_raw_data()).hexdigest(),
            "isStaticBatch": static, "sourceHasUv1": 5 in layout,
            "sourceBoundsMin": source_min, "sourceBoundsMax": source_max,
            "sourceWorldBoundsMin": lo, "sourceWorldBoundsMax": hi}
    return part, (world, original_ids, original_indices)


def validate_asset(asset, evidence, np):
    pivot = np.array(asset["sourcePivotWorld"], dtype=float)
    for part, (world, original_ids, original_indices) in zip(asset["parts"], evidence):
        local = np.array(part["vertices"], dtype=float)
        require(local.shape == world.shape and local.shape[1] == 14 and np.isfinite(local).all(), "Invalid payload")
        restored = local.copy()
        restored[:, :3] += pivot
        require(np.allclose(restored[:, :3], world[:, :3], rtol=0, atol=1e-12), "Pivot round-trip failed")
        require(np.array_equal(restored[:, 3:], world[:, 3:]), "Original world attributes changed")
        require(len(part["submeshes"]) == len(original_indices) == len(part["materialNames"]), "Slot order changed")
        for local_indices, original in zip(part["submeshes"], original_indices):
            require(all(type(i) is int and 0 <= i < len(local) for i in local_indices), "Invalid local index")
            require([original_ids[i] for i in local_indices] == original,
                    "Original indexed triangles/winding/order changed")
    lo, hi = bounds([v for p in asset["parts"] for v in p["vertices"]])
    require((lo, hi) == (asset["boundsMin"], asset["boundsMax"]), "Asset bounds changed")
    require(lo[1] == 0 and abs(lo[0]+hi[0]) < 1e-10 and abs(lo[2]+hi[2]) < 1e-10,
            "Expected bottom/center asset pivot")


def export(data_dir, output, check=False):
    verify_inputs(data_dir)
    for package, version in (("UnityPy", "1.25.4"), ("numpy", "2.5.4")):
        require(importlib.metadata.version(package) == version, f"Use existing isolated {package} {version}")
    source = Source(data_dir)
    for group_id, expected in LOD0_PROOFS.items():
        lod = source.tree(group_id, 205)["m_LODs"][0]
        require([r["renderer"] for r in lod["renderers"]] == [pointer(i) for i in expected],
                "Pinned LOD0 membership changed")
    assets, proofs = [], []
    for key, native_name, facing, targets in ASSETS:
        parts, evidence = [], []
        for target in targets:
            part, proof = extract_part(source, target)
            parts.append(part)
            evidence.append(proof)
        lo, hi = bounds([v for p in parts for v in p["vertices"]])
        pivot = [(lo[0]+hi[0])/2, lo[1], (lo[2]+hi[2])/2]
        for part in parts:
            for vertex in part["vertices"]:
                for axis in range(3):
                    vertex[axis] -= pivot[axis]
        minimum, maximum = bounds([v for p in parts for v in p["vertices"]])
        asset = {"key": key, "boundsMin": minimum, "boundsMax": maximum, "parts": parts,
                 "sourceName": native_name, "sourcePivotWorld": pivot, "facingHint": facing}
        validate_asset(asset, evidence, source.np)
        assets.append(asset)
        proofs.append(evidence)
    require(sum(len(p["vertices"]) for a in assets for p in a["parts"]) <= 30000, "Vertex budget exceeded")
    require(all(len(a["parts"]) <= 12 for a in assets), "Part budget exceeded")
    document = {"schema": 1, "levelSha256": SOURCE_SHA256["level1"], "assets": assets}
    encoded = (json.dumps(document, ensure_ascii=True, allow_nan=False, separators=(",", ":")) + "\n").encode("utf-8")
    decoded = json.loads(encoded)
    require(decoded == document, "JSON round-trip changed native data")
    for asset, evidence in zip(decoded["assets"], proofs):
        validate_asset(asset, evidence, source.np)
    verify_inputs(data_dir)  # Refuse inputs changing during extraction.
    if check:
        require(output.read_bytes() == encoded, "Fixture differs from verified native extraction")
    else:
        output.parent.mkdir(parents=True, exist_ok=True)
        output.write_bytes(encoded)
    return {"output": str(output), "checkedOnly": check, "bytes": len(encoded),
            "sha256": hashlib.sha256(encoded).hexdigest(),
            "vertices": sum(len(p["vertices"]) for a in assets for p in a["parts"]),
            "parts": sum(len(a["parts"]) for a in assets),
            "triangles": sum(len(s)//3 for a in assets for p in a["parts"] for s in p["submeshes"]),
            "assets": [{"key": a["key"], "sizeXYZ": [a["boundsMax"][i]-a["boundsMin"][i] for i in range(3)],
                        "parts": len(a["parts"]), "vertices": sum(len(p["vertices"]) for p in a["parts"]),
                        "sourcePaths": [p["sourcePath"] for p in a["parts"]]} for a in assets],
            "validation": "input files+external span pinned before/after; unique exact scene paths; native slot/submesh ranges; original indexed triangles/order; attributes+UVs; transforms+pivots; JSON roundtrip"}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--data-dir", required=True, type=Path)
    parser.add_argument("--check", action="store_true", help="Regenerate and compare, without writing")
    args = parser.parse_args()
    output = Path(__file__).resolve().parent.parent / "assets" / "native-furniture.json"
    try:
        print(json.dumps(export(args.data_dir, output, args.check), indent=2))
    except (ValueError, KeyError, OSError, importlib.metadata.PackageNotFoundError) as error:
        print(f"Native furniture extraction refused: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
