using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Gamepad;
using UnityEngine;

namespace Il2CppScheduleOne.Platform;

public class PlatformDefaultSettings : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_Platform;

	private static readonly IntPtr NativeFieldInfoPtr_DisplaySettings;

	private static readonly IntPtr NativeFieldInfoPtr_GraphicsSettings;

	private static readonly IntPtr NativeFieldInfoPtr_InputSettings;

	private static readonly IntPtr NativeFieldInfoPtr_GamepadSettings;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EHardwarePlatform Platform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Platform);
			return *(EHardwarePlatform*)num;
		}
		set
		{
			*(EHardwarePlatform*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Platform)) = eHardwarePlatform;
		}
	}

	public unsafe DisplaySettings DisplaySettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplaySettings);
			return *(DisplaySettings*)num;
		}
		set
		{
			*(DisplaySettings*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplaySettings)) = displaySettings;
		}
	}

	public unsafe GraphicsSettings GraphicsSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GraphicsSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GraphicsSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GraphicsSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)graphicsSettings));
		}
	}

	public unsafe InputSettings InputSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InputSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<InputSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InputSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputSettings));
		}
	}

	public unsafe GamepadSettings GamepadSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GamepadSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GamepadSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GamepadSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gamepadSettings));
		}
	}

	static PlatformDefaultSettings()
	{
		Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Platform", "PlatformDefaultSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr);
		NativeFieldInfoPtr_Platform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr, "Platform");
		NativeFieldInfoPtr_DisplaySettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr, "DisplaySettings");
		NativeFieldInfoPtr_GraphicsSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr, "GraphicsSettings");
		NativeFieldInfoPtr_InputSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr, "InputSettings");
		NativeFieldInfoPtr_GamepadSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr, "GamepadSettings");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr, 100668695);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlatformDefaultSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlatformDefaultSettings>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlatformDefaultSettings(IntPtr pointer)
		: base(pointer)
	{
	}
}
