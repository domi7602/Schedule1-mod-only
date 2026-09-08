using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Gamepad;
using Il2CppSystem;
using UnityEngine;

namespace Il2Cpp;

public class GamepadPointerSnapToHandler : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__manager;

	private static readonly System.IntPtr NativeFieldInfoPtr__maxFriction;

	private static readonly System.IntPtr NativeFieldInfoPtr__frictionCurve;

	private static readonly System.IntPtr NativeFieldInfoPtr__currentSnapTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr__previousSnapTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr__mainCamera;

	private static readonly System.IntPtr NativeFieldInfoPtr__requireJoystickReset;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasInputReset;

	private static readonly System.IntPtr NativeFieldInfoPtr__currentDistanceToTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr_INPUT_DEADZONE_SQR;

	private static readonly System.IntPtr NativeFieldInfoPtr_PIXEL_DISTANCE_REFERENCE;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialise_Public_Virtual_Final_New_Void_GamepadPointer_Single_AnimationCurve_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPosition_Public_Virtual_Final_New_Vector2_Vector2_Vector2_Single_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindClosestLure_Private_IGamepadPointerLure_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindLureToSnapTo_Private_IGamepadPointerLure_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLureScreenPosition_Private_Vector2_IGamepadPointerLure_0;

	public unsafe GamepadPointer _manager
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manager);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GamepadPointer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manager)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gamepadPointer));
		}
	}

	public unsafe float _maxFriction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxFriction);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxFriction)) = num;
		}
	}

	public unsafe AnimationCurve _frictionCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frictionCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frictionCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe IGamepadPointerLure _currentSnapTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentSnapTarget);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IGamepadPointerLure>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentSnapTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gamepadPointerLure));
		}
	}

	public unsafe IGamepadPointerLure _previousSnapTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__previousSnapTarget);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IGamepadPointerLure>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__previousSnapTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gamepadPointerLure));
		}
	}

	public unsafe Camera _mainCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mainCamera);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Camera>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mainCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera));
		}
	}

	public unsafe bool _requireJoystickReset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__requireJoystickReset);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__requireJoystickReset)) = flag;
		}
	}

	public unsafe bool _hasInputReset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasInputReset);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasInputReset)) = flag;
		}
	}

	public unsafe float _currentDistanceToTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentDistanceToTarget);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentDistanceToTarget)) = num;
		}
	}

	public unsafe static float INPUT_DEADZONE_SQR
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_INPUT_DEADZONE_SQR, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_INPUT_DEADZONE_SQR, (void*)(&num));
		}
	}

	public unsafe static float PIXEL_DISTANCE_REFERENCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PIXEL_DISTANCE_REFERENCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PIXEL_DISTANCE_REFERENCE, (void*)(&num));
		}
	}

	static GamepadPointerSnapToHandler()
	{
		Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "GamepadPointerSnapToHandler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr);
		NativeFieldInfoPtr__manager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_manager");
		NativeFieldInfoPtr__maxFriction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_maxFriction");
		NativeFieldInfoPtr__frictionCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_frictionCurve");
		NativeFieldInfoPtr__currentSnapTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_currentSnapTarget");
		NativeFieldInfoPtr__previousSnapTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_previousSnapTarget");
		NativeFieldInfoPtr__mainCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_mainCamera");
		NativeFieldInfoPtr__requireJoystickReset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_requireJoystickReset");
		NativeFieldInfoPtr__hasInputReset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_hasInputReset");
		NativeFieldInfoPtr__currentDistanceToTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "_currentDistanceToTarget");
		NativeFieldInfoPtr_INPUT_DEADZONE_SQR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "INPUT_DEADZONE_SQR");
		NativeFieldInfoPtr_PIXEL_DISTANCE_REFERENCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, "PIXEL_DISTANCE_REFERENCE");
		NativeMethodInfoPtr__ctor_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, 100663348);
		NativeMethodInfoPtr_Initialise_Public_Virtual_Final_New_Void_GamepadPointer_Single_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, 100663349);
		NativeMethodInfoPtr_GetPosition_Public_Virtual_Final_New_Vector2_Vector2_Vector2_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, 100663350);
		NativeMethodInfoPtr_FindClosestLure_Private_IGamepadPointerLure_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, 100663351);
		NativeMethodInfoPtr_FindLureToSnapTo_Private_IGamepadPointerLure_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, 100663352);
		NativeMethodInfoPtr_GetLureScreenPosition_Private_Vector2_IGamepadPointerLure_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr, 100663353);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 65202, RefRangeEnd = 65206, XrefRangeStart = 65201, XrefRangeEnd = 65202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GamepadPointerSnapToHandler(bool requireJoystickReset = true)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GamepadPointerSnapToHandler>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&requireJoystickReset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65206, XrefRangeEnd = 65210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Initialise(GamepadPointer manager, float maxFriction, AnimationCurve frictionCurve)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)manager);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxFriction;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)frictionCurve);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialise_Public_Virtual_Final_New_Void_GamepadPointer_Single_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65210, XrefRangeEnd = 65216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual Vector2 GetPosition(Vector2 rawInput, Vector2 pointerPosition, float speed, bool isAimAssistActive)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&rawInput);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointerPosition;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &speed;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &isAimAssistActive;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPosition_Public_Virtual_Final_New_Vector2_Vector2_Vector2_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 65235, RefRangeEnd = 65236, XrefRangeStart = 65216, XrefRangeEnd = 65235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IGamepadPointerLure FindClosestLure(Vector2 pointerPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&pointerPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindClosestLure_Private_IGamepadPointerLure_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IGamepadPointerLure>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65236, XrefRangeEnd = 65256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IGamepadPointerLure FindLureToSnapTo(Vector2 normalizedInputDir, Vector2 pointerPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&normalizedInputDir);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointerPosition;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindLureToSnapTo_Private_IGamepadPointerLure_Vector2_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IGamepadPointerLure>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 65262, RefRangeEnd = 65265, XrefRangeStart = 65256, XrefRangeEnd = 65262, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 GetLureScreenPosition(IGamepadPointerLure lure)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lure);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLureScreenPosition_Private_Vector2_IGamepadPointerLure_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public GamepadPointerSnapToHandler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
