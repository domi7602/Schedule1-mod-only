using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Product;

[System.Serializable]
public class ShroomAppearanceSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultPrimaryColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultSecondaryColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultSpotsColor;

	private static readonly System.IntPtr NativeFieldInfoPtr__PrimaryColor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__SecondaryColor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__HasSpots_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__SpotsColor_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PrimaryColor_Public_get_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_PrimaryColor_Private_set_Void_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SecondaryColor_Public_get_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_SecondaryColor_Private_set_Void_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasSpots_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HasSpots_Private_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SpotsColor_Public_get_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_SpotsColor_Private_set_Void_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_Boolean_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0;

	public unsafe static Color32 DefaultPrimaryColor
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultPrimaryColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultPrimaryColor, (void*)(&color));
		}
	}

	public unsafe static Color32 DefaultSecondaryColor
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultSecondaryColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultSecondaryColor, (void*)(&color));
		}
	}

	public unsafe static Color32 DefaultSpotsColor
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultSpotsColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultSpotsColor, (void*)(&color));
		}
	}

	public unsafe Color32 _PrimaryColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PrimaryColor_k__BackingField);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PrimaryColor_k__BackingField)) = color;
		}
	}

	public unsafe Color32 _SecondaryColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SecondaryColor_k__BackingField);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SecondaryColor_k__BackingField)) = color;
		}
	}

	public unsafe bool _HasSpots_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HasSpots_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HasSpots_k__BackingField)) = flag;
		}
	}

	public unsafe Color32 _SpotsColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SpotsColor_k__BackingField);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SpotsColor_k__BackingField)) = color;
		}
	}

	public unsafe Color32 PrimaryColor
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29049, RefRangeEnd = 29051, XrefRangeStart = 29049, XrefRangeEnd = 29051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PrimaryColor_Public_get_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color32*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_PrimaryColor_Private_set_Void_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color32 SecondaryColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SecondaryColor_Public_get_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color32*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 168968, RefRangeEnd = 168976, XrefRangeStart = 168968, XrefRangeEnd = 168976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SecondaryColor_Private_set_Void_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool HasSpots
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasSpots_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 192743, RefRangeEnd = 192745, XrefRangeStart = 192743, XrefRangeEnd = 192745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HasSpots_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color32 SpotsColor
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 36888, RefRangeEnd = 36892, XrefRangeStart = 36888, XrefRangeEnd = 36892, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SpotsColor_Public_get_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color32*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SpotsColor_Private_set_Void_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ShroomAppearanceSettings()
	{
		Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "ShroomAppearanceSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultPrimaryColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "DefaultPrimaryColor");
		NativeFieldInfoPtr_DefaultSecondaryColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "DefaultSecondaryColor");
		NativeFieldInfoPtr_DefaultSpotsColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "DefaultSpotsColor");
		NativeFieldInfoPtr__PrimaryColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "<PrimaryColor>k__BackingField");
		NativeFieldInfoPtr__SecondaryColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "<SecondaryColor>k__BackingField");
		NativeFieldInfoPtr__HasSpots_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "<HasSpots>k__BackingField");
		NativeFieldInfoPtr__SpotsColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, "<SpotsColor>k__BackingField");
		NativeMethodInfoPtr_get_PrimaryColor_Public_get_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679598);
		NativeMethodInfoPtr_set_PrimaryColor_Private_set_Void_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679599);
		NativeMethodInfoPtr_get_SecondaryColor_Public_get_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679600);
		NativeMethodInfoPtr_set_SecondaryColor_Private_set_Void_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679601);
		NativeMethodInfoPtr_get_HasSpots_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679602);
		NativeMethodInfoPtr_set_HasSpots_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679603);
		NativeMethodInfoPtr_get_SpotsColor_Public_get_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679604);
		NativeMethodInfoPtr_set_SpotsColor_Private_set_Void_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679605);
		NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_Boolean_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679606);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679607);
		NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr, 100679608);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 241898, XrefRangeEnd = 241899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShroomAppearanceSettings(Color32 primary, Color32 secondary, bool hasSpots, Color32 spotsColor)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&primary);
		*(Color32**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &secondary;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &hasSpots;
		*(Color32**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &spotsColor;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_Boolean_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShroomAppearanceSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShroomAppearanceSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 235898, RefRangeEnd = 235903, XrefRangeStart = 235898, XrefRangeEnd = 235903, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsUnintialized()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ShroomAppearanceSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
