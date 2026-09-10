using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.AvatarFramework.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Inventory : Il2CppSystem.Object
{
	[System.Serializable]
	public class WeightedItem : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Item;

		private static readonly System.IntPtr NativeFieldInfoPtr_Weight;

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

		public unsafe float Weight
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Weight);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Weight)) = num;
			}
		}

		static WeightedItem()
		{
			Il2CppClassPointerStore<WeightedItem>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "WeightedItem");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WeightedItem>.NativeClassPtr);
			NativeFieldInfoPtr_Item = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeightedItem>.NativeClassPtr, "Item");
			NativeFieldInfoPtr_Weight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeightedItem>.NativeClassPtr, "Weight");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeightedItem>.NativeClassPtr, 100682819);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe WeightedItem()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WeightedItem>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public WeightedItem(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_InventorySlotCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_ClearInventoryOnNewDay;

	private static readonly System.IntPtr NativeFieldInfoPtr_RandomizeInventory;

	private static readonly System.IntPtr NativeFieldInfoPtr_AllowDuplicateRandomItems;

	private static readonly System.IntPtr NativeFieldInfoPtr_RandomInventoryItems;

	private static readonly System.IntPtr NativeFieldInfoPtr_RandomizeCash;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinRandomCash;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxRandomCash;

	private static readonly System.IntPtr NativeFieldInfoPtr_StartingInventoryItems;

	private static readonly System.IntPtr NativeFieldInfoPtr_CanBePickpocketed;

	private static readonly System.IntPtr NativeFieldInfoPtr_PickpocketDifficulty;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultCombatWeapon;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Inventory_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int InventorySlotCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InventorySlotCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InventorySlotCount)) = num;
		}
	}

	public unsafe bool ClearInventoryOnNewDay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClearInventoryOnNewDay);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClearInventoryOnNewDay)) = flag;
		}
	}

	public unsafe bool RandomizeInventory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomizeInventory);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomizeInventory)) = flag;
		}
	}

	public unsafe bool AllowDuplicateRandomItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AllowDuplicateRandomItems);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AllowDuplicateRandomItems)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<WeightedItem> RandomInventoryItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomInventoryItems);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<WeightedItem>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomInventoryItems)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool RandomizeCash
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomizeCash);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomizeCash)) = flag;
		}
	}

	public unsafe int MinRandomCash
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinRandomCash);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinRandomCash)) = num;
		}
	}

	public unsafe int MaxRandomCash
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxRandomCash);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxRandomCash)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<ItemDefinition> StartingInventoryItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartingInventoryItems);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ItemDefinition>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartingInventoryItems)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool CanBePickpocketed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanBePickpocketed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanBePickpocketed)) = flag;
		}
	}

	public unsafe float PickpocketDifficulty
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickpocketDifficulty);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickpocketDifficulty)) = num;
		}
	}

	public unsafe AvatarWeapon DefaultCombatWeapon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultCombatWeapon);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AvatarWeapon>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultCombatWeapon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatarWeapon));
		}
	}

	static Inventory()
	{
		Il2CppClassPointerStore<Inventory>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Inventory");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Inventory>.NativeClassPtr);
		NativeFieldInfoPtr_InventorySlotCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "InventorySlotCount");
		NativeFieldInfoPtr_ClearInventoryOnNewDay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "ClearInventoryOnNewDay");
		NativeFieldInfoPtr_RandomizeInventory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "RandomizeInventory");
		NativeFieldInfoPtr_AllowDuplicateRandomItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "AllowDuplicateRandomItems");
		NativeFieldInfoPtr_RandomInventoryItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "RandomInventoryItems");
		NativeFieldInfoPtr_RandomizeCash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "RandomizeCash");
		NativeFieldInfoPtr_MinRandomCash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "MinRandomCash");
		NativeFieldInfoPtr_MaxRandomCash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "MaxRandomCash");
		NativeFieldInfoPtr_StartingInventoryItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "StartingInventoryItems");
		NativeFieldInfoPtr_CanBePickpocketed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "CanBePickpocketed");
		NativeFieldInfoPtr_PickpocketDifficulty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "PickpocketDifficulty");
		NativeFieldInfoPtr_DefaultCombatWeapon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Inventory>.NativeClassPtr, "DefaultCombatWeapon");
		NativeMethodInfoPtr_GetCopy_Public_Inventory_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Inventory>.NativeClassPtr, 100682817);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Inventory>.NativeClassPtr, 100682818);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 272276, RefRangeEnd = 272277, XrefRangeStart = 272255, XrefRangeEnd = 272276, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Inventory GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Inventory_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Inventory>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272277, XrefRangeEnd = 272278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Inventory()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Inventory>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Inventory(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
