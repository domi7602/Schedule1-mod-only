using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.Quests;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class ContractData : QuestData
{
	private static readonly IntPtr NativeFieldInfoPtr_CustomerGUID;

	private static readonly IntPtr NativeFieldInfoPtr_Payment;

	private static readonly IntPtr NativeFieldInfoPtr_ProductList;

	private static readonly IntPtr NativeFieldInfoPtr_DeliveryLocationGUID;

	private static readonly IntPtr NativeFieldInfoPtr_DeliveryWindow;

	private static readonly IntPtr NativeFieldInfoPtr_PickupScheduleIndex;

	private static readonly IntPtr NativeFieldInfoPtr_AcceptTime;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_Boolean_String_String_Boolean_GameDateTimeData_Il2CppReferenceArray_1_QuestEntryData_String_Single_ProductList_String_QuestWindowConfig_Int32_GameDateTimeData_0;

	public unsafe string CustomerGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomerGUID);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomerGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float Payment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Payment);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Payment)) = num;
		}
	}

	public unsafe ProductList ProductList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductList);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ProductList>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)productList));
		}
	}

	public unsafe string DeliveryLocationGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryLocationGUID);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryLocationGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe QuestWindowConfig DeliveryWindow
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryWindow);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QuestWindowConfig>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryWindow)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)questWindowConfig));
		}
	}

	public unsafe int PickupScheduleIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickupScheduleIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickupScheduleIndex)) = num;
		}
	}

	public unsafe GameDateTimeData AcceptTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AcceptTime);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameDateTimeData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AcceptTime)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameDateTimeData));
		}
	}

	static ContractData()
	{
		Il2CppClassPointerStore<ContractData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ContractData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ContractData>.NativeClassPtr);
		NativeFieldInfoPtr_CustomerGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "CustomerGUID");
		NativeFieldInfoPtr_Payment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "Payment");
		NativeFieldInfoPtr_ProductList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "ProductList");
		NativeFieldInfoPtr_DeliveryLocationGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "DeliveryLocationGUID");
		NativeFieldInfoPtr_DeliveryWindow = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "DeliveryWindow");
		NativeFieldInfoPtr_PickupScheduleIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "PickupScheduleIndex");
		NativeFieldInfoPtr_AcceptTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractData>.NativeClassPtr, "AcceptTime");
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_Boolean_String_String_Boolean_GameDateTimeData_Il2CppReferenceArray_1_QuestEntryData_String_Single_ProductList_String_QuestWindowConfig_Int32_GameDateTimeData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ContractData>.NativeClassPtr, 100669454);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135273, RefRangeEnd = 135274, XrefRangeStart = 135262, XrefRangeEnd = 135273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ContractData(string guid, EQuestState state, bool isTracked, string title, string desc, bool isTimed, GameDateTimeData expiry, Il2CppReferenceArray<QuestEntryData> entries, string customerGUID, float payment, ProductList productList, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTimeData acceptTime)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ContractData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[15];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(guid);
		*(EQuestState**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &state;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &isTracked;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(title);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(desc);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &isTimed;
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)expiry);
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entries);
		*(IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(customerGUID);
		*(float**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(IntPtr)))) = &payment;
		*(IntPtr*)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)productList);
		*(IntPtr*)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(deliveryLocationGUID);
		*(IntPtr*)((byte*)ptr + checked((nuint)12u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deliveryWindow);
		*(int**)((byte*)ptr + checked((nuint)13u * unchecked((nuint)sizeof(IntPtr)))) = &pickupScheduleIndex;
		*(IntPtr*)((byte*)ptr + checked((nuint)14u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)acceptTime);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_Boolean_String_String_Boolean_GameDateTimeData_Il2CppReferenceArray_1_QuestEntryData_String_Single_ProductList_String_QuestWindowConfig_Int32_GameDateTimeData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ContractData(IntPtr pointer)
		: base(pointer)
	{
	}
}
