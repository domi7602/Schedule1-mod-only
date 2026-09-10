using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Temperature;

public static class TemperatureAlgorithm : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_NegligibleInfluenceThreshold;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTemperatureAtPoint_Public_Static_Single_Single_Vector3_Vector3_Il2CppStructArray_1_TemperatureEmitterInfo_0;

	public unsafe static float NegligibleInfluenceThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NegligibleInfluenceThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NegligibleInfluenceThreshold, (void*)(&num));
		}
	}

	static TemperatureAlgorithm()
	{
		Il2CppClassPointerStore<TemperatureAlgorithm>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Temperature", "TemperatureAlgorithm");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TemperatureAlgorithm>.NativeClassPtr);
		NativeFieldInfoPtr_NegligibleInfluenceThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureAlgorithm>.NativeClassPtr, "NegligibleInfluenceThreshold");
		NativeMethodInfoPtr_GetTemperatureAtPoint_Public_Static_Single_Single_Vector3_Vector3_Il2CppStructArray_1_TemperatureEmitterInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureAlgorithm>.NativeClassPtr, 100666974);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 102406, RefRangeEnd = 102408, XrefRangeStart = 102403, XrefRangeEnd = 102406, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetTemperatureAtPoint(float ambientTemperature, Vector3 originPoint, Vector3 point, Il2CppStructArray<TemperatureEmitterInfo> emitters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&ambientTemperature);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &originPoint;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &point;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)emitters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTemperatureAtPoint_Public_Static_Single_Single_Vector3_Vector3_Il2CppStructArray_1_TemperatureEmitterInfo_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public TemperatureAlgorithm(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
