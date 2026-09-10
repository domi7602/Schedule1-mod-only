using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.UI;

public class CashSlotUI : ItemSlotUI
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static CashSlotUI()
	{
		Il2CppClassPointerStore<CashSlotUI>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "CashSlotUI");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CashSlotUI>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CashSlotUI>.NativeClassPtr, 100686833);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 302698, XrefRangeEnd = 302699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CashSlotUI()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CashSlotUI>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CashSlotUI(IntPtr pointer)
		: base(pointer)
	{
	}
}
