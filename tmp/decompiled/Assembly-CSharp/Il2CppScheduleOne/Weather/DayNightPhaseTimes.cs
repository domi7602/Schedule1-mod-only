using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Weather;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct DayNightPhaseTimes
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MinDawnHour;

	private static readonly System.IntPtr NativeFieldInfoPtr_SunRiseHour;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxDawnHour;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinDuskHour;

	private static readonly System.IntPtr NativeFieldInfoPtr_SunSetHour;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxDuskHour;

	[FieldOffset(0)]
	public int MinDawnHour;

	[FieldOffset(4)]
	public int SunRiseHour;

	[FieldOffset(8)]
	public int MaxDawnHour;

	[FieldOffset(12)]
	public int MinDuskHour;

	[FieldOffset(16)]
	public int SunSetHour;

	[FieldOffset(20)]
	public int MaxDuskHour;

	static DayNightPhaseTimes()
	{
		Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Weather", "DayNightPhaseTimes");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr);
		NativeFieldInfoPtr_MinDawnHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, "MinDawnHour");
		NativeFieldInfoPtr_SunRiseHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, "SunRiseHour");
		NativeFieldInfoPtr_MaxDawnHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, "MaxDawnHour");
		NativeFieldInfoPtr_MinDuskHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, "MinDuskHour");
		NativeFieldInfoPtr_SunSetHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, "SunSetHour");
		NativeFieldInfoPtr_MaxDuskHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, "MaxDuskHour");
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<DayNightPhaseTimes>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
