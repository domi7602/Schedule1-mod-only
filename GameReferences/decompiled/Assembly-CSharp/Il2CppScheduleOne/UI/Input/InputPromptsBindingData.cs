using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.UI.Input;

public class InputPromptsBindingData : ScriptableObject
{
	[OriginalName("Assembly-CSharp.dll", "", "ESpriteSettingType")]
	public enum ESpriteSettingType
	{
		Auto,
		Custom
	}

	private static readonly IntPtr NativeFieldInfoPtr_ControlScheme;

	private static readonly IntPtr NativeFieldInfoPtr_PlatformType;

	private static readonly IntPtr NativeFieldInfoPtr_ControlPath;

	private static readonly IntPtr NativeFieldInfoPtr_Sprite;

	private static readonly IntPtr NativeFieldInfoPtr_SpriteVariation;

	private static readonly IntPtr NativeFieldInfoPtr_SpriteSize;

	private static readonly IntPtr NativeFieldInfoPtr_SpriteLabelSettingType;

	private static readonly IntPtr NativeFieldInfoPtr_SpriteLabel;

	private static readonly IntPtr NativeFieldInfoPtr_SpritePixelMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_EnableSpriteBackdrop;

	private static readonly IntPtr NativeFieldInfoPtr_SpriteColor;

	private static readonly IntPtr NativeFieldInfoPtr_InlineId;

	private static readonly IntPtr NativeFieldInfoPtr_InlineLabel;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GameInput.InputDeviceType ControlScheme
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlScheme);
			return *(GameInput.InputDeviceType*)num;
		}
		set
		{
			*(GameInput.InputDeviceType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlScheme)) = inputDeviceType;
		}
	}

	public unsafe EPlatformType PlatformType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlatformType);
			return *(EPlatformType*)num;
		}
		set
		{
			*(EPlatformType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlatformType)) = ePlatformType;
		}
	}

	public unsafe string ControlPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPath);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPath)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Sprite Sprite
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Sprite);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Sprite)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite));
		}
	}

	public unsafe Sprite SpriteVariation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteVariation);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteVariation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite));
		}
	}

	public unsafe Vector2 SpriteSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteSize);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteSize)) = vector;
		}
	}

	public unsafe ESpriteSettingType SpriteLabelSettingType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteLabelSettingType);
			return *(ESpriteSettingType*)num;
		}
		set
		{
			*(ESpriteSettingType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteLabelSettingType)) = eSpriteSettingType;
		}
	}

	public unsafe string SpriteLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteLabel);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteLabel)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float SpritePixelMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpritePixelMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpritePixelMultiplier)) = num;
		}
	}

	public unsafe bool EnableSpriteBackdrop
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EnableSpriteBackdrop);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EnableSpriteBackdrop)) = flag;
		}
	}

	public unsafe Color SpriteColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpriteColor)) = color;
		}
	}

	public unsafe string InlineId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InlineId);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InlineId)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string InlineLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InlineLabel);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InlineLabel)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static InputPromptsBindingData()
	{
		Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Input", "InputPromptsBindingData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr);
		NativeFieldInfoPtr_ControlScheme = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "ControlScheme");
		NativeFieldInfoPtr_PlatformType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "PlatformType");
		NativeFieldInfoPtr_ControlPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "ControlPath");
		NativeFieldInfoPtr_Sprite = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "Sprite");
		NativeFieldInfoPtr_SpriteVariation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "SpriteVariation");
		NativeFieldInfoPtr_SpriteSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "SpriteSize");
		NativeFieldInfoPtr_SpriteLabelSettingType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "SpriteLabelSettingType");
		NativeFieldInfoPtr_SpriteLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "SpriteLabel");
		NativeFieldInfoPtr_SpritePixelMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "SpritePixelMultiplier");
		NativeFieldInfoPtr_EnableSpriteBackdrop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "EnableSpriteBackdrop");
		NativeFieldInfoPtr_SpriteColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "SpriteColor");
		NativeFieldInfoPtr_InlineId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "InlineId");
		NativeFieldInfoPtr_InlineLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, "InlineLabel");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr, 100689118);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 328977, XrefRangeEnd = 328978, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputPromptsBindingData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InputPromptsBindingData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InputPromptsBindingData(IntPtr pointer)
		: base(pointer)
	{
	}
}
