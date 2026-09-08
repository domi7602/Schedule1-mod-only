using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public static class CGUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateUV2_Public_Static_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateUV2_Public_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetControlPointsWithOptions_Public_Static_List_1_ControlPointOption_CGDataRequestMetaCGOptions_CurvySpline_Single_Single_Boolean_byref_Int32_byref_Single_0;

	static CGUtility()
	{
		Il2CppClassPointerStore<CGUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGUtility>.NativeClassPtr);
		NativeMethodInfoPtr_CalculateUV2_Public_Static_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGUtility>.NativeClassPtr, 100666198);
		NativeMethodInfoPtr_CalculateUV2_Public_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGUtility>.NativeClassPtr, 100666199);
		NativeMethodInfoPtr_GetControlPointsWithOptions_Public_Static_List_1_ControlPointOption_CGDataRequestMetaCGOptions_CurvySpline_Single_Single_Boolean_byref_Int32_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGUtility>.NativeClassPtr, 100666200);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51228, XrefRangeEnd = 51231, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<Vector2> CalculateUV2(Il2CppStructArray<Vector2> uv)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uv);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateUV2_Public_Static_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe static void CalculateUV2(Il2CppStructArray<Vector2> uv, Il2CppStructArray<Vector2> uv2, int elementsNumber)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uv);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uv2);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementsNumber;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateUV2_Public_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 51296, RefRangeEnd = 51297, XrefRangeStart = 51231, XrefRangeEnd = 51296, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<ControlPointOption> GetControlPointsWithOptions(CGDataRequestMetaCGOptions options, CurvySpline shape, float startDist, float endDist, bool optimize, out int initialMaterialID, out float initialMaxStep)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)options);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shape);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startDist;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endDist;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &optimize;
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref initialMaterialID);
		*(void**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref initialMaxStep);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetControlPointsWithOptions_Public_Static_List_1_ControlPointOption_CGDataRequestMetaCGOptions_CurvySpline_Single_Single_Boolean_byref_Int32_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ControlPointOption>>(intPtr) : null;
	}

	public CGUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
