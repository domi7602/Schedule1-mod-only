using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vehicles.AI;

[System.Serializable]
public class DriveFlags : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EObstacleMode")]
	public enum EObstacleMode
	{
		Default,
		IgnoreAll,
		IgnoreOnlySquishy
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_OverrideSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_OverriddenSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_OverriddenReverseSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_SpeedLimitMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_IgnoreTrafficLights;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseRoads;

	private static readonly System.IntPtr NativeFieldInfoPtr_StuckDetection;

	private static readonly System.IntPtr NativeFieldInfoPtr_ObstacleMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoBrakeAtDestination;

	private static readonly System.IntPtr NativeFieldInfoPtr_TurnBasedSpeedReduction;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetFlags_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool OverrideSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverrideSpeed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverrideSpeed)) = flag;
		}
	}

	public unsafe float OverriddenSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverriddenSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverriddenSpeed)) = num;
		}
	}

	public unsafe float OverriddenReverseSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverriddenReverseSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverriddenReverseSpeed)) = num;
		}
	}

	public unsafe float SpeedLimitMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpeedLimitMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpeedLimitMultiplier)) = num;
		}
	}

	public unsafe bool IgnoreTrafficLights
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IgnoreTrafficLights);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IgnoreTrafficLights)) = flag;
		}
	}

	public unsafe bool UseRoads
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseRoads);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseRoads)) = flag;
		}
	}

	public unsafe bool StuckDetection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckDetection);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckDetection)) = flag;
		}
	}

	public unsafe EObstacleMode ObstacleMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObstacleMode);
			return *(EObstacleMode*)num;
		}
		set
		{
			*(EObstacleMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObstacleMode)) = eObstacleMode;
		}
	}

	public unsafe bool AutoBrakeAtDestination
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoBrakeAtDestination);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoBrakeAtDestination)) = flag;
		}
	}

	public unsafe bool TurnBasedSpeedReduction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnBasedSpeedReduction);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnBasedSpeedReduction)) = flag;
		}
	}

	static DriveFlags()
	{
		Il2CppClassPointerStore<DriveFlags>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "DriveFlags");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr);
		NativeFieldInfoPtr_OverrideSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "OverrideSpeed");
		NativeFieldInfoPtr_OverriddenSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "OverriddenSpeed");
		NativeFieldInfoPtr_OverriddenReverseSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "OverriddenReverseSpeed");
		NativeFieldInfoPtr_SpeedLimitMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "SpeedLimitMultiplier");
		NativeFieldInfoPtr_IgnoreTrafficLights = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "IgnoreTrafficLights");
		NativeFieldInfoPtr_UseRoads = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "UseRoads");
		NativeFieldInfoPtr_StuckDetection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "StuckDetection");
		NativeFieldInfoPtr_ObstacleMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "ObstacleMode");
		NativeFieldInfoPtr_AutoBrakeAtDestination = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "AutoBrakeAtDestination");
		NativeFieldInfoPtr_TurnBasedSpeedReduction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, "TurnBasedSpeedReduction");
		NativeMethodInfoPtr_ResetFlags_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, 100666367);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr, 100666368);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 95290, RefRangeEnd = 95292, XrefRangeStart = 95290, XrefRangeEnd = 95290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetFlags()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetFlags_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95292, XrefRangeEnd = 95293, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DriveFlags()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DriveFlags>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DriveFlags(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
