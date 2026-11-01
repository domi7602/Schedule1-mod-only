#!/usr/bin/env python3
# LOCAL PROPRIETARY GAME DATA: this tool and its generated native-facade.json
# are for the owner's local installation only. Do not upload, publish, or bundle
# the native geometry for redistribution. No Blender input or game connection.
"""Extract three ORIGINAL, UNCLIPPED, renderer-owned static-batch contributions.

Requires the already provisioned isolated Python/UnityPy 1.25.4 environment.
Run with -B to avoid bytecode writes. No installation or source assembly needed:
  <isolated-python> -B build/extract-native-facade.py --data-dir <Schedule I_Data>
Add --check to compare the existing fixture without writing it.

All position/normal/tangent channels are ALREADY BAKED IN WORLD COORDINATES.
Do not apply the source GameObject transforms again. Output vertex order is
[x,y,z,nx,ny,nz,tx,ty,tz,tw,u,v,u1,v1]. There is no clipping, welding, attribute
normalization, quantization, UV substitution, material export, or runtime logic.
Local indices are assigned separately per renderer, in ascending original-index
order. Submesh/material/triangle/index order and float32 bit patterns survive.
Only the assigned assets/native-facade.json is written, after full validation.
"""

from __future__ import annotations

import argparse
import hashlib
import importlib.metadata
import json
import math
from pathlib import Path
import struct
import sys

UNITY_VERSION = "2022.3.62f2"
UNITYPY_VERSION = "1.25.4"
BUILDING_GUID = "c747a750-98cb-47fc-9c40-cf5912acc390"
BUILDING_PATH = "Map/Hyland Point/Region_Downtown/TownCenter/TownHall"
BUILDING_CLASS = ("NPCEnterableBuilding", "ScheduleOne.Map", "Assembly-CSharp")
BUILDING_RAW_SHA256 = "60d3690e1fdc86d53bc9666b1028278bbc2ce19b24399087ca770f669f206f98"
SCRIPT_RAW_SHA256 = "4cb1d9aab840fd1a125ed449ddbdc0fd90be2879c3f1c1b605ce69975976dad4"
# Hashes cover every input, including MonoScript metadata (NOT an assembly).
SOURCE_SHA256 = {
    "level1": "8d17438080e7692895a8bb3ce63ca1a3f980bf0f2d5c8972f1ce67ea65cd047f",
    "sharedassets1.assets": "b4ccd4981fae098f41344a1451a2566bb985323dc265757088cb32fe30706d73",
    "sharedassets0.assets": "1ed15c8cde5fc8a205fc40a92c087498638d9711689bece9d950902814cc82fe",
    "globalgamemanagers.assets": "f6609c9f133cf26060968ce2309f43f748366d80a19810090915eb8fc11648e8",
}
# Unity 2022 VertexAttribute slots; offsets are read from m_VertexData, not guessed.
CHANNELS = ((0, 3, "position"), (1, 3, "normal"), (2, 4, "tangent"),
            (4, 2, "UV0"), (5, 2, "UV1"))
# IDs are local to the hash-pinned serialized files, not runtime instance IDs.
TARGETS = (
    {
        "path": "townhall/Walls", "go": 69766, "transform": 170536,
        "renderer": 302660, "filter": 345008, "mesh": 41,
        "meshName": "Combined Mesh (root: scene) 23", "firstSubMesh": 84,
        "sourceVertexCount": 41514, "localVertexCount": 24,
        "meshSha256": "5b6b79a78596f3d00093154ebb52c7d40d662aa5977911663fe024c4b4eb0d8b",
        "descriptors": ((16716, 48, 0),),
        "materials": ((3, 90, "brick dark light orange"),),
        "boundsMin": [54.349998474121094, 1.2499988079071045, 20.500030517578125],
        "boundsMax": [63.349998474121094, 8.250000953674316, 41.500030517578125],
    },
    {
        "path": "Cube", "go": 28103, "transform": 154918,
        "renderer": 284066, "filter": 340181, "mesh": 125,
        "meshName": "Combined Mesh (root: scene) 26", "firstSubMesh": 127,
        "sourceVertexCount": 63485, "localVertexCount": 24,
        "meshSha256": "2725658a53e908eb9d3bbf81a8c3f74cbfa9a4a32b41b529de37fd0c15d7999c",
        "descriptors": ((58758, 36, 0),),
        "materials": ((4, 745, "Lit"),),
        "boundsMin": [54.39999771118164, 0.6500000953674316, 20.60003089904785],
        "boundsMax": [63.29999923706055, 9.649999618530273, 41.40003204345703],
    },
    {
        "path": "townhall/Cube", "go": 5870, "transform": 200638,
        "renderer": 294387, "filter": 365941, "mesh": 95,
        "meshName": "Combined Mesh (root: scene) 25", "firstSubMesh": 84,
        "sourceVertexCount": 59729, "localVertexCount": 216,
        "meshSha256": "f06ebef26c20d4e5d78bbe0900899a335e517e65919e449908968eefc936b4d2",
        "descriptors": ((80988, 300, 0), (81588, 90, 0), (81768, 66, 0)),
        "materials": ((3, 133, "concrete_browngrey light"),
                      (3, 89, "brick dark grey"), (3, 90, "brick dark light orange")),
        "boundsMin": [51.4805908203125, -0.1391758918762207, 20.153423309326172],
        "boundsMax": [63.713497161865234, 11.943995475769043, 41.846458435058594],
    },
)


def require(condition, message):
    if not condition:
        raise ValueError(message)


def file_sha256(path):
    h = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            h.update(block)
    return h.hexdigest()


def verify_inputs(data_dir):
    # Check level1 FIRST, before importing UnityPy or parsing any source file.
    for name, expected in SOURCE_SHA256.items():
        actual = file_sha256(data_dir / name)
        require(actual == expected, f"{name}: source SHA-256 mismatch: {actual}; expected {expected}")


def ptr(file_id, path_id):
    return {"m_FileID": file_id, "m_PathID": path_id}


def read_object(files, name, path_id, class_id, class_name, raw_sha256=None):
    obj = files[name].objects[path_id]
    require(obj.class_id == class_id and obj.type.name == class_name,
            f"{name}:{path_id}: expected {class_name} (classID {class_id})")
    if raw_sha256 is not None:
        require(hashlib.sha256(obj.get_raw_data()).hexdigest() == raw_sha256,
                f"{name}:{path_id}: serialized object hash changed")
    return obj


def scene_tree(files, path_id, class_id, class_name):
    return read_object(files, "level1", path_id, class_id, class_name).read_typetree()


def transform_path(files, transform_id):
    names, seen = [], set()
    while transform_id:
        require(transform_id not in seen, "Transform ancestor cycle")
        seen.add(transform_id)
        t = scene_tree(files, transform_id, 4, "Transform")
        require(t["m_GameObject"]["m_FileID"] == 0 and t["m_Father"]["m_FileID"] == 0,
                "Unexpected external Transform ancestry")
        go = scene_tree(files, t["m_GameObject"]["m_PathID"], 1, "GameObject")
        require(ptr(0, transform_id) in [c["component"] for c in go["m_Component"]],
                "Transform not owned by its GameObject")
        names.append(go["m_Name"])
        transform_id = t["m_Father"]["m_PathID"]
    return "/".join(reversed(names))


def verify_building(files):
    level = files["level1"]
    require(level.reader.endian == "<", "Expected little-endian source")
    for name, source in files.items():
        require(source.unity_version == UNITY_VERSION, f"{name}: unexpected Unity version")
    for file_id, name in ((1, "globalgamemanagers.assets"),
                          (3, "sharedassets1.assets"), (4, "sharedassets0.assets")):
        require(level.externals[file_id - 1].path.replace("\\", "/").split("/")[-1] == name,
                f"level1 external fileID {file_id}: expected {name}")
    require(transform_path(files, 152451) == BUILDING_PATH, "TownHall hierarchy changed")
    root = scene_tree(files, 62727, 1, "GameObject")
    require(ptr(0, 520158) in [c["component"] for c in root["m_Component"]],
            "TownHall does not own the expected building component")
    component = read_object(files, "level1", 520158, 114, "MonoBehaviour", BUILDING_RAW_SHA256)
    raw = component.get_raw_data()
    # Read only the known Unity base header. Custom IL2CPP field type trees are
    # incomplete; exact object hash + length-prefixed GUID avoid guessing fields.
    require(struct.unpack_from("<iq", raw, 0) == (0, 62727), "Building GameObject changed")
    require(struct.unpack_from("<iq", raw, 16) == (1, 1671), "Building MonoScript pointer changed")
    guid = BUILDING_GUID.encode("ascii")
    require(raw.count(struct.pack("<i", len(guid)) + guid) == 1, "Building GUID mismatch")
    script = read_object(files, "globalgamemanagers.assets", 1671, 115, "MonoScript",
                         SCRIPT_RAW_SHA256).read_typetree()
    identity = tuple(script[k] for k in ("m_ClassName", "m_Namespace", "m_AssemblyName"))
    require(identity == BUILDING_CLASS, f"Building class mismatch: {identity!r}")


def vertex_reader(mesh):
    require(mesh["m_StreamData"]["size"] == 0, "External vertex stream is not supported")
    vd = mesh["m_VertexData"]
    channels = vd["m_Channels"]
    required_slots = {slot for slot, _, _ in CHANNELS}
    require({i for i, c in enumerate(channels) if c["dimension"]} == required_slots,
            "Missing required vertex channel (including UV1), or unexpected active channel")
    layout, occupied = [], set()
    for slot, dimension, name in CHANNELS:
        c = channels[slot]
        require(c["stream"] == 0 and c["format"] == 0 and c["dimension"] == dimension,
                f"{name}: expected native float32 stream 0, dimension {dimension}")
        byte_range = set(range(c["offset"], c["offset"] + dimension * 4))
        require(c["offset"] >= 0 and not occupied.intersection(byte_range), f"{name}: invalid overlap")
        occupied.update(byte_range)
        layout.append((c["offset"], dimension * 4))
    stride = max(occupied) + 1
    require(stride == 56 and len(occupied) == 56, "Expected complete 56-byte native vertex payload")
    raw = bytes(vd["m_DataSize"])
    require(len(raw) == vd["m_VertexCount"] * stride, "Vertex buffer length mismatch")

    def payload_bytes(index):
        require(0 <= index < vd["m_VertexCount"], f"Vertex index out of range: {index}")
        start = index * stride
        return b"".join(raw[start + offset:start + offset + size] for offset, size in layout)

    return payload_bytes


def submesh_indices(mesh, submesh):
    require(mesh["m_IndexFormat"] in (0, 1), "Unsupported index format")
    width, fmt = (2, "<H") if mesh["m_IndexFormat"] == 0 else (4, "<I")
    begin, count, base = (submesh[k] for k in ("firstByte", "indexCount", "baseVertex"))
    raw = bytes(mesh["m_IndexBuffer"])
    require(submesh["topology"] == 0 and count > 0 and count % 3 == 0,
            "Expected nonempty original triangle-list submesh")
    require(begin >= 0 and begin % width == 0 and begin + count * width <= len(raw),
            "Submesh index buffer range mismatch")
    indices = [v[0] + base for v in struct.iter_unpack(fmt, raw[begin:begin + count * width])]
    require(all(0 <= i < mesh["m_VertexData"]["m_VertexCount"] for i in indices),
            "Index + baseVertex is outside the full combined vertex buffer")
    require(all(submesh["firstVertex"] <= i < submesh["firstVertex"] + submesh["vertexCount"]
                for i in indices), "Indices outside source submesh vertex span")
    return indices


def bounds(vertices):
    return ([min(v[axis] for v in vertices) for axis in range(3)],
            [max(v[axis] for v in vertices) for axis in range(3)])


def validate_roundtrip(source, source_indices, original_ids, payload_bytes):
    vertices, submeshes = source["vertices"], source["submeshes"]
    require(len(vertices) == len(original_ids), "Local vertex count mismatch")
    require(len(submeshes) == len(source_indices) == len(source["materialNames"]),
            "Submesh/material slot count mismatch")
    for v, original in zip(vertices, original_ids):
        require(len(v) == 14 and all(math.isfinite(x) for x in v), "Invalid vertex payload")
        require(struct.pack("<14f", *v) == payload_bytes(original),
                "Local vertex attribute bitwise round-trip failed")
    for slot, (local, original) in enumerate(zip(submeshes, source_indices)):
        require(len(local) == len(original) and len(local) % 3 == 0, "Triangle count mismatch")
        require(all(type(i) is int and 0 <= i < len(vertices) for i in local), "Invalid local index")
        restored = [original_ids[i] for i in local]
        require(restored == original, f"Submesh {slot}: original index/triangle/winding order changed")
        # Check every indexed vertex, not just a sample or unique-position set.
        for i, original_index in zip(local, original):
            require(struct.pack("<14f", *vertices[i]) == payload_bytes(original_index),
                    f"Submesh {slot}: indexed payload round-trip failed")
        before = [struct.unpack("<14f", payload_bytes(i)) for i in original]
        after = [vertices[i] for i in local]
        require(bounds(before) == bounds(after), f"Submesh {slot}: world bounds changed")
    require(bounds(vertices) == (source["boundsMin"], source["boundsMax"]),
            "Renderer contribution world bounds changed")


def extract_source(files, target):
    go = scene_tree(files, target["go"], 1, "GameObject")
    components = [c["component"] for c in go["m_Component"]]
    require(all(ptr(0, target[k]) in components for k in ("transform", "renderer", "filter")),
            f"{target['path']}: component ownership changed")
    require(transform_path(files, target["transform"]) == BUILDING_PATH + "/" + target["path"],
            f"{target['path']}: hierarchy changed")
    renderer = scene_tree(files, target["renderer"], 23, "MeshRenderer")
    mesh_filter = scene_tree(files, target["filter"], 33, "MeshFilter")
    require(renderer["m_GameObject"] == mesh_filter["m_GameObject"] == ptr(0, target["go"]),
            "Renderer/MeshFilter GameObject mismatch")
    require(mesh_filter["m_Mesh"] == ptr(0, target["mesh"]), "MeshFilter source mesh changed")
    require(renderer["m_StaticBatchInfo"] == {
        "firstSubMesh": target["firstSubMesh"], "subMeshCount": len(target["materials"])},
        "Renderer-owned static-batch submesh range changed")
    require(all(renderer[k] == ptr(0, 0) for k in
                ("m_StaticBatchRoot", "m_AdditionalVertexStreams", "m_EnlightenVertexStream")),
            "Expected scene-root world-baked vertices without overriding streams")
    require(renderer["m_Materials"] == [ptr(fid, pid) for fid, pid, _ in target["materials"]],
            "Renderer material pointer/order changed")
    material_names = []
    for fid, pid, expected in target["materials"]:
        name = {3: "sharedassets1.assets", 4: "sharedassets0.assets"}[fid]
        mat = read_object(files, name, pid, 21, "Material").read_typetree()
        require(mat["m_Name"] == expected, "Material name/order changed")
        material_names.append(mat["m_Name"])
    mesh = read_object(files, "level1", target["mesh"], 43, "Mesh",
                       target["meshSha256"]).read_typetree()
    require(mesh["m_Name"] == target["meshName"], "Combined mesh name changed")
    require(mesh["m_VertexData"]["m_VertexCount"] == target["sourceVertexCount"],
            "Full combined source vertex count changed")
    payload_bytes = vertex_reader(mesh)
    original_submeshes = []
    for offset, expected in enumerate(target["descriptors"]):
        sub = mesh["m_SubMeshes"][target["firstSubMesh"] + offset]
        require(tuple(sub[k] for k in ("firstByte", "indexCount", "baseVertex")) == expected,
                "Source submesh descriptor changed")
        original_submeshes.append(submesh_indices(mesh, sub))
    original_ids = sorted({i for sub in original_submeshes for i in sub})
    require(len(original_ids) == target["localVertexCount"], "Attributed vertex count changed")
    remap = {original: local for local, original in enumerate(original_ids)}
    vertices = [list(struct.unpack("<14f", payload_bytes(i))) for i in original_ids]
    minimum, maximum = bounds(vertices)
    require(minimum == target["boundsMin"] and maximum == target["boundsMax"],
            "Expected baked world-space bounds changed (do not transform the vertices)")
    source = {
        "path": target["path"], "meshName": mesh["m_Name"],
        "firstSubMesh": target["firstSubMesh"],
        "sourceVertexCount": mesh["m_VertexData"]["m_VertexCount"],
        "boundsMin": minimum, "boundsMax": maximum, "materialNames": material_names,
        "vertices": vertices,
        "submeshes": [[remap[i] for i in sub] for sub in original_submeshes],
    }
    validate_roundtrip(source, original_submeshes, original_ids, payload_bytes)
    return source, (original_submeshes, original_ids, payload_bytes)


def export(data_dir, output, check=False):
    verify_inputs(data_dir)
    require(importlib.metadata.version("UnityPy") == UNITYPY_VERSION,
            f"Use the existing isolated UnityPy {UNITYPY_VERSION} environment")
    import UnityPy  # No game process/bridge, Blender, network, or assembly is opened.

    paths = [data_dir / name for name in SOURCE_SHA256]
    env = UnityPy.load(*(str(path) for path in paths))
    files = {path.name: env.files[str(path)] for path in paths}
    verify_building(files)
    sources, validation = [], []
    for target in TARGETS:
        source, evidence = extract_source(files, target)
        sources.append(source)
        validation.append(evidence)
    document = {"schema": 1, "unityVersion": UNITY_VERSION,
                "levelSha256": SOURCE_SHA256["level1"], "buildingGuid": BUILDING_GUID,
                "sources": sources}
    # No rounding: Python's shortest exact float repr round-trips the native f32.
    encoded = (json.dumps(document, ensure_ascii=True, allow_nan=False,
                          separators=(",", ":")) + "\n").encode("utf-8")
    decoded = json.loads(encoded)
    require(decoded == document, "JSON round-trip changed the document")
    for source, evidence in zip(decoded["sources"], validation):
        validate_roundtrip(source, *evidence)
    verify_inputs(data_dir)  # Refuse changed inputs between loading and publication.
    if check:
        require(output.read_bytes() == encoded, f"{output}: fixture differs from verified extraction")
    else:
        output.parent.mkdir(parents=True, exist_ok=True)
        output.write_bytes(encoded)
    summary = [{"path": s["path"], "renderer": t["renderer"], "mesh": t["mesh"],
                "vertices": len(s["vertices"]),
                "trianglesPerSubmesh": [len(sub) // 3 for sub in s["submeshes"]],
                "boundsMin": s["boundsMin"], "boundsMax": s["boundsMax"]}
               for t, s in zip(TARGETS, sources)]
    return {"output": str(output), "checkedOnly": check, "bytes": len(encoded),
            "sha256": hashlib.sha256(encoded).hexdigest(), "sources": summary,
            "vertices": sum(len(s["vertices"]) for s in sources),
            "triangles": sum(len(sub) // 3 for s in sources for sub in s["submeshes"]),
            "validation": "bitwise original+JSON indexed payloads; exact submesh triangles/winding/order; world bounds"}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--data-dir", required=True, type=Path, help="Local Schedule I_Data directory")
    parser.add_argument("--check", action="store_true", help="Verify existing fixture; do not write")
    args = parser.parse_args()
    output = Path(__file__).resolve().parent.parent / "assets" / "native-facade.json"
    try:
        print(json.dumps(export(args.data_dir, output, args.check), indent=2))
    except (ValueError, KeyError, OSError, importlib.metadata.PackageNotFoundError) as error:
        print(f"Native facade extraction refused: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
