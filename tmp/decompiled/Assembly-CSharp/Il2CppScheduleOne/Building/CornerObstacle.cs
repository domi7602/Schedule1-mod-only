using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Building;

public class CornerObstacle : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_obstacleEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_parentFootprint;

	private static readonly IntPtr NativeFieldInfoPtr_coordinates;

	private static readonly IntPtr NativeMethodInfoPtr_GetNeighbourTiles_Public_List_1_Tile_Tile_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApproxEquals_Private_Boolean_Single_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool obstacleEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_obstacleEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_obstacleEnabled)) = flag;
		}
	}

	public unsafe FootprintTile parentFootprint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parentFootprint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<FootprintTile>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parentFootprint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)footprintTile));
		}
	}

	public unsafe Vector2 coordinates
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coordinates);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coordinates)) = vector;
		}
	}

	static CornerObstacle()
	{
		Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Building", "CornerObstacle");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr);
		NativeFieldInfoPtr_obstacleEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr, "obstacleEnabled");
		NativeFieldInfoPtr_parentFootprint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr, "parentFootprint");
		NativeFieldInfoPtr_coordinates = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr, "coordinates");
		NativeMethodInfoPtr_GetNeighbourTiles_Public_List_1_Tile_Tile_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr, 100676668);
		NativeMethodInfoPtr_ApproxEquals_Private_Boolean_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr, 100676669);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr, 100676670);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 213362, RefRangeEnd = 213364, XrefRangeStart = 213332, XrefRangeEnd = 213362, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<Tile> GetNeighbourTiles(Tile pairedTile)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pairedTile);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNeighbourTiles_Public_List_1_Tile_Tile_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Tile>>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe bool ApproxEquals(float a, float b, float precision)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &precision;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApproxEquals_Private_Boolean_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213364, XrefRangeEnd = 213367, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CornerObstacle()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CornerObstacle>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CornerObstacle(IntPtr pointer)
		: base(pointer)
	{
	}
}
