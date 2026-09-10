using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.UI.Phone;

namespace Il2CppScheduleOne.NPCs.Framework;

[Serializable]
public class SupplierNPCData : NPCData
{
	private static readonly IntPtr NativeFieldInfoPtr_MinimumDeaddropOrderLimit;

	private static readonly IntPtr NativeFieldInfoPtr_MaximumDeaddropOrderLimit;

	private static readonly IntPtr NativeFieldInfoPtr_DeliveryShopListings;

	private static readonly IntPtr NativeFieldInfoPtr_SupplierRecommendMessage;

	private static readonly IntPtr NativeFieldInfoPtr_SupplierUnlockHint;

	private static readonly IntPtr NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_NPCData_0;

	private static readonly IntPtr NativeMethodInfoPtr_PopulateSupplierData_Private_Void_SupplierNPCData_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float MinimumDeaddropOrderLimit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinimumDeaddropOrderLimit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinimumDeaddropOrderLimit)) = num;
		}
	}

	public unsafe float MaximumDeaddropOrderLimit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaximumDeaddropOrderLimit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaximumDeaddropOrderLimit)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<PhoneShopInterface.Listing> DeliveryShopListings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryShopListings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<PhoneShopInterface.Listing>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeliveryShopListings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe string SupplierRecommendMessage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SupplierRecommendMessage);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SupplierRecommendMessage)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string SupplierUnlockHint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SupplierUnlockHint);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SupplierUnlockHint)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static SupplierNPCData()
	{
		Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "SupplierNPCData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr);
		NativeFieldInfoPtr_MinimumDeaddropOrderLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, "MinimumDeaddropOrderLimit");
		NativeFieldInfoPtr_MaximumDeaddropOrderLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, "MaximumDeaddropOrderLimit");
		NativeFieldInfoPtr_DeliveryShopListings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, "DeliveryShopListings");
		NativeFieldInfoPtr_SupplierRecommendMessage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, "SupplierRecommendMessage");
		NativeFieldInfoPtr_SupplierUnlockHint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, "SupplierUnlockHint");
		NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, 100682867);
		NativeMethodInfoPtr_PopulateSupplierData_Private_Void_SupplierNPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, 100682868);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr, 100682869);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272819, XrefRangeEnd = 272833, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override NPCData GetDeepCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_NPCData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NPCData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272833, XrefRangeEnd = 272847, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PopulateSupplierData(SupplierNPCData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PopulateSupplierData_Private_Void_SupplierNPCData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 272856, RefRangeEnd = 272858, XrefRangeStart = 272847, XrefRangeEnd = 272856, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SupplierNPCData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SupplierNPCData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SupplierNPCData(IntPtr pointer)
		: base(pointer)
	{
	}
}
