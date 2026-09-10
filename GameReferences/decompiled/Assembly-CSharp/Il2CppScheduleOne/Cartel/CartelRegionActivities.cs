using System;
using System.Runtime.CompilerServices;
using Il2Cpp;
using Il2CppFishNet.Connection;
using Il2CppFishNet.Object;
using Il2CppFishNet.Serializing;
using Il2CppFishNet.Transporting;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.Persistence;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Cartel;

public class CartelRegionActivities : NetworkBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_MIN_COOLDOWN;

	private static readonly IntPtr NativeFieldInfoPtr_MAX_COOLDOWN;

	private static readonly IntPtr NativeFieldInfoPtr_TEST_MODE;

	private static readonly IntPtr NativeFieldInfoPtr__CurrentActivity_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__HoursUntilNextActivity_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_Active;

	private static readonly IntPtr NativeFieldInfoPtr_Region;

	private static readonly IntPtr NativeFieldInfoPtr_Activities;

	private static readonly IntPtr NativeFieldInfoPtr_AmbushLocations;

	private static readonly IntPtr NativeFieldInfoPtr_CartelDealer;

	private static readonly IntPtr NativeFieldInfoPtr__debugActivityIndex;

	private static readonly IntPtr NativeFieldInfoPtr_field_Private_Boolean_0;

	private static readonly IntPtr NativeFieldInfoPtr_field_Private_Boolean_1;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentActivity_Public_get_CartelActivity_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CurrentActivity_Private_set_Void_CartelActivity_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_HoursUntilNextActivity_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_HoursUntilNextActivity_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSpawnServer_Public_Virtual_Void_NetworkConnection_0;

	private static readonly IntPtr NativeMethodInfoPtr_HourPass_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_TryStartActivity_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartActivity_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartAcivity_Private_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_ActivateDeal_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartActivity_Private_Void_NetworkConnection_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_ActivityEnded_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetData_Public_CartelRegionalActivityData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Load_Public_Void_CartelRegionalActivityData_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetNewCooldown_Public_Static_Int32_EMapRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_CartelStatusChange_Private_Void_ECartelStatus_ECartelStatus_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NetworkInitialize___Early_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NetworkInitialize__Late_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NetworkInitializeIfDisabled_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcWriter___Observers_StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcLogic___StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcReader___Observers_StartActivity_2681120339_Private_Void_PooledReader_Channel_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcWriter___Target_StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcReader___Target_StartActivity_2681120339_Private_Void_PooledReader_Channel_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Public_Virtual_Void_0;

	public unsafe static int MIN_COOLDOWN
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MIN_COOLDOWN, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MIN_COOLDOWN, (void*)(&num));
		}
	}

	public unsafe static int MAX_COOLDOWN
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_COOLDOWN, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_COOLDOWN, (void*)(&num));
		}
	}

	public unsafe bool TEST_MODE
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TEST_MODE);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TEST_MODE)) = flag;
		}
	}

	public unsafe CartelActivity _CurrentActivity_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentActivity_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CartelActivity>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentActivity_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cartelActivity));
		}
	}

	public unsafe int _HoursUntilNextActivity_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HoursUntilNextActivity_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HoursUntilNextActivity_k__BackingField)) = num;
		}
	}

	public unsafe bool Active
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Active);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Active)) = flag;
		}
	}

	public unsafe EMapRegion Region
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Region);
			return *(EMapRegion*)num;
		}
		set
		{
			*(EMapRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Region)) = eMapRegion;
		}
	}

	public unsafe List<CartelActivity> Activities
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Activities);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<CartelActivity>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Activities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppReferenceArray<CartelAmbushLocation> AmbushLocations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AmbushLocations);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CartelAmbushLocation>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AmbushLocations)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe CartelDealer CartelDealer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CartelDealer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CartelDealer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CartelDealer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cartelDealer));
		}
	}

	public unsafe int _debugActivityIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugActivityIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugActivityIndex)) = num;
		}
	}

	public unsafe bool field_Private_Boolean_0
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_field_Private_Boolean_0);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_field_Private_Boolean_0)) = flag;
		}
	}

	public unsafe bool field_Private_Boolean_1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_field_Private_Boolean_1);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_field_Private_Boolean_1)) = flag;
		}
	}

	public unsafe CartelActivity CurrentActivity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentActivity_Public_get_CartelActivity_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CartelActivity>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentActivity_Private_set_Void_CartelActivity_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int HoursUntilNextActivity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HoursUntilNextActivity_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HoursUntilNextActivity_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static CartelRegionActivities()
	{
		Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Cartel", "CartelRegionActivities");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr);
		NativeFieldInfoPtr_MIN_COOLDOWN = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "MIN_COOLDOWN");
		NativeFieldInfoPtr_MAX_COOLDOWN = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "MAX_COOLDOWN");
		NativeFieldInfoPtr_TEST_MODE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "TEST_MODE");
		NativeFieldInfoPtr__CurrentActivity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "<CurrentActivity>k__BackingField");
		NativeFieldInfoPtr__HoursUntilNextActivity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "<HoursUntilNextActivity>k__BackingField");
		NativeFieldInfoPtr_Active = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "Active");
		NativeFieldInfoPtr_Region = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "Region");
		NativeFieldInfoPtr_Activities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "Activities");
		NativeFieldInfoPtr_AmbushLocations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "AmbushLocations");
		NativeFieldInfoPtr_CartelDealer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "CartelDealer");
		NativeFieldInfoPtr__debugActivityIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "_debugActivityIndex");
		NativeFieldInfoPtr_field_Private_Boolean_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "NetworkInitialize___EarlyScheduleOne.Cartel.CartelRegionActivitiesAssembly-CSharp.dll_Excuted");
		NativeFieldInfoPtr_field_Private_Boolean_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, "NetworkInitialize__LateScheduleOne.Cartel.CartelRegionActivitiesAssembly-CSharp.dll_Excuted");
		NativeMethodInfoPtr_get_CurrentActivity_Public_get_CartelActivity_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676479);
		NativeMethodInfoPtr_set_CurrentActivity_Private_set_Void_CartelActivity_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676480);
		NativeMethodInfoPtr_get_HoursUntilNextActivity_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676481);
		NativeMethodInfoPtr_set_HoursUntilNextActivity_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676482);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676483);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676484);
		NativeMethodInfoPtr_OnSpawnServer_Public_Virtual_Void_NetworkConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676485);
		NativeMethodInfoPtr_HourPass_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676486);
		NativeMethodInfoPtr_TryStartActivity_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676487);
		NativeMethodInfoPtr_StartActivity_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676488);
		NativeMethodInfoPtr_StartAcivity_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676489);
		NativeMethodInfoPtr_ActivateDeal_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676490);
		NativeMethodInfoPtr_StartActivity_Private_Void_NetworkConnection_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676491);
		NativeMethodInfoPtr_ActivityEnded_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676492);
		NativeMethodInfoPtr_GetData_Public_CartelRegionalActivityData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676493);
		NativeMethodInfoPtr_Load_Public_Void_CartelRegionalActivityData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676494);
		NativeMethodInfoPtr_GetNewCooldown_Public_Static_Int32_EMapRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676495);
		NativeMethodInfoPtr_CartelStatusChange_Private_Void_ECartelStatus_ECartelStatus_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676496);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676497);
		NativeMethodInfoPtr_NetworkInitialize___Early_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676498);
		NativeMethodInfoPtr_NetworkInitialize__Late_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676499);
		NativeMethodInfoPtr_NetworkInitializeIfDisabled_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676500);
		NativeMethodInfoPtr_RpcWriter___Observers_StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676501);
		NativeMethodInfoPtr_RpcLogic___StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676502);
		NativeMethodInfoPtr_RpcReader___Observers_StartActivity_2681120339_Private_Void_PooledReader_Channel_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676503);
		NativeMethodInfoPtr_RpcWriter___Target_StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676504);
		NativeMethodInfoPtr_RpcReader___Target_StartActivity_2681120339_Private_Void_PooledReader_Channel_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676505);
		NativeMethodInfoPtr_Awake_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr, 100676506);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210684, XrefRangeEnd = 210693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210693, XrefRangeEnd = 210734, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210734, XrefRangeEnd = 210744, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnSpawnServer(NetworkConnection connection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)connection);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnSpawnServer_Public_Virtual_Void_NetworkConnection_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210744, XrefRangeEnd = 210756, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HourPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HourPass_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 210816, RefRangeEnd = 210817, XrefRangeStart = 210756, XrefRangeEnd = 210816, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TryStartActivity()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryStartActivity_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210817, XrefRangeEnd = 210818, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartActivity()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartActivity_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210818, XrefRangeEnd = 210819, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartAcivity(int activityIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&activityIndex);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartAcivity_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210819, XrefRangeEnd = 210820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ActivateDeal()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ActivateDeal_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 210861, RefRangeEnd = 210867, XrefRangeStart = 210820, XrefRangeEnd = 210861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartActivity(NetworkConnection conn, int activityIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &activityIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartActivity_Private_Void_NetworkConnection_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210867, XrefRangeEnd = 210883, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ActivityEnded()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ActivityEnded_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 210894, RefRangeEnd = 210895, XrefRangeStart = 210883, XrefRangeEnd = 210894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelRegionalActivityData GetData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetData_Public_CartelRegionalActivityData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CartelRegionalActivityData>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 210899, RefRangeEnd = 210900, XrefRangeStart = 210895, XrefRangeEnd = 210899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Load(CartelRegionalActivityData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Load_Public_Void_CartelRegionalActivityData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 210958, RefRangeEnd = 210962, XrefRangeStart = 210900, XrefRangeEnd = 210958, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetNewCooldown(EMapRegion region)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&region);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNewCooldown_Public_Static_Int32_EMapRegion_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210962, XrefRangeEnd = 210963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CartelStatusChange(ECartelStatus oldStatus, ECartelStatus newStatus)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&oldStatus);
		*(ECartelStatus**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &newStatus;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CartelStatusChange_Private_Void_ECartelStatus_ECartelStatus_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210963, XrefRangeEnd = 210971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelRegionActivities()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelRegionActivities>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210971, XrefRangeEnd = 210984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void NetworkInitialize___Early()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NetworkInitialize___Early_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void NetworkInitialize__Late()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NetworkInitialize__Late_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void NetworkInitializeIfDisabled()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NetworkInitializeIfDisabled_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 210984, XrefRangeEnd = 210995, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcWriter___Observers_StartActivity_2681120339(NetworkConnection conn, int activityIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &activityIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcWriter___Observers_StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 211033, RefRangeEnd = 211036, XrefRangeStart = 210995, XrefRangeEnd = 211033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcLogic___StartActivity_2681120339(NetworkConnection conn, int activityIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &activityIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcLogic___StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 211036, XrefRangeEnd = 211041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcReader___Observers_StartActivity_2681120339(PooledReader PooledReader0, Channel channel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)PooledReader0);
		*(Channel**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &channel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcReader___Observers_StartActivity_2681120339_Private_Void_PooledReader_Channel_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 211041, XrefRangeEnd = 211052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcWriter___Target_StartActivity_2681120339(NetworkConnection conn, int activityIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &activityIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcWriter___Target_StartActivity_2681120339_Private_Void_NetworkConnection_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 211052, XrefRangeEnd = 211057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcReader___Target_StartActivity_2681120339(PooledReader PooledReader0, Channel channel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)PooledReader0);
		*(Channel**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &channel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcReader___Target_StartActivity_2681120339_Private_Void_PooledReader_Channel_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CartelRegionActivities(IntPtr pointer)
		: base(pointer)
	{
	}
}
