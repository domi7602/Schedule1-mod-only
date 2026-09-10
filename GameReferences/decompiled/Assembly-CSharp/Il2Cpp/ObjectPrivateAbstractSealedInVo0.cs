using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2Cpp;

[ObfuscatedName("$BurstDirectCallInitializer")]
public static class ObjectPrivateAbstractSealedInVo0 : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Private_Static_Void_0;

	static ObjectPrivateAbstractSealedInVo0()
	{
		Il2CppClassPointerStore<ObjectPrivateAbstractSealedInVo0>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "$BurstDirectCallInitializer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectPrivateAbstractSealedInVo0>.NativeClassPtr);
		NativeMethodInfoPtr_Initialize_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectPrivateAbstractSealedInVo0>.NativeClassPtr, 100690079);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 341698, XrefRangeEnd = 341706, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Initialize()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ObjectPrivateAbstractSealedInVo0(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
