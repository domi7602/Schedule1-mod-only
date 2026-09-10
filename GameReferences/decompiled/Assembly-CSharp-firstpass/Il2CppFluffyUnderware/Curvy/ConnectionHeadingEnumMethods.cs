using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy;

public static class ConnectionHeadingEnumMethods : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ResolveAuto_Public_Static_ConnectionHeadingEnum_ConnectionHeadingEnum_CurvySplineSegment_0;

	static ConnectionHeadingEnumMethods()
	{
		Il2CppClassPointerStore<ConnectionHeadingEnumMethods>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "ConnectionHeadingEnumMethods");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ConnectionHeadingEnumMethods>.NativeClassPtr);
		NativeMethodInfoPtr_ResolveAuto_Public_Static_ConnectionHeadingEnum_ConnectionHeadingEnum_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConnectionHeadingEnumMethods>.NativeClassPtr, 100664580);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 36184, RefRangeEnd = 36185, XrefRangeStart = 36177, XrefRangeEnd = 36184, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ConnectionHeadingEnum ResolveAuto(this ConnectionHeadingEnum heading, CurvySplineSegment followUp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&heading);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)followUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResolveAuto_Public_Static_ConnectionHeadingEnum_ConnectionHeadingEnum_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ConnectionHeadingEnum*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ConnectionHeadingEnumMethods(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
