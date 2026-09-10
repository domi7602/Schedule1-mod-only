using System;
using System.Runtime.CompilerServices;
using Il2CppEPOOutline;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Il2Cpp;

public class URPOutlineFeature : ScriptableRendererFeature
{
	public class SRPOutline : ScriptableRenderPass
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_temporaryOutlinables;

		private static readonly System.IntPtr NativeFieldInfoPtr_Renderer;

		private static readonly System.IntPtr NativeFieldInfoPtr_UseColorTargetForDepth;

		private static readonly System.IntPtr NativeFieldInfoPtr_Outliner;

		private static readonly System.IntPtr NativeFieldInfoPtr_Parameters;

		private static readonly System.IntPtr NativeFieldInfoPtr_outliners;

		private static readonly System.IntPtr NativeFieldInfoPtr_nameId;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_IsDepthTextureAvailable_Private_Boolean_ScriptableRenderer_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetDepthTarget_Private_RenderTargetIdentifier_ScriptableRenderer_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetColorTarget_Private_RenderTargetIdentifier_ScriptableRenderer_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0;

		public unsafe static List<Outlinable> temporaryOutlinables
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_temporaryOutlinables, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Outlinable>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_temporaryOutlinables, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe ScriptableRenderer Renderer
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Renderer);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ScriptableRenderer>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Renderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scriptableRenderer));
			}
		}

		public unsafe bool UseColorTargetForDepth
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseColorTargetForDepth);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseColorTargetForDepth)) = flag;
			}
		}

		public unsafe Outliner Outliner
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Outliner);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Outliner>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Outliner)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outliner));
			}
		}

		public unsafe OutlineParameters Parameters
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Parameters);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OutlineParameters>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Parameters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outlineParameters));
			}
		}

		public unsafe List<Outliner> outliners
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outliners);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Outliner>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outliners)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe FieldInfo nameId
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nameId);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<FieldInfo>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nameId)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fieldInfo));
			}
		}

		static SRPOutline()
		{
			Il2CppClassPointerStore<SRPOutline>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, "SRPOutline");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr);
			NativeFieldInfoPtr_temporaryOutlinables = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "temporaryOutlinables");
			NativeFieldInfoPtr_Renderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "Renderer");
			NativeFieldInfoPtr_UseColorTargetForDepth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "UseColorTargetForDepth");
			NativeFieldInfoPtr_Outliner = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "Outliner");
			NativeFieldInfoPtr_Parameters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "Parameters");
			NativeFieldInfoPtr_outliners = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "outliners");
			NativeFieldInfoPtr_nameId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, "nameId");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, 100663318);
			NativeMethodInfoPtr_IsDepthTextureAvailable_Private_Boolean_ScriptableRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, 100663319);
			NativeMethodInfoPtr_GetDepthTarget_Private_RenderTargetIdentifier_ScriptableRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, 100663320);
			NativeMethodInfoPtr_GetColorTarget_Private_RenderTargetIdentifier_ScriptableRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, 100663321);
			NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr, 100663322);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3926, XrefRangeEnd = 3953, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SRPOutline()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SRPOutline>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3953, XrefRangeEnd = 3969, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool IsDepthTextureAvailable(ScriptableRenderer renderer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsDepthTextureAvailable_Private_Boolean_ScriptableRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3969, XrefRangeEnd = 3971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe RenderTargetIdentifier GetDepthTarget(ScriptableRenderer renderer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDepthTarget_Private_RenderTargetIdentifier_ScriptableRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderTargetIdentifier*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3971, XrefRangeEnd = 3973, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe RenderTargetIdentifier GetColorTarget(ScriptableRenderer renderer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColorTarget_Private_RenderTargetIdentifier_ScriptableRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderTargetIdentifier*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3973, XrefRangeEnd = 4050, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

		public SRPOutline(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class Pool : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_outlines;

		private static readonly System.IntPtr NativeFieldInfoPtr_createdOutlines;

		private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_SRPOutline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReleaseAll_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Stack<SRPOutline> outlines
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outlines);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stack<SRPOutline>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outlines)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stack));
			}
		}

		public unsafe List<SRPOutline> createdOutlines
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_createdOutlines);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SRPOutline>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_createdOutlines)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		static Pool()
		{
			Il2CppClassPointerStore<Pool>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, "Pool");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Pool>.NativeClassPtr);
			NativeFieldInfoPtr_outlines = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Pool>.NativeClassPtr, "outlines");
			NativeFieldInfoPtr_createdOutlines = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Pool>.NativeClassPtr, "createdOutlines");
			NativeMethodInfoPtr_Get_Public_SRPOutline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool>.NativeClassPtr, 100663324);
			NativeMethodInfoPtr_ReleaseAll_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool>.NativeClassPtr, 100663325);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool>.NativeClassPtr, 100663326);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 4096, RefRangeEnd = 4097, XrefRangeStart = 4050, XrefRangeEnd = 4096, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SRPOutline Get()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_SRPOutline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SRPOutline>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 4117, RefRangeEnd = 4118, XrefRangeStart = 4097, XrefRangeEnd = 4117, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ReleaseAll()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReleaseAll_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 4118, XrefRangeEnd = 4133, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Pool()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Pool>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Pool(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_lastSelectedCamera;

	private static readonly System.IntPtr NativeFieldInfoPtr_outlinePool;

	private static readonly System.IntPtr NativeFieldInfoPtr_outliners;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOutlinersToRenderWith_Private_Boolean_RenderingData_List_1_Outliner_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GameObject lastSelectedCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastSelectedCamera);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastSelectedCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe Pool outlinePool
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outlinePool);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Pool>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outlinePool)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pool));
		}
	}

	public unsafe List<Outliner> outliners
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outliners);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Outliner>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outliners)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static URPOutlineFeature()
	{
		Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "", "URPOutlineFeature");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr);
		NativeFieldInfoPtr_lastSelectedCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, "lastSelectedCamera");
		NativeFieldInfoPtr_outlinePool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, "outlinePool");
		NativeFieldInfoPtr_outliners = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, "outliners");
		NativeMethodInfoPtr_GetOutlinersToRenderWith_Private_Boolean_RenderingData_List_1_Outliner_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, 100663314);
		NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, 100663315);
		NativeMethodInfoPtr_Create_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, 100663316);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr, 100663317);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 4133, XrefRangeEnd = 4141, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GetOutlinersToRenderWith(RenderingData renderingData, List<Outliner> outliners)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outliners);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutlinersToRenderWith_Private_Boolean_RenderingData_List_1_Outliner_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 4141, XrefRangeEnd = 4192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 4192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Create()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Create_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19142, XrefRangeEnd = 19169, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe URPOutlineFeature()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<URPOutlineFeature>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public URPOutlineFeature(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
