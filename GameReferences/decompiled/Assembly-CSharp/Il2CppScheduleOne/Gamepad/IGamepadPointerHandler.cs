using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Gamepad;

public class IGamepadPointerHandler : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_Initialise_Public_Abstract_Virtual_New_Void_GamepadPointer_Single_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetPosition_Public_Abstract_Virtual_New_Vector2_Vector2_Vector2_Single_Boolean_0;

	static IGamepadPointerHandler()
	{
		Il2CppClassPointerStore<IGamepadPointerHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "IGamepadPointerHandler");
		NativeMethodInfoPtr_Initialise_Public_Abstract_Virtual_New_Void_GamepadPointer_Single_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IGamepadPointerHandler>.NativeClassPtr, 100685898);
		NativeMethodInfoPtr_GetPosition_Public_Abstract_Virtual_New_Vector2_Vector2_Vector2_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IGamepadPointerHandler>.NativeClassPtr, 100685899);
	}

	[CallerCount(0)]
	public unsafe virtual void Initialise(GamepadPointer manager, float maxFriction, AnimationCurve frictionCurve)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)manager);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &maxFriction;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)frictionCurve);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialise_Public_Abstract_Virtual_New_Void_GamepadPointer_Single_AnimationCurve_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual Vector2 GetPosition(Vector2 rawInput, Vector2 pointerPosition, float speed, bool isAimAssistActive)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)(&rawInput);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &pointerPosition;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &speed;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &isAimAssistActive;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetPosition_Public_Abstract_Virtual_New_Vector2_Vector2_Vector2_Single_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public IGamepadPointerHandler(IntPtr pointer)
		: base(pointer)
	{
	}
}
