using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[System.Serializable]
public class CGModuleProperties : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Dimensions;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr_LabelWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr_BackgroundColor;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Rect Dimensions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dimensions);
			return *(Rect*)num;
		}
		set
		{
			*(Rect*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dimensions)) = rect;
		}
	}

	public unsafe float MinWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinWidth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinWidth)) = num;
		}
	}

	public unsafe float LabelWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelWidth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelWidth)) = num;
		}
	}

	public unsafe Color BackgroundColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BackgroundColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BackgroundColor)) = color;
		}
	}

	static CGModuleProperties()
	{
		Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGModuleProperties");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr);
		NativeFieldInfoPtr_Dimensions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr, "Dimensions");
		NativeFieldInfoPtr_MinWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr, "MinWidth");
		NativeFieldInfoPtr_LabelWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr, "LabelWidth");
		NativeFieldInfoPtr_BackgroundColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr, "BackgroundColor");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr, 100666070);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 49912, RefRangeEnd = 49913, XrefRangeStart = 49911, XrefRangeEnd = 49912, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleProperties()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGModuleProperties>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGModuleProperties(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
