using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.GameTime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Cartel;

[System.Serializable]
public class CartelDealInfo : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EStatus")]
	public enum EStatus
	{
		Pending,
		Overdue
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_RequestedProductID;

	private static readonly System.IntPtr NativeFieldInfoPtr_RequestedProductQuantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_PaymentAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr_DueTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_Status;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_GameDateTime_EStatus_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsValid_Public_Boolean_0;

	public unsafe string RequestedProductID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequestedProductID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequestedProductID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int RequestedProductQuantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequestedProductQuantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequestedProductQuantity)) = num;
		}
	}

	public unsafe int PaymentAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PaymentAmount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PaymentAmount)) = num;
		}
	}

	public unsafe GameDateTime DueTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DueTime);
			return *(GameDateTime*)num;
		}
		set
		{
			*(GameDateTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DueTime)) = gameDateTime;
		}
	}

	public unsafe EStatus Status
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Status);
			return *(EStatus*)num;
		}
		set
		{
			*(EStatus*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Status)) = eStatus;
		}
	}

	static CartelDealInfo()
	{
		Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Cartel", "CartelDealInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr);
		NativeFieldInfoPtr_RequestedProductID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, "RequestedProductID");
		NativeFieldInfoPtr_RequestedProductQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, "RequestedProductQuantity");
		NativeFieldInfoPtr_PaymentAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, "PaymentAmount");
		NativeFieldInfoPtr_DueTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, "DueTime");
		NativeFieldInfoPtr_Status = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, "Status");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_GameDateTime_EStatus_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, 100676367);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, 100676368);
		NativeMethodInfoPtr_IsValid_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr, 100676369);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209453, XrefRangeEnd = 209455, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelDealInfo(string requestedProductID, int requestedProductQuantity, int payment, GameDateTime dueTime, EStatus status)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(requestedProductID);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &requestedProductQuantity;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &payment;
		*(GameDateTime**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &dueTime;
		*(EStatus**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &status;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_GameDateTime_EStatus_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 209459, RefRangeEnd = 209460, XrefRangeStart = 209455, XrefRangeEnd = 209459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelDealInfo()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelDealInfo>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209460, XrefRangeEnd = 209465, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsValid()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsValid_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CartelDealInfo(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
