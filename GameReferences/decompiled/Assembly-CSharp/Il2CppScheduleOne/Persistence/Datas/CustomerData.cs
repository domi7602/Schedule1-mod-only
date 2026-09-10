using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Quests;

namespace Il2CppScheduleOne.Persistence.Datas;

public class CustomerData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Dependence;

	private static readonly IntPtr NativeFieldInfoPtr_ProductAffinities;

	private static readonly IntPtr NativeFieldInfoPtr_TimeSinceLastDealCompleted;

	private static readonly IntPtr NativeFieldInfoPtr_TimeSinceLastDealOffered;

	private static readonly IntPtr NativeFieldInfoPtr_OfferedDeals;

	private static readonly IntPtr NativeFieldInfoPtr_CompletedDeals;

	private static readonly IntPtr NativeFieldInfoPtr_IsContractOffered;

	private static readonly IntPtr NativeFieldInfoPtr_OfferedContract;

	private static readonly IntPtr NativeFieldInfoPtr_OfferedContractTime;

	private static readonly IntPtr NativeFieldInfoPtr_TimeSincePlayerApproached;

	private static readonly IntPtr NativeFieldInfoPtr_TimeSinceInstantDealOffered;

	private static readonly IntPtr NativeFieldInfoPtr_HasBeenRecommended;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Il2CppStructArray_1_Single_Int32_Int32_Int32_Int32_Boolean_ContractInfo_GameDateTime_Int32_Int32_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float Dependence
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dependence);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dependence)) = num;
		}
	}

	public unsafe Il2CppStructArray<float> ProductAffinities
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductAffinities);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductAffinities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int TimeSinceLastDealCompleted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSinceLastDealCompleted);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSinceLastDealCompleted)) = num;
		}
	}

	public unsafe int TimeSinceLastDealOffered
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSinceLastDealOffered);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSinceLastDealOffered)) = num;
		}
	}

	public unsafe int OfferedDeals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OfferedDeals);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OfferedDeals)) = num;
		}
	}

	public unsafe int CompletedDeals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompletedDeals);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompletedDeals)) = num;
		}
	}

	public unsafe bool IsContractOffered
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsContractOffered);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsContractOffered)) = flag;
		}
	}

	public unsafe ContractInfo OfferedContract
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OfferedContract);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ContractInfo>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OfferedContract)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)contractInfo));
		}
	}

	public unsafe GameDateTime OfferedContractTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OfferedContractTime);
			return *(GameDateTime*)num;
		}
		set
		{
			*(GameDateTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OfferedContractTime)) = gameDateTime;
		}
	}

	public unsafe int TimeSincePlayerApproached
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSincePlayerApproached);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSincePlayerApproached)) = num;
		}
	}

	public unsafe int TimeSinceInstantDealOffered
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSinceInstantDealOffered);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeSinceInstantDealOffered)) = num;
		}
	}

	public unsafe bool HasBeenRecommended
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasBeenRecommended);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasBeenRecommended)) = flag;
		}
	}

	static CustomerData()
	{
		Il2CppClassPointerStore<CustomerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "CustomerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CustomerData>.NativeClassPtr);
		NativeFieldInfoPtr_Dependence = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "Dependence");
		NativeFieldInfoPtr_ProductAffinities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "ProductAffinities");
		NativeFieldInfoPtr_TimeSinceLastDealCompleted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "TimeSinceLastDealCompleted");
		NativeFieldInfoPtr_TimeSinceLastDealOffered = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "TimeSinceLastDealOffered");
		NativeFieldInfoPtr_OfferedDeals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "OfferedDeals");
		NativeFieldInfoPtr_CompletedDeals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "CompletedDeals");
		NativeFieldInfoPtr_IsContractOffered = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "IsContractOffered");
		NativeFieldInfoPtr_OfferedContract = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "OfferedContract");
		NativeFieldInfoPtr_OfferedContractTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "OfferedContractTime");
		NativeFieldInfoPtr_TimeSincePlayerApproached = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "TimeSincePlayerApproached");
		NativeFieldInfoPtr_TimeSinceInstantDealOffered = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "TimeSinceInstantDealOffered");
		NativeFieldInfoPtr_HasBeenRecommended = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, "HasBeenRecommended");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Il2CppStructArray_1_Single_Int32_Int32_Int32_Int32_Boolean_ContractInfo_GameDateTime_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, 100669317);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomerData>.NativeClassPtr, 100669318);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134343, RefRangeEnd = 134344, XrefRangeStart = 134340, XrefRangeEnd = 134343, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CustomerData(float dependence, Il2CppStructArray<float> productAffinities, int timeSinceLastDealCompleted, int timeSinceLastDealOffered, int offeredDeals, int completedDeals, bool isContractOffered, ContractInfo offeredContract, GameDateTime offeredTime, int timeSincePlayerApproached, int timeSinceInstantDealOffered, bool hasBeenRecommended)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CustomerData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[12];
		*ptr = (nint)(&dependence);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)productAffinities);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &timeSinceLastDealCompleted;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &timeSinceLastDealOffered;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &offeredDeals;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &completedDeals;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &isContractOffered;
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)offeredContract);
		*(GameDateTime**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = &offeredTime;
		*(int**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(IntPtr)))) = &timeSincePlayerApproached;
		*(int**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(IntPtr)))) = &timeSinceInstantDealOffered;
		*(bool**)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(IntPtr)))) = &hasBeenRecommended;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Il2CppStructArray_1_Single_Int32_Int32_Int32_Int32_Boolean_ContractInfo_GameDateTime_Int32_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134345, RefRangeEnd = 134346, XrefRangeStart = 134344, XrefRangeEnd = 134345, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CustomerData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CustomerData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CustomerData(IntPtr pointer)
		: base(pointer)
	{
	}
}
