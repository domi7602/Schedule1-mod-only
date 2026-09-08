using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppScheduleOne.AvatarFramework.Customization;

[System.Serializable]
public class BasicAvatarSettings : ScriptableObject
{
	private sealed class MethodInfoStoreGeneric_SetValue_Public_T_String_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_SetValue_Public_T_String_T_0, Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetValue_Public_T_String_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetValue_Public_T_String_0, Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_GenderScaleMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaleUnderwearPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_FemaleUnderwearPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_Gender;

	private static readonly System.IntPtr NativeFieldInfoPtr_Weight;

	private static readonly System.IntPtr NativeFieldInfoPtr_SkinColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_HairStyle;

	private static readonly System.IntPtr NativeFieldInfoPtr_HairColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Mouth;

	private static readonly System.IntPtr NativeFieldInfoPtr_FacialHair;

	private static readonly System.IntPtr NativeFieldInfoPtr_FacialDetails;

	private static readonly System.IntPtr NativeFieldInfoPtr_FacialDetailsIntensity;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyeballColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_UpperEyeLidRestingPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_LowerEyeLidRestingPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_PupilDilation;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyebrowScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyebrowThickness;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyebrowRestingHeight;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyebrowRestingAngle;

	private static readonly System.IntPtr NativeFieldInfoPtr_Top;

	private static readonly System.IntPtr NativeFieldInfoPtr_TopColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Bottom;

	private static readonly System.IntPtr NativeFieldInfoPtr_BottomColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Shoes;

	private static readonly System.IntPtr NativeFieldInfoPtr_ShoesColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Headwear;

	private static readonly System.IntPtr NativeFieldInfoPtr_HeadwearColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Eyewear;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyewearColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Tattoos;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetValue_Public_T_String_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetValue_Public_T_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAvatarSettings_Public_AvatarSettings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNippleColor_Public_Static_Color_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetJson_Public_Virtual_New_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float GenderScaleMultiplier
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GenderScaleMultiplier, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GenderScaleMultiplier, (void*)(&num));
		}
	}

	public unsafe static string MaleUnderwearPath
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaleUnderwearPath, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaleUnderwearPath, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string FemaleUnderwearPath
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FemaleUnderwearPath, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FemaleUnderwearPath, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int Gender
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Gender);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Gender)) = num;
		}
	}

	public unsafe float Weight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Weight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Weight)) = num;
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

	public unsafe string HairStyle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HairStyle);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HairStyle)), IL2CPP.ManagedStringToIl2Cpp(text));
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

	public unsafe string Mouth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mouth);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mouth)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string FacialHair
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FacialHair);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FacialHair)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string FacialDetails
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FacialDetails);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FacialDetails)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float FacialDetailsIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FacialDetailsIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FacialDetailsIntensity)) = num;
		}
	}

	public unsafe Color EyeballColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyeballColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyeballColor)) = color;
		}
	}

	public unsafe float UpperEyeLidRestingPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpperEyeLidRestingPosition);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpperEyeLidRestingPosition)) = num;
		}
	}

	public unsafe float LowerEyeLidRestingPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LowerEyeLidRestingPosition);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LowerEyeLidRestingPosition)) = num;
		}
	}

	public unsafe float PupilDilation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PupilDilation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PupilDilation)) = num;
		}
	}

	public unsafe float EyebrowScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowScale)) = num;
		}
	}

	public unsafe float EyebrowThickness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowThickness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowThickness)) = num;
		}
	}

	public unsafe float EyebrowRestingHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowRestingHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowRestingHeight)) = num;
		}
	}

	public unsafe float EyebrowRestingAngle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowRestingAngle);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyebrowRestingAngle)) = num;
		}
	}

	public unsafe string Top
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Top);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Top)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Color TopColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TopColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TopColor)) = color;
		}
	}

	public unsafe string Bottom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bottom);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bottom)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Color BottomColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BottomColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BottomColor)) = color;
		}
	}

	public unsafe string Shoes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Shoes);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Shoes)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Color ShoesColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShoesColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShoesColor)) = color;
		}
	}

	public unsafe string Headwear
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Headwear);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Headwear)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Color HeadwearColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HeadwearColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HeadwearColor)) = color;
		}
	}

	public unsafe string Eyewear
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Eyewear);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Eyewear)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Color EyewearColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyewearColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyewearColor)) = color;
		}
	}

	public unsafe List<string> Tattoos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tattoos);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tattoos)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static BasicAvatarSettings()
	{
		Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Customization", "BasicAvatarSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr);
		NativeFieldInfoPtr_GenderScaleMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "GenderScaleMultiplier");
		NativeFieldInfoPtr_MaleUnderwearPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "MaleUnderwearPath");
		NativeFieldInfoPtr_FemaleUnderwearPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "FemaleUnderwearPath");
		NativeFieldInfoPtr_Gender = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Gender");
		NativeFieldInfoPtr_Weight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Weight");
		NativeFieldInfoPtr_SkinColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "SkinColor");
		NativeFieldInfoPtr_HairStyle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "HairStyle");
		NativeFieldInfoPtr_HairColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "HairColor");
		NativeFieldInfoPtr_Mouth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Mouth");
		NativeFieldInfoPtr_FacialHair = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "FacialHair");
		NativeFieldInfoPtr_FacialDetails = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "FacialDetails");
		NativeFieldInfoPtr_FacialDetailsIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "FacialDetailsIntensity");
		NativeFieldInfoPtr_EyeballColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "EyeballColor");
		NativeFieldInfoPtr_UpperEyeLidRestingPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "UpperEyeLidRestingPosition");
		NativeFieldInfoPtr_LowerEyeLidRestingPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "LowerEyeLidRestingPosition");
		NativeFieldInfoPtr_PupilDilation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "PupilDilation");
		NativeFieldInfoPtr_EyebrowScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "EyebrowScale");
		NativeFieldInfoPtr_EyebrowThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "EyebrowThickness");
		NativeFieldInfoPtr_EyebrowRestingHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "EyebrowRestingHeight");
		NativeFieldInfoPtr_EyebrowRestingAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "EyebrowRestingAngle");
		NativeFieldInfoPtr_Top = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Top");
		NativeFieldInfoPtr_TopColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "TopColor");
		NativeFieldInfoPtr_Bottom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Bottom");
		NativeFieldInfoPtr_BottomColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "BottomColor");
		NativeFieldInfoPtr_Shoes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Shoes");
		NativeFieldInfoPtr_ShoesColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "ShoesColor");
		NativeFieldInfoPtr_Headwear = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Headwear");
		NativeFieldInfoPtr_HeadwearColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "HeadwearColor");
		NativeFieldInfoPtr_Eyewear = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Eyewear");
		NativeFieldInfoPtr_EyewearColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "EyewearColor");
		NativeFieldInfoPtr_Tattoos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, "Tattoos");
		NativeMethodInfoPtr_SetValue_Public_T_String_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, 100677615);
		NativeMethodInfoPtr_GetValue_Public_T_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, 100677616);
		NativeMethodInfoPtr_GetAvatarSettings_Public_AvatarSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, 100677617);
		NativeMethodInfoPtr_GetNippleColor_Public_Static_Color_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, 100677618);
		NativeMethodInfoPtr_GetJson_Public_Virtual_New_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, 100677619);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr, 100677620);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 222261, XrefRangeEnd = 222266, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T SetValue<T>(string fieldName, T value)
	{
		//IL_0073->IL0076: Incompatible stack types: I vs Ref
		//IL_0052->IL0076: Incompatible stack types: I vs Ref
		//IL_005f->IL0076: Incompatible stack types: I vs Ref
		//IL_0066->IL0076: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldName);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = value;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref value;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_SetValue_Public_T_String_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 222272, RefRangeEnd = 222273, XrefRangeStart = 222266, XrefRangeEnd = 222272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetValue<T>(string fieldName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetValue_Public_T_String_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 222375, RefRangeEnd = 222387, XrefRangeStart = 222273, XrefRangeEnd = 222375, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AvatarSettings GetAvatarSettings()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAvatarSettings_Public_AvatarSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AvatarSettings>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe static Color GetNippleColor(Color skinColor)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&skinColor);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNippleColor_Public_Static_Color_Color_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual string GetJson(bool prettyPrint = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&prettyPrint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetJson_Public_Virtual_New_String_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 222387, XrefRangeEnd = 222395, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BasicAvatarSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BasicAvatarSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BasicAvatarSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
