using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Gamepad;

[System.Serializable]
public class HapticSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Intensity;

	private static readonly System.IntPtr NativeFieldInfoPtr_PulseCyclesPerSecond;

	private static readonly System.IntPtr NativeFieldInfoPtr_PulseIntensity;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float Intensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Intensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Intensity)) = num;
		}
	}

	public unsafe float PulseCyclesPerSecond
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PulseCyclesPerSecond);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PulseCyclesPerSecond)) = num;
		}
	}

	public unsafe float PulseIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PulseIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PulseIntensity)) = num;
		}
	}

	static HapticSettings()
	{
		Il2CppClassPointerStore<HapticSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "HapticSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HapticSettings>.NativeClassPtr);
		NativeFieldInfoPtr_Intensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticSettings>.NativeClassPtr, "Intensity");
		NativeFieldInfoPtr_PulseCyclesPerSecond = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticSettings>.NativeClassPtr, "PulseCyclesPerSecond");
		NativeFieldInfoPtr_PulseIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticSettings>.NativeClassPtr, "PulseIntensity");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticSettings>.NativeClassPtr, 100685923);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HapticSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HapticSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public HapticSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
