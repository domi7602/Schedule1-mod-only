using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyShape2D : CurvyShape
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static CurvyShape2D()
	{
		Il2CppClassPointerStore<CurvyShape2D>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyShape2D");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyShape2D>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape2D>.NativeClassPtr, 100664578);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 36169, RefRangeEnd = 36175, XrefRangeStart = 36165, XrefRangeEnd = 36169, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyShape2D()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyShape2D>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyShape2D(IntPtr pointer)
		: base(pointer)
	{
	}
}
