using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class FloatExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_IsBetween0And1_Public_Static_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsBetween_Public_Static_Boolean_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Repeat_Public_Static_Single_Single_Single_Single_0;

	static FloatExt()
	{
		Il2CppClassPointerStore<FloatExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "FloatExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FloatExt>.NativeClassPtr);
		NativeMethodInfoPtr_IsBetween0And1_Public_Static_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatExt>.NativeClassPtr, 100664355);
		NativeMethodInfoPtr_IsBetween_Public_Static_Boolean_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatExt>.NativeClassPtr, 100664356);
		NativeMethodInfoPtr_Repeat_Public_Static_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatExt>.NativeClassPtr, 100664357);
	}

	[CallerCount(0)]
	public unsafe static bool IsBetween0And1(this float v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsBetween0And1_Public_Static_Boolean_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool IsBetween(this float v, float a, float b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&v);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsBetween_Public_Static_Boolean_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 33798, RefRangeEnd = 33801, XrefRangeStart = 33796, XrefRangeEnd = 33798, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float Repeat(this float v, float min, float max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&v);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &min;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Repeat_Public_Static_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public FloatExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
