using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Experimental;

[System.Serializable]
public class SkateboardSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_TurnForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_TurnChangeRate;

	private static readonly System.IntPtr NativeFieldInfoPtr_TurnReturnToRestRate;

	private static readonly System.IntPtr NativeFieldInfoPtr_TurnSpeedBoost;

	private static readonly System.IntPtr NativeFieldInfoPtr_TurnForceMap;

	private static readonly System.IntPtr NativeFieldInfoPtr_Gravity;

	private static readonly System.IntPtr NativeFieldInfoPtr_BrakeForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_ReverseTopSpeed_Kmh;

	private static readonly System.IntPtr NativeFieldInfoPtr_RotationClampForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_FrictionEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_LongitudinalFrictionCurve;

	private static readonly System.IntPtr NativeFieldInfoPtr_LongitudinalFrictionMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_LateralFrictionForceMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_JumpForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_JumpDuration_Min;

	private static readonly System.IntPtr NativeFieldInfoPtr_JumpDuration_Max;

	private static readonly System.IntPtr NativeFieldInfoPtr_FrontAxleJumpCurve;

	private static readonly System.IntPtr NativeFieldInfoPtr_RearAxleJumpCurve;

	private static readonly System.IntPtr NativeFieldInfoPtr_JumpForwardForceCurve;

	private static readonly System.IntPtr NativeFieldInfoPtr_JumpForwardBoost;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoverForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoverRayLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoverHeight;

	private static readonly System.IntPtr NativeFieldInfoPtr_Hover_P;

	private static readonly System.IntPtr NativeFieldInfoPtr_Hover_I;

	private static readonly System.IntPtr NativeFieldInfoPtr_Hover_D;

	private static readonly System.IntPtr NativeFieldInfoPtr_TopSpeed_Kmh;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushForceMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushForceMultiplierMap;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushForceDuration;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushDelay;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushForceCurve;

	private static readonly System.IntPtr NativeFieldInfoPtr_AirMovementEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_AirMovementForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_AirMovementJumpReductionDuration;

	private static readonly System.IntPtr NativeFieldInfoPtr_AirMovementJumpReductionCurve;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TopSpeed_Ms_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_SkateboardSettings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Blend_Public_SkateboardSettings_SkateboardSettings_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float TurnForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnForce)) = num;
		}
	}

	public unsafe float TurnChangeRate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnChangeRate);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnChangeRate)) = num;
		}
	}

	public unsafe float TurnReturnToRestRate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnReturnToRestRate);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnReturnToRestRate)) = num;
		}
	}

	public unsafe float TurnSpeedBoost
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnSpeedBoost);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnSpeedBoost)) = num;
		}
	}

	public unsafe AnimationCurve TurnForceMap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnForceMap);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnForceMap)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float Gravity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Gravity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Gravity)) = num;
		}
	}

	public unsafe float BrakeForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrakeForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrakeForce)) = num;
		}
	}

	public unsafe float ReverseTopSpeed_Kmh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReverseTopSpeed_Kmh);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReverseTopSpeed_Kmh)) = num;
		}
	}

	public unsafe float RotationClampForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RotationClampForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RotationClampForce)) = num;
		}
	}

	public unsafe bool FrictionEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrictionEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrictionEnabled)) = flag;
		}
	}

	public unsafe AnimationCurve LongitudinalFrictionCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LongitudinalFrictionCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LongitudinalFrictionCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float LongitudinalFrictionMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LongitudinalFrictionMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LongitudinalFrictionMultiplier)) = num;
		}
	}

	public unsafe float LateralFrictionForceMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LateralFrictionForceMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LateralFrictionForceMultiplier)) = num;
		}
	}

	public unsafe float JumpForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpForce)) = num;
		}
	}

	public unsafe float JumpDuration_Min
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpDuration_Min);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpDuration_Min)) = num;
		}
	}

	public unsafe float JumpDuration_Max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpDuration_Max);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpDuration_Max)) = num;
		}
	}

	public unsafe AnimationCurve FrontAxleJumpCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrontAxleJumpCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrontAxleJumpCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe AnimationCurve RearAxleJumpCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RearAxleJumpCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RearAxleJumpCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe AnimationCurve JumpForwardForceCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpForwardForceCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpForwardForceCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float JumpForwardBoost
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpForwardBoost);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpForwardBoost)) = num;
		}
	}

	public unsafe float HoverForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverForce)) = num;
		}
	}

	public unsafe float HoverRayLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverRayLength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverRayLength)) = num;
		}
	}

	public unsafe float HoverHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverHeight)) = num;
		}
	}

	public unsafe float Hover_P
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hover_P);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hover_P)) = num;
		}
	}

	public unsafe float Hover_I
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hover_I);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hover_I)) = num;
		}
	}

	public unsafe float Hover_D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hover_D);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hover_D)) = num;
		}
	}

	public unsafe float TopSpeed_Kmh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TopSpeed_Kmh);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TopSpeed_Kmh)) = num;
		}
	}

	public unsafe float PushForceMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceMultiplier)) = num;
		}
	}

	public unsafe AnimationCurve PushForceMultiplierMap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceMultiplierMap);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceMultiplierMap)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float PushForceDuration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceDuration);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceDuration)) = num;
		}
	}

	public unsafe float PushDelay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushDelay);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushDelay)) = num;
		}
	}

	public unsafe AnimationCurve PushForceCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushForceCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe bool AirMovementEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementEnabled)) = flag;
		}
	}

	public unsafe float AirMovementForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementForce)) = num;
		}
	}

	public unsafe float AirMovementJumpReductionDuration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementJumpReductionDuration);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementJumpReductionDuration)) = num;
		}
	}

	public unsafe AnimationCurve AirMovementJumpReductionCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementJumpReductionCurve);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementJumpReductionCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float TopSpeed_Ms
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 294852, RefRangeEnd = 294854, XrefRangeStart = 294852, XrefRangeEnd = 294852, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TopSpeed_Ms_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkateboardSettings()
	{
		Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Experimental", "SkateboardSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr);
		NativeFieldInfoPtr_TurnForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "TurnForce");
		NativeFieldInfoPtr_TurnChangeRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "TurnChangeRate");
		NativeFieldInfoPtr_TurnReturnToRestRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "TurnReturnToRestRate");
		NativeFieldInfoPtr_TurnSpeedBoost = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "TurnSpeedBoost");
		NativeFieldInfoPtr_TurnForceMap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "TurnForceMap");
		NativeFieldInfoPtr_Gravity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "Gravity");
		NativeFieldInfoPtr_BrakeForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "BrakeForce");
		NativeFieldInfoPtr_ReverseTopSpeed_Kmh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "ReverseTopSpeed_Kmh");
		NativeFieldInfoPtr_RotationClampForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "RotationClampForce");
		NativeFieldInfoPtr_FrictionEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "FrictionEnabled");
		NativeFieldInfoPtr_LongitudinalFrictionCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "LongitudinalFrictionCurve");
		NativeFieldInfoPtr_LongitudinalFrictionMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "LongitudinalFrictionMultiplier");
		NativeFieldInfoPtr_LateralFrictionForceMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "LateralFrictionForceMultiplier");
		NativeFieldInfoPtr_JumpForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "JumpForce");
		NativeFieldInfoPtr_JumpDuration_Min = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "JumpDuration_Min");
		NativeFieldInfoPtr_JumpDuration_Max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "JumpDuration_Max");
		NativeFieldInfoPtr_FrontAxleJumpCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "FrontAxleJumpCurve");
		NativeFieldInfoPtr_RearAxleJumpCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "RearAxleJumpCurve");
		NativeFieldInfoPtr_JumpForwardForceCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "JumpForwardForceCurve");
		NativeFieldInfoPtr_JumpForwardBoost = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "JumpForwardBoost");
		NativeFieldInfoPtr_HoverForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "HoverForce");
		NativeFieldInfoPtr_HoverRayLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "HoverRayLength");
		NativeFieldInfoPtr_HoverHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "HoverHeight");
		NativeFieldInfoPtr_Hover_P = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "Hover_P");
		NativeFieldInfoPtr_Hover_I = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "Hover_I");
		NativeFieldInfoPtr_Hover_D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "Hover_D");
		NativeFieldInfoPtr_TopSpeed_Kmh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "TopSpeed_Kmh");
		NativeFieldInfoPtr_PushForceMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "PushForceMultiplier");
		NativeFieldInfoPtr_PushForceMultiplierMap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "PushForceMultiplierMap");
		NativeFieldInfoPtr_PushForceDuration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "PushForceDuration");
		NativeFieldInfoPtr_PushDelay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "PushDelay");
		NativeFieldInfoPtr_PushForceCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "PushForceCurve");
		NativeFieldInfoPtr_AirMovementEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "AirMovementEnabled");
		NativeFieldInfoPtr_AirMovementForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "AirMovementForce");
		NativeFieldInfoPtr_AirMovementJumpReductionDuration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "AirMovementJumpReductionDuration");
		NativeFieldInfoPtr_AirMovementJumpReductionCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, "AirMovementJumpReductionCurve");
		NativeMethodInfoPtr_get_TopSpeed_Ms_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, 100685961);
		NativeMethodInfoPtr_Clone_Public_SkateboardSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, 100685962);
		NativeMethodInfoPtr_Blend_Public_SkateboardSettings_SkateboardSettings_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, 100685963);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr, 100685964);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 294899, RefRangeEnd = 294901, XrefRangeStart = 294854, XrefRangeEnd = 294899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkateboardSettings Clone()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_SkateboardSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkateboardSettings>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 294981, RefRangeEnd = 294982, XrefRangeStart = 294901, XrefRangeEnd = 294981, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkateboardSettings Blend(SkateboardSettings other, float blendFactor)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &blendFactor;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Blend_Public_SkateboardSettings_SkateboardSettings_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkateboardSettings>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 294983, RefRangeEnd = 294984, XrefRangeStart = 294982, XrefRangeEnd = 294983, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkateboardSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkateboardSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkateboardSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
