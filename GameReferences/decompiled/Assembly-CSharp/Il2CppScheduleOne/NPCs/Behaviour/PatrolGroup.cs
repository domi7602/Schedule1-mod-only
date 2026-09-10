using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.NPCs.Behaviour;

public class PatrolGroup : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Members;

	private static readonly System.IntPtr NativeFieldInfoPtr_Route;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentWaypoint;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_FootPatrolRoute_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDestination_Public_Vector3_NPC_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisbandGroup_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdvanceGroup_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMemberOffset_Private_Vector3_NPC_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsGroupReadyToAdvance_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsPaused_Public_Boolean_0;

	public unsafe List<NPC> Members
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Members);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<NPC>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Members)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe FootPatrolRoute Route
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Route);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<FootPatrolRoute>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Route)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)footPatrolRoute));
		}
	}

	public unsafe int CurrentWaypoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentWaypoint);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentWaypoint)) = num;
		}
	}

	static PatrolGroup()
	{
		Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Behaviour", "PatrolGroup");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr);
		NativeFieldInfoPtr_Members = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, "Members");
		NativeFieldInfoPtr_Route = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, "Route");
		NativeFieldInfoPtr_CurrentWaypoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, "CurrentWaypoint");
		NativeMethodInfoPtr__ctor_Public_Void_FootPatrolRoute_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684425);
		NativeMethodInfoPtr_GetDestination_Public_Vector3_NPC_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684426);
		NativeMethodInfoPtr_DisbandGroup_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684427);
		NativeMethodInfoPtr_AdvanceGroup_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684428);
		NativeMethodInfoPtr_GetMemberOffset_Private_Vector3_NPC_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684429);
		NativeMethodInfoPtr_IsGroupReadyToAdvance_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684430);
		NativeMethodInfoPtr_IsPaused_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr, 100684431);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 282798, RefRangeEnd = 282799, XrefRangeStart = 282789, XrefRangeEnd = 282798, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PatrolGroup(FootPatrolRoute route)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PatrolGroup>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)route);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_FootPatrolRoute_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 282834, RefRangeEnd = 282837, XrefRangeStart = 282799, XrefRangeEnd = 282834, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetDestination(NPC member)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)member);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDestination_Public_Vector3_NPC_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 282862, RefRangeEnd = 282864, XrefRangeStart = 282837, XrefRangeEnd = 282862, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DisbandGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisbandGroup_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void AdvanceGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdvanceGroup_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 282864, XrefRangeEnd = 282875, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetMemberOffset(NPC member)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)member);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMemberOffset_Private_Vector3_NPC_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 282875, XrefRangeEnd = 282883, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsGroupReadyToAdvance()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsGroupReadyToAdvance_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 282883, XrefRangeEnd = 282902, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsPaused()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPaused_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PatrolGroup(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
