using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppFluffyUnderware.Curvy.Pools;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyGlobalManager : DTSingleton<CurvyGlobalManager>
{
	private static readonly IntPtr NativeFieldInfoPtr_DefaultDefaultGizmoColor;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultDefaultGizmoSelectionColor;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultGizmoOrientationColor;

	private static readonly IntPtr NativeFieldInfoPtr_HideManager;

	private static readonly IntPtr NativeFieldInfoPtr_SaveGeneratorOutputs;

	private static readonly IntPtr NativeFieldInfoPtr_SceneViewResolution;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultGizmoColor;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultGizmoSelectionColor;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultInterpolation;

	private static readonly IntPtr NativeFieldInfoPtr_GizmoControlPointSize;

	private static readonly IntPtr NativeFieldInfoPtr_GizmoOrientationLength;

	private static readonly IntPtr NativeFieldInfoPtr_GizmoOrientationColor;

	private static readonly IntPtr NativeFieldInfoPtr_SplineLayer;

	private static readonly IntPtr NativeFieldInfoPtr_Gizmos;

	private static readonly IntPtr NativeFieldInfoPtr_poolManager;

	private static readonly IntPtr NativeFieldInfoPtr_controlPointPool;

	private static readonly IntPtr NativeFieldInfoPtr_arrayPoolsSettings;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowCurveGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowCurveGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowConnectionsGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowConnectionsGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowApproximationGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowApproximationGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowTangentsGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowTangentsGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowOrientationGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowOrientationGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowTFsGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowTFsGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowRelativeDistancesGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowRelativeDistancesGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowLabelsGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowLabelsGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowMetadataGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowMetadataGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowBoundsGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowBoundsGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ShowOrientationAnchorsGizmo_Public_Static_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ShowOrientationAnchorsGizmo_Public_Static_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PoolManager_Public_get_PoolManager_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ControlPointPool_Public_get_ComponentPool_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ArrayPoolsSettings_Public_get_ArrayPoolsSettings_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Connections_Public_get_Il2CppReferenceArray_1_CurvyConnection_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetContainingConnections_Public_Il2CppReferenceArray_1_CurvyConnection_Il2CppReferenceArray_1_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_InitializeOnLoad_Private_Static_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LoadRuntimeSettings_Private_Static_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SaveRuntimeSettings_Public_Static_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_MergeDoubleLoaded_Public_Virtual_Void_IDTSingleton_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Color DefaultDefaultGizmoColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultDefaultGizmoColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultDefaultGizmoColor, (void*)(&color));
		}
	}

	public unsafe static Color DefaultDefaultGizmoSelectionColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultDefaultGizmoSelectionColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultDefaultGizmoSelectionColor, (void*)(&color));
		}
	}

	public unsafe static Color DefaultGizmoOrientationColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultGizmoOrientationColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultGizmoOrientationColor, (void*)(&color));
		}
	}

	public unsafe static bool HideManager
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HideManager, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HideManager, (void*)(&flag));
		}
	}

	public unsafe static bool SaveGeneratorOutputs
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SaveGeneratorOutputs, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SaveGeneratorOutputs, (void*)(&flag));
		}
	}

	public unsafe static float SceneViewResolution
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SceneViewResolution, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SceneViewResolution, (void*)(&num));
		}
	}

	public unsafe static Color DefaultGizmoColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultGizmoColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultGizmoColor, (void*)(&color));
		}
	}

	public unsafe static Color DefaultGizmoSelectionColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultGizmoSelectionColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultGizmoSelectionColor, (void*)(&color));
		}
	}

	public unsafe static CurvyInterpolation DefaultInterpolation
	{
		get
		{
			Unsafe.SkipInit(out CurvyInterpolation result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultInterpolation, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultInterpolation, (void*)(&curvyInterpolation));
		}
	}

	public unsafe static float GizmoControlPointSize
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GizmoControlPointSize, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GizmoControlPointSize, (void*)(&num));
		}
	}

	public unsafe static float GizmoOrientationLength
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GizmoOrientationLength, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GizmoOrientationLength, (void*)(&num));
		}
	}

	public unsafe static Color GizmoOrientationColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GizmoOrientationColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GizmoOrientationColor, (void*)(&color));
		}
	}

	public unsafe static int SplineLayer
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SplineLayer, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SplineLayer, (void*)(&num));
		}
	}

	public unsafe static CurvySplineGizmos Gizmos
	{
		get
		{
			Unsafe.SkipInit(out CurvySplineGizmos result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Gizmos, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Gizmos, (void*)(&curvySplineGizmos));
		}
	}

	public unsafe PoolManager poolManager
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolManager);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PoolManager>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolManager)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)poolManager));
		}
	}

	public unsafe ComponentPool controlPointPool
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controlPointPool);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ComponentPool>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controlPointPool)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)componentPool));
		}
	}

	public unsafe ArrayPoolsSettings arrayPoolsSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_arrayPoolsSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ArrayPoolsSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_arrayPoolsSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPoolsSettings));
		}
	}

	public unsafe static bool ShowCurveGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34989, XrefRangeEnd = 34993, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowCurveGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34993, XrefRangeEnd = 34997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowCurveGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowConnectionsGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34997, XrefRangeEnd = 35001, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowConnectionsGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35001, XrefRangeEnd = 35005, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowConnectionsGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowApproximationGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35005, XrefRangeEnd = 35009, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowApproximationGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35009, XrefRangeEnd = 35013, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowApproximationGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowTangentsGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35013, XrefRangeEnd = 35017, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowTangentsGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35017, XrefRangeEnd = 35021, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowTangentsGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowOrientationGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35021, XrefRangeEnd = 35025, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowOrientationGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35025, XrefRangeEnd = 35029, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowOrientationGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowTFsGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35029, XrefRangeEnd = 35033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowTFsGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35033, XrefRangeEnd = 35037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowTFsGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowRelativeDistancesGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35037, XrefRangeEnd = 35041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowRelativeDistancesGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35041, XrefRangeEnd = 35045, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowRelativeDistancesGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowLabelsGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35045, XrefRangeEnd = 35049, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowLabelsGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35049, XrefRangeEnd = 35053, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowLabelsGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowMetadataGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35053, XrefRangeEnd = 35057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowMetadataGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35057, XrefRangeEnd = 35061, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowMetadataGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowBoundsGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35061, XrefRangeEnd = 35065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowBoundsGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35065, XrefRangeEnd = 35069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowBoundsGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool ShowOrientationAnchorsGizmo
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35069, XrefRangeEnd = 35073, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowOrientationAnchorsGizmo_Public_Static_get_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35073, XrefRangeEnd = 35077, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowOrientationAnchorsGizmo_Public_Static_set_Void_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PoolManager PoolManager
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35077, XrefRangeEnd = 35085, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PoolManager_Public_get_PoolManager_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PoolManager>(intPtr) : null;
		}
	}

	public unsafe ComponentPool ControlPointPool
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ControlPointPool_Public_get_ComponentPool_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ComponentPool>(intPtr) : null;
		}
	}

	public unsafe ArrayPoolsSettings ArrayPoolsSettings
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35085, XrefRangeEnd = 35093, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ArrayPoolsSettings_Public_get_ArrayPoolsSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ArrayPoolsSettings>(intPtr) : null;
		}
	}

	public unsafe Il2CppReferenceArray<CurvyConnection> Connections
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35093, XrefRangeEnd = 35096, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Connections_Public_get_Il2CppReferenceArray_1_CurvyConnection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvyConnection>>(intPtr) : null;
		}
	}

	static CurvyGlobalManager()
	{
		Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyGlobalManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultDefaultGizmoColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "DefaultDefaultGizmoColor");
		NativeFieldInfoPtr_DefaultDefaultGizmoSelectionColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "DefaultDefaultGizmoSelectionColor");
		NativeFieldInfoPtr_DefaultGizmoOrientationColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "DefaultGizmoOrientationColor");
		NativeFieldInfoPtr_HideManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "HideManager");
		NativeFieldInfoPtr_SaveGeneratorOutputs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "SaveGeneratorOutputs");
		NativeFieldInfoPtr_SceneViewResolution = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "SceneViewResolution");
		NativeFieldInfoPtr_DefaultGizmoColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "DefaultGizmoColor");
		NativeFieldInfoPtr_DefaultGizmoSelectionColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "DefaultGizmoSelectionColor");
		NativeFieldInfoPtr_DefaultInterpolation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "DefaultInterpolation");
		NativeFieldInfoPtr_GizmoControlPointSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "GizmoControlPointSize");
		NativeFieldInfoPtr_GizmoOrientationLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "GizmoOrientationLength");
		NativeFieldInfoPtr_GizmoOrientationColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "GizmoOrientationColor");
		NativeFieldInfoPtr_SplineLayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "SplineLayer");
		NativeFieldInfoPtr_Gizmos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "Gizmos");
		NativeFieldInfoPtr_poolManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "poolManager");
		NativeFieldInfoPtr_controlPointPool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "controlPointPool");
		NativeFieldInfoPtr_arrayPoolsSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, "arrayPoolsSettings");
		NativeMethodInfoPtr_get_ShowCurveGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664507);
		NativeMethodInfoPtr_set_ShowCurveGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664508);
		NativeMethodInfoPtr_get_ShowConnectionsGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664509);
		NativeMethodInfoPtr_set_ShowConnectionsGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664510);
		NativeMethodInfoPtr_get_ShowApproximationGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664511);
		NativeMethodInfoPtr_set_ShowApproximationGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664512);
		NativeMethodInfoPtr_get_ShowTangentsGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664513);
		NativeMethodInfoPtr_set_ShowTangentsGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664514);
		NativeMethodInfoPtr_get_ShowOrientationGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664515);
		NativeMethodInfoPtr_set_ShowOrientationGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664516);
		NativeMethodInfoPtr_get_ShowTFsGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664517);
		NativeMethodInfoPtr_set_ShowTFsGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664518);
		NativeMethodInfoPtr_get_ShowRelativeDistancesGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664519);
		NativeMethodInfoPtr_set_ShowRelativeDistancesGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664520);
		NativeMethodInfoPtr_get_ShowLabelsGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664521);
		NativeMethodInfoPtr_set_ShowLabelsGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664522);
		NativeMethodInfoPtr_get_ShowMetadataGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664523);
		NativeMethodInfoPtr_set_ShowMetadataGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664524);
		NativeMethodInfoPtr_get_ShowBoundsGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664525);
		NativeMethodInfoPtr_set_ShowBoundsGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664526);
		NativeMethodInfoPtr_get_ShowOrientationAnchorsGizmo_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664527);
		NativeMethodInfoPtr_set_ShowOrientationAnchorsGizmo_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664528);
		NativeMethodInfoPtr_get_PoolManager_Public_get_PoolManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664529);
		NativeMethodInfoPtr_get_ControlPointPool_Public_get_ComponentPool_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664530);
		NativeMethodInfoPtr_get_ArrayPoolsSettings_Public_get_ArrayPoolsSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664531);
		NativeMethodInfoPtr_get_Connections_Public_get_Il2CppReferenceArray_1_CurvyConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664532);
		NativeMethodInfoPtr_GetContainingConnections_Public_Il2CppReferenceArray_1_CurvyConnection_Il2CppReferenceArray_1_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664533);
		NativeMethodInfoPtr_Awake_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664534);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664535);
		NativeMethodInfoPtr_InitializeOnLoad_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664536);
		NativeMethodInfoPtr_LoadRuntimeSettings_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664537);
		NativeMethodInfoPtr_SaveRuntimeSettings_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664538);
		NativeMethodInfoPtr_MergeDoubleLoaded_Public_Virtual_Void_IDTSingleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664539);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr, 100664540);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35096, XrefRangeEnd = 35176, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<CurvyConnection> GetContainingConnections([Optional] Il2CppReferenceArray<CurvySpline> splines)
	{
		if (splines == null)
		{
			splines = new Il2CppReferenceArray<CurvySpline>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splines);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetContainingConnections_Public_Il2CppReferenceArray_1_CurvyConnection_Il2CppReferenceArray_1_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvyConnection>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35176, XrefRangeEnd = 35219, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35219, XrefRangeEnd = 35241, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35241, XrefRangeEnd = 35247, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitializeOnLoad()
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeOnLoad_Private_Static_Void_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35247, XrefRangeEnd = 35328, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LoadRuntimeSettings()
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadRuntimeSettings_Private_Static_Void_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35393, RefRangeEnd = 35394, XrefRangeStart = 35328, XrefRangeEnd = 35393, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SaveRuntimeSettings()
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveRuntimeSettings_Public_Static_Void_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35394, XrefRangeEnd = 35406, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void MergeDoubleLoaded(IDTSingleton newInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newInstance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_MergeDoubleLoaded_Public_Virtual_Void_IDTSingleton_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35406, XrefRangeEnd = 35412, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyGlobalManager()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyGlobalManager>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Il2CppReferenceArray<CurvyConnection> GetContainingConnections(params CurvySpline[] splines)
	{
		return GetContainingConnections(new Il2CppReferenceArray<CurvySpline>(splines));
	}

	public CurvyGlobalManager(IntPtr pointer)
		: base(pointer)
	{
	}
}
