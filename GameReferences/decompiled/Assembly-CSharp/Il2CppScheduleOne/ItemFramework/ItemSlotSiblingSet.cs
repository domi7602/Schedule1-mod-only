using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.ItemFramework;

public class ItemSlotSiblingSet : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Slots;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_ItemSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSlot_Public_Void_ItemSlot_0;

	public unsafe List<ItemSlot> Slots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Slots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemSlot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Slots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static ItemSlotSiblingSet()
	{
		Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemSlotSiblingSet");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr);
		NativeFieldInfoPtr_Slots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr, "Slots");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr, 100672613);
		NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr, 100672614);
		NativeMethodInfoPtr_AddSlot_Public_Void_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr, 100672615);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 168434, RefRangeEnd = 168435, XrefRangeStart = 168424, XrefRangeEnd = 168434, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemSlotSiblingSet([Optional] Il2CppReferenceArray<ItemSlot> slots)
	{
		if (slots == null)
		{
			slots = new Il2CppReferenceArray<ItemSlot>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 168458, RefRangeEnd = 168467, XrefRangeStart = 168435, XrefRangeEnd = 168458, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemSlotSiblingSet(List<ItemSlot> slots)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemSlotSiblingSet>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 168487, RefRangeEnd = 168489, XrefRangeStart = 168467, XrefRangeEnd = 168487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddSlot(ItemSlot slot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSlot_Public_Void_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ItemSlotSiblingSet(params ItemSlot[] slots)
		: this(new Il2CppReferenceArray<ItemSlot>(slots))
	{
	}

	public ItemSlotSiblingSet(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
