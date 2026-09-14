# SnackVendor — Vending-Maschine Modell (Blender headless)
# Baut eine stilisierte Snack-Vending-Maschine im Schedule-I-Look (low-poly, flat colors)
# und exportiert sie als GLB + rendert ein Preview-PNG.
# Ausführen: blender --background --python build_snackvendor_model.py
import bpy
import random
import math

random.seed(42)

# ---------- Szene aufräumen ----------
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
for blk in (bpy.data.meshes, bpy.data.materials, bpy.data.lights, bpy.data.cameras):
    for item in list(blk):
        if item.users == 0:
            blk.remove(item)

# ---------- Material-Helper (Principled BSDF, glTF-kompatibel) ----------
def make_mat(name, color, metallic=0.1, roughness=0.6, alpha=1.0):
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    inputs = bsdf.inputs
    def set_in(key, val):
        if key in inputs:
            inputs[key].default_value = val
    set_in("Base Color", (*color, 1.0))
    set_in("Metallic", metallic)
    set_in("Roughness", roughness)
    if alpha < 1.0:
        set_in("Alpha", alpha)
        mat.blend_method = 'BLEND'
    return mat

# ---------- Geometrie-Helper ----------
def add_box(name, loc, size, mat, bevel=0.012, parent=None, rot=(0, 0, 0)):
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc, rotation=rot)
    o = bpy.context.active_object
    o.name = name
    o.scale = size
    if bevel > 0:
        bev = o.modifiers.new("Bevel", 'BEVEL')
        bev.width = bevel
        bev.segments = 2
        bev.limit_method = 'ANGLE'
    o.data.materials.append(mat)
    bpy.ops.object.shade_smooth()
    if parent:
        o.parent = parent
        o.matrix_parent_inverse = parent.matrix_world.inverted()
    return o

def add_cyl(name, loc, radius, depth, mat, parent=None, rot=(0, 0, 0), verts=16):
    bpy.ops.mesh.primitive_cylinder_add(vertices=verts, radius=radius, depth=depth,
                                        location=loc, rotation=rot)
    o = bpy.context.active_object
    o.name = name
    o.data.materials.append(mat)
    bpy.ops.object.shade_smooth()
    if parent:
        o.parent = parent
        o.matrix_parent_inverse = parent.matrix_world.inverted()
    return o

# ---------- Materialien (Namen survive GLB-Export -> C# tintet per Name) ----------
MAT_BODY   = make_mat("SnackVendor_Body_Mat",   (0.78, 0.12, 0.10), metallic=0.25, roughness=0.45)  # rot wie Cuke-Maschine
MAT_DARK   = make_mat("SnackVendor_Dark_Mat",   (0.10, 0.10, 0.11), metallic=0.55, roughness=0.40)  # Anthrazit
MAT_GLASS  = make_mat("SnackVendor_Glass_Mat",  (0.65, 0.85, 0.95), metallic=0.0,  roughness=0.08, alpha=0.28)
MAT_TRIM   = make_mat("SnackVendor_Trim_Mat",   (0.92, 0.92, 0.94), metallic=0.85, roughness=0.25)  # Chrom
MAT_SHELF  = make_mat("SnackVendor_Shelf_Mat",  (0.22, 0.23, 0.25), metallic=0.35, roughness=0.55)
MAT_PANEL  = make_mat("SnackVendor_Panel_Mat",  (0.16, 0.17, 0.19), metallic=0.30, roughness=0.50)
MAT_LOGO   = make_mat("SnackVendor_Logo_Mat",   (0.99, 0.80, 0.15), metallic=0.10, roughness=0.55)  # Gelb-Logo
MAT_WHITE  = make_mat("SnackVendor_White_Mat",  (0.94, 0.94, 0.92), roughness=0.60)
SNACK_MATS = [
    make_mat("Snack_Red",    (0.85, 0.20, 0.18), roughness=0.6),
    make_mat("Snack_Blue",   (0.18, 0.40, 0.85), roughness=0.6),
    make_mat("Snack_Yellow", (0.95, 0.78, 0.12), roughness=0.6),
    make_mat("Snack_Green",  (0.25, 0.70, 0.30), roughness=0.6),
    make_mat("Snack_Orange", (0.95, 0.50, 0.10), roughness=0.6),
    make_mat("Snack_Purple", (0.60, 0.25, 0.75), roughness=0.6),
    make_mat("Snack_White",  (0.92, 0.90, 0.85), roughness=0.6),
]

# ---------- Root ----------
root = bpy.ops.object.empty_add(location=(0, 0, 0))
root = bpy.context.active_object
root.name = "SnackVendor_Model"
root.empty_display_size = 0.3

# ---------- Maße (Meter, Schedule-I-Maßstab: ~1.85m hoch wie Cuke-Automat) ----------
W, H, D = 0.95, 1.85, 0.72   # Breite, Höhe, Tiefe
GLASS_W, GLASS_H = 0.62, 1.15
SHELF_Z0, SHELF_DZ, N_SHELVES = 0.72, 0.30, 4

# Korpus (hohler Eindruck über dunkle Innenkammer + Glasfront)
inner = add_box("SnackVendor_InnerChamber", (0, 0, H * 0.5 - 0.02), (W - 0.06, D - 0.06, H - 0.10), MAT_DARK, bevel=0.02, parent=root)
add_box("SnackVendor_Body", (0, 0.01, H * 0.5), (W, D, H), MAT_BODY, bevel=0.025, parent=root)  # wird vorn geöffnet: siehe Ausschnitt unten

# Stellage: Regalböden + Snacks
for i in range(N_SHELVES):
    z = SHELF_Z0 + i * SHELF_DZ
    add_box(f"SnackVendor_Shelf_{i}", (-0.10, 0.02, z), (GLASS_W - 0.06, D * 0.55, 0.018), MAT_SHELF, bevel=0.004, parent=root)
    # Snack-Reihen: Boxen + Dosen, deterministisch gemischt
    random.seed(100 + i)
    for col in range(5):
        x = -0.10 - (GLASS_W - 0.06) / 2 + 0.075 + col * ((GLASS_W - 0.06 - 0.15) / 4)
        if random.random() < 0.6:
            add_box(f"SnackVendor_Snack_S{i}C{col}", (x, 0.02, z + 0.075), (0.085, 0.11, 0.145), random.choice(SNACK_MATS), bevel=0.006, parent=root)
        else:
            add_cyl(f"SnackVendor_Can_S{i}C{col}", (x, 0.02, z + 0.065), 0.038, 0.13, random.choice(SNACK_MATS), parent=root)

# Glasfront ( Tür' — vanilla VendingMachine.DoorMesh zeigt hier)
add_box("SnackVendor_Door", (0.0, D / 2 - 0.012, SHELF_Z0 + GLASS_H / 2 - 0.02), (GLASS_W + 0.04, 0.015, GLASS_H), MAT_GLASS, bevel=0.004, parent=root)
# Türrahmen
add_box("SnackVendor_DoorFrame_Top",    (0.0, D / 2 - 0.012, SHELF_Z0 + GLASS_H + 0.02), (GLASS_W + 0.10, 0.05, 0.05), MAT_TRIM, parent=root)
add_box("SnackVendor_DoorFrame_Bottom", (0.0, D / 2 - 0.012, SHELF_Z0 - 0.05),           (GLASS_W + 0.10, 0.05, 0.05), MAT_TRIM, parent=root)
add_box("SnackVendor_DoorFrame_L",      (-0.5 * GLASS_W - 0.05, D / 2 - 0.012, SHELF_Z0 + GLASS_H / 2 - 0.02), (0.05, 0.05, GLASS_H + 0.10), MAT_TRIM, parent=root)
add_box("SnackVendor_DoorFrame_R",      ( 0.5 * GLASS_W + 0.05, D / 2 - 0.012, SHELF_Z0 + GLASS_H / 2 - 0.02), (0.05, 0.05, GLASS_H + 0.10), MAT_TRIM, parent=root)

# Bedienpanel rechts: Display, Keypad, Münzeinwurf
add_box("SnackVendor_Panel", (W / 2 - 0.11, D / 2 - 0.012, SHELF_Z0 + GLASS_H * 0.55), (0.16, 0.035, GLASS_H * 0.75), MAT_PANEL, bevel=0.008, parent=root)
add_box("SnackVendor_Display", (W / 2 - 0.11, D / 2 + 0.008, SHELF_Z0 + GLASS_H * 0.86), (0.115, 0.012, 0.085), MAT_LOGO, bevel=0.004, parent=root)
for r in range(3):
    for c in range(3):
        add_box(f"SnackVendor_Key_R{r}C{c}", (W / 2 - 0.147 + c * 0.037, D / 2 + 0.008, SHELF_Z0 + GLASS_H * 0.60 - r * 0.042), (0.028, 0.012, 0.028), MAT_WHITE, bevel=0.003, parent=root)
add_box("SnackVendor_CoinSlot", (W / 2 - 0.11, D / 2 + 0.008, SHELF_Z0 + GLASS_H * 0.30), (0.045, 0.012, 0.012), MAT_TRIM, bevel=0.002, parent=root)
add_box("SnackVendor_CoinPlate", (W / 2 - 0.11, D / 2 + 0.006, SHELF_Z0 + GLASS_H * 0.36), (0.085, 0.010, 0.06), MAT_TRIM, bevel=0.004, parent=root)

# Ausgabefach unten (offene Box)
add_box("SnackVendor_Tray_Bottom", (0.0, D / 2 - 0.03, 0.24), (GLASS_W + 0.04, 0.16, 0.02), MAT_DARK, parent=root)
add_box("SnackVendor_Tray_Back",   (0.0, D / 2 - 0.10, 0.30), (GLASS_W + 0.04, 0.02, 0.13), MAT_DARK, parent=root)
add_box("SnackVendor_Tray_Front",  (0.0, D / 2 + 0.03, 0.30), (GLASS_W + 0.04, 0.02, 0.13), MAT_TRIM, parent=root)
add_box("SnackVendor_Tray_L", (-0.5 * GLASS_W - 0.02, D / 2 - 0.03, 0.30), (0.02, 0.16, 0.13), MAT_DARK, parent=root)
add_box("SnackVendor_Tray_R", ( 0.5 * GLASS_W + 0.02, D / 2 - 0.03, 0.30), (0.02, 0.16, 0.13), MAT_DARK, parent=root)

# Sockel + Füße
add_box("SnackVendor_Plinth", (0, 0.0, 0.09), (W - 0.02, D - 0.02, 0.18), MAT_DARK, bevel=0.015, parent=root)
for fx in (-W / 2 + 0.12, W / 2 - 0.12):
    for fy in (-D / 2 + 0.12, D / 2 - 0.12):
        add_cyl(f"SnackVendor_Foot_{fx:+.2f}_{fy:+.2f}", (fx, fy, 0.02), 0.035, 0.04, MAT_TRIM, parent=root)

# Logo-Streifen an der Seite (Geometrie statt Text — kein Font-Asset nötig)
for i in range(3):
    add_box(f"SnackVendor_SideStripe_{i}", (-W / 2 - 0.005, 0.01, 0.75 + i * 0.28), (0.012, 0.42, 0.10), MAT_LOGO if i == 1 else MAT_WHITE, parent=root)
add_cyl("SnackVendor_SideLogoDot", (-W / 2 - 0.012, 0.01, 1.52), 0.09, 0.012, MAT_LOGO, parent=root, rot=(0, math.radians(90), 0))

# Dach-Blende
add_box("SnackVendor_TopTrim", (0, 0.01, H - 0.045), (W + 0.03, D + 0.03, 0.07), MAT_TRIM, bevel=0.018, parent=root)

# ---------- Referenz-Punkte für die VendingMachine-Komponente (Weg A) ----------
# Vanilla VendingMachine braucht: ItemSpawnPoint, AccessPoint, CashSpawnPoint (Transforms)
for ref_name, loc in [
    ("ItemSpawnPoint", (0.0, D / 2 + 0.10, 0.34)),   # vor dem Ausgabefach
    ("AccessPoint",    (0.0, D / 2 + 0.45, 0.9)),    # Interaktionsposition
    ("CashSpawnPoint", (0.4, D / 2 + 0.12, 0.30)),   # neben dem Fach (bei Zerstörung)
]:
    bpy.ops.object.empty_add(location=loc)
    e = bpy.context.active_object
    e.name = ref_name
    e.empty_display_type = 'PLAIN_AXES'
    e.parent = root
    e.matrix_parent_inverse = root.matrix_world.inverted()

# ---------- GLB-Export ----------
GLB_OUT = bpy.path.abspath("//SnackVendor_model.glb")
bpy.ops.export_scene.gltf(
    filepath=GLB_OUT,
    export_format='GLB',
    export_apply=True,          # Modifier anwenden (Bevels backen)
    export_yup=True,            # Unity Y-up
    export_animations=False,
    export_materials='EXPORT',
)
print(f"GLB_EXPORTED: {GLB_OUT}")

# ---------- Preview-Render ----------
try:
    scene = bpy.context.scene
    # Kamera
    cam_data = bpy.data.cameras.new("Cam")
    cam = bpy.data.objects.new("Cam", cam_data)
    scene.collection.objects.link(cam)
    cam.location = (2.6, -2.9, 1.6)
    direction = bpy.data.objects['SnackVendor_Model'].matrix_world.translation - cam.location
    rot_quat = direction.to_track_quat('-Z', 'Y')
    cam.rotation_euler = rot_quat.to_euler()
    cam_data.lens = 50
    scene.camera = cam
    # Licht
    light_data = bpy.data.lights.new("Key", 'AREA')
    light_data.energy = 350
    key = bpy.data.objects.new("Key", light_data)
    key.location = (3, -4, 3)
    scene.collection.objects.link(key)
    fill_data = bpy.data.lights.new("Fill", 'AREA')
    fill_data.energy = 120
    fill = bpy.data.objects.new("Fill", fill_data)
    fill.location = (-3, -2, 2)
    scene.collection.objects.link(fill)
    # Welt-Hintergrund
    world = bpy.data.worlds.new("World")
    scene.world = world
    world.use_nodes = True
    bg = world.node_tree.nodes.get("Background")
    if bg:
        bg.inputs[0].default_value = (0.06, 0.07, 0.09, 1.0)
        bg.inputs[1].default_value = 1.0
    scene.render.resolution_x = 1280
    scene.render.resolution_y = 960
    scene.render.resolution_percentage = 100
    scene.render.image_settings.file_format = 'PNG'
    PNG_OUT = bpy.path.abspath("//SnackVendor_preview.png")
    scene.render.filepath = PNG_OUT
    scene.render.engine = 'CYCLES'
    scene.cycles.device = 'CPU'
    scene.cycles.samples = 24
    scene.cycles.use_denoising = True
    bpy.ops.render.render(write_still=True)
    print(f"PREVIEW_RENDERED: {PNG_OUT}")
except Exception as ex:
    print(f"PREVIEW_FAILED: {ex}")
