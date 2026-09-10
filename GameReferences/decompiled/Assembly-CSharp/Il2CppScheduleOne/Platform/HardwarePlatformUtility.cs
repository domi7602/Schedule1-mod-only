using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppSystem;

namespace Il2CppScheduleOne.Platform;

public static class HardwarePlatformUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetCurrentPlatform_Public_Static_EHardwarePlatform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsAnyGamepadConnected_Public_Static_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAvailableDisplayModes_Public_Static_Il2CppStructArray_1_EDisplayMode_0;

	static HardwarePlatformUtility()
	{
		Il2CppClassPointerStore<HardwarePlatformUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Platform", "HardwarePlatformUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HardwarePlatformUtility>.NativeClassPtr);
		NativeMethodInfoPtr_GetCurrentPlatform_Public_Static_EHardwarePlatform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HardwarePlatformUtility>.NativeClassPtr, 100668663);
		NativeMethodInfoPtr_IsAnyGamepadConnected_Public_Static_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HardwarePlatformUtility>.NativeClassPtr, 100668664);
		NativeMethodInfoPtr_GetAvailableDisplayModes_Public_Static_Il2CppStructArray_1_EDisplayMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HardwarePlatformUtility>.NativeClassPtr, 100668665);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 123680, RefRangeEnd = 123681, XrefRangeStart = 123678, XrefRangeEnd = 123680, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EHardwarePlatform GetCurrentPlatform()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCurrentPlatform_Public_Static_EHardwarePlatform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EHardwarePlatform*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 123683, RefRangeEnd = 123684, XrefRangeStart = 123681, XrefRangeEnd = 123683, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsAnyGamepadConnected()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsAnyGamepadConnected_Public_Static_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 123687, RefRangeEnd = 123692, XrefRangeStart = 123684, XrefRangeEnd = 123687, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<DisplaySettings.EDisplayMode> GetAvailableDisplayModes()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAvailableDisplayModes_Public_Static_Il2CppStructArray_1_EDisplayMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<DisplaySettings.EDisplayMode>>(intPtr) : null;
	}

	public HardwarePlatformUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
