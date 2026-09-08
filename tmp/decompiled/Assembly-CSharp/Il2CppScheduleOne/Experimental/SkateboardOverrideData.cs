using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Experimental;

public class SkateboardOverrideData : ScriptableObject
{
	[OriginalName("Assembly-CSharp.dll", "", "OverrideCategory")]
	[Flags]
	public enum OverrideCategory
	{
		None = 0,
		Turning = 1,
		General = 2,
		Friction = 4,
		Jump = 8,
		Hover = 0x10,
		Pushing = 0x20,
		AirMovement = 0x40
	}

	[OriginalName("Assembly-CSharp.dll", "", "TurningOverrides")]
	[Flags]
	public enum TurningOverrides
	{
		None = 0,
		TurnForce = 1,
		TurnChangeRate = 2,
		TurnReturnToRestRate = 4,
		TurnSpeedBoost = 8
	}

	[OriginalName("Assembly-CSharp.dll", "", "GeneralOverrides")]
	[Flags]
	public enum GeneralOverrides
	{
		None = 0,
		Gravity = 1,
		BrakeForce = 2,
		ReverseTopSpeed_Kmh = 4,
		RotationClampForce = 8
	}

	[OriginalName("Assembly-CSharp.dll", "", "FrictionOverrides")]
	[Flags]
	public enum FrictionOverrides
	{
		None = 0,
		LongitudinalFrictionMultiplier = 1,
		LateralFrictionForceMultiplier = 2
	}

	[OriginalName("Assembly-CSharp.dll", "", "JumpOverrides")]
	[Flags]
	public enum JumpOverrides
	{
		None = 0,
		JumpForce = 1,
		JumpDuration_Min = 2,
		JumpDuration_Max = 4,
		JumpForwardBoost = 8
	}

	[OriginalName("Assembly-CSharp.dll", "", "HoverOverrides")]
	[Flags]
	public enum HoverOverrides
	{
		None = 0,
		HoverForce = 1,
		HoverRayLength = 2,
		HoverHeight = 4,
		Hover_P = 8,
		Hover_I = 0x10,
		Hover_D = 0x20
	}

	[OriginalName("Assembly-CSharp.dll", "", "PushingOverrides")]
	[Flags]
	public enum PushingOverrides
	{
		None = 0,
		TopSpeed_Kmh = 1,
		PushForceMultiplier = 2,
		PushForceDuration = 4,
		PushDelay = 8
	}

	[OriginalName("Assembly-CSharp.dll", "", "AirMovementOverrides")]
	[Flags]
	public enum AirMovementOverrides
	{
		None = 0,
		AirMovementForce = 1,
		AirMovementJumpReductionDuration = 2
	}

	private static readonly IntPtr NativeFieldInfoPtr_Settings;

	private static readonly IntPtr NativeFieldInfoPtr_Categories;

	private static readonly IntPtr NativeFieldInfoPtr_TurningFlags;

	private static readonly IntPtr NativeFieldInfoPtr_GeneralFlags;

	private static readonly IntPtr NativeFieldInfoPtr_FrictionFlags;

	private static readonly IntPtr NativeFieldInfoPtr_JumpFlags;

	private static readonly IntPtr NativeFieldInfoPtr_HoverFlags;

	private static readonly IntPtr NativeFieldInfoPtr_PushingFlags;

	private static readonly IntPtr NativeFieldInfoPtr_AirMovementFlags;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkateboardSettings Settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Settings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkateboardSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skateboardSettings));
		}
	}

	public unsafe OverrideCategory Categories
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Categories);
			return *(OverrideCategory*)num;
		}
		set
		{
			*(OverrideCategory*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Categories)) = overrideCategory;
		}
	}

	public unsafe TurningOverrides TurningFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurningFlags);
			return *(TurningOverrides*)num;
		}
		set
		{
			*(TurningOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurningFlags)) = turningOverrides;
		}
	}

	public unsafe GeneralOverrides GeneralFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GeneralFlags);
			return *(GeneralOverrides*)num;
		}
		set
		{
			*(GeneralOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GeneralFlags)) = generalOverrides;
		}
	}

	public unsafe FrictionOverrides FrictionFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrictionFlags);
			return *(FrictionOverrides*)num;
		}
		set
		{
			*(FrictionOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrictionFlags)) = frictionOverrides;
		}
	}

	public unsafe JumpOverrides JumpFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpFlags);
			return *(JumpOverrides*)num;
		}
		set
		{
			*(JumpOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JumpFlags)) = jumpOverrides;
		}
	}

	public unsafe HoverOverrides HoverFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverFlags);
			return *(HoverOverrides*)num;
		}
		set
		{
			*(HoverOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoverFlags)) = hoverOverrides;
		}
	}

	public unsafe PushingOverrides PushingFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushingFlags);
			return *(PushingOverrides*)num;
		}
		set
		{
			*(PushingOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PushingFlags)) = pushingOverrides;
		}
	}

	public unsafe AirMovementOverrides AirMovementFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementFlags);
			return *(AirMovementOverrides*)num;
		}
		set
		{
			*(AirMovementOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AirMovementFlags)) = airMovementOverrides;
		}
	}

	static SkateboardOverrideData()
	{
		Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Experimental", "SkateboardOverrideData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr);
		NativeFieldInfoPtr_Settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "Settings");
		NativeFieldInfoPtr_Categories = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "Categories");
		NativeFieldInfoPtr_TurningFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "TurningFlags");
		NativeFieldInfoPtr_GeneralFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "GeneralFlags");
		NativeFieldInfoPtr_FrictionFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "FrictionFlags");
		NativeFieldInfoPtr_JumpFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "JumpFlags");
		NativeFieldInfoPtr_HoverFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "HoverFlags");
		NativeFieldInfoPtr_PushingFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "PushingFlags");
		NativeFieldInfoPtr_AirMovementFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, "AirMovementFlags");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr, 100685965);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkateboardOverrideData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkateboardOverrideData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkateboardOverrideData(IntPtr pointer)
		: base(pointer)
	{
	}
}
