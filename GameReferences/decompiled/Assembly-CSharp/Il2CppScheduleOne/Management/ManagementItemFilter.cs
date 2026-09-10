using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Management;

public class ManagementItemFilter : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EMode")]
	public enum EMode
	{
		Whitelist,
		Blacklist
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__Mode_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Items_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Mode_Public_get_EMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Mode_Private_set_Void_EMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Items_Public_get_List_1_ItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Items_Private_set_Void_List_1_ItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_EMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMode_Public_Void_EMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddItem_Public_Void_ItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveItem_Public_Void_ItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Contains_Public_Boolean_ItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoesItemMeetFilter_Public_Boolean_ItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDescription_Public_String_0;

	public unsafe EMode _Mode_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Mode_k__BackingField);
			return *(EMode*)num;
		}
		set
		{
			*(EMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Mode_k__BackingField)) = eMode;
		}
	}

	public unsafe List<ItemDefinition> _Items_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Items_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemDefinition>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Items_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe EMode Mode
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29049, RefRangeEnd = 29051, XrefRangeStart = 29049, XrefRangeEnd = 29051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Mode_Public_get_EMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Mode_Private_set_Void_EMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<ItemDefinition> Items
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Items_Public_get_List_1_ItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemDefinition>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Items_Private_set_Void_List_1_ItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ManagementItemFilter()
	{
		Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Management", "ManagementItemFilter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr);
		NativeFieldInfoPtr__Mode_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, "<Mode>k__BackingField");
		NativeFieldInfoPtr__Items_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, "<Items>k__BackingField");
		NativeMethodInfoPtr_get_Mode_Public_get_EMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670955);
		NativeMethodInfoPtr_set_Mode_Private_set_Void_EMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670956);
		NativeMethodInfoPtr_get_Items_Public_get_List_1_ItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670957);
		NativeMethodInfoPtr_set_Items_Private_set_Void_List_1_ItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670958);
		NativeMethodInfoPtr__ctor_Public_Void_EMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670959);
		NativeMethodInfoPtr_SetMode_Public_Void_EMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670960);
		NativeMethodInfoPtr_AddItem_Public_Void_ItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670961);
		NativeMethodInfoPtr_RemoveItem_Public_Void_ItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670962);
		NativeMethodInfoPtr_Contains_Public_Boolean_ItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670963);
		NativeMethodInfoPtr_DoesItemMeetFilter_Public_Boolean_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670964);
		NativeMethodInfoPtr_GetDescription_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr, 100670965);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 150581, RefRangeEnd = 150583, XrefRangeStart = 150568, XrefRangeEnd = 150581, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ManagementItemFilter(EMode mode)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ManagementItemFilter>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_EMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMode(EMode mode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMode_Public_Void_EMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 150589, RefRangeEnd = 150591, XrefRangeStart = 150583, XrefRangeEnd = 150589, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddItem(ItemDefinition item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddItem_Public_Void_ItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 150591, XrefRangeEnd = 150595, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveItem(ItemDefinition item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveItem_Public_Void_ItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 150595, XrefRangeEnd = 150599, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Contains(ItemDefinition item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Contains_Public_Boolean_ItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 150606, RefRangeEnd = 150607, XrefRangeStart = 150599, XrefRangeEnd = 150606, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool DoesItemMeetFilter(ItemInstance item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoesItemMeetFilter_Public_Boolean_ItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 150607, XrefRangeEnd = 150615, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetDescription()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDescription_Public_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public ManagementItemFilter(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
