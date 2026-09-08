using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Cartel;

[System.Serializable]
public class CartelGoonAppearance : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_IsMale;

	private static readonly System.IntPtr NativeFieldInfoPtr_BaseAppearanceIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_SkinColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_HairColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_ClothingIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_VoiceIndex;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Boolean_Int32_Color_Color_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool IsMale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsMale);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsMale)) = flag;
		}
	}

	public unsafe int BaseAppearanceIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseAppearanceIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseAppearanceIndex)) = num;
		}
	}

	public unsafe Color SkinColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SkinColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SkinColor)) = color;
		}
	}

	public unsafe Color HairColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HairColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HairColor)) = color;
		}
	}

	public unsafe int ClothingIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClothingIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClothingIndex)) = num;
		}
	}

	public unsafe int VoiceIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VoiceIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VoiceIndex)) = num;
		}
	}

	static CartelGoonAppearance()
	{
		Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Cartel", "CartelGoonAppearance");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr);
		NativeFieldInfoPtr_IsMale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, "IsMale");
		NativeFieldInfoPtr_BaseAppearanceIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, "BaseAppearanceIndex");
		NativeFieldInfoPtr_SkinColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, "SkinColor");
		NativeFieldInfoPtr_HairColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, "HairColor");
		NativeFieldInfoPtr_ClothingIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, "ClothingIndex");
		NativeFieldInfoPtr_VoiceIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, "VoiceIndex");
		NativeMethodInfoPtr__ctor_Public_Void_Boolean_Int32_Color_Color_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, 100676448);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr, 100676449);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210322, XrefRangeEnd = 210323, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelGoonAppearance(bool isMale, int baseAppearanceIndex, Color skinColor, Color hairColor, int clothingIndex, int voiceIndex)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&isMale);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &baseAppearanceIndex;
		*(Color**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &skinColor;
		*(Color**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &hairColor;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &clothingIndex;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &voiceIndex;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Boolean_Int32_Color_Color_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelGoonAppearance()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelGoonAppearance>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CartelGoonAppearance(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
