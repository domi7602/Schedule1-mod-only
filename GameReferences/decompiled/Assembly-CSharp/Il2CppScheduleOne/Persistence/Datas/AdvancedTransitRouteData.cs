using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Management;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class AdvancedTransitRouteData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SourceGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_DestinationGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_FilterMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_FilterItemIDs;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_EMode_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string SourceGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SourceGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SourceGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string DestinationGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe ManagementItemFilter.EMode FilterMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FilterMode);
			return *(ManagementItemFilter.EMode*)num;
		}
		set
		{
			*(ManagementItemFilter.EMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FilterMode)) = eMode;
		}
	}

	public unsafe List<string> FilterItemIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FilterItemIDs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FilterItemIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static AdvancedTransitRouteData()
	{
		Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "AdvancedTransitRouteData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr);
		NativeFieldInfoPtr_SourceGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr, "SourceGUID");
		NativeFieldInfoPtr_DestinationGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr, "DestinationGUID");
		NativeFieldInfoPtr_FilterMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr, "FilterMode");
		NativeFieldInfoPtr_FilterItemIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr, "FilterItemIDs");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_EMode_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr, 100669376);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr, 100669377);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134787, RefRangeEnd = 134788, XrefRangeStart = 134783, XrefRangeEnd = 134787, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AdvancedTransitRouteData(string sourceGUID, string destinationGUID, ManagementItemFilter.EMode filtermode, List<string> filterGUIDs)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(sourceGUID);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(destinationGUID);
		*(ManagementItemFilter.EMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &filtermode;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)filterGUIDs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_EMode_List_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AdvancedTransitRouteData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AdvancedTransitRouteData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AdvancedTransitRouteData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
