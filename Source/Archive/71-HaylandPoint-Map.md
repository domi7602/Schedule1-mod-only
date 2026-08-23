# HaylandPoint — Map Mesh Catalog

**Mesh Path:** `Source/Game/HaylandPoint/HaylandPoint/meshes/`  
**Format:** Wavefront OBJ  
**Inventory Date:** July 23, 2026

## Summary

| Metric | Value |
|---|---:|
| OBJ files | 2,271 |
| Filename range | `mesh_000000.obj`–`mesh_002270.obj` |
| Total size | 1,244,865,486 bytes (1.159 GiB) |
| Total vertices | 8,108,065 |
| Total faces/triangles | 11,083,305 |
| Files with object name hint from `instances.csv` | 227 |
| Files without object name hint | 2,044 |
| OBJ files without faces | 0 |
| Subfolders in `meshes/` | none |

All faces of the inspected OBJ files are triangles. The specified polygon approximation is therefore identical to the directly counted face and triangle count; it was not estimated from file size.

## Data Quality and Limitations

- The OBJ files are named exclusively `mesh_######.obj`. No region or object type can be derived from the filename itself.
- `instances.csv` contains usable object names for 227 meshes. Due to decimal commas, the CSV column structure of transformation matrices is corrupted; this is also documented in `export_region7_houses.py`.
- The first two CSV fields (`meshFile`, `name`) are usable. Mapping takes into account the normalization described in `export_region7_houses.py` converting short numbers like `mesh_0646.obj` to `mesh_000646.obj`.
- Names in `instances.csv` denote instances. A mesh can therefore possess multiple names and categories. For the tables, the most informative name hint was selected.
- Only **Docks** and **Sewer** are directly verifiable in object names. A reliable mapping of remaining meshes to regions is not possible without intact transformation data or visual inspection in Blender.
- Reference documents use two different region lists:
  - `Docs/41-Map-Detail.md`: Downtown, Docks, Suburb, Industrial, Residential, Sewer.
  - `../systems/13-Regions.md`: Northtown, Westville, Downtown, Docks, Suburbia, Uptown.
- This catalog uses the requested list from `Docs/41-Map-Detail.md` for the region section, but marks only directly evidenced matches.

## Directory Structure

```text
Source/Game/HaylandPoint/
└── HaylandPoint/
    ├── meshes/
    │   ├── mesh_000000.obj
    │   ├── mesh_000001.obj
    │   ├── ...
    │   └── mesh_002270.obj
    ├── instances.csv
    ├── import_hayland_point.py
    └── export_region7_houses.py
```

## Distribution by Number Block

Since no semantic file prefix is present, the numerical 100 block forms the only complete and reproducible grouping of all files.

| Number Block | Files | Size | Triangles | With Name Hint |
|---|---:|---:|---:|---:|
| `000000–000099` | 100 | 284.94 MiB | 1,645,527 | 10 |
| `000100–000199` | 100 | 8.07 MiB | 52,995 | 10 |
| `000200–000299` | 100 | 9.06 MiB | 57,267 | 10 |
| `000300–000399` | 100 | 2.66 MiB | 18,425 | 10 |
| `000400–000499` | 100 | 3.95 MiB | 26,592 | 10 |
| `000500–000599` | 100 | 0.77 MiB | 5,934 | 10 |
| `000600–000699` | 100 | 0.88 MiB | 5,845 | 10 |
| `000700–000799` | 100 | 4.57 MiB | 32,504 | 10 |
| `000800–000899` | 100 | 2.10 MiB | 15,872 | 10 |
| `000900–000999` | 100 | 0.99 MiB | 6,750 | 10 |
| `001000–001099` | 100 | 2.78 MiB | 19,469 | 10 |
| `001100–001199` | 100 | 55.43 MiB | 415,392 | 10 |
| `001200–001299` | 100 | 1.93 MiB | 14,055 | 10 |
| `001300–001399` | 100 | 4.94 MiB | 32,383 | 10 |
| `001400–001499` | 100 | 2.10 MiB | 16,143 | 10 |
| `001500–001599` | 100 | 5.03 MiB | 30,478 | 10 |
| `001600–001699` | 100 | 3.42 MiB | 24,249 | 10 |
| `001700–001799` | 100 | 778.27 MiB | 8,564,153 | 10 |
| `001800–001899` | 100 | 1.60 MiB | 10,566 | 10 |
| `001900–001999` | 100 | 2.34 MiB | 15,362 | 10 |
| `002000–002099` | 100 | 2.27 MiB | 13,998 | 10 |
| `002100–002199` | 100 | 7.45 MiB | 46,768 | 10 |
| `002200–002270` | 71 | 1.66 MiB | 12,578 | 7 |
| **Total** | **2,271** | **1.159 GiB** | **11,083,305** | **227** |

The block `001700–001799` contains around two-thirds of total file size and over three-quarters of all triangles due to `mesh_001723.obj`.

## Semantic Distribution of Named Subset

This distribution includes only the 227 meshes with name hints. It makes no statement about the 2,044 unnamed meshes.

| Category | Meshes | Size | Triangles | Basis |
|---|---:|---:|---:|---|
| Region matches | 4 | 50.62 KiB | 346 | `Docks`, `Sewer` in name |
| Buildings/Components | 45 | 1.46 MiB | 8,758 | House, Wall, Roof, Door, Window, Garage, Fence, etc. |
| Road/Traffic | 21 | 469.12 KiB | 3,019 | Road, Sidewalk, Path, etc. |
| Vegetation | 5 | 384.53 KiB | 3,148 | Grass, Shrub |
| Water | 1 | 2.91 KiB | 18 | Water |
| Miscellaneous/Unclear | 151 | 41.07 MiB | 248,581 | Characters, props, LODs, generic names |
| **Named subset** | **227** | **43.46 MiB** | **263,870** | |

## Regions

### Downtown

No file bears a direct `Downtown` name hint. Mapping by position is unreliable due to damaged transformation columns.

### Docks

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000266.obj` | 16,579 B | 120 | `Docks_wall_LOD0_(EMC_Merge)`; also Garage/Window/Brick hints |
| `mesh_000826.obj` | 12,898 B | 82 | `docks_mezzanine`; also Sign, Quad, Body_LOD0 |

### Suburb

No file bears a direct `Suburb` or `Suburbia` name hint. House meshes listed further below are residential objects, but cannot be assigned position-wise to this region.

### Industrial

No file bears a direct `Industrial` name hint. Shipping containers, trucks, and technical props are thematically industrial, but not regionally verifiable.

### Residential

No file bears a direct `Residential` name hint. House meshes are thematically suitable, but not regionally verifiable.

### Sewer

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_001126.obj` | 21,612 B | 140 | `seweragepipe`; also Dirt, Fence |
| `mesh_001966.obj` | 742 B | 4 | `Sewer_stairs`; also Main, Base |

## Buildings and Components

### Explicit Houses

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_001896.obj` | 4,300 B | 28 | `House_C_1` |
| `mesh_001926.obj` | 60,324 B | 247 | `House_C_3` |
| `mesh_001936.obj` | 146,511 B | 672 | `House_B_4` |
| `mesh_001946.obj` | 3,656 B | 20 | `House_D_1` |
| `mesh_002196.obj` | 2,944 B | 16 | `House_E_2` |

### Garages, Doors, and Windows

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000266.obj` | 16,579 B | 120 | `Garage_entrance_walls`, `WindowCover` |
| `mesh_000286.obj` | 3,454 B | 24 | `GarageDoor` |
| `mesh_000656.obj` | 8,241 B | 48 | `DoorHandle`, Glass |
| `mesh_001226.obj` | 9,012 B | 60 | Door, Roof |
| `mesh_001236.obj` | 2,009 B | 10 | `GarageDoor`, CornerFence |
| `mesh_001276.obj` | 15,827 B | 108 | `MarineDoor` |
| `mesh_001286.obj` | 3,780 B | 24 | `BackDoorFrame` |
| `mesh_001326.obj` | 29,673 B | 156 | `B1B_CornerWindow_LOW(Clone)` |
| `mesh_001336.obj` | 742 B | 4 | `B1C_WallDoor_(1/2)` |
| `mesh_001346.obj` | 32,148 B | 240 | `B1C_WallDoor_LOW(Clone)` |
| `mesh_001446.obj` | 7,540 B | 48 | `GarageDoor` |
| `mesh_001456.obj` | 1,829 B | 10 | multiple `GarageDoor` instances |
| `mesh_001626.obj` | 39,926 B | 294 | Window |
| `mesh_001636.obj` | 42,064 B | 320 | `WindowTrim` |
| `mesh_001756.obj` | 147,125 B | 514 | Door |
| `mesh_001766.obj` | 65,402 B | 315 | `DoorHandle` |
| `mesh_001776.obj` | 4,486 B | 26 | `DoorHandle.001` |
| `mesh_001906.obj` | 1,465 B | 8 | `Garage_A2` |
| `mesh_001976.obj` | 159,448 B | 964 | Door |
| `mesh_002216.obj` | 737 B | 4 | `DoorFrame` |

### Walls, Roofs, Foundations, and Interiors

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000206.obj` | 65,402 B | 270 | Fence |
| `mesh_000216.obj` | 15,606 B | 176 | `Fence_base` |
| `mesh_000226.obj` | 41,989 B | 226 | `Fence_base.001` |
| `mesh_000236.obj` | 115,417 B | 694 | `FenceTrim` |
| `mesh_000246.obj` | 47,928 B | 224 | Foundation, Roof, WindowFrame |
| `mesh_000386.obj` | 16,962 B | 136 | Fence, Grid/Billboard components |
| `mesh_000596.obj` | 1,619 B | 14 | Outer_LOD0, Column, Roof |
| `mesh_000716.obj` | 8,241 B | 48 | Interior, LowerRoof, BigWall |
| `mesh_000786.obj` | 46,590 B | 200 | Interior |
| `mesh_001116.obj` | 1,834 B | 10 | Fence |
| `mesh_001256.obj` | 381 B | 2 | FrontLowerWall, Plaque |
| `mesh_001306.obj` | 92,952 B | 726 | RoofAirConditioning, SideWalk |
| `mesh_001316.obj` | 56,781 B | 304 | BlankWall |
| `mesh_001396.obj` | 11,301 B | 110 | Fence |
| `mesh_001616.obj` | 76,572 B | 516 | SmallWall |
| `mesh_001646.obj` | 2,309 B | 13 | SmallWall.001 |
| `mesh_001656.obj` | 3,301 B | 18 | SmallWall.002 |
| `mesh_001666.obj` | 68,363 B | 390 | SmallWall.003 |
| `mesh_001676.obj` | 11,805 B | 64 | Wal_Other |
| `mesh_001686.obj` | 9,419 B | 68 | Wal_Other.001 |
| `mesh_001696.obj` | 5,599 B | 44 | Wal_Other.002 |
| `mesh_001706.obj` | 1,463 B | 8 | Wal_Other.003 |
| `mesh_001716.obj` | 51,685 B | 552 | Wal_Other.004 |
| `mesh_001726.obj` | 118,837 B | 820 | Wal_Other.005 |
| `mesh_001736.obj` | 102,035 B | 588 | Wal_Other.006 |
| `mesh_001746.obj` | 779 B | 6 | Wall |
| `mesh_001796.obj` | 25,592 B | 303 | Roof, Foundation, Tank |

## Roads and Traffic Surfaces

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000176.obj` | 5,471 B | 40 | Road |
| `mesh_000196.obj` | 87,096 B | 572 | Road_lowres |
| `mesh_000256.obj` | 6,776 B | 40 | Road_lowres |
| `mesh_000296.obj` | 1,344 B | 7 | SideWalk.002 |
| `mesh_000306.obj` | 14,240 B | 88 | Road_lowres |
| `mesh_000346.obj` | 21,311 B | 134 | Road_lowres |
| `mesh_000356.obj` | 8,867 B | 48 | Road_lowres |
| `mesh_000366.obj` | 386 B | 2 | Road |
| `mesh_000406.obj` | 194,119 B | 1,186 | Road |
| `mesh_000416.obj` | 32,972 B | 196 | Road, Road_lowres |
| `mesh_000426.obj` | 16,495 B | 96 | Road_lowres |
| `mesh_000446.obj` | 1,468 B | 8 | Road, Road_(1) |
| `mesh_000476.obj` | 6,595 B | 36 | Road |
| `mesh_000536.obj` | 6,694 B | 40 | Road_lowres |
| `mesh_000606.obj` | 44,710 B | 312 | Sidewalk_lowres |
| `mesh_000616.obj` | 380 B | 2 | Sidewalk_lowres |
| `mesh_000626.obj` | 2,945 B | 16 | Sidewalk_lowres |
| `mesh_000636.obj` | 8,239 B | 48 | Sidewalk_lowres |
| `mesh_000646.obj` | 1,849 B | 10 | Sidewalk_lowres |
| `mesh_001406.obj` | 15,424 B | 120 | modularpath_2m_straight |
| `mesh_001416.obj` | 3,000 B | 18 | modularpath_2m_corner |

## Vegetation

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000726.obj` | 33,715 B | 204 | GrassSingle_LOD0 |
| `mesh_000736.obj` | 1,077 B | 9 | SGS_Shrub_LOD0/LOD1, GrassSingle |
| `mesh_000746.obj` | 4,849 B | 31 | SGS_Shrub_LOD0/LOD1, GrassSingle |
| `mesh_000756.obj` | 15,876 B | 114 | SGS_Shrub_LOD0/LOD1, GrassSingle |
| `mesh_000766.obj` | 338,244 B | 2,790 | SGS_Shrub_LOD0/LOD1, GrassSingle |

No direct name hints for Tree, Bush, Flower, or Hedge were found. `Shrub` and `Grass` form the verifiable vegetation group.

## Water

| Mesh | Size | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000336.obj` | 2,982 B | 18 | `Water_Plane`, `Circular_Water` |

`seweragepipe` is listed under Sewer and not counted as a water surface.

## Other Identifiable Groups

### Character and Interaction Meshes

| Mesh Range | Examples | Classification |
|---|---|---|
| `mesh_000006`–`mesh_000076` | Model, Eye, EyeBrow, Pupil | Character models |
| `mesh_000046.obj` | Interaction, Pickpocket_Interaction, Capsule | Interaction / collider geometry |
| `mesh_000086`–`mesh_000126` | Body_LOD0–LOD3, Face | Character LODs |
| `mesh_000136`–`mesh_000156` | Body LODs, Face, Worldbase | Character / worldbase geometry |

### Harbor, Vehicles, and Technical Props

| Mesh Range | Examples | Classification |
|---|---|---|
| `mesh_000806.obj` | ShippingContainer | Harbor / industrial prop |
| `mesh_000846`–`mesh_001096` | Truck, Hanger, Wheel, Plane | Vehicle / trailer components |
| `mesh_001196.obj` | mooring_bollard | Harbor prop |
| `mesh_002226`–`mesh_002266` | Arm, Housing, Pole, Body | Technical object |

### Cemetery and Chapel

| Mesh Range | Examples | Classification |
|---|---|---|
| `mesh_001106.obj` | Chapel_LOD0 | Chapel |
| `mesh_001136`–`mesh_001186` | gravesurface, Cross, Headstone, Round, Small, Square | Tomb / cemetery objects |
| `mesh_001226`–`mesh_001296` | gravesurface, Cross, Headstone | Additional cemetery / environment objects |

### Product and Inventory Objects

| Mesh Range | Examples | Classification |
|---|---|---|
| `mesh_002056`–`mesh_002086` | Bud_LOD0–LOD3 | Product LODs |
| `mesh_002096`–`mesh_002116` | Crystal_LOD0–LOD2 | Product LODs |
| `mesh_002126`–`mesh_002156` | CocaineRock_LOD0–LOD3 | Product LODs |
| `mesh_002166`–`mesh_002186` | Mushroom1_LOD0–LOD2 | Product LODs |

These groups demonstrate that the export contains not only static map scenery, but also character, interaction, vehicle, and product meshes.

## Largest Files

| Rank | Mesh | Size | Vertices | Triangles | Share of Total Size |
|---:|---|---:|---:|---:|---:|
| 1 | `mesh_001723.obj` | 760.19 MiB | 4,198,401 | 8,388,608 | 64.03 % |
| 2 | `mesh_001724.obj` | 11.88 MiB | 66,049 | 131,072 | 1.00 % |
| 3 | `mesh_001185.obj` | 10.05 MiB | 71,681 | 88,285 | 0.85 % |
| 4 | `mesh_000049.obj` | 7.10 MiB | 63,288 | 43,227 | 0.60 % |
| 5 | `mesh_000073.obj` | 7.03 MiB | 63,878 | 40,408 | 0.59 % |
| 6 | `mesh_000074.obj` | 6.97 MiB | 63,916 | 39,338 | 0.59 % |
| 7 | `mesh_000050.obj` | 6.97 MiB | 63,945 | 39,699 | 0.59 % |
| 8 | `mesh_000040.obj` | 6.95 MiB | 63,953 | 39,731 | 0.59 % |
| 9 | `mesh_000048.obj` | 6.95 MiB | 63,942 | 39,731 | 0.59 % |
| 10 | `mesh_000035.obj` | 6.93 MiB | 63,820 | 39,073 | 0.58 % |
| 11 | `mesh_000046.obj` | 6.93 MiB | 63,645 | 39,582 | 0.58 % |
| 12 | `mesh_000030.obj` | 6.93 MiB | 63,953 | 38,912 | 0.58 % |
| 13 | `mesh_000076.obj` | 6.93 MiB | 63,502 | 39,514 | 0.58 % |
| 14 | `mesh_000047.obj` | 6.92 MiB | 63,679 | 39,628 | 0.58 % |
| 15 | `mesh_000052.obj` | 6.92 MiB | 63,662 | 39,483 | 0.58 % |
| 16 | `mesh_000042.obj` | 6.92 MiB | 63,800 | 39,115 | 0.58 % |
| 17 | `mesh_000019.obj` | 6.92 MiB | 63,845 | 39,315 | 0.58 % |
| 18 | `mesh_000043.obj` | 6.92 MiB | 63,775 | 39,109 | 0.58 % |
| 19 | `mesh_000034.obj` | 6.92 MiB | 63,931 | 39,117 | 0.58 % |
| 20 | `mesh_000058.obj` | 6.91 MiB | 63,961 | 38,862 | 0.58 % |

`mesh_001723.obj` is a prominent outlier. It contains exactly 2²³ triangles and 4,198,401 vertices, but no name hint in `instances.csv`. The pattern suggests a very high-resolution terrain/surface geometry; substantive assignment requires visual inspection.

## Alphabetical File Excerpt — First 100 Meshes

Due to the total volume of 2,271 files, the first 100 are shown completely in alphabetical/numerical order. All remaining files are covered by the 100-block distribution table.

| Mesh | Size (Bytes) | Triangles | Name Hint |
|---|---:|---:|---|
| `mesh_000000.obj` | 24,726 | 304 | — |
| `mesh_000001.obj` | 25,065 | 304 | — |
| `mesh_000002.obj` | 37,379 | 480 | — |
| `mesh_000003.obj` | 3,905 | 28 | — |
| `mesh_000004.obj` | 74,364 | 832 | — |
| `mesh_000005.obj` | 69,200 | 768 | — |
| `mesh_000006.obj` | 389 | 2 | Model |
| `mesh_000007.obj` | 8,369 | 112 | — |
| `mesh_000008.obj` | 791,170 | 8,584 | — |
| `mesh_000009.obj` | 289,101 | 3,004 | — |
| `mesh_000010.obj` | 145,037 | 1,492 | — |
| `mesh_000011.obj` | 77,052 | 668 | — |
| `mesh_000012.obj` | 15,199 | 192 | — |
| `mesh_000013.obj` | 802,288 | 8,584 | — |
| `mesh_000014.obj` | 15,212 | 192 | — |
| `mesh_000015.obj` | 5,714,211 | 59,399 | — |
| `mesh_000016.obj` | 7,067,190 | 36,116 | Model |
| `mesh_000017.obj` | 7,075,257 | 38,067 | — |
| `mesh_000018.obj` | 7,206,041 | 38,318 | — |
| `mesh_000019.obj` | 7,255,424 | 39,315 | — |
| `mesh_000020.obj` | 2,201 | 12 | — |
| `mesh_000021.obj` | 4,144 | 26 | — |
| `mesh_000022.obj` | 4,148 | 26 | — |
| `mesh_000023.obj` | 2,203 | 12 | — |
| `mesh_000024.obj` | 519,002 | 3,237 | — |
| `mesh_000025.obj` | 7,184,530 | 38,083 | — |
| `mesh_000026.obj` | 2,302,297 | 14,116 | Eye |
| `mesh_000027.obj` | 2,205 | 12 | — |
| `mesh_000028.obj` | 273,805 | 1,752 | — |
| `mesh_000029.obj` | 7,123,056 | 37,448 | — |
| `mesh_000030.obj` | 7,264,256 | 38,912 | — |
| `mesh_000031.obj` | 59,181 | 398 | — |
| `mesh_000032.obj` | 519,571 | 3,058 | — |
| `mesh_000033.obj` | 15,949 | 200 | — |
| `mesh_000034.obj` | 7,253,366 | 39,117 | — |
| `mesh_000035.obj` | 7,269,899 | 39,073 | — |
| `mesh_000036.obj` | 7,157,564 | 37,421 | EyeBrow |
| `mesh_000037.obj` | 7,227,665 | 39,343 | — |
| `mesh_000038.obj` | 7,127,721 | 37,670 | — |
| `mesh_000039.obj` | 7,203,817 | 38,192 | — |
| `mesh_000040.obj` | 7,289,903 | 39,731 | — |
| `mesh_000041.obj` | 7,204,483 | 38,278 | — |
| `mesh_000042.obj` | 7,255,579 | 39,115 | — |
| `mesh_000043.obj` | 7,254,038 | 39,109 | — |
| `mesh_000044.obj` | 7,243,076 | 38,744 | — |
| `mesh_000045.obj` | 7,245,426 | 38,852 | — |
| `mesh_000046.obj` | 7,267,335 | 39,582 | Interaction, Pickpocket_Interaction, Capsule |
| `mesh_000047.obj` | 7,258,794 | 39,628 | — |
| `mesh_000048.obj` | 7,289,082 | 39,731 | — |
| `mesh_000049.obj` | 7,440,237 | 43,227 | — |
| `mesh_000050.obj` | 7,308,868 | 39,699 | — |
| `mesh_000051.obj` | 7,132,358 | 37,187 | — |
| `mesh_000052.obj` | 7,256,554 | 39,483 | — |
| `mesh_000053.obj` | 7,185,776 | 37,832 | — |
| `mesh_000054.obj` | 7,199,363 | 38,554 | — |
| `mesh_000055.obj` | 7,190,895 | 38,190 | — |
| `mesh_000056.obj` | 792 | 6 | Sphere, GameObject, VehicleRecoveryPoint |
| `mesh_000057.obj` | 639,641 | 3,175 | — |
| `mesh_000058.obj` | 7,248,663 | 38,862 | — |
| `mesh_000059.obj` | 262,484 | 1,301 | — |
| `mesh_000060.obj` | 7,188,132 | 37,488 | — |
| `mesh_000061.obj` | 7,019,174 | 36,327 | — |
| `mesh_000062.obj` | 7,193,322 | 37,457 | — |
| `mesh_000063.obj` | 7,104,940 | 35,753 | — |
| `mesh_000064.obj` | 7,226,485 | 38,395 | — |
| `mesh_000065.obj` | 489,020 | 3,064 | — |
| `mesh_000066.obj` | 3,731 | 22 | Quad, Impostor, Billboard |
| `mesh_000067.obj` | 168,455 | 1,064 | — |
| `mesh_000068.obj` | 1,402,576 | 7,598 | — |
| `mesh_000069.obj` | 3,734 | 22 | — |
| `mesh_000070.obj` | 21,255 | 112 | — |
| `mesh_000071.obj` | 190,582 | 1,168 | — |
| `mesh_000072.obj` | 3,049 | 24 | — |
| `mesh_000073.obj` | 7,371,973 | 40,408 | — |
| `mesh_000074.obj` | 7,313,693 | 39,338 | — |
| `mesh_000075.obj` | 7,086,820 | 38,764 | — |
| `mesh_000076.obj` | 7,262,941 | 39,514 | Pupil |
| `mesh_000077.obj` | 379 | 2 | — |
| `mesh_000078.obj` | 38,915 | 274 | — |
| `mesh_000079.obj` | 120,293 | 878 | — |
| `mesh_000080.obj` | 272,326 | 1,487 | — |
| `mesh_000081.obj` | 3,511 | 26 | — |
| `mesh_000082.obj` | 1,534,826 | 8,489 | — |
| `mesh_000083.obj` | 52,949 | 356 | — |
| `mesh_000084.obj` | 27,716 | 180 | — |
| `mesh_000085.obj` | 32,089 | 248 | — |
| `mesh_000086.obj` | 30,465 | 236 | Body_LOD0 |
| `mesh_000087.obj` | 2,792 | 16 | — |
| `mesh_000088.obj` | 6,209 | 44 | — |
| `mesh_000089.obj` | 1,089 | 6 | — |
| `mesh_000090.obj` | 22,254 | 130 | — |
| `mesh_000091.obj` | 5,636 | 34 | — |
| `mesh_000092.obj` | 5,602 | 34 | — |
| `mesh_000093.obj` | 25,712 | 218 | — |
| `mesh_000094.obj` | 27,714 | 180 | — |
| `mesh_000095.obj` | 32,089 | 248 | — |
| `mesh_000096.obj` | 30,465 | 236 | Body_LOD1 |
| `mesh_000097.obj` | 26,465 | 180 | — |
| `mesh_000098.obj` | 34,898 | 248 | — |
| `mesh_000099.obj` | 10,949 | 104 | — |

**Continuation:** `mesh_000100.obj` through `mesh_002270.obj` are covered in the 100-block distribution table. Semantically identifiable representatives are listed in the category and region tables.

## Notable Observations

1. `mesh_001723.obj` dominates the export with 760.19 MiB and 8,388,608 triangles.
2. The sequence is gapless: 2,271 files from index 0 to 2,270.
3. Name mapping noticeably follows every tenth mesh ending in digit `6`; this matches the damaged/shortened mesh reference in `instances.csv` and should not be interpreted as a semantic export order.
4. The export contains characters, interaction bodies, vehicles, props, and product models in addition to map geometry.
5. Directly recognizable map categories are buildings/components, roads/sidewalks, grass/shrubs, water, and docks/sewer components.
6. A complete region distribution cannot be reliably established with the existing filenames and damaged transformation data.

## Sources

- `Source/Game/Docs/05-HaylandPoint.md`
- `Source/Game/Docs/41-Map-Detail.md`
- `../systems/13-Regions.md`
- `Source/Game/HaylandPoint/HaylandPoint/instances.csv`
- `Source/Game/HaylandPoint/HaylandPoint/import_hayland_point.py`
- `Source/Game/HaylandPoint/HaylandPoint/export_region7_houses.py`
- 2,271 directly evaluated OBJ files in `Source/Game/HaylandPoint/HaylandPoint/meshes/`
