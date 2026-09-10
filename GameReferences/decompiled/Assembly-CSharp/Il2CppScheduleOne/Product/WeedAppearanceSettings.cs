using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Product;

[System.Serializable]
public class WeedAppearanceSettings : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EWeedAppearanceType")]
	public enum EWeedAppearanceType
	{
		Main,
		Secondary,
		Leaf,
		Stem
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MainColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_SecondaryColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeafColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_StemColor;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_Color32_Color32_0;

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

	public unsafe Color32 LeafColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeafColor);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeafColor)) = color;
		}
	}

	public unsafe Color32 StemColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StemColor);
			return *(Color32*)num;
		}
		set
		{
			*(Color32*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StemColor)) = color;
		}
	}

	static WeedAppearanceSettings()
	{
		Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "WeedAppearanceSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr);
		NativeFieldInfoPtr_MainColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, "MainColor");
		NativeFieldInfoPtr_SecondaryColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, "SecondaryColor");
		NativeFieldInfoPtr_LeafColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, "LeafColor");
		NativeFieldInfoPtr_StemColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, "StemColor");
		NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_Color32_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, 100679669);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, 100679670);
		NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr, 100679671);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 242460, RefRangeEnd = 242461, XrefRangeStart = 242459, XrefRangeEnd = 242460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WeedAppearanceSettings(Color32 mainColor, Color32 secondaryColor, Color32 leafColor, Color32 stemColor)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&mainColor);
		*(Color32**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &secondaryColor;
		*(Color32**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &leafColor;
		*(Color32**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &stemColor;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Color32_Color32_Color32_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WeedAppearanceSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WeedAppearanceSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 242461, RefRangeEnd = 242462, XrefRangeStart = 242461, XrefRangeEnd = 242461, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsUnintialized()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsUnintialized_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public WeedAppearanceSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
