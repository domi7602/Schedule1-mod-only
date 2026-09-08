using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppVLB;

public static class MaterialManager : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "BlendingMode")]
	public enum BlendingMode
	{
		Additive,
		SoftAdditive,
		TraditionalTransparency,
		Count
	}

	[OriginalName("Assembly-CSharp.dll", "", "ColorGradient")]
	public enum ColorGradient
	{
		Off,
		MatrixLow,
		MatrixHigh,
		Count
	}

	[OriginalName("Assembly-CSharp.dll", "", "Noise3D")]
	public enum Noise3D
	{
		Off,
		On,
		Count
	}

	public static class SD : Il2CppSystem.Object
	{
		[OriginalName("Assembly-CSharp.dll", "", "DepthBlend")]
		public enum DepthBlend
		{
			Off,
			On,
			Count
		}

		[OriginalName("Assembly-CSharp.dll", "", "DynamicOcclusion")]
		public enum DynamicOcclusion
		{
			Off,
			ClippingPlane,
			DepthTexture,
			Count
		}

		[OriginalName("Assembly-CSharp.dll", "", "MeshSkewing")]
		public enum MeshSkewing
		{
			Off,
			On,
			Count
		}

		[OriginalName("Assembly-CSharp.dll", "", "ShaderAccuracy")]
		public enum ShaderAccuracy
		{
			Fast,
			High,
			Count
		}

		static SD()
		{
			Il2CppClassPointerStore<SD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "SD");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SD>.NativeClassPtr);
		}

		public SD(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class HD : Il2CppSystem.Object
	{
		[OriginalName("Assembly-CSharp.dll", "", "Attenuation")]
		public enum Attenuation
		{
			Linear,
			Quadratic,
			Count
		}

		[OriginalName("Assembly-CSharp.dll", "", "Shadow")]
		public enum Shadow
		{
			Off,
			On,
			Count
		}

		[OriginalName("Assembly-CSharp.dll", "", "Cookie")]
		public enum Cookie
		{
			Off,
			SingleChannel,
			RGBA,
			Count
		}

		static HD()
		{
			Il2CppClassPointerStore<HD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "HD");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HD>.NativeClassPtr);
		}

		public HD(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class IStaticProperties : Il2CppObjectBase
	{
		private static readonly System.IntPtr NativeMethodInfoPtr_GetPropertiesCount_Public_Abstract_Virtual_New_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetMaterialID_Public_Abstract_Virtual_New_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToMaterial_Public_Abstract_Virtual_New_Void_Material_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetShaderMode_Public_Abstract_Virtual_New_ShaderMode_0;

		static IStaticProperties()
		{
			Il2CppClassPointerStore<IStaticProperties>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "IStaticProperties");
			NativeMethodInfoPtr_GetPropertiesCount_Public_Abstract_Virtual_New_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStaticProperties>.NativeClassPtr, 100664017);
			NativeMethodInfoPtr_GetMaterialID_Public_Abstract_Virtual_New_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStaticProperties>.NativeClassPtr, 100664018);
			NativeMethodInfoPtr_ApplyToMaterial_Public_Abstract_Virtual_New_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStaticProperties>.NativeClassPtr, 100664019);
			NativeMethodInfoPtr_GetShaderMode_Public_Abstract_Virtual_New_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStaticProperties>.NativeClassPtr, 100664020);
		}

		[CallerCount(0)]
		public unsafe virtual int GetPropertiesCount()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetPropertiesCount_Public_Abstract_Virtual_New_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe virtual int GetMaterialID()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetMaterialID_Public_Abstract_Virtual_New_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe virtual void ApplyToMaterial(Material mat)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ApplyToMaterial_Public_Abstract_Virtual_New_Void_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual ShaderMode GetShaderMode()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetShaderMode_Public_Abstract_Virtual_New_ShaderMode_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ShaderMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public IStaticProperties(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct StaticPropertiesSD
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_blendingMode;

		private static readonly System.IntPtr NativeFieldInfoPtr_noise3D;

		private static readonly System.IntPtr NativeFieldInfoPtr_depthBlend;

		private static readonly System.IntPtr NativeFieldInfoPtr_colorGradient;

		private static readonly System.IntPtr NativeFieldInfoPtr_dynamicOcclusion;

		private static readonly System.IntPtr NativeFieldInfoPtr_meshSkewing;

		private static readonly System.IntPtr NativeFieldInfoPtr_shaderAccuracy;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetShaderMode_Public_Virtual_Final_New_ShaderMode_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_staticPropertiesCount_Public_Static_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetPropertiesCount_Public_Virtual_Final_New_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_blendingModeID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_noise3DID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_depthBlendID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_colorGradientID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_dynamicOcclusionID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_meshSkewingID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_shaderAccuracyID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetMaterialID_Public_Virtual_Final_New_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToMaterial_Public_Virtual_Final_New_Void_Material_0;

		[FieldOffset(0)]
		public BlendingMode blendingMode;

		[FieldOffset(4)]
		public Noise3D noise3D;

		[FieldOffset(8)]
		public SD.DepthBlend depthBlend;

		[FieldOffset(12)]
		public ColorGradient colorGradient;

		[FieldOffset(16)]
		public SD.DynamicOcclusion dynamicOcclusion;

		[FieldOffset(20)]
		public SD.MeshSkewing meshSkewing;

		[FieldOffset(24)]
		public SD.ShaderAccuracy shaderAccuracy;

		public unsafe static int staticPropertiesCount
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_staticPropertiesCount_Public_Static_get_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int blendingModeID
		{
			[CallerCount(501)]
			[CachedScanResults(RefRangeStart = 40619, RefRangeEnd = 41120, XrefRangeStart = 40619, XrefRangeEnd = 41120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_blendingModeID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int noise3DID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70909, XrefRangeEnd = 70910, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_noise3DID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int depthBlendID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70910, XrefRangeEnd = 70911, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_depthBlendID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int colorGradientID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70911, XrefRangeEnd = 70912, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_colorGradientID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int dynamicOcclusionID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70912, XrefRangeEnd = 70913, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_dynamicOcclusionID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int meshSkewingID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70913, XrefRangeEnd = 70914, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_meshSkewingID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int shaderAccuracyID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70914, XrefRangeEnd = 70915, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_shaderAccuracyID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static StaticPropertiesSD()
		{
			Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "StaticPropertiesSD");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr);
			NativeFieldInfoPtr_blendingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "blendingMode");
			NativeFieldInfoPtr_noise3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "noise3D");
			NativeFieldInfoPtr_depthBlend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "depthBlend");
			NativeFieldInfoPtr_colorGradient = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "colorGradient");
			NativeFieldInfoPtr_dynamicOcclusion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "dynamicOcclusion");
			NativeFieldInfoPtr_meshSkewing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "meshSkewing");
			NativeFieldInfoPtr_shaderAccuracy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, "shaderAccuracy");
			NativeMethodInfoPtr_GetShaderMode_Public_Virtual_Final_New_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664021);
			NativeMethodInfoPtr_get_staticPropertiesCount_Public_Static_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664022);
			NativeMethodInfoPtr_GetPropertiesCount_Public_Virtual_Final_New_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664023);
			NativeMethodInfoPtr_get_blendingModeID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664024);
			NativeMethodInfoPtr_get_noise3DID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664025);
			NativeMethodInfoPtr_get_depthBlendID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664026);
			NativeMethodInfoPtr_get_colorGradientID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664027);
			NativeMethodInfoPtr_get_dynamicOcclusionID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664028);
			NativeMethodInfoPtr_get_meshSkewingID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664029);
			NativeMethodInfoPtr_get_shaderAccuracyID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664030);
			NativeMethodInfoPtr_GetMaterialID_Public_Virtual_Final_New_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664031);
			NativeMethodInfoPtr_ApplyToMaterial_Public_Virtual_Final_New_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, 100664032);
		}

		[CallerCount(73)]
		[CachedScanResults(RefRangeStart = 31078, RefRangeEnd = 31151, XrefRangeStart = 31078, XrefRangeEnd = 31151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual ShaderMode GetShaderMode()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetShaderMode_Public_Virtual_Final_New_ShaderMode_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ShaderMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe virtual int GetPropertiesCount()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPropertiesCount_Public_Virtual_Final_New_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70915, XrefRangeEnd = 70927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual int GetMaterialID()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaterialID_Public_Virtual_Final_New_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 70971, RefRangeEnd = 70972, XrefRangeStart = 70927, XrefRangeEnd = 70971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void ApplyToMaterial(Material mat)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyToMaterial_Public_Virtual_Final_New_Void_Material_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<StaticPropertiesSD>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct StaticPropertiesHD
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_blendingMode;

		private static readonly System.IntPtr NativeFieldInfoPtr_attenuation;

		private static readonly System.IntPtr NativeFieldInfoPtr_noise3D;

		private static readonly System.IntPtr NativeFieldInfoPtr_colorGradient;

		private static readonly System.IntPtr NativeFieldInfoPtr_shadow;

		private static readonly System.IntPtr NativeFieldInfoPtr_cookie;

		private static readonly System.IntPtr NativeFieldInfoPtr_raymarchingQualityIndex;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetShaderMode_Public_Virtual_Final_New_ShaderMode_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_staticPropertiesCount_Public_Static_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetPropertiesCount_Public_Virtual_Final_New_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_blendingModeID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_attenuationID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_noise3DID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_colorGradientID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_dynamicOcclusionID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_cookieID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_raymarchingQualityID_Private_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetMaterialID_Public_Virtual_Final_New_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToMaterial_Public_Virtual_Final_New_Void_Material_0;

		[FieldOffset(0)]
		public BlendingMode blendingMode;

		[FieldOffset(4)]
		public HD.Attenuation attenuation;

		[FieldOffset(8)]
		public Noise3D noise3D;

		[FieldOffset(12)]
		public ColorGradient colorGradient;

		[FieldOffset(16)]
		public HD.Shadow shadow;

		[FieldOffset(20)]
		public HD.Cookie cookie;

		[FieldOffset(24)]
		public int raymarchingQualityIndex;

		public unsafe static int staticPropertiesCount
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70972, XrefRangeEnd = 70973, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_staticPropertiesCount_Public_Static_get_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int blendingModeID
		{
			[CallerCount(501)]
			[CachedScanResults(RefRangeStart = 40619, RefRangeEnd = 41120, XrefRangeStart = 40619, XrefRangeEnd = 41120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_blendingModeID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int attenuationID
		{
			[CallerCount(120)]
			[CachedScanResults(RefRangeStart = 54296, RefRangeEnd = 54416, XrefRangeStart = 54296, XrefRangeEnd = 54416, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_attenuationID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int noise3DID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70973, XrefRangeEnd = 70974, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_noise3DID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int colorGradientID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_colorGradientID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int dynamicOcclusionID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70974, XrefRangeEnd = 70975, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_dynamicOcclusionID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int cookieID
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70975, XrefRangeEnd = 70976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_cookieID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe int raymarchingQualityID
		{
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 3891, RefRangeEnd = 3894, XrefRangeStart = 3891, XrefRangeEnd = 3894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_raymarchingQualityID_Private_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static StaticPropertiesHD()
		{
			Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "StaticPropertiesHD");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr);
			NativeFieldInfoPtr_blendingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "blendingMode");
			NativeFieldInfoPtr_attenuation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "attenuation");
			NativeFieldInfoPtr_noise3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "noise3D");
			NativeFieldInfoPtr_colorGradient = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "colorGradient");
			NativeFieldInfoPtr_shadow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "shadow");
			NativeFieldInfoPtr_cookie = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "cookie");
			NativeFieldInfoPtr_raymarchingQualityIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, "raymarchingQualityIndex");
			NativeMethodInfoPtr_GetShaderMode_Public_Virtual_Final_New_ShaderMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664033);
			NativeMethodInfoPtr_get_staticPropertiesCount_Public_Static_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664034);
			NativeMethodInfoPtr_GetPropertiesCount_Public_Virtual_Final_New_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664035);
			NativeMethodInfoPtr_get_blendingModeID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664036);
			NativeMethodInfoPtr_get_attenuationID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664037);
			NativeMethodInfoPtr_get_noise3DID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664038);
			NativeMethodInfoPtr_get_colorGradientID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664039);
			NativeMethodInfoPtr_get_dynamicOcclusionID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664040);
			NativeMethodInfoPtr_get_cookieID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664041);
			NativeMethodInfoPtr_get_raymarchingQualityID_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664042);
			NativeMethodInfoPtr_GetMaterialID_Public_Virtual_Final_New_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664043);
			NativeMethodInfoPtr_ApplyToMaterial_Public_Virtual_Final_New_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, 100664044);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 70633, RefRangeEnd = 70636, XrefRangeStart = 70633, XrefRangeEnd = 70636, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual ShaderMode GetShaderMode()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetShaderMode_Public_Virtual_Final_New_ShaderMode_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ShaderMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual int GetPropertiesCount()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPropertiesCount_Public_Virtual_Final_New_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70976, XrefRangeEnd = 70986, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual int GetMaterialID()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaterialID_Public_Virtual_Final_New_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 71038, RefRangeEnd = 71039, XrefRangeStart = 70986, XrefRangeEnd = 71038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void ApplyToMaterial(Material mat)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyToMaterial_Public_Virtual_Final_New_Void_Material_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<StaticPropertiesHD>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public class MaterialsGroup : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_materials;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		public unsafe Il2CppReferenceArray<Material> materials
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materials);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		static MaterialsGroup()
		{
			Il2CppClassPointerStore<MaterialsGroup>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "MaterialsGroup");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialsGroup>.NativeClassPtr);
			NativeFieldInfoPtr_materials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialsGroup>.NativeClassPtr, "materials");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialsGroup>.NativeClassPtr, 100664045);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71039, XrefRangeEnd = 71044, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MaterialsGroup(int count)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialsGroup>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&count);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MaterialsGroup(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[OriginalName("Assembly-CSharp.dll", "", "ZWrite")]
	public enum ZWrite
	{
		Off,
		On
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_materialPropertyBlock;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlendingMode_SrcFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlendingMode_DstFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlendingMode_AlphaAsBlack;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_MaterialsGroupSD;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_MaterialsGroupHD;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewMaterialPersistent_Public_Static_Material_Shader_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInstancedMaterial_Public_Static_Material_UInt32_byref_StaticPropertiesSD_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInstancedMaterial_Public_Static_Material_UInt32_byref_StaticPropertiesHD_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInstancedMaterial_Private_Static_Material_Hashtable_UInt32_byref_IStaticProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBlendingMode_Private_Static_Void_Material_Int32_BlendMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetStencilRef_Private_Static_Void_Material_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetStencilComp_Private_Static_Void_Material_Int32_CompareFunction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetStencilOp_Private_Static_Void_Material_Int32_StencilOp_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCull_Private_Static_Void_Material_Int32_CullMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetZWrite_Private_Static_Void_Material_Int32_ZWrite_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetZTest_Private_Static_Void_Material_Int32_CompareFunction_0;

	public unsafe static MaterialPropertyBlock materialPropertyBlock
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_materialPropertyBlock, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<MaterialPropertyBlock>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_materialPropertyBlock, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertyBlock));
		}
	}

	public unsafe static Il2CppStructArray<BlendMode> BlendingMode_SrcFactor
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BlendingMode_SrcFactor, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BlendMode>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BlendingMode_SrcFactor, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<BlendMode> BlendingMode_DstFactor
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BlendingMode_DstFactor, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BlendMode>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BlendingMode_DstFactor, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<bool> BlendingMode_AlphaAsBlack
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BlendingMode_AlphaAsBlack, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<bool>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BlendingMode_AlphaAsBlack, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Hashtable ms_MaterialsGroupSD
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_MaterialsGroupSD, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Hashtable>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_MaterialsGroupSD, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashtable));
		}
	}

	public unsafe static Hashtable ms_MaterialsGroupHD
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_MaterialsGroupHD, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Hashtable>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_MaterialsGroupHD, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashtable));
		}
	}

	static MaterialManager()
	{
		Il2CppClassPointerStore<MaterialManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "MaterialManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr);
		NativeFieldInfoPtr_materialPropertyBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "materialPropertyBlock");
		NativeFieldInfoPtr_BlendingMode_SrcFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "BlendingMode_SrcFactor");
		NativeFieldInfoPtr_BlendingMode_DstFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "BlendingMode_DstFactor");
		NativeFieldInfoPtr_BlendingMode_AlphaAsBlack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "BlendingMode_AlphaAsBlack");
		NativeFieldInfoPtr_ms_MaterialsGroupSD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "ms_MaterialsGroupSD");
		NativeFieldInfoPtr_ms_MaterialsGroupHD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, "ms_MaterialsGroupHD");
		NativeMethodInfoPtr_NewMaterialPersistent_Public_Static_Material_Shader_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664005);
		NativeMethodInfoPtr_GetInstancedMaterial_Public_Static_Material_UInt32_byref_StaticPropertiesSD_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664006);
		NativeMethodInfoPtr_GetInstancedMaterial_Public_Static_Material_UInt32_byref_StaticPropertiesHD_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664007);
		NativeMethodInfoPtr_GetInstancedMaterial_Private_Static_Material_Hashtable_UInt32_byref_IStaticProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664008);
		NativeMethodInfoPtr_SetBlendingMode_Private_Static_Void_Material_Int32_BlendMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664009);
		NativeMethodInfoPtr_SetStencilRef_Private_Static_Void_Material_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664010);
		NativeMethodInfoPtr_SetStencilComp_Private_Static_Void_Material_Int32_CompareFunction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664011);
		NativeMethodInfoPtr_SetStencilOp_Private_Static_Void_Material_Int32_StencilOp_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664012);
		NativeMethodInfoPtr_SetCull_Private_Static_Void_Material_Int32_CullMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664013);
		NativeMethodInfoPtr_SetZWrite_Private_Static_Void_Material_Int32_ZWrite_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664014);
		NativeMethodInfoPtr_SetZTest_Private_Static_Void_Material_Int32_CompareFunction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialManager>.NativeClassPtr, 100664015);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71044, XrefRangeEnd = 71055, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material NewMaterialPersistent(Shader shader, bool gpuInstanced)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &gpuInstanced;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewMaterialPersistent_Public_Static_Material_Shader_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71055, XrefRangeEnd = 71063, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material GetInstancedMaterial(uint groupID, ref StaticPropertiesSD staticProps)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&groupID);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref staticProps);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInstancedMaterial_Public_Static_Material_UInt32_byref_StaticPropertiesSD_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71063, XrefRangeEnd = 71071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material GetInstancedMaterial(uint groupID, ref StaticPropertiesHD staticProps)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&groupID);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref staticProps);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInstancedMaterial_Public_Static_Material_UInt32_byref_StaticPropertiesHD_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 71106, RefRangeEnd = 71110, XrefRangeStart = 71071, XrefRangeEnd = 71106, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material GetInstancedMaterial(Hashtable groups, uint groupID, ref IStaticProperties staticProps)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)groups);
		*(uint**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &groupID;
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)staticProps);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInstancedMaterial_Private_Static_Material_Hashtable_UInt32_byref_IStaticProperties_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		System.IntPtr intPtr4 = intPtr;
		staticProps = ((intPtr4 == (System.IntPtr)0) ? null : new IStaticProperties(intPtr4));
		return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71110, XrefRangeEnd = 71158, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetBlendingMode(this Material mat, int nameID, BlendMode value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(BlendMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBlendingMode_Private_Static_Void_Material_Int32_BlendMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetStencilRef(this Material mat, int nameID, int value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStencilRef_Private_Static_Void_Material_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetStencilComp(this Material mat, int nameID, CompareFunction value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(CompareFunction**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStencilComp_Private_Static_Void_Material_Int32_CompareFunction_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetStencilOp(this Material mat, int nameID, StencilOp value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(StencilOp**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStencilOp_Private_Static_Void_Material_Int32_StencilOp_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetCull(this Material mat, int nameID, CullMode value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(CullMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCull_Private_Static_Void_Material_Int32_CullMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetZWrite(this Material mat, int nameID, ZWrite value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(ZWrite**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetZWrite_Private_Static_Void_Material_Int32_ZWrite_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetZTest(this Material mat, int nameID, CompareFunction value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &nameID;
		*(CompareFunction**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetZTest_Private_Static_Void_Material_Int32_CompareFunction_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MaterialManager(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
