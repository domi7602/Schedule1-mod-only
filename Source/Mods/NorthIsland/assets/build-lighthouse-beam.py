"""Author the Northshore lighthouse beam and export lighthouse-beam.glb.

Run: blender --background --python build-lighthouse-beam.py
Pivot is the lamp. The shaft extends along Blender -Y, which the glTF exporter
maps to +Z forward. Three crossed cards taper from a narrow throat to a wide tip.
U runs along the shaft (0 at the lamp), V runs across each card (0.5 is the core).
"""
import math
from pathlib import Path

import bpy
import bmesh

OUT = Path(__file__).resolve().parent
LENGTH = 58.0
BASE_HALF = 0.22
TIP_HALF = 4.4

bpy.ops.wm.read_factory_settings(use_empty=True)

mesh = bpy.data.meshes.new("LighthouseBeam")
obj = bpy.data.objects.new("LighthouseBeam", mesh)
bpy.context.collection.objects.link(obj)
bm = bmesh.new()
uv_layer = bm.loops.layers.uv.new("UVMap")


def add_card(angle):
    right_x = math.cos(angle)
    right_z = math.sin(angle)

    def vert(along, side):
        half = BASE_HALF + (TIP_HALF - BASE_HALF) * along
        return bm.verts.new((right_x * side * half, -LENGTH * along, right_z * side * half))

    corners = [vert(0.0, -1.0), vert(0.0, 1.0), vert(1.0, 1.0), vert(1.0, -1.0)]
    face = bm.faces.new(corners)
    for loop in face.loops:
        co = loop.vert.co
        along = -co.y / LENGTH
        half = BASE_HALF + (TIP_HALF - BASE_HALF) * along
        side = co.x * right_x + co.z * right_z
        across = 0.5 if half < 1e-6 else 0.5 + 0.5 * (side / half)
        loop[uv_layer].uv = (along, max(0.0, min(1.0, across)))


for index in range(3):
    add_card(index * math.pi / 3.0)

bm.to_mesh(mesh)
bm.free()
mesh.update()

image = bpy.data.images.new("BeamGradient", 128, 64, alpha=True)
pixels = [0.0] * (128 * 64 * 4)
for y in range(64):
    across = abs((y + 0.5) / 64.0 - 0.5) * 2.0
    edge = max(0.0, 1.0 - across)
    edge = edge * edge * (3.0 - 2.0 * edge)
    for x in range(128):
        along = (x + 0.5) / 128.0
        fade = max(0.0, 1.0 - along)
        fade = fade * fade
        alpha = fade * edge
        offset = (y * 128 + x) * 4
        pixels[offset] = 1.0
        pixels[offset + 1] = 0.93
        pixels[offset + 2] = 0.72
        pixels[offset + 3] = alpha
image.pixels = pixels
image.pack()

material = bpy.data.materials.new("LighthouseBeam")
material.use_nodes = True
material.blend_method = "BLEND"
if hasattr(material, "surface_render_method"):
    material.surface_render_method = "BLENDED"
nodes = material.node_tree.nodes
links = material.node_tree.links
bsdf = nodes.get("Principled BSDF")
tex = nodes.new("ShaderNodeTexImage")
tex.image = image
tex.interpolation = "Linear"
if bsdf is not None:
    links.new(tex.outputs["Color"], bsdf.inputs["Base Color"])
    if "Alpha" in bsdf.inputs:
        links.new(tex.outputs["Alpha"], bsdf.inputs["Alpha"])
    if "Emission Color" in bsdf.inputs:
        links.new(tex.outputs["Color"], bsdf.inputs["Emission Color"])
    elif "Emission" in bsdf.inputs:
        links.new(tex.outputs["Color"], bsdf.inputs["Emission"])
    if "Emission Strength" in bsdf.inputs:
        bsdf.inputs["Emission Strength"].default_value = 3.0
mesh.materials.append(material)

bpy.ops.object.select_all(action="DESELECT")
obj.select_set(True)
bpy.context.view_layer.objects.active = obj

glb = str(OUT / "lighthouse-beam.glb")
export_args = {
    "filepath": glb,
    "export_format": "GLB",
    "use_selection": True,
    "export_apply": True,
}
operator = bpy.ops.export_scene.gltf
supported = operator.get_rna_type().properties.keys()
if "export_yup" in supported:
    export_args["export_yup"] = True
if "export_image_format" in supported:
    export_args["export_image_format"] = "AUTO"
operator(**export_args)
bpy.ops.wm.save_as_mainfile(filepath=str(OUT / "lighthouse-beam.blend"))
print(f"exported {glb}")
