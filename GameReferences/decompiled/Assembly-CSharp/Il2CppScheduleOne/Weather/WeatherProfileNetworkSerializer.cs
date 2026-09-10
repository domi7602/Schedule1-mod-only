using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Serializing;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Weather;
using Il2CppSystem;

namespace Il2CppScheduleOne.Weather;

public static class WeatherProfileNetworkSerializer : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Null;

	private static readonly System.IntPtr NativeMethodInfoPtr_WriteWeatherProfile_Public_Static_Void_Writer_WeatherProfile_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadWeatherProfile_Public_Static_WeatherProfile_Reader_0;

	public unsafe static string Null
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Null, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Null, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static WeatherProfileNetworkSerializer()
	{
		Il2CppClassPointerStore<WeatherProfileNetworkSerializer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Weather", "WeatherProfileNetworkSerializer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WeatherProfileNetworkSerializer>.NativeClassPtr);
		NativeFieldInfoPtr_Null = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherProfileNetworkSerializer>.NativeClassPtr, "Null");
		NativeMethodInfoPtr_WriteWeatherProfile_Public_Static_Void_Writer_WeatherProfile_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherProfileNetworkSerializer>.NativeClassPtr, 100685818);
		NativeMethodInfoPtr_ReadWeatherProfile_Public_Static_WeatherProfile_Reader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherProfileNetworkSerializer>.NativeClassPtr, 100685819);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 293801, XrefRangeEnd = 293810, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void WriteWeatherProfile(this Writer writer, WeatherProfile value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)writer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteWeatherProfile_Public_Static_Void_Writer_WeatherProfile_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 293810, XrefRangeEnd = 293819, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static WeatherProfile ReadWeatherProfile(this Reader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadWeatherProfile_Public_Static_WeatherProfile_Reader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<WeatherProfile>(intPtr) : null;
	}

	public WeatherProfileNetworkSerializer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
