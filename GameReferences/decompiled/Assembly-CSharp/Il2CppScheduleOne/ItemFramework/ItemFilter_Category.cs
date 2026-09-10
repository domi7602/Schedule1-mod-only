using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.ItemFramework;

public class ItemFilter_Category : ItemFilter
{
	private static readonly IntPtr NativeFieldInfoPtr_AcceptedCategories;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_EItemCategory_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0;

	public unsafe List<EItemCategory> AcceptedCategories
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AcceptedCategories);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<EItemCategory>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AcceptedCategories)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static ItemFilter_Category()
	{
		Il2CppClassPointerStore<ItemFilter_Category>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemFilter_Category");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemFilter_Category>.NativeClassPtr);
		NativeFieldInfoPtr_AcceptedCategories = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemFilter_Category>.NativeClassPtr, "AcceptedCategories");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_EItemCategory_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFilter_Category>.NativeClassPtr, 100672473);
		NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFilter_Category>.NativeClassPtr, 100672474);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 166948, RefRangeEnd = 166952, XrefRangeStart = 166939, XrefRangeEnd = 166948, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemFilter_Category(List<EItemCategory> acceptedCategories)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemFilter_Category>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)acceptedCategories);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_EItemCategory_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166952, XrefRangeEnd = 166955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool DoesItemMatchFilter(ItemInstance instance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ItemFilter_Category(IntPtr pointer)
		: base(pointer)
	{
	}
}
