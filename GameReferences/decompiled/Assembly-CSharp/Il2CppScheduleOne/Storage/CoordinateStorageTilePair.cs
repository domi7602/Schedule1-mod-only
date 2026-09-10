using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem;

namespace Il2CppScheduleOne.Storage;

[System.Serializable]
public sealed class CoordinateStorageTilePair : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_coord;

	private static readonly System.IntPtr NativeFieldInfoPtr_tile;

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

	public unsafe StorageTile tile
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tile);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorageTile>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tile)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storageTile));
		}
	}

	static CoordinateStorageTilePair()
	{
		Il2CppClassPointerStore<CoordinateStorageTilePair>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Storage", "CoordinateStorageTilePair");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CoordinateStorageTilePair>.NativeClassPtr);
		NativeFieldInfoPtr_coord = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateStorageTilePair>.NativeClassPtr, "coord");
		NativeFieldInfoPtr_tile = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateStorageTilePair>.NativeClassPtr, "tile");
	}

	public CoordinateStorageTilePair(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public CoordinateStorageTilePair()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CoordinateStorageTilePair>.NativeClassPtr))
	{
	}
}
