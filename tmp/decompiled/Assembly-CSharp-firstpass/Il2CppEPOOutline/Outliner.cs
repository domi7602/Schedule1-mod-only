using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppEPOOutline;

public class Outliner : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_temporaryOutlinables;

	private static readonly IntPtr NativeFieldInfoPtr_parameters;

	private static readonly IntPtr NativeFieldInfoPtr_targetCamera;

	private static readonly IntPtr NativeFieldInfoPtr_stage;

	private static readonly IntPtr NativeFieldInfoPtr_renderingStrategy;

	private static readonly IntPtr NativeFieldInfoPtr_renderingMode;

	private static readonly IntPtr NativeFieldInfoPtr_outlineLayerMask;

	private static readonly IntPtr NativeFieldInfoPtr_primaryBufferSizeMode;

	private static readonly IntPtr NativeFieldInfoPtr_primaryRendererScale;

	private static readonly IntPtr NativeFieldInfoPtr_primarySizeReference;

	private static readonly IntPtr NativeFieldInfoPtr_blurShift;

	private static readonly IntPtr NativeFieldInfoPtr_dilateShift;

	private static readonly IntPtr NativeFieldInfoPtr_dilateIterations;

	private static readonly IntPtr NativeFieldInfoPtr_dilateQuality;

	private static readonly IntPtr NativeFieldInfoPtr_blurIterations;

	private static readonly IntPtr NativeFieldInfoPtr_blurType;

	private static readonly IntPtr NativeMethodInfoPtr_get_InfoRendererScale_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_InfoRendererScale_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PrimarySizeReference_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_PrimarySizeReference_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PrimaryBufferSizeMode_Public_get_BufferSizeMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_PrimaryBufferSizeMode_Public_set_Void_BufferSizeMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Event_Private_get_CameraEvent_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RenderingStrategy_Public_get_OutlineRenderingStrategy_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RenderingStrategy_Public_set_Void_OutlineRenderingStrategy_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RenderStage_Public_get_RenderStage_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RenderStage_Public_set_Void_RenderStage_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DilateQuality_Public_get_DilateQuality_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_DilateQuality_Public_set_Void_DilateQuality_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RenderingMode_Private_get_RenderingMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RenderingMode_Private_set_Void_RenderingMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_BlurShift_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_BlurShift_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DilateShift_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_DilateShift_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_OutlineLayerMask_Public_get_Int64_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_OutlineLayerMask_Public_set_Void_Int64_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PrimaryRendererScale_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_PrimaryRendererScale_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_BlurIterrations_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_BlurIterrations_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_BlurIterations_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_BlurIterations_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_BlurType_Public_get_BlurType_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_BlurType_Public_set_Void_BlurType_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DilateIterration_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_DilateIterration_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DilateIterations_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_DilateIterations_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnValidate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateBuffer_Private_Void_Camera_CommandBuffer_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnPreRender_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetupOutline_Private_Void_Camera_OutlineParameters_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateSharedParameters_Public_Void_OutlineParameters_Camera_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateParameters_Private_Void_OutlineParameters_Camera_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static List<Outlinable> temporaryOutlinables
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_temporaryOutlinables, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != (IntPtr)0) ? Il2CppObjectPool.Get<List<Outlinable>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_temporaryOutlinables, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe OutlineParameters parameters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parameters);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<OutlineParameters>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parameters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outlineParameters));
		}
	}

	public unsafe Camera targetCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetCamera);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Camera>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera));
		}
	}

	public unsafe RenderStage stage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stage);
			return *(RenderStage*)num;
		}
		set
		{
			*(RenderStage*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stage)) = renderStage;
		}
	}

	public unsafe OutlineRenderingStrategy renderingStrategy
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderingStrategy);
			return *(OutlineRenderingStrategy*)num;
		}
		set
		{
			*(OutlineRenderingStrategy*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderingStrategy)) = outlineRenderingStrategy;
		}
	}

	public unsafe RenderingMode renderingMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderingMode);
			return *(RenderingMode*)num;
		}
		set
		{
			*(RenderingMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderingMode)) = renderingMode;
		}
	}

	public unsafe long outlineLayerMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outlineLayerMask);
			return *(long*)num;
		}
		set
		{
			*(long*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outlineLayerMask)) = num;
		}
	}

	public unsafe BufferSizeMode primaryBufferSizeMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primaryBufferSizeMode);
			return *(BufferSizeMode*)num;
		}
		set
		{
			*(BufferSizeMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primaryBufferSizeMode)) = bufferSizeMode;
		}
	}

	public unsafe float primaryRendererScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primaryRendererScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primaryRendererScale)) = num;
		}
	}

	public unsafe int primarySizeReference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primarySizeReference);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primarySizeReference)) = num;
		}
	}

	public unsafe float blurShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blurShift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blurShift)) = num;
		}
	}

	public unsafe float dilateShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dilateShift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dilateShift)) = num;
		}
	}

	public unsafe int dilateIterations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dilateIterations);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dilateIterations)) = num;
		}
	}

	public unsafe DilateQuality dilateQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dilateQuality);
			return *(DilateQuality*)num;
		}
		set
		{
			*(DilateQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dilateQuality)) = dilateQuality;
		}
	}

	public unsafe int blurIterations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blurIterations);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blurIterations)) = num;
		}
	}

	public unsafe BlurType blurType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blurType);
			return *(BlurType*)num;
		}
		set
		{
			*(BlurType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blurType)) = blurType;
		}
	}

	public unsafe float InfoRendererScale
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63922, XrefRangeEnd = 63927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InfoRendererScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63927, XrefRangeEnd = 63932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_InfoRendererScale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int PrimarySizeReference
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38157, RefRangeEnd = 38163, XrefRangeStart = 38157, XrefRangeEnd = 38163, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PrimarySizeReference_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 44553, RefRangeEnd = 44554, XrefRangeStart = 44553, XrefRangeEnd = 44554, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_PrimarySizeReference_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe BufferSizeMode PrimaryBufferSizeMode
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38121, RefRangeEnd = 38127, XrefRangeStart = 38121, XrefRangeEnd = 38127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PrimaryBufferSizeMode_Public_get_BufferSizeMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(BufferSizeMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 44612, RefRangeEnd = 44616, XrefRangeStart = 44612, XrefRangeEnd = 44616, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_PrimaryBufferSizeMode_Public_set_Void_BufferSizeMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CameraEvent Event
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Event_Private_get_CameraEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CameraEvent*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe OutlineRenderingStrategy RenderingStrategy
	{
		[CallerCount(126)]
		[CachedScanResults(RefRangeStart = 41326, RefRangeEnd = 41452, XrefRangeStart = 41326, XrefRangeEnd = 41452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RenderingStrategy_Public_get_OutlineRenderingStrategy_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(OutlineRenderingStrategy*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RenderingStrategy_Public_set_Void_OutlineRenderingStrategy_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe RenderStage RenderStage
	{
		[CallerCount(149)]
		[CachedScanResults(RefRangeStart = 35494, RefRangeEnd = 35643, XrefRangeStart = 35494, XrefRangeEnd = 35643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RenderStage_Public_get_RenderStage_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderStage*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 63932, RefRangeEnd = 63933, XrefRangeStart = 63932, XrefRangeEnd = 63932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RenderStage_Public_set_Void_RenderStage_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe DilateQuality DilateQuality
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 44557, RefRangeEnd = 44560, XrefRangeStart = 44557, XrefRangeEnd = 44560, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DilateQuality_Public_get_DilateQuality_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(DilateQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(17)]
		[CachedScanResults(RefRangeStart = 44560, RefRangeEnd = 44577, XrefRangeStart = 44560, XrefRangeEnd = 44577, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DilateQuality_Public_set_Void_DilateQuality_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe RenderingMode RenderingMode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RenderingMode_Private_get_RenderingMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderingMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RenderingMode_Private_set_Void_RenderingMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float BlurShift
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BlurShift_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BlurShift_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float DilateShift
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DilateShift_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DilateShift_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe long OutlineLayerMask
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30474, RefRangeEnd = 30475, XrefRangeStart = 30474, XrefRangeEnd = 30475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OutlineLayerMask_Public_get_Int64_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(long*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OutlineLayerMask_Public_set_Void_Int64_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float PrimaryRendererScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PrimaryRendererScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_PrimaryRendererScale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int BlurIterrations
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38226, RefRangeEnd = 38232, XrefRangeStart = 38226, XrefRangeEnd = 38232, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BlurIterrations_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BlurIterrations_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int BlurIterations
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38226, RefRangeEnd = 38232, XrefRangeStart = 38226, XrefRangeEnd = 38232, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BlurIterations_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BlurIterations_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe BlurType BlurType
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 63933, RefRangeEnd = 63934, XrefRangeStart = 63933, XrefRangeEnd = 63933, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BlurType_Public_get_BlurType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(BlurType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 63934, RefRangeEnd = 63943, XrefRangeStart = 63934, XrefRangeEnd = 63934, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BlurType_Public_set_Void_BlurType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int DilateIterration
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 63943, RefRangeEnd = 63949, XrefRangeStart = 63943, XrefRangeEnd = 63943, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DilateIterration_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DilateIterration_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int DilateIterations
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 63943, RefRangeEnd = 63949, XrefRangeStart = 63943, XrefRangeEnd = 63949, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DilateIterations_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DilateIterations_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Outliner()
	{
		Il2CppClassPointerStore<Outliner>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "Outliner");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Outliner>.NativeClassPtr);
		NativeFieldInfoPtr_temporaryOutlinables = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "temporaryOutlinables");
		NativeFieldInfoPtr_parameters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "parameters");
		NativeFieldInfoPtr_targetCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "targetCamera");
		NativeFieldInfoPtr_stage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "stage");
		NativeFieldInfoPtr_renderingStrategy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "renderingStrategy");
		NativeFieldInfoPtr_renderingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "renderingMode");
		NativeFieldInfoPtr_outlineLayerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "outlineLayerMask");
		NativeFieldInfoPtr_primaryBufferSizeMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "primaryBufferSizeMode");
		NativeFieldInfoPtr_primaryRendererScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "primaryRendererScale");
		NativeFieldInfoPtr_primarySizeReference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "primarySizeReference");
		NativeFieldInfoPtr_blurShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "blurShift");
		NativeFieldInfoPtr_dilateShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "dilateShift");
		NativeFieldInfoPtr_dilateIterations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "dilateIterations");
		NativeFieldInfoPtr_dilateQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "dilateQuality");
		NativeFieldInfoPtr_blurIterations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "blurIterations");
		NativeFieldInfoPtr_blurType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Outliner>.NativeClassPtr, "blurType");
		NativeMethodInfoPtr_get_InfoRendererScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667383);
		NativeMethodInfoPtr_set_InfoRendererScale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667384);
		NativeMethodInfoPtr_get_PrimarySizeReference_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667385);
		NativeMethodInfoPtr_set_PrimarySizeReference_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667386);
		NativeMethodInfoPtr_get_PrimaryBufferSizeMode_Public_get_BufferSizeMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667387);
		NativeMethodInfoPtr_set_PrimaryBufferSizeMode_Public_set_Void_BufferSizeMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667388);
		NativeMethodInfoPtr_get_Event_Private_get_CameraEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667389);
		NativeMethodInfoPtr_get_RenderingStrategy_Public_get_OutlineRenderingStrategy_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667390);
		NativeMethodInfoPtr_set_RenderingStrategy_Public_set_Void_OutlineRenderingStrategy_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667391);
		NativeMethodInfoPtr_get_RenderStage_Public_get_RenderStage_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667392);
		NativeMethodInfoPtr_set_RenderStage_Public_set_Void_RenderStage_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667393);
		NativeMethodInfoPtr_get_DilateQuality_Public_get_DilateQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667394);
		NativeMethodInfoPtr_set_DilateQuality_Public_set_Void_DilateQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667395);
		NativeMethodInfoPtr_get_RenderingMode_Private_get_RenderingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667396);
		NativeMethodInfoPtr_set_RenderingMode_Private_set_Void_RenderingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667397);
		NativeMethodInfoPtr_get_BlurShift_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667398);
		NativeMethodInfoPtr_set_BlurShift_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667399);
		NativeMethodInfoPtr_get_DilateShift_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667400);
		NativeMethodInfoPtr_set_DilateShift_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667401);
		NativeMethodInfoPtr_get_OutlineLayerMask_Public_get_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667402);
		NativeMethodInfoPtr_set_OutlineLayerMask_Public_set_Void_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667403);
		NativeMethodInfoPtr_get_PrimaryRendererScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667404);
		NativeMethodInfoPtr_set_PrimaryRendererScale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667405);
		NativeMethodInfoPtr_get_BlurIterrations_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667406);
		NativeMethodInfoPtr_set_BlurIterrations_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667407);
		NativeMethodInfoPtr_get_BlurIterations_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667408);
		NativeMethodInfoPtr_set_BlurIterations_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667409);
		NativeMethodInfoPtr_get_BlurType_Public_get_BlurType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667410);
		NativeMethodInfoPtr_set_BlurType_Public_set_Void_BlurType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667411);
		NativeMethodInfoPtr_get_DilateIterration_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667412);
		NativeMethodInfoPtr_set_DilateIterration_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667413);
		NativeMethodInfoPtr_get_DilateIterations_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667414);
		NativeMethodInfoPtr_set_DilateIterations_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667415);
		NativeMethodInfoPtr_OnValidate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667416);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667417);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667418);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667419);
		NativeMethodInfoPtr_UpdateBuffer_Private_Void_Camera_CommandBuffer_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667420);
		NativeMethodInfoPtr_OnPreRender_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667421);
		NativeMethodInfoPtr_SetupOutline_Private_Void_Camera_OutlineParameters_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667422);
		NativeMethodInfoPtr_UpdateSharedParameters_Public_Void_OutlineParameters_Camera_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667423);
		NativeMethodInfoPtr_UpdateParameters_Private_Void_OutlineParameters_Camera_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667424);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Outliner>.NativeClassPtr, 100667425);
	}

	[CallerCount(0)]
	public unsafe void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnValidate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63949, XrefRangeEnd = 63970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63970, XrefRangeEnd = 63975, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63975, XrefRangeEnd = 63981, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63981, XrefRangeEnd = 63984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateBuffer(Camera targetCamera, CommandBuffer buffer, bool removeOnly)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)targetCamera);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)buffer);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &removeOnly;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateBuffer_Private_Void_Camera_CommandBuffer_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63984, XrefRangeEnd = 63992, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnPreRender()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnPreRender_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 64042, RefRangeEnd = 64043, XrefRangeStart = 63992, XrefRangeEnd = 64042, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetupOutline(Camera cameraToUse, OutlineParameters parametersToUse, bool isEditor)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cameraToUse);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parametersToUse);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &isEditor;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupOutline_Private_Void_Camera_OutlineParameters_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 64059, RefRangeEnd = 64060, XrefRangeStart = 64043, XrefRangeEnd = 64059, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSharedParameters(OutlineParameters parameters, Camera camera, bool editorCamera)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &editorCamera;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSharedParameters_Public_Void_OutlineParameters_Camera_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 64123, RefRangeEnd = 64124, XrefRangeStart = 64060, XrefRangeEnd = 64123, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateParameters(OutlineParameters parameters, Camera camera, bool editorCamera)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &editorCamera;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateParameters_Private_Void_OutlineParameters_Camera_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64124, XrefRangeEnd = 64159, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Outliner()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Outliner>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Outliner(IntPtr pointer)
		: base(pointer)
	{
	}
}
