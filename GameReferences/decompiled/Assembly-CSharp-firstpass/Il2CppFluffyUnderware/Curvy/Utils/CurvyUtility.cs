using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Utils;

public static class CurvyUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ClampTF_Public_Static_Single_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampTF_Public_Static_Single_Single_byref_Int32_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampValue_Public_Static_Single_Single_CurvyClamping_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_CurvyClamping_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_CurvyClamping_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_byref_Int32_CurvyClamping_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_byref_Int32_CurvyClamping_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDefaultMaterial_Public_Static_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolationSearch_Public_Static_Int32_Il2CppStructArray_1_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolationSearch_Public_Static_Int32_Il2CppStructArray_1_Single_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SplineToMesh_Public_Static_Mesh_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointIndex_Public_Static_Void_Vector3_Il2CppStructArray_1_Vector3_Int32_byref_Int32_byref_Single_0;

	static CurvyUtility()
	{
		Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Utils", "CurvyUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr);
		NativeMethodInfoPtr_ClampTF_Public_Static_Single_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665294);
		NativeMethodInfoPtr_ClampTF_Public_Static_Single_Single_byref_Int32_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665295);
		NativeMethodInfoPtr_ClampValue_Public_Static_Single_Single_CurvyClamping_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665296);
		NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_CurvyClamping_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665297);
		NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_CurvyClamping_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665298);
		NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_byref_Int32_CurvyClamping_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665299);
		NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_byref_Int32_CurvyClamping_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665300);
		NativeMethodInfoPtr_GetDefaultMaterial_Public_Static_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665301);
		NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665302);
		NativeMethodInfoPtr_InterpolationSearch_Public_Static_Int32_Il2CppStructArray_1_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665303);
		NativeMethodInfoPtr_InterpolationSearch_Public_Static_Int32_Il2CppStructArray_1_Single_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665304);
		NativeMethodInfoPtr_SplineToMesh_Public_Static_Mesh_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665305);
		NativeMethodInfoPtr_GetNearestPointIndex_Public_Static_Void_Vector3_Il2CppStructArray_1_Vector3_Int32_byref_Int32_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyUtility>.NativeClassPtr, 100665306);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 43670, RefRangeEnd = 43677, XrefRangeStart = 43668, XrefRangeEnd = 43670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampTF(float tf, CurvyClamping clamping)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampTF_Public_Static_Single_Single_CurvyClamping_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 43684, RefRangeEnd = 43686, XrefRangeStart = 43677, XrefRangeEnd = 43684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampTF(float tf, ref int dir, CurvyClamping clamping)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dir);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampTF_Public_Static_Single_Single_byref_Int32_CurvyClamping_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 43690, RefRangeEnd = 43697, XrefRangeStart = 43686, XrefRangeEnd = 43690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampValue(float tf, CurvyClamping clamping, float minTF, float maxTF)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&tf);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &minTF;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxTF;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampValue_Public_Static_Single_Single_CurvyClamping_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 43697, RefRangeEnd = 43705, XrefRangeStart = 43697, XrefRangeEnd = 43697, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampDistance(float distance, CurvyClamping clamping, float length)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&distance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_CurvyClamping_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43715, RefRangeEnd = 43716, XrefRangeStart = 43705, XrefRangeEnd = 43715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampDistance(float distance, CurvyClamping clamping, float length, float min, float max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&distance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &min;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_CurvyClamping_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 43716, RefRangeEnd = 43718, XrefRangeStart = 43716, XrefRangeEnd = 43716, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampDistance(float distance, ref int dir, CurvyClamping clamping, float length)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&distance);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dir);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_byref_Int32_CurvyClamping_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43729, RefRangeEnd = 43730, XrefRangeStart = 43718, XrefRangeEnd = 43729, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ClampDistance(float distance, ref int dir, CurvyClamping clamping, float length, float min, float max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&distance);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dir);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &min;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Static_Single_Single_byref_Int32_CurvyClamping_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 43754, RefRangeEnd = 43762, XrefRangeStart = 43730, XrefRangeEnd = 43754, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material GetDefaultMaterial()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDefaultMaterial_Public_Static_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 43771, RefRangeEnd = 43782, XrefRangeStart = 43762, XrefRangeEnd = 43771, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Approximately(this float x, float y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&x);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43782, XrefRangeEnd = 43784, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int InterpolationSearch(Il2CppStructArray<float> array, float x)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolationSearch_Public_Static_Int32_Il2CppStructArray_1_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 43785, RefRangeEnd = 43789, XrefRangeStart = 43784, XrefRangeEnd = 43785, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int InterpolationSearch(Il2CppStructArray<float> array, int elementsCount, float x)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementsCount;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolationSearch_Public_Static_Int32_Il2CppStructArray_1_Single_Int32_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43789, XrefRangeEnd = 43823, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh SplineToMesh(this CurvySpline spline)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SplineToMesh_Public_Static_Mesh_CurvySpline_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 43827, RefRangeEnd = 43829, XrefRangeStart = 43823, XrefRangeEnd = 43827, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GetNearestPointIndex(Vector3 point, Il2CppStructArray<Vector3> points, int pointsCount, out int index, out float fragement)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&point);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)points);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointsCount;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref index);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref fragement);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointIndex_Public_Static_Void_Vector3_Il2CppStructArray_1_Vector3_Int32_byref_Int32_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
