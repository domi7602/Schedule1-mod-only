using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Temperature;

public static class TemperatureUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_get_TemperatureSystemEnabled_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToFahrenheit_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FormatCelsiusTemperature_Public_Static_String_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FormatFahrenheitTemperature_Public_Static_String_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FormatTemperatureWithAppropriateUnit_Public_Static_String_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NormalizeTemperature_Public_Static_Single_Single_0;

	public unsafe static bool TemperatureSystemEnabled
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 102427, RefRangeEnd = 102435, XrefRangeStart = 102421, XrefRangeEnd = 102427, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TemperatureSystemEnabled_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static TemperatureUtility()
	{
		Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Temperature", "TemperatureUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr);
		NativeMethodInfoPtr_get_TemperatureSystemEnabled_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr, 100666987);
		NativeMethodInfoPtr_ToFahrenheit_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr, 100666988);
		NativeMethodInfoPtr_FormatCelsiusTemperature_Public_Static_String_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr, 100666989);
		NativeMethodInfoPtr_FormatFahrenheitTemperature_Public_Static_String_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr, 100666990);
		NativeMethodInfoPtr_FormatTemperatureWithAppropriateUnit_Public_Static_String_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr, 100666991);
		NativeMethodInfoPtr_NormalizeTemperature_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureUtility>.NativeClassPtr, 100666992);
	}

	[CallerCount(0)]
	public unsafe static float ToFahrenheit(float celsius)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&celsius);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToFahrenheit_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 102435, XrefRangeEnd = 102442, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string FormatCelsiusTemperature(float celsius, int decimalPoints)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&celsius);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimalPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FormatCelsiusTemperature_Public_Static_String_Single_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 102442, XrefRangeEnd = 102449, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string FormatFahrenheitTemperature(float fahrenheit, int decimalPoints)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&fahrenheit);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimalPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FormatFahrenheitTemperature_Public_Static_String_Single_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 102470, RefRangeEnd = 102471, XrefRangeStart = 102449, XrefRangeEnd = 102470, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string FormatTemperatureWithAppropriateUnit(float celsius, int decimalPoints = 1)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&celsius);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimalPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FormatTemperatureWithAppropriateUnit_Public_Static_String_Single_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 102471, RefRangeEnd = 102472, XrefRangeStart = 102471, XrefRangeEnd = 102471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float NormalizeTemperature(float celsius)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&celsius);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NormalizeTemperature_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public TemperatureUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
