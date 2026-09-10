using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Gamepad;

[System.Serializable]
public class GamepadSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_GamepadCameraSensitivity;

	private static readonly System.IntPtr NativeFieldInfoPtr_PointerSensitivity;

	private static readonly System.IntPtr NativeFieldInfoPtr_DEFAULT_SENSITIVITY;

	private static readonly System.IntPtr NativeFieldInfoPtr_MIN_SENSITIVITY;

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_SENSITIVITY;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float GamepadCameraSensitivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GamepadCameraSensitivity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GamepadCameraSensitivity)) = num;
		}
	}

	public unsafe float PointerSensitivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PointerSensitivity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PointerSensitivity)) = num;
		}
	}

	public unsafe static float DEFAULT_SENSITIVITY
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DEFAULT_SENSITIVITY, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DEFAULT_SENSITIVITY, (void*)(&num));
		}
	}

	public unsafe static float MIN_SENSITIVITY
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MIN_SENSITIVITY, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MIN_SENSITIVITY, (void*)(&num));
		}
	}

	public unsafe static float MAX_SENSITIVITY
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_SENSITIVITY, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_SENSITIVITY, (void*)(&num));
		}
	}

	static GamepadSettings()
	{
		Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "GamepadSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr);
		NativeFieldInfoPtr_GamepadCameraSensitivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr, "GamepadCameraSensitivity");
		NativeFieldInfoPtr_PointerSensitivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr, "PointerSensitivity");
		NativeFieldInfoPtr_DEFAULT_SENSITIVITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr, "DEFAULT_SENSITIVITY");
		NativeFieldInfoPtr_MIN_SENSITIVITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr, "MIN_SENSITIVITY");
		NativeFieldInfoPtr_MAX_SENSITIVITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr, "MAX_SENSITIVITY");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr, 100685959);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 294849, RefRangeEnd = 294852, XrefRangeStart = 294848, XrefRangeEnd = 294849, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GamepadSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GamepadSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GamepadSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
