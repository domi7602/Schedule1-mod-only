using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppVLB;

public static class ShaderKeywords : Il2CppSystem.Object
{
	public static class SD : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_DepthBlend;

		private static readonly System.IntPtr NativeFieldInfoPtr_OcclusionClippingPlane;

		private static readonly System.IntPtr NativeFieldInfoPtr_OcclusionDepthTexture;

		private static readonly System.IntPtr NativeFieldInfoPtr_MeshSkewing;

		private static readonly System.IntPtr NativeFieldInfoPtr_ShaderAccuracyHigh;

		public unsafe static string DepthBlend
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthBlend, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthBlend, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string OcclusionClippingPlane
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OcclusionClippingPlane, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OcclusionClippingPlane, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string OcclusionDepthTexture
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OcclusionDepthTexture, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OcclusionDepthTexture, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string MeshSkewing
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MeshSkewing, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MeshSkewing, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string ShaderAccuracyHigh
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ShaderAccuracyHigh, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ShaderAccuracyHigh, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static SD()
		{
			Il2CppClassPointerStore<SD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr, "SD");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SD>.NativeClassPtr);
			NativeFieldInfoPtr_DepthBlend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "DepthBlend");
			NativeFieldInfoPtr_OcclusionClippingPlane = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "OcclusionClippingPlane");
			NativeFieldInfoPtr_OcclusionDepthTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "OcclusionDepthTexture");
			NativeFieldInfoPtr_MeshSkewing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "MeshSkewing");
			NativeFieldInfoPtr_ShaderAccuracyHigh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "ShaderAccuracyHigh");
		}

		public SD(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class HD : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationLinear;

		private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationQuad;

		private static readonly System.IntPtr NativeFieldInfoPtr_Shadow;

		private static readonly System.IntPtr NativeFieldInfoPtr_CookieSingleChannel;

		private static readonly System.IntPtr NativeFieldInfoPtr_CookieRGBA;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaymarchingStepCount;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetRaymarchingQuality_Public_Static_String_Int32_0;

		public unsafe static string AttenuationLinear
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationLinear, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationLinear, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string AttenuationQuad
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationQuad, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationQuad, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string Shadow
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Shadow, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Shadow, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string CookieSingleChannel
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CookieSingleChannel, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CookieSingleChannel, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string CookieRGBA
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CookieRGBA, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CookieRGBA, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string RaymarchingStepCount
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaymarchingStepCount, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaymarchingStepCount, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static HD()
		{
			Il2CppClassPointerStore<HD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr, "HD");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HD>.NativeClassPtr);
			NativeFieldInfoPtr_AttenuationLinear = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "AttenuationLinear");
			NativeFieldInfoPtr_AttenuationQuad = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "AttenuationQuad");
			NativeFieldInfoPtr_Shadow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "Shadow");
			NativeFieldInfoPtr_CookieSingleChannel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "CookieSingleChannel");
			NativeFieldInfoPtr_CookieRGBA = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "CookieRGBA");
			NativeFieldInfoPtr_RaymarchingStepCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "RaymarchingStepCount");
			NativeMethodInfoPtr_GetRaymarchingQuality_Public_Static_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HD>.NativeClassPtr, 100664359);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 73746, RefRangeEnd = 73747, XrefRangeStart = 73742, XrefRangeEnd = 73746, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string GetRaymarchingQuality(int id)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&id);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRaymarchingQuality_Public_Static_String_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		public HD(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_AlphaAsBlack;

	private static readonly System.IntPtr NativeFieldInfoPtr_ColorGradientMatrixLow;

	private static readonly System.IntPtr NativeFieldInfoPtr_ColorGradientMatrixHigh;

	private static readonly System.IntPtr NativeFieldInfoPtr_Noise3D;

	public unsafe static string AlphaAsBlack
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AlphaAsBlack, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AlphaAsBlack, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string ColorGradientMatrixLow
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ColorGradientMatrixLow, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ColorGradientMatrixLow, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string ColorGradientMatrixHigh
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ColorGradientMatrixHigh, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ColorGradientMatrixHigh, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string Noise3D
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Noise3D, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Noise3D, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ShaderKeywords()
	{
		Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "ShaderKeywords");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr);
		NativeFieldInfoPtr_AlphaAsBlack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr, "AlphaAsBlack");
		NativeFieldInfoPtr_ColorGradientMatrixLow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr, "ColorGradientMatrixLow");
		NativeFieldInfoPtr_ColorGradientMatrixHigh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr, "ColorGradientMatrixHigh");
		NativeFieldInfoPtr_Noise3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderKeywords>.NativeClassPtr, "Noise3D");
	}

	public ShaderKeywords(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
