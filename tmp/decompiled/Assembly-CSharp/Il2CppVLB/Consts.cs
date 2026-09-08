using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class Consts : Il2CppSystem.Object
{
	public static class Help : Il2CppSystem.Object
	{
		public static class SD : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_UrlBeam;

			private static readonly System.IntPtr NativeFieldInfoPtr_UrlDynamicOcclusionRaycasting;

			private static readonly System.IntPtr NativeFieldInfoPtr_UrlDynamicOcclusionDepthBuffer;

			private static readonly System.IntPtr NativeFieldInfoPtr_UrlSkewingHandle;

			public unsafe static string UrlBeam
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlBeam, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlBeam, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			public unsafe static string UrlDynamicOcclusionRaycasting
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlDynamicOcclusionRaycasting, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlDynamicOcclusionRaycasting, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			public unsafe static string UrlDynamicOcclusionDepthBuffer
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlDynamicOcclusionDepthBuffer, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlDynamicOcclusionDepthBuffer, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			public unsafe static string UrlSkewingHandle
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlSkewingHandle, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlSkewingHandle, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			static SD()
			{
				Il2CppClassPointerStore<SD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Help>.NativeClassPtr, "SD");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SD>.NativeClassPtr);
				NativeFieldInfoPtr_UrlBeam = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "UrlBeam");
				NativeFieldInfoPtr_UrlDynamicOcclusionRaycasting = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "UrlDynamicOcclusionRaycasting");
				NativeFieldInfoPtr_UrlDynamicOcclusionDepthBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "UrlDynamicOcclusionDepthBuffer");
				NativeFieldInfoPtr_UrlSkewingHandle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "UrlSkewingHandle");
			}

			public SD(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		public static class HD : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_UrlBeam;

			private static readonly System.IntPtr NativeFieldInfoPtr_UrlShadow;

			private static readonly System.IntPtr NativeFieldInfoPtr_UrlCookie;

			private static readonly System.IntPtr NativeFieldInfoPtr_UrlTrackRealtimeChangesOnLight;

			public unsafe static string UrlBeam
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlBeam, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlBeam, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			public unsafe static string UrlShadow
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlShadow, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlShadow, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			public unsafe static string UrlCookie
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlCookie, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlCookie, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			public unsafe static string UrlTrackRealtimeChangesOnLight
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlTrackRealtimeChangesOnLight, (void*)(&intPtr));
					return IL2CPP.Il2CppStringToManaged(intPtr);
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlTrackRealtimeChangesOnLight, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
				}
			}

			static HD()
			{
				Il2CppClassPointerStore<HD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Help>.NativeClassPtr, "HD");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HD>.NativeClassPtr);
				NativeFieldInfoPtr_UrlBeam = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "UrlBeam");
				NativeFieldInfoPtr_UrlShadow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "UrlShadow");
				NativeFieldInfoPtr_UrlCookie = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "UrlCookie");
				NativeFieldInfoPtr_UrlTrackRealtimeChangesOnLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "UrlTrackRealtimeChangesOnLight");
			}

			public HD(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlBase;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlSuffix;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlDustParticles;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlTriggerZone;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlEffectFlicker;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlEffectPulse;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlEffectFromProfile;

		private static readonly System.IntPtr NativeFieldInfoPtr_UrlConfig;

		public unsafe static string UrlBase
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlBase, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlBase, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlSuffix
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlSuffix, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlSuffix, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlDustParticles
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlDustParticles, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlDustParticles, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlTriggerZone
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlTriggerZone, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlTriggerZone, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlEffectFlicker
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlEffectFlicker, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlEffectFlicker, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlEffectPulse
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlEffectPulse, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlEffectPulse, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlEffectFromProfile
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlEffectFromProfile, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlEffectFromProfile, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string UrlConfig
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UrlConfig, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UrlConfig, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static Help()
		{
			Il2CppClassPointerStore<Help>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Help");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Help>.NativeClassPtr);
			NativeFieldInfoPtr_UrlBase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlBase");
			NativeFieldInfoPtr_UrlSuffix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlSuffix");
			NativeFieldInfoPtr_UrlDustParticles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlDustParticles");
			NativeFieldInfoPtr_UrlTriggerZone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlTriggerZone");
			NativeFieldInfoPtr_UrlEffectFlicker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlEffectFlicker");
			NativeFieldInfoPtr_UrlEffectPulse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlEffectPulse");
			NativeFieldInfoPtr_UrlEffectFromProfile = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlEffectFromProfile");
			NativeFieldInfoPtr_UrlConfig = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Help>.NativeClassPtr, "UrlConfig");
		}

		public Help(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class Internal : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ProceduralObjectsVisibleInEditor;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_ProceduralObjectsHideFlags_Public_Static_get_HideFlags_0;

		public unsafe static bool ProceduralObjectsVisibleInEditor
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ProceduralObjectsVisibleInEditor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ProceduralObjectsVisibleInEditor, (void*)(&flag));
			}
		}

		public unsafe static HideFlags ProceduralObjectsHideFlags
		{
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 69365, RefRangeEnd = 69368, XrefRangeStart = 69361, XrefRangeEnd = 69365, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ProceduralObjectsHideFlags_Public_Static_get_HideFlags_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(HideFlags*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static Internal()
		{
			Il2CppClassPointerStore<Internal>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Internal");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Internal>.NativeClassPtr);
			NativeFieldInfoPtr_ProceduralObjectsVisibleInEditor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Internal>.NativeClassPtr, "ProceduralObjectsVisibleInEditor");
			NativeMethodInfoPtr_get_ProceduralObjectsHideFlags_Public_Static_get_HideFlags_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Internal>.NativeClassPtr, 100663730);
		}

		public Internal(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class Beam : Il2CppSystem.Object
	{
		public static class SD : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_FresnelPowMaxValue;

			private static readonly System.IntPtr NativeFieldInfoPtr_FresnelPow;

			private static readonly System.IntPtr NativeFieldInfoPtr_GlareFrontalDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_GlareBehindDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_GlareMin;

			private static readonly System.IntPtr NativeFieldInfoPtr_GlareMax;

			private static readonly System.IntPtr NativeFieldInfoPtr_TiltDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_SkewingLocalForwardDirectionDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_ClippingPlaneTransformDefault;

			public unsafe static float FresnelPowMaxValue
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FresnelPowMaxValue, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FresnelPowMaxValue, (void*)(&num));
				}
			}

			public unsafe static float FresnelPow
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FresnelPow, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FresnelPow, (void*)(&num));
				}
			}

			public unsafe static float GlareFrontalDefault
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GlareFrontalDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GlareFrontalDefault, (void*)(&num));
				}
			}

			public unsafe static float GlareBehindDefault
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GlareBehindDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GlareBehindDefault, (void*)(&num));
				}
			}

			public unsafe static float GlareMin
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GlareMin, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GlareMin, (void*)(&num));
				}
			}

			public unsafe static float GlareMax
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GlareMax, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GlareMax, (void*)(&num));
				}
			}

			public unsafe static Vector2 TiltDefault
			{
				get
				{
					Unsafe.SkipInit(out Vector2 result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TiltDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TiltDefault, (void*)(&vector));
				}
			}

			public unsafe static Vector3 SkewingLocalForwardDirectionDefault
			{
				get
				{
					Unsafe.SkipInit(out Vector3 result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SkewingLocalForwardDirectionDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SkewingLocalForwardDirectionDefault, (void*)(&vector));
				}
			}

			public unsafe static Transform ClippingPlaneTransformDefault
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ClippingPlaneTransformDefault, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ClippingPlaneTransformDefault, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
				}
			}

			static SD()
			{
				Il2CppClassPointerStore<SD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Beam>.NativeClassPtr, "SD");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SD>.NativeClassPtr);
				NativeFieldInfoPtr_FresnelPowMaxValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "FresnelPowMaxValue");
				NativeFieldInfoPtr_FresnelPow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "FresnelPow");
				NativeFieldInfoPtr_GlareFrontalDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "GlareFrontalDefault");
				NativeFieldInfoPtr_GlareBehindDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "GlareBehindDefault");
				NativeFieldInfoPtr_GlareMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "GlareMin");
				NativeFieldInfoPtr_GlareMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "GlareMax");
				NativeFieldInfoPtr_TiltDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "TiltDefault");
				NativeFieldInfoPtr_SkewingLocalForwardDirectionDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "SkewingLocalForwardDirectionDefault");
				NativeFieldInfoPtr_ClippingPlaneTransformDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SD>.NativeClassPtr, "ClippingPlaneTransformDefault");
			}

			public SD(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		public static class HD : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationEquationDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_SideSoftnessDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_SideSoftnessMin;

			private static readonly System.IntPtr NativeFieldInfoPtr_SideSoftnessMax;

			private static readonly System.IntPtr NativeFieldInfoPtr_JitteringFactorDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_JitteringFactorMin;

			private static readonly System.IntPtr NativeFieldInfoPtr_JitteringFrameRateDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_JitteringFrameRateMin;

			private static readonly System.IntPtr NativeFieldInfoPtr_JitteringFrameRateMax;

			private static readonly System.IntPtr NativeFieldInfoPtr_JitteringLerpRange;

			public unsafe static AttenuationEquationHD AttenuationEquationDefault
			{
				get
				{
					Unsafe.SkipInit(out AttenuationEquationHD result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationEquationDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationEquationDefault, (void*)(&attenuationEquationHD));
				}
			}

			public unsafe static float SideSoftnessDefault
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SideSoftnessDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SideSoftnessDefault, (void*)(&num));
				}
			}

			public unsafe static float SideSoftnessMin
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SideSoftnessMin, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SideSoftnessMin, (void*)(&num));
				}
			}

			public unsafe static float SideSoftnessMax
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SideSoftnessMax, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SideSoftnessMax, (void*)(&num));
				}
			}

			public unsafe static float JitteringFactorDefault
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JitteringFactorDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JitteringFactorDefault, (void*)(&num));
				}
			}

			public unsafe static float JitteringFactorMin
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JitteringFactorMin, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JitteringFactorMin, (void*)(&num));
				}
			}

			public unsafe static int JitteringFrameRateDefault
			{
				get
				{
					Unsafe.SkipInit(out int result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JitteringFrameRateDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JitteringFrameRateDefault, (void*)(&num));
				}
			}

			public unsafe static int JitteringFrameRateMin
			{
				get
				{
					Unsafe.SkipInit(out int result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JitteringFrameRateMin, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JitteringFrameRateMin, (void*)(&num));
				}
			}

			public unsafe static int JitteringFrameRateMax
			{
				get
				{
					Unsafe.SkipInit(out int result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JitteringFrameRateMax, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JitteringFrameRateMax, (void*)(&num));
				}
			}

			public unsafe static MinMaxRangeFloat JitteringLerpRange
			{
				get
				{
					Unsafe.SkipInit(out MinMaxRangeFloat result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JitteringLerpRange, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JitteringLerpRange, (void*)(&minMaxRangeFloat));
				}
			}

			static HD()
			{
				Il2CppClassPointerStore<HD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Beam>.NativeClassPtr, "HD");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HD>.NativeClassPtr);
				NativeFieldInfoPtr_AttenuationEquationDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "AttenuationEquationDefault");
				NativeFieldInfoPtr_SideSoftnessDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "SideSoftnessDefault");
				NativeFieldInfoPtr_SideSoftnessMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "SideSoftnessMin");
				NativeFieldInfoPtr_SideSoftnessMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "SideSoftnessMax");
				NativeFieldInfoPtr_JitteringFactorDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "JitteringFactorDefault");
				NativeFieldInfoPtr_JitteringFactorMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "JitteringFactorMin");
				NativeFieldInfoPtr_JitteringFrameRateDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "JitteringFrameRateDefault");
				NativeFieldInfoPtr_JitteringFrameRateMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "JitteringFrameRateMin");
				NativeFieldInfoPtr_JitteringFrameRateMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "JitteringFrameRateMax");
				NativeFieldInfoPtr_JitteringLerpRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "JitteringLerpRange");
			}

			public HD(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_FlatColor;

		private static readonly System.IntPtr NativeFieldInfoPtr_ColorModeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_MultiplierDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_MultiplierMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_IntensityDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_IntensityMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_HDRPExposureWeightDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_HDRPExposureWeightMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_HDRPExposureWeightMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_SpotAngleDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_SpotAngleMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_SpotAngleMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_ConeRadiusStart;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomMeshType;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomSidesDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomSidesMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomSidesMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomSegmentsDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomSegmentsMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomSegmentsMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeomCap;

		private static readonly System.IntPtr NativeFieldInfoPtr_ScalableDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationEquationDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationCustomBlendingDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationCustomBlendingMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_AttenuationCustomBlendingMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_FallOffStart;

		private static readonly System.IntPtr NativeFieldInfoPtr_FallOffEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_FallOffDistancesMinThreshold;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthBlendDistance;

		private static readonly System.IntPtr NativeFieldInfoPtr_CameraClippingDistance;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseModeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseIntensityMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseIntensityMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseIntensityDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseScaleMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseScaleMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseScaleDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseVelocityDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_BlendingModeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_ShaderAccuracyDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FadeOutBeginDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FadeOutEndDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DimensionsDefault;

		public unsafe static Color FlatColor
		{
			get
			{
				Unsafe.SkipInit(out Color result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FlatColor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FlatColor, (void*)(&color));
			}
		}

		public unsafe static ColorMode ColorModeDefault
		{
			get
			{
				Unsafe.SkipInit(out ColorMode result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ColorModeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ColorModeDefault, (void*)(&colorMode));
			}
		}

		public unsafe static float MultiplierDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MultiplierDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MultiplierDefault, (void*)(&num));
			}
		}

		public unsafe static float MultiplierMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MultiplierMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MultiplierMin, (void*)(&num));
			}
		}

		public unsafe static float IntensityDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IntensityDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IntensityDefault, (void*)(&num));
			}
		}

		public unsafe static float IntensityMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IntensityMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IntensityMin, (void*)(&num));
			}
		}

		public unsafe static float HDRPExposureWeightDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HDRPExposureWeightDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HDRPExposureWeightDefault, (void*)(&num));
			}
		}

		public unsafe static float HDRPExposureWeightMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HDRPExposureWeightMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HDRPExposureWeightMin, (void*)(&num));
			}
		}

		public unsafe static float HDRPExposureWeightMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HDRPExposureWeightMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HDRPExposureWeightMax, (void*)(&num));
			}
		}

		public unsafe static float SpotAngleDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SpotAngleDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SpotAngleDefault, (void*)(&num));
			}
		}

		public unsafe static float SpotAngleMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SpotAngleMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SpotAngleMin, (void*)(&num));
			}
		}

		public unsafe static float SpotAngleMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SpotAngleMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SpotAngleMax, (void*)(&num));
			}
		}

		public unsafe static float ConeRadiusStart
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ConeRadiusStart, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ConeRadiusStart, (void*)(&num));
			}
		}

		public unsafe static MeshType GeomMeshType
		{
			get
			{
				Unsafe.SkipInit(out MeshType result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomMeshType, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomMeshType, (void*)(&meshType));
			}
		}

		public unsafe static int GeomSidesDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomSidesDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomSidesDefault, (void*)(&num));
			}
		}

		public unsafe static int GeomSidesMin
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomSidesMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomSidesMin, (void*)(&num));
			}
		}

		public unsafe static int GeomSidesMax
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomSidesMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomSidesMax, (void*)(&num));
			}
		}

		public unsafe static int GeomSegmentsDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomSegmentsDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomSegmentsDefault, (void*)(&num));
			}
		}

		public unsafe static int GeomSegmentsMin
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomSegmentsMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomSegmentsMin, (void*)(&num));
			}
		}

		public unsafe static int GeomSegmentsMax
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomSegmentsMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomSegmentsMax, (void*)(&num));
			}
		}

		public unsafe static bool GeomCap
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeomCap, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeomCap, (void*)(&flag));
			}
		}

		public unsafe static bool ScalableDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ScalableDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ScalableDefault, (void*)(&flag));
			}
		}

		public unsafe static AttenuationEquation AttenuationEquationDefault
		{
			get
			{
				Unsafe.SkipInit(out AttenuationEquation result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationEquationDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationEquationDefault, (void*)(&attenuationEquation));
			}
		}

		public unsafe static float AttenuationCustomBlendingDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationCustomBlendingDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationCustomBlendingDefault, (void*)(&num));
			}
		}

		public unsafe static float AttenuationCustomBlendingMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationCustomBlendingMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationCustomBlendingMin, (void*)(&num));
			}
		}

		public unsafe static float AttenuationCustomBlendingMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttenuationCustomBlendingMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttenuationCustomBlendingMax, (void*)(&num));
			}
		}

		public unsafe static float FallOffStart
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FallOffStart, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FallOffStart, (void*)(&num));
			}
		}

		public unsafe static float FallOffEnd
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FallOffEnd, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FallOffEnd, (void*)(&num));
			}
		}

		public unsafe static float FallOffDistancesMinThreshold
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FallOffDistancesMinThreshold, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FallOffDistancesMinThreshold, (void*)(&num));
			}
		}

		public unsafe static float DepthBlendDistance
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthBlendDistance, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthBlendDistance, (void*)(&num));
			}
		}

		public unsafe static float CameraClippingDistance
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CameraClippingDistance, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CameraClippingDistance, (void*)(&num));
			}
		}

		public unsafe static NoiseMode NoiseModeDefault
		{
			get
			{
				Unsafe.SkipInit(out NoiseMode result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseModeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseModeDefault, (void*)(&noiseMode));
			}
		}

		public unsafe static float NoiseIntensityMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseIntensityMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseIntensityMin, (void*)(&num));
			}
		}

		public unsafe static float NoiseIntensityMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseIntensityMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseIntensityMax, (void*)(&num));
			}
		}

		public unsafe static float NoiseIntensityDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseIntensityDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseIntensityDefault, (void*)(&num));
			}
		}

		public unsafe static float NoiseScaleMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseScaleMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseScaleMin, (void*)(&num));
			}
		}

		public unsafe static float NoiseScaleMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseScaleMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseScaleMax, (void*)(&num));
			}
		}

		public unsafe static float NoiseScaleDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseScaleDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseScaleDefault, (void*)(&num));
			}
		}

		public unsafe static Vector3 NoiseVelocityDefault
		{
			get
			{
				Unsafe.SkipInit(out Vector3 result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseVelocityDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseVelocityDefault, (void*)(&vector));
			}
		}

		public unsafe static BlendingMode BlendingModeDefault
		{
			get
			{
				Unsafe.SkipInit(out BlendingMode result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BlendingModeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BlendingModeDefault, (void*)(&blendingMode));
			}
		}

		public unsafe static ShaderAccuracy ShaderAccuracyDefault
		{
			get
			{
				Unsafe.SkipInit(out ShaderAccuracy result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ShaderAccuracyDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ShaderAccuracyDefault, (void*)(&shaderAccuracy));
			}
		}

		public unsafe static float FadeOutBeginDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FadeOutBeginDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FadeOutBeginDefault, (void*)(&num));
			}
		}

		public unsafe static float FadeOutEndDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FadeOutEndDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FadeOutEndDefault, (void*)(&num));
			}
		}

		public unsafe static Dimensions DimensionsDefault
		{
			get
			{
				Unsafe.SkipInit(out Dimensions result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DimensionsDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DimensionsDefault, (void*)(&dimensions));
			}
		}

		static Beam()
		{
			Il2CppClassPointerStore<Beam>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Beam");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Beam>.NativeClassPtr);
			NativeFieldInfoPtr_FlatColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "FlatColor");
			NativeFieldInfoPtr_ColorModeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "ColorModeDefault");
			NativeFieldInfoPtr_MultiplierDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "MultiplierDefault");
			NativeFieldInfoPtr_MultiplierMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "MultiplierMin");
			NativeFieldInfoPtr_IntensityDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "IntensityDefault");
			NativeFieldInfoPtr_IntensityMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "IntensityMin");
			NativeFieldInfoPtr_HDRPExposureWeightDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "HDRPExposureWeightDefault");
			NativeFieldInfoPtr_HDRPExposureWeightMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "HDRPExposureWeightMin");
			NativeFieldInfoPtr_HDRPExposureWeightMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "HDRPExposureWeightMax");
			NativeFieldInfoPtr_SpotAngleDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "SpotAngleDefault");
			NativeFieldInfoPtr_SpotAngleMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "SpotAngleMin");
			NativeFieldInfoPtr_SpotAngleMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "SpotAngleMax");
			NativeFieldInfoPtr_ConeRadiusStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "ConeRadiusStart");
			NativeFieldInfoPtr_GeomMeshType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomMeshType");
			NativeFieldInfoPtr_GeomSidesDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomSidesDefault");
			NativeFieldInfoPtr_GeomSidesMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomSidesMin");
			NativeFieldInfoPtr_GeomSidesMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomSidesMax");
			NativeFieldInfoPtr_GeomSegmentsDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomSegmentsDefault");
			NativeFieldInfoPtr_GeomSegmentsMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomSegmentsMin");
			NativeFieldInfoPtr_GeomSegmentsMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomSegmentsMax");
			NativeFieldInfoPtr_GeomCap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "GeomCap");
			NativeFieldInfoPtr_ScalableDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "ScalableDefault");
			NativeFieldInfoPtr_AttenuationEquationDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "AttenuationEquationDefault");
			NativeFieldInfoPtr_AttenuationCustomBlendingDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "AttenuationCustomBlendingDefault");
			NativeFieldInfoPtr_AttenuationCustomBlendingMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "AttenuationCustomBlendingMin");
			NativeFieldInfoPtr_AttenuationCustomBlendingMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "AttenuationCustomBlendingMax");
			NativeFieldInfoPtr_FallOffStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "FallOffStart");
			NativeFieldInfoPtr_FallOffEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "FallOffEnd");
			NativeFieldInfoPtr_FallOffDistancesMinThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "FallOffDistancesMinThreshold");
			NativeFieldInfoPtr_DepthBlendDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "DepthBlendDistance");
			NativeFieldInfoPtr_CameraClippingDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "CameraClippingDistance");
			NativeFieldInfoPtr_NoiseModeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseModeDefault");
			NativeFieldInfoPtr_NoiseIntensityMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseIntensityMin");
			NativeFieldInfoPtr_NoiseIntensityMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseIntensityMax");
			NativeFieldInfoPtr_NoiseIntensityDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseIntensityDefault");
			NativeFieldInfoPtr_NoiseScaleMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseScaleMin");
			NativeFieldInfoPtr_NoiseScaleMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseScaleMax");
			NativeFieldInfoPtr_NoiseScaleDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseScaleDefault");
			NativeFieldInfoPtr_NoiseVelocityDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "NoiseVelocityDefault");
			NativeFieldInfoPtr_BlendingModeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "BlendingModeDefault");
			NativeFieldInfoPtr_ShaderAccuracyDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "ShaderAccuracyDefault");
			NativeFieldInfoPtr_FadeOutBeginDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "FadeOutBeginDefault");
			NativeFieldInfoPtr_FadeOutEndDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "FadeOutEndDefault");
			NativeFieldInfoPtr_DimensionsDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Beam>.NativeClassPtr, "DimensionsDefault");
		}

		public Beam(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class DustParticles : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_AlphaDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_SizeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DirectionDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_VelocityDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DensityDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DensityMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_DensityMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_SpawnDistanceRangeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_CullingEnabledDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_CullingMaxDistanceDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_CullingMaxDistanceMin;

		public unsafe static float AlphaDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AlphaDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AlphaDefault, (void*)(&num));
			}
		}

		public unsafe static float SizeDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SizeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SizeDefault, (void*)(&num));
			}
		}

		public unsafe static ParticlesDirection DirectionDefault
		{
			get
			{
				Unsafe.SkipInit(out ParticlesDirection result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DirectionDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DirectionDefault, (void*)(&particlesDirection));
			}
		}

		public unsafe static Vector3 VelocityDefault
		{
			get
			{
				Unsafe.SkipInit(out Vector3 result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_VelocityDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_VelocityDefault, (void*)(&vector));
			}
		}

		public unsafe static float DensityDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DensityDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DensityDefault, (void*)(&num));
			}
		}

		public unsafe static float DensityMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DensityMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DensityMin, (void*)(&num));
			}
		}

		public unsafe static float DensityMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DensityMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DensityMax, (void*)(&num));
			}
		}

		public unsafe static MinMaxRangeFloat SpawnDistanceRangeDefault
		{
			get
			{
				Unsafe.SkipInit(out MinMaxRangeFloat result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SpawnDistanceRangeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SpawnDistanceRangeDefault, (void*)(&minMaxRangeFloat));
			}
		}

		public unsafe static bool CullingEnabledDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullingEnabledDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullingEnabledDefault, (void*)(&flag));
			}
		}

		public unsafe static float CullingMaxDistanceDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullingMaxDistanceDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullingMaxDistanceDefault, (void*)(&num));
			}
		}

		public unsafe static float CullingMaxDistanceMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullingMaxDistanceMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullingMaxDistanceMin, (void*)(&num));
			}
		}

		static DustParticles()
		{
			Il2CppClassPointerStore<DustParticles>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "DustParticles");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DustParticles>.NativeClassPtr);
			NativeFieldInfoPtr_AlphaDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "AlphaDefault");
			NativeFieldInfoPtr_SizeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "SizeDefault");
			NativeFieldInfoPtr_DirectionDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "DirectionDefault");
			NativeFieldInfoPtr_VelocityDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "VelocityDefault");
			NativeFieldInfoPtr_DensityDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "DensityDefault");
			NativeFieldInfoPtr_DensityMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "DensityMin");
			NativeFieldInfoPtr_DensityMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "DensityMax");
			NativeFieldInfoPtr_SpawnDistanceRangeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "SpawnDistanceRangeDefault");
			NativeFieldInfoPtr_CullingEnabledDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "CullingEnabledDefault");
			NativeFieldInfoPtr_CullingMaxDistanceDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "CullingMaxDistanceDefault");
			NativeFieldInfoPtr_CullingMaxDistanceMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DustParticles>.NativeClassPtr, "CullingMaxDistanceMin");
		}

		public DustParticles(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class DynOcclusion : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_LayerMaskDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_UpdateRateDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_WaitFramesCountDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingDimensionsDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingConsiderTriggersDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMinOccluderAreaDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMinSurfaceRatioDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMinSurfaceRatioMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMinSurfaceRatioMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMaxSurfaceDotDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingPlaneAlignmentDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingPlaneOffsetDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RaycastingFadeDistanceToSurfaceDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthBufferDepthMapResolutionDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthBufferOcclusionCullingDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthBufferFadeDistanceToSurfaceDefault;

		public unsafe static LayerMask LayerMaskDefault
		{
			get
			{
				Unsafe.SkipInit(out LayerMask result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LayerMaskDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LayerMaskDefault, (void*)(&layerMask));
			}
		}

		public unsafe static DynamicOcclusionUpdateRate UpdateRateDefault
		{
			get
			{
				Unsafe.SkipInit(out DynamicOcclusionUpdateRate result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UpdateRateDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UpdateRateDefault, (void*)(&dynamicOcclusionUpdateRate));
			}
		}

		public unsafe static int WaitFramesCountDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WaitFramesCountDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WaitFramesCountDefault, (void*)(&num));
			}
		}

		public unsafe static Dimensions RaycastingDimensionsDefault
		{
			get
			{
				Unsafe.SkipInit(out Dimensions result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingDimensionsDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingDimensionsDefault, (void*)(&dimensions));
			}
		}

		public unsafe static bool RaycastingConsiderTriggersDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingConsiderTriggersDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingConsiderTriggersDefault, (void*)(&flag));
			}
		}

		public unsafe static float RaycastingMinOccluderAreaDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMinOccluderAreaDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMinOccluderAreaDefault, (void*)(&num));
			}
		}

		public unsafe static float RaycastingMinSurfaceRatioDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMinSurfaceRatioDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMinSurfaceRatioDefault, (void*)(&num));
			}
		}

		public unsafe static float RaycastingMinSurfaceRatioMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMinSurfaceRatioMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMinSurfaceRatioMin, (void*)(&num));
			}
		}

		public unsafe static float RaycastingMinSurfaceRatioMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMinSurfaceRatioMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMinSurfaceRatioMax, (void*)(&num));
			}
		}

		public unsafe static float RaycastingMaxSurfaceDotDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMaxSurfaceDotDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMaxSurfaceDotDefault, (void*)(&num));
			}
		}

		public unsafe static float RaycastingMaxSurfaceAngleMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMin, (void*)(&num));
			}
		}

		public unsafe static float RaycastingMaxSurfaceAngleMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMax, (void*)(&num));
			}
		}

		public unsafe static PlaneAlignment RaycastingPlaneAlignmentDefault
		{
			get
			{
				Unsafe.SkipInit(out PlaneAlignment result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingPlaneAlignmentDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingPlaneAlignmentDefault, (void*)(&planeAlignment));
			}
		}

		public unsafe static float RaycastingPlaneOffsetDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingPlaneOffsetDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingPlaneOffsetDefault, (void*)(&num));
			}
		}

		public unsafe static float RaycastingFadeDistanceToSurfaceDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaycastingFadeDistanceToSurfaceDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaycastingFadeDistanceToSurfaceDefault, (void*)(&num));
			}
		}

		public unsafe static int DepthBufferDepthMapResolutionDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthBufferDepthMapResolutionDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthBufferDepthMapResolutionDefault, (void*)(&num));
			}
		}

		public unsafe static bool DepthBufferOcclusionCullingDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthBufferOcclusionCullingDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthBufferOcclusionCullingDefault, (void*)(&flag));
			}
		}

		public unsafe static float DepthBufferFadeDistanceToSurfaceDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthBufferFadeDistanceToSurfaceDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthBufferFadeDistanceToSurfaceDefault, (void*)(&num));
			}
		}

		static DynOcclusion()
		{
			Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "DynOcclusion");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr);
			NativeFieldInfoPtr_LayerMaskDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "LayerMaskDefault");
			NativeFieldInfoPtr_UpdateRateDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "UpdateRateDefault");
			NativeFieldInfoPtr_WaitFramesCountDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "WaitFramesCountDefault");
			NativeFieldInfoPtr_RaycastingDimensionsDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingDimensionsDefault");
			NativeFieldInfoPtr_RaycastingConsiderTriggersDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingConsiderTriggersDefault");
			NativeFieldInfoPtr_RaycastingMinOccluderAreaDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMinOccluderAreaDefault");
			NativeFieldInfoPtr_RaycastingMinSurfaceRatioDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMinSurfaceRatioDefault");
			NativeFieldInfoPtr_RaycastingMinSurfaceRatioMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMinSurfaceRatioMin");
			NativeFieldInfoPtr_RaycastingMinSurfaceRatioMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMinSurfaceRatioMax");
			NativeFieldInfoPtr_RaycastingMaxSurfaceDotDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMaxSurfaceDotDefault");
			NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMaxSurfaceAngleMin");
			NativeFieldInfoPtr_RaycastingMaxSurfaceAngleMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingMaxSurfaceAngleMax");
			NativeFieldInfoPtr_RaycastingPlaneAlignmentDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingPlaneAlignmentDefault");
			NativeFieldInfoPtr_RaycastingPlaneOffsetDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingPlaneOffsetDefault");
			NativeFieldInfoPtr_RaycastingFadeDistanceToSurfaceDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "RaycastingFadeDistanceToSurfaceDefault");
			NativeFieldInfoPtr_DepthBufferDepthMapResolutionDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "DepthBufferDepthMapResolutionDefault");
			NativeFieldInfoPtr_DepthBufferOcclusionCullingDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "DepthBufferOcclusionCullingDefault");
			NativeFieldInfoPtr_DepthBufferFadeDistanceToSurfaceDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynOcclusion>.NativeClassPtr, "DepthBufferFadeDistanceToSurfaceDefault");
		}

		public DynOcclusion(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class Effects : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ComponentsToChangeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RestoreIntensityOnDisableDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FrequencyDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_PerformPausesDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RestoreIntensityOnPauseDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FlickeringDurationDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_PauseDurationDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_IntensityAmplitudeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_SmoothingDefault;

		public unsafe static EffectAbstractBase.ComponentsToChange ComponentsToChangeDefault
		{
			get
			{
				Unsafe.SkipInit(out EffectAbstractBase.ComponentsToChange result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ComponentsToChangeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ComponentsToChangeDefault, (void*)(&componentsToChange));
			}
		}

		public unsafe static bool RestoreIntensityOnDisableDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RestoreIntensityOnDisableDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RestoreIntensityOnDisableDefault, (void*)(&flag));
			}
		}

		public unsafe static float FrequencyDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FrequencyDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FrequencyDefault, (void*)(&num));
			}
		}

		public unsafe static bool PerformPausesDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PerformPausesDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PerformPausesDefault, (void*)(&flag));
			}
		}

		public unsafe static bool RestoreIntensityOnPauseDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RestoreIntensityOnPauseDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RestoreIntensityOnPauseDefault, (void*)(&flag));
			}
		}

		public unsafe static MinMaxRangeFloat FlickeringDurationDefault
		{
			get
			{
				Unsafe.SkipInit(out MinMaxRangeFloat result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FlickeringDurationDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FlickeringDurationDefault, (void*)(&minMaxRangeFloat));
			}
		}

		public unsafe static MinMaxRangeFloat PauseDurationDefault
		{
			get
			{
				Unsafe.SkipInit(out MinMaxRangeFloat result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PauseDurationDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PauseDurationDefault, (void*)(&minMaxRangeFloat));
			}
		}

		public unsafe static MinMaxRangeFloat IntensityAmplitudeDefault
		{
			get
			{
				Unsafe.SkipInit(out MinMaxRangeFloat result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IntensityAmplitudeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IntensityAmplitudeDefault, (void*)(&minMaxRangeFloat));
			}
		}

		public unsafe static float SmoothingDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmoothingDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmoothingDefault, (void*)(&num));
			}
		}

		static Effects()
		{
			Il2CppClassPointerStore<Effects>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Effects");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Effects>.NativeClassPtr);
			NativeFieldInfoPtr_ComponentsToChangeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "ComponentsToChangeDefault");
			NativeFieldInfoPtr_RestoreIntensityOnDisableDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "RestoreIntensityOnDisableDefault");
			NativeFieldInfoPtr_FrequencyDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "FrequencyDefault");
			NativeFieldInfoPtr_PerformPausesDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "PerformPausesDefault");
			NativeFieldInfoPtr_RestoreIntensityOnPauseDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "RestoreIntensityOnPauseDefault");
			NativeFieldInfoPtr_FlickeringDurationDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "FlickeringDurationDefault");
			NativeFieldInfoPtr_PauseDurationDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "PauseDurationDefault");
			NativeFieldInfoPtr_IntensityAmplitudeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "IntensityAmplitudeDefault");
			NativeFieldInfoPtr_SmoothingDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Effects>.NativeClassPtr, "SmoothingDefault");
		}

		public Effects(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class Shadow : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_StrengthDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_StrengthMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_StrengthMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_LayerMaskDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_UpdateRateDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_WaitFramesCountDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthMapResolutionDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_OcclusionCullingDefault;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetErrorChangeRuntimeDepthMapResolution_Public_Static_String_VolumetricShadowHD_0;

		public unsafe static float StrengthDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrengthDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrengthDefault, (void*)(&num));
			}
		}

		public unsafe static float StrengthMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrengthMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrengthMin, (void*)(&num));
			}
		}

		public unsafe static float StrengthMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrengthMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrengthMax, (void*)(&num));
			}
		}

		public unsafe static LayerMask LayerMaskDefault
		{
			get
			{
				Unsafe.SkipInit(out LayerMask result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LayerMaskDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LayerMaskDefault, (void*)(&layerMask));
			}
		}

		public unsafe static ShadowUpdateRate UpdateRateDefault
		{
			get
			{
				Unsafe.SkipInit(out ShadowUpdateRate result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UpdateRateDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UpdateRateDefault, (void*)(&shadowUpdateRate));
			}
		}

		public unsafe static int WaitFramesCountDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WaitFramesCountDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WaitFramesCountDefault, (void*)(&num));
			}
		}

		public unsafe static int DepthMapResolutionDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthMapResolutionDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthMapResolutionDefault, (void*)(&num));
			}
		}

		public unsafe static bool OcclusionCullingDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OcclusionCullingDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OcclusionCullingDefault, (void*)(&flag));
			}
		}

		static Shadow()
		{
			Il2CppClassPointerStore<Shadow>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Shadow");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Shadow>.NativeClassPtr);
			NativeFieldInfoPtr_StrengthDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "StrengthDefault");
			NativeFieldInfoPtr_StrengthMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "StrengthMin");
			NativeFieldInfoPtr_StrengthMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "StrengthMax");
			NativeFieldInfoPtr_LayerMaskDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "LayerMaskDefault");
			NativeFieldInfoPtr_UpdateRateDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "UpdateRateDefault");
			NativeFieldInfoPtr_WaitFramesCountDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "WaitFramesCountDefault");
			NativeFieldInfoPtr_DepthMapResolutionDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "DepthMapResolutionDefault");
			NativeFieldInfoPtr_OcclusionCullingDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shadow>.NativeClassPtr, "OcclusionCullingDefault");
			NativeMethodInfoPtr_GetErrorChangeRuntimeDepthMapResolution_Public_Static_String_VolumetricShadowHD_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shadow>.NativeClassPtr, 100663738);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69368, XrefRangeEnd = 69376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string GetErrorChangeRuntimeDepthMapResolution(VolumetricShadowHD comp)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)comp);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetErrorChangeRuntimeDepthMapResolution_Public_Static_String_VolumetricShadowHD_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		public Shadow(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class Cookie : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ContributionDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_ContributionMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_ContributionMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_CookieTextureDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_ChannelDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_NegativeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_TranslationDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_RotationDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_ScaleDefault;

		public unsafe static float ContributionDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ContributionDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ContributionDefault, (void*)(&num));
			}
		}

		public unsafe static float ContributionMin
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ContributionMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ContributionMin, (void*)(&num));
			}
		}

		public unsafe static float ContributionMax
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ContributionMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ContributionMax, (void*)(&num));
			}
		}

		public unsafe static Texture CookieTextureDefault
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CookieTextureDefault, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CookieTextureDefault, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture));
			}
		}

		public unsafe static CookieChannel ChannelDefault
		{
			get
			{
				Unsafe.SkipInit(out CookieChannel result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ChannelDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ChannelDefault, (void*)(&cookieChannel));
			}
		}

		public unsafe static bool NegativeDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NegativeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NegativeDefault, (void*)(&flag));
			}
		}

		public unsafe static Vector2 TranslationDefault
		{
			get
			{
				Unsafe.SkipInit(out Vector2 result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TranslationDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TranslationDefault, (void*)(&vector));
			}
		}

		public unsafe static float RotationDefault
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RotationDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RotationDefault, (void*)(&num));
			}
		}

		public unsafe static Vector2 ScaleDefault
		{
			get
			{
				Unsafe.SkipInit(out Vector2 result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ScaleDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ScaleDefault, (void*)(&vector));
			}
		}

		static Cookie()
		{
			Il2CppClassPointerStore<Cookie>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Cookie");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Cookie>.NativeClassPtr);
			NativeFieldInfoPtr_ContributionDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "ContributionDefault");
			NativeFieldInfoPtr_ContributionMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "ContributionMin");
			NativeFieldInfoPtr_ContributionMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "ContributionMax");
			NativeFieldInfoPtr_CookieTextureDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "CookieTextureDefault");
			NativeFieldInfoPtr_ChannelDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "ChannelDefault");
			NativeFieldInfoPtr_NegativeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "NegativeDefault");
			NativeFieldInfoPtr_TranslationDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "TranslationDefault");
			NativeFieldInfoPtr_RotationDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "RotationDefault");
			NativeFieldInfoPtr_ScaleDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Cookie>.NativeClassPtr, "ScaleDefault");
		}

		public Cookie(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class Config : Il2CppSystem.Object
	{
		public static class HD : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_GeometryRenderQueueDefault;

			private static readonly System.IntPtr NativeFieldInfoPtr_CameraBlendingDistance;

			private static readonly System.IntPtr NativeFieldInfoPtr_RaymarchingQualitiesStepsMin;

			public unsafe static RenderQueue GeometryRenderQueueDefault
			{
				get
				{
					Unsafe.SkipInit(out RenderQueue result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryRenderQueueDefault, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryRenderQueueDefault, (void*)(&renderQueue));
				}
			}

			public unsafe static float CameraBlendingDistance
			{
				get
				{
					Unsafe.SkipInit(out float result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CameraBlendingDistance, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CameraBlendingDistance, (void*)(&num));
				}
			}

			public unsafe static int RaymarchingQualitiesStepsMin
			{
				get
				{
					Unsafe.SkipInit(out int result);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RaymarchingQualitiesStepsMin, (void*)(&result));
					return result;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RaymarchingQualitiesStepsMin, (void*)(&num));
				}
			}

			static HD()
			{
				Il2CppClassPointerStore<HD>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Config>.NativeClassPtr, "HD");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HD>.NativeClassPtr);
				NativeFieldInfoPtr_GeometryRenderQueueDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "GeometryRenderQueueDefault");
				NativeFieldInfoPtr_CameraBlendingDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "CameraBlendingDistance");
				NativeFieldInfoPtr_RaymarchingQualitiesStepsMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HD>.NativeClassPtr, "RaymarchingQualitiesStepsMin");
			}

			public HD(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_GeometryOverrideLayerDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeometryLayerIDDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeometryTagDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FadeOutCameraTagDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeometryRenderQueueDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeometryRenderPipelineDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_GeometryRenderingModeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_Noise3DSizeDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_DitheringFactor;

		private static readonly System.IntPtr NativeFieldInfoPtr_UseLightColorTemperatureDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FeatureEnabledDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_FeatureEnabledColorGradientDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_SharedMeshSidesDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_SharedMeshSidesMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_SharedMeshSidesMax;

		private static readonly System.IntPtr NativeFieldInfoPtr_SharedMeshSegmentsDefault;

		private static readonly System.IntPtr NativeFieldInfoPtr_SharedMeshSegmentsMin;

		private static readonly System.IntPtr NativeFieldInfoPtr_SharedMeshSegmentsMax;

		public unsafe static bool GeometryOverrideLayerDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryOverrideLayerDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryOverrideLayerDefault, (void*)(&flag));
			}
		}

		public unsafe static int GeometryLayerIDDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryLayerIDDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryLayerIDDefault, (void*)(&num));
			}
		}

		public unsafe static string GeometryTagDefault
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryTagDefault, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryTagDefault, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static string FadeOutCameraTagDefault
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FadeOutCameraTagDefault, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FadeOutCameraTagDefault, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static RenderQueue GeometryRenderQueueDefault
		{
			get
			{
				Unsafe.SkipInit(out RenderQueue result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryRenderQueueDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryRenderQueueDefault, (void*)(&renderQueue));
			}
		}

		public unsafe static RenderPipeline GeometryRenderPipelineDefault
		{
			get
			{
				Unsafe.SkipInit(out RenderPipeline result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryRenderPipelineDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryRenderPipelineDefault, (void*)(&renderPipeline));
			}
		}

		public unsafe static RenderingMode GeometryRenderingModeDefault
		{
			get
			{
				Unsafe.SkipInit(out RenderingMode result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GeometryRenderingModeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GeometryRenderingModeDefault, (void*)(&renderingMode));
			}
		}

		public unsafe static int Noise3DSizeDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Noise3DSizeDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Noise3DSizeDefault, (void*)(&num));
			}
		}

		public unsafe static float DitheringFactor
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DitheringFactor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DitheringFactor, (void*)(&num));
			}
		}

		public unsafe static bool UseLightColorTemperatureDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UseLightColorTemperatureDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UseLightColorTemperatureDefault, (void*)(&flag));
			}
		}

		public unsafe static bool FeatureEnabledDefault
		{
			get
			{
				Unsafe.SkipInit(out bool result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FeatureEnabledDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FeatureEnabledDefault, (void*)(&flag));
			}
		}

		public unsafe static FeatureEnabledColorGradient FeatureEnabledColorGradientDefault
		{
			get
			{
				Unsafe.SkipInit(out FeatureEnabledColorGradient result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FeatureEnabledColorGradientDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FeatureEnabledColorGradientDefault, (void*)(&featureEnabledColorGradient));
			}
		}

		public unsafe static int SharedMeshSidesDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SharedMeshSidesDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SharedMeshSidesDefault, (void*)(&num));
			}
		}

		public unsafe static int SharedMeshSidesMin
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SharedMeshSidesMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SharedMeshSidesMin, (void*)(&num));
			}
		}

		public unsafe static int SharedMeshSidesMax
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SharedMeshSidesMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SharedMeshSidesMax, (void*)(&num));
			}
		}

		public unsafe static int SharedMeshSegmentsDefault
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SharedMeshSegmentsDefault, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SharedMeshSegmentsDefault, (void*)(&num));
			}
		}

		public unsafe static int SharedMeshSegmentsMin
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SharedMeshSegmentsMin, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SharedMeshSegmentsMin, (void*)(&num));
			}
		}

		public unsafe static int SharedMeshSegmentsMax
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SharedMeshSegmentsMax, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SharedMeshSegmentsMax, (void*)(&num));
			}
		}

		static Config()
		{
			Il2CppClassPointerStore<Config>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Consts>.NativeClassPtr, "Config");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Config>.NativeClassPtr);
			NativeFieldInfoPtr_GeometryOverrideLayerDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "GeometryOverrideLayerDefault");
			NativeFieldInfoPtr_GeometryLayerIDDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "GeometryLayerIDDefault");
			NativeFieldInfoPtr_GeometryTagDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "GeometryTagDefault");
			NativeFieldInfoPtr_FadeOutCameraTagDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "FadeOutCameraTagDefault");
			NativeFieldInfoPtr_GeometryRenderQueueDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "GeometryRenderQueueDefault");
			NativeFieldInfoPtr_GeometryRenderPipelineDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "GeometryRenderPipelineDefault");
			NativeFieldInfoPtr_GeometryRenderingModeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "GeometryRenderingModeDefault");
			NativeFieldInfoPtr_Noise3DSizeDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "Noise3DSizeDefault");
			NativeFieldInfoPtr_DitheringFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "DitheringFactor");
			NativeFieldInfoPtr_UseLightColorTemperatureDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "UseLightColorTemperatureDefault");
			NativeFieldInfoPtr_FeatureEnabledDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "FeatureEnabledDefault");
			NativeFieldInfoPtr_FeatureEnabledColorGradientDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "FeatureEnabledColorGradientDefault");
			NativeFieldInfoPtr_SharedMeshSidesDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "SharedMeshSidesDefault");
			NativeFieldInfoPtr_SharedMeshSidesMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "SharedMeshSidesMin");
			NativeFieldInfoPtr_SharedMeshSidesMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "SharedMeshSidesMax");
			NativeFieldInfoPtr_SharedMeshSegmentsDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "SharedMeshSegmentsDefault");
			NativeFieldInfoPtr_SharedMeshSegmentsMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "SharedMeshSegmentsMin");
			NativeFieldInfoPtr_SharedMeshSegmentsMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Config>.NativeClassPtr, "SharedMeshSegmentsMax");
		}

		public Config(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_PluginFolder;

	public unsafe static string PluginFolder
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PluginFolder, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PluginFolder, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static Consts()
	{
		Il2CppClassPointerStore<Consts>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "Consts");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Consts>.NativeClassPtr);
		NativeFieldInfoPtr_PluginFolder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Consts>.NativeClassPtr, "PluginFolder");
	}

	public Consts(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
