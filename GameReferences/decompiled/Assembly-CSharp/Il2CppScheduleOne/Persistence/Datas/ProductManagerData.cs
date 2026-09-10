using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Product;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class ProductManagerData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_DiscoveredProducts;

	private static readonly IntPtr NativeFieldInfoPtr_ListedProducts;

	private static readonly IntPtr NativeFieldInfoPtr_ActiveMixOperation;

	private static readonly IntPtr NativeFieldInfoPtr_IsMixComplete;

	private static readonly IntPtr NativeFieldInfoPtr_MixRecipes;

	private static readonly IntPtr NativeFieldInfoPtr_ProductPrices;

	private static readonly IntPtr NativeFieldInfoPtr_FavouritedProducts;

	private static readonly IntPtr NativeFieldInfoPtr_CreatedWeed;

	private static readonly IntPtr NativeFieldInfoPtr_CreatedMeth;

	private static readonly IntPtr NativeFieldInfoPtr_CreatedCocaine;

	private static readonly IntPtr NativeFieldInfoPtr_CreatedShrooms;

	private static readonly IntPtr NativeFieldInfoPtr_ContractReceipts;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppStringArray_Il2CppStringArray_NewMixOperation_Boolean_Il2CppReferenceArray_1_MixRecipeData_Il2CppReferenceArray_1_StringIntPair_Il2CppStringArray_Il2CppReferenceArray_1_WeedProductData_Il2CppReferenceArray_1_MethProductData_Il2CppReferenceArray_1_CocaineProductData_Il2CppReferenceArray_1_ShroomProductData_Il2CppReferenceArray_1_ContractReceipt_0;

	public unsafe Il2CppStringArray DiscoveredProducts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DiscoveredProducts);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DiscoveredProducts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStringArray ListedProducts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ListedProducts);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ListedProducts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe NewMixOperation ActiveMixOperation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveMixOperation);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NewMixOperation>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveMixOperation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newMixOperation));
		}
	}

	public unsafe bool IsMixComplete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsMixComplete);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsMixComplete)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<MixRecipeData> MixRecipes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MixRecipes);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<MixRecipeData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MixRecipes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<StringIntPair> ProductPrices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductPrices);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<StringIntPair>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductPrices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStringArray FavouritedProducts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FavouritedProducts);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FavouritedProducts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<WeedProductData> CreatedWeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedWeed);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<WeedProductData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedWeed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<MethProductData> CreatedMeth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedMeth);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<MethProductData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedMeth)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<CocaineProductData> CreatedCocaine
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedCocaine);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CocaineProductData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedCocaine)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<ShroomProductData> CreatedShrooms
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedShrooms);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ShroomProductData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreatedShrooms)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<ContractReceipt> ContractReceipts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ContractReceipts);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ContractReceipt>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ContractReceipts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ProductManagerData()
	{
		Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ProductManagerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr);
		NativeFieldInfoPtr_DiscoveredProducts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "DiscoveredProducts");
		NativeFieldInfoPtr_ListedProducts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "ListedProducts");
		NativeFieldInfoPtr_ActiveMixOperation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "ActiveMixOperation");
		NativeFieldInfoPtr_IsMixComplete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "IsMixComplete");
		NativeFieldInfoPtr_MixRecipes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "MixRecipes");
		NativeFieldInfoPtr_ProductPrices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "ProductPrices");
		NativeFieldInfoPtr_FavouritedProducts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "FavouritedProducts");
		NativeFieldInfoPtr_CreatedWeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "CreatedWeed");
		NativeFieldInfoPtr_CreatedMeth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "CreatedMeth");
		NativeFieldInfoPtr_CreatedCocaine = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "CreatedCocaine");
		NativeFieldInfoPtr_CreatedShrooms = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "CreatedShrooms");
		NativeFieldInfoPtr_ContractReceipts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, "ContractReceipts");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppStringArray_Il2CppStringArray_NewMixOperation_Boolean_Il2CppReferenceArray_1_MixRecipeData_Il2CppReferenceArray_1_StringIntPair_Il2CppStringArray_Il2CppReferenceArray_1_WeedProductData_Il2CppReferenceArray_1_MethProductData_Il2CppReferenceArray_1_CocaineProductData_Il2CppReferenceArray_1_ShroomProductData_Il2CppReferenceArray_1_ContractReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr, 100669452);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135254, RefRangeEnd = 135255, XrefRangeStart = 135242, XrefRangeEnd = 135254, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ProductManagerData(Il2CppStringArray discoveredProducts, Il2CppStringArray listedProducts, NewMixOperation activeOperation, bool isMixComplete, Il2CppReferenceArray<MixRecipeData> mixRecipes, Il2CppReferenceArray<StringIntPair> productPrices, Il2CppStringArray favouritedProducts, Il2CppReferenceArray<WeedProductData> createdWeed, Il2CppReferenceArray<MethProductData> createdMeth, Il2CppReferenceArray<CocaineProductData> createdCocaine, Il2CppReferenceArray<ShroomProductData> createdShrooms, Il2CppReferenceArray<ContractReceipt> receipts)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ProductManagerData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[12];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)discoveredProducts);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listedProducts);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeOperation);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &isMixComplete;
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mixRecipes);
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)productPrices);
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)favouritedProducts);
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)createdWeed);
		*(IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)createdMeth);
		*(IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)createdCocaine);
		*(IntPtr*)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)createdShrooms);
		*(IntPtr*)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)receipts);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppStringArray_Il2CppStringArray_NewMixOperation_Boolean_Il2CppReferenceArray_1_MixRecipeData_Il2CppReferenceArray_1_StringIntPair_Il2CppStringArray_Il2CppReferenceArray_1_WeedProductData_Il2CppReferenceArray_1_MethProductData_Il2CppReferenceArray_1_CocaineProductData_Il2CppReferenceArray_1_ShroomProductData_Il2CppReferenceArray_1_ContractReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ProductManagerData(IntPtr pointer)
		: base(pointer)
	{
	}
}
