using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class AnimationCurveExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ValueIsOne_Public_Static_Boolean_AnimationCurve_0;

	static AnimationCurveExt()
	{
		Il2CppClassPointerStore<AnimationCurveExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "AnimationCurveExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationCurveExt>.NativeClassPtr);
		NativeMethodInfoPtr_ValueIsOne_Public_Static_Boolean_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationCurveExt>.NativeClassPtr, 100664351);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 33759, RefRangeEnd = 33762, XrefRangeStart = 33751, XrefRangeEnd = 33759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool ValueIsOne(this AnimationCurve curve)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curve);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ValueIsOne_Public_Static_Boolean_AnimationCurve_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public AnimationCurveExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
