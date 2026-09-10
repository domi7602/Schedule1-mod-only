using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Il2CppLiquidVolumeFX;

public class LiquidVolumeDepthPrePassRenderFeature : ScriptableRendererFeature
{
	public static class ShaderParams : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_RTBackBufferName;

		private static readonly System.IntPtr NativeFieldInfoPtr_RTBackBuffer;

		private static readonly System.IntPtr NativeFieldInfoPtr_RTFrontBufferName;

		private static readonly System.IntPtr NativeFieldInfoPtr_RTFrontBuffer;

		private static readonly System.IntPtr NativeFieldInfoPtr_FlaskThickness;

		private static readonly System.IntPtr NativeFieldInfoPtr_ForcedInvisible;

		private static readonly System.IntPtr NativeFieldInfoPtr_SKW_FP_RENDER_TEXTURE;

		public unsafe static string RTBackBufferName
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RTBackBufferName, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RTBackBufferName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static int RTBackBuffer
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RTBackBuffer, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RTBackBuffer, (void*)(&num));
			}
		}

		public unsafe static string RTFrontBufferName
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RTFrontBufferName, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RTFrontBufferName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe static int RTFrontBuffer
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RTFrontBuffer, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RTFrontBuffer, (void*)(&num));
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

		public unsafe static int ForcedInvisible
		{
			get
			{
				Unsafe.SkipInit(out int result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ForcedInvisible, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ForcedInvisible, (void*)(&num));
			}
		}

		public unsafe static string SKW_FP_RENDER_TEXTURE
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SKW_FP_RENDER_TEXTURE, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SKW_FP_RENDER_TEXTURE, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static ShaderParams()
		{
			Il2CppClassPointerStore<ShaderParams>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "ShaderParams");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr);
			NativeFieldInfoPtr_RTBackBufferName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "RTBackBufferName");
			NativeFieldInfoPtr_RTBackBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "RTBackBuffer");
			NativeFieldInfoPtr_RTFrontBufferName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "RTFrontBufferName");
			NativeFieldInfoPtr_RTFrontBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "RTFrontBuffer");
			NativeFieldInfoPtr_FlaskThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "FlaskThickness");
			NativeFieldInfoPtr_ForcedInvisible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "ForcedInvisible");
			NativeFieldInfoPtr_SKW_FP_RENDER_TEXTURE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShaderParams>.NativeClassPtr, "SKW_FP_RENDER_TEXTURE");
		}

		public ShaderParams(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[OriginalName("Assembly-CSharp.dll", "", "Pass")]
	public enum Pass
	{
		BackBuffer,
		FrontBuffer
	}

	public class DepthPass : ScriptableRenderPass
	{
		public class PassData : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_cam;

			private static readonly System.IntPtr NativeFieldInfoPtr_cmd;

			private static readonly System.IntPtr NativeFieldInfoPtr_depthPass;

			private static readonly System.IntPtr NativeFieldInfoPtr_mat;

			private static readonly System.IntPtr NativeFieldInfoPtr_source;

			private static readonly System.IntPtr NativeFieldInfoPtr_depth;

			private static readonly System.IntPtr NativeFieldInfoPtr_cameraTargetDescriptor;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			public unsafe Camera cam
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cam);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Camera>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cam)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera));
				}
			}

			public unsafe CommandBuffer cmd
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cmd);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CommandBuffer>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cmd)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)commandBuffer));
				}
			}

			public unsafe DepthPass depthPass
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_depthPass);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DepthPass>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_depthPass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)depthPass));
				}
			}

			public unsafe Material mat
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mat);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mat)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
				}
			}

			public unsafe RTHandle source
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_source);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_source)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
				}
			}

			public unsafe RTHandle depth
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_depth);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_depth)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
				}
			}

			public unsafe RenderTextureDescriptor cameraTargetDescriptor
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cameraTargetDescriptor);
					return *(RenderTextureDescriptor*)num;
				}
				set
				{
					*(RenderTextureDescriptor*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cameraTargetDescriptor)) = renderTextureDescriptor;
				}
			}

			static PassData()
			{
				Il2CppClassPointerStore<PassData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "PassData");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PassData>.NativeClassPtr);
				NativeFieldInfoPtr_cam = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "cam");
				NativeFieldInfoPtr_cmd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "cmd");
				NativeFieldInfoPtr_depthPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "depthPass");
				NativeFieldInfoPtr_mat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "mat");
				NativeFieldInfoPtr_source = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "source");
				NativeFieldInfoPtr_depth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "depth");
				NativeFieldInfoPtr_cameraTargetDescriptor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PassData>.NativeClassPtr, "cameraTargetDescriptor");
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PassData>.NativeClassPtr, 100664741);
			}

			[CallerCount(2575)]
			[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe PassData()
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PassData>.NativeClassPtr))
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			public PassData(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_profilerTag;

		private static readonly System.IntPtr NativeFieldInfoPtr_mat;

		private static readonly System.IntPtr NativeFieldInfoPtr_targetNameId;

		private static readonly System.IntPtr NativeFieldInfoPtr_targetRT;

		private static readonly System.IntPtr NativeFieldInfoPtr_passId;

		private static readonly System.IntPtr NativeFieldInfoPtr_lvRenderers;

		private static readonly System.IntPtr NativeFieldInfoPtr_renderer;

		private static readonly System.IntPtr NativeFieldInfoPtr_interleavedRendering;

		private static readonly System.IntPtr NativeFieldInfoPtr_currentCameraPosition;

		private static readonly System.IntPtr NativeFieldInfoPtr_passData;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Material_Pass_RenderPassEvent_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Setup_Public_Void_LiquidVolumeDepthPrePassRenderFeature_ScriptableRenderer_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SortByDistanceToCamera_Private_Int32_LiquidVolume_LiquidVolume_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Configure_Public_Virtual_Void_CommandBuffer_RenderTextureDescriptor_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ExecutePass_Private_Static_Void_PassData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CleanUp_Public_Void_0;

		public unsafe static string profilerTag
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_profilerTag, (void*)(&intPtr));
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_profilerTag, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe Material mat
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mat);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mat)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
			}
		}

		public unsafe int targetNameId
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetNameId);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetNameId)) = num;
			}
		}

		public unsafe RTHandle targetRT
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetRT);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetRT)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
			}
		}

		public unsafe int passId
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_passId);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_passId)) = num;
			}
		}

		public unsafe List<LiquidVolume> lvRenderers
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lvRenderers);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<LiquidVolume>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lvRenderers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe ScriptableRenderer renderer
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderer);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ScriptableRenderer>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scriptableRenderer));
			}
		}

		public unsafe bool interleavedRendering
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interleavedRendering);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interleavedRendering)) = flag;
			}
		}

		public unsafe static Vector3 currentCameraPosition
		{
			get
			{
				Unsafe.SkipInit(out Vector3 result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_currentCameraPosition, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_currentCameraPosition, (void*)(&vector));
			}
		}

		public unsafe PassData passData
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_passData);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PassData>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_passData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)passData));
			}
		}

		static DepthPass()
		{
			Il2CppClassPointerStore<DepthPass>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "DepthPass");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DepthPass>.NativeClassPtr);
			NativeFieldInfoPtr_profilerTag = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "profilerTag");
			NativeFieldInfoPtr_mat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "mat");
			NativeFieldInfoPtr_targetNameId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "targetNameId");
			NativeFieldInfoPtr_targetRT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "targetRT");
			NativeFieldInfoPtr_passId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "passId");
			NativeFieldInfoPtr_lvRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "lvRenderers");
			NativeFieldInfoPtr_renderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "renderer");
			NativeFieldInfoPtr_interleavedRendering = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "interleavedRendering");
			NativeFieldInfoPtr_currentCameraPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "currentCameraPosition");
			NativeFieldInfoPtr_passData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, "passData");
			NativeMethodInfoPtr__ctor_Public_Void_Material_Pass_RenderPassEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664734);
			NativeMethodInfoPtr_Setup_Public_Void_LiquidVolumeDepthPrePassRenderFeature_ScriptableRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664735);
			NativeMethodInfoPtr_SortByDistanceToCamera_Private_Int32_LiquidVolume_LiquidVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664736);
			NativeMethodInfoPtr_Configure_Public_Virtual_Void_CommandBuffer_RenderTextureDescriptor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664737);
			NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664738);
			NativeMethodInfoPtr_ExecutePass_Private_Static_Void_PassData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664739);
			NativeMethodInfoPtr_CleanUp_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DepthPass>.NativeClassPtr, 100664740);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 77106, RefRangeEnd = 77108, XrefRangeStart = 77064, XrefRangeEnd = 77106, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DepthPass(Material mat, Pass pass, RenderPassEvent renderPassEvent)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DepthPass>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
			*(Pass**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pass;
			*(RenderPassEvent**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &renderPassEvent;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Material_Pass_RenderPassEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77108, XrefRangeEnd = 77109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Setup(LiquidVolumeDepthPrePassRenderFeature feature, ScriptableRenderer renderer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)feature);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Setup_Public_Void_LiquidVolumeDepthPrePassRenderFeature_ScriptableRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77109, XrefRangeEnd = 77131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int SortByDistanceToCamera(LiquidVolume lv1, LiquidVolume lv2)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv1);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv2);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortByDistanceToCamera_Private_Int32_LiquidVolume_LiquidVolume_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77131, XrefRangeEnd = 77139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cmd);
			*(RenderTextureDescriptor**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraTextureDescriptor;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Configure_Public_Virtual_Void_CommandBuffer_RenderTextureDescriptor_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77139, XrefRangeEnd = 77159, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&context);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 77200, RefRangeEnd = 77201, XrefRangeStart = 77159, XrefRangeEnd = 77200, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void ExecutePass(PassData passData)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)passData);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ExecutePass_Private_Static_Void_PassData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77201, XrefRangeEnd = 77205, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void CleanUp()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CleanUp_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public DepthPass(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_lvBackRenderers;

	private static readonly System.IntPtr NativeFieldInfoPtr_lvFrontRenderers;

	private static readonly System.IntPtr NativeFieldInfoPtr_shader;

	private static readonly System.IntPtr NativeFieldInfoPtr_installed;

	private static readonly System.IntPtr NativeFieldInfoPtr_mat;

	private static readonly System.IntPtr NativeFieldInfoPtr_backPass;

	private static readonly System.IntPtr NativeFieldInfoPtr_frontPass;

	private static readonly System.IntPtr NativeFieldInfoPtr_interleavedRendering;

	private static readonly System.IntPtr NativeFieldInfoPtr_renderPassEvent;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddLiquidToBackRenderers_Public_Static_Void_LiquidVolume_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveLiquidFromBackRenderers_Public_Static_Void_LiquidVolume_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddLiquidToFrontRenderers_Public_Static_Void_LiquidVolume_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveLiquidFromFrontRenderers_Public_Static_Void_LiquidVolume_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static List<LiquidVolume> lvBackRenderers
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lvBackRenderers, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<LiquidVolume>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lvBackRenderers, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static List<LiquidVolume> lvFrontRenderers
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lvFrontRenderers, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<LiquidVolume>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lvFrontRenderers, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Shader shader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shader);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Shader>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader));
		}
	}

	public unsafe static bool installed
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_installed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_installed, (void*)(&flag));
		}
	}

	public unsafe Material mat
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mat);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mat)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe DepthPass backPass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_backPass);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DepthPass>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_backPass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)depthPass));
		}
	}

	public unsafe DepthPass frontPass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frontPass);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DepthPass>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frontPass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)depthPass));
		}
	}

	public unsafe bool interleavedRendering
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interleavedRendering);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interleavedRendering)) = flag;
		}
	}

	public unsafe RenderPassEvent renderPassEvent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderPassEvent);
			return *(RenderPassEvent*)num;
		}
		set
		{
			*(RenderPassEvent*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderPassEvent)) = renderPassEvent;
		}
	}

	static LiquidVolumeDepthPrePassRenderFeature()
	{
		Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "LiquidVolumeFX", "LiquidVolumeDepthPrePassRenderFeature");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr);
		NativeFieldInfoPtr_lvBackRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "lvBackRenderers");
		NativeFieldInfoPtr_lvFrontRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "lvFrontRenderers");
		NativeFieldInfoPtr_shader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "shader");
		NativeFieldInfoPtr_installed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "installed");
		NativeFieldInfoPtr_mat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "mat");
		NativeFieldInfoPtr_backPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "backPass");
		NativeFieldInfoPtr_frontPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "frontPass");
		NativeFieldInfoPtr_interleavedRendering = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "interleavedRendering");
		NativeFieldInfoPtr_renderPassEvent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, "renderPassEvent");
		NativeMethodInfoPtr_AddLiquidToBackRenderers_Public_Static_Void_LiquidVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664724);
		NativeMethodInfoPtr_RemoveLiquidFromBackRenderers_Public_Static_Void_LiquidVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664725);
		NativeMethodInfoPtr_AddLiquidToFrontRenderers_Public_Static_Void_LiquidVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664726);
		NativeMethodInfoPtr_RemoveLiquidFromFrontRenderers_Public_Static_Void_LiquidVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664727);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664728);
		NativeMethodInfoPtr_Create_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664729);
		NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664730);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr, 100664731);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77205, XrefRangeEnd = 77222, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void AddLiquidToBackRenderers(LiquidVolume lv)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddLiquidToBackRenderers_Public_Static_Void_LiquidVolume_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 77239, RefRangeEnd = 77241, XrefRangeStart = 77222, XrefRangeEnd = 77239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RemoveLiquidFromBackRenderers(LiquidVolume lv)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveLiquidFromBackRenderers_Public_Static_Void_LiquidVolume_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77241, XrefRangeEnd = 77258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void AddLiquidToFrontRenderers(LiquidVolume lv)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddLiquidToFrontRenderers_Public_Static_Void_LiquidVolume_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 77275, RefRangeEnd = 77277, XrefRangeStart = 77258, XrefRangeEnd = 77275, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RemoveLiquidFromFrontRenderers(LiquidVolume lv)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lv);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveLiquidFromFrontRenderers_Public_Static_Void_LiquidVolume_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77277, XrefRangeEnd = 77294, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77294, XrefRangeEnd = 77319, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Create()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Create_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77319, XrefRangeEnd = 77335, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 77335, XrefRangeEnd = 77336, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LiquidVolumeDepthPrePassRenderFeature()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LiquidVolumeDepthPrePassRenderFeature>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LiquidVolumeDepthPrePassRenderFeature(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
