using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class ItemSet : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Items;

	private static readonly System.IntPtr NativeFieldInfoPtr_SlotFilters;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetJSON_Public_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_ItemSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadTo_Public_Void_List_1_ItemSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadTo_Public_Void_Il2CppReferenceArray_1_ItemSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadTo_Public_Void_ItemSlot_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_String_byref_DeserializedItemSet_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_ItemSet_byref_DeserializedItemSet_0;

	public unsafe Il2CppStringArray Items
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<SlotFilter> SlotFilters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SlotFilters);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SlotFilter>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SlotFilters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ItemSet()
	{
		Il2CppClassPointerStore<ItemSet>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ItemSet");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemSet>.NativeClassPtr);
		NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, "Items");
		NativeFieldInfoPtr_SlotFilters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, "SlotFilters");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669304);
		NativeMethodInfoPtr_GetJSON_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669305);
		NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669306);
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669307);
		NativeMethodInfoPtr_LoadTo_Public_Void_List_1_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669308);
		NativeMethodInfoPtr_LoadTo_Public_Void_Il2CppReferenceArray_1_ItemSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669309);
		NativeMethodInfoPtr_LoadTo_Public_Void_ItemSlot_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669310);
		NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_String_byref_DeserializedItemSet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669311);
		NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_ItemSet_byref_DeserializedItemSet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSet>.NativeClassPtr, 100669312);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 134127, XrefRangeEnd = 134138, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemSet(List<ItemData> items)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemSet>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)items);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 134139, RefRangeEnd = 134143, XrefRangeStart = 134138, XrefRangeEnd = 134139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetJSON()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetJSON_Public_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 134176, RefRangeEnd = 134199, XrefRangeStart = 134143, XrefRangeEnd = 134176, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemSet(List<ItemSlot> itemSlots)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemSet>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemSlots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134222, RefRangeEnd = 134224, XrefRangeStart = 134199, XrefRangeEnd = 134222, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemSet(Il2CppReferenceArray<ItemSlot> itemSlots)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemSet>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemSlots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(15)]
	[CachedScanResults(RefRangeStart = 134233, RefRangeEnd = 134248, XrefRangeStart = 134224, XrefRangeEnd = 134233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadTo(List<ItemSlot> slots)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadTo_Public_Void_List_1_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 134252, RefRangeEnd = 134256, XrefRangeStart = 134248, XrefRangeEnd = 134252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadTo(Il2CppReferenceArray<ItemSlot> slots)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadTo_Public_Void_Il2CppReferenceArray_1_ItemSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 134258, RefRangeEnd = 134264, XrefRangeStart = 134256, XrefRangeEnd = 134258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadTo(ItemSlot slot, int index = 0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadTo_Public_Void_ItemSlot_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 134282, RefRangeEnd = 134287, XrefRangeStart = 134264, XrefRangeEnd = 134282, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TryDeserialize(string json, out DeserializedItemSet itemSet)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(json);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_String_byref_DeserializedItemSet_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		itemSet = ((num3 == 0) ? null : new DeserializedItemSet(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134310, RefRangeEnd = 134312, XrefRangeStart = 134287, XrefRangeEnd = 134310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TryDeserialize(ItemSet set, out DeserializedItemSet itemSet)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)set);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_ItemSet_byref_DeserializedItemSet_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		itemSet = ((num3 == 0) ? null : new DeserializedItemSet(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ItemSet(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
