using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Delivery;

namespace Il2CppScheduleOne.Persistence.Datas;

public class DeliveriesData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_ActiveDeliveries;

	private static readonly IntPtr NativeFieldInfoPtr_DeliveryVehicles;

	private static readonly IntPtr NativeFieldInfoPtr_DeliveryHistory;

	private static readonly IntPtr NativeFieldInfoPtr_DisplayedDeliveryHistory;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_DeliveryInstance_Il2CppReferenceArray_1_VehicleData_Il2CppReferenceArray_1_DeliveryReceipt_Il2CppReferenceArray_1_DeliveryReceipt_0;

	public unsafe Il2CppReferenceArray<DeliveryInstance> ActiveDeliveries
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveDeliveries);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<DeliveryInstance>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveDeliveries)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<VehicleData> DeliveryVehicles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryVehicles);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<VehicleData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryVehicles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<DeliveryReceipt> DeliveryHistory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryHistory);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<DeliveryReceipt>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryHistory)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<DeliveryReceipt> DisplayedDeliveryHistory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplayedDeliveryHistory);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<DeliveryReceipt>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplayedDeliveryHistory)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static DeliveriesData()
	{
		Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "DeliveriesData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr);
		NativeFieldInfoPtr_ActiveDeliveries = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr, "ActiveDeliveries");
		NativeFieldInfoPtr_DeliveryVehicles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr, "DeliveryVehicles");
		NativeFieldInfoPtr_DeliveryHistory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr, "DeliveryHistory");
		NativeFieldInfoPtr_DisplayedDeliveryHistory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr, "DisplayedDeliveryHistory");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_DeliveryInstance_Il2CppReferenceArray_1_VehicleData_Il2CppReferenceArray_1_DeliveryReceipt_Il2CppReferenceArray_1_DeliveryReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr, 100669321);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134371, RefRangeEnd = 134373, XrefRangeStart = 134366, XrefRangeEnd = 134371, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeliveriesData(Il2CppReferenceArray<DeliveryInstance> deliveries, Il2CppReferenceArray<VehicleData> deliveryVehicles, Il2CppReferenceArray<DeliveryReceipt> deliveryHistory, Il2CppReferenceArray<DeliveryReceipt> displayedDeliveryHistory)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeliveriesData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deliveries);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deliveryVehicles);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deliveryHistory);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)displayedDeliveryHistory);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_DeliveryInstance_Il2CppReferenceArray_1_VehicleData_Il2CppReferenceArray_1_DeliveryReceipt_Il2CppReferenceArray_1_DeliveryReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeliveriesData(IntPtr pointer)
		: base(pointer)
	{
	}
}
