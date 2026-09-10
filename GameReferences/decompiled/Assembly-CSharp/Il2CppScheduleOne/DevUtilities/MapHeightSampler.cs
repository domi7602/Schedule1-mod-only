using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class MapHeightSampler : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SampleHeight;

	private static readonly System.IntPtr NativeFieldInfoPtr_SampleDistance;

	private static readonly System.IntPtr NativeMethodInfoPtr_TrySample_Public_Static_Boolean_Single_Single_byref_Vector3_0;

	public unsafe static float SampleHeight
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SampleHeight, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SampleHeight, (void*)(&num));
		}
	}

	public unsafe static float SampleDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SampleDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SampleDistance, (void*)(&num));
		}
	}

	static MapHeightSampler()
	{
		Il2CppClassPointerStore<MapHeightSampler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "MapHeightSampler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MapHeightSampler>.NativeClassPtr);
		NativeFieldInfoPtr_SampleHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapHeightSampler>.NativeClassPtr, "SampleHeight");
		NativeFieldInfoPtr_SampleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapHeightSampler>.NativeClassPtr, "SampleDistance");
		NativeMethodInfoPtr_TrySample_Public_Static_Boolean_Single_Single_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapHeightSampler>.NativeClassPtr, 100675076);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 194780, RefRangeEnd = 194785, XrefRangeStart = 194769, XrefRangeEnd = 194780, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TrySample(float x, float z, out Vector3 hitPoint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&x);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &z;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TrySample_Public_Static_Boolean_Single_Single_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public MapHeightSampler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
