using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Police;
using Il2CppSystem;

namespace Il2CppScheduleOne.Law;

[System.Serializable]
public class CheckpointInstance : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MIN_ACTIVATION_DISTANCE;

	private static readonly System.IntPtr NativeFieldInfoPtr_Location;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinMembers;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxMembers;

	private static readonly System.IntPtr NativeFieldInfoPtr_StartTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_EndTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_IntensityRequirement;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnlyIfCurfewEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_checkPoint;

	private static readonly System.IntPtr NativeFieldInfoPtr__activeCheckpoint_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_activeCheckpoint_Public_get_RoadCheckpoint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_activeCheckpoint_Protected_set_Void_RoadCheckpoint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Evaluate_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnableCheckpoint_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DistanceRequirementsMet_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MinPass_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisableCheckpoint_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float MIN_ACTIVATION_DISTANCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MIN_ACTIVATION_DISTANCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MIN_ACTIVATION_DISTANCE, (void*)(&num));
		}
	}

	public unsafe CheckpointManager.ECheckpointLocation Location
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Location);
			return *(CheckpointManager.ECheckpointLocation*)num;
		}
		set
		{
			*(CheckpointManager.ECheckpointLocation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Location)) = eCheckpointLocation;
		}
	}

	public unsafe int MinMembers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinMembers);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinMembers)) = num;
		}
	}

	public unsafe int MaxMembers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxMembers);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxMembers)) = num;
		}
	}

	public unsafe int StartTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartTime)) = num;
		}
	}

	public unsafe int EndTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EndTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EndTime)) = num;
		}
	}

	public unsafe int IntensityRequirement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IntensityRequirement);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IntensityRequirement)) = num;
		}
	}

	public unsafe bool OnlyIfCurfewEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnlyIfCurfewEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnlyIfCurfewEnabled)) = flag;
		}
	}

	public unsafe RoadCheckpoint checkPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_checkPoint);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RoadCheckpoint>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_checkPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)roadCheckpoint));
		}
	}

	public unsafe RoadCheckpoint _activeCheckpoint_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__activeCheckpoint_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RoadCheckpoint>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__activeCheckpoint_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)roadCheckpoint));
		}
	}

	public unsafe RoadCheckpoint activeCheckpoint
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 2980, RefRangeEnd = 2987, XrefRangeStart = 2980, XrefRangeEnd = 2987, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_activeCheckpoint_Public_get_RoadCheckpoint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RoadCheckpoint>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_activeCheckpoint_Protected_set_Void_RoadCheckpoint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static CheckpointInstance()
	{
		Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Law", "CheckpointInstance");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr);
		NativeFieldInfoPtr_MIN_ACTIVATION_DISTANCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "MIN_ACTIVATION_DISTANCE");
		NativeFieldInfoPtr_Location = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "Location");
		NativeFieldInfoPtr_MinMembers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "MinMembers");
		NativeFieldInfoPtr_MaxMembers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "MaxMembers");
		NativeFieldInfoPtr_StartTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "StartTime");
		NativeFieldInfoPtr_EndTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "EndTime");
		NativeFieldInfoPtr_IntensityRequirement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "IntensityRequirement");
		NativeFieldInfoPtr_OnlyIfCurfewEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "OnlyIfCurfewEnabled");
		NativeFieldInfoPtr_checkPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "checkPoint");
		NativeFieldInfoPtr__activeCheckpoint_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, "<activeCheckpoint>k__BackingField");
		NativeMethodInfoPtr_get_activeCheckpoint_Public_get_RoadCheckpoint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671128);
		NativeMethodInfoPtr_set_activeCheckpoint_Protected_set_Void_RoadCheckpoint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671129);
		NativeMethodInfoPtr_Evaluate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671130);
		NativeMethodInfoPtr_EnableCheckpoint_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671131);
		NativeMethodInfoPtr_DistanceRequirementsMet_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671132);
		NativeMethodInfoPtr_MinPass_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671133);
		NativeMethodInfoPtr_DisableCheckpoint_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671134);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr, 100671135);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 152070, RefRangeEnd = 152071, XrefRangeStart = 152042, XrefRangeEnd = 152070, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Evaluate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Evaluate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 152107, RefRangeEnd = 152108, XrefRangeStart = 152071, XrefRangeEnd = 152107, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnableCheckpoint()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnableCheckpoint_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 152126, RefRangeEnd = 152128, XrefRangeStart = 152108, XrefRangeEnd = 152126, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool DistanceRequirementsMet()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DistanceRequirementsMet_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 152128, XrefRangeEnd = 152135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MinPass_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 152162, RefRangeEnd = 152163, XrefRangeStart = 152135, XrefRangeEnd = 152162, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DisableCheckpoint()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisableCheckpoint_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 152163, XrefRangeEnd = 152164, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CheckpointInstance()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CheckpointInstance>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CheckpointInstance(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
