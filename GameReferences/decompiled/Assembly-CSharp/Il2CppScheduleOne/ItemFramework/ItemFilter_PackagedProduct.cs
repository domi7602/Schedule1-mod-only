using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.ItemFramework;

public class ItemFilter_PackagedProduct : ItemFilter_Category
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0;

	static ItemFilter_PackagedProduct()
	{
		Il2CppClassPointerStore<ItemFilter_PackagedProduct>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemFilter_PackagedProduct");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemFilter_PackagedProduct>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFilter_PackagedProduct>.NativeClassPtr, 100672488);
		NativeMethodInfoPtr_DoesItemMatchFilter_Public_Virtual_Boolean_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFilter_PackagedProduct>.NativeClassPtr, 100672489);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 167041, RefRangeEnd = 167042, XrefRangeStart = 167028, XrefRangeEnd = 167041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemFilter_PackagedProduct()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemFilter_PackagedProduct>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 167042, XrefRangeEnd = 167052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public ItemFilter_PackagedProduct(IntPtr pointer)
		: base(pointer)
	{
	}
}
