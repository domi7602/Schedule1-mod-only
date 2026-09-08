using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.StationFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.ObjectScripts;

[System.Serializable]
public class OvenCookOperation : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__itemDefinition;

	private static readonly System.IntPtr NativeFieldInfoPtr__productionDefinition;

	private static readonly System.IntPtr NativeFieldInfoPtr__cookable;

	private static readonly System.IntPtr NativeFieldInfoPtr_IngredientID;

	private static readonly System.IntPtr NativeFieldInfoPtr_IngredientQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_IngredientQuantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_ProductID;

	private static readonly System.IntPtr NativeFieldInfoPtr_CookProgress;

	private static readonly System.IntPtr NativeFieldInfoPtr_cookDuration;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Ingredient_Public_get_StorableItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Product_Public_get_StorableItemDefinition_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Cookable_Public_get_CookableModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Int32_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateCookProgress_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCookDuration_Public_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsComplete_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetProductItem_Public_ItemInstance_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsReady_Public_Boolean_0;

	public unsafe StorableItemDefinition _itemDefinition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemDefinition);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorableItemDefinition>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemDefinition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storableItemDefinition));
		}
	}

	public unsafe StorableItemDefinition _productionDefinition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__productionDefinition);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorableItemDefinition>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__productionDefinition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storableItemDefinition));
		}
	}

	public unsafe CookableModule _cookable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cookable);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CookableModule>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cookable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cookableModule));
		}
	}

	public unsafe string IngredientID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IngredientID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IngredientID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EQuality IngredientQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IngredientQuality);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IngredientQuality)) = eQuality;
		}
	}

	public unsafe int IngredientQuantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IngredientQuantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IngredientQuantity)) = num;
		}
	}

	public unsafe string ProductID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int CookProgress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookProgress);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookProgress)) = num;
		}
	}

	public unsafe int cookDuration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cookDuration);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cookDuration)) = num;
		}
	}

	public unsafe StorableItemDefinition Ingredient
	{
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 255535, RefRangeEnd = 255544, XrefRangeStart = 255525, XrefRangeEnd = 255535, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Ingredient_Public_get_StorableItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorableItemDefinition>(intPtr) : null;
		}
	}

	public unsafe StorableItemDefinition Product
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 255554, RefRangeEnd = 255558, XrefRangeStart = 255544, XrefRangeEnd = 255554, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Product_Public_get_StorableItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorableItemDefinition>(intPtr) : null;
		}
	}

	public unsafe CookableModule Cookable
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 255567, RefRangeEnd = 255573, XrefRangeStart = 255558, XrefRangeEnd = 255567, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Cookable_Public_get_CookableModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CookableModule>(intPtr) : null;
		}
	}

	static OvenCookOperation()
	{
		Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ObjectScripts", "OvenCookOperation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr);
		NativeFieldInfoPtr__itemDefinition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "_itemDefinition");
		NativeFieldInfoPtr__productionDefinition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "_productionDefinition");
		NativeFieldInfoPtr__cookable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "_cookable");
		NativeFieldInfoPtr_IngredientID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "IngredientID");
		NativeFieldInfoPtr_IngredientQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "IngredientQuality");
		NativeFieldInfoPtr_IngredientQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "IngredientQuantity");
		NativeFieldInfoPtr_ProductID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "ProductID");
		NativeFieldInfoPtr_CookProgress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "CookProgress");
		NativeFieldInfoPtr_cookDuration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, "cookDuration");
		NativeMethodInfoPtr_get_Ingredient_Public_get_StorableItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100680994);
		NativeMethodInfoPtr_get_Product_Public_get_StorableItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100680995);
		NativeMethodInfoPtr_get_Cookable_Public_get_CookableModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100680996);
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100680997);
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Int32_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100680998);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100680999);
		NativeMethodInfoPtr_UpdateCookProgress_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100681000);
		NativeMethodInfoPtr_GetCookDuration_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100681001);
		NativeMethodInfoPtr_IsComplete_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100681002);
		NativeMethodInfoPtr_GetProductItem_Public_ItemInstance_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100681003);
		NativeMethodInfoPtr_IsReady_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr, 100681004);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 255576, RefRangeEnd = 255579, XrefRangeStart = 255573, XrefRangeEnd = 255576, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OvenCookOperation(string ingredientID, EQuality ingredientQuality, int ingredientQuantity, string productID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(ingredientID);
		*(EQuality**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &ingredientQuality;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &ingredientQuantity;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(productID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 255582, RefRangeEnd = 255584, XrefRangeStart = 255579, XrefRangeEnd = 255582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OvenCookOperation(string ingredientID, EQuality ingredientQuality, int ingredientQuantity, string productID, int progress)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(ingredientID);
		*(EQuality**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &ingredientQuality;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &ingredientQuantity;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(productID);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &progress;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Int32_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 255585, RefRangeEnd = 255586, XrefRangeStart = 255584, XrefRangeEnd = 255585, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OvenCookOperation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OvenCookOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void UpdateCookProgress(int change)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&change);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateCookProgress_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 255590, RefRangeEnd = 255594, XrefRangeStart = 255586, XrefRangeEnd = 255590, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetCookDuration()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCookDuration_Public_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 255598, RefRangeEnd = 255603, XrefRangeStart = 255594, XrefRangeEnd = 255598, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsComplete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsComplete_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 255617, RefRangeEnd = 255620, XrefRangeStart = 255603, XrefRangeEnd = 255617, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemInstance GetProductItem(int quantity)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&quantity);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetProductItem_Public_ItemInstance_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 255598, RefRangeEnd = 255603, XrefRangeStart = 255598, XrefRangeEnd = 255603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsReady()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsReady_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public OvenCookOperation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
