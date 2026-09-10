using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vision;

public class VisionEvent : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_NOTICE_DROP_THRESHOLD;

	private static readonly System.IntPtr NativeFieldInfoPtr__Target_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__State_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Owner_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__FullNoticeTime_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_timeSinceSighted;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentNoticeTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_playTremolo;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Target_Public_get_ISightable_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Target_Protected_set_Void_ISightable_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_State_Public_get_EntityVisualState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_State_Protected_set_Void_EntityVisualState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Owner_Public_get_VisionCone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Owner_Protected_set_Void_VisionCone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FullNoticeTime_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_FullNoticeTime_Protected_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NormalizedNoticeLevel_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_VisionCone_ISightable_EntityVisualState_Single_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateEvent_Public_Void_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EndEvent_Public_Void_0;

	public unsafe static float NOTICE_DROP_THRESHOLD
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NOTICE_DROP_THRESHOLD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NOTICE_DROP_THRESHOLD, (void*)(&num));
		}
	}

	public unsafe ISightable _Target_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Target_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ISightable>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Target_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sightable));
		}
	}

	public unsafe EntityVisualState _State_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__State_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EntityVisualState>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__State_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entityVisualState));
		}
	}

	public unsafe VisionCone _Owner_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Owner_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VisionCone>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Owner_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)visionCone));
		}
	}

	public unsafe float _FullNoticeTime_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__FullNoticeTime_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__FullNoticeTime_k__BackingField)) = num;
		}
	}

	public unsafe float timeSinceSighted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceSighted);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceSighted)) = num;
		}
	}

	public unsafe float currentNoticeTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentNoticeTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentNoticeTime)) = num;
		}
	}

	public unsafe bool playTremolo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_playTremolo);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_playTremolo)) = flag;
		}
	}

	public unsafe ISightable Target
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Target_Public_get_ISightable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ISightable>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29107, RefRangeEnd = 29109, XrefRangeStart = 29107, XrefRangeEnd = 29109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Target_Protected_set_Void_ISightable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe EntityVisualState State
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_State_Public_get_EntityVisualState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EntityVisualState>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_State_Protected_set_Void_EntityVisualState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe VisionCone Owner
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Owner_Public_get_VisionCone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VisionCone>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Owner_Protected_set_Void_VisionCone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float FullNoticeTime
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 55725, RefRangeEnd = 55726, XrefRangeStart = 55725, XrefRangeEnd = 55726, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FullNoticeTime_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_FullNoticeTime_Protected_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float NormalizedNoticeLevel
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 123608, RefRangeEnd = 123609, XrefRangeStart = 123608, XrefRangeEnd = 123608, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NormalizedNoticeLevel_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static VisionEvent()
	{
		Il2CppClassPointerStore<VisionEvent>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vision", "VisionEvent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr);
		NativeFieldInfoPtr_NOTICE_DROP_THRESHOLD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "NOTICE_DROP_THRESHOLD");
		NativeFieldInfoPtr__Target_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "<Target>k__BackingField");
		NativeFieldInfoPtr__State_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "<State>k__BackingField");
		NativeFieldInfoPtr__Owner_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "<Owner>k__BackingField");
		NativeFieldInfoPtr__FullNoticeTime_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "<FullNoticeTime>k__BackingField");
		NativeFieldInfoPtr_timeSinceSighted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "timeSinceSighted");
		NativeFieldInfoPtr_currentNoticeTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "currentNoticeTime");
		NativeFieldInfoPtr_playTremolo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, "playTremolo");
		NativeMethodInfoPtr_get_Target_Public_get_ISightable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668648);
		NativeMethodInfoPtr_set_Target_Protected_set_Void_ISightable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668649);
		NativeMethodInfoPtr_get_State_Public_get_EntityVisualState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668650);
		NativeMethodInfoPtr_set_State_Protected_set_Void_EntityVisualState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668651);
		NativeMethodInfoPtr_get_Owner_Public_get_VisionCone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668652);
		NativeMethodInfoPtr_set_Owner_Protected_set_Void_VisionCone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668653);
		NativeMethodInfoPtr_get_FullNoticeTime_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668654);
		NativeMethodInfoPtr_set_FullNoticeTime_Protected_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668655);
		NativeMethodInfoPtr_get_NormalizedNoticeLevel_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668656);
		NativeMethodInfoPtr__ctor_Public_Void_VisionCone_ISightable_EntityVisualState_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668657);
		NativeMethodInfoPtr_UpdateEvent_Public_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668658);
		NativeMethodInfoPtr_EndEvent_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr, 100668659);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 123627, RefRangeEnd = 123628, XrefRangeStart = 123609, XrefRangeEnd = 123627, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VisionEvent(VisionCone _owner, ISightable _target, EntityVisualState _state, float _noticeTime, bool _playTremolo)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VisionEvent>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_owner);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_target);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_state);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &_noticeTime;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &_playTremolo;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_VisionCone_ISightable_EntityVisualState_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 123656, RefRangeEnd = 123657, XrefRangeStart = 123628, XrefRangeEnd = 123656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateEvent(float visionDeltaThisFrame, float tickTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&visionDeltaThisFrame);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tickTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateEvent_Public_Void_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 123663, RefRangeEnd = 123665, XrefRangeStart = 123657, XrefRangeEnd = 123663, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EndEvent()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndEvent_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VisionEvent(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
