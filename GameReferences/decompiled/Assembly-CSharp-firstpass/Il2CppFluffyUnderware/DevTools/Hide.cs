using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class Hide : DTAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static Hide()
	{
		Il2CppClassPointerStore<Hide>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "Hide");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Hide>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Hide>.NativeClassPtr, 100663891);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30711, XrefRangeEnd = 30712, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Hide()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Hide>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Hide(IntPtr pointer)
		: base(pointer)
	{
	}
}
