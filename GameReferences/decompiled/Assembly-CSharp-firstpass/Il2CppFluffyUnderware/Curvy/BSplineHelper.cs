using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public static class BSplineHelper : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_DeBoorClamped_Public_Static_Vector3_Int32_Int32_Single_Int32_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeBoorUnclamped_Public_Static_Vector3_Int32_Int32_Single_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBSplineN_Public_Static_Int32_Int32_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBSplineUAndK_Public_Static_Void_Single_Boolean_Int32_Int32_byref_Single_byref_Int32_0;

	static BSplineHelper()
	{
		Il2CppClassPointerStore<BSplineHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "BSplineHelper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BSplineHelper>.NativeClassPtr);
		NativeMethodInfoPtr_DeBoorClamped_Public_Static_Vector3_Int32_Int32_Single_Int32_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineHelper>.NativeClassPtr, 100664472);
		NativeMethodInfoPtr_DeBoorUnclamped_Public_Static_Vector3_Int32_Int32_Single_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineHelper>.NativeClassPtr, 100664473);
		NativeMethodInfoPtr_GetBSplineN_Public_Static_Int32_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineHelper>.NativeClassPtr, 100664474);
		NativeMethodInfoPtr_GetBSplineUAndK_Public_Static_Void_Single_Boolean_Int32_Int32_byref_Single_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineHelper>.NativeClassPtr, 100664475);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 34772, RefRangeEnd = 34776, XrefRangeStart = 34766, XrefRangeEnd = 34772, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 DeBoorClamped(int p, int k, float u, int nPlus1, Il2CppStructArray<Vector3> pArray)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&p);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &k;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &u;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &nPlus1;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pArray);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeBoorClamped_Public_Static_Vector3_Int32_Int32_Single_Int32_Il2CppStructArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 34779, RefRangeEnd = 34783, XrefRangeStart = 34776, XrefRangeEnd = 34779, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 DeBoorUnclamped(int p, int k, float u, Il2CppStructArray<Vector3> pArray)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&p);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &k;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &u;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pArray);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeBoorUnclamped_Public_Static_Vector3_Int32_Int32_Single_Il2CppStructArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static int GetBSplineN(int controlPointsCount, int degree, bool closed)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&controlPointsCount);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &degree;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &closed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBSplineN_Public_Static_Int32_Int32_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static void GetBSplineUAndK(float tf, bool isClamped, int p, int n, out float u, out int k)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&tf);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isClamped;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &n;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref u);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref k);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBSplineUAndK_Public_Static_Void_Single_Boolean_Int32_Int32_byref_Single_byref_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BSplineHelper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
