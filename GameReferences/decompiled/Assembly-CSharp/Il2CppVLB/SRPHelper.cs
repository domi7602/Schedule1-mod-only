using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppVLB;

public static class SRPHelper : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_m_IsRenderPipelineCached;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_RenderPipelineCached;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_renderPipelineScriptingDefineSymbolAsString_Public_Static_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_projectRenderPipeline_Public_Static_get_RenderPipeline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeRenderPipeline_Private_Static_RenderPipeline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsUsingCustomRenderPipeline_Public_Static_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterOnBeginCameraRendering_Public_Static_Void_Action_2_ScriptableRenderContext_Camera_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnregisterOnBeginCameraRendering_Public_Static_Void_Action_2_ScriptableRenderContext_Camera_0;

	public unsafe static bool m_IsRenderPipelineCached
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_m_IsRenderPipelineCached, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_m_IsRenderPipelineCached, (void*)(&flag));
		}
	}

	public unsafe static RenderPipeline m_RenderPipelineCached
	{
		get
		{
			Unsafe.SkipInit(out RenderPipeline result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_m_RenderPipelineCached, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_m_RenderPipelineCached, (void*)(&renderPipeline));
		}
	}

	public unsafe static string renderPipelineScriptingDefineSymbolAsString
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73768, XrefRangeEnd = 73770, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_renderPipelineScriptingDefineSymbolAsString_Public_Static_get_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	public unsafe static RenderPipeline projectRenderPipeline
	{
		[CallerCount(13)]
		[CachedScanResults(RefRangeStart = 73792, RefRangeEnd = 73805, XrefRangeStart = 73770, XrefRangeEnd = 73792, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_projectRenderPipeline_Public_Static_get_RenderPipeline_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RenderPipeline*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SRPHelper()
	{
		Il2CppClassPointerStore<SRPHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "SRPHelper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr);
		NativeFieldInfoPtr_m_IsRenderPipelineCached = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, "m_IsRenderPipelineCached");
		NativeFieldInfoPtr_m_RenderPipelineCached = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, "m_RenderPipelineCached");
		NativeMethodInfoPtr_get_renderPipelineScriptingDefineSymbolAsString_Public_Static_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, 100664366);
		NativeMethodInfoPtr_get_projectRenderPipeline_Public_Static_get_RenderPipeline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, 100664367);
		NativeMethodInfoPtr_ComputeRenderPipeline_Private_Static_RenderPipeline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, 100664368);
		NativeMethodInfoPtr_IsUsingCustomRenderPipeline_Public_Static_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, 100664369);
		NativeMethodInfoPtr_RegisterOnBeginCameraRendering_Public_Static_Void_Action_2_ScriptableRenderContext_Camera_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, 100664370);
		NativeMethodInfoPtr_UnregisterOnBeginCameraRendering_Public_Static_Void_Action_2_ScriptableRenderContext_Camera_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SRPHelper>.NativeClassPtr, 100664371);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73805, XrefRangeEnd = 73820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RenderPipeline ComputeRenderPipeline()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeRenderPipeline_Private_Static_RenderPipeline_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RenderPipeline*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 73828, RefRangeEnd = 73836, XrefRangeStart = 73820, XrefRangeEnd = 73828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsUsingCustomRenderPipeline()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsUsingCustomRenderPipeline_Public_Static_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73842, RefRangeEnd = 73844, XrefRangeStart = 73836, XrefRangeEnd = 73842, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RegisterOnBeginCameraRendering(Il2CppSystem.Action<ScriptableRenderContext, Camera> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterOnBeginCameraRendering_Public_Static_Void_Action_2_ScriptableRenderContext_Camera_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73849, RefRangeEnd = 73851, XrefRangeStart = 73844, XrefRangeEnd = 73849, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void UnregisterOnBeginCameraRendering(Il2CppSystem.Action<ScriptableRenderContext, Camera> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnregisterOnBeginCameraRendering_Public_Static_Void_Action_2_ScriptableRenderContext_Camera_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SRPHelper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
