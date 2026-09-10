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

namespace Il2Cpp;

public class GrabScreenFeature : ScriptableRendererFeature
{
	[System.Serializable]
	public class Settings : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_TextureName;

		private static readonly System.IntPtr NativeFieldInfoPtr_RenderPassEvent;

		private static readonly System.IntPtr NativeFieldInfoPtr_LayerMask;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string TextureName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TextureName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TextureName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe RenderPassEvent RenderPassEvent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RenderPassEvent);
				return *(RenderPassEvent*)num;
			}
			set
			{
				*(RenderPassEvent*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RenderPassEvent)) = renderPassEvent;
			}
		}

		public unsafe LayerMask LayerMask
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LayerMask);
				return *(LayerMask*)num;
			}
			set
			{
				*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LayerMask)) = layerMask;
			}
		}

		static Settings()
		{
			Il2CppClassPointerStore<Settings>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, "Settings");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Settings>.NativeClassPtr);
			NativeFieldInfoPtr_TextureName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "TextureName");
			NativeFieldInfoPtr_RenderPassEvent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "RenderPassEvent");
			NativeFieldInfoPtr_LayerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "LayerMask");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Settings>.NativeClassPtr, 100663424);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66198, XrefRangeEnd = 66203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Settings()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Settings>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Settings(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class GrabPass : ScriptableRenderPass
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_settings;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_GrabbedTextureHandle;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_CameraColorHandle;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Settings_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Void_0;

		public unsafe Settings settings
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Settings>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings));
			}
		}

		public unsafe RTHandle m_GrabbedTextureHandle
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GrabbedTextureHandle);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GrabbedTextureHandle)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
			}
		}

		public unsafe RTHandle m_CameraColorHandle
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CameraColorHandle);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CameraColorHandle)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
			}
		}

		static GrabPass()
		{
			Il2CppClassPointerStore<GrabPass>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, "GrabPass");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GrabPass>.NativeClassPtr);
			NativeFieldInfoPtr_settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, "settings");
			NativeFieldInfoPtr_m_GrabbedTextureHandle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, "m_GrabbedTextureHandle");
			NativeFieldInfoPtr_m_CameraColorHandle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, "m_CameraColorHandle");
			NativeMethodInfoPtr__ctor_Public_Void_Settings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, 100663425);
			NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, 100663426);
			NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, 100663427);
			NativeMethodInfoPtr_Dispose_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabPass>.NativeClassPtr, 100663428);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66203, XrefRangeEnd = 66208, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe GrabPass(Settings s)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GrabPass>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Settings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66208, XrefRangeEnd = 66217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cmd);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66217, XrefRangeEnd = 66234, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66234, XrefRangeEnd = 66235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public GrabPass(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class RenderPass : ScriptableRenderPass
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_settings;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_ShaderTagIdList;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_FilteringSettings;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_RenderStateBlock;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Settings_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0;

		public unsafe Settings settings
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Settings>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings));
			}
		}

		public unsafe List<ShaderTagId> m_ShaderTagIdList
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ShaderTagIdList);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ShaderTagId>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ShaderTagIdList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe FilteringSettings m_FilteringSettings
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FilteringSettings);
				return *(FilteringSettings*)num;
			}
			set
			{
				*(FilteringSettings*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FilteringSettings)) = filteringSettings;
			}
		}

		public unsafe RenderStateBlock m_RenderStateBlock
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RenderStateBlock);
				return *(RenderStateBlock*)num;
			}
			set
			{
				*(RenderStateBlock*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RenderStateBlock)) = renderStateBlock;
			}
		}

		static RenderPass()
		{
			Il2CppClassPointerStore<RenderPass>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, "RenderPass");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RenderPass>.NativeClassPtr);
			NativeFieldInfoPtr_settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderPass>.NativeClassPtr, "settings");
			NativeFieldInfoPtr_m_ShaderTagIdList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderPass>.NativeClassPtr, "m_ShaderTagIdList");
			NativeFieldInfoPtr_m_FilteringSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderPass>.NativeClassPtr, "m_FilteringSettings");
			NativeFieldInfoPtr_m_RenderStateBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderPass>.NativeClassPtr, "m_RenderStateBlock");
			NativeMethodInfoPtr__ctor_Public_Void_Settings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderPass>.NativeClassPtr, 100663429);
			NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderPass>.NativeClassPtr, 100663430);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 66276, RefRangeEnd = 66277, XrefRangeStart = 66235, XrefRangeEnd = 66276, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe RenderPass(Settings settings)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RenderPass>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Settings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66277, XrefRangeEnd = 66293, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

		public RenderPass(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_grabPass;

	private static readonly System.IntPtr NativeFieldInfoPtr_renderPass;

	private static readonly System.IntPtr NativeFieldInfoPtr_settings;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Protected_Virtual_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GrabPass grabPass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_grabPass);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GrabPass>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_grabPass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grabPass));
		}
	}

	public unsafe RenderPass renderPass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderPass);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RenderPass>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderPass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderPass));
		}
	}

	public unsafe Settings settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Settings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings));
		}
	}

	static GrabScreenFeature()
	{
		Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "GrabScreenFeature");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr);
		NativeFieldInfoPtr_grabPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, "grabPass");
		NativeFieldInfoPtr_renderPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, "renderPass");
		NativeFieldInfoPtr_settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, "settings");
		NativeMethodInfoPtr_Create_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, 100663420);
		NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, 100663421);
		NativeMethodInfoPtr_Dispose_Protected_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, 100663422);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr, 100663423);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66293, XrefRangeEnd = 66308, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Create()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Create_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66308, XrefRangeEnd = 66311, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66311, XrefRangeEnd = 66312, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Dispose(bool disposing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&disposing);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Dispose_Protected_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66312, XrefRangeEnd = 66322, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GrabScreenFeature()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GrabScreenFeature>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GrabScreenFeature(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
