using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public static class CurvySplineSegmentDefaultValues : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Swirl;

	private static readonly System.IntPtr NativeFieldInfoPtr_SynchronizeTCB;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoHandles;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoHandleDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_HandleIn;

	private static readonly System.IntPtr NativeFieldInfoPtr_HandleOut;

	public unsafe static CurvyOrientationSwirl Swirl
	{
		get
		{
			Unsafe.SkipInit(out CurvyOrientationSwirl result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Swirl, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Swirl, (void*)(&curvyOrientationSwirl));
		}
	}

	public unsafe static bool SynchronizeTCB
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SynchronizeTCB, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SynchronizeTCB, (void*)(&flag));
		}
	}

	public unsafe static bool AutoHandles
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AutoHandles, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AutoHandles, (void*)(&flag));
		}
	}

	public unsafe static float AutoHandleDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AutoHandleDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AutoHandleDistance, (void*)(&num));
		}
	}

	public unsafe static Vector3 HandleIn
	{
		get
		{
			Unsafe.SkipInit(out Vector3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HandleIn, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HandleIn, (void*)(&vector));
		}
	}

	public unsafe static Vector3 HandleOut
	{
		get
		{
			Unsafe.SkipInit(out Vector3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HandleOut, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HandleOut, (void*)(&vector));
		}
	}

	static CurvySplineSegmentDefaultValues()
	{
		Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySplineSegmentDefaultValues");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr);
		NativeFieldInfoPtr_Swirl = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr, "Swirl");
		NativeFieldInfoPtr_SynchronizeTCB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr, "SynchronizeTCB");
		NativeFieldInfoPtr_AutoHandles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr, "AutoHandles");
		NativeFieldInfoPtr_AutoHandleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr, "AutoHandleDistance");
		NativeFieldInfoPtr_HandleIn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr, "HandleIn");
		NativeFieldInfoPtr_HandleOut = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegmentDefaultValues>.NativeClassPtr, "HandleOut");
	}

	public CurvySplineSegmentDefaultValues(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
