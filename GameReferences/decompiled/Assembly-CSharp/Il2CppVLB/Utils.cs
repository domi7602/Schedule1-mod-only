using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppVLB;

public static class Utils : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "FloatPackingPrecision")]
	public enum FloatPackingPrecision
	{
		High = 64,
		Low = 8,
		Undef = 0
	}

	private sealed class MethodInfoStoreGeneric_Swap_Public_Static_Void_byref_T_byref_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Swap_Public_Static_Void_byref_T_byref_T_0, Il2CppClassPointerStore<Utils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_NewWithComponent_Public_Static_T_String_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_NewWithComponent_Public_Static_T_String_0, Il2CppClassPointerStore<Utils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetOrAddComponent_Public_Static_T_GameObject_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetOrAddComponent_Public_Static_T_GameObject_0, Il2CppClassPointerStore<Utils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetOrAddComponent_Public_Static_T_MonoBehaviour_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetOrAddComponent_Public_Static_T_MonoBehaviour_0, Il2CppClassPointerStore<Utils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ForeachComponentsInAnyChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ForeachComponentsInAnyChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0, Il2CppClassPointerStore<Utils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ForeachComponentsInDirectChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ForeachComponentsInDirectChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0, Il2CppClassPointerStore<Utils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_kEpsilon;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_FloatPackingPrecision;

	private static readonly System.IntPtr NativeFieldInfoPtr_kFloatPackingHighMinShaderLevel;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeConeRadiusEnd_Public_Static_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeSpotAngle_Public_Static_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Swap_Public_Static_Void_byref_T_byref_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPath_Public_Static_String_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewWithComponent_Public_Static_T_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrAddComponent_Public_Static_T_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrAddComponent_Public_Static_T_MonoBehaviour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ForeachComponentsInAnyChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ForeachComponentsInDirectChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupDepthCamera_Public_Static_Void_Camera_Single_Single_Single_Single_Vector3_Vector3_Boolean_Quaternion_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_Enum_Enum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Divide_Public_Static_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_xy_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_xz_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_yz_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_yx_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_zx_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_zy_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector2_Vector2_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector4_Vector4_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AsVector4_Public_Static_Vector4_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PlaneEquation_Public_Static_Vector4_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetVolumeCubic_Public_Static_Single_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaxArea2D_Public_Static_Single_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Opaque_Public_Static_Color_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeComplementaryColor_Public_Static_Color_Color_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TranslateCustom_Public_Static_Plane_Plane_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClosestPointOnPlaneCustom_Public_Static_Vector3_Plane_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsAlmostZero_Public_Static_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsValid_Public_Static_Boolean_Plane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetKeywordEnabled_Public_Static_Void_Material_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetShaderKeywordEnabled_Public_Static_Void_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SampleInMatrix_Public_Static_Matrix4x4_Gradient_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SampleInArray_Public_Static_Il2CppStructArray_1_Color_Gradient_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Vector4_Floor_Private_Static_Vector4_Vector4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PackToFloat_Public_Static_Single_Color_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFloatPackingPrecision_Public_Static_FloatPackingPrecision_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasAtLeastOneFlag_Public_Static_Boolean_Enum_Enum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MarkCurrentSceneDirty_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MarkObjectDirty_Public_Static_Void_Object_0;

	public unsafe static float kEpsilon
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kEpsilon, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kEpsilon, (void*)(&num));
		}
	}

	public unsafe static FloatPackingPrecision ms_FloatPackingPrecision
	{
		get
		{
			Unsafe.SkipInit(out FloatPackingPrecision result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_FloatPackingPrecision, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_FloatPackingPrecision, (void*)(&floatPackingPrecision));
		}
	}

	public unsafe static int kFloatPackingHighMinShaderLevel
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kFloatPackingHighMinShaderLevel, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kFloatPackingHighMinShaderLevel, (void*)(&num));
		}
	}

	static Utils()
	{
		Il2CppClassPointerStore<Utils>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "Utils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Utils>.NativeClassPtr);
		NativeFieldInfoPtr_kEpsilon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Utils>.NativeClassPtr, "kEpsilon");
		NativeFieldInfoPtr_ms_FloatPackingPrecision = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Utils>.NativeClassPtr, "ms_FloatPackingPrecision");
		NativeFieldInfoPtr_kFloatPackingHighMinShaderLevel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Utils>.NativeClassPtr, "kFloatPackingHighMinShaderLevel");
		NativeMethodInfoPtr_ComputeConeRadiusEnd_Public_Static_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664379);
		NativeMethodInfoPtr_ComputeSpotAngle_Public_Static_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664380);
		NativeMethodInfoPtr_Swap_Public_Static_Void_byref_T_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664381);
		NativeMethodInfoPtr_GetPath_Public_Static_String_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664382);
		NativeMethodInfoPtr_NewWithComponent_Public_Static_T_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664383);
		NativeMethodInfoPtr_GetOrAddComponent_Public_Static_T_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664384);
		NativeMethodInfoPtr_GetOrAddComponent_Public_Static_T_MonoBehaviour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664385);
		NativeMethodInfoPtr_ForeachComponentsInAnyChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664386);
		NativeMethodInfoPtr_ForeachComponentsInDirectChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664387);
		NativeMethodInfoPtr_SetupDepthCamera_Public_Static_Void_Camera_Single_Single_Single_Single_Vector3_Vector3_Boolean_Quaternion_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664388);
		NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_Enum_Enum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664389);
		NativeMethodInfoPtr_Divide_Public_Static_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664390);
		NativeMethodInfoPtr_xy_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664391);
		NativeMethodInfoPtr_xz_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664392);
		NativeMethodInfoPtr_yz_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664393);
		NativeMethodInfoPtr_yx_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664394);
		NativeMethodInfoPtr_zx_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664395);
		NativeMethodInfoPtr_zy_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664396);
		NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664397);
		NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector2_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664398);
		NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664399);
		NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector4_Vector4_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664400);
		NativeMethodInfoPtr_AsVector4_Public_Static_Vector4_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664401);
		NativeMethodInfoPtr_PlaneEquation_Public_Static_Vector4_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664402);
		NativeMethodInfoPtr_GetVolumeCubic_Public_Static_Single_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664403);
		NativeMethodInfoPtr_GetMaxArea2D_Public_Static_Single_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664404);
		NativeMethodInfoPtr_Opaque_Public_Static_Color_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664405);
		NativeMethodInfoPtr_ComputeComplementaryColor_Public_Static_Color_Color_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664406);
		NativeMethodInfoPtr_TranslateCustom_Public_Static_Plane_Plane_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664407);
		NativeMethodInfoPtr_ClosestPointOnPlaneCustom_Public_Static_Vector3_Plane_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664408);
		NativeMethodInfoPtr_IsAlmostZero_Public_Static_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664409);
		NativeMethodInfoPtr_IsValid_Public_Static_Boolean_Plane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664410);
		NativeMethodInfoPtr_SetKeywordEnabled_Public_Static_Void_Material_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664411);
		NativeMethodInfoPtr_SetShaderKeywordEnabled_Public_Static_Void_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664412);
		NativeMethodInfoPtr_SampleInMatrix_Public_Static_Matrix4x4_Gradient_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664413);
		NativeMethodInfoPtr_SampleInArray_Public_Static_Il2CppStructArray_1_Color_Gradient_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664414);
		NativeMethodInfoPtr_Vector4_Floor_Private_Static_Vector4_Vector4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664415);
		NativeMethodInfoPtr_PackToFloat_Public_Static_Single_Color_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664416);
		NativeMethodInfoPtr_GetFloatPackingPrecision_Public_Static_FloatPackingPrecision_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664417);
		NativeMethodInfoPtr_HasAtLeastOneFlag_Public_Static_Boolean_Enum_Enum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664418);
		NativeMethodInfoPtr_MarkCurrentSceneDirty_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664419);
		NativeMethodInfoPtr_MarkObjectDirty_Public_Static_Void_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Utils>.NativeClassPtr, 100664420);
	}

	[CallerCount(19)]
	[CachedScanResults(RefRangeStart = 73950, RefRangeEnd = 73969, XrefRangeStart = 73949, XrefRangeEnd = 73950, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ComputeConeRadiusEnd(float fallOffEnd, float spotAngle)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&fallOffEnd);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &spotAngle;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeConeRadiusEnd_Public_Static_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73970, RefRangeEnd = 73972, XrefRangeStart = 73969, XrefRangeEnd = 73970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ComputeSpotAngle(float fallOffEnd, float coneRadiusEnd)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&fallOffEnd);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coneRadiusEnd;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeSpotAngle_Public_Static_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73972, XrefRangeEnd = 73988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Swap<T>(ref T a, ref T b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)Unsafe.As<T, object>(ref a));
		*ptr = (nint)(&intPtr);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)Unsafe.As<T, object>(ref b));
		*(System.IntPtr**)num = &intPtr2;
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_Swap_Public_Static_Void_byref_T_byref_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		System.IntPtr intPtr5 = intPtr;
		Unsafe.As<T, object>(ref a) = ((intPtr5 == (System.IntPtr)0) ? ((T)null) : IL2CPP.PointerToValueGeneric<T>(intPtr5, false, false));
		System.IntPtr intPtr6 = intPtr2;
		Unsafe.As<T, object>(ref b) = ((intPtr6 == (System.IntPtr)0) ? ((T)null) : IL2CPP.PointerToValueGeneric<T>(intPtr6, false, false));
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 74003, RefRangeEnd = 74006, XrefRangeStart = 73988, XrefRangeEnd = 74003, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetPath(Transform current)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)current);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPath_Public_Static_String_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 74025, RefRangeEnd = 74029, XrefRangeStart = 74006, XrefRangeEnd = 74025, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T NewWithComponent<T>(string name) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_NewWithComponent_Public_Static_T_String_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 74036, RefRangeEnd = 74041, XrefRangeStart = 74029, XrefRangeEnd = 74036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetOrAddComponent<T>(this GameObject self) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetOrAddComponent_Public_Static_T_GameObject_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74041, XrefRangeEnd = 74050, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetOrAddComponent<T>(this MonoBehaviour self) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetOrAddComponent_Public_Static_T_MonoBehaviour_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74050, XrefRangeEnd = 74058, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ForeachComponentsInAnyChildrenOnly<T>(this GameObject self, Il2CppSystem.Action<T> lambda, bool includeInactive = false) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lambda);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInactive;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ForeachComponentsInAnyChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 74068, RefRangeEnd = 74071, XrefRangeStart = 74058, XrefRangeEnd = 74068, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ForeachComponentsInDirectChildrenOnly<T>(this GameObject self, Il2CppSystem.Action<T> lambda, bool includeInactive = false) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lambda);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInactive;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ForeachComponentsInDirectChildrenOnly_Public_Static_Void_GameObject_Action_1_T_Boolean_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 74085, RefRangeEnd = 74087, XrefRangeStart = 74071, XrefRangeEnd = 74085, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupDepthCamera(Camera depthCamera, float coneApexOffsetZ, float maxGeometryDistance, float coneRadiusStart, float coneRadiusEnd, Vector3 beamLocalForward, Vector3 lossyScale, bool isScalable, Quaternion beamInternalLocalRotation, bool shouldScaleMinNearClipPlane)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[10];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)depthCamera);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coneApexOffsetZ;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxGeometryDistance;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &coneRadiusStart;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &coneRadiusEnd;
		*(Vector3**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &beamLocalForward;
		*(Vector3**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &lossyScale;
		*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &isScalable;
		*(Quaternion**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &beamInternalLocalRotation;
		*(bool**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &shouldScaleMinNearClipPlane;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupDepthCamera_Public_Static_Void_Camera_Single_Single_Single_Single_Vector3_Vector3_Boolean_Quaternion_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74087, XrefRangeEnd = 74094, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool HasFlag(this Il2CppSystem.Enum mask, Il2CppSystem.Enum flags)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mask);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)flags);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_Enum_Enum_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74099, RefRangeEnd = 74100, XrefRangeStart = 74094, XrefRangeEnd = 74099, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Divide(this Vector3 aVector, Vector3 scale)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&aVector);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Divide_Public_Static_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(155)]
	[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 xy(this Vector3 aVector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&aVector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_xy_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 74100, RefRangeEnd = 74109, XrefRangeStart = 74100, XrefRangeEnd = 74100, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 xz(this Vector3 aVector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&aVector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_xz_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 yz(this Vector3 aVector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&aVector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_yz_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 yx(this Vector3 aVector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&aVector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_yx_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 zx(this Vector3 aVector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&aVector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_zx_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 zy(this Vector3 aVector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&aVector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_zy_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 74109, RefRangeEnd = 74116, XrefRangeStart = 74109, XrefRangeEnd = 74109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Approximately(this float a, float b, float epsilon = 1E-05f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &epsilon;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74116, RefRangeEnd = 74117, XrefRangeStart = 74116, XrefRangeEnd = 74116, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Approximately(this Vector2 a, Vector2 b, float epsilon = 1E-05f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &epsilon;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector2_Vector2_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool Approximately(this Vector3 a, Vector3 b, float epsilon = 1E-05f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &epsilon;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool Approximately(this Vector4 a, Vector4 b, float epsilon = 1E-05f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(Vector4**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &epsilon;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector4_Vector4_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 74117, RefRangeEnd = 74121, XrefRangeStart = 74117, XrefRangeEnd = 74117, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector4 AsVector4(this Vector3 vec3, float w)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&vec3);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &w;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AsVector4_Public_Static_Vector4_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector4*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74121, RefRangeEnd = 74122, XrefRangeStart = 74121, XrefRangeEnd = 74121, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector4 PlaneEquation(Vector3 normalizedNormal, Vector3 pt)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&normalizedNormal);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pt;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PlaneEquation_Public_Static_Vector4_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector4*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float GetVolumeCubic(this Bounds self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVolumeCubic_Public_Static_Single_Bounds_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 74122, RefRangeEnd = 74124, XrefRangeStart = 74122, XrefRangeEnd = 74122, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetMaxArea2D(this Bounds self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaxArea2D_Public_Static_Single_Bounds_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Color Opaque(this Color self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Opaque_Public_Static_Color_Color_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Color ComputeComplementaryColor(this Color self, bool blackAndWhite)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&self);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &blackAndWhite;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeComplementaryColor_Public_Static_Color_Color_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74126, RefRangeEnd = 74127, XrefRangeStart = 74124, XrefRangeEnd = 74126, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Plane TranslateCustom(this Plane plane, Vector3 translation)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&plane);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &translation;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TranslateCustom_Public_Static_Plane_Plane_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Plane*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 ClosestPointOnPlaneCustom(this Plane plane, Vector3 point)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&plane);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &point;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClosestPointOnPlaneCustom_Public_Static_Vector3_Plane_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74127, RefRangeEnd = 74128, XrefRangeStart = 74127, XrefRangeEnd = 74127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsAlmostZero(float f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&f);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsAlmostZero_Public_Static_Boolean_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74128, RefRangeEnd = 74129, XrefRangeStart = 74128, XrefRangeEnd = 74128, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsValid(this Plane plane)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&plane);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsValid_Public_Static_Boolean_Plane_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(54)]
	[CachedScanResults(RefRangeStart = 74132, RefRangeEnd = 74186, XrefRangeStart = 74129, XrefRangeEnd = 74132, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetKeywordEnabled(this Material mat, string name, bool enabled)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &enabled;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetKeywordEnabled_Public_Static_Void_Material_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74186, XrefRangeEnd = 74188, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetShaderKeywordEnabled(string name, bool enabled)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &enabled;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetShaderKeywordEnabled_Public_Static_Void_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 74195, RefRangeEnd = 74197, XrefRangeStart = 74188, XrefRangeEnd = 74195, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix4x4 SampleInMatrix(this Gradient self, int floatPackingPrecision)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &floatPackingPrecision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SampleInMatrix_Public_Static_Matrix4x4_Gradient_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix4x4*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74197, XrefRangeEnd = 74202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<Color> SampleInArray(this Gradient self, int samplesCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &samplesCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SampleInArray_Public_Static_Il2CppStructArray_1_Color_Gradient_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Color>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74202, XrefRangeEnd = 74206, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector4 Vector4_Floor(Vector4 vec)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&vec);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Vector4_Floor_Private_Static_Vector4_Vector4_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector4*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74206, XrefRangeEnd = 74210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float PackToFloat(this Color color, int floatPackingPrecision)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&color);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &floatPackingPrecision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PackToFloat_Public_Static_Single_Color_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 74215, RefRangeEnd = 74220, XrefRangeStart = 74210, XrefRangeEnd = 74215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static FloatPackingPrecision GetFloatPackingPrecision()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFloatPackingPrecision_Public_Static_FloatPackingPrecision_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatPackingPrecision*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 74225, RefRangeEnd = 74227, XrefRangeStart = 74220, XrefRangeEnd = 74225, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool HasAtLeastOneFlag(this Il2CppSystem.Enum mask, Il2CppSystem.Enum flags)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mask);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)flags);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasAtLeastOneFlag_Public_Static_Boolean_Enum_Enum_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MarkCurrentSceneDirty()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MarkCurrentSceneDirty_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MarkObjectDirty(UnityEngine.Object obj)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MarkObjectDirty_Public_Static_Void_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Utils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
