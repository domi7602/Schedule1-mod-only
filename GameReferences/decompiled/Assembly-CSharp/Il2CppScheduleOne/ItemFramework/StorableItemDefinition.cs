using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Levelling;
using Il2CppScheduleOne.StationFramework;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.UI.Shop;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.ItemFramework;

[Serializable]
public class StorableItemDefinition : ItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr_BasePurchasePrice;

	private static readonly IntPtr NativeFieldInfoPtr_ShopCategories;

	private static readonly IntPtr NativeFieldInfoPtr_RequiresLevelToPurchase;

	private static readonly IntPtr NativeFieldInfoPtr_RequiredRank;

	private static readonly IntPtr NativeFieldInfoPtr_ResellMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_StoredItem;

	private static readonly IntPtr NativeFieldInfoPtr_PickpocketDifficultyMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_StationItem;

	private static readonly IntPtr NativeFieldInfoPtr_CombatUtility;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsUnlocked_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetIsUnlocked_Protected_Virtual_New_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float BasePurchasePrice
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BasePurchasePrice);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BasePurchasePrice)) = num;
		}
	}

	public unsafe List<ShopListing.CategoryInstance> ShopCategories
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShopCategories);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<ShopListing.CategoryInstance>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShopCategories)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool RequiresLevelToPurchase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiresLevelToPurchase);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiresLevelToPurchase)) = flag;
		}
	}

	public unsafe FullRank RequiredRank
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiredRank);
			return *(FullRank*)num;
		}
		set
		{
			*(FullRank*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiredRank)) = fullRank;
		}
	}

	public unsafe float ResellMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ResellMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ResellMultiplier)) = num;
		}
	}

	public unsafe StoredItem StoredItem
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StoredItem);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StoredItem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StoredItem)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storedItem));
		}
	}

	public unsafe float PickpocketDifficultyMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickpocketDifficultyMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickpocketDifficultyMultiplier)) = num;
		}
	}

	public unsafe StationItem StationItem
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StationItem);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StationItem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StationItem)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stationItem));
		}
	}

	public unsafe float CombatUtility
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CombatUtility);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CombatUtility)) = num;
		}
	}

	public unsafe bool IsUnlocked
	{
		[CallerCount(37)]
		[CachedScanResults(RefRangeStart = 142897, RefRangeEnd = 142934, XrefRangeStart = 142897, XrefRangeEnd = 142934, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsUnlocked_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static StorableItemDefinition()
	{
		Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "StorableItemDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_BasePurchasePrice = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "BasePurchasePrice");
		NativeFieldInfoPtr_ShopCategories = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "ShopCategories");
		NativeFieldInfoPtr_RequiresLevelToPurchase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "RequiresLevelToPurchase");
		NativeFieldInfoPtr_RequiredRank = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "RequiredRank");
		NativeFieldInfoPtr_ResellMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "ResellMultiplier");
		NativeFieldInfoPtr_StoredItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "StoredItem");
		NativeFieldInfoPtr_PickpocketDifficultyMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "PickpocketDifficultyMultiplier");
		NativeFieldInfoPtr_StationItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "StationItem");
		NativeFieldInfoPtr_CombatUtility = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, "CombatUtility");
		NativeMethodInfoPtr_get_IsUnlocked_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, 100672668);
		NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, 100672669);
		NativeMethodInfoPtr_GetIsUnlocked_Protected_Virtual_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, 100672670);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr, 100672671);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 168832, XrefRangeEnd = 168836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override ItemInstance GetDefaultInstance(int quantity = 1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&quantity);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 168836, XrefRangeEnd = 168837, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool GetIsUnlocked()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetIsUnlocked_Protected_Virtual_New_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 168845, RefRangeEnd = 168861, XrefRangeStart = 168837, XrefRangeEnd = 168845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StorableItemDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StorableItemDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StorableItemDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
