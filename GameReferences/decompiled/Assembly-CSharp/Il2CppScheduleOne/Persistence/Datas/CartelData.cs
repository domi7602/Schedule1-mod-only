using System;
using System.Runtime.CompilerServices;
using Il2Cpp;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Cartel;
using Il2CppScheduleOne.Map;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class CartelData : SaveData
{
	[Serializable]
	public class RegionIntDict : SerializableDictionary<EMapRegion, int>
	{
		private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		static RegionIntDict()
		{
			Il2CppClassPointerStore<RegionIntDict>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "RegionIntDict");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RegionIntDict>.NativeClassPtr);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionIntDict>.NativeClassPtr, 100669316);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 134331, XrefRangeEnd = 134334, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe RegionIntDict()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RegionIntDict>.NativeClassPtr))
		{
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public RegionIntDict(IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly IntPtr NativeFieldInfoPtr_Status;

	private static readonly IntPtr NativeFieldInfoPtr_HoursSinceStatusChange;

	private static readonly IntPtr NativeFieldInfoPtr_RegionInfluence;

	private static readonly IntPtr NativeFieldInfoPtr_HoursUntilNextGlobalActivity;

	private static readonly IntPtr NativeFieldInfoPtr_RegionalActivityData;

	private static readonly IntPtr NativeFieldInfoPtr_ActiveCartelDeal;

	private static readonly IntPtr NativeFieldInfoPtr_HoursUntilNextDealRequest;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_ECartelStatus_Int32_Il2CppReferenceArray_1_RegionInfluenceData_Int32_Il2CppReferenceArray_1_CartelRegionalActivityData_CartelDealInfo_Int32_0;

	public unsafe ECartelStatus Status
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Status);
			return *(ECartelStatus*)num;
		}
		set
		{
			*(ECartelStatus*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Status)) = eCartelStatus;
		}
	}

	public unsafe int HoursSinceStatusChange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursSinceStatusChange);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursSinceStatusChange)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<CartelInfluence.RegionInfluenceData> RegionInfluence
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionInfluence);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CartelInfluence.RegionInfluenceData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionInfluence)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int HoursUntilNextGlobalActivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursUntilNextGlobalActivity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursUntilNextGlobalActivity)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<CartelRegionalActivityData> RegionalActivityData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionalActivityData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CartelRegionalActivityData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionalActivityData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe CartelDealInfo ActiveCartelDeal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveCartelDeal);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CartelDealInfo>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveCartelDeal)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cartelDealInfo));
		}
	}

	public unsafe int HoursUntilNextDealRequest
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursUntilNextDealRequest);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursUntilNextDealRequest)) = num;
		}
	}

	static CartelData()
	{
		Il2CppClassPointerStore<CartelData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "CartelData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CartelData>.NativeClassPtr);
		NativeFieldInfoPtr_Status = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "Status");
		NativeFieldInfoPtr_HoursSinceStatusChange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "HoursSinceStatusChange");
		NativeFieldInfoPtr_RegionInfluence = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "RegionInfluence");
		NativeFieldInfoPtr_HoursUntilNextGlobalActivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "HoursUntilNextGlobalActivity");
		NativeFieldInfoPtr_RegionalActivityData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "RegionalActivityData");
		NativeFieldInfoPtr_ActiveCartelDeal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "ActiveCartelDeal");
		NativeFieldInfoPtr_HoursUntilNextDealRequest = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelData>.NativeClassPtr, "HoursUntilNextDealRequest");
		NativeMethodInfoPtr__ctor_Public_Void_ECartelStatus_Int32_Il2CppReferenceArray_1_RegionInfluenceData_Int32_Il2CppReferenceArray_1_CartelRegionalActivityData_CartelDealInfo_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelData>.NativeClassPtr, 100669315);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134338, RefRangeEnd = 134340, XrefRangeStart = 134334, XrefRangeEnd = 134338, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelData(ECartelStatus status, int hoursSinceStatusChange, Il2CppReferenceArray<CartelInfluence.RegionInfluenceData> regionInfluence, int hoursUntilNextGlobalActivity, Il2CppReferenceArray<CartelRegionalActivityData> regionalActivityData, CartelDealInfo activeCartelDeal, int hoursUntilNextDealRequest)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = (nint)(&status);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &hoursSinceStatusChange;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regionInfluence);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &hoursUntilNextGlobalActivity;
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regionalActivityData);
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeCartelDeal);
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &hoursUntilNextDealRequest;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ECartelStatus_Int32_Il2CppReferenceArray_1_RegionInfluenceData_Int32_Il2CppReferenceArray_1_CartelRegionalActivityData_CartelDealInfo_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CartelData(IntPtr pointer)
		: base(pointer)
	{
	}
}
