using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Connection;
using Il2CppFishNet.Serializing;
using Il2CppFishNet.Transporting;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Product;
using Il2CppScheduleOne.Product.Packaging;

namespace Il2CppScheduleOne.Cartel;

public class CartelDealer : Dealer
{
	private static readonly IntPtr NativeFieldInfoPtr_DEALER_DEFEATED_INFLUENCE_CHANGE;

	private static readonly IntPtr NativeFieldInfoPtr_PRODUCT_COUNT_MIN;

	private static readonly IntPtr NativeFieldInfoPtr_PRODUCT_COUNT_MAX;

	private static readonly IntPtr NativeFieldInfoPtr_PRODUCT_QUANTITY_MIN;

	private static readonly IntPtr NativeFieldInfoPtr_PRODUCT_QUANTITY_MAX;

	private static readonly IntPtr NativeFieldInfoPtr__IsAcceptingDeals_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_RandomProducts;

	private static readonly IntPtr NativeFieldInfoPtr_ProductQuality;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultPackaging;

	private static readonly IntPtr NativeFieldInfoPtr_appearance;

	private static readonly IntPtr NativeFieldInfoPtr_field_Private_Boolean_0;

	private static readonly IntPtr NativeFieldInfoPtr_field_Private_Boolean_1;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsAcceptingDeals_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IsAcceptingDeals_Private_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_GoonPool_Private_get_GoonPool_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSpawnServer_Public_Virtual_Void_NetworkConnection_0;

	private static readonly IntPtr NativeMethodInfoPtr_RandomizeInventory_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RandomizeAppearance_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ConfigureGoonSettings_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetIsAcceptingDeals_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_CanCurrentlyAcceptDeal_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_DiedOrKnockedOut_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NetworkInitialize___Early_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NetworkInitialize__Late_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NetworkInitializeIfDisabled_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcWriter___Observers_ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcLogic___ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcReader___Observers_ConfigureGoonSettings_3427656873_Private_Void_PooledReader_Channel_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcWriter___Target_ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_RpcReader___Target_ConfigureGoonSettings_3427656873_Private_Void_PooledReader_Channel_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Public_Virtual_Void_0;

	public unsafe static float DEALER_DEFEATED_INFLUENCE_CHANGE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DEALER_DEFEATED_INFLUENCE_CHANGE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DEALER_DEFEATED_INFLUENCE_CHANGE, (void*)(&num));
		}
	}

	public unsafe static int PRODUCT_COUNT_MIN
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRODUCT_COUNT_MIN, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRODUCT_COUNT_MIN, (void*)(&num));
		}
	}

	public unsafe static int PRODUCT_COUNT_MAX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRODUCT_COUNT_MAX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRODUCT_COUNT_MAX, (void*)(&num));
		}
	}

	public unsafe static int PRODUCT_QUANTITY_MIN
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRODUCT_QUANTITY_MIN, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRODUCT_QUANTITY_MIN, (void*)(&num));
		}
	}

	public unsafe static int PRODUCT_QUANTITY_MAX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRODUCT_QUANTITY_MAX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRODUCT_QUANTITY_MAX, (void*)(&num));
		}
	}

	public unsafe bool _IsAcceptingDeals_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsAcceptingDeals_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsAcceptingDeals_k__BackingField)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<ProductDefinition> RandomProducts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomProducts);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ProductDefinition>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomProducts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe EQuality ProductQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuality);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuality)) = eQuality;
		}
	}

	public unsafe PackagingDefinition DefaultPackaging
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultPackaging);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PackagingDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultPackaging)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)packagingDefinition));
		}
	}

	public unsafe CartelGoonAppearance appearance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_appearance);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CartelGoonAppearance>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_appearance)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cartelGoonAppearance));
		}
	}

	public new unsafe bool field_Private_Boolean_0
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

	public new unsafe bool field_Private_Boolean_1
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

	public unsafe bool IsAcceptingDeals
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsAcceptingDeals_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsAcceptingDeals_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe GoonPool GoonPool
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 209269, RefRangeEnd = 209277, XrefRangeStart = 209265, XrefRangeEnd = 209269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GoonPool_Private_get_GoonPool_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GoonPool>(intPtr) : null;
		}
	}

	static CartelDealer()
	{
		Il2CppClassPointerStore<CartelDealer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Cartel", "CartelDealer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr);
		NativeFieldInfoPtr_DEALER_DEFEATED_INFLUENCE_CHANGE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "DEALER_DEFEATED_INFLUENCE_CHANGE");
		NativeFieldInfoPtr_PRODUCT_COUNT_MIN = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "PRODUCT_COUNT_MIN");
		NativeFieldInfoPtr_PRODUCT_COUNT_MAX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "PRODUCT_COUNT_MAX");
		NativeFieldInfoPtr_PRODUCT_QUANTITY_MIN = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "PRODUCT_QUANTITY_MIN");
		NativeFieldInfoPtr_PRODUCT_QUANTITY_MAX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "PRODUCT_QUANTITY_MAX");
		NativeFieldInfoPtr__IsAcceptingDeals_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "<IsAcceptingDeals>k__BackingField");
		NativeFieldInfoPtr_RandomProducts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "RandomProducts");
		NativeFieldInfoPtr_ProductQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "ProductQuality");
		NativeFieldInfoPtr_DefaultPackaging = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "DefaultPackaging");
		NativeFieldInfoPtr_appearance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "appearance");
		NativeFieldInfoPtr_field_Private_Boolean_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "NetworkInitialize___EarlyScheduleOne.Cartel.CartelDealerAssembly-CSharp.dll_Excuted");
		NativeFieldInfoPtr_field_Private_Boolean_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, "NetworkInitialize__LateScheduleOne.Cartel.CartelDealerAssembly-CSharp.dll_Excuted");
		NativeMethodInfoPtr_get_IsAcceptingDeals_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676346);
		NativeMethodInfoPtr_set_IsAcceptingDeals_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676347);
		NativeMethodInfoPtr_get_GoonPool_Private_get_GoonPool_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676348);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676349);
		NativeMethodInfoPtr_OnSpawnServer_Public_Virtual_Void_NetworkConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676350);
		NativeMethodInfoPtr_RandomizeInventory_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676351);
		NativeMethodInfoPtr_RandomizeAppearance_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676352);
		NativeMethodInfoPtr_ConfigureGoonSettings_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676353);
		NativeMethodInfoPtr_SetIsAcceptingDeals_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676354);
		NativeMethodInfoPtr_CanCurrentlyAcceptDeal_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676355);
		NativeMethodInfoPtr_DiedOrKnockedOut_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676356);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676357);
		NativeMethodInfoPtr_NetworkInitialize___Early_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676358);
		NativeMethodInfoPtr_NetworkInitialize__Late_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676359);
		NativeMethodInfoPtr_NetworkInitializeIfDisabled_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676360);
		NativeMethodInfoPtr_RpcWriter___Observers_ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676361);
		NativeMethodInfoPtr_RpcLogic___ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676362);
		NativeMethodInfoPtr_RpcReader___Observers_ConfigureGoonSettings_3427656873_Private_Void_PooledReader_Channel_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676363);
		NativeMethodInfoPtr_RpcWriter___Target_ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676364);
		NativeMethodInfoPtr_RpcReader___Target_ConfigureGoonSettings_3427656873_Private_Void_PooledReader_Channel_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676365);
		NativeMethodInfoPtr_Awake_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr, 100676366);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209277, XrefRangeEnd = 209286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209286, XrefRangeEnd = 209289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209289, XrefRangeEnd = 209298, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RandomizeInventory()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RandomizeInventory_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209298, XrefRangeEnd = 209304, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RandomizeAppearance()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RandomizeAppearance_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 209345, RefRangeEnd = 209348, XrefRangeStart = 209304, XrefRangeEnd = 209345, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConfigureGoonSettings(NetworkConnection conn, CartelGoonAppearance appearance, float moveSpeed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)appearance);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &moveSpeed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConfigureGoonSettings_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 209372, RefRangeEnd = 209374, XrefRangeStart = 209348, XrefRangeEnd = 209372, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetIsAcceptingDeals(bool accepting)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&accepting);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetIsAcceptingDeals_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 209375, RefRangeEnd = 209376, XrefRangeStart = 209374, XrefRangeEnd = 209375, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CanCurrentlyAcceptDeal()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanCurrentlyAcceptDeal_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209376, XrefRangeEnd = 209384, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DiedOrKnockedOut()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DiedOrKnockedOut_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209384, XrefRangeEnd = 209388, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelDealer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelDealer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209388, XrefRangeEnd = 209402, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void NetworkInitialize___Early()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NetworkInitialize___Early_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209402, XrefRangeEnd = 209403, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209403, XrefRangeEnd = 209414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcWriter___Observers_ConfigureGoonSettings_3427656873(NetworkConnection conn, CartelGoonAppearance appearance, float moveSpeed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)appearance);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &moveSpeed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcWriter___Observers_ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 209428, RefRangeEnd = 209431, XrefRangeStart = 209414, XrefRangeEnd = 209428, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcLogic___ConfigureGoonSettings_3427656873(NetworkConnection conn, CartelGoonAppearance appearance, float moveSpeed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)appearance);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &moveSpeed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcLogic___ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209431, XrefRangeEnd = 209436, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcReader___Observers_ConfigureGoonSettings_3427656873(PooledReader PooledReader0, Channel channel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)PooledReader0);
		*(Channel**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &channel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcReader___Observers_ConfigureGoonSettings_3427656873_Private_Void_PooledReader_Channel_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209436, XrefRangeEnd = 209447, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcWriter___Target_ConfigureGoonSettings_3427656873(NetworkConnection conn, CartelGoonAppearance appearance, float moveSpeed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)appearance);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &moveSpeed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcWriter___Target_ConfigureGoonSettings_3427656873_Private_Void_NetworkConnection_CartelGoonAppearance_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209447, XrefRangeEnd = 209452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RpcReader___Target_ConfigureGoonSettings_3427656873(PooledReader PooledReader0, Channel channel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)PooledReader0);
		*(Channel**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &channel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RpcReader___Target_ConfigureGoonSettings_3427656873_Private_Void_PooledReader_Channel_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 209452, XrefRangeEnd = 209453, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CartelDealer(IntPtr pointer)
		: base(pointer)
	{
	}
}
