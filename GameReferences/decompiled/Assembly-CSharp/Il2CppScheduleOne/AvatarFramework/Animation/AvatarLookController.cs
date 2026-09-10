using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppRootMotion.FinalIK;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.PlayerScripts;
using Unity.Profiling;
using UnityEngine;

namespace Il2CppScheduleOne.AvatarFramework.Animation;

public class AvatarLookController : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_CullRange;

	private static readonly IntPtr NativeFieldInfoPtr_CullRangeSqr;

	private static readonly IntPtr NativeFieldInfoPtr_LookAtPlayerRange;

	private static readonly IntPtr NativeFieldInfoPtr_EyeContractRange;

	private static readonly IntPtr NativeFieldInfoPtr_TempContainer;

	private static readonly IntPtr NativeFieldInfoPtr_DEBUG;

	private static readonly IntPtr NativeFieldInfoPtr__BodyRotationSpeedMultiplier_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_Aim;

	private static readonly IntPtr NativeFieldInfoPtr_HeadBone;

	private static readonly IntPtr NativeFieldInfoPtr_LookForwardTarget;

	private static readonly IntPtr NativeFieldInfoPtr_LookOrigin;

	private static readonly IntPtr NativeFieldInfoPtr_Eyes;

	private static readonly IntPtr NativeFieldInfoPtr_AutoLookAtPlayer;

	private static readonly IntPtr NativeFieldInfoPtr_LookLerpSpeed;

	private static readonly IntPtr NativeFieldInfoPtr_AimIKWeight;

	private static readonly IntPtr NativeFieldInfoPtr_BodyRotationSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__parentNPC;

	private static readonly IntPtr NativeFieldInfoPtr__parentPlayer;

	private static readonly IntPtr NativeFieldInfoPtr_avatar;

	private static readonly IntPtr NativeFieldInfoPtr_lookAtPos;

	private static readonly IntPtr NativeFieldInfoPtr_lookAtTarget;

	private static readonly IntPtr NativeFieldInfoPtr_lastFrameOffset;

	private static readonly IntPtr NativeFieldInfoPtr_overrideLookAt;

	private static readonly IntPtr NativeFieldInfoPtr_overriddenLookTarget;

	private static readonly IntPtr NativeFieldInfoPtr_overrideLookPriority;

	private static readonly IntPtr NativeFieldInfoPtr_overrideRotateBody;

	private static readonly IntPtr NativeFieldInfoPtr_blockLookOverrides;

	private static readonly IntPtr NativeFieldInfoPtr_ForceLookTarget;

	private static readonly IntPtr NativeFieldInfoPtr_ForceLookRotateBody;

	private static readonly IntPtr NativeFieldInfoPtr_defaultIKWeight;

	private static readonly IntPtr NativeFieldInfoPtr_nearestPlayer;

	private static readonly IntPtr NativeFieldInfoPtr_nearestPlayerDist;

	private static readonly IntPtr NativeFieldInfoPtr_localPlayerSqrDist;

	private static readonly IntPtr NativeFieldInfoPtr_updateLookMarker;

	private static readonly IntPtr NativeFieldInfoPtr_lerpTargetMarker;

	private static readonly IntPtr NativeFieldInfoPtr_eyeLookAtMarker;

	private static readonly IntPtr NativeMethodInfoPtr_get_BodyRotationSpeedMultiplier_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_BodyRotationSpeedMultiplier_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateLook_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateNearestPlayer_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OverrideLookTarget_Public_Void_Vector3_Int32_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_BlockLookTargetOverrides_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LookForward_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LerpTargetTransform_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CanLookAt_Private_Boolean_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_RagdollChange_Protected_Void_Boolean_Boolean_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OverrideIKWeight_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ResetIKWeight_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float CullRange
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullRange, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullRange, (void*)(&num));
		}
	}

	public unsafe static float CullRangeSqr
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullRangeSqr, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullRangeSqr, (void*)(&num));
		}
	}

	public unsafe static float LookAtPlayerRange
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LookAtPlayerRange, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LookAtPlayerRange, (void*)(&num));
		}
	}

	public unsafe static float EyeContractRange
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EyeContractRange, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EyeContractRange, (void*)(&num));
		}
	}

	public unsafe static Transform TempContainer
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TempContainer, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TempContainer, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool DEBUG
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DEBUG);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DEBUG)) = flag;
		}
	}

	public unsafe float _BodyRotationSpeedMultiplier_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BodyRotationSpeedMultiplier_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BodyRotationSpeedMultiplier_k__BackingField)) = num;
		}
	}

	public unsafe AimIK Aim
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Aim);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AimIK>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Aim)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)aimIK));
		}
	}

	public unsafe Transform HeadBone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HeadBone);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HeadBone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform LookForwardTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LookForwardTarget);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LookForwardTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform LookOrigin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LookOrigin);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LookOrigin)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe EyeController Eyes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Eyes);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EyeController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Eyes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eyeController));
		}
	}

	public unsafe bool AutoLookAtPlayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoLookAtPlayer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoLookAtPlayer)) = flag;
		}
	}

	public unsafe float LookLerpSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LookLerpSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LookLerpSpeed)) = num;
		}
	}

	public unsafe float AimIKWeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AimIKWeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AimIKWeight)) = num;
		}
	}

	public unsafe float BodyRotationSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BodyRotationSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BodyRotationSpeed)) = num;
		}
	}

	public unsafe NPC _parentNPC
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parentNPC);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NPC>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parentNPC)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nPC));
		}
	}

	public unsafe Player _parentPlayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parentPlayer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Player>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parentPlayer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)player));
		}
	}

	public unsafe Avatar avatar
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_avatar);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Avatar>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_avatar)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatar));
		}
	}

	public unsafe Vector3 lookAtPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lookAtPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lookAtPos)) = vector;
		}
	}

	public unsafe Transform lookAtTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lookAtTarget);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lookAtTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Vector3 lastFrameOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastFrameOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastFrameOffset)) = vector;
		}
	}

	public unsafe bool overrideLookAt
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideLookAt);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideLookAt)) = flag;
		}
	}

	public unsafe Vector3 overriddenLookTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overriddenLookTarget);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overriddenLookTarget)) = vector;
		}
	}

	public unsafe int overrideLookPriority
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideLookPriority);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideLookPriority)) = num;
		}
	}

	public unsafe bool overrideRotateBody
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideRotateBody);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideRotateBody)) = flag;
		}
	}

	public unsafe bool blockLookOverrides
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blockLookOverrides);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blockLookOverrides)) = flag;
		}
	}

	public unsafe Transform ForceLookTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForceLookTarget);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForceLookTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool ForceLookRotateBody
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForceLookRotateBody);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForceLookRotateBody)) = flag;
		}
	}

	public unsafe float defaultIKWeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultIKWeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultIKWeight)) = num;
		}
	}

	public unsafe Player nearestPlayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nearestPlayer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Player>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nearestPlayer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)player));
		}
	}

	public unsafe float nearestPlayerDist
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nearestPlayerDist);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nearestPlayerDist)) = num;
		}
	}

	public unsafe float localPlayerSqrDist
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_localPlayerSqrDist);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_localPlayerSqrDist)) = num;
		}
	}

	public unsafe static ProfilerMarker updateLookMarker
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_updateLookMarker, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_updateLookMarker, (void*)(&profilerMarker));
		}
	}

	public unsafe static ProfilerMarker lerpTargetMarker
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lerpTargetMarker, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lerpTargetMarker, (void*)(&profilerMarker));
		}
	}

	public unsafe static ProfilerMarker eyeLookAtMarker
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_eyeLookAtMarker, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_eyeLookAtMarker, (void*)(&profilerMarker));
		}
	}

	public unsafe float BodyRotationSpeedMultiplier
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BodyRotationSpeedMultiplier_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(14)]
		[CachedScanResults(RefRangeStart = 29058, RefRangeEnd = 29072, XrefRangeStart = 29058, XrefRangeEnd = 29072, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BodyRotationSpeedMultiplier_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static AvatarLookController()
	{
		Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Animation", "AvatarLookController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr);
		NativeFieldInfoPtr_CullRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "CullRange");
		NativeFieldInfoPtr_CullRangeSqr = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "CullRangeSqr");
		NativeFieldInfoPtr_LookAtPlayerRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "LookAtPlayerRange");
		NativeFieldInfoPtr_EyeContractRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "EyeContractRange");
		NativeFieldInfoPtr_TempContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "TempContainer");
		NativeFieldInfoPtr_DEBUG = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "DEBUG");
		NativeFieldInfoPtr__BodyRotationSpeedMultiplier_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "<BodyRotationSpeedMultiplier>k__BackingField");
		NativeFieldInfoPtr_Aim = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "Aim");
		NativeFieldInfoPtr_HeadBone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "HeadBone");
		NativeFieldInfoPtr_LookForwardTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "LookForwardTarget");
		NativeFieldInfoPtr_LookOrigin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "LookOrigin");
		NativeFieldInfoPtr_Eyes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "Eyes");
		NativeFieldInfoPtr_AutoLookAtPlayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "AutoLookAtPlayer");
		NativeFieldInfoPtr_LookLerpSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "LookLerpSpeed");
		NativeFieldInfoPtr_AimIKWeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "AimIKWeight");
		NativeFieldInfoPtr_BodyRotationSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "BodyRotationSpeed");
		NativeFieldInfoPtr__parentNPC = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "_parentNPC");
		NativeFieldInfoPtr__parentPlayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "_parentPlayer");
		NativeFieldInfoPtr_avatar = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "avatar");
		NativeFieldInfoPtr_lookAtPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "lookAtPos");
		NativeFieldInfoPtr_lookAtTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "lookAtTarget");
		NativeFieldInfoPtr_lastFrameOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "lastFrameOffset");
		NativeFieldInfoPtr_overrideLookAt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "overrideLookAt");
		NativeFieldInfoPtr_overriddenLookTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "overriddenLookTarget");
		NativeFieldInfoPtr_overrideLookPriority = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "overrideLookPriority");
		NativeFieldInfoPtr_overrideRotateBody = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "overrideRotateBody");
		NativeFieldInfoPtr_blockLookOverrides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "blockLookOverrides");
		NativeFieldInfoPtr_ForceLookTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "ForceLookTarget");
		NativeFieldInfoPtr_ForceLookRotateBody = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "ForceLookRotateBody");
		NativeFieldInfoPtr_defaultIKWeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "defaultIKWeight");
		NativeFieldInfoPtr_nearestPlayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "nearestPlayer");
		NativeFieldInfoPtr_nearestPlayerDist = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "nearestPlayerDist");
		NativeFieldInfoPtr_localPlayerSqrDist = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "localPlayerSqrDist");
		NativeFieldInfoPtr_updateLookMarker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "updateLookMarker");
		NativeFieldInfoPtr_lerpTargetMarker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "lerpTargetMarker");
		NativeFieldInfoPtr_eyeLookAtMarker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, "eyeLookAtMarker");
		NativeMethodInfoPtr_get_BodyRotationSpeedMultiplier_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677718);
		NativeMethodInfoPtr_set_BodyRotationSpeedMultiplier_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677719);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677720);
		NativeMethodInfoPtr_UpdateLook_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677721);
		NativeMethodInfoPtr_UpdateNearestPlayer_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677722);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677723);
		NativeMethodInfoPtr_OverrideLookTarget_Public_Void_Vector3_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677724);
		NativeMethodInfoPtr_BlockLookTargetOverrides_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677725);
		NativeMethodInfoPtr_LookForward_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677726);
		NativeMethodInfoPtr_LerpTargetTransform_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677727);
		NativeMethodInfoPtr_CanLookAt_Private_Boolean_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677728);
		NativeMethodInfoPtr_RagdollChange_Protected_Void_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677729);
		NativeMethodInfoPtr_OverrideIKWeight_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677730);
		NativeMethodInfoPtr_ResetIKWeight_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677731);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr, 100677732);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 223495, XrefRangeEnd = 223560, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 223583, RefRangeEnd = 223584, XrefRangeStart = 223560, XrefRangeEnd = 223583, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateLook()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateLook_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 223584, XrefRangeEnd = 223602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateNearestPlayer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateNearestPlayer_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 223602, XrefRangeEnd = 223652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 223669, RefRangeEnd = 223685, XrefRangeStart = 223652, XrefRangeEnd = 223669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OverrideLookTarget(Vector3 targetPosition, int priority, bool rotateBody = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&targetPosition);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &priority;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &rotateBody;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OverrideLookTarget_Public_Void_Vector3_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 223685, RefRangeEnd = 223686, XrefRangeStart = 223685, XrefRangeEnd = 223685, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BlockLookTargetOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BlockLookTargetOverrides_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 223697, RefRangeEnd = 223698, XrefRangeStart = 223686, XrefRangeEnd = 223697, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LookForward()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LookForward_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 223716, RefRangeEnd = 223717, XrefRangeStart = 223698, XrefRangeEnd = 223716, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LerpTargetTransform()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LerpTargetTransform_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 223721, RefRangeEnd = 223722, XrefRangeStart = 223717, XrefRangeEnd = 223721, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CanLookAt(Vector3 position)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&position);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanLookAt_Private_Boolean_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RagdollChange(bool oldValue, bool ragdoll, bool playStandUpAnim)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&oldValue);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &ragdoll;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &playStandUpAnim;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RagdollChange_Protected_Void_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 223724, RefRangeEnd = 223727, XrefRangeStart = 223722, XrefRangeEnd = 223724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OverrideIKWeight(float weight)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&weight);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OverrideIKWeight_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 223729, RefRangeEnd = 223730, XrefRangeStart = 223727, XrefRangeEnd = 223729, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetIKWeight()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetIKWeight_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 223730, XrefRangeEnd = 223737, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AvatarLookController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AvatarLookController>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AvatarLookController(IntPtr pointer)
		: base(pointer)
	{
	}
}
