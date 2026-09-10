using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

public class TrashContainerData : GridItemData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ContentData;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_TrashContentData_0;

	public unsafe TrashContentData ContentData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ContentData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrashContentData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ContentData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashContentData));
		}
	}

	static TrashContainerData()
	{
		Il2CppClassPointerStore<TrashContainerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "TrashContainerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashContainerData>.NativeClassPtr);
		NativeFieldInfoPtr_ContentData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashContainerData>.NativeClassPtr, "ContentData");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_TrashContentData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashContainerData>.NativeClassPtr, 100669442);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 135060, RefRangeEnd = 135067, XrefRangeStart = 135060, XrefRangeEnd = 135067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashContainerData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, TrashContentData contentData)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashContainerData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &originCoordinate;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)contentData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_TrashContentData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashContainerData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
