using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.ItemFramework;

public static class ItemQuality : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Heavenly_Threshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_Premium_Threshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_Standard_Threshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_Poor_Threshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_Heavenly_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Premium_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Standard_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Poor_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Trash_Color;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetQuality_Public_Static_EQuality_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShiftQuality_Public_Static_EQuality_EQuality_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColor_Public_Static_Color_EQuality_0;

	public unsafe static float Heavenly_Threshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Heavenly_Threshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Heavenly_Threshold, (void*)(&num));
		}
	}

	public unsafe static float Premium_Threshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Premium_Threshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Premium_Threshold, (void*)(&num));
		}
	}

	public unsafe static float Standard_Threshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Standard_Threshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Standard_Threshold, (void*)(&num));
		}
	}

	public unsafe static float Poor_Threshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Poor_Threshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Poor_Threshold, (void*)(&num));
		}
	}

	public unsafe static Color Heavenly_Color
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Heavenly_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Heavenly_Color, (void*)(&color));
		}
	}

	public unsafe static Color Premium_Color
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Premium_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Premium_Color, (void*)(&color));
		}
	}

	public unsafe static Color Standard_Color
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Standard_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Standard_Color, (void*)(&color));
		}
	}

	public unsafe static Color Poor_Color
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Poor_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Poor_Color, (void*)(&color));
		}
	}

	public unsafe static Color Trash_Color
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Trash_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Trash_Color, (void*)(&color));
		}
	}

	static ItemQuality()
	{
		Il2CppClassPointerStore<ItemQuality>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemQuality");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr);
		NativeFieldInfoPtr_Heavenly_Threshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Heavenly_Threshold");
		NativeFieldInfoPtr_Premium_Threshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Premium_Threshold");
		NativeFieldInfoPtr_Standard_Threshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Standard_Threshold");
		NativeFieldInfoPtr_Poor_Threshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Poor_Threshold");
		NativeFieldInfoPtr_Heavenly_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Heavenly_Color");
		NativeFieldInfoPtr_Premium_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Premium_Color");
		NativeFieldInfoPtr_Standard_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Standard_Color");
		NativeFieldInfoPtr_Poor_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Poor_Color");
		NativeFieldInfoPtr_Trash_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, "Trash_Color");
		NativeMethodInfoPtr_GetQuality_Public_Static_EQuality_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, 100672549);
		NativeMethodInfoPtr_ShiftQuality_Public_Static_EQuality_EQuality_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, 100672550);
		NativeMethodInfoPtr_GetColor_Public_Static_Color_EQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemQuality>.NativeClassPtr, 100672551);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 167638, RefRangeEnd = 167641, XrefRangeStart = 167638, XrefRangeEnd = 167638, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EQuality GetQuality(float qualityScalar)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&qualityScalar);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetQuality_Public_Static_EQuality_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 167652, RefRangeEnd = 167655, XrefRangeStart = 167641, XrefRangeEnd = 167652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EQuality ShiftQuality(EQuality baseQuality, int shiftAmount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&baseQuality);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &shiftAmount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShiftQuality_Public_Static_EQuality_EQuality_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 167662, RefRangeEnd = 167671, XrefRangeStart = 167655, XrefRangeEnd = 167662, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetColor(EQuality quality)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&quality);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColor_Public_Static_Color_EQuality_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ItemQuality(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
