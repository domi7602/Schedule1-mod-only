using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class GridItemData : BuildableItemData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_GridGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_OriginCoordinate;

	private static readonly System.IntPtr NativeFieldInfoPtr_Rotation;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_0;

	public unsafe string GridGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GridGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GridGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Vector2 OriginCoordinate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OriginCoordinate);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OriginCoordinate)) = vector;
		}
	}

	public unsafe int Rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation)) = num;
		}
	}

	static GridItemData()
	{
		Il2CppClassPointerStore<GridItemData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "GridItemData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GridItemData>.NativeClassPtr);
		NativeFieldInfoPtr_GridGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GridItemData>.NativeClassPtr, "GridGUID");
		NativeFieldInfoPtr_OriginCoordinate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GridItemData>.NativeClassPtr, "OriginCoordinate");
		NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GridItemData>.NativeClassPtr, "Rotation");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GridItemData>.NativeClassPtr, 100669424);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135109, RefRangeEnd = 135110, XrefRangeStart = 135103, XrefRangeEnd = 135109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GridItemData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GridItemData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &originCoordinate;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GridItemData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
