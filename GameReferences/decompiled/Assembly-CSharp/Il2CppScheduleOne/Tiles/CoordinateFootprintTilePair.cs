using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Tiles;

[System.Serializable]
public sealed class CoordinateFootprintTilePair : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_coord;

	private static readonly System.IntPtr NativeFieldInfoPtr_footprintTile;

	public unsafe Coordinate coord
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coord);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Coordinate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coord)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)coordinate));
		}
	}

	public unsafe FootprintTile footprintTile
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_footprintTile);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<FootprintTile>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_footprintTile)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)footprintTile));
		}
	}

	static CoordinateFootprintTilePair()
	{
		Il2CppClassPointerStore<CoordinateFootprintTilePair>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tiles", "CoordinateFootprintTilePair");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CoordinateFootprintTilePair>.NativeClassPtr);
		NativeFieldInfoPtr_coord = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateFootprintTilePair>.NativeClassPtr, "coord");
		NativeFieldInfoPtr_footprintTile = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateFootprintTilePair>.NativeClassPtr, "footprintTile");
	}

	public CoordinateFootprintTilePair(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public CoordinateFootprintTilePair()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CoordinateFootprintTilePair>.NativeClassPtr))
	{
	}
}
