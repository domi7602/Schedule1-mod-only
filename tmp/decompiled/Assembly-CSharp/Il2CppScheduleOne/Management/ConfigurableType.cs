using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Management;

public static class ConfigurableType : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetTypeName_Public_Static_String_EConfigurableType_0;

	static ConfigurableType()
	{
		Il2CppClassPointerStore<ConfigurableType>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Management", "ConfigurableType");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ConfigurableType>.NativeClassPtr);
		NativeMethodInfoPtr_GetTypeName_Public_Static_String_EConfigurableType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurableType>.NativeClassPtr, 100670799);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 149046, RefRangeEnd = 149054, XrefRangeStart = 149026, XrefRangeEnd = 149046, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetTypeName(EConfigurableType type)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTypeName_Public_Static_String_EConfigurableType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public ConfigurableType(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
