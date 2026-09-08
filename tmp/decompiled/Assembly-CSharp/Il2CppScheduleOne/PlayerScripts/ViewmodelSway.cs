using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using UnityEngine;

namespace Il2CppScheduleOne.PlayerScripts;

public class ViewmodelSway : PlayerSingleton<ViewmodelSway>
{
	private static readonly IntPtr NativeFieldInfoPtr_DEBUG;

	private static readonly IntPtr NativeFieldInfoPtr_breatheBobbingEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_breathingHeightMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_breathingSpeedMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_lastHeight;

	private static readonly IntPtr NativeFieldInfoPtr_breatheBobPos;

	private static readonly IntPtr NativeFieldInfoPtr_swayingEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_horizontalSwayMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_verticalSwayMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_maxHorizontal;

	private static readonly IntPtr NativeFieldInfoPtr_maxVertical;

	private static readonly IntPtr NativeFieldInfoPtr_swaySmooth;

	private static readonly IntPtr NativeFieldInfoPtr_returnMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_initialPos;

	private static readonly IntPtr NativeFieldInfoPtr_swayPos;

	private static readonly IntPtr NativeFieldInfoPtr_walkBobbingEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_verticalMovement;

	private static readonly IntPtr NativeFieldInfoPtr_horizontalMovement;

	private static readonly IntPtr NativeFieldInfoPtr_verticalBobHeight;

	private static readonly IntPtr NativeFieldInfoPtr_verticalBobSpeed;

	private static readonly IntPtr NativeFieldInfoPtr_horizontalBobWidth;

	private static readonly IntPtr NativeFieldInfoPtr_horizontalBobSpeed;

	private static readonly IntPtr NativeFieldInfoPtr_walkBobSmooth;

	private static readonly IntPtr NativeFieldInfoPtr_sprintSpeedMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_walkBobMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_walkBobPos;

	private static readonly IntPtr NativeFieldInfoPtr_timeSinceWalkStart_vert;

	private static readonly IntPtr NativeFieldInfoPtr_timeSinceWalkStart_horiz;

	private static readonly IntPtr NativeFieldInfoPtr_jumpJoltEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_jumpCurve;

	private static readonly IntPtr NativeFieldInfoPtr_jumpJoltTime;

	private static readonly IntPtr NativeFieldInfoPtr_jumpJoltHeight;

	private static readonly IntPtr NativeFieldInfoPtr_jumpJoltSmooth;

	private static readonly IntPtr NativeFieldInfoPtr_equipBopVerticalOffset;

	private static readonly IntPtr NativeFieldInfoPtr_equipBopTime;

	private static readonly IntPtr NativeFieldInfoPtr_equipBopPos;

	private static readonly IntPtr NativeFieldInfoPtr_timeSinceJumpStart;

	private static readonly IntPtr NativeFieldInfoPtr_jumpPos;

	private static readonly IntPtr NativeFieldInfoPtr_fallOffsetRate;

	private static readonly IntPtr NativeFieldInfoPtr_maxFallOffsetAmount;

	private static readonly IntPtr NativeFieldInfoPtr_fallOffsetPos;

	private static readonly IntPtr NativeFieldInfoPtr_landCurve;

	private static readonly IntPtr NativeFieldInfoPtr_landJoltTime;

	private static readonly IntPtr NativeFieldInfoPtr_landJoltSmooth;

	private static readonly IntPtr NativeFieldInfoPtr_landPos;

	private static readonly IntPtr NativeFieldInfoPtr_timeSinceLanded;

	private static readonly IntPtr NativeFieldInfoPtr_landJoltMultiplier;

	private static readonly IntPtr NativeMethodInfoPtr_get_calculatedJumpJoltHeight_Protected_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnStartClient_Public_Virtual_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_InventoryStateChanged_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEquippedSlotChanged_Private_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_RefreshViewmodel_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_BreatheBob_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Sway_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_WalkBob_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartJump_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateJump_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Land_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe bool breatheBobbingEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breatheBobbingEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breatheBobbingEnabled)) = flag;
		}
	}

	public unsafe float breathingHeightMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breathingHeightMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breathingHeightMultiplier)) = num;
		}
	}

	public unsafe float breathingSpeedMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breathingSpeedMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breathingSpeedMultiplier)) = num;
		}
	}

	public unsafe float lastHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastHeight)) = num;
		}
	}

	public unsafe Vector3 breatheBobPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breatheBobPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_breatheBobPos)) = vector;
		}
	}

	public unsafe bool swayingEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_swayingEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_swayingEnabled)) = flag;
		}
	}

	public unsafe float horizontalSwayMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalSwayMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalSwayMultiplier)) = num;
		}
	}

	public unsafe float verticalSwayMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalSwayMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalSwayMultiplier)) = num;
		}
	}

	public unsafe float maxHorizontal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxHorizontal);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxHorizontal)) = num;
		}
	}

	public unsafe float maxVertical
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxVertical);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxVertical)) = num;
		}
	}

	public unsafe float swaySmooth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_swaySmooth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_swaySmooth)) = num;
		}
	}

	public unsafe float returnMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_returnMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_returnMultiplier)) = num;
		}
	}

	public unsafe Vector3 initialPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialPos)) = vector;
		}
	}

	public unsafe Vector3 swayPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_swayPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_swayPos)) = vector;
		}
	}

	public unsafe bool walkBobbingEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobbingEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobbingEnabled)) = flag;
		}
	}

	public unsafe AnimationCurve verticalMovement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalMovement);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalMovement)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe AnimationCurve horizontalMovement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalMovement);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalMovement)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float verticalBobHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalBobHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalBobHeight)) = num;
		}
	}

	public unsafe float verticalBobSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalBobSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_verticalBobSpeed)) = num;
		}
	}

	public unsafe float horizontalBobWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalBobWidth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalBobWidth)) = num;
		}
	}

	public unsafe float horizontalBobSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalBobSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_horizontalBobSpeed)) = num;
		}
	}

	public unsafe float walkBobSmooth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobSmooth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobSmooth)) = num;
		}
	}

	public unsafe float sprintSpeedMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sprintSpeedMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sprintSpeedMultiplier)) = num;
		}
	}

	public unsafe float walkBobMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobMultiplier)) = num;
		}
	}

	public unsafe Vector3 walkBobPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_walkBobPos)) = vector;
		}
	}

	public unsafe float timeSinceWalkStart_vert
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceWalkStart_vert);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceWalkStart_vert)) = num;
		}
	}

	public unsafe float timeSinceWalkStart_horiz
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceWalkStart_horiz);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceWalkStart_horiz)) = num;
		}
	}

	public unsafe bool jumpJoltEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltEnabled)) = flag;
		}
	}

	public unsafe AnimationCurve jumpCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float jumpJoltTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltTime)) = num;
		}
	}

	public unsafe float jumpJoltHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltHeight)) = num;
		}
	}

	public unsafe float jumpJoltSmooth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltSmooth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpJoltSmooth)) = num;
		}
	}

	public unsafe float equipBopVerticalOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equipBopVerticalOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equipBopVerticalOffset)) = num;
		}
	}

	public unsafe float equipBopTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equipBopTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equipBopTime)) = num;
		}
	}

	public unsafe Vector3 equipBopPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equipBopPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equipBopPos)) = vector;
		}
	}

	public unsafe float timeSinceJumpStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceJumpStart);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceJumpStart)) = num;
		}
	}

	public unsafe Vector3 jumpPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_jumpPos)) = vector;
		}
	}

	public unsafe float fallOffsetRate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fallOffsetRate);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fallOffsetRate)) = num;
		}
	}

	public unsafe float maxFallOffsetAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxFallOffsetAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxFallOffsetAmount)) = num;
		}
	}

	public unsafe Vector3 fallOffsetPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fallOffsetPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fallOffsetPos)) = vector;
		}
	}

	public unsafe AnimationCurve landCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float landJoltTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landJoltTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landJoltTime)) = num;
		}
	}

	public unsafe float landJoltSmooth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landJoltSmooth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landJoltSmooth)) = num;
		}
	}

	public unsafe Vector3 landPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landPos);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landPos)) = vector;
		}
	}

	public unsafe float timeSinceLanded
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceLanded);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceLanded)) = num;
		}
	}

	public unsafe float landJoltMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landJoltMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_landJoltMultiplier)) = num;
		}
	}

	public unsafe float calculatedJumpJoltHeight
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_calculatedJumpJoltHeight_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static ViewmodelSway()
	{
		Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerScripts", "ViewmodelSway");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr);
		NativeFieldInfoPtr_DEBUG = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "DEBUG");
		NativeFieldInfoPtr_breatheBobbingEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "breatheBobbingEnabled");
		NativeFieldInfoPtr_breathingHeightMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "breathingHeightMultiplier");
		NativeFieldInfoPtr_breathingSpeedMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "breathingSpeedMultiplier");
		NativeFieldInfoPtr_lastHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "lastHeight");
		NativeFieldInfoPtr_breatheBobPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "breatheBobPos");
		NativeFieldInfoPtr_swayingEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "swayingEnabled");
		NativeFieldInfoPtr_horizontalSwayMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "horizontalSwayMultiplier");
		NativeFieldInfoPtr_verticalSwayMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "verticalSwayMultiplier");
		NativeFieldInfoPtr_maxHorizontal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "maxHorizontal");
		NativeFieldInfoPtr_maxVertical = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "maxVertical");
		NativeFieldInfoPtr_swaySmooth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "swaySmooth");
		NativeFieldInfoPtr_returnMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "returnMultiplier");
		NativeFieldInfoPtr_initialPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "initialPos");
		NativeFieldInfoPtr_swayPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "swayPos");
		NativeFieldInfoPtr_walkBobbingEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "walkBobbingEnabled");
		NativeFieldInfoPtr_verticalMovement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "verticalMovement");
		NativeFieldInfoPtr_horizontalMovement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "horizontalMovement");
		NativeFieldInfoPtr_verticalBobHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "verticalBobHeight");
		NativeFieldInfoPtr_verticalBobSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "verticalBobSpeed");
		NativeFieldInfoPtr_horizontalBobWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "horizontalBobWidth");
		NativeFieldInfoPtr_horizontalBobSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "horizontalBobSpeed");
		NativeFieldInfoPtr_walkBobSmooth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "walkBobSmooth");
		NativeFieldInfoPtr_sprintSpeedMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "sprintSpeedMultiplier");
		NativeFieldInfoPtr_walkBobMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "walkBobMultiplier");
		NativeFieldInfoPtr_walkBobPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "walkBobPos");
		NativeFieldInfoPtr_timeSinceWalkStart_vert = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "timeSinceWalkStart_vert");
		NativeFieldInfoPtr_timeSinceWalkStart_horiz = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "timeSinceWalkStart_horiz");
		NativeFieldInfoPtr_jumpJoltEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "jumpJoltEnabled");
		NativeFieldInfoPtr_jumpCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "jumpCurve");
		NativeFieldInfoPtr_jumpJoltTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "jumpJoltTime");
		NativeFieldInfoPtr_jumpJoltHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "jumpJoltHeight");
		NativeFieldInfoPtr_jumpJoltSmooth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "jumpJoltSmooth");
		NativeFieldInfoPtr_equipBopVerticalOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "equipBopVerticalOffset");
		NativeFieldInfoPtr_equipBopTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "equipBopTime");
		NativeFieldInfoPtr_equipBopPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "equipBopPos");
		NativeFieldInfoPtr_timeSinceJumpStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "timeSinceJumpStart");
		NativeFieldInfoPtr_jumpPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "jumpPos");
		NativeFieldInfoPtr_fallOffsetRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "fallOffsetRate");
		NativeFieldInfoPtr_maxFallOffsetAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "maxFallOffsetAmount");
		NativeFieldInfoPtr_fallOffsetPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "fallOffsetPos");
		NativeFieldInfoPtr_landCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "landCurve");
		NativeFieldInfoPtr_landJoltTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "landJoltTime");
		NativeFieldInfoPtr_landJoltSmooth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "landJoltSmooth");
		NativeFieldInfoPtr_landPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "landPos");
		NativeFieldInfoPtr_timeSinceLanded = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "timeSinceLanded");
		NativeFieldInfoPtr_landJoltMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, "landJoltMultiplier");
		NativeMethodInfoPtr_get_calculatedJumpJoltHeight_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672250);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672251);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672252);
		NativeMethodInfoPtr_OnStartClient_Public_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672253);
		NativeMethodInfoPtr_Update_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672254);
		NativeMethodInfoPtr_InventoryStateChanged_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672255);
		NativeMethodInfoPtr_OnEquippedSlotChanged_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672256);
		NativeMethodInfoPtr_RefreshViewmodel_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672257);
		NativeMethodInfoPtr_BreatheBob_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672258);
		NativeMethodInfoPtr_Sway_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672259);
		NativeMethodInfoPtr_WalkBob_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672260);
		NativeMethodInfoPtr_StartJump_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672261);
		NativeMethodInfoPtr_UpdateJump_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672262);
		NativeMethodInfoPtr_Land_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672263);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr, 100672264);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164444, XrefRangeEnd = 164447, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164447, XrefRangeEnd = 164452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164452, XrefRangeEnd = 164525, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnStartClient(bool IsOwner)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&IsOwner);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnStartClient_Public_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 164553, RefRangeEnd = 164555, XrefRangeStart = 164525, XrefRangeEnd = 164553, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164555, XrefRangeEnd = 164556, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InventoryStateChanged(bool active)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&active);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InventoryStateChanged_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164556, XrefRangeEnd = 164557, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEquippedSlotChanged(int slotIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&slotIndex);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEquippedSlotChanged_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 164615, RefRangeEnd = 164617, XrefRangeStart = 164557, XrefRangeEnd = 164615, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RefreshViewmodel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RefreshViewmodel_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164617, XrefRangeEnd = 164619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BreatheBob()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BreatheBob_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 164653, RefRangeEnd = 164654, XrefRangeStart = 164619, XrefRangeEnd = 164653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Sway()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Sway_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 164683, RefRangeEnd = 164684, XrefRangeStart = 164654, XrefRangeEnd = 164683, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void WalkBob()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WalkBob_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void StartJump()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartJump_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 164721, RefRangeEnd = 164722, XrefRangeStart = 164684, XrefRangeEnd = 164721, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateJump()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateJump_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164722, XrefRangeEnd = 164724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Land()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Land_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164724, XrefRangeEnd = 164735, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ViewmodelSway()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ViewmodelSway>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ViewmodelSway(IntPtr pointer)
		: base(pointer)
	{
	}
}
