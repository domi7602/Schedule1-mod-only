using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Map;

public class ScheduledMaterialChange : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EOnState")]
	public enum EOnState
	{
		Undecided,
		On,
		Off
	}

	private static readonly IntPtr NativeFieldInfoPtr_Renderers;

	private static readonly IntPtr NativeFieldInfoPtr_MaterialIndex;

	private static readonly IntPtr NativeFieldInfoPtr_Enabled;

	private static readonly IntPtr NativeFieldInfoPtr_LogState;

	private static readonly IntPtr NativeFieldInfoPtr_OutsideTimeRangeMaterial;

	private static readonly IntPtr NativeFieldInfoPtr_InsideTimeRangeMaterial;

	private static readonly IntPtr NativeFieldInfoPtr_TimeRangeMin;

	private static readonly IntPtr NativeFieldInfoPtr_TimeRangeMax;

	private static readonly IntPtr NativeFieldInfoPtr_TimeRangeShift;

	private static readonly IntPtr NativeFieldInfoPtr_TimeRangeRandomization;

	private static readonly IntPtr NativeFieldInfoPtr_TurnOnChance;

	private static readonly IntPtr NativeFieldInfoPtr_TurnOffChance;

	private static readonly IntPtr NativeFieldInfoPtr_appliedInsideTimeRange;

	private static readonly IntPtr NativeFieldInfoPtr_onState;

	private static readonly IntPtr NativeFieldInfoPtr_randomShift;

	private static readonly IntPtr NativeFieldInfoPtr__shouldTurnOn;

	private static readonly IntPtr NativeFieldInfoPtr__shouldTurnOff;

	private static readonly IntPtr NativeFieldInfoPtr__lastOnState;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetOnOffStatus_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMaterial_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<MeshRenderer> Renderers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Renderers);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<MeshRenderer>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Renderers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int MaterialIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialIndex)) = num;
		}
	}

	public unsafe bool Enabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Enabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Enabled)) = flag;
		}
	}

	public unsafe bool LogState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LogState);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LogState)) = flag;
		}
	}

	public unsafe Material OutsideTimeRangeMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutsideTimeRangeMaterial);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutsideTimeRangeMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material InsideTimeRangeMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InsideTimeRangeMaterial);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InsideTimeRangeMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe int TimeRangeMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeMin);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeMin)) = num;
		}
	}

	public unsafe int TimeRangeMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeMax);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeMax)) = num;
		}
	}

	public unsafe int TimeRangeShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeShift);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeShift)) = num;
		}
	}

	public unsafe int TimeRangeRandomization
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeRandomization);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeRangeRandomization)) = num;
		}
	}

	public unsafe float TurnOnChance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnOnChance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnOnChance)) = num;
		}
	}

	public unsafe float TurnOffChance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnOffChance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TurnOffChance)) = num;
		}
	}

	public unsafe bool appliedInsideTimeRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_appliedInsideTimeRange);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_appliedInsideTimeRange)) = flag;
		}
	}

	public unsafe EOnState onState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onState);
			return *(EOnState*)num;
		}
		set
		{
			*(EOnState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onState)) = eOnState;
		}
	}

	public unsafe int randomShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_randomShift);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_randomShift)) = num;
		}
	}

	public unsafe bool _shouldTurnOn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shouldTurnOn);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shouldTurnOn)) = flag;
		}
	}

	public unsafe bool _shouldTurnOff
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shouldTurnOff);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shouldTurnOff)) = flag;
		}
	}

	public unsafe EOnState _lastOnState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lastOnState);
			return *(EOnState*)num;
		}
		set
		{
			*(EOnState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lastOnState)) = eOnState;
		}
	}

	static ScheduledMaterialChange()
	{
		Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map", "ScheduledMaterialChange");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr);
		NativeFieldInfoPtr_Renderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "Renderers");
		NativeFieldInfoPtr_MaterialIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "MaterialIndex");
		NativeFieldInfoPtr_Enabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "Enabled");
		NativeFieldInfoPtr_LogState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "LogState");
		NativeFieldInfoPtr_OutsideTimeRangeMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "OutsideTimeRangeMaterial");
		NativeFieldInfoPtr_InsideTimeRangeMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "InsideTimeRangeMaterial");
		NativeFieldInfoPtr_TimeRangeMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "TimeRangeMin");
		NativeFieldInfoPtr_TimeRangeMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "TimeRangeMax");
		NativeFieldInfoPtr_TimeRangeShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "TimeRangeShift");
		NativeFieldInfoPtr_TimeRangeRandomization = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "TimeRangeRandomization");
		NativeFieldInfoPtr_TurnOnChance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "TurnOnChance");
		NativeFieldInfoPtr_TurnOffChance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "TurnOffChance");
		NativeFieldInfoPtr_appliedInsideTimeRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "appliedInsideTimeRange");
		NativeFieldInfoPtr_onState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "onState");
		NativeFieldInfoPtr_randomShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "randomShift");
		NativeFieldInfoPtr__shouldTurnOn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "_shouldTurnOn");
		NativeFieldInfoPtr__shouldTurnOff = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "_shouldTurnOff");
		NativeFieldInfoPtr__lastOnState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, "_lastOnState");
		NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670255);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670256);
		NativeMethodInfoPtr_Reset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670257);
		NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670258);
		NativeMethodInfoPtr_SetOnOffStatus_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670259);
		NativeMethodInfoPtr_SetMaterial_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670260);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr, 100670261);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 142934, XrefRangeEnd = 142971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 142971, XrefRangeEnd = 143001, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 143001, XrefRangeEnd = 143004, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 143004, XrefRangeEnd = 143042, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnUncappedMinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetOnOffStatus()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetOnOffStatus_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 143047, RefRangeEnd = 143050, XrefRangeStart = 143042, XrefRangeEnd = 143047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMaterial(bool insideTimeRange)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&insideTimeRange);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMaterial_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 143050, XrefRangeEnd = 143051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ScheduledMaterialChange()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ScheduledMaterialChange>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ScheduledMaterialChange(IntPtr pointer)
		: base(pointer)
	{
	}
}
