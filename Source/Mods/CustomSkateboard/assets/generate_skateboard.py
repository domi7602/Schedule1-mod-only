import bpy
import math
import os

def clean_scene():
    bpy.ops.wm.read_factory_settings(use_empty=True)

def create_materials():
    materials = {}
    
    # Grip tape material
    grip_mat = bpy.data.materials.new(name="Mat_GripTape")
    grip_mat.use_nodes = True
    bsdf = grip_mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = (0.08, 0.08, 0.08, 1.0)
        bsdf.inputs["Roughness"].default_value = 0.95
    materials["Grip"] = grip_mat
    
    # Deck Graphic material
    deck_mat = bpy.data.materials.new(name="Mat_DeckGraphic")
    deck_mat.use_nodes = True
    bsdf = deck_mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = (0.15, 0.55, 0.95, 1.0)
        bsdf.inputs["Roughness"].default_value = 0.35
        bsdf.inputs["Metallic"].default_value = 0.1
    materials["Deck"] = deck_mat
    
    # Truck metal material
    truck_mat = bpy.data.materials.new(name="Mat_TruckMetal")
    truck_mat.use_nodes = True
    bsdf = truck_mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = (0.75, 0.75, 0.78, 1.0)
        bsdf.inputs["Metallic"].default_value = 0.9
        bsdf.inputs["Roughness"].default_value = 0.25
    materials["Truck"] = truck_mat
    
    # Wheels material
    wheel_mat = bpy.data.materials.new(name="Mat_Wheels")
    wheel_mat.use_nodes = True
    bsdf = wheel_mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = (0.95, 0.90, 0.20, 1.0) # Yellow-gold polyurethane
        bsdf.inputs["Roughness"].default_value = 0.4
    materials["Wheel"] = wheel_mat
    
    return materials

def create_deck(materials):
    # Length: 0.80m (Y-axis), Width: 0.21m (X-axis), Thickness: 0.012m (Z-axis)
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=(0, 0, 0.045))
    deck = bpy.context.active_object
    deck.name = "Skateboard_Deck"
    deck.scale = (0.21, 0.80, 0.012)
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    
    # Bevel modifier for rounded edges
    mod_bevel = deck.modifiers.new(name="DeckBevel", type='BEVEL')
    mod_bevel.width = 0.01
    mod_bevel.segments = 3
    
    # Assign materials
    deck.data.materials.append(materials["Grip"])
    deck.data.materials.append(materials["Deck"])
    return deck

def create_truck(name, y_pos, materials):
    # Baseplate
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=(0, y_pos, 0.032))
    baseplate = bpy.context.active_object
    baseplate.name = f"Truck_Baseplate_{name}"
    baseplate.scale = (0.07, 0.07, 0.01)
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    baseplate.data.materials.append(materials["Truck"])
    
    # Hanger (Axle rod)
    bpy.ops.mesh.primitive_cylinder_add(radius=0.008, depth=0.19, location=(0, y_pos, 0.020))
    hanger = bpy.context.active_object
    hanger.name = f"Truck_Hanger_{name}"
    hanger.rotation_euler = (0, math.radians(90), 0)
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
    hanger.data.materials.append(materials["Truck"])
    
    return hanger

def create_wheel(name, location, materials):
    # Radius: 0.027m (54mm), Depth/Width: 0.032m
    bpy.ops.mesh.primitive_cylinder_add(radius=0.027, depth=0.032, location=location)
    wheel = bpy.context.active_object
    wheel.name = f"Wheel_{name}"
    wheel.rotation_euler = (0, math.radians(90), 0)
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
    
    mod_bevel = wheel.modifiers.new(name="WheelBevel", type='BEVEL')
    mod_bevel.width = 0.004
    mod_bevel.segments = 2
    
    wheel.data.materials.append(materials["Wheel"])
    return wheel

def create_ik_marker(name, location):
    empty = bpy.data.objects.new(name, None)
    empty.location = location
    empty.empty_display_size = 0.05
    empty.empty_display_type = 'ARROWS'
    bpy.context.scene.collection.objects.link(empty)
    return empty

def build_skateboard(output_dir):
    clean_scene()
    materials = create_materials()
    
    # Deck
    deck = create_deck(materials)
    
    # Trucks at Y = +0.22 and Y = -0.22
    truck_front = create_truck("Front", 0.22, materials)
    truck_rear = create_truck("Rear", -0.22, materials)
    
    # 4 Wheels
    w_offset_x = 0.105
    w_front_y = 0.22
    w_rear_y = -0.22
    w_z = 0.027 # touching ground at Z=0
    
    wheel_fl = create_wheel("Front_Left", (-w_offset_x, w_front_y, w_z), materials)
    wheel_fr = create_wheel("Front_Right", (w_offset_x, w_front_y, w_z), materials)
    wheel_rl = create_wheel("Rear_Left", (-w_offset_x, w_rear_y, w_z), materials)
    wheel_rr = create_wheel("Rear_Right", (w_offset_x, w_rear_y, w_z), materials)
    
    # IK Alignment Targets
    left_foot = create_ik_marker("IK_LeftFoot_Alignment", (0.0, 0.15, 0.052))
    right_foot = create_ik_marker("IK_RightFoot_Alignment", (0.0, -0.25, 0.052))
    com = create_ik_marker("IK_CenterOfMass", (0.0, 0.0, 0.035))
    front_axle = create_ik_marker("Hover_FrontAxle", (0.0, 0.22, 0.0))
    rear_axle = create_ik_marker("Hover_RearAxle", (0.0, -0.22, 0.0))
    
    # Create Root Object
    root = bpy.data.objects.new("CustomSkateboard_Root", None)
    root.empty_display_type = 'PLAIN_AXES'
    bpy.context.scene.collection.objects.link(root)
    
    # Parent everything to Root
    objects_to_parent = [deck, truck_front, truck_rear, wheel_fl, wheel_fr, wheel_rl, wheel_rr,
                         left_foot, right_foot, com, front_axle, rear_axle]
    for obj in objects_to_parent:
        obj.parent = root
        
    os.makedirs(output_dir, exist_ok=True)
    blend_path = os.path.join(output_dir, "custom_skateboard.blend")
    obj_path = os.path.join(output_dir, "custom_skateboard.obj")
    
    bpy.ops.wm.save_as_mainfile(filepath=blend_path)
    print(f"Saved .blend: {blend_path}")
    
    # Export OBJ
    try:
        bpy.ops.wm.obj_export(filepath=obj_path, export_materials=True)
        print(f"Exported OBJ: {obj_path}")
    except Exception as e:
        print(f"wm.obj_export fallback: {e}")
        try:
            bpy.ops.export_scene.obj(filepath=obj_path, use_materials=True)
            print(f"Exported OBJ via export_scene: {obj_path}")
        except Exception as e2:
            print(f"Failed exporting OBJ: {e2}")

if __name__ == "__main__":
    import os
    out_dir = os.path.join(os.path.dirname(os.path.abspath(__file__)), "models")
    build_skateboard(out_dir)
