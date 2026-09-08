using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Storage;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.StationFramework;

[System.Serializable]
public class StationRecipe : ScriptableObject
{
	[OriginalName("Assembly-CSharp.dll", "", "EQualityCalculationMethod")]
	public enum EQualityCalculationMethod
	{
		Additive
	}

	[System.Serializable]
	public class ItemQuantity : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Item;

		private static readonly System.IntPtr NativeFieldInfoPtr_Quantity;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe ItemDefinition Item
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Item);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemDefinition>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Item)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemDefinition));
			}
		}

		public unsafe int Quantity
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quantity);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quantity)) = num;
			}
		}

		static ItemQuantity()
		{
			Il2CppClassPointerStore<ItemQuantity>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "ItemQuantity");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemQuantity>.NativeClassPtr);
			NativeFieldInfoPtr_Item = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuantity>.NativeClassPtr, "Item");
			NativeFieldInfoPtr_Quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemQuantity>.NativeClassPtr, "Quantity");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemQuantity>.NativeClassPtr, 100679178);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235742, XrefRangeEnd = 235743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ItemQuantity()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemQuantity>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ItemQuantity(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	public class IngredientQuantity : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Items;

		private static readonly System.IntPtr NativeFieldInfoPtr_Quantity;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_ItemDefinition_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe List<ItemDefinition> Items
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemDefinition>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe int Quantity
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quantity);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quantity)) = num;
			}
		}

		public unsafe ItemDefinition Item
		{
			[CallerCount(9)]
			[CachedScanResults(RefRangeStart = 235746, RefRangeEnd = 235755, XrefRangeStart = 235743, XrefRangeEnd = 235746, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_ItemDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemDefinition>(intPtr) : null;
			}
		}

		static IngredientQuantity()
		{
			Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "IngredientQuantity");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr);
			NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr, "Items");
			NativeFieldInfoPtr_Quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr, "Quantity");
			NativeMethodInfoPtr_get_Item_Public_get_ItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr, 100679179);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr, 100679180);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 235763, RefRangeEnd = 235765, XrefRangeStart = 235755, XrefRangeEnd = 235763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IngredientQuantity()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IngredientQuantity>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public IngredientQuantity(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.StationFramework.StationRecipe+<>c__DisplayClass21_0")]
	public sealed class __c__DisplayClass21_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ingredientVariant;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__DoIngredientsSuffice_b__0_Internal_Boolean_ItemInstance_0;

		public unsafe ItemDefinition ingredientVariant
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ingredientVariant);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemDefinition>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ingredientVariant)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemDefinition));
			}
		}

		static __c__DisplayClass21_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass21_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "<>c__DisplayClass21_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass21_0>.NativeClassPtr);
			NativeFieldInfoPtr_ingredientVariant = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass21_0>.NativeClassPtr, "ingredientVariant");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass21_0>.NativeClassPtr, 100679181);
			NativeMethodInfoPtr__DoIngredientsSuffice_b__0_Internal_Boolean_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass21_0>.NativeClassPtr, 100679182);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass21_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass21_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235765, XrefRangeEnd = 235768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _DoIngredientsSuffice_b__0(ItemInstance x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__DoIngredientsSuffice_b__0_Internal_Boolean_ItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass21_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_IsDiscovered;

	private static readonly System.IntPtr NativeFieldInfoPtr_RecipeTitle;

	private static readonly System.IntPtr NativeFieldInfoPtr_Unlocked;

	private static readonly System.IntPtr NativeFieldInfoPtr_Ingredients;

	private static readonly System.IntPtr NativeFieldInfoPtr_Product;

	private static readonly System.IntPtr NativeFieldInfoPtr_FinalLiquidColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_CookTime_Mins;

	private static readonly System.IntPtr NativeFieldInfoPtr_CookTemperature;

	private static readonly System.IntPtr NativeFieldInfoPtr_CookTemperatureTolerance;

	private static readonly System.IntPtr NativeFieldInfoPtr_QualityCalculationMethod;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CookTemperatureLowerBound_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CookTemperatureUpperBound_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RecipeID_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetProductInstance_Public_StorableItemInstance_List_1_ItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetProductInstance_Public_StorableItemInstance_EQuality_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoIngredientsSuffice_Public_Boolean_List_1_ItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateQuality_Public_EQuality_List_1_ItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool IsDiscovered
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsDiscovered);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsDiscovered)) = flag;
		}
	}

	public unsafe string RecipeTitle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RecipeTitle);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RecipeTitle)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool Unlocked
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Unlocked);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Unlocked)) = flag;
		}
	}

	public unsafe List<IngredientQuantity> Ingredients
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Ingredients);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<IngredientQuantity>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Ingredients)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe ItemQuantity Product
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemQuantity>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemQuantity));
		}
	}

	public unsafe Color FinalLiquidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FinalLiquidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FinalLiquidColor)) = color;
		}
	}

	public unsafe int CookTime_Mins
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTime_Mins);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTime_Mins)) = num;
		}
	}

	public unsafe float CookTemperature
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTemperature);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTemperature)) = num;
		}
	}

	public unsafe float CookTemperatureTolerance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTemperatureTolerance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTemperatureTolerance)) = num;
		}
	}

	public unsafe EQualityCalculationMethod QualityCalculationMethod
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QualityCalculationMethod);
			return *(EQualityCalculationMethod*)num;
		}
		set
		{
			*(EQualityCalculationMethod*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QualityCalculationMethod)) = eQualityCalculationMethod;
		}
	}

	public unsafe float CookTemperatureLowerBound
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 235768, RefRangeEnd = 235770, XrefRangeStart = 235768, XrefRangeEnd = 235768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CookTemperatureLowerBound_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float CookTemperatureUpperBound
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 235770, RefRangeEnd = 235775, XrefRangeStart = 235770, XrefRangeEnd = 235770, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CookTemperatureUpperBound_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe string RecipeID
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 235780, RefRangeEnd = 235784, XrefRangeStart = 235775, XrefRangeEnd = 235780, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RecipeID_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static StationRecipe()
	{
		Il2CppClassPointerStore<StationRecipe>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.StationFramework", "StationRecipe");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr);
		NativeFieldInfoPtr_IsDiscovered = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "IsDiscovered");
		NativeFieldInfoPtr_RecipeTitle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "RecipeTitle");
		NativeFieldInfoPtr_Unlocked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "Unlocked");
		NativeFieldInfoPtr_Ingredients = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "Ingredients");
		NativeFieldInfoPtr_Product = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "Product");
		NativeFieldInfoPtr_FinalLiquidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "FinalLiquidColor");
		NativeFieldInfoPtr_CookTime_Mins = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "CookTime_Mins");
		NativeFieldInfoPtr_CookTemperature = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "CookTemperature");
		NativeFieldInfoPtr_CookTemperatureTolerance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "CookTemperatureTolerance");
		NativeFieldInfoPtr_QualityCalculationMethod = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, "QualityCalculationMethod");
		NativeMethodInfoPtr_get_CookTemperatureLowerBound_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679170);
		NativeMethodInfoPtr_get_CookTemperatureUpperBound_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679171);
		NativeMethodInfoPtr_get_RecipeID_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679172);
		NativeMethodInfoPtr_GetProductInstance_Public_StorableItemInstance_List_1_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679173);
		NativeMethodInfoPtr_GetProductInstance_Public_StorableItemInstance_EQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679174);
		NativeMethodInfoPtr_DoIngredientsSuffice_Public_Boolean_List_1_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679175);
		NativeMethodInfoPtr_CalculateQuality_Public_EQuality_List_1_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679176);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr, 100679177);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 235790, RefRangeEnd = 235792, XrefRangeStart = 235784, XrefRangeEnd = 235790, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StorableItemInstance GetProductInstance(List<ItemInstance> ingredients)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ingredients);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetProductInstance_Public_StorableItemInstance_List_1_ItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorableItemInstance>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 235796, RefRangeEnd = 235797, XrefRangeStart = 235792, XrefRangeEnd = 235796, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StorableItemInstance GetProductInstance(EQuality quality)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&quality);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetProductInstance_Public_StorableItemInstance_EQuality_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StorableItemInstance>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 235849, RefRangeEnd = 235850, XrefRangeStart = 235797, XrefRangeEnd = 235849, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool DoIngredientsSuffice(List<ItemInstance> ingredients)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ingredients);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoIngredientsSuffice_Public_Boolean_List_1_ItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 235863, RefRangeEnd = 235866, XrefRangeStart = 235850, XrefRangeEnd = 235863, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EQuality CalculateQuality(List<ItemInstance> ingredients)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ingredients);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateQuality_Public_EQuality_List_1_ItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235866, XrefRangeEnd = 235874, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StationRecipe()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StationRecipe>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StationRecipe(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
