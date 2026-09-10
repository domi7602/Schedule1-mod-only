using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.CustomUI;

namespace Il2CppScheduleOne.UI;

public class UISelectable_CashItemSlot : UISelectable_ItemSlot
{
	private static readonly IntPtr NativeMethodInfoPtr_CanBeSelectedWhileDraggingItem_Protected_Virtual_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static UISelectable_CashItemSlot()
	{
		Il2CppClassPointerStore<UISelectable_CashItemSlot>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "UISelectable_CashItemSlot");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UISelectable_CashItemSlot>.NativeClassPtr);
		NativeMethodInfoPtr_CanBeSelectedWhileDraggingItem_Protected_Virtual_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_CashItemSlot>.NativeClassPtr, 100686264);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_CashItemSlot>.NativeClassPtr, 100686265);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 297978, XrefRangeEnd = 297983, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool CanBeSelectedWhileDraggingItem()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_CanBeSelectedWhileDraggingItem_Protected_Virtual_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 297983, XrefRangeEnd = 297984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UISelectable_CashItemSlot()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UISelectable_CashItemSlot>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UISelectable_CashItemSlot(IntPtr pointer)
		: base(pointer)
	{
	}
}
