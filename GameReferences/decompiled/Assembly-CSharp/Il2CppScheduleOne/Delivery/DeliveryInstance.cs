using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Property;
using Il2CppSystem;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Delivery;

[System.Serializable]
public class DeliveryInstance : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DeliveryID;

	private static readonly System.IntPtr NativeFieldInfoPtr_StoreName;

	private static readonly System.IntPtr NativeFieldInfoPtr_DestinationCode;

	private static readonly System.IntPtr NativeFieldInfoPtr_LoadingDockIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_Items;

	private static readonly System.IntPtr NativeFieldInfoPtr_Status;

	private static readonly System.IntPtr NativeFieldInfoPtr_TimeUntilArrival;

	private static readonly System.IntPtr NativeFieldInfoPtr__ActiveVehicle_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_onDeliveryCompleted;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ActiveVehicle_Public_get_DeliveryVehicle_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ActiveVehicle_Private_set_Void_DeliveryVehicle_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Destination_Public_get_Property_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LoadingDock_Public_get_LoadingDock_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_String_Int32_Il2CppReferenceArray_1_StringIntPair_EDeliveryStatus_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTimeStatus_Public_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetStatus_Public_Void_EDeliveryStatus_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddItemsToDeliveryVehicle_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetReceipt_Public_DeliveryReceipt_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnTimePass_Public_Void_Int32_0;

	public unsafe string DeliveryID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string StoreName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StoreName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StoreName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string DestinationCode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationCode);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationCode)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int LoadingDockIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LoadingDockIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LoadingDockIndex)) = num;
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

	public unsafe EDeliveryStatus Status
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Status);
			return *(EDeliveryStatus*)num;
		}
		set
		{
			*(EDeliveryStatus*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Status)) = eDeliveryStatus;
		}
	}

	public unsafe int TimeUntilArrival
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeUntilArrival);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeUntilArrival)) = num;
		}
	}

	public unsafe DeliveryVehicle _ActiveVehicle_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ActiveVehicle_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DeliveryVehicle>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ActiveVehicle_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deliveryVehicle));
		}
	}

	public unsafe UnityEvent onDeliveryCompleted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onDeliveryCompleted);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onDeliveryCompleted)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe DeliveryVehicle ActiveVehicle
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30474, RefRangeEnd = 30475, XrefRangeStart = 30474, XrefRangeEnd = 30475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ActiveVehicle_Public_get_DeliveryVehicle_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DeliveryVehicle>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ActiveVehicle_Private_set_Void_DeliveryVehicle_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppScheduleOne.Property.Property Destination
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 198466, RefRangeEnd = 198473, XrefRangeStart = 198460, XrefRangeEnd = 198466, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Destination_Public_get_Property_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppScheduleOne.Property.Property>(intPtr) : null;
		}
	}

	public unsafe LoadingDock LoadingDock
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 198473, XrefRangeEnd = 198474, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LoadingDock_Public_get_LoadingDock_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<LoadingDock>(intPtr) : null;
		}
	}

	static DeliveryInstance()
	{
		Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Delivery", "DeliveryInstance");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr);
		NativeFieldInfoPtr_DeliveryID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "DeliveryID");
		NativeFieldInfoPtr_StoreName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "StoreName");
		NativeFieldInfoPtr_DestinationCode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "DestinationCode");
		NativeFieldInfoPtr_LoadingDockIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "LoadingDockIndex");
		NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "Items");
		NativeFieldInfoPtr_Status = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "Status");
		NativeFieldInfoPtr_TimeUntilArrival = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "TimeUntilArrival");
		NativeFieldInfoPtr__ActiveVehicle_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "<ActiveVehicle>k__BackingField");
		NativeFieldInfoPtr_onDeliveryCompleted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, "onDeliveryCompleted");
		NativeMethodInfoPtr_get_ActiveVehicle_Public_get_DeliveryVehicle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675355);
		NativeMethodInfoPtr_set_ActiveVehicle_Private_set_Void_DeliveryVehicle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675356);
		NativeMethodInfoPtr_get_Destination_Public_get_Property_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675357);
		NativeMethodInfoPtr_get_LoadingDock_Public_get_LoadingDock_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675358);
		NativeMethodInfoPtr__ctor_Public_Void_String_String_String_Int32_Il2CppReferenceArray_1_StringIntPair_EDeliveryStatus_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675359);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675360);
		NativeMethodInfoPtr_GetTimeStatus_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675361);
		NativeMethodInfoPtr_SetStatus_Public_Void_EDeliveryStatus_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675362);
		NativeMethodInfoPtr_AddItemsToDeliveryVehicle_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675363);
		NativeMethodInfoPtr_GetReceipt_Public_DeliveryReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675364);
		NativeMethodInfoPtr_OnTimePass_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr, 100675365);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 198479, RefRangeEnd = 198480, XrefRangeStart = 198474, XrefRangeEnd = 198479, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeliveryInstance(string deliveryID, string storeName, string destinationCode, int loadingDockIndex, Il2CppReferenceArray<StringIntPair> items, EDeliveryStatus status, int timeUntilArrival)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(deliveryID);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(storeName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(destinationCode);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadingDockIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)items);
		*(EDeliveryStatus**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &status;
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &timeUntilArrival;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_String_Int32_Il2CppReferenceArray_1_StringIntPair_EDeliveryStatus_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeliveryInstance()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeliveryInstance>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 198480, RefRangeEnd = 198481, XrefRangeStart = 198480, XrefRangeEnd = 198480, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetTimeStatus()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTimeStatus_Public_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 198523, RefRangeEnd = 198525, XrefRangeStart = 198481, XrefRangeEnd = 198523, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetStatus(EDeliveryStatus status)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&status);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStatus_Public_Void_EDeliveryStatus_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 198534, RefRangeEnd = 198535, XrefRangeStart = 198525, XrefRangeEnd = 198534, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddItemsToDeliveryVehicle()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddItemsToDeliveryVehicle_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 198543, RefRangeEnd = 198544, XrefRangeStart = 198535, XrefRangeEnd = 198543, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeliveryReceipt GetReceipt()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetReceipt_Public_DeliveryReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DeliveryReceipt>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe void OnTimePass(int minutes)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&minutes);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnTimePass_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeliveryInstance(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
