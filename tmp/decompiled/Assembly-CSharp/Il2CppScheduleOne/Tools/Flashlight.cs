using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Equipping;

namespace Il2CppScheduleOne.Tools;

public class Flashlight : Equippable_Viewmodel
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static Flashlight()
	{
		Il2CppClassPointerStore<Flashlight>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "Flashlight");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Flashlight>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Flashlight>.NativeClassPtr, 100677837);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Flashlight()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Flashlight>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Flashlight(IntPtr pointer)
		: base(pointer)
	{
	}
}
