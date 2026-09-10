using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.State;
using UnityEngine;

namespace Il2CppScheduleOne.Skating;

public class Skateboard_Equippable : Equippable_Viewmodel
{
	private static readonly IntPtr NativeFieldInfoPtr_ModelLerpSpeed;

	private static readonly IntPtr NativeFieldInfoPtr_SurfaceSampleDistance;

	private static readonly IntPtr NativeFieldInfoPtr_SurfaceSampleRayLength;

	private static readonly IntPtr NativeFieldInfoPtr_BoardSpawnUpwardsShift;

	private static readonly IntPtr NativeFieldInfoPtr_BoardSpawnAngleLimit;

	private static readonly IntPtr NativeFieldInfoPtr_MountTime;

	private static readonly IntPtr NativeFieldInfoPtr_BoardMomentumTransfer;

	private static readonly IntPtr NativeFieldInfoPtr_DismountAngle;

	private static readonly IntPtr NativeFieldInfoPtr__IsRiding_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__ActiveSkateboard_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_SkateboardPrefab;

	private static readonly IntPtr NativeFieldInfoPtr_blockDismount;

	private static readonly IntPtr NativeFieldInfoPtr_ModelContainer;

	private static readonly IntPtr NativeFieldInfoPtr_ModelPosition_Raised;

	private static readonly IntPtr NativeFieldInfoPtr_ModelPosition_Lowered;

	private static readonly IntPtr NativeFieldInfoPtr_mountTime;

	private static readonly IntPtr NativeFieldInfoPtr__state;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsRiding_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IsRiding_Private_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ActiveSkateboard_Public_get_Skateboard_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ActiveSkateboard_Private_set_Void_Skateboard_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_State_Public_get_MonoState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_Exit_Private_Void_ExitAction_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateModel_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Mount_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Dismount_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnMount_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDismount_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CanMountHere_Private_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetSkateboardSpawnPose_Private_Pose_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float ModelLerpSpeed
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ModelLerpSpeed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ModelLerpSpeed, (void*)(&num));
		}
	}

	public unsafe static float SurfaceSampleDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SurfaceSampleDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SurfaceSampleDistance, (void*)(&num));
		}
	}

	public unsafe static float SurfaceSampleRayLength
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SurfaceSampleRayLength, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SurfaceSampleRayLength, (void*)(&num));
		}
	}

	public unsafe static float BoardSpawnUpwardsShift
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BoardSpawnUpwardsShift, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BoardSpawnUpwardsShift, (void*)(&num));
		}
	}

	public unsafe static float BoardSpawnAngleLimit
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BoardSpawnAngleLimit, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BoardSpawnAngleLimit, (void*)(&num));
		}
	}

	public unsafe static float MountTime
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MountTime, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MountTime, (void*)(&num));
		}
	}

	public unsafe static float BoardMomentumTransfer
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BoardMomentumTransfer, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BoardMomentumTransfer, (void*)(&num));
		}
	}

	public unsafe static float DismountAngle
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DismountAngle, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DismountAngle, (void*)(&num));
		}
	}

	public unsafe bool _IsRiding_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsRiding_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsRiding_k__BackingField)) = flag;
		}
	}

	public unsafe Skateboard _ActiveSkateboard_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ActiveSkateboard_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Skateboard>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ActiveSkateboard_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skateboard));
		}
	}

	public unsafe Skateboard SkateboardPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SkateboardPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Skateboard>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SkateboardPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skateboard));
		}
	}

	public unsafe bool blockDismount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blockDismount);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blockDismount)) = flag;
		}
	}

	public unsafe Transform ModelContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModelContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModelContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform ModelPosition_Raised
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModelPosition_Raised);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModelPosition_Raised)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform ModelPosition_Lowered
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModelPosition_Lowered);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModelPosition_Lowered)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe float mountTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mountTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mountTime)) = num;
		}
	}

	public unsafe MonoState _state
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__state);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MonoState>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__state)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)monoState));
		}
	}

	public unsafe bool IsRiding
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsRiding_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsRiding_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Skateboard ActiveSkateboard
	{
		[CallerCount(16)]
		[CachedScanResults(RefRangeStart = 21999, RefRangeEnd = 22015, XrefRangeStart = 21999, XrefRangeEnd = 22015, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ActiveSkateboard_Public_get_Skateboard_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Skateboard>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105299, XrefRangeEnd = 105300, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ActiveSkateboard_Private_set_Void_Skateboard_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe MonoState State
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38414, RefRangeEnd = 38415, XrefRangeStart = 38414, XrefRangeEnd = 38415, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_State_Public_get_MonoState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MonoState>(intPtr) : null;
		}
	}

	static Skateboard_Equippable()
	{
		Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Skating", "Skateboard_Equippable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr);
		NativeFieldInfoPtr_ModelLerpSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "ModelLerpSpeed");
		NativeFieldInfoPtr_SurfaceSampleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "SurfaceSampleDistance");
		NativeFieldInfoPtr_SurfaceSampleRayLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "SurfaceSampleRayLength");
		NativeFieldInfoPtr_BoardSpawnUpwardsShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "BoardSpawnUpwardsShift");
		NativeFieldInfoPtr_BoardSpawnAngleLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "BoardSpawnAngleLimit");
		NativeFieldInfoPtr_MountTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "MountTime");
		NativeFieldInfoPtr_BoardMomentumTransfer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "BoardMomentumTransfer");
		NativeFieldInfoPtr_DismountAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "DismountAngle");
		NativeFieldInfoPtr__IsRiding_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "<IsRiding>k__BackingField");
		NativeFieldInfoPtr__ActiveSkateboard_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "<ActiveSkateboard>k__BackingField");
		NativeFieldInfoPtr_SkateboardPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "SkateboardPrefab");
		NativeFieldInfoPtr_blockDismount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "blockDismount");
		NativeFieldInfoPtr_ModelContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "ModelContainer");
		NativeFieldInfoPtr_ModelPosition_Raised = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "ModelPosition_Raised");
		NativeFieldInfoPtr_ModelPosition_Lowered = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "ModelPosition_Lowered");
		NativeFieldInfoPtr_mountTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "mountTime");
		NativeFieldInfoPtr__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, "_state");
		NativeMethodInfoPtr_get_IsRiding_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667252);
		NativeMethodInfoPtr_set_IsRiding_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667253);
		NativeMethodInfoPtr_get_ActiveSkateboard_Public_get_Skateboard_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667254);
		NativeMethodInfoPtr_set_ActiveSkateboard_Private_set_Void_Skateboard_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667255);
		NativeMethodInfoPtr_get_State_Public_get_MonoState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667256);
		NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667257);
		NativeMethodInfoPtr_Exit_Private_Void_ExitAction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667258);
		NativeMethodInfoPtr_Update_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667259);
		NativeMethodInfoPtr_UpdateModel_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667260);
		NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667261);
		NativeMethodInfoPtr_Mount_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667262);
		NativeMethodInfoPtr_Dismount_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667263);
		NativeMethodInfoPtr_OnMount_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667264);
		NativeMethodInfoPtr_OnDismount_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667265);
		NativeMethodInfoPtr_CanMountHere_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667266);
		NativeMethodInfoPtr_GetSkateboardSpawnPose_Private_Pose_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667267);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr, 100667268);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105300, XrefRangeEnd = 105345, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Equip(ItemInstance item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105345, XrefRangeEnd = 105348, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Exit(ExitAction action)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Exit_Private_Void_ExitAction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105348, XrefRangeEnd = 105393, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105393, XrefRangeEnd = 105399, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateModel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateModel_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105399, XrefRangeEnd = 105418, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Unequip()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105418, XrefRangeEnd = 105420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Mount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Mount_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 105422, RefRangeEnd = 105423, XrefRangeStart = 105420, XrefRangeEnd = 105422, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Dismount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dismount_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105423, XrefRangeEnd = 105453, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnMount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnMount_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105453, XrefRangeEnd = 105488, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDismount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDismount_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105488, XrefRangeEnd = 105495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CanMountHere()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanMountHere_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 105553, RefRangeEnd = 105556, XrefRangeStart = 105495, XrefRangeEnd = 105553, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Pose GetSkateboardSpawnPose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSkateboardSpawnPose_Private_Pose_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Pose*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 105556, XrefRangeEnd = 105557, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Skateboard_Equippable()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Skateboard_Equippable>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Skateboard_Equippable(IntPtr pointer)
		: base(pointer)
	{
	}
}
