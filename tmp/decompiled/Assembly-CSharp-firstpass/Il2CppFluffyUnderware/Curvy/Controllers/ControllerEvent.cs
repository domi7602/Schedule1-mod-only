using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

[Serializable]
public class ControllerEvent : UnityEventEx<CurvyController>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static ControllerEvent()
	{
		Il2CppClassPointerStore<ControllerEvent>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "ControllerEvent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ControllerEvent>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControllerEvent>.NativeClassPtr, 100665353);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44513, XrefRangeEnd = 44516, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ControllerEvent()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ControllerEvent>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ControllerEvent(IntPtr pointer)
		: base(pointer)
	{
	}
}
