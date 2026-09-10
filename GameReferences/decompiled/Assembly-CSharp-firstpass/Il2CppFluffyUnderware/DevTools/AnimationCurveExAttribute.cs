using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class AnimationCurveExAttribute : DTPropertyAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_0;

	static AnimationCurveExAttribute()
	{
		Il2CppClassPointerStore<AnimationCurveExAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "AnimationCurveExAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationCurveExAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationCurveExAttribute>.NativeClassPtr, 100664163);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32237, XrefRangeEnd = 32240, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AnimationCurveExAttribute(string label = "", string tooltip = "")
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationCurveExAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(label);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AnimationCurveExAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
