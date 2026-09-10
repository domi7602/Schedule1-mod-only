using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools;

public static class Environment : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsThreadingSupported_Public_Static_get_Boolean_0;

	public unsafe static bool IsThreadingSupported
	{
		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsThreadingSupported_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static Environment()
	{
		Il2CppClassPointerStore<Environment>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "Environment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Environment>.NativeClassPtr);
		NativeMethodInfoPtr_get_IsThreadingSupported_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Environment>.NativeClassPtr, 100664038);
	}

	public Environment(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
