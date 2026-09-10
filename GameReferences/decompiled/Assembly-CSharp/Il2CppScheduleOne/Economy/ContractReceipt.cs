using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Economy;

[System.Serializable]
public class ContractReceipt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ReceiptId;

	private static readonly System.IntPtr NativeFieldInfoPtr_CompletedBy;

	private static readonly System.IntPtr NativeFieldInfoPtr_CustomerId;

	private static readonly System.IntPtr NativeFieldInfoPtr_CompletionTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_Items;

	private static readonly System.IntPtr NativeFieldInfoPtr_AmountPaid;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_EContractParty_String_GameDateTime_Il2CppReferenceArray_1_StringIntPair_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int ReceiptId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReceiptId);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReceiptId)) = num;
		}
	}

	public unsafe EContractParty CompletedBy
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompletedBy);
			return *(EContractParty*)num;
		}
		set
		{
			*(EContractParty*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompletedBy)) = eContractParty;
		}
	}

	public unsafe string CustomerId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomerId);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomerId)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe GameDateTime CompletionTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompletionTime);
			return *(GameDateTime*)num;
		}
		set
		{
			*(GameDateTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompletionTime)) = gameDateTime;
		}
	}

	public unsafe Il2CppReferenceArray<StringIntPair> Items
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<StringIntPair>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float AmountPaid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AmountPaid);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AmountPaid)) = num;
		}
	}

	static ContractReceipt()
	{
		Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Economy", "ContractReceipt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr);
		NativeFieldInfoPtr_ReceiptId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, "ReceiptId");
		NativeFieldInfoPtr_CompletedBy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, "CompletedBy");
		NativeFieldInfoPtr_CustomerId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, "CustomerId");
		NativeFieldInfoPtr_CompletionTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, "CompletionTime");
		NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, "Items");
		NativeFieldInfoPtr_AmountPaid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, "AmountPaid");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_EContractParty_String_GameDateTime_Il2CppReferenceArray_1_StringIntPair_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, 100673746);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr, 100673747);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179219, XrefRangeEnd = 179222, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ContractReceipt(int receiptId, EContractParty completedBy, string customerID, GameDateTime completionTime, Il2CppReferenceArray<StringIntPair> items, float amountPaid)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&receiptId);
		*(EContractParty**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &completedBy;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(customerID);
		*(GameDateTime**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &completionTime;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)items);
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &amountPaid;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_EContractParty_String_GameDateTime_Il2CppReferenceArray_1_StringIntPair_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 179231, RefRangeEnd = 179232, XrefRangeStart = 179222, XrefRangeEnd = 179231, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ContractReceipt()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ContractReceipt>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ContractReceipt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
