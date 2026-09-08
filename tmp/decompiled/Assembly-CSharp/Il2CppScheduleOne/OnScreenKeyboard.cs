using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSteamworks;
using Il2CppSystem;

namespace Il2CppScheduleOne;

public static class OnScreenKeyboard : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ExitCooldown;

	private static readonly System.IntPtr NativeFieldInfoPtr__IsOpen_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__TimeOnLastClose_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_s_charLimit;

	private static readonly System.IntPtr NativeFieldInfoPtr_s_onSubmit;

	private static readonly System.IntPtr NativeFieldInfoPtr_s_onCancel;

	private static readonly System.IntPtr NativeFieldInfoPtr_s_onGamepadTextInputDismissed;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsOpen_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_IsOpen_Private_Static_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TimeOnLastClose_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_TimeOnLastClose_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Show_Public_Static_Void_Action_1_String_Action_String_UInt32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Exit_Private_Static_Void_ExitAction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Hide_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnGamepadTextInputDismissed_Private_Static_Void_GamepadTextInputDismissed_t_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnHide_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsOSKAvailable_Public_Static_Boolean_0;

	public unsafe static float ExitCooldown
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ExitCooldown, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ExitCooldown, (void*)(&num));
		}
	}

	public unsafe static bool _IsOpen_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__IsOpen_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__IsOpen_k__BackingField, (void*)(&flag));
		}
	}

	public unsafe static float _TimeOnLastClose_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__TimeOnLastClose_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__TimeOnLastClose_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static uint s_charLimit
	{
		get
		{
			Unsafe.SkipInit(out uint result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_s_charLimit, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_s_charLimit, (void*)(&num));
		}
	}

	public unsafe static Il2CppSystem.Action<string> s_onSubmit
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_s_onSubmit, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<string>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_s_onSubmit, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static Il2CppSystem.Action s_onCancel
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_s_onCancel, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_s_onCancel, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static Callback<GamepadTextInputDismissed_t> s_onGamepadTextInputDismissed
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_s_onGamepadTextInputDismissed, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Callback<GamepadTextInputDismissed_t>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_s_onGamepadTextInputDismissed, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback));
		}
	}

	public unsafe static bool IsOpen
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79648, XrefRangeEnd = 79652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsOpen_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79652, XrefRangeEnd = 79656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsOpen_Private_Static_set_Void_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float TimeOnLastClose
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79656, XrefRangeEnd = 79660, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TimeOnLastClose_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79660, XrefRangeEnd = 79664, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TimeOnLastClose_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static OnScreenKeyboard()
	{
		Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "OnScreenKeyboard");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr);
		NativeFieldInfoPtr_ExitCooldown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "ExitCooldown");
		NativeFieldInfoPtr__IsOpen_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "<IsOpen>k__BackingField");
		NativeFieldInfoPtr__TimeOnLastClose_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "<TimeOnLastClose>k__BackingField");
		NativeFieldInfoPtr_s_charLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "s_charLimit");
		NativeFieldInfoPtr_s_onSubmit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "s_onSubmit");
		NativeFieldInfoPtr_s_onCancel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "s_onCancel");
		NativeFieldInfoPtr_s_onGamepadTextInputDismissed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, "s_onGamepadTextInputDismissed");
		NativeMethodInfoPtr_get_IsOpen_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664983);
		NativeMethodInfoPtr_set_IsOpen_Private_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664984);
		NativeMethodInfoPtr_get_TimeOnLastClose_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664985);
		NativeMethodInfoPtr_set_TimeOnLastClose_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664986);
		NativeMethodInfoPtr_Show_Public_Static_Void_Action_1_String_Action_String_UInt32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664987);
		NativeMethodInfoPtr_Exit_Private_Static_Void_ExitAction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664988);
		NativeMethodInfoPtr_Hide_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664989);
		NativeMethodInfoPtr_OnGamepadTextInputDismissed_Private_Static_Void_GamepadTextInputDismissed_t_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664990);
		NativeMethodInfoPtr_OnHide_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664991);
		NativeMethodInfoPtr_IsOSKAvailable_Public_Static_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnScreenKeyboard>.NativeClassPtr, 100664992);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79727, RefRangeEnd = 79728, XrefRangeStart = 79664, XrefRangeEnd = 79727, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Show(Il2CppSystem.Action<string> onSubmit, Il2CppSystem.Action onCancel = null, string description = "", uint charMax = 32u, string defaultText = "")
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)onSubmit);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)onCancel);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(description);
		*(uint**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &charMax;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultText);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Show_Public_Static_Void_Action_1_String_Action_String_UInt32_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79728, XrefRangeEnd = 79758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Exit(ExitAction exit)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exit);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Exit_Private_Static_Void_ExitAction_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79758, XrefRangeEnd = 79778, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Hide()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Hide_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79778, XrefRangeEnd = 79793, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t param)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&param);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnGamepadTextInputDismissed_Private_Static_Void_GamepadTextInputDismissed_t_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 79836, RefRangeEnd = 79839, XrefRangeStart = 79793, XrefRangeEnd = 79836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OnHide()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnHide_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 79861, RefRangeEnd = 79865, XrefRangeStart = 79839, XrefRangeEnd = 79861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsOSKAvailable()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsOSKAvailable_Public_Static_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public OnScreenKeyboard(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
