using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Packaging;
using Il2CppScheduleOne.Storage;

namespace Il2CppScheduleOne.Product.Packaging;

[Serializable]
public class PackagingDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr_Quantity;

	private static readonly IntPtr NativeFieldInfoPtr_StealthLevel;

	private static readonly IntPtr NativeFieldInfoPtr_FunctionalPackaging;

	private static readonly IntPtr NativeFieldInfoPtr_Equippable_Filled;

	private static readonly IntPtr NativeFieldInfoPtr_StoredItem_Filled;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe EStealthLevel StealthLevel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StealthLevel);
			return *(EStealthLevel*)num;
		}
		set
		{
			*(EStealthLevel*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StealthLevel)) = eStealthLevel;
		}
	}

	public unsafe FunctionalPackaging FunctionalPackaging
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FunctionalPackaging);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<FunctionalPackaging>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FunctionalPackaging)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)functionalPackaging));
		}
	}

	public unsafe Equippable Equippable_Filled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Equippable_Filled);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Equippable>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Equippable_Filled)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equippable));
		}
	}

	public unsafe StoredItem StoredItem_Filled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StoredItem_Filled);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StoredItem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StoredItem_Filled)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storedItem));
		}
	}

	static PackagingDefinition()
	{
		Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product.Packaging", "PackagingDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_Quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr, "Quantity");
		NativeFieldInfoPtr_StealthLevel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr, "StealthLevel");
		NativeFieldInfoPtr_FunctionalPackaging = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr, "FunctionalPackaging");
		NativeFieldInfoPtr_Equippable_Filled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr, "Equippable_Filled");
		NativeFieldInfoPtr_StoredItem_Filled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr, "StoredItem_Filled");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr, 100679715);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 243096, XrefRangeEnd = 243097, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PackagingDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PackagingDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PackagingDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
