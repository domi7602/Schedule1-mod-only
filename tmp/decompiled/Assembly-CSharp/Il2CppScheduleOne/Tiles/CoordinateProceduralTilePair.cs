using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Object;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Tiles;

[System.Serializable]
public sealed class CoordinateProceduralTilePair : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_coord;

	private static readonly System.IntPtr NativeFieldInfoPtr_tileParent;

	private static readonly System.IntPtr NativeFieldInfoPtr_tileIndex;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tile_Public_get_ProceduralTile_0;

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

	public unsafe NetworkObject tileParent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tileParent);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NetworkObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tileParent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)networkObject));
		}
	}

	public unsafe int tileIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tileIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tileIndex)) = num;
		}
	}

	public unsafe ProceduralTile tile
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 100932, RefRangeEnd = 100940, XrefRangeStart = 100921, XrefRangeEnd = 100932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tile_Public_get_ProceduralTile_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ProceduralTile>(intPtr) : null;
		}
	}

	static CoordinateProceduralTilePair()
	{
		Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tiles", "CoordinateProceduralTilePair");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr);
		NativeFieldInfoPtr_coord = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr, "coord");
		NativeFieldInfoPtr_tileParent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr, "tileParent");
		NativeFieldInfoPtr_tileIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr, "tileIndex");
		NativeMethodInfoPtr_get_tile_Public_get_ProceduralTile_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr, 100666875);
	}

	public CoordinateProceduralTilePair(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public CoordinateProceduralTilePair()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CoordinateProceduralTilePair>.NativeClassPtr))
	{
	}
}
