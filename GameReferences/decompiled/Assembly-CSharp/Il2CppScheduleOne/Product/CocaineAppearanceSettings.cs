using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Product;

[System.Serializable]
public class CocaineAppearanceSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MainColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_SecondaryColor;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0;

	public unsafe Color32 MainColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MainColor);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MainColor)) = color;
		}
	}

	public unsafe Color32 SecondaryColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SecondaryColor);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SecondaryColor)) = color;
		}
	}

	static CocaineAppearanceSettings()
	{
		Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "CocaineAppearanceSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr);
		NativeFieldInfoPtr_MainColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr, "MainColor");
		NativeFieldInfoPtr_SecondaryColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr, "SecondaryColor");
		NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr, 100679186);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr, 100679187);
		NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr, 100679188);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235897, XrefRangeEnd = 235898, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CocaineAppearanceSettings(Color32 mainColor, Color32 secondaryColor)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&mainColor);
		*(Color32**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &secondaryColor;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CocaineAppearanceSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CocaineAppearanceSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 235898, RefRangeEnd = 235903, XrefRangeStart = 235898, XrefRangeEnd = 235898, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsUnintialized()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CocaineAppearanceSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
