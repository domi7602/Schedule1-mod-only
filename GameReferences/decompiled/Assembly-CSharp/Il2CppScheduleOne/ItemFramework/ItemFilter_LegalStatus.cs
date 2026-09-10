using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Core.Items.Framework;

namespace Il2CppScheduleOne.ItemFramework;

public class ItemFilter_LegalStatus : ItemFilter
{
	private static readonly IntPtr NativeFieldInfoPtr_RequiredLegalStatus;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_ELegalStatus_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0;

	public unsafe ELegalStatus RequiredLegalStatus
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiredLegalStatus);
			return *(ELegalStatus*)num;
		}
		set
		{
			*(ELegalStatus*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiredLegalStatus)) = eLegalStatus;
		}
	}

	static ItemFilter_LegalStatus()
	{
		Il2CppClassPointerStore<ItemFilter_LegalStatus>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemFilter_LegalStatus");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemFilter_LegalStatus>.NativeClassPtr);
		NativeFieldInfoPtr_RequiredLegalStatus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemFilter_LegalStatus>.NativeClassPtr, "RequiredLegalStatus");
		NativeMethodInfoPtr__ctor_Public_Void_ELegalStatus_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFilter_LegalStatus>.NativeClassPtr, 100672484);
		NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFilter_LegalStatus>.NativeClassPtr, 100672485);
	}

	[CallerCount(83)]
	[CachedScanResults(RefRangeStart = 65267, RefRangeEnd = 65350, XrefRangeStart = 65267, XrefRangeEnd = 65350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemFilter_LegalStatus(ELegalStatus requiredLegalStatus)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemFilter_LegalStatus>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&requiredLegalStatus);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ELegalStatus_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 167018, XrefRangeEnd = 167019, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public ItemFilter_LegalStatus(IntPtr pointer)
		: base(pointer)
	{
	}
}
