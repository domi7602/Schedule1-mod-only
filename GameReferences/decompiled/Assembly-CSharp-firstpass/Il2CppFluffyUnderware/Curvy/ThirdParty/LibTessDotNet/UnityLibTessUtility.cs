using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public static class UnityLibTessUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ToContourVertex_Public_Static_Il2CppReferenceArray_1_ContourVertex_Il2CppStructArray_1_Vector3_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToContourVertex_Public_Static_Il2CppReferenceArray_1_ContourVertex_SubArray_1_Vector3_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FromContourVertex_Public_Static_Void_Il2CppReferenceArray_1_ContourVertex_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ContourVerticesToPositions_Public_Static_SubArray_1_Vector3_Il2CppReferenceArray_1_ContourVertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetFromContourVertex_Public_Static_Void_byref_Il2CppStructArray_1_Vector3_byref_Il2CppReferenceArray_1_ContourVertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetToContourVertex_Public_Static_Void_byref_Il2CppReferenceArray_1_ContourVertex_byref_Il2CppStructArray_1_Vector3_0;

	static UnityLibTessUtility()
	{
		Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "UnityLibTessUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr);
		NativeMethodInfoPtr_ToContourVertex_Public_Static_Il2CppReferenceArray_1_ContourVertex_Il2CppStructArray_1_Vector3_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr, 100667247);
		NativeMethodInfoPtr_ToContourVertex_Public_Static_Il2CppReferenceArray_1_ContourVertex_SubArray_1_Vector3_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr, 100667248);
		NativeMethodInfoPtr_FromContourVertex_Public_Static_Void_Il2CppReferenceArray_1_ContourVertex_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr, 100667249);
		NativeMethodInfoPtr_ContourVerticesToPositions_Public_Static_SubArray_1_Vector3_Il2CppReferenceArray_1_ContourVertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr, 100667250);
		NativeMethodInfoPtr_SetFromContourVertex_Public_Static_Void_byref_Il2CppStructArray_1_Vector3_byref_Il2CppReferenceArray_1_ContourVertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr, 100667251);
		NativeMethodInfoPtr_SetToContourVertex_Public_Static_Void_byref_Il2CppReferenceArray_1_ContourVertex_byref_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityLibTessUtility>.NativeClassPtr, 100667252);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60715, XrefRangeEnd = 60720, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<ContourVertex> ToContourVertex(Il2CppStructArray<Vector3> v, bool zeroZ = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &zeroZ;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToContourVertex_Public_Static_Il2CppReferenceArray_1_ContourVertex_Il2CppStructArray_1_Vector3_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ContourVertex>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60724, RefRangeEnd = 60725, XrefRangeStart = 60720, XrefRangeEnd = 60724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<ContourVertex> ToContourVertex(SubArray<Vector3> v, bool zeroZ = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)v));
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &zeroZ;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToContourVertex_Public_Static_Il2CppReferenceArray_1_ContourVertex_SubArray_1_Vector3_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ContourVertex>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60725, RefRangeEnd = 60726, XrefRangeStart = 60725, XrefRangeEnd = 60725, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FromContourVertex(Il2CppReferenceArray<ContourVertex> v, SubArray<Vector3> output)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)output));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FromContourVertex_Public_Static_Void_Il2CppReferenceArray_1_ContourVertex_SubArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60726, XrefRangeEnd = 60735, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SubArray<Vector3> ContourVerticesToPositions(Il2CppReferenceArray<ContourVertex> v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ContourVerticesToPositions_Public_Static_SubArray_1_Vector3_Il2CppReferenceArray_1_ContourVertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60735, XrefRangeEnd = 60739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetFromContourVertex(ref Il2CppStructArray<Vector3> v3Array, ref Il2CppReferenceArray<ContourVertex> cvArray)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v3Array);
		*ptr = (nint)(&intPtr);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cvArray);
		*(System.IntPtr**)num = &intPtr2;
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFromContourVertex_Public_Static_Void_byref_Il2CppStructArray_1_Vector3_byref_Il2CppReferenceArray_1_ContourVertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		System.IntPtr intPtr5 = intPtr;
		v3Array = ((intPtr5 == (System.IntPtr)0) ? null : new Il2CppStructArray<Vector3>(intPtr5));
		System.IntPtr intPtr6 = intPtr2;
		cvArray = ((intPtr6 == (System.IntPtr)0) ? null : new Il2CppReferenceArray<ContourVertex>(intPtr6));
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60739, XrefRangeEnd = 60743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetToContourVertex(ref Il2CppReferenceArray<ContourVertex> cvArray, ref Il2CppStructArray<Vector3> v3Array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cvArray);
		*ptr = (nint)(&intPtr);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v3Array);
		*(System.IntPtr**)num = &intPtr2;
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetToContourVertex_Public_Static_Void_byref_Il2CppReferenceArray_1_ContourVertex_byref_Il2CppStructArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		System.IntPtr intPtr5 = intPtr;
		cvArray = ((intPtr5 == (System.IntPtr)0) ? null : new Il2CppReferenceArray<ContourVertex>(intPtr5));
		System.IntPtr intPtr6 = intPtr2;
		v3Array = ((intPtr6 == (System.IntPtr)0) ? null : new Il2CppStructArray<Vector3>(intPtr6));
	}

	public UnityLibTessUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
