using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.Police;
using Il2CppSystem;

namespace Il2CppScheduleOne.Law;

[System.Serializable]
public class VehiclePatrolInstance : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Route;

	private static readonly System.IntPtr NativeFieldInfoPtr_StartTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_IntensityRequirement;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnlyIfCurfewEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_activeOfficer;

	private static readonly System.IntPtr NativeFieldInfoPtr_latestStartTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_startedThisCycle;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_nearestStation_Private_get_PoliceStation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Evaluate_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckEnd_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartPatrol_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe VehiclePatrolRoute Route
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Route);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VehiclePatrolRoute>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Route)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehiclePatrolRoute));
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

	public unsafe PoliceOfficer activeOfficer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeOfficer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PoliceOfficer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeOfficer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)policeOfficer));
		}
	}

	public unsafe int latestStartTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_latestStartTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_latestStartTime)) = num;
		}
	}

	public unsafe bool startedThisCycle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startedThisCycle);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startedThisCycle)) = flag;
		}
	}

	public unsafe PoliceStation nearestStation
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 153252, RefRangeEnd = 153258, XrefRangeStart = 153246, XrefRangeEnd = 153252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_nearestStation_Private_get_PoliceStation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PoliceStation>(intPtr) : null;
		}
	}

	static VehiclePatrolInstance()
	{
		Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Law", "VehiclePatrolInstance");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr);
		NativeFieldInfoPtr_Route = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "Route");
		NativeFieldInfoPtr_StartTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "StartTime");
		NativeFieldInfoPtr_IntensityRequirement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "IntensityRequirement");
		NativeFieldInfoPtr_OnlyIfCurfewEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "OnlyIfCurfewEnabled");
		NativeFieldInfoPtr_activeOfficer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "activeOfficer");
		NativeFieldInfoPtr_latestStartTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "latestStartTime");
		NativeFieldInfoPtr_startedThisCycle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, "startedThisCycle");
		NativeMethodInfoPtr_get_nearestStation_Private_get_PoliceStation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, 100671294);
		NativeMethodInfoPtr_Evaluate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, 100671295);
		NativeMethodInfoPtr_CheckEnd_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, 100671296);
		NativeMethodInfoPtr_StartPatrol_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, 100671297);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr, 100671298);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 153295, RefRangeEnd = 153296, XrefRangeStart = 153258, XrefRangeEnd = 153295, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Evaluate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Evaluate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153296, XrefRangeEnd = 153301, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckEnd()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckEnd_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153301, XrefRangeEnd = 153320, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartPatrol()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartPatrol_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153320, XrefRangeEnd = 153321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehiclePatrolInstance()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VehiclePatrolInstance>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VehiclePatrolInstance(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
