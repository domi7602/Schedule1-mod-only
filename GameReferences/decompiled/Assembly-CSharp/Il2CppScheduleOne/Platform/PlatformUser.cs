using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Platform;

public static class PlatformUser : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetUserID_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPublicPersonaName_Public_Static_String_0;

	static PlatformUser()
	{
		Il2CppClassPointerStore<PlatformUser>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Platform", "PlatformUser");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlatformUser>.NativeClassPtr);
		NativeMethodInfoPtr_GetUserID_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformUser>.NativeClassPtr, 100668711);
		NativeMethodInfoPtr_GetPublicPersonaName_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformUser>.NativeClassPtr, 100668712);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 124253, RefRangeEnd = 124260, XrefRangeStart = 124243, XrefRangeEnd = 124253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetUserID()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetUserID_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124269, RefRangeEnd = 124270, XrefRangeStart = 124260, XrefRangeEnd = 124269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetPublicPersonaName()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPublicPersonaName_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public PlatformUser(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
