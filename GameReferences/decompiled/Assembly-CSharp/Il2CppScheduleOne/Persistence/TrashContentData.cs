using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Trash;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence;

[System.Serializable]
public class TrashContentData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_TrashIDs;

	private static readonly System.IntPtr NativeFieldInfoPtr_TrashQuantities;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppStringArray_Il2CppStructArray_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_TrashItem_0;

	public unsafe Il2CppStringArray TrashIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TrashIDs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TrashIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<int> TrashQuantities
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TrashQuantities);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TrashQuantities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static TrashContentData()
	{
		Il2CppClassPointerStore<TrashContentData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence", "TrashContentData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr);
		NativeFieldInfoPtr_TrashIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr, "TrashIDs");
		NativeFieldInfoPtr_TrashQuantities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr, "TrashQuantities");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr, 100668911);
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppStringArray_Il2CppStructArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr, 100668912);
		NativeMethodInfoPtr__ctor_Public_Void_List_1_TrashItem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr, 100668913);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 127292, RefRangeEnd = 127294, XrefRangeStart = 127283, XrefRangeEnd = 127292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashContentData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(53)]
	[CachedScanResults(RefRangeStart = 100943, RefRangeEnd = 100996, XrefRangeStart = 100943, XrefRangeEnd = 100996, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashContentData(Il2CppStringArray trashIDs, Il2CppStructArray<int> trashQuantities)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashIDs);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashQuantities);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppStringArray_Il2CppStructArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 127357, RefRangeEnd = 127358, XrefRangeStart = 127294, XrefRangeEnd = 127357, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashContentData(List<TrashItem> trashItems)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashContentData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashItems);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_TrashItem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashContentData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
