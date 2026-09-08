using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppSystem;

namespace Il2CppScheduleOne.State;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct StateProperties
{
	[OriginalName("Assembly-CSharp.dll", "", "EPreset")]
	public enum EPreset
	{
		Unenforced,
		UIDefault,
		Vehicle,
		UIWithBlur,
		Task,
		UINoInventory,
		Cutscene
	}

	[OriginalName("Assembly-CSharp.dll", "", "EMouseState")]
	public enum EMouseState
	{
		Unenforced,
		Free,
		Locked
	}

	[OriginalName("Assembly-CSharp.dll", "", "ECrosshairState")]
	public enum ECrosshairState
	{
		Unenforced,
		Visible,
		Hidden
	}

	[OriginalName("Assembly-CSharp.dll", "", "EEquippingState")]
	public enum EEquippingState
	{
		Unenforced,
		Enabled,
		Disabled
	}

	[OriginalName("Assembly-CSharp.dll", "", "EInventoryState")]
	public enum EInventoryState
	{
		Unenforced,
		Interactable,
		NonInteractable,
		Disabled
	}

	[OriginalName("Assembly-CSharp.dll", "", "EHUDState")]
	public enum EHUDState
	{
		Unenforced,
		Visible,
		Hidden
	}

	[OriginalName("Assembly-CSharp.dll", "", "EMovementState")]
	public enum EMovementState
	{
		Unenforced,
		Free,
		Locked
	}

	[OriginalName("Assembly-CSharp.dll", "", "ELookState")]
	public enum ELookState
	{
		Unenforced,
		Free,
		Locked
	}

	[OriginalName("Assembly-CSharp.dll", "", "EBlurState")]
	public enum EBlurState
	{
		Unenforced,
		Enabled,
		Disabled
	}

	[OriginalName("Assembly-CSharp.dll", "", "ECompassState")]
	public enum ECompassState
	{
		Unenforced,
		Visible,
		Hidden
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MouseState;

	private static readonly System.IntPtr NativeFieldInfoPtr_Crosshair;

	private static readonly System.IntPtr NativeFieldInfoPtr_Equipping;

	private static readonly System.IntPtr NativeFieldInfoPtr_Inventory;

	private static readonly System.IntPtr NativeFieldInfoPtr_HUD;

	private static readonly System.IntPtr NativeFieldInfoPtr_Compass;

	private static readonly System.IntPtr NativeFieldInfoPtr_Movement;

	private static readonly System.IntPtr NativeFieldInfoPtr_CameraLook;

	private static readonly System.IntPtr NativeFieldInfoPtr_Blur;

	private static readonly System.IntPtr NativeFieldInfoPtr_CameraMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_Unenforced;

	private static readonly System.IntPtr NativeFieldInfoPtr_UIDefault;

	private static readonly System.IntPtr NativeFieldInfoPtr_UIWithBlur;

	private static readonly System.IntPtr NativeFieldInfoPtr_Vehicle;

	private static readonly System.IntPtr NativeFieldInfoPtr_Skateboard;

	private static readonly System.IntPtr NativeFieldInfoPtr_Task;

	private static readonly System.IntPtr NativeFieldInfoPtr_UINoInventory;

	private static readonly System.IntPtr NativeFieldInfoPtr_Cutscene;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_EMouseState_ECrosshairState_EEquippingState_EInventoryState_EHUDState_ECompassState_EMovementState_ELookState_EBlurState_ECameraMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Transition_Public_Static_Void_StateProperties_StateProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPreset_Public_Static_StateProperties_EPreset_0;

	[FieldOffset(0)]
	public EMouseState MouseState;

	[FieldOffset(4)]
	public ECrosshairState Crosshair;

	[FieldOffset(8)]
	public EEquippingState Equipping;

	[FieldOffset(12)]
	public EInventoryState Inventory;

	[FieldOffset(16)]
	public EHUDState HUD;

	[FieldOffset(20)]
	public ECompassState Compass;

	[FieldOffset(24)]
	public EMovementState Movement;

	[FieldOffset(28)]
	public ELookState CameraLook;

	[FieldOffset(32)]
	public EBlurState Blur;

	[FieldOffset(36)]
	public PlayerCamera.ECameraMode CameraMode;

	public unsafe static StateProperties Unenforced
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Unenforced, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Unenforced, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties UIDefault
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UIDefault, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UIDefault, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties UIWithBlur
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UIWithBlur, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UIWithBlur, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties Vehicle
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Vehicle, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Vehicle, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties Skateboard
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Skateboard, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Skateboard, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties Task
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Task, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Task, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties UINoInventory
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UINoInventory, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UINoInventory, (void*)(&stateProperties));
		}
	}

	public unsafe static StateProperties Cutscene
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Cutscene, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Cutscene, (void*)(&stateProperties));
		}
	}

	static StateProperties()
	{
		Il2CppClassPointerStore<StateProperties>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.State", "StateProperties");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StateProperties>.NativeClassPtr);
		NativeFieldInfoPtr_MouseState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "MouseState");
		NativeFieldInfoPtr_Crosshair = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Crosshair");
		NativeFieldInfoPtr_Equipping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Equipping");
		NativeFieldInfoPtr_Inventory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Inventory");
		NativeFieldInfoPtr_HUD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "HUD");
		NativeFieldInfoPtr_Compass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Compass");
		NativeFieldInfoPtr_Movement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Movement");
		NativeFieldInfoPtr_CameraLook = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "CameraLook");
		NativeFieldInfoPtr_Blur = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Blur");
		NativeFieldInfoPtr_CameraMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "CameraMode");
		NativeFieldInfoPtr_Unenforced = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Unenforced");
		NativeFieldInfoPtr_UIDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "UIDefault");
		NativeFieldInfoPtr_UIWithBlur = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "UIWithBlur");
		NativeFieldInfoPtr_Vehicle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Vehicle");
		NativeFieldInfoPtr_Skateboard = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Skateboard");
		NativeFieldInfoPtr_Task = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Task");
		NativeFieldInfoPtr_UINoInventory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "UINoInventory");
		NativeFieldInfoPtr_Cutscene = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, "Cutscene");
		NativeMethodInfoPtr__ctor_Public_Void_EMouseState_ECrosshairState_EEquippingState_EInventoryState_EHUDState_ECompassState_EMovementState_ELookState_EBlurState_ECameraMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, 100667120);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, 100667121);
		NativeMethodInfoPtr_Transition_Public_Static_Void_StateProperties_StateProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, 100667122);
		NativeMethodInfoPtr_GetPreset_Public_Static_StateProperties_EPreset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, 100667123);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 103774, RefRangeEnd = 103775, XrefRangeStart = 103774, XrefRangeEnd = 103774, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StateProperties(EMouseState mouseState = EMouseState.Unenforced, ECrosshairState crosshairVisible = ECrosshairState.Unenforced, EEquippingState equippingEnabled = EEquippingState.Unenforced, EInventoryState inventoryEnabled = EInventoryState.Unenforced, EHUDState hudVisible = EHUDState.Unenforced, ECompassState compassVisible = ECompassState.Unenforced, EMovementState canMove = EMovementState.Unenforced, ELookState canLook = ELookState.Unenforced, EBlurState blur = EBlurState.Unenforced, PlayerCamera.ECameraMode cameraMode = PlayerCamera.ECameraMode.Default)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[10];
		*ptr = (nint)(&mouseState);
		*(ECrosshairState**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &crosshairVisible;
		*(EEquippingState**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &equippingEnabled;
		*(EInventoryState**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &inventoryEnabled;
		*(EHUDState**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &hudVisible;
		*(ECompassState**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &compassVisible;
		*(EMovementState**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &canMove;
		*(ELookState**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &canLook;
		*(EBlurState**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &blur;
		*(PlayerCamera.ECameraMode**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraMode;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_EMouseState_ECrosshairState_EEquippingState_EInventoryState_EHUDState_ECompassState_EMovementState_ELookState_EBlurState_ECameraMode_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 103775, XrefRangeEnd = 103826, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 103883, RefRangeEnd = 103884, XrefRangeStart = 103826, XrefRangeEnd = 103883, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Transition(StateProperties from, StateProperties to)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&from);
		*(StateProperties**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &to;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Transition_Public_Static_Void_StateProperties_StateProperties_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 103922, RefRangeEnd = 103926, XrefRangeStart = 103884, XrefRangeEnd = 103922, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static StateProperties GetPreset(EPreset preset)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&preset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPreset_Public_Static_StateProperties_EPreset_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(StateProperties*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<StateProperties>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
