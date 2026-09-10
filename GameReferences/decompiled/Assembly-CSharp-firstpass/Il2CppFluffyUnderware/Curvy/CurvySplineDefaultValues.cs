using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy;

public static class CurvySplineDefaultValues : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_AutoEndTangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_Orientation;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoHandleDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_CacheDensity;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxPointsPerUnit;

	private static readonly System.IntPtr NativeFieldInfoPtr_UsePooling;

	private static readonly System.IntPtr NativeFieldInfoPtr_UpdateIn;

	private static readonly System.IntPtr NativeFieldInfoPtr_CheckTransform;

	private static readonly System.IntPtr NativeFieldInfoPtr_BSplineDegree;

	private static readonly System.IntPtr NativeFieldInfoPtr_IsBSplineClamped;

	public unsafe static bool AutoEndTangents
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AutoEndTangents, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AutoEndTangents, (void*)(&flag));
		}
	}

	public unsafe static CurvyOrientation Orientation
	{
		get
		{
			Unsafe.SkipInit(out CurvyOrientation result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Orientation, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Orientation, (void*)(&curvyOrientation));
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

	public unsafe static int CacheDensity
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CacheDensity, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CacheDensity, (void*)(&num));
		}
	}

	public unsafe static float MaxPointsPerUnit
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxPointsPerUnit, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxPointsPerUnit, (void*)(&num));
		}
	}

	public unsafe static bool UsePooling
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UsePooling, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UsePooling, (void*)(&flag));
		}
	}

	public unsafe static CurvyUpdateMethod UpdateIn
	{
		get
		{
			Unsafe.SkipInit(out CurvyUpdateMethod result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UpdateIn, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UpdateIn, (void*)(&curvyUpdateMethod));
		}
	}

	public unsafe static bool CheckTransform
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CheckTransform, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CheckTransform, (void*)(&flag));
		}
	}

	public unsafe static int BSplineDegree
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BSplineDegree, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BSplineDegree, (void*)(&num));
		}
	}

	public unsafe static bool IsBSplineClamped
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IsBSplineClamped, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IsBSplineClamped, (void*)(&flag));
		}
	}

	static CurvySplineDefaultValues()
	{
		Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySplineDefaultValues");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr);
		NativeFieldInfoPtr_AutoEndTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "AutoEndTangents");
		NativeFieldInfoPtr_Orientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "Orientation");
		NativeFieldInfoPtr_AutoHandleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "AutoHandleDistance");
		NativeFieldInfoPtr_CacheDensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "CacheDensity");
		NativeFieldInfoPtr_MaxPointsPerUnit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "MaxPointsPerUnit");
		NativeFieldInfoPtr_UsePooling = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "UsePooling");
		NativeFieldInfoPtr_UpdateIn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "UpdateIn");
		NativeFieldInfoPtr_CheckTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "CheckTransform");
		NativeFieldInfoPtr_BSplineDegree = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "BSplineDegree");
		NativeFieldInfoPtr_IsBSplineClamped = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineDefaultValues>.NativeClassPtr, "IsBSplineClamped");
	}

	public CurvySplineDefaultValues(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
