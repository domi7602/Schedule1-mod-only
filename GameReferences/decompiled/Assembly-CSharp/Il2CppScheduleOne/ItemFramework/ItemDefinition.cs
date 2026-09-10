using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.UI.Items;

namespace Il2CppScheduleOne.ItemFramework;

[Serializable]
public class ItemDefinition : BaseItemDefinition
{
	[OriginalName("Assembly-CSharp.dll", "", "EEquipMode")]
	public enum EEquipMode
	{
		Legacy,
		New
	}

	private static readonly IntPtr NativeFieldInfoPtr_AvailableInDemo;

	private static readonly IntPtr NativeFieldInfoPtr_EquipMode;

	private static readonly IntPtr NativeFieldInfoPtr_Equippable;

	private static readonly IntPtr NativeFieldInfoPtr_CustomItemUI;

	private static readonly IntPtr NativeFieldInfoPtr_CustomInfoContent;

	private static readonly IntPtr NativeMethodInfoPtr_GetDefaultInstance_Public_Abstract_Virtual_New_ItemInstance_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe bool AvailableInDemo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvailableInDemo);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvailableInDemo)) = flag;
		}
	}

	public unsafe EEquipMode EquipMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EquipMode);
			return *(EEquipMode*)num;
		}
		set
		{
			*(EEquipMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EquipMode)) = eEquipMode;
		}
	}

	public unsafe Equippable Equippable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Equippable);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Equippable>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Equippable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equippable));
		}
	}

	public unsafe ItemUI CustomItemUI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomItemUI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomItemUI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemUI));
		}
	}

	public unsafe ItemInfoContent CustomInfoContent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomInfoContent);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemInfoContent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomInfoContent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemInfoContent));
		}
	}

	static ItemDefinition()
	{
		Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_AvailableInDemo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, "AvailableInDemo");
		NativeFieldInfoPtr_EquipMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, "EquipMode");
		NativeFieldInfoPtr_Equippable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, "Equippable");
		NativeFieldInfoPtr_CustomItemUI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, "CustomItemUI");
		NativeFieldInfoPtr_CustomInfoContent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, "CustomInfoContent");
		NativeMethodInfoPtr_GetDefaultInstance_Public_Abstract_Virtual_New_ItemInstance_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, 100672516);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr, 100672517);
	}

	[CallerCount(0)]
	public unsafe virtual ItemInstance GetDefaultInstance(int quantity = 1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&quantity);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDefaultInstance_Public_Abstract_Virtual_New_ItemInstance_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 167285, XrefRangeEnd = 167286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ItemDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
