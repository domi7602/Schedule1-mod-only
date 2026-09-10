using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Employees;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2Cpp;

public class DeveloperSettings : ScriptableObject
{
	[System.Serializable]
	public class PropertyEmployees : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_PropertyCode;

		private static readonly System.IntPtr NativeFieldInfoPtr_Employees;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string PropertyCode
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PropertyCode);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PropertyCode)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe List<EEmployeeType> Employees
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Employees);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<EEmployeeType>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Employees)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		static PropertyEmployees()
		{
			Il2CppClassPointerStore<PropertyEmployees>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "PropertyEmployees");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PropertyEmployees>.NativeClassPtr);
			NativeFieldInfoPtr_PropertyCode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PropertyEmployees>.NativeClassPtr, "PropertyCode");
			NativeFieldInfoPtr_Employees = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PropertyEmployees>.NativeClassPtr, "Employees");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyEmployees>.NativeClassPtr, 100663395);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65712, XrefRangeEnd = 65720, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe PropertyEmployees()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PropertyEmployees>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public PropertyEmployees(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	public class Item : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Amount;

		private static readonly System.IntPtr NativeFieldInfoPtr_Definition;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe int Amount
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amount);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amount)) = num;
			}
		}

		public unsafe ItemDefinition Definition
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Definition);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemDefinition>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Definition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemDefinition));
			}
		}

		static Item()
		{
			Il2CppClassPointerStore<Item>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "Item");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Item>.NativeClassPtr);
			NativeFieldInfoPtr_Amount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Item>.NativeClassPtr, "Amount");
			NativeFieldInfoPtr_Definition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Item>.NativeClassPtr, "Definition");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Item>.NativeClassPtr, 100663396);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Item()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Item>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Item(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_IsActive;

	private static readonly System.IntPtr NativeFieldInfoPtr_Cash;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnlineBalance;

	private static readonly System.IntPtr NativeFieldInfoPtr_TimeOfDay;

	private static readonly System.IntPtr NativeFieldInfoPtr_XP;

	private static readonly System.IntPtr NativeFieldInfoPtr_Weather;

	private static readonly System.IntPtr NativeFieldInfoPtr_TeleportLocation;

	private static readonly System.IntPtr NativeFieldInfoPtr_OwnedProperties;

	private static readonly System.IntPtr NativeFieldInfoPtr_PropertySaves;

	private static readonly System.IntPtr NativeFieldInfoPtr_Items;

	private static readonly System.IntPtr NativeFieldInfoPtr_EmployeeSettings;

	private static readonly System.IntPtr NativeFieldInfoPtr_UnlockedNPCs;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool IsActive
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsActive);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsActive)) = flag;
		}
	}

	public unsafe int Cash
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cash);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cash)) = num;
		}
	}

	public unsafe int OnlineBalance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnlineBalance);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnlineBalance)) = num;
		}
	}

	public unsafe int TimeOfDay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeOfDay);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeOfDay)) = num;
		}
	}

	public unsafe int XP
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_XP);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_XP)) = num;
		}
	}

	public unsafe string Weather
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Weather);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Weather)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string TeleportLocation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TeleportLocation);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TeleportLocation)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe List<string> OwnedProperties
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OwnedProperties);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OwnedProperties)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<TextAsset> PropertySaves
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PropertySaves);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<TextAsset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PropertySaves)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<Item> Items
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Item>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<PropertyEmployees> EmployeeSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EmployeeSettings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PropertyEmployees>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EmployeeSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<string> UnlockedNPCs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockedNPCs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockedNPCs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static DeveloperSettings()
	{
		Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "DeveloperSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr);
		NativeFieldInfoPtr_IsActive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "IsActive");
		NativeFieldInfoPtr_Cash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "Cash");
		NativeFieldInfoPtr_OnlineBalance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "OnlineBalance");
		NativeFieldInfoPtr_TimeOfDay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "TimeOfDay");
		NativeFieldInfoPtr_XP = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "XP");
		NativeFieldInfoPtr_Weather = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "Weather");
		NativeFieldInfoPtr_TeleportLocation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "TeleportLocation");
		NativeFieldInfoPtr_OwnedProperties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "OwnedProperties");
		NativeFieldInfoPtr_PropertySaves = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "PropertySaves");
		NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "Items");
		NativeFieldInfoPtr_EmployeeSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "EmployeeSettings");
		NativeFieldInfoPtr_UnlockedNPCs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, "UnlockedNPCs");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr, 100663394);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65720, XrefRangeEnd = 65754, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeveloperSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeveloperSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeveloperSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
