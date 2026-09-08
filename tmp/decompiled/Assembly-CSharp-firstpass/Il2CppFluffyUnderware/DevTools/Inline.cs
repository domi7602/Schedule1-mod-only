using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class Inline : DTAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static Inline()
	{
		Il2CppClassPointerStore<Inline>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "Inline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Inline>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Inline>.NativeClassPtr, 100663892);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30712, XrefRangeEnd = 30713, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Inline()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Inline>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Inline(IntPtr pointer)
		: base(pointer)
	{
	}
}
