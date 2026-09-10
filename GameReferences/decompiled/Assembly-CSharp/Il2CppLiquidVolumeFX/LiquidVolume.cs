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

namespace Il2CppLiquidVolumeFX;

public class LiquidVolume : MonoBehaviour
{
	public sealed class MeshCache : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_verticesSorted;

		private static readonly System.IntPtr NativeFieldInfoPtr_verticesUnsorted;

		private static readonly System.IntPtr NativeFieldInfoPtr_indices;

		public unsafe Il2CppStructArray<Vector3> verticesSorted
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesSorted);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesSorted)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppStructArray<Vector3> verticesUnsorted
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesUnsorted);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesUnsorted)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppStructArray<int> indices
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_indices);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_indices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		static MeshCache()
		{
			Il2CppClassPointerStore<MeshCache>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "MeshCache");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshCache>.NativeClassPtr);
			NativeFieldInfoPtr_verticesSorted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshCache>.NativeClassPtr, "verticesSorted");
			NativeFieldInfoPtr_verticesUnsorted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshCache>.NativeClassPtr, "verticesUnsorted");
			NativeFieldInfoPtr_indices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshCache>.NativeClassPtr, "indices");
		}

		public MeshCache(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public MeshCache()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshCache>.NativeClassPtr))
		{
		}
	}

	public sealed class MeshVolumeCalcFunction : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Single_Single_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Single_Single_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Single_IAsyncResult_0;

		static MeshVolumeCalcFunction()
		{
			Il2CppClassPointerStore<MeshVolumeCalcFunction>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "MeshVolumeCalcFunction");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshVolumeCalcFunction>.NativeClassPtr, 100664719);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshVolumeCalcFunction>.NativeClassPtr, 100664720);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Single_Single_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshVolumeCalcFunction>.NativeClassPtr, 100664721);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Single_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshVolumeCalcFunction>.NativeClassPtr, 100664722);
		}

		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 75246, RefRangeEnd = 75251, XrefRangeStart = 75243, XrefRangeEnd = 75246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MeshVolumeCalcFunction(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshVolumeCalcFunction>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual float Invoke(float level01, float yExtent)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&level01);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &yExtent;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75251, XrefRangeEnd = 75257, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(float level01, float yExtent, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = (nint)(&level01);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &yExtent;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Single_Single_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75257, XrefRangeEnd = 75259, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual float EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Single_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public MeshVolumeCalcFunction(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator MeshVolumeCalcFunction(System.Func<float, float, float> P_0)
		{
			return DelegateSupport.ConvertDelegate<MeshVolumeCalcFunction>((System.Delegate)P_0);
		}

		public static MeshVolumeCalcFunction operator +(MeshVolumeCalcFunction P_0, MeshVolumeCalcFunction P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<MeshVolumeCalcFunction>();
		}

		public static MeshVolumeCalcFunction operator -(MeshVolumeCalcFunction P_0, MeshVolumeCalcFunction P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<MeshVolumeCalcFunction>();
			}
			return (MeshVolumeCalcFunction)obj;
		}
	}

	public static class ShaderParams : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_PointLightInsideAtten;

		private static readonly System.IntPtr NativeFieldInfoPtr_PointLightColorArray;

		private static readonly System.IntPtr NativeFieldInfoPtr_PointLightPositionArray;

		private static readonly System.IntPtr NativeFieldInfoPtr_PointLightCount;

		private static readonly System.IntPtr NativeFieldInfoPtr_GlossinessInt;

		private static readonly System.IntPtr NativeFieldInfoPtr_DoubleSidedBias;

		private static readonly System.IntPtr NativeFieldInfoPtr_BackDepthBias;

		private static readonly System.IntPtr NativeFieldInfoPtr_Muddy;

		private static readonly System.IntPtr NativeFieldInfoPtr_Alpha;

		private static readonly System.IntPtr NativeFieldInfoPtr_AlphaCombined;

		private static readonly System.IntPtr NativeFieldInfoPtr_SparklingIntensity;

		private static readonly System.IntPtr NativeFieldInfoPtr_SparklingThreshold;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthAtten;

		private static readonly System.IntPtr NativeFieldInfoPtr_SmokeColor;

		private static readonly System.IntPtr NativeFieldInfoPtr_SmokeAtten;

		private static readonly System.IntPtr NativeFieldInfoPtr_SmokeSpeed;

		private static readonly System.IntPtr NativeFieldInfoPtr_SmokeHeightAtten;

		private static readonly System.IntPtr NativeFieldInfoPtr_SmokeRaySteps;

		private static readonly System.IntPtr NativeFieldInfoPtr_LiquidRaySteps;

		private static readonly System.IntPtr NativeFieldInfoPtr_FlaskBlurIntensity;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamColor;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamRaySteps;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamDensity;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamWeight;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamBottom;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamTurbulence;

		private static readonly System.IntPtr NativeFieldInfoPtr_RefractTex;

		private static readonly System.IntPtr NativeFieldInfoPtr_FlaskThickness;

		private static readonly System.IntPtr NativeFieldInfoPtr_Size;

		private static readonly System.IntPtr NativeFieldInfoPtr_Scale;

		private static readonly System.IntPtr NativeFieldInfoPtr_Center;

		private static readonly System.IntPtr NativeFieldInfoPtr_SizeWorld;

		private static readonly System.IntPtr NativeFieldInfoPtr_DepthAwareOffset;

		private static readonly System.IntPtr NativeFieldInfoPtr_Turbulence;

		private static readonly System.IntPtr NativeFieldInfoPtr_TurbulenceSpeed;

		private static readonly System.IntPtr NativeFieldInfoPtr_MurkinessSpeed;

		private static readonly System.IntPtr NativeFieldInfoPtr_Color1;

		private static readonly System.IntPtr NativeFieldInfoPtr_Color2;

		private static readonly System.IntPtr NativeFieldInfoPtr_EmissionColor;

		private static readonly System.IntPtr NativeFieldInfoPtr_LightColor;

		private static readonly System.IntPtr NativeFieldInfoPtr_LightDir;

		private static readonly System.IntPtr NativeFieldInfoPtr_LevelPos;

		private static readonly System.IntPtr NativeFieldInfoPtr_UpperLimit;

		private static readonly System.IntPtr NativeFieldInfoPtr_LowerLimit;

		private static readonly System.IntPtr NativeFieldInfoPtr_FoamMaxPos;

		private static readonly System.IntPtr NativeFieldInfoPtr_CullMode;

		private static readonly System.IntPtr NativeFieldInfoPtr_ZTestMode;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseTex;

		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseTexUnwrapped;

		private static readonly System.IntPtr NativeFieldInfoPtr_GlobalRefractionTexture;

		private static readonly System.IntPtr NativeFieldInfoPtr_RotationMatrix;

		private static readonly System.IntPtr NativeFieldInfoPtr_QueueOffset;

		private static readonly System.IntPtr NativeFieldInfoPtr_PreserveSpecular;

		public unsafe static int PointLightInsideAtten
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PointLightInsideAtten, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PointLightInsideAtten, (void*)(&num));
			}
		}

		public unsafe static int PointLightColorArray
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PointLightColorArray, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PointLightColorArray, (void*)(&num));
			}
		}

		public unsafe static int PointLightPositionArray
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PointLightPositionArray, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PointLightPositionArray, (void*)(&num));
			}
		}

		public unsafe static int PointLightCount
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PointLightCount, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PointLightCount, (void*)(&num));
			}
		}

		public unsafe static int GlossinessInt
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GlossinessInt, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GlossinessInt, (void*)(&num));
			}
		}

		public unsafe static int DoubleSidedBias
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DoubleSidedBias, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DoubleSidedBias, (void*)(&num));
			}
		}

		public unsafe static int BackDepthBias
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BackDepthBias, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BackDepthBias, (void*)(&num));
			}
		}

		public unsafe static int Muddy
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Muddy, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Muddy, (void*)(&num));
			}
		}

		public unsafe static int Alpha
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Alpha, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Alpha, (void*)(&num));
			}
		}

		public unsafe static int AlphaCombined
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AlphaCombined, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AlphaCombined, (void*)(&num));
			}
		}

		public unsafe static int SparklingIntensity
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SparklingIntensity, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SparklingIntensity, (void*)(&num));
			}
		}

		public unsafe static int SparklingThreshold
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SparklingThreshold, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SparklingThreshold, (void*)(&num));
			}
		}

		public unsafe static int DepthAtten
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthAtten, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthAtten, (void*)(&num));
			}
		}

		public unsafe static int SmokeColor
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmokeColor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmokeColor, (void*)(&num));
			}
		}

		public unsafe static int SmokeAtten
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmokeAtten, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmokeAtten, (void*)(&num));
			}
		}

		public unsafe static int SmokeSpeed
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmokeSpeed, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmokeSpeed, (void*)(&num));
			}
		}

		public unsafe static int SmokeHeightAtten
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmokeHeightAtten, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmokeHeightAtten, (void*)(&num));
			}
		}

		public unsafe static int SmokeRaySteps
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmokeRaySteps, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmokeRaySteps, (void*)(&num));
			}
		}

		public unsafe static int LiquidRaySteps
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LiquidRaySteps, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LiquidRaySteps, (void*)(&num));
			}
		}

		public unsafe static int FlaskBlurIntensity
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FlaskBlurIntensity, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FlaskBlurIntensity, (void*)(&num));
			}
		}

		public unsafe static int FoamColor
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamColor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamColor, (void*)(&num));
			}
		}

		public unsafe static int FoamRaySteps
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamRaySteps, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamRaySteps, (void*)(&num));
			}
		}

		public unsafe static int FoamDensity
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamDensity, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamDensity, (void*)(&num));
			}
		}

		public unsafe static int FoamWeight
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamWeight, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamWeight, (void*)(&num));
			}
		}

		public unsafe static int FoamBottom
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamBottom, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamBottom, (void*)(&num));
			}
		}

		public unsafe static int FoamTurbulence
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamTurbulence, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamTurbulence, (void*)(&num));
			}
		}

		public unsafe static int RefractTex
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RefractTex, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RefractTex, (void*)(&num));
			}
		}

		public unsafe static int FlaskThickness
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FlaskThickness, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FlaskThickness, (void*)(&num));
			}
		}

		public unsafe static int Size
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Size, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Size, (void*)(&num));
			}
		}

		public unsafe static int Scale
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Scale, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Scale, (void*)(&num));
			}
		}

		public unsafe static int Center
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Center, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Center, (void*)(&num));
			}
		}

		public unsafe static int SizeWorld
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SizeWorld, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SizeWorld, (void*)(&num));
			}
		}

		public unsafe static int DepthAwareOffset
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DepthAwareOffset, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DepthAwareOffset, (void*)(&num));
			}
		}

		public unsafe static int Turbulence
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Turbulence, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Turbulence, (void*)(&num));
			}
		}

		public unsafe static int TurbulenceSpeed
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TurbulenceSpeed, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TurbulenceSpeed, (void*)(&num));
			}
		}

		public unsafe static int MurkinessSpeed
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MurkinessSpeed, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MurkinessSpeed, (void*)(&num));
			}
		}

		public unsafe static int Color1
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Color1, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Color1, (void*)(&num));
			}
		}

		public unsafe static int Color2
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Color2, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Color2, (void*)(&num));
			}
		}

		public unsafe static int EmissionColor
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EmissionColor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EmissionColor, (void*)(&num));
			}
		}

		public unsafe static int LightColor
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LightColor, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LightColor, (void*)(&num));
			}
		}

		public unsafe static int LightDir
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LightDir, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LightDir, (void*)(&num));
			}
		}

		public unsafe static int LevelPos
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LevelPos, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LevelPos, (void*)(&num));
			}
		}

		public unsafe static int UpperLimit
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UpperLimit, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UpperLimit, (void*)(&num));
			}
		}

		public unsafe static int LowerLimit
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LowerLimit, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LowerLimit, (void*)(&num));
			}
		}

		public unsafe static int FoamMaxPos
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FoamMaxPos, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FoamMaxPos, (void*)(&num));
			}
		}

		public unsafe static int CullMode
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullMode, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullMode, (void*)(&num));
			}
		}

		public unsafe static int ZTestMode
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ZTestMode, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ZTestMode, (void*)(&num));
			}
		}

		public unsafe static int NoiseTex
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseTex, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseTex, (void*)(&num));
			}
		}

		public unsafe static int NoiseTexUnwrapped
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NoiseTexUnwrapped, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NoiseTexUnwrapped, (void*)(&num));
			}
		}

		public unsafe static int GlobalRefractionTexture
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GlobalRefractionTexture, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GlobalRefractionTexture, (void*)(&num));
			}
		}

		public unsafe static int RotationMatrix
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RotationMatrix, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RotationMatrix, (void*)(&num));
			}
		}

		public unsafe static int QueueOffset
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_QueueOffset, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_QueueOffset, (void*)(&num));
			}
		}

		public unsafe static int PreserveSpecular
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PreserveSpecular, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PreserveSpecular, (void*)(&num));
			}
		}

		static ShaderParams()
		{
			Il2CppClassPointerStore<ShaderParams>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "ShaderParams");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr);
			NativeFieldInfoPtr_PointLightInsideAtten = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "PointLightInsideAtten");
			NativeFieldInfoPtr_PointLightColorArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "PointLightColorArray");
			NativeFieldInfoPtr_PointLightPositionArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "PointLightPositionArray");
			NativeFieldInfoPtr_PointLightCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "PointLightCount");
			NativeFieldInfoPtr_GlossinessInt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "GlossinessInt");
			NativeFieldInfoPtr_DoubleSidedBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "DoubleSidedBias");
			NativeFieldInfoPtr_BackDepthBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "BackDepthBias");
			NativeFieldInfoPtr_Muddy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Muddy");
			NativeFieldInfoPtr_Alpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Alpha");
			NativeFieldInfoPtr_AlphaCombined = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "AlphaCombined");
			NativeFieldInfoPtr_SparklingIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SparklingIntensity");
			NativeFieldInfoPtr_SparklingThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SparklingThreshold");
			NativeFieldInfoPtr_DepthAtten = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "DepthAtten");
			NativeFieldInfoPtr_SmokeColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SmokeColor");
			NativeFieldInfoPtr_SmokeAtten = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SmokeAtten");
			NativeFieldInfoPtr_SmokeSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SmokeSpeed");
			NativeFieldInfoPtr_SmokeHeightAtten = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SmokeHeightAtten");
			NativeFieldInfoPtr_SmokeRaySteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SmokeRaySteps");
			NativeFieldInfoPtr_LiquidRaySteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "LiquidRaySteps");
			NativeFieldInfoPtr_FlaskBlurIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FlaskBlurIntensity");
			NativeFieldInfoPtr_FoamColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamColor");
			NativeFieldInfoPtr_FoamRaySteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamRaySteps");
			NativeFieldInfoPtr_FoamDensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamDensity");
			NativeFieldInfoPtr_FoamWeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamWeight");
			NativeFieldInfoPtr_FoamBottom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamBottom");
			NativeFieldInfoPtr_FoamTurbulence = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamTurbulence");
			NativeFieldInfoPtr_RefractTex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "RefractTex");
			NativeFieldInfoPtr_FlaskThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FlaskThickness");
			NativeFieldInfoPtr_Size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Size");
			NativeFieldInfoPtr_Scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Scale");
			NativeFieldInfoPtr_Center = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Center");
			NativeFieldInfoPtr_SizeWorld = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SizeWorld");
			NativeFieldInfoPtr_DepthAwareOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "DepthAwareOffset");
			NativeFieldInfoPtr_Turbulence = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Turbulence");
			NativeFieldInfoPtr_TurbulenceSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "TurbulenceSpeed");
			NativeFieldInfoPtr_MurkinessSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "MurkinessSpeed");
			NativeFieldInfoPtr_Color1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Color1");
			NativeFieldInfoPtr_Color2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "Color2");
			NativeFieldInfoPtr_EmissionColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "EmissionColor");
			NativeFieldInfoPtr_LightColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "LightColor");
			NativeFieldInfoPtr_LightDir = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "LightDir");
			NativeFieldInfoPtr_LevelPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "LevelPos");
			NativeFieldInfoPtr_UpperLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "UpperLimit");
			NativeFieldInfoPtr_LowerLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "LowerLimit");
			NativeFieldInfoPtr_FoamMaxPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FoamMaxPos");
			NativeFieldInfoPtr_CullMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "CullMode");
			NativeFieldInfoPtr_ZTestMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "ZTestMode");
			NativeFieldInfoPtr_NoiseTex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "NoiseTex");
			NativeFieldInfoPtr_NoiseTexUnwrapped = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "NoiseTexUnwrapped");
			NativeFieldInfoPtr_GlobalRefractionTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "GlobalRefractionTexture");
			NativeFieldInfoPtr_RotationMatrix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "RotationMatrix");
			NativeFieldInfoPtr_QueueOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "QueueOffset");
			NativeFieldInfoPtr_PreserveSpecular = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "PreserveSpecular");
		}

		public ShaderParams(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_FORCE_GLES_COMPATIBILITY;

	private static readonly System.IntPtr NativeFieldInfoPtr_onPropertiesChanged;

	private static readonly System.IntPtr NativeFieldInfoPtr__topology;

	private static readonly System.IntPtr NativeFieldInfoPtr__detail;

	private static readonly System.IntPtr NativeFieldInfoPtr__level;

	private static readonly System.IntPtr NativeFieldInfoPtr__levelMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr__useLightColor;

	private static readonly System.IntPtr NativeFieldInfoPtr__useLightDirection;

	private static readonly System.IntPtr NativeFieldInfoPtr__directionalLight;

	private static readonly System.IntPtr NativeFieldInfoPtr__liquidColor1;

	private static readonly System.IntPtr NativeFieldInfoPtr__liquidScale1;

	private static readonly System.IntPtr NativeFieldInfoPtr__liquidColor2;

	private static readonly System.IntPtr NativeFieldInfoPtr__liquidScale2;

	private static readonly System.IntPtr NativeFieldInfoPtr__alpha;

	private static readonly System.IntPtr NativeFieldInfoPtr__emissionColor;

	private static readonly System.IntPtr NativeFieldInfoPtr__ditherShadows;

	private static readonly System.IntPtr NativeFieldInfoPtr__murkiness;

	private static readonly System.IntPtr NativeFieldInfoPtr__turbulence1;

	private static readonly System.IntPtr NativeFieldInfoPtr__turbulence2;

	private static readonly System.IntPtr NativeFieldInfoPtr__frecuency;

	private static readonly System.IntPtr NativeFieldInfoPtr__speed;

	private static readonly System.IntPtr NativeFieldInfoPtr__sparklingIntensity;

	private static readonly System.IntPtr NativeFieldInfoPtr__sparklingAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr__deepObscurance;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamColor;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamScale;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamThickness;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamDensity;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamWeight;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamTurbulence;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamVisibleFromBottom;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeColor;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeScale;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeBaseObscurance;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeHeightAtten;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr__fixMesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_originalMesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_originalPivotOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr__pivotOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr__limitVerticalRange;

	private static readonly System.IntPtr NativeFieldInfoPtr__upperLimit;

	private static readonly System.IntPtr NativeFieldInfoPtr__lowerLimit;

	private static readonly System.IntPtr NativeFieldInfoPtr__subMeshIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr__flaskMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr__flaskThickness;

	private static readonly System.IntPtr NativeFieldInfoPtr__glossinessInternal;

	private static readonly System.IntPtr NativeFieldInfoPtr__scatteringEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr__scatteringPower;

	private static readonly System.IntPtr NativeFieldInfoPtr__scatteringAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr__refractionBlur;

	private static readonly System.IntPtr NativeFieldInfoPtr__blurIntensity;

	private static readonly System.IntPtr NativeFieldInfoPtr__liquidRaySteps;

	private static readonly System.IntPtr NativeFieldInfoPtr__foamRaySteps;

	private static readonly System.IntPtr NativeFieldInfoPtr__smokeRaySteps;

	private static readonly System.IntPtr NativeFieldInfoPtr__bumpMap;

	private static readonly System.IntPtr NativeFieldInfoPtr__bumpStrength;

	private static readonly System.IntPtr NativeFieldInfoPtr__bumpDistortionScale;

	private static readonly System.IntPtr NativeFieldInfoPtr__bumpDistortionOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr__distortionMap;

	private static readonly System.IntPtr NativeFieldInfoPtr__texture;

	private static readonly System.IntPtr NativeFieldInfoPtr__textureScale;

	private static readonly System.IntPtr NativeFieldInfoPtr__textureOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr__distortionAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr__depthAware;

	private static readonly System.IntPtr NativeFieldInfoPtr__depthAwareOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr__irregularDepthDebug;

	private static readonly System.IntPtr NativeFieldInfoPtr__depthAwareCustomPass;

	private static readonly System.IntPtr NativeFieldInfoPtr__depthAwareCustomPassDebug;

	private static readonly System.IntPtr NativeFieldInfoPtr__doubleSidedBias;

	private static readonly System.IntPtr NativeFieldInfoPtr__backDepthBias;

	private static readonly System.IntPtr NativeFieldInfoPtr__rotationLevelCompensation;

	private static readonly System.IntPtr NativeFieldInfoPtr__ignoreGravity;

	private static readonly System.IntPtr NativeFieldInfoPtr__reactToForces;

	private static readonly System.IntPtr NativeFieldInfoPtr__extentsScale;

	private static readonly System.IntPtr NativeFieldInfoPtr__noiseVariation;

	private static readonly System.IntPtr NativeFieldInfoPtr__allowViewFromInside;

	private static readonly System.IntPtr NativeFieldInfoPtr__debugSpillPoint;

	private static readonly System.IntPtr NativeFieldInfoPtr__renderQueue;

	private static readonly System.IntPtr NativeFieldInfoPtr__reflectionTexture;

	private static readonly System.IntPtr NativeFieldInfoPtr__physicsMass;

	private static readonly System.IntPtr NativeFieldInfoPtr__physicsAngularDamp;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_INDEX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY_INDEX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB_INDEX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_TOPOLOGY_INDEX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_REFRACTION_INDEX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_SPHERE;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_CUBE;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_CYLINDER;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_IRREGULAR;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_FP_RENDER_TEXTURE;

	private static readonly System.IntPtr NativeFieldInfoPtr_SHADER_KEYWORD_USE_REFRACTION;

	private static readonly System.IntPtr NativeFieldInfoPtr_SPILL_POINT_GIZMO;

	private static readonly System.IntPtr NativeFieldInfoPtr_liqMat;

	private static readonly System.IntPtr NativeFieldInfoPtr_liqMatSimple;

	private static readonly System.IntPtr NativeFieldInfoPtr_liqMatDefaultNoFlask;

	private static readonly System.IntPtr NativeFieldInfoPtr_mesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_mr;

	private static readonly System.IntPtr NativeFieldInfoPtr_mrSharedMaterials;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_shaderKeywords;

	private static readonly System.IntPtr NativeFieldInfoPtr_camInside;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastDistanceToCam;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentDetail;

	private static readonly System.IntPtr NativeFieldInfoPtr_turb;

	private static readonly System.IntPtr NativeFieldInfoPtr_shaderTurb;

	private static readonly System.IntPtr NativeFieldInfoPtr_turbulenceSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_murkinessSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_liquidLevelPos;

	private static readonly System.IntPtr NativeFieldInfoPtr_shouldUpdateMaterialProperties;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentNoiseVariation;

	private static readonly System.IntPtr NativeFieldInfoPtr_levelMultipled;

	private static readonly System.IntPtr NativeFieldInfoPtr_noise3DUnwrapped;

	private static readonly System.IntPtr NativeFieldInfoPtr_noise3DTex;

	private static readonly System.IntPtr NativeFieldInfoPtr_colors3D;

	private static readonly System.IntPtr NativeFieldInfoPtr_verticesUnsorted;

	private static readonly System.IntPtr NativeFieldInfoPtr_verticesSorted;

	private static readonly System.IntPtr NativeFieldInfoPtr_rotatedVertices;

	private static readonly System.IntPtr NativeFieldInfoPtr_verticesIndices;

	private static readonly System.IntPtr NativeFieldInfoPtr_volumeRef;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastLevelVolumeRef;

	private static readonly System.IntPtr NativeFieldInfoPtr_inertia;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastAvgVelocity;

	private static readonly System.IntPtr NativeFieldInfoPtr_angularVelocity;

	private static readonly System.IntPtr NativeFieldInfoPtr_angularInertia;

	private static readonly System.IntPtr NativeFieldInfoPtr_turbulenceDueForces;

	private static readonly System.IntPtr NativeFieldInfoPtr_liquidRot;

	private static readonly System.IntPtr NativeFieldInfoPtr_prevThickness;

	private static readonly System.IntPtr NativeFieldInfoPtr_spillPointGizmo;

	private static readonly System.IntPtr NativeFieldInfoPtr_defaultContainerNames;

	private static readonly System.IntPtr NativeFieldInfoPtr_pointLightColorBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_pointLightPositionBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastPointLightCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshCache;

	private static readonly System.IntPtr NativeFieldInfoPtr_verts;

	private static readonly System.IntPtr NativeFieldInfoPtr_cutPoints;

	private static readonly System.IntPtr NativeFieldInfoPtr_cutPlaneCenter;

	private static readonly System.IntPtr NativeFieldInfoPtr_fixedMesh;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_onPropertiesChanged_Public_add_Void_PropertiesChangedEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_onPropertiesChanged_Public_rem_Void_PropertiesChangedEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_topology_Public_get_TOPOLOGY_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_topology_Public_set_Void_TOPOLOGY_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_detail_Public_get_DETAIL_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_detail_Public_set_Void_DETAIL_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_level_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_level_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_levelMultiplier_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_levelMultiplier_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_useLightColor_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_useLightColor_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_useLightDirection_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_useLightDirection_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_directionalLight_Public_get_Light_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_directionalLight_Public_set_Void_Light_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_liquidColor1_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_liquidColor1_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_liquidScale1_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_liquidScale1_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_liquidColor2_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_liquidColor2_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_liquidScale2_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_liquidScale2_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_alpha_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_alpha_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_emissionColor_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_emissionColor_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ditherShadows_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ditherShadows_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_murkiness_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_murkiness_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_turbulence1_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_turbulence1_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_turbulence2_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_turbulence2_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_frecuency_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_frecuency_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_speed_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_speed_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_sparklingIntensity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_sparklingIntensity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_sparklingAmount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_sparklingAmount_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_deepObscurance_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_deepObscurance_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamColor_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamColor_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamScale_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamScale_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamThickness_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamThickness_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamDensity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamDensity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamWeight_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamWeight_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamTurbulence_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamTurbulence_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamVisibleFromBottom_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamVisibleFromBottom_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeEnabled_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeEnabled_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeColor_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeColor_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeScale_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeScale_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeBaseObscurance_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeBaseObscurance_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeHeightAtten_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeHeightAtten_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeSpeed_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeSpeed_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_fixMesh_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_fixMesh_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_pivotOffset_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_pivotOffset_Public_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_limitVerticalRange_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_limitVerticalRange_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_upperLimit_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_upperLimit_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_lowerLimit_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_lowerLimit_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_subMeshIndex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_subMeshIndex_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_flaskMaterial_Public_get_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_flaskMaterial_Public_set_Void_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_flaskThickness_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_flaskThickness_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_glossinessInternal_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_glossinessInternal_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_scatteringEnabled_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_scatteringEnabled_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_scatteringPower_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_scatteringPower_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_scatteringAmount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_scatteringAmount_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_refractionBlur_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_refractionBlur_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_blurIntensity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_blurIntensity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_liquidRaySteps_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_liquidRaySteps_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_foamRaySteps_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_foamRaySteps_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_smokeRaySteps_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_smokeRaySteps_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_bumpMap_Public_get_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_bumpMap_Public_set_Void_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_bumpStrength_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_bumpStrength_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_bumpDistortionScale_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_bumpDistortionScale_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_bumpDistortionOffset_Public_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_bumpDistortionOffset_Public_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_distortionMap_Public_get_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_distortionMap_Public_set_Void_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_texture_Public_get_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_texture_Public_set_Void_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_textureScale_Public_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_textureScale_Public_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_textureOffset_Public_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_textureOffset_Public_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_distortionAmount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_distortionAmount_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_depthAware_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_depthAware_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_depthAwareOffset_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_depthAwareOffset_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_irregularDepthDebug_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_irregularDepthDebug_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_depthAwareCustomPass_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_depthAwareCustomPass_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_depthAwareCustomPassDebug_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_depthAwareCustomPassDebug_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_doubleSidedBias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_doubleSidedBias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_backDepthBias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_backDepthBias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_rotationLevelCompensation_Public_get_LEVEL_COMPENSATION_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_rotationLevelCompensation_Public_set_Void_LEVEL_COMPENSATION_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ignoreGravity_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ignoreGravity_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_reactToForces_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_reactToForces_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_extentsScale_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_extentsScale_Public_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_noiseVariation_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_noiseVariation_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_allowViewFromInside_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_allowViewFromInside_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_debugSpillPoint_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_debugSpillPoint_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_renderQueue_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_renderQueue_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_reflectionTexture_Public_get_Cubemap_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_reflectionTexture_Public_set_Void_Cubemap_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_physicsMass_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_physicsMass_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_physicsAngularDamp_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_physicsAngularDamp_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_useFPRenderTextures_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RenderObject_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnWillRenderObject_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDidApplyAnimationProperties_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearMeshCache_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadVertices_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_vertexComparer_Private_Int32_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateAnimations_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateMaterialProperties_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateMaterialPropertiesNow_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyGlobalAlpha_Private_Color_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRenderer_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateLevels_Private_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RotateVertices_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SignedVolumeOfTriangle_Private_Single_Vector3_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMeshVolumeUnderLevelFast_Public_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMeshVolumeWSFast_Public_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMeshVolumeUnderLevelWSFast_Public_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampVertexToSlicePlane_Private_Vector3_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMeshVolumeUnderLevel_Public_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMeshVolumeWS_Public_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMeshVolumeUnderLevelWS_Public_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PolygonSortOnPlane_Private_Int32_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateTurbulence_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckInsideOut_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PointInAABB_Private_Boolean_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PointInCylinder_Private_Boolean_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateInsideOut_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_liquidSurfaceYPosition_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSpillPoint_Public_Boolean_byref_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSpillPoint_Public_Boolean_byref_Vector3_byref_Single_Single_LEVEL_COMPENSATION_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSpillPointGizmo_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BakeRotation_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CenterPivot_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CenterPivot_Public_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RefreshMeshAndCollider_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Redraw_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckMeshDisplacement_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RestoreOriginalMesh_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyFrom_Public_Void_LiquidVolume_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static bool FORCE_GLES_COMPATIBILITY
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FORCE_GLES_COMPATIBILITY, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FORCE_GLES_COMPATIBILITY, (void*)(&flag));
		}
	}

	public unsafe PropertiesChangedEvent onPropertiesChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onPropertiesChanged);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PropertiesChangedEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onPropertiesChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)propertiesChangedEvent));
		}
	}

	public unsafe TOPOLOGY _topology
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__topology);
			return *(TOPOLOGY*)num;
		}
		set
		{
			*(TOPOLOGY*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__topology)) = tOPOLOGY;
		}
	}

	public unsafe DETAIL _detail
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__detail);
			return *(DETAIL*)num;
		}
		set
		{
			*(DETAIL*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__detail)) = dETAIL;
		}
	}

	public unsafe float _level
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__level);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__level)) = num;
		}
	}

	public unsafe float _levelMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__levelMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__levelMultiplier)) = num;
		}
	}

	public unsafe bool _useLightColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__useLightColor);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__useLightColor)) = flag;
		}
	}

	public unsafe bool _useLightDirection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__useLightDirection);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__useLightDirection)) = flag;
		}
	}

	public unsafe Light _directionalLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__directionalLight);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Light>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__directionalLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light));
		}
	}

	public unsafe Color _liquidColor1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidColor1);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidColor1)) = color;
		}
	}

	public unsafe float _liquidScale1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidScale1);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidScale1)) = num;
		}
	}

	public unsafe Color _liquidColor2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidColor2);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidColor2)) = color;
		}
	}

	public unsafe float _liquidScale2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidScale2);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidScale2)) = num;
		}
	}

	public unsafe float _alpha
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__alpha);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__alpha)) = num;
		}
	}

	public unsafe Color _emissionColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__emissionColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__emissionColor)) = color;
		}
	}

	public unsafe bool _ditherShadows
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ditherShadows);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ditherShadows)) = flag;
		}
	}

	public unsafe float _murkiness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__murkiness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__murkiness)) = num;
		}
	}

	public unsafe float _turbulence1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__turbulence1);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__turbulence1)) = num;
		}
	}

	public unsafe float _turbulence2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__turbulence2);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__turbulence2)) = num;
		}
	}

	public unsafe float _frecuency
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frecuency);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frecuency)) = num;
		}
	}

	public unsafe float _speed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__speed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__speed)) = num;
		}
	}

	public unsafe float _sparklingIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sparklingIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sparklingIntensity)) = num;
		}
	}

	public unsafe float _sparklingAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sparklingAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sparklingAmount)) = num;
		}
	}

	public unsafe float _deepObscurance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__deepObscurance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__deepObscurance)) = num;
		}
	}

	public unsafe Color _foamColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamColor)) = color;
		}
	}

	public unsafe float _foamScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamScale)) = num;
		}
	}

	public unsafe float _foamThickness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamThickness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamThickness)) = num;
		}
	}

	public unsafe float _foamDensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamDensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamDensity)) = num;
		}
	}

	public unsafe float _foamWeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamWeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamWeight)) = num;
		}
	}

	public unsafe float _foamTurbulence
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamTurbulence);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamTurbulence)) = num;
		}
	}

	public unsafe bool _foamVisibleFromBottom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamVisibleFromBottom);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamVisibleFromBottom)) = flag;
		}
	}

	public unsafe bool _smokeEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeEnabled)) = flag;
		}
	}

	public unsafe Color _smokeColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeColor)) = color;
		}
	}

	public unsafe float _smokeScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeScale)) = num;
		}
	}

	public unsafe float _smokeBaseObscurance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeBaseObscurance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeBaseObscurance)) = num;
		}
	}

	public unsafe float _smokeHeightAtten
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeHeightAtten);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeHeightAtten)) = num;
		}
	}

	public unsafe float _smokeSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeSpeed)) = num;
		}
	}

	public unsafe bool _fixMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fixMesh);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fixMesh)) = flag;
		}
	}

	public unsafe Mesh originalMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalMesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe Vector3 originalPivotOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalPivotOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalPivotOffset)) = vector;
		}
	}

	public unsafe Vector3 _pivotOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pivotOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pivotOffset)) = vector;
		}
	}

	public unsafe bool _limitVerticalRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__limitVerticalRange);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__limitVerticalRange)) = flag;
		}
	}

	public unsafe float _upperLimit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__upperLimit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__upperLimit)) = num;
		}
	}

	public unsafe float _lowerLimit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lowerLimit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lowerLimit)) = num;
		}
	}

	public unsafe int _subMeshIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__subMeshIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__subMeshIndex)) = num;
		}
	}

	public unsafe Material _flaskMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flaskMaterial);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flaskMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe float _flaskThickness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flaskThickness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flaskThickness)) = num;
		}
	}

	public unsafe float _glossinessInternal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__glossinessInternal);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__glossinessInternal)) = num;
		}
	}

	public unsafe bool _scatteringEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scatteringEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scatteringEnabled)) = flag;
		}
	}

	public unsafe int _scatteringPower
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scatteringPower);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scatteringPower)) = num;
		}
	}

	public unsafe float _scatteringAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scatteringAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scatteringAmount)) = num;
		}
	}

	public unsafe bool _refractionBlur
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__refractionBlur);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__refractionBlur)) = flag;
		}
	}

	public unsafe float _blurIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__blurIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__blurIntensity)) = num;
		}
	}

	public unsafe int _liquidRaySteps
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidRaySteps);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__liquidRaySteps)) = num;
		}
	}

	public unsafe int _foamRaySteps
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamRaySteps);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__foamRaySteps)) = num;
		}
	}

	public unsafe int _smokeRaySteps
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeRaySteps);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smokeRaySteps)) = num;
		}
	}

	public unsafe Texture2D _bumpMap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpMap);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpMap)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe float _bumpStrength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpStrength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpStrength)) = num;
		}
	}

	public unsafe float _bumpDistortionScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpDistortionScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpDistortionScale)) = num;
		}
	}

	public unsafe Vector2 _bumpDistortionOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpDistortionOffset);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bumpDistortionOffset)) = vector;
		}
	}

	public unsafe Texture2D _distortionMap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__distortionMap);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__distortionMap)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Texture2D _texture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__texture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__texture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Vector2 _textureScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureScale);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureScale)) = vector;
		}
	}

	public unsafe Vector2 _textureOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureOffset);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureOffset)) = vector;
		}
	}

	public unsafe float _distortionAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__distortionAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__distortionAmount)) = num;
		}
	}

	public unsafe bool _depthAware
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAware);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAware)) = flag;
		}
	}

	public unsafe float _depthAwareOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAwareOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAwareOffset)) = num;
		}
	}

	public unsafe bool _irregularDepthDebug
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__irregularDepthDebug);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__irregularDepthDebug)) = flag;
		}
	}

	public unsafe bool _depthAwareCustomPass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAwareCustomPass);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAwareCustomPass)) = flag;
		}
	}

	public unsafe bool _depthAwareCustomPassDebug
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAwareCustomPassDebug);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__depthAwareCustomPassDebug)) = flag;
		}
	}

	public unsafe float _doubleSidedBias
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__doubleSidedBias);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__doubleSidedBias)) = num;
		}
	}

	public unsafe float _backDepthBias
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__backDepthBias);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__backDepthBias)) = num;
		}
	}

	public unsafe LEVEL_COMPENSATION _rotationLevelCompensation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rotationLevelCompensation);
			return *(LEVEL_COMPENSATION*)num;
		}
		set
		{
			*(LEVEL_COMPENSATION*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rotationLevelCompensation)) = lEVEL_COMPENSATION;
		}
	}

	public unsafe bool _ignoreGravity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ignoreGravity);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ignoreGravity)) = flag;
		}
	}

	public unsafe bool _reactToForces
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reactToForces);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reactToForces)) = flag;
		}
	}

	public unsafe Vector3 _extentsScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__extentsScale);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__extentsScale)) = vector;
		}
	}

	public unsafe int _noiseVariation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__noiseVariation);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__noiseVariation)) = num;
		}
	}

	public unsafe bool _allowViewFromInside
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allowViewFromInside);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allowViewFromInside)) = flag;
		}
	}

	public unsafe bool _debugSpillPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugSpillPoint);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugSpillPoint)) = flag;
		}
	}

	public unsafe int _renderQueue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__renderQueue);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__renderQueue)) = num;
		}
	}

	public unsafe Cubemap _reflectionTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reflectionTexture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Cubemap>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reflectionTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cubemap));
		}
	}

	public unsafe float _physicsMass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__physicsMass);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__physicsMass)) = num;
		}
	}

	public unsafe float _physicsAngularDamp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__physicsAngularDamp);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__physicsAngularDamp)) = num;
		}
	}

	public unsafe static int SHADER_KEYWORD_DEPTH_AWARE_INDEX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_INDEX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_INDEX, (void*)(&num));
		}
	}

	public unsafe static int SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX, (void*)(&num));
		}
	}

	public unsafe static int SHADER_KEYWORD_IGNORE_GRAVITY_INDEX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY_INDEX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY_INDEX, (void*)(&num));
		}
	}

	public unsafe static int SHADER_KEYWORD_NON_AABB_INDEX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB_INDEX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB_INDEX, (void*)(&num));
		}
	}

	public unsafe static int SHADER_KEYWORD_TOPOLOGY_INDEX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_TOPOLOGY_INDEX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_TOPOLOGY_INDEX, (void*)(&num));
		}
	}

	public unsafe static int SHADER_KEYWORD_REFRACTION_INDEX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_REFRACTION_INDEX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_REFRACTION_INDEX, (void*)(&num));
		}
	}

	public unsafe static string SHADER_KEYWORD_DEPTH_AWARE
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_NON_AABB
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_IGNORE_GRAVITY
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_SPHERE
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_SPHERE, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_SPHERE, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_CUBE
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_CUBE, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_CUBE, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_CYLINDER
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_CYLINDER, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_CYLINDER, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_IRREGULAR
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_IRREGULAR, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_IRREGULAR, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_FP_RENDER_TEXTURE
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_FP_RENDER_TEXTURE, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_FP_RENDER_TEXTURE, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SHADER_KEYWORD_USE_REFRACTION
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SHADER_KEYWORD_USE_REFRACTION, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SHADER_KEYWORD_USE_REFRACTION, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string SPILL_POINT_GIZMO
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SPILL_POINT_GIZMO, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SPILL_POINT_GIZMO, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Material liqMat
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liqMat);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liqMat)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material liqMatSimple
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liqMatSimple);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liqMatSimple)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material liqMatDefaultNoFlask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liqMatDefaultNoFlask);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liqMatDefaultNoFlask)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Mesh mesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe Renderer mr
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mr);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Renderer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mr)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer));
		}
	}

	public unsafe static List<Material> mrSharedMaterials
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_mrSharedMaterials, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Material>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_mrSharedMaterials, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Vector3 lastPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastPosition)) = vector;
		}
	}

	public unsafe Vector3 lastScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastScale);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastScale)) = vector;
		}
	}

	public unsafe Quaternion lastRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastRotation);
			return *(Quaternion*)num;
		}
		set
		{
			*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastRotation)) = quaternion;
		}
	}

	public unsafe Il2CppStringArray shaderKeywords
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shaderKeywords);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shaderKeywords)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool camInside
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_camInside);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_camInside)) = flag;
		}
	}

	public unsafe float lastDistanceToCam
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastDistanceToCam);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastDistanceToCam)) = num;
		}
	}

	public unsafe DETAIL currentDetail
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentDetail);
			return *(DETAIL*)num;
		}
		set
		{
			*(DETAIL*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentDetail)) = dETAIL;
		}
	}

	public unsafe Vector4 turb
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turb);
			return *(Vector4*)num;
		}
		set
		{
			*(Vector4*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turb)) = vector;
		}
	}

	public unsafe Vector4 shaderTurb
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shaderTurb);
			return *(Vector4*)num;
		}
		set
		{
			*(Vector4*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shaderTurb)) = vector;
		}
	}

	public unsafe float turbulenceSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turbulenceSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turbulenceSpeed)) = num;
		}
	}

	public unsafe float murkinessSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_murkinessSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_murkinessSpeed)) = num;
		}
	}

	public unsafe float liquidLevelPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liquidLevelPos);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liquidLevelPos)) = num;
		}
	}

	public unsafe bool shouldUpdateMaterialProperties
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shouldUpdateMaterialProperties);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shouldUpdateMaterialProperties)) = flag;
		}
	}

	public unsafe int currentNoiseVariation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentNoiseVariation);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentNoiseVariation)) = num;
		}
	}

	public unsafe float levelMultipled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_levelMultipled);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_levelMultipled)) = num;
		}
	}

	public unsafe Texture2D noise3DUnwrapped
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noise3DUnwrapped);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noise3DUnwrapped)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Il2CppReferenceArray<Texture3D> noise3DTex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noise3DTex);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Texture3D>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noise3DTex)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<Il2CppStructArray<Color>> colors3D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colors3D);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Il2CppStructArray<Color>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colors3D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector3> verticesUnsorted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesUnsorted);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesUnsorted)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector3> verticesSorted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesSorted);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesSorted)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<Vector3> rotatedVertices
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_rotatedVertices, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_rotatedVertices, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<int> verticesIndices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesIndices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticesIndices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float volumeRef
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_volumeRef);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_volumeRef)) = num;
		}
	}

	public unsafe float lastLevelVolumeRef
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastLevelVolumeRef);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastLevelVolumeRef)) = num;
		}
	}

	public unsafe Vector3 inertia
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_inertia);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_inertia)) = vector;
		}
	}

	public unsafe Vector3 lastAvgVelocity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastAvgVelocity);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastAvgVelocity)) = vector;
		}
	}

	public unsafe float angularVelocity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_angularVelocity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_angularVelocity)) = num;
		}
	}

	public unsafe float angularInertia
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_angularInertia);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_angularInertia)) = num;
		}
	}

	public unsafe float turbulenceDueForces
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turbulenceDueForces);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turbulenceDueForces)) = num;
		}
	}

	public unsafe Quaternion liquidRot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liquidRot);
			return *(Quaternion*)num;
		}
		set
		{
			*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_liquidRot)) = quaternion;
		}
	}

	public unsafe float prevThickness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_prevThickness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_prevThickness)) = num;
		}
	}

	public unsafe GameObject spillPointGizmo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spillPointGizmo);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spillPointGizmo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe static Il2CppStringArray defaultContainerNames
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_defaultContainerNames, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_defaultContainerNames, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Color> pointLightColorBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pointLightColorBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Color>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pointLightColorBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector4> pointLightPositionBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pointLightPositionBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector4>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pointLightPositionBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int lastPointLightCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastPointLightCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastPointLightCount)) = num;
		}
	}

	public unsafe static Dictionary<Mesh, MeshCache> meshCache
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_meshCache, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Mesh, MeshCache>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_meshCache, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe List<Vector3> verts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verts);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<Vector3> cutPoints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutPoints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutPoints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Vector3 cutPlaneCenter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutPlaneCenter);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutPlaneCenter)) = vector;
		}
	}

	public unsafe Mesh fixedMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fixedMesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fixedMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe TOPOLOGY topology
	{
		[CallerCount(29)]
		[CachedScanResults(RefRangeStart = 29072, RefRangeEnd = 29101, XrefRangeStart = 29072, XrefRangeEnd = 29101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_topology_Public_get_TOPOLOGY_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(TOPOLOGY*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 75272, RefRangeEnd = 75274, XrefRangeStart = 75267, XrefRangeEnd = 75272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_topology_Public_set_Void_TOPOLOGY_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe DETAIL detail
	{
		[CallerCount(27)]
		[CachedScanResults(RefRangeStart = 70643, RefRangeEnd = 70670, XrefRangeStart = 70643, XrefRangeEnd = 70670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_detail_Public_get_DETAIL_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(DETAIL*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75274, XrefRangeEnd = 75279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_detail_Public_set_Void_DETAIL_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float level
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29130, RefRangeEnd = 29131, XrefRangeStart = 29130, XrefRangeEnd = 29131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_level_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 75284, RefRangeEnd = 75285, XrefRangeStart = 75279, XrefRangeEnd = 75284, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_level_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float levelMultiplier
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_levelMultiplier_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75285, XrefRangeEnd = 75290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_levelMultiplier_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool useLightColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_useLightColor_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75290, XrefRangeEnd = 75295, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_useLightColor_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool useLightDirection
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_useLightDirection_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75295, XrefRangeEnd = 75300, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_useLightDirection_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Light directionalLight
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30474, RefRangeEnd = 30475, XrefRangeStart = 30474, XrefRangeEnd = 30475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_directionalLight_Public_get_Light_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Light>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 75310, RefRangeEnd = 75311, XrefRangeStart = 75300, XrefRangeEnd = 75310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_directionalLight_Public_set_Void_Light_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color liquidColor1
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_liquidColor1_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 75316, RefRangeEnd = 75321, XrefRangeStart = 75311, XrefRangeEnd = 75316, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_liquidColor1_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float liquidScale1
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_liquidScale1_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75321, XrefRangeEnd = 75326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_liquidScale1_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color liquidColor2
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_liquidColor2_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 75331, RefRangeEnd = 75336, XrefRangeStart = 75326, XrefRangeEnd = 75331, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_liquidColor2_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float liquidScale2
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_liquidScale2_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75336, XrefRangeEnd = 75341, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_liquidScale2_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float alpha
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_alpha_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75341, XrefRangeEnd = 75349, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_alpha_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color emissionColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_emissionColor_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75349, XrefRangeEnd = 75354, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_emissionColor_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ditherShadows
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ditherShadows_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75354, XrefRangeEnd = 75359, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ditherShadows_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float murkiness
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_murkiness_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75359, XrefRangeEnd = 75364, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_murkiness_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float turbulence1
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_turbulence1_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75364, XrefRangeEnd = 75369, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_turbulence1_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float turbulence2
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_turbulence2_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75369, XrefRangeEnd = 75374, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_turbulence2_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float frecuency
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_frecuency_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75374, XrefRangeEnd = 75379, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_frecuency_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float speed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_speed_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75379, XrefRangeEnd = 75384, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_speed_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float sparklingIntensity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_sparklingIntensity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75384, XrefRangeEnd = 75389, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_sparklingIntensity_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float sparklingAmount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_sparklingAmount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75389, XrefRangeEnd = 75394, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_sparklingAmount_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float deepObscurance
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_deepObscurance_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75394, XrefRangeEnd = 75399, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_deepObscurance_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color foamColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamColor_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75399, XrefRangeEnd = 75404, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamColor_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float foamScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75404, XrefRangeEnd = 75409, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamScale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float foamThickness
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamThickness_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75409, XrefRangeEnd = 75414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamThickness_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float foamDensity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamDensity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75414, XrefRangeEnd = 75419, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamDensity_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float foamWeight
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamWeight_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75419, XrefRangeEnd = 75424, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamWeight_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float foamTurbulence
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamTurbulence_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75424, XrefRangeEnd = 75429, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamTurbulence_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool foamVisibleFromBottom
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamVisibleFromBottom_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75429, XrefRangeEnd = 75434, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamVisibleFromBottom_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool smokeEnabled
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeEnabled_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75434, XrefRangeEnd = 75439, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeEnabled_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color smokeColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeColor_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75439, XrefRangeEnd = 75444, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeColor_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float smokeScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75444, XrefRangeEnd = 75449, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeScale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float smokeBaseObscurance
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeBaseObscurance_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75449, XrefRangeEnd = 75454, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeBaseObscurance_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float smokeHeightAtten
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeHeightAtten_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75454, XrefRangeEnd = 75459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeHeightAtten_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float smokeSpeed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeSpeed_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75459, XrefRangeEnd = 75464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeSpeed_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool fixMesh
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_fixMesh_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75464, XrefRangeEnd = 75469, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_fixMesh_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector3 pivotOffset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_pivotOffset_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75469, XrefRangeEnd = 75474, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_pivotOffset_Public_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool limitVerticalRange
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_limitVerticalRange_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75474, XrefRangeEnd = 75479, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_limitVerticalRange_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float upperLimit
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 75479, RefRangeEnd = 75481, XrefRangeStart = 75479, XrefRangeEnd = 75479, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_upperLimit_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75481, XrefRangeEnd = 75486, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_upperLimit_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float lowerLimit
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 75486, RefRangeEnd = 75489, XrefRangeStart = 75486, XrefRangeEnd = 75486, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_lowerLimit_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75489, XrefRangeEnd = 75494, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_lowerLimit_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int subMeshIndex
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_subMeshIndex_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75494, XrefRangeEnd = 75499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_subMeshIndex_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Material flaskMaterial
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_flaskMaterial_Public_get_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75499, XrefRangeEnd = 75509, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_flaskMaterial_Public_set_Void_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float flaskThickness
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_flaskThickness_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75509, XrefRangeEnd = 75514, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_flaskThickness_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float glossinessInternal
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_glossinessInternal_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75514, XrefRangeEnd = 75519, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_glossinessInternal_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool scatteringEnabled
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_scatteringEnabled_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75519, XrefRangeEnd = 75524, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_scatteringEnabled_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int scatteringPower
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_scatteringPower_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75524, XrefRangeEnd = 75529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_scatteringPower_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float scatteringAmount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_scatteringAmount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75529, XrefRangeEnd = 75534, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_scatteringAmount_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool refractionBlur
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_refractionBlur_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75534, XrefRangeEnd = 75539, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_refractionBlur_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float blurIntensity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_blurIntensity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75539, XrefRangeEnd = 75547, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_blurIntensity_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int liquidRaySteps
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_liquidRaySteps_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75547, XrefRangeEnd = 75552, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_liquidRaySteps_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int foamRaySteps
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_foamRaySteps_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75552, XrefRangeEnd = 75557, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_foamRaySteps_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int smokeRaySteps
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_smokeRaySteps_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75557, XrefRangeEnd = 75562, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_smokeRaySteps_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Texture2D bumpMap
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bumpMap_Public_get_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75562, XrefRangeEnd = 75572, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_bumpMap_Public_set_Void_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float bumpStrength
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bumpStrength_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75572, XrefRangeEnd = 75577, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_bumpStrength_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float bumpDistortionScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bumpDistortionScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75577, XrefRangeEnd = 75582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_bumpDistortionScale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector2 bumpDistortionOffset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bumpDistortionOffset_Public_get_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75582, XrefRangeEnd = 75587, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_bumpDistortionOffset_Public_set_Void_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Texture2D distortionMap
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_distortionMap_Public_get_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75587, XrefRangeEnd = 75597, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_distortionMap_Public_set_Void_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Texture2D texture
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_texture_Public_get_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75597, XrefRangeEnd = 75607, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_texture_Public_set_Void_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector2 textureScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_textureScale_Public_get_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75607, XrefRangeEnd = 75612, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_textureScale_Public_set_Void_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector2 textureOffset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_textureOffset_Public_get_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75612, XrefRangeEnd = 75617, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_textureOffset_Public_set_Void_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float distortionAmount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_distortionAmount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75617, XrefRangeEnd = 75622, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_distortionAmount_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool depthAware
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_depthAware_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75622, XrefRangeEnd = 75627, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_depthAware_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float depthAwareOffset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_depthAwareOffset_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75627, XrefRangeEnd = 75632, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_depthAwareOffset_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool irregularDepthDebug
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_irregularDepthDebug_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75632, XrefRangeEnd = 75637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_irregularDepthDebug_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool depthAwareCustomPass
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_depthAwareCustomPass_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75637, XrefRangeEnd = 75642, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_depthAwareCustomPass_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool depthAwareCustomPassDebug
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_depthAwareCustomPassDebug_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75642, XrefRangeEnd = 75647, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_depthAwareCustomPassDebug_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float doubleSidedBias
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_doubleSidedBias_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75647, XrefRangeEnd = 75652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_doubleSidedBias_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float backDepthBias
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_backDepthBias_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75652, XrefRangeEnd = 75657, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_backDepthBias_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe LEVEL_COMPENSATION rotationLevelCompensation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_rotationLevelCompensation_Public_get_LEVEL_COMPENSATION_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(LEVEL_COMPENSATION*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75657, XrefRangeEnd = 75662, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_rotationLevelCompensation_Public_set_Void_LEVEL_COMPENSATION_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ignoreGravity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ignoreGravity_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75662, XrefRangeEnd = 75667, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ignoreGravity_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool reactToForces
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_reactToForces_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75667, XrefRangeEnd = 75672, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_reactToForces_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector3 extentsScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_extentsScale_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75672, XrefRangeEnd = 75677, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_extentsScale_Public_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int noiseVariation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_noiseVariation_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75677, XrefRangeEnd = 75682, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_noiseVariation_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool allowViewFromInside
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_allowViewFromInside_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75682, XrefRangeEnd = 75683, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_allowViewFromInside_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool debugSpillPoint
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_debugSpillPoint_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_debugSpillPoint_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int renderQueue
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_renderQueue_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75683, XrefRangeEnd = 75688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_renderQueue_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Cubemap reflectionTexture
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 75688, RefRangeEnd = 75698, XrefRangeStart = 75688, XrefRangeEnd = 75688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_reflectionTexture_Public_get_Cubemap_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Cubemap>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75698, XrefRangeEnd = 75708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_reflectionTexture_Public_set_Void_Cubemap_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float physicsMass
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_physicsMass_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75708, XrefRangeEnd = 75713, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_physicsMass_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float physicsAngularDamp
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_physicsAngularDamp_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75713, XrefRangeEnd = 75718, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_physicsAngularDamp_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool useFPRenderTextures
	{
		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_useFPRenderTextures_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float liquidSurfaceYPosition
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_liquidSurfaceYPosition_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static LiquidVolume()
	{
		Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "LiquidVolumeFX", "LiquidVolume");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr);
		NativeFieldInfoPtr_FORCE_GLES_COMPATIBILITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "FORCE_GLES_COMPATIBILITY");
		NativeFieldInfoPtr_onPropertiesChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "onPropertiesChanged");
		NativeFieldInfoPtr__topology = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_topology");
		NativeFieldInfoPtr__detail = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_detail");
		NativeFieldInfoPtr__level = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_level");
		NativeFieldInfoPtr__levelMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_levelMultiplier");
		NativeFieldInfoPtr__useLightColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_useLightColor");
		NativeFieldInfoPtr__useLightDirection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_useLightDirection");
		NativeFieldInfoPtr__directionalLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_directionalLight");
		NativeFieldInfoPtr__liquidColor1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_liquidColor1");
		NativeFieldInfoPtr__liquidScale1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_liquidScale1");
		NativeFieldInfoPtr__liquidColor2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_liquidColor2");
		NativeFieldInfoPtr__liquidScale2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_liquidScale2");
		NativeFieldInfoPtr__alpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_alpha");
		NativeFieldInfoPtr__emissionColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_emissionColor");
		NativeFieldInfoPtr__ditherShadows = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_ditherShadows");
		NativeFieldInfoPtr__murkiness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_murkiness");
		NativeFieldInfoPtr__turbulence1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_turbulence1");
		NativeFieldInfoPtr__turbulence2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_turbulence2");
		NativeFieldInfoPtr__frecuency = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_frecuency");
		NativeFieldInfoPtr__speed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_speed");
		NativeFieldInfoPtr__sparklingIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_sparklingIntensity");
		NativeFieldInfoPtr__sparklingAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_sparklingAmount");
		NativeFieldInfoPtr__deepObscurance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_deepObscurance");
		NativeFieldInfoPtr__foamColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamColor");
		NativeFieldInfoPtr__foamScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamScale");
		NativeFieldInfoPtr__foamThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamThickness");
		NativeFieldInfoPtr__foamDensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamDensity");
		NativeFieldInfoPtr__foamWeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamWeight");
		NativeFieldInfoPtr__foamTurbulence = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamTurbulence");
		NativeFieldInfoPtr__foamVisibleFromBottom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamVisibleFromBottom");
		NativeFieldInfoPtr__smokeEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeEnabled");
		NativeFieldInfoPtr__smokeColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeColor");
		NativeFieldInfoPtr__smokeScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeScale");
		NativeFieldInfoPtr__smokeBaseObscurance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeBaseObscurance");
		NativeFieldInfoPtr__smokeHeightAtten = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeHeightAtten");
		NativeFieldInfoPtr__smokeSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeSpeed");
		NativeFieldInfoPtr__fixMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_fixMesh");
		NativeFieldInfoPtr_originalMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "originalMesh");
		NativeFieldInfoPtr_originalPivotOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "originalPivotOffset");
		NativeFieldInfoPtr__pivotOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_pivotOffset");
		NativeFieldInfoPtr__limitVerticalRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_limitVerticalRange");
		NativeFieldInfoPtr__upperLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_upperLimit");
		NativeFieldInfoPtr__lowerLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_lowerLimit");
		NativeFieldInfoPtr__subMeshIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_subMeshIndex");
		NativeFieldInfoPtr__flaskMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_flaskMaterial");
		NativeFieldInfoPtr__flaskThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_flaskThickness");
		NativeFieldInfoPtr__glossinessInternal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_glossinessInternal");
		NativeFieldInfoPtr__scatteringEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_scatteringEnabled");
		NativeFieldInfoPtr__scatteringPower = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_scatteringPower");
		NativeFieldInfoPtr__scatteringAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_scatteringAmount");
		NativeFieldInfoPtr__refractionBlur = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_refractionBlur");
		NativeFieldInfoPtr__blurIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_blurIntensity");
		NativeFieldInfoPtr__liquidRaySteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_liquidRaySteps");
		NativeFieldInfoPtr__foamRaySteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_foamRaySteps");
		NativeFieldInfoPtr__smokeRaySteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_smokeRaySteps");
		NativeFieldInfoPtr__bumpMap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_bumpMap");
		NativeFieldInfoPtr__bumpStrength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_bumpStrength");
		NativeFieldInfoPtr__bumpDistortionScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_bumpDistortionScale");
		NativeFieldInfoPtr__bumpDistortionOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_bumpDistortionOffset");
		NativeFieldInfoPtr__distortionMap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_distortionMap");
		NativeFieldInfoPtr__texture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_texture");
		NativeFieldInfoPtr__textureScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_textureScale");
		NativeFieldInfoPtr__textureOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_textureOffset");
		NativeFieldInfoPtr__distortionAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_distortionAmount");
		NativeFieldInfoPtr__depthAware = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_depthAware");
		NativeFieldInfoPtr__depthAwareOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_depthAwareOffset");
		NativeFieldInfoPtr__irregularDepthDebug = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_irregularDepthDebug");
		NativeFieldInfoPtr__depthAwareCustomPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_depthAwareCustomPass");
		NativeFieldInfoPtr__depthAwareCustomPassDebug = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_depthAwareCustomPassDebug");
		NativeFieldInfoPtr__doubleSidedBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_doubleSidedBias");
		NativeFieldInfoPtr__backDepthBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_backDepthBias");
		NativeFieldInfoPtr__rotationLevelCompensation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_rotationLevelCompensation");
		NativeFieldInfoPtr__ignoreGravity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_ignoreGravity");
		NativeFieldInfoPtr__reactToForces = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_reactToForces");
		NativeFieldInfoPtr__extentsScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_extentsScale");
		NativeFieldInfoPtr__noiseVariation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_noiseVariation");
		NativeFieldInfoPtr__allowViewFromInside = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_allowViewFromInside");
		NativeFieldInfoPtr__debugSpillPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_debugSpillPoint");
		NativeFieldInfoPtr__renderQueue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_renderQueue");
		NativeFieldInfoPtr__reflectionTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_reflectionTexture");
		NativeFieldInfoPtr__physicsMass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_physicsMass");
		NativeFieldInfoPtr__physicsAngularDamp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "_physicsAngularDamp");
		NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_INDEX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_DEPTH_AWARE_INDEX");
		NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS_INDEX");
		NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY_INDEX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_IGNORE_GRAVITY_INDEX");
		NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB_INDEX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_NON_AABB_INDEX");
		NativeFieldInfoPtr_SHADER_KEYWORD_TOPOLOGY_INDEX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_TOPOLOGY_INDEX");
		NativeFieldInfoPtr_SHADER_KEYWORD_REFRACTION_INDEX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_REFRACTION_INDEX");
		NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_DEPTH_AWARE");
		NativeFieldInfoPtr_SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_DEPTH_AWARE_CUSTOM_PASS");
		NativeFieldInfoPtr_SHADER_KEYWORD_NON_AABB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_NON_AABB");
		NativeFieldInfoPtr_SHADER_KEYWORD_IGNORE_GRAVITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_IGNORE_GRAVITY");
		NativeFieldInfoPtr_SHADER_KEYWORD_SPHERE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_SPHERE");
		NativeFieldInfoPtr_SHADER_KEYWORD_CUBE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_CUBE");
		NativeFieldInfoPtr_SHADER_KEYWORD_CYLINDER = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_CYLINDER");
		NativeFieldInfoPtr_SHADER_KEYWORD_IRREGULAR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_IRREGULAR");
		NativeFieldInfoPtr_SHADER_KEYWORD_FP_RENDER_TEXTURE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_FP_RENDER_TEXTURE");
		NativeFieldInfoPtr_SHADER_KEYWORD_USE_REFRACTION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SHADER_KEYWORD_USE_REFRACTION");
		NativeFieldInfoPtr_SPILL_POINT_GIZMO = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "SPILL_POINT_GIZMO");
		NativeFieldInfoPtr_liqMat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "liqMat");
		NativeFieldInfoPtr_liqMatSimple = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "liqMatSimple");
		NativeFieldInfoPtr_liqMatDefaultNoFlask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "liqMatDefaultNoFlask");
		NativeFieldInfoPtr_mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "mesh");
		NativeFieldInfoPtr_mr = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "mr");
		NativeFieldInfoPtr_mrSharedMaterials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "mrSharedMaterials");
		NativeFieldInfoPtr_lastPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastPosition");
		NativeFieldInfoPtr_lastScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastScale");
		NativeFieldInfoPtr_lastRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastRotation");
		NativeFieldInfoPtr_shaderKeywords = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "shaderKeywords");
		NativeFieldInfoPtr_camInside = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "camInside");
		NativeFieldInfoPtr_lastDistanceToCam = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastDistanceToCam");
		NativeFieldInfoPtr_currentDetail = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "currentDetail");
		NativeFieldInfoPtr_turb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "turb");
		NativeFieldInfoPtr_shaderTurb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "shaderTurb");
		NativeFieldInfoPtr_turbulenceSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "turbulenceSpeed");
		NativeFieldInfoPtr_murkinessSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "murkinessSpeed");
		NativeFieldInfoPtr_liquidLevelPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "liquidLevelPos");
		NativeFieldInfoPtr_shouldUpdateMaterialProperties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "shouldUpdateMaterialProperties");
		NativeFieldInfoPtr_currentNoiseVariation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "currentNoiseVariation");
		NativeFieldInfoPtr_levelMultipled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "levelMultipled");
		NativeFieldInfoPtr_noise3DUnwrapped = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "noise3DUnwrapped");
		NativeFieldInfoPtr_noise3DTex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "noise3DTex");
		NativeFieldInfoPtr_colors3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "colors3D");
		NativeFieldInfoPtr_verticesUnsorted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "verticesUnsorted");
		NativeFieldInfoPtr_verticesSorted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "verticesSorted");
		NativeFieldInfoPtr_rotatedVertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "rotatedVertices");
		NativeFieldInfoPtr_verticesIndices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "verticesIndices");
		NativeFieldInfoPtr_volumeRef = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "volumeRef");
		NativeFieldInfoPtr_lastLevelVolumeRef = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastLevelVolumeRef");
		NativeFieldInfoPtr_inertia = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "inertia");
		NativeFieldInfoPtr_lastAvgVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastAvgVelocity");
		NativeFieldInfoPtr_angularVelocity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "angularVelocity");
		NativeFieldInfoPtr_angularInertia = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "angularInertia");
		NativeFieldInfoPtr_turbulenceDueForces = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "turbulenceDueForces");
		NativeFieldInfoPtr_liquidRot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "liquidRot");
		NativeFieldInfoPtr_prevThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "prevThickness");
		NativeFieldInfoPtr_spillPointGizmo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "spillPointGizmo");
		NativeFieldInfoPtr_defaultContainerNames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "defaultContainerNames");
		NativeFieldInfoPtr_pointLightColorBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "pointLightColorBuffer");
		NativeFieldInfoPtr_pointLightPositionBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "pointLightPositionBuffer");
		NativeFieldInfoPtr_lastPointLightCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "lastPointLightCount");
		NativeFieldInfoPtr_meshCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "meshCache");
		NativeFieldInfoPtr_verts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "verts");
		NativeFieldInfoPtr_cutPoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "cutPoints");
		NativeFieldInfoPtr_cutPlaneCenter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "cutPlaneCenter");
		NativeFieldInfoPtr_fixedMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, "fixedMesh");
		NativeMethodInfoPtr_add_onPropertiesChanged_Public_add_Void_PropertiesChangedEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664513);
		NativeMethodInfoPtr_remove_onPropertiesChanged_Public_rem_Void_PropertiesChangedEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664514);
		NativeMethodInfoPtr_get_topology_Public_get_TOPOLOGY_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664515);
		NativeMethodInfoPtr_set_topology_Public_set_Void_TOPOLOGY_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664516);
		NativeMethodInfoPtr_get_detail_Public_get_DETAIL_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664517);
		NativeMethodInfoPtr_set_detail_Public_set_Void_DETAIL_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664518);
		NativeMethodInfoPtr_get_level_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664519);
		NativeMethodInfoPtr_set_level_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664520);
		NativeMethodInfoPtr_get_levelMultiplier_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664521);
		NativeMethodInfoPtr_set_levelMultiplier_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664522);
		NativeMethodInfoPtr_get_useLightColor_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664523);
		NativeMethodInfoPtr_set_useLightColor_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664524);
		NativeMethodInfoPtr_get_useLightDirection_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664525);
		NativeMethodInfoPtr_set_useLightDirection_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664526);
		NativeMethodInfoPtr_get_directionalLight_Public_get_Light_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664527);
		NativeMethodInfoPtr_set_directionalLight_Public_set_Void_Light_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664528);
		NativeMethodInfoPtr_get_liquidColor1_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664529);
		NativeMethodInfoPtr_set_liquidColor1_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664530);
		NativeMethodInfoPtr_get_liquidScale1_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664531);
		NativeMethodInfoPtr_set_liquidScale1_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664532);
		NativeMethodInfoPtr_get_liquidColor2_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664533);
		NativeMethodInfoPtr_set_liquidColor2_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664534);
		NativeMethodInfoPtr_get_liquidScale2_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664535);
		NativeMethodInfoPtr_set_liquidScale2_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664536);
		NativeMethodInfoPtr_get_alpha_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664537);
		NativeMethodInfoPtr_set_alpha_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664538);
		NativeMethodInfoPtr_get_emissionColor_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664539);
		NativeMethodInfoPtr_set_emissionColor_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664540);
		NativeMethodInfoPtr_get_ditherShadows_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664541);
		NativeMethodInfoPtr_set_ditherShadows_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664542);
		NativeMethodInfoPtr_get_murkiness_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664543);
		NativeMethodInfoPtr_set_murkiness_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664544);
		NativeMethodInfoPtr_get_turbulence1_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664545);
		NativeMethodInfoPtr_set_turbulence1_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664546);
		NativeMethodInfoPtr_get_turbulence2_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664547);
		NativeMethodInfoPtr_set_turbulence2_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664548);
		NativeMethodInfoPtr_get_frecuency_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664549);
		NativeMethodInfoPtr_set_frecuency_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664550);
		NativeMethodInfoPtr_get_speed_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664551);
		NativeMethodInfoPtr_set_speed_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664552);
		NativeMethodInfoPtr_get_sparklingIntensity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664553);
		NativeMethodInfoPtr_set_sparklingIntensity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664554);
		NativeMethodInfoPtr_get_sparklingAmount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664555);
		NativeMethodInfoPtr_set_sparklingAmount_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664556);
		NativeMethodInfoPtr_get_deepObscurance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664557);
		NativeMethodInfoPtr_set_deepObscurance_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664558);
		NativeMethodInfoPtr_get_foamColor_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664559);
		NativeMethodInfoPtr_set_foamColor_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664560);
		NativeMethodInfoPtr_get_foamScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664561);
		NativeMethodInfoPtr_set_foamScale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664562);
		NativeMethodInfoPtr_get_foamThickness_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664563);
		NativeMethodInfoPtr_set_foamThickness_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664564);
		NativeMethodInfoPtr_get_foamDensity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664565);
		NativeMethodInfoPtr_set_foamDensity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664566);
		NativeMethodInfoPtr_get_foamWeight_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664567);
		NativeMethodInfoPtr_set_foamWeight_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664568);
		NativeMethodInfoPtr_get_foamTurbulence_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664569);
		NativeMethodInfoPtr_set_foamTurbulence_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664570);
		NativeMethodInfoPtr_get_foamVisibleFromBottom_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664571);
		NativeMethodInfoPtr_set_foamVisibleFromBottom_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664572);
		NativeMethodInfoPtr_get_smokeEnabled_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664573);
		NativeMethodInfoPtr_set_smokeEnabled_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664574);
		NativeMethodInfoPtr_get_smokeColor_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664575);
		NativeMethodInfoPtr_set_smokeColor_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664576);
		NativeMethodInfoPtr_get_smokeScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664577);
		NativeMethodInfoPtr_set_smokeScale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664578);
		NativeMethodInfoPtr_get_smokeBaseObscurance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664579);
		NativeMethodInfoPtr_set_smokeBaseObscurance_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664580);
		NativeMethodInfoPtr_get_smokeHeightAtten_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664581);
		NativeMethodInfoPtr_set_smokeHeightAtten_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664582);
		NativeMethodInfoPtr_get_smokeSpeed_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664583);
		NativeMethodInfoPtr_set_smokeSpeed_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664584);
		NativeMethodInfoPtr_get_fixMesh_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664585);
		NativeMethodInfoPtr_set_fixMesh_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664586);
		NativeMethodInfoPtr_get_pivotOffset_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664587);
		NativeMethodInfoPtr_set_pivotOffset_Public_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664588);
		NativeMethodInfoPtr_get_limitVerticalRange_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664589);
		NativeMethodInfoPtr_set_limitVerticalRange_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664590);
		NativeMethodInfoPtr_get_upperLimit_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664591);
		NativeMethodInfoPtr_set_upperLimit_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664592);
		NativeMethodInfoPtr_get_lowerLimit_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664593);
		NativeMethodInfoPtr_set_lowerLimit_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664594);
		NativeMethodInfoPtr_get_subMeshIndex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664595);
		NativeMethodInfoPtr_set_subMeshIndex_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664596);
		NativeMethodInfoPtr_get_flaskMaterial_Public_get_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664597);
		NativeMethodInfoPtr_set_flaskMaterial_Public_set_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664598);
		NativeMethodInfoPtr_get_flaskThickness_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664599);
		NativeMethodInfoPtr_set_flaskThickness_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664600);
		NativeMethodInfoPtr_get_glossinessInternal_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664601);
		NativeMethodInfoPtr_set_glossinessInternal_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664602);
		NativeMethodInfoPtr_get_scatteringEnabled_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664603);
		NativeMethodInfoPtr_set_scatteringEnabled_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664604);
		NativeMethodInfoPtr_get_scatteringPower_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664605);
		NativeMethodInfoPtr_set_scatteringPower_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664606);
		NativeMethodInfoPtr_get_scatteringAmount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664607);
		NativeMethodInfoPtr_set_scatteringAmount_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664608);
		NativeMethodInfoPtr_get_refractionBlur_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664609);
		NativeMethodInfoPtr_set_refractionBlur_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664610);
		NativeMethodInfoPtr_get_blurIntensity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664611);
		NativeMethodInfoPtr_set_blurIntensity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664612);
		NativeMethodInfoPtr_get_liquidRaySteps_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664613);
		NativeMethodInfoPtr_set_liquidRaySteps_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664614);
		NativeMethodInfoPtr_get_foamRaySteps_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664615);
		NativeMethodInfoPtr_set_foamRaySteps_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664616);
		NativeMethodInfoPtr_get_smokeRaySteps_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664617);
		NativeMethodInfoPtr_set_smokeRaySteps_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664618);
		NativeMethodInfoPtr_get_bumpMap_Public_get_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664619);
		NativeMethodInfoPtr_set_bumpMap_Public_set_Void_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664620);
		NativeMethodInfoPtr_get_bumpStrength_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664621);
		NativeMethodInfoPtr_set_bumpStrength_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664622);
		NativeMethodInfoPtr_get_bumpDistortionScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664623);
		NativeMethodInfoPtr_set_bumpDistortionScale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664624);
		NativeMethodInfoPtr_get_bumpDistortionOffset_Public_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664625);
		NativeMethodInfoPtr_set_bumpDistortionOffset_Public_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664626);
		NativeMethodInfoPtr_get_distortionMap_Public_get_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664627);
		NativeMethodInfoPtr_set_distortionMap_Public_set_Void_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664628);
		NativeMethodInfoPtr_get_texture_Public_get_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664629);
		NativeMethodInfoPtr_set_texture_Public_set_Void_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664630);
		NativeMethodInfoPtr_get_textureScale_Public_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664631);
		NativeMethodInfoPtr_set_textureScale_Public_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664632);
		NativeMethodInfoPtr_get_textureOffset_Public_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664633);
		NativeMethodInfoPtr_set_textureOffset_Public_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664634);
		NativeMethodInfoPtr_get_distortionAmount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664635);
		NativeMethodInfoPtr_set_distortionAmount_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664636);
		NativeMethodInfoPtr_get_depthAware_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664637);
		NativeMethodInfoPtr_set_depthAware_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664638);
		NativeMethodInfoPtr_get_depthAwareOffset_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664639);
		NativeMethodInfoPtr_set_depthAwareOffset_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664640);
		NativeMethodInfoPtr_get_irregularDepthDebug_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664641);
		NativeMethodInfoPtr_set_irregularDepthDebug_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664642);
		NativeMethodInfoPtr_get_depthAwareCustomPass_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664643);
		NativeMethodInfoPtr_set_depthAwareCustomPass_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664644);
		NativeMethodInfoPtr_get_depthAwareCustomPassDebug_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664645);
		NativeMethodInfoPtr_set_depthAwareCustomPassDebug_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664646);
		NativeMethodInfoPtr_get_doubleSidedBias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664647);
		NativeMethodInfoPtr_set_doubleSidedBias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664648);
		NativeMethodInfoPtr_get_backDepthBias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664649);
		NativeMethodInfoPtr_set_backDepthBias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664650);
		NativeMethodInfoPtr_get_rotationLevelCompensation_Public_get_LEVEL_COMPENSATION_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664651);
		NativeMethodInfoPtr_set_rotationLevelCompensation_Public_set_Void_LEVEL_COMPENSATION_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664652);
		NativeMethodInfoPtr_get_ignoreGravity_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664653);
		NativeMethodInfoPtr_set_ignoreGravity_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664654);
		NativeMethodInfoPtr_get_reactToForces_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664655);
		NativeMethodInfoPtr_set_reactToForces_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664656);
		NativeMethodInfoPtr_get_extentsScale_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664657);
		NativeMethodInfoPtr_set_extentsScale_Public_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664658);
		NativeMethodInfoPtr_get_noiseVariation_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664659);
		NativeMethodInfoPtr_set_noiseVariation_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664660);
		NativeMethodInfoPtr_get_allowViewFromInside_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664661);
		NativeMethodInfoPtr_set_allowViewFromInside_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664662);
		NativeMethodInfoPtr_get_debugSpillPoint_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664663);
		NativeMethodInfoPtr_set_debugSpillPoint_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664664);
		NativeMethodInfoPtr_get_renderQueue_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664665);
		NativeMethodInfoPtr_set_renderQueue_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664666);
		NativeMethodInfoPtr_get_reflectionTexture_Public_get_Cubemap_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664667);
		NativeMethodInfoPtr_set_reflectionTexture_Public_set_Void_Cubemap_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664668);
		NativeMethodInfoPtr_get_physicsMass_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664669);
		NativeMethodInfoPtr_set_physicsMass_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664670);
		NativeMethodInfoPtr_get_physicsAngularDamp_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664671);
		NativeMethodInfoPtr_set_physicsAngularDamp_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664672);
		NativeMethodInfoPtr_get_useFPRenderTextures_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664673);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664674);
		NativeMethodInfoPtr_Reset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664675);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664676);
		NativeMethodInfoPtr_RenderObject_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664677);
		NativeMethodInfoPtr_OnWillRenderObject_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664678);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664679);
		NativeMethodInfoPtr_OnDidApplyAnimationProperties_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664680);
		NativeMethodInfoPtr_ClearMeshCache_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664681);
		NativeMethodInfoPtr_ReadVertices_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664682);
		NativeMethodInfoPtr_vertexComparer_Private_Int32_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664683);
		NativeMethodInfoPtr_UpdateAnimations_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664684);
		NativeMethodInfoPtr_UpdateMaterialProperties_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664685);
		NativeMethodInfoPtr_UpdateMaterialPropertiesNow_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664686);
		NativeMethodInfoPtr_ApplyGlobalAlpha_Private_Color_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664687);
		NativeMethodInfoPtr_GetRenderer_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664688);
		NativeMethodInfoPtr_UpdateLevels_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664689);
		NativeMethodInfoPtr_RotateVertices_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664690);
		NativeMethodInfoPtr_SignedVolumeOfTriangle_Private_Single_Vector3_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664691);
		NativeMethodInfoPtr_GetMeshVolumeUnderLevelFast_Public_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664692);
		NativeMethodInfoPtr_GetMeshVolumeWSFast_Public_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664693);
		NativeMethodInfoPtr_GetMeshVolumeUnderLevelWSFast_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664694);
		NativeMethodInfoPtr_ClampVertexToSlicePlane_Private_Vector3_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664695);
		NativeMethodInfoPtr_GetMeshVolumeUnderLevel_Public_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664696);
		NativeMethodInfoPtr_GetMeshVolumeWS_Public_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664697);
		NativeMethodInfoPtr_GetMeshVolumeUnderLevelWS_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664698);
		NativeMethodInfoPtr_PolygonSortOnPlane_Private_Int32_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664699);
		NativeMethodInfoPtr_UpdateTurbulence_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664700);
		NativeMethodInfoPtr_CheckInsideOut_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664701);
		NativeMethodInfoPtr_PointInAABB_Private_Boolean_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664702);
		NativeMethodInfoPtr_PointInCylinder_Private_Boolean_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664703);
		NativeMethodInfoPtr_UpdateInsideOut_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664704);
		NativeMethodInfoPtr_get_liquidSurfaceYPosition_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664705);
		NativeMethodInfoPtr_GetSpillPoint_Public_Boolean_byref_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664706);
		NativeMethodInfoPtr_GetSpillPoint_Public_Boolean_byref_Vector3_byref_Single_Single_LEVEL_COMPENSATION_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664707);
		NativeMethodInfoPtr_UpdateSpillPointGizmo_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664708);
		NativeMethodInfoPtr_BakeRotation_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664709);
		NativeMethodInfoPtr_CenterPivot_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664710);
		NativeMethodInfoPtr_CenterPivot_Public_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664711);
		NativeMethodInfoPtr_RefreshMeshAndCollider_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664712);
		NativeMethodInfoPtr_Redraw_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664713);
		NativeMethodInfoPtr_CheckMeshDisplacement_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664714);
		NativeMethodInfoPtr_RestoreOriginalMesh_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664715);
		NativeMethodInfoPtr_CopyFrom_Public_Void_LiquidVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664716);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr, 100664717);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75259, XrefRangeEnd = 75263, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void add_onPropertiesChanged(PropertiesChangedEvent value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_onPropertiesChanged_Public_add_Void_PropertiesChangedEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75263, XrefRangeEnd = 75267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void remove_onPropertiesChanged(PropertiesChangedEvent value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_onPropertiesChanged_Public_rem_Void_PropertiesChangedEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75718, XrefRangeEnd = 75739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75739, XrefRangeEnd = 75776, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75776, XrefRangeEnd = 75812, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75882, RefRangeEnd = 75883, XrefRangeStart = 75812, XrefRangeEnd = 75882, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RenderObject()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenderObject_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75883, XrefRangeEnd = 75884, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnWillRenderObject()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnWillRenderObject_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75884, XrefRangeEnd = 75894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnDidApplyAnimationProperties()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDidApplyAnimationProperties_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75902, RefRangeEnd = 75903, XrefRangeStart = 75894, XrefRangeEnd = 75902, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearMeshCache()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearMeshCache_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75926, RefRangeEnd = 75927, XrefRangeStart = 75903, XrefRangeEnd = 75926, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReadVertices()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadVertices_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe int vertexComparer(Vector3 v0, Vector3 v1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&v0);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v1;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_vertexComparer_Private_Int32_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 75996, RefRangeEnd = 75998, XrefRangeStart = 75927, XrefRangeEnd = 75996, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateAnimations()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateAnimations_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75998, XrefRangeEnd = 76003, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateMaterialProperties()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateMaterialProperties_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(81)]
	[CachedScanResults(RefRangeStart = 76287, RefRangeEnd = 76368, XrefRangeStart = 76003, XrefRangeEnd = 76287, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateMaterialPropertiesNow()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateMaterialPropertiesNow_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe Color ApplyGlobalAlpha(Color originalColor)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&originalColor);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyGlobalAlpha_Private_Color_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76385, RefRangeEnd = 76387, XrefRangeStart = 76368, XrefRangeEnd = 76385, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetRenderer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRenderer_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76549, RefRangeEnd = 76551, XrefRangeStart = 76387, XrefRangeEnd = 76549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateLevels(bool updateShaderKeywords = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&updateShaderKeywords);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateLevels_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76572, RefRangeEnd = 76574, XrefRangeStart = 76551, XrefRangeEnd = 76572, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RotateVertices()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RotateVertices_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 zeroPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&p1);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p3;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &zeroPoint;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SignedVolumeOfTriangle_Private_Single_Vector3_Vector3_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76574, XrefRangeEnd = 76577, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetMeshVolumeUnderLevelFast(float level01, float yExtent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&level01);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &yExtent;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMeshVolumeUnderLevelFast_Public_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76577, XrefRangeEnd = 76578, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetMeshVolumeWSFast()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMeshVolumeWSFast_Public_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 76584, RefRangeEnd = 76588, XrefRangeStart = 76578, XrefRangeEnd = 76584, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetMeshVolumeUnderLevelWSFast(float level)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&level);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMeshVolumeUnderLevelWSFast_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76588, XrefRangeEnd = 76589, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 ClampVertexToSlicePlane(Vector3 p, Vector3 q, float level)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&p);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &q;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &level;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampVertexToSlicePlane_Private_Vector3_Vector3_Vector3_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76589, XrefRangeEnd = 76592, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetMeshVolumeUnderLevel(float level01, float yExtent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&level01);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &yExtent;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMeshVolumeUnderLevel_Public_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76592, XrefRangeEnd = 76593, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetMeshVolumeWS()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMeshVolumeWS_Public_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 76714, RefRangeEnd = 76718, XrefRangeStart = 76593, XrefRangeEnd = 76714, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetMeshVolumeUnderLevelWS(float level)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&level);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMeshVolumeUnderLevelWS_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76718, XrefRangeEnd = 76720, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int PolygonSortOnPlane(Vector3 p1, Vector3 p2)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&p1);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PolygonSortOnPlane_Private_Int32_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76730, RefRangeEnd = 76732, XrefRangeStart = 76720, XrefRangeEnd = 76730, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateTurbulence()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateTurbulence_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76762, RefRangeEnd = 76764, XrefRangeStart = 76732, XrefRangeEnd = 76762, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckInsideOut()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckInsideOut_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76764, XrefRangeEnd = 76767, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool PointInAABB(Vector3 point)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&point);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PointInAABB_Private_Boolean_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76767, XrefRangeEnd = 76772, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool PointInCylinder(Vector3 point)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&point);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PointInCylinder_Private_Boolean_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76807, RefRangeEnd = 76809, XrefRangeStart = 76772, XrefRangeEnd = 76807, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateInsideOut()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateInsideOut_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76809, XrefRangeEnd = 76810, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GetSpillPoint(out Vector3 spillPosition, float apertureStart = 1f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref spillPosition);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &apertureStart;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSpillPoint_Public_Boolean_byref_Vector3_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76830, RefRangeEnd = 76832, XrefRangeStart = 76810, XrefRangeEnd = 76830, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GetSpillPoint(out Vector3 spillPosition, out float spillAmount, float apertureStart = 1f, LEVEL_COMPENSATION rotationCompensation = LEVEL_COMPENSATION.None)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)Unsafe.AsPointer(ref spillPosition);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref spillAmount);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &apertureStart;
		*(LEVEL_COMPENSATION**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotationCompensation;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSpillPoint_Public_Boolean_byref_Vector3_byref_Single_Single_LEVEL_COMPENSATION_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 76882, RefRangeEnd = 76883, XrefRangeStart = 76832, XrefRangeEnd = 76882, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSpillPointGizmo()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSpillPointGizmo_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76883, XrefRangeEnd = 76934, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BakeRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BakeRotation_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 76934, XrefRangeEnd = 76937, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CenterPivot()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CenterPivot_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 76968, RefRangeEnd = 76970, XrefRangeStart = 76937, XrefRangeEnd = 76968, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CenterPivot(Vector3 offset)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&offset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CenterPivot_Public_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 76981, RefRangeEnd = 76984, XrefRangeStart = 76970, XrefRangeEnd = 76981, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RefreshMeshAndCollider()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RefreshMeshAndCollider_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Redraw()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Redraw_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 77020, RefRangeEnd = 77021, XrefRangeStart = 76984, XrefRangeEnd = 77020, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckMeshDisplacement()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckMeshDisplacement_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 77037, RefRangeEnd = 77039, XrefRangeStart = 77021, XrefRangeEnd = 77037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RestoreOriginalMesh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RestoreOriginalMesh_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77039, XrefRangeEnd = 77047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CopyFrom(LiquidVolume lv)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyFrom_Public_Void_LiquidVolume_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77047, XrefRangeEnd = 77064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LiquidVolume()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LiquidVolume>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LiquidVolume(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
