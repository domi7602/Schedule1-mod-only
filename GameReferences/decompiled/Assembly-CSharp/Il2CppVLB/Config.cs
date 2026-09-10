using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppVLB;

public class Config : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_ClassName;

	private static readonly IntPtr NativeFieldInfoPtr_kAssetName;

	private static readonly IntPtr NativeFieldInfoPtr_kAssetNameExt;

	private static readonly IntPtr NativeFieldInfoPtr_geometryOverrideLayer;

	private static readonly IntPtr NativeFieldInfoPtr_geometryLayerID;

	private static readonly IntPtr NativeFieldInfoPtr_geometryTag;

	private static readonly IntPtr NativeFieldInfoPtr_geometryRenderQueue;

	private static readonly IntPtr NativeFieldInfoPtr_geometryRenderQueueHD;

	private static readonly IntPtr NativeFieldInfoPtr_m_RenderPipeline;

	private static readonly IntPtr NativeFieldInfoPtr_m_RenderingMode;

	private static readonly IntPtr NativeFieldInfoPtr_ditheringFactor;

	private static readonly IntPtr NativeFieldInfoPtr_useLightColorTemperature;

	private static readonly IntPtr NativeFieldInfoPtr_sharedMeshSides;

	private static readonly IntPtr NativeFieldInfoPtr_sharedMeshSegments;

	private static readonly IntPtr NativeFieldInfoPtr_hdBeamsCameraBlendingDistance;

	private static readonly IntPtr NativeFieldInfoPtr_urpDepthCameraScriptableRendererIndex;

	private static readonly IntPtr NativeFieldInfoPtr_globalNoiseScale;

	private static readonly IntPtr NativeFieldInfoPtr_globalNoiseVelocity;

	private static readonly IntPtr NativeFieldInfoPtr_fadeOutCameraTag;

	private static readonly IntPtr NativeFieldInfoPtr_noiseTexture3D;

	private static readonly IntPtr NativeFieldInfoPtr_dustParticlesPrefab;

	private static readonly IntPtr NativeFieldInfoPtr_ditheringNoiseTexture;

	private static readonly IntPtr NativeFieldInfoPtr_jitteringNoiseTexture;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledColorGradient;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledDepthBlend;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledNoise3D;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledDynamicOcclusion;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledMeshSkewing;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledShaderAccuracyHigh;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledShadow;

	private static readonly IntPtr NativeFieldInfoPtr_featureEnabledCookie;

	private static readonly IntPtr NativeFieldInfoPtr_m_RaymarchingQualities;

	private static readonly IntPtr NativeFieldInfoPtr_m_DefaultRaymarchingQualityUniqueID;

	private static readonly IntPtr NativeFieldInfoPtr_pluginVersion;

	private static readonly IntPtr NativeFieldInfoPtr__DummyMaterial;

	private static readonly IntPtr NativeFieldInfoPtr__DummyMaterialHD;

	private static readonly IntPtr NativeFieldInfoPtr__BeamShader;

	private static readonly IntPtr NativeFieldInfoPtr__BeamShaderHD;

	private static readonly IntPtr NativeFieldInfoPtr_m_CachedFadeOutCamera;

	private static readonly IntPtr NativeFieldInfoPtr_ms_Instance;

	private static readonly IntPtr NativeMethodInfoPtr_get_renderPipeline_Public_get_RenderPipeline_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_renderPipeline_Public_set_Void_RenderPipeline_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_renderingMode_Public_get_RenderingMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_renderingMode_Public_set_Void_RenderingMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsSRPBatcherSupported_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetActualRenderingMode_Public_RenderingMode_ShaderMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SD_useSinglePassShader_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SD_requiresDoubleSidedMesh_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetBeamShader_Public_Shader_ShaderMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetBeamShaderInternal_Private_byref_Shader_ShaderMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRenderQueueInternal_Private_Int32_ShaderMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewMaterialTransient_Public_Material_ShaderMode_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetURPScriptableRendererIndexToDepthCamera_Public_Void_Camera_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_fadeOutCameraTransform_Public_get_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr_ForceUpdateFadeOutCamera_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_defaultRaymarchingQualityUniqueID_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRaymarchingQualityForIndex_Public_RaymarchingQuality_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRaymarchingQualityForUniqueID_Public_RaymarchingQuality_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRaymarchingQualityIndexForUniqueID_Public_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsRaymarchingQualityUniqueIDValid_Public_Boolean_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_raymarchingQualitiesCount_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateDefaultRaymarchingQualityPreset_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_isHDRPExposureWeightSupported_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_hasRenderPipelineMismatch_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnStartup_Private_Static_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RefreshGlobalShaderProperties_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ResetInternalData_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewVolumetricDustParticles_Public_ParticleSystem_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleBackwardCompatibility_Private_Void_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Instance_Public_Static_get_Config_0;

	private static readonly IntPtr NativeMethodInfoPtr_LoadAssetInternal_Private_Static_Config_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetInstance_Private_Static_Config_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static string ClassName
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ClassName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ClassName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string kAssetName
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kAssetName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kAssetName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string kAssetNameExt
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kAssetNameExt, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kAssetNameExt, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool geometryOverrideLayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryOverrideLayer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryOverrideLayer)) = flag;
		}
	}

	public unsafe int geometryLayerID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryLayerID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryLayerID)) = num;
		}
	}

	public unsafe string geometryTag
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryTag);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryTag)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int geometryRenderQueue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryRenderQueue);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryRenderQueue)) = num;
		}
	}

	public unsafe int geometryRenderQueueHD
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryRenderQueueHD);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_geometryRenderQueueHD)) = num;
		}
	}

	public unsafe RenderPipeline m_RenderPipeline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RenderPipeline);
			return *(RenderPipeline*)num;
		}
		set
		{
			*(RenderPipeline*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RenderPipeline)) = renderPipeline;
		}
	}

	public unsafe RenderingMode m_RenderingMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RenderingMode);
			return *(RenderingMode*)num;
		}
		set
		{
			*(RenderingMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RenderingMode)) = renderingMode;
		}
	}

	public unsafe float ditheringFactor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ditheringFactor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ditheringFactor)) = num;
		}
	}

	public unsafe bool useLightColorTemperature
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_useLightColorTemperature);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_useLightColorTemperature)) = flag;
		}
	}

	public unsafe int sharedMeshSides
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sharedMeshSides);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sharedMeshSides)) = num;
		}
	}

	public unsafe int sharedMeshSegments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sharedMeshSegments);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sharedMeshSegments)) = num;
		}
	}

	public unsafe float hdBeamsCameraBlendingDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hdBeamsCameraBlendingDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hdBeamsCameraBlendingDistance)) = num;
		}
	}

	public unsafe int urpDepthCameraScriptableRendererIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_urpDepthCameraScriptableRendererIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_urpDepthCameraScriptableRendererIndex)) = num;
		}
	}

	public unsafe float globalNoiseScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_globalNoiseScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_globalNoiseScale)) = num;
		}
	}

	public unsafe Vector3 globalNoiseVelocity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_globalNoiseVelocity);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_globalNoiseVelocity)) = vector;
		}
	}

	public unsafe string fadeOutCameraTag
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fadeOutCameraTag);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fadeOutCameraTag)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Texture3D noiseTexture3D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noiseTexture3D);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Texture3D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noiseTexture3D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture3D));
		}
	}

	public unsafe ParticleSystem dustParticlesPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dustParticlesPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ParticleSystem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dustParticlesPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)particleSystem));
		}
	}

	public unsafe Texture2D ditheringNoiseTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ditheringNoiseTexture);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ditheringNoiseTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Texture2D jitteringNoiseTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jitteringNoiseTexture);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jitteringNoiseTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe FeatureEnabledColorGradient featureEnabledColorGradient
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledColorGradient);
			return *(FeatureEnabledColorGradient*)num;
		}
		set
		{
			*(FeatureEnabledColorGradient*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledColorGradient)) = featureEnabledColorGradient;
		}
	}

	public unsafe bool featureEnabledDepthBlend
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledDepthBlend);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledDepthBlend)) = flag;
		}
	}

	public unsafe bool featureEnabledNoise3D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledNoise3D);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledNoise3D)) = flag;
		}
	}

	public unsafe bool featureEnabledDynamicOcclusion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledDynamicOcclusion);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledDynamicOcclusion)) = flag;
		}
	}

	public unsafe bool featureEnabledMeshSkewing
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledMeshSkewing);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledMeshSkewing)) = flag;
		}
	}

	public unsafe bool featureEnabledShaderAccuracyHigh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledShaderAccuracyHigh);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledShaderAccuracyHigh)) = flag;
		}
	}

	public unsafe bool featureEnabledShadow
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledShadow);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledShadow)) = flag;
		}
	}

	public unsafe bool featureEnabledCookie
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledCookie);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_featureEnabledCookie)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<RaymarchingQuality> m_RaymarchingQualities
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RaymarchingQualities);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<RaymarchingQuality>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RaymarchingQualities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int m_DefaultRaymarchingQualityUniqueID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_DefaultRaymarchingQualityUniqueID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_DefaultRaymarchingQualityUniqueID)) = num;
		}
	}

	public unsafe int pluginVersion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pluginVersion);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pluginVersion)) = num;
		}
	}

	public unsafe Material _DummyMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DummyMaterial);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DummyMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material _DummyMaterialHD
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DummyMaterialHD);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DummyMaterialHD)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Shader _BeamShader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeamShader);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Shader>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeamShader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader));
		}
	}

	public unsafe Shader _BeamShaderHD
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeamShaderHD);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Shader>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeamShaderHD)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader));
		}
	}

	public unsafe Transform m_CachedFadeOutCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CachedFadeOutCamera);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CachedFadeOutCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe static Config ms_Instance
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_Instance, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != (IntPtr)0) ? Il2CppObjectPool.Get<Config>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_Instance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)config));
		}
	}

	public unsafe RenderPipeline renderPipeline
	{
		[CallerCount(149)]
		[CachedScanResults(RefRangeStart = 35494, RefRangeEnd = 35643, XrefRangeStart = 35494, XrefRangeEnd = 35643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_renderPipeline_Public_get_RenderPipeline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderPipeline*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68912, XrefRangeEnd = 68918, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_renderPipeline_Public_set_Void_RenderPipeline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe RenderingMode renderingMode
	{
		[CallerCount(126)]
		[CachedScanResults(RefRangeStart = 41326, RefRangeEnd = 41452, XrefRangeStart = 41326, XrefRangeEnd = 41452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_renderingMode_Public_get_RenderingMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderingMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68918, XrefRangeEnd = 68931, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_renderingMode_Public_set_Void_RenderingMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool SD_useSinglePassShader
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68937, XrefRangeEnd = 68938, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SD_useSinglePassShader_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool SD_requiresDoubleSidedMesh
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SD_requiresDoubleSidedMesh_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Transform fadeOutCameraTransform
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68980, XrefRangeEnd = 68991, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_fadeOutCameraTransform_Public_get_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
	}

	public unsafe int defaultRaymarchingQualityUniqueID
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 68998, RefRangeEnd = 68999, XrefRangeStart = 68998, XrefRangeEnd = 68998, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_defaultRaymarchingQualityUniqueID_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int raymarchingQualitiesCount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_raymarchingQualitiesCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool isHDRPExposureWeightSupported
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isHDRPExposureWeightSupported_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool hasRenderPipelineMismatch
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69042, XrefRangeEnd = 69043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hasRenderPipelineMismatch_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static Config Instance
	{
		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 69194, RefRangeEnd = 69212, XrefRangeStart = 69193, XrefRangeEnd = 69194, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Instance_Public_Static_get_Config_0, (IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Config>(intPtr) : null;
		}
	}

	static Config()
	{
		Il2CppClassPointerStore<Config>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "Config");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Config>.NativeClassPtr);
		NativeFieldInfoPtr_ClassName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "ClassName");
		NativeFieldInfoPtr_kAssetName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "kAssetName");
		NativeFieldInfoPtr_kAssetNameExt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "kAssetNameExt");
		NativeFieldInfoPtr_geometryOverrideLayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "geometryOverrideLayer");
		NativeFieldInfoPtr_geometryLayerID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "geometryLayerID");
		NativeFieldInfoPtr_geometryTag = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "geometryTag");
		NativeFieldInfoPtr_geometryRenderQueue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "geometryRenderQueue");
		NativeFieldInfoPtr_geometryRenderQueueHD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "geometryRenderQueueHD");
		NativeFieldInfoPtr_m_RenderPipeline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "m_RenderPipeline");
		NativeFieldInfoPtr_m_RenderingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "m_RenderingMode");
		NativeFieldInfoPtr_ditheringFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "ditheringFactor");
		NativeFieldInfoPtr_useLightColorTemperature = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "useLightColorTemperature");
		NativeFieldInfoPtr_sharedMeshSides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "sharedMeshSides");
		NativeFieldInfoPtr_sharedMeshSegments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "sharedMeshSegments");
		NativeFieldInfoPtr_hdBeamsCameraBlendingDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "hdBeamsCameraBlendingDistance");
		NativeFieldInfoPtr_urpDepthCameraScriptableRendererIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "urpDepthCameraScriptableRendererIndex");
		NativeFieldInfoPtr_globalNoiseScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "globalNoiseScale");
		NativeFieldInfoPtr_globalNoiseVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "globalNoiseVelocity");
		NativeFieldInfoPtr_fadeOutCameraTag = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "fadeOutCameraTag");
		NativeFieldInfoPtr_noiseTexture3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "noiseTexture3D");
		NativeFieldInfoPtr_dustParticlesPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "dustParticlesPrefab");
		NativeFieldInfoPtr_ditheringNoiseTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "ditheringNoiseTexture");
		NativeFieldInfoPtr_jitteringNoiseTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "jitteringNoiseTexture");
		NativeFieldInfoPtr_featureEnabledColorGradient = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledColorGradient");
		NativeFieldInfoPtr_featureEnabledDepthBlend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledDepthBlend");
		NativeFieldInfoPtr_featureEnabledNoise3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledNoise3D");
		NativeFieldInfoPtr_featureEnabledDynamicOcclusion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledDynamicOcclusion");
		NativeFieldInfoPtr_featureEnabledMeshSkewing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledMeshSkewing");
		NativeFieldInfoPtr_featureEnabledShaderAccuracyHigh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledShaderAccuracyHigh");
		NativeFieldInfoPtr_featureEnabledShadow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledShadow");
		NativeFieldInfoPtr_featureEnabledCookie = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "featureEnabledCookie");
		NativeFieldInfoPtr_m_RaymarchingQualities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "m_RaymarchingQualities");
		NativeFieldInfoPtr_m_DefaultRaymarchingQualityUniqueID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "m_DefaultRaymarchingQualityUniqueID");
		NativeFieldInfoPtr_pluginVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "pluginVersion");
		NativeFieldInfoPtr__DummyMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "_DummyMaterial");
		NativeFieldInfoPtr__DummyMaterialHD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "_DummyMaterialHD");
		NativeFieldInfoPtr__BeamShader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "_BeamShader");
		NativeFieldInfoPtr__BeamShaderHD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "_BeamShaderHD");
		NativeFieldInfoPtr_m_CachedFadeOutCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "m_CachedFadeOutCamera");
		NativeFieldInfoPtr_ms_Instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "ms_Instance");
		NativeMethodInfoPtr_get_renderPipeline_Public_get_RenderPipeline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663695);
		NativeMethodInfoPtr_set_renderPipeline_Public_set_Void_RenderPipeline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663696);
		NativeMethodInfoPtr_get_renderingMode_Public_get_RenderingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663697);
		NativeMethodInfoPtr_set_renderingMode_Public_set_Void_RenderingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663698);
		NativeMethodInfoPtr_IsSRPBatcherSupported_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663699);
		NativeMethodInfoPtr_GetActualRenderingMode_Public_RenderingMode_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663700);
		NativeMethodInfoPtr_get_SD_useSinglePassShader_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663701);
		NativeMethodInfoPtr_get_SD_requiresDoubleSidedMesh_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663702);
		NativeMethodInfoPtr_GetBeamShader_Public_Shader_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663703);
		NativeMethodInfoPtr_GetBeamShaderInternal_Private_byref_Shader_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663704);
		NativeMethodInfoPtr_GetRenderQueueInternal_Private_Int32_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663705);
		NativeMethodInfoPtr_NewMaterialTransient_Public_Material_ShaderMode_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663706);
		NativeMethodInfoPtr_SetURPScriptableRendererIndexToDepthCamera_Public_Void_Camera_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663707);
		NativeMethodInfoPtr_get_fadeOutCameraTransform_Public_get_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663708);
		NativeMethodInfoPtr_ForceUpdateFadeOutCamera_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663709);
		NativeMethodInfoPtr_get_defaultRaymarchingQualityUniqueID_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663710);
		NativeMethodInfoPtr_GetRaymarchingQualityForIndex_Public_RaymarchingQuality_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663711);
		NativeMethodInfoPtr_GetRaymarchingQualityForUniqueID_Public_RaymarchingQuality_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663712);
		NativeMethodInfoPtr_GetRaymarchingQualityIndexForUniqueID_Public_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663713);
		NativeMethodInfoPtr_IsRaymarchingQualityUniqueIDValid_Public_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663714);
		NativeMethodInfoPtr_get_raymarchingQualitiesCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663715);
		NativeMethodInfoPtr_CreateDefaultRaymarchingQualityPreset_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663716);
		NativeMethodInfoPtr_get_isHDRPExposureWeightSupported_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663717);
		NativeMethodInfoPtr_get_hasRenderPipelineMismatch_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663718);
		NativeMethodInfoPtr_OnStartup_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663719);
		NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663720);
		NativeMethodInfoPtr_RefreshGlobalShaderProperties_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663721);
		NativeMethodInfoPtr_ResetInternalData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663722);
		NativeMethodInfoPtr_NewVolumetricDustParticles_Public_ParticleSystem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663723);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663724);
		NativeMethodInfoPtr_HandleBackwardCompatibility_Private_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663725);
		NativeMethodInfoPtr_get_Instance_Public_Static_get_Config_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663726);
		NativeMethodInfoPtr_LoadAssetInternal_Private_Static_Config_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663727);
		NativeMethodInfoPtr_GetInstance_Private_Static_Config_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663728);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Config>.NativeClassPtr, 100663729);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68931, XrefRangeEnd = 68932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsSRPBatcherSupported()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsSRPBatcherSupported_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 68933, RefRangeEnd = 68937, XrefRangeStart = 68932, XrefRangeEnd = 68933, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RenderingMode GetActualRenderingMode(ShaderMode shaderMode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&shaderMode);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetActualRenderingMode_Public_RenderingMode_ShaderMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RenderingMode*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe Shader GetBeamShader(ShaderMode mode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBeamShader_Public_Shader_ShaderMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Shader>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe ref Shader GetBeamShaderInternal(ShaderMode mode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBeamShaderInternal_Private_byref_Shader_ShaderMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return ref *(Shader*)intPtr;
	}

	[CallerCount(0)]
	public unsafe int GetRenderQueueInternal(ShaderMode mode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRenderQueueInternal_Private_Int32_ShaderMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 68970, RefRangeEnd = 68973, XrefRangeStart = 68938, XrefRangeEnd = 68970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Material NewMaterialTransient(ShaderMode mode, bool gpuInstanced)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&mode);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &gpuInstanced;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewMaterialTransient_Public_Material_ShaderMode_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 68978, RefRangeEnd = 68980, XrefRangeStart = 68973, XrefRangeEnd = 68978, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetURPScriptableRendererIndexToDepthCamera(Camera camera)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetURPScriptableRendererIndexToDepthCamera_Public_Void_Camera_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68991, XrefRangeEnd = 68998, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ForceUpdateFadeOutCamera()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForceUpdateFadeOutCamera_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 68999, RefRangeEnd = 69002, XrefRangeStart = 68999, XrefRangeEnd = 68999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RaymarchingQuality GetRaymarchingQualityForIndex(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRaymarchingQualityForIndex_Public_RaymarchingQuality_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RaymarchingQuality>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 69003, RefRangeEnd = 69005, XrefRangeStart = 69002, XrefRangeEnd = 69003, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RaymarchingQuality GetRaymarchingQualityForUniqueID(int id)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&id);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRaymarchingQualityForUniqueID_Public_RaymarchingQuality_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RaymarchingQuality>(intPtr) : null;
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 69010, RefRangeEnd = 69017, XrefRangeStart = 69005, XrefRangeEnd = 69010, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetRaymarchingQualityIndexForUniqueID(int id)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&id);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRaymarchingQualityIndexForUniqueID_Public_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69017, XrefRangeEnd = 69018, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsRaymarchingQualityUniqueIDValid(int id)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&id);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsRaymarchingQualityUniqueIDValid_Public_Boolean_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 69040, RefRangeEnd = 69042, XrefRangeStart = 69018, XrefRangeEnd = 69040, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CreateDefaultRaymarchingQualityPreset(bool onlyIfNeeded)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&onlyIfNeeded);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateDefaultRaymarchingQualityPreset_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69043, XrefRangeEnd = 69075, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OnStartup()
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnStartup_Private_Static_Void_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69075, XrefRangeEnd = 69101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69101, XrefRangeEnd = 69121, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RefreshGlobalShaderProperties()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RefreshGlobalShaderProperties_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 69163, RefRangeEnd = 69164, XrefRangeStart = 69121, XrefRangeEnd = 69163, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetInternalData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetInternalData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 69191, RefRangeEnd = 69192, XrefRangeStart = 69164, XrefRangeEnd = 69191, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ParticleSystem NewVolumetricDustParticles()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewVolumetricDustParticles_Public_ParticleSystem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ParticleSystem>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69192, XrefRangeEnd = 69193, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleBackwardCompatibility(int serializedVersion, int newVersion)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&serializedVersion);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &newVersion;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleBackwardCompatibility_Private_Void_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69212, XrefRangeEnd = 69215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Config LoadAssetInternal(string assetName)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(assetName);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadAssetInternal_Private_Static_Config_String_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Config>(intPtr) : null;
	}

	[CallerCount(99)]
	[CachedScanResults(RefRangeStart = 69249, RefRangeEnd = 69348, XrefRangeStart = 69215, XrefRangeEnd = 69249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Config GetInstance(bool assertIfNotFound)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&assertIfNotFound);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInstance_Private_Static_Config_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Config>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69348, XrefRangeEnd = 69361, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Config()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Config>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Config(IntPtr pointer)
		: base(pointer)
	{
	}
}
