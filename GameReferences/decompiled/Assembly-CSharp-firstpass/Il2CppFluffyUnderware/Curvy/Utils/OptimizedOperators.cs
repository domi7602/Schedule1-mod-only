using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Utils;

public static class OptimizedOperators : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Addition_Public_Static_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnaryNegation_Public_Static_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Subtraction_Public_Static_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Multiply_Public_Static_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Multiply_Public_Static_Vector3_Single_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Division_Public_Static_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Normalize_Public_Static_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LerpUnclamped_Public_Static_Vector3_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Multiply_Public_Static_Color_Color_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Multiply_Public_Static_Color_Single_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Multiply_Public_Static_Quaternion_Quaternion_Quaternion_0;

	static OptimizedOperators()
	{
		Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Utils", "OptimizedOperators");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr);
		NativeMethodInfoPtr_Addition_Public_Static_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665307);
		NativeMethodInfoPtr_UnaryNegation_Public_Static_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665308);
		NativeMethodInfoPtr_Subtraction_Public_Static_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665309);
		NativeMethodInfoPtr_Multiply_Public_Static_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665310);
		NativeMethodInfoPtr_Multiply_Public_Static_Vector3_Single_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665311);
		NativeMethodInfoPtr_Division_Public_Static_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665312);
		NativeMethodInfoPtr_Normalize_Public_Static_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665313);
		NativeMethodInfoPtr_LerpUnclamped_Public_Static_Vector3_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665314);
		NativeMethodInfoPtr_Multiply_Public_Static_Color_Color_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665315);
		NativeMethodInfoPtr_Multiply_Public_Static_Color_Single_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665316);
		NativeMethodInfoPtr_Multiply_Public_Static_Quaternion_Quaternion_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OptimizedOperators>.NativeClassPtr, 100665317);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 43829, RefRangeEnd = 43836, XrefRangeStart = 43829, XrefRangeEnd = 43829, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Addition(this Vector3 a, Vector3 b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Addition_Public_Static_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 UnaryNegation(this Vector3 a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&a);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnaryNegation_Public_Static_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43836, RefRangeEnd = 43837, XrefRangeStart = 43836, XrefRangeEnd = 43836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Subtraction(this Vector3 a, Vector3 b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Subtraction_Public_Static_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 43837, RefRangeEnd = 43845, XrefRangeStart = 43837, XrefRangeEnd = 43837, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Multiply(this Vector3 a, float d)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &d;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Multiply_Public_Static_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 Multiply(this float d, Vector3 a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&d);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Multiply_Public_Static_Vector3_Single_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 Division(this Vector3 a, float d)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &d;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Division_Public_Static_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43850, RefRangeEnd = 43851, XrefRangeStart = 43845, XrefRangeEnd = 43850, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Normalize(this Vector3 value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Normalize_Public_Static_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 43851, RefRangeEnd = 43857, XrefRangeStart = 43851, XrefRangeEnd = 43851, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 LerpUnclamped(this Vector3 a, Vector3 b, float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LerpUnclamped_Public_Static_Vector3_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Color Multiply(this Color a, float b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Multiply_Public_Static_Color_Color_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Color Multiply(this float b, Color a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&b);
		*(Color**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Multiply_Public_Static_Color_Single_Color_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Quaternion Multiply(this Quaternion lhs, Quaternion rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(Quaternion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Multiply_Public_Static_Quaternion_Quaternion_Quaternion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Quaternion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public OptimizedOperators(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
