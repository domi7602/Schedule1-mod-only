using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFishNet.Serializing.Generated;

[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public static class GeneratedComparers___Internal : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_InitializeOnce_Private_Static_Void_0;

	static GeneratedComparers___Internal()
	{
		Il2CppClassPointerStore<GeneratedComparers___Internal>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishNet.Serializing.Generated", "GeneratedComparers___Internal");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GeneratedComparers___Internal>.NativeClassPtr);
		NativeMethodInfoPtr_InitializeOnce_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GeneratedComparers___Internal>.NativeClassPtr, 100689844);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitializeOnce()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeOnce_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GeneratedComparers___Internal(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
