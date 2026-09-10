using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Skating;

public class SkateboardAnimation : MonoBehaviour
{
	[System.Serializable]
	public class AlignmentSet : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Transform;

		private static readonly System.IntPtr NativeFieldInfoPtr_Default;

		private static readonly System.IntPtr NativeFieldInfoPtr_Animated;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Transform Transform
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Transform);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Transform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
			}
		}

		public unsafe Transform Default
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Default);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Default)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
			}
		}

		public unsafe Transform Animated
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Animated);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Animated)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
			}
		}

		static AlignmentSet()
		{
			Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "AlignmentSet");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr);
			NativeFieldInfoPtr_Transform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr, "Transform");
			NativeFieldInfoPtr_Default = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr, "Default");
			NativeFieldInfoPtr_Animated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr, "Animated");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr, 100667216);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe AlignmentSet()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AlignmentSet>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public AlignmentSet(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_JumpCrouchAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr_CrouchSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_ArmLiftRate;

	private static readonly System.IntPtr NativeFieldInfoPtr_PelvisMaxRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_HandsMaxRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_PelvisOffsetBlend;

	private static readonly System.IntPtr NativeFieldInfoPtr_VerticalMomentumMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_VerticalMomentumOffsetClamp;

	private static readonly System.IntPtr NativeFieldInfoPtr_MomentumMoveSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_IKBlendChangeRate;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushAnimationDuration;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushAnimationSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushAnim;

	private static readonly System.IntPtr NativeFieldInfoPtr_PelvisContainerAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_PelvisAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_SpineContainerAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_SpineAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_SpineAlignment_Hunched;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeftFootAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_RightFootAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeftLegBendTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr_RightLegBendTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeftHandAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_RightHandAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_AvatarFaceTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr_HandContainer;

	private static readonly System.IntPtr NativeFieldInfoPtr_IKAnimation;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeftHandLoweredAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeftHandRaisedAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_RightHandLoweredAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_RightHandRaisedAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_board;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentCrouchShift;

	private static readonly System.IntPtr NativeFieldInfoPtr_targetArmLift;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentArmLift;

	private static readonly System.IntPtr NativeFieldInfoPtr_pelvisDefaultRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_pelvisDefaultPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_spineDefaultPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentMomentumOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_ikBlend;

	private static readonly System.IntPtr NativeFieldInfoPtr_alignmentSets;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentCrouchShift_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateIKBlend_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateBodyAlignment_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateArmLift_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdatePelvisRotation_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetArmLift_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnPushStart_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float JumpCrouchAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpCrouchAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpCrouchAmount)) = num;
		}
	}

	public unsafe float CrouchSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CrouchSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CrouchSpeed)) = num;
		}
	}

	public unsafe float ArmLiftRate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ArmLiftRate);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ArmLiftRate)) = num;
		}
	}

	public unsafe float PelvisMaxRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisMaxRotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisMaxRotation)) = num;
		}
	}

	public unsafe float HandsMaxRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandsMaxRotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandsMaxRotation)) = num;
		}
	}

	public unsafe float PelvisOffsetBlend
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisOffsetBlend);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisOffsetBlend)) = num;
		}
	}

	public unsafe float VerticalMomentumMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VerticalMomentumMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VerticalMomentumMultiplier)) = num;
		}
	}

	public unsafe float VerticalMomentumOffsetClamp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VerticalMomentumOffsetClamp);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VerticalMomentumOffsetClamp)) = num;
		}
	}

	public unsafe float MomentumMoveSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MomentumMoveSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MomentumMoveSpeed)) = num;
		}
	}

	public unsafe float IKBlendChangeRate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IKBlendChangeRate);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IKBlendChangeRate)) = num;
		}
	}

	public unsafe float PushAnimationDuration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushAnimationDuration);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushAnimationDuration)) = num;
		}
	}

	public unsafe float PushAnimationSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushAnimationSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushAnimationSpeed)) = num;
		}
	}

	public unsafe AnimationClip PushAnim
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushAnim);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationClip>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushAnim)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationClip));
		}
	}

	public unsafe AlignmentSet PelvisContainerAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisContainerAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisContainerAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet PelvisAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelvisAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet SpineContainerAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpineContainerAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpineContainerAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet SpineAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpineAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpineAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe Transform SpineAlignment_Hunched
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpineAlignment_Hunched);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpineAlignment_Hunched)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe AlignmentSet LeftFootAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftFootAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftFootAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet RightFootAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightFootAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightFootAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet LeftLegBendTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftLegBendTarget);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftLegBendTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet RightLegBendTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightLegBendTarget);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightLegBendTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet LeftHandAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftHandAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftHandAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet RightHandAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightHandAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightHandAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe Transform AvatarFaceTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvatarFaceTarget);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvatarFaceTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform HandContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandContainer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Animation IKAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IKAnimation);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IKAnimation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
		}
	}

	public unsafe AlignmentSet LeftHandLoweredAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftHandLoweredAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftHandLoweredAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet LeftHandRaisedAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftHandRaisedAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftHandRaisedAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet RightHandLoweredAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightHandLoweredAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightHandLoweredAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe AlignmentSet RightHandRaisedAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightHandRaisedAlignment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AlignmentSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightHandRaisedAlignment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)alignmentSet));
		}
	}

	public unsafe Skateboard board
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_board);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skateboard>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_board)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skateboard));
		}
	}

	public unsafe float currentCrouchShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentCrouchShift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentCrouchShift)) = num;
		}
	}

	public unsafe float targetArmLift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetArmLift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetArmLift)) = num;
		}
	}

	public unsafe float currentArmLift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentArmLift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentArmLift)) = num;
		}
	}

	public unsafe Quaternion pelvisDefaultRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pelvisDefaultRotation);
			return *(Quaternion*)num;
		}
		set
		{
			*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pelvisDefaultRotation)) = quaternion;
		}
	}

	public unsafe Vector3 pelvisDefaultPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pelvisDefaultPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pelvisDefaultPosition)) = vector;
		}
	}

	public unsafe Vector3 spineDefaultPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spineDefaultPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spineDefaultPosition)) = vector;
		}
	}

	public unsafe float currentMomentumOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentMomentumOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentMomentumOffset)) = num;
		}
	}

	public unsafe float ikBlend
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ikBlend);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ikBlend)) = num;
		}
	}

	public unsafe List<AlignmentSet> alignmentSets
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alignmentSets);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AlignmentSet>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alignmentSets)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe float CurrentCrouchShift
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentCrouchShift_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkateboardAnimation()
	{
		Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Skating", "SkateboardAnimation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr);
		NativeFieldInfoPtr_JumpCrouchAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "JumpCrouchAmount");
		NativeFieldInfoPtr_CrouchSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "CrouchSpeed");
		NativeFieldInfoPtr_ArmLiftRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "ArmLiftRate");
		NativeFieldInfoPtr_PelvisMaxRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PelvisMaxRotation");
		NativeFieldInfoPtr_HandsMaxRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "HandsMaxRotation");
		NativeFieldInfoPtr_PelvisOffsetBlend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PelvisOffsetBlend");
		NativeFieldInfoPtr_VerticalMomentumMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "VerticalMomentumMultiplier");
		NativeFieldInfoPtr_VerticalMomentumOffsetClamp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "VerticalMomentumOffsetClamp");
		NativeFieldInfoPtr_MomentumMoveSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "MomentumMoveSpeed");
		NativeFieldInfoPtr_IKBlendChangeRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "IKBlendChangeRate");
		NativeFieldInfoPtr_PushAnimationDuration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PushAnimationDuration");
		NativeFieldInfoPtr_PushAnimationSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PushAnimationSpeed");
		NativeFieldInfoPtr_PushAnim = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PushAnim");
		NativeFieldInfoPtr_PelvisContainerAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PelvisContainerAlignment");
		NativeFieldInfoPtr_PelvisAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "PelvisAlignment");
		NativeFieldInfoPtr_SpineContainerAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "SpineContainerAlignment");
		NativeFieldInfoPtr_SpineAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "SpineAlignment");
		NativeFieldInfoPtr_SpineAlignment_Hunched = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "SpineAlignment_Hunched");
		NativeFieldInfoPtr_LeftFootAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "LeftFootAlignment");
		NativeFieldInfoPtr_RightFootAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "RightFootAlignment");
		NativeFieldInfoPtr_LeftLegBendTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "LeftLegBendTarget");
		NativeFieldInfoPtr_RightLegBendTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "RightLegBendTarget");
		NativeFieldInfoPtr_LeftHandAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "LeftHandAlignment");
		NativeFieldInfoPtr_RightHandAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "RightHandAlignment");
		NativeFieldInfoPtr_AvatarFaceTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "AvatarFaceTarget");
		NativeFieldInfoPtr_HandContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "HandContainer");
		NativeFieldInfoPtr_IKAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "IKAnimation");
		NativeFieldInfoPtr_LeftHandLoweredAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "LeftHandLoweredAlignment");
		NativeFieldInfoPtr_LeftHandRaisedAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "LeftHandRaisedAlignment");
		NativeFieldInfoPtr_RightHandLoweredAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "RightHandLoweredAlignment");
		NativeFieldInfoPtr_RightHandRaisedAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "RightHandRaisedAlignment");
		NativeFieldInfoPtr_board = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "board");
		NativeFieldInfoPtr_currentCrouchShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "currentCrouchShift");
		NativeFieldInfoPtr_targetArmLift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "targetArmLift");
		NativeFieldInfoPtr_currentArmLift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "currentArmLift");
		NativeFieldInfoPtr_pelvisDefaultRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "pelvisDefaultRotation");
		NativeFieldInfoPtr_pelvisDefaultPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "pelvisDefaultPosition");
		NativeFieldInfoPtr_spineDefaultPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "spineDefaultPosition");
		NativeFieldInfoPtr_currentMomentumOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "currentMomentumOffset");
		NativeFieldInfoPtr_ikBlend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "ikBlend");
		NativeFieldInfoPtr_alignmentSets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, "alignmentSets");
		NativeMethodInfoPtr_get_CurrentCrouchShift_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667204);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667205);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667206);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667207);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667208);
		NativeMethodInfoPtr_UpdateIKBlend_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667209);
		NativeMethodInfoPtr_UpdateBodyAlignment_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667210);
		NativeMethodInfoPtr_UpdateArmLift_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667211);
		NativeMethodInfoPtr_UpdatePelvisRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667212);
		NativeMethodInfoPtr_SetArmLift_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667213);
		NativeMethodInfoPtr_OnPushStart_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667214);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr, 100667215);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 104581, XrefRangeEnd = 104653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 104653, XrefRangeEnd = 104654, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 104654, XrefRangeEnd = 104657, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 104685, RefRangeEnd = 104686, XrefRangeStart = 104657, XrefRangeEnd = 104685, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateIKBlend()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateIKBlend_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 104703, RefRangeEnd = 104704, XrefRangeStart = 104686, XrefRangeEnd = 104703, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateBodyAlignment()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateBodyAlignment_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 104728, RefRangeEnd = 104729, XrefRangeStart = 104704, XrefRangeEnd = 104728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateArmLift()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateArmLift_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 104741, RefRangeEnd = 104742, XrefRangeStart = 104729, XrefRangeEnd = 104741, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdatePelvisRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdatePelvisRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void SetArmLift(float lift)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&lift);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetArmLift_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 104742, XrefRangeEnd = 104749, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnPushStart()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnPushStart_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 104749, XrefRangeEnd = 104757, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkateboardAnimation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkateboardAnimation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkateboardAnimation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
