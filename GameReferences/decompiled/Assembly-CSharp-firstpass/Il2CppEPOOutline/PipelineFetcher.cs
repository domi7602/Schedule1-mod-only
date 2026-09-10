using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine.Rendering;

namespace Il2CppEPOOutline;

public static class PipelineFetcher : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentAsset_Public_Static_get_RenderPipelineAsset_0;

	public unsafe static RenderPipelineAsset CurrentAsset
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 64218, RefRangeEnd = 64220, XrefRangeStart = 64212, XrefRangeEnd = 64218, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentAsset_Public_Static_get_RenderPipelineAsset_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RenderPipelineAsset>(intPtr) : null;
		}
	}

	static PipelineFetcher()
	{
		Il2CppClassPointerStore<PipelineFetcher>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "PipelineFetcher");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PipelineFetcher>.NativeClassPtr);
		NativeMethodInfoPtr_get_CurrentAsset_Public_Static_get_RenderPipelineAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PipelineFetcher>.NativeClassPtr, 100667444);
	}

	public PipelineFetcher(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
