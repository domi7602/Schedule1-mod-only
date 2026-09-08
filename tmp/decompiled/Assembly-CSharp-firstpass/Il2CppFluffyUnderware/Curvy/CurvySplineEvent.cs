using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy;

[Serializable]
public class CurvySplineEvent : UnityEventEx<CurvySplineEventArgs>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static CurvySplineEvent()
	{
		Il2CppClassPointerStore<CurvySplineEvent>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySplineEvent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineEvent>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineEvent>.NativeClassPtr, 100664948);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 40403, RefRangeEnd = 40406, XrefRangeStart = 40397, XrefRangeEnd = 40403, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineEvent()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvySplineEvent>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvySplineEvent(IntPtr pointer)
		: base(pointer)
	{
	}
}
