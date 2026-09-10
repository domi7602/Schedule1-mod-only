using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Clothing;

[Serializable]
public class ClothingDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr_Slot;

	private static readonly IntPtr NativeFieldInfoPtr_ApplicationType;

	private static readonly IntPtr NativeFieldInfoPtr_ClothingAssetPath;

	private static readonly IntPtr NativeFieldInfoPtr_Colorable;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultColor;

	private static readonly IntPtr NativeFieldInfoPtr_SlotsToBlock;

	private static readonly IntPtr NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EClothingSlot Slot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Slot);
			return *(EClothingSlot*)num;
		}
		set
		{
			*(EClothingSlot*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Slot)) = eClothingSlot;
		}
	}

	public unsafe EClothingApplicationType ApplicationType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ApplicationType);
			return *(EClothingApplicationType*)num;
		}
		set
		{
			*(EClothingApplicationType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ApplicationType)) = eClothingApplicationType;
		}
	}

	public unsafe string ClothingAssetPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClothingAssetPath);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClothingAssetPath)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool Colorable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Colorable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Colorable)) = flag;
		}
	}

	public unsafe EClothingColor DefaultColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultColor);
			return *(EClothingColor*)num;
		}
		set
		{
			*(EClothingColor*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultColor)) = eClothingColor;
		}
	}

	public unsafe List<EClothingSlot> SlotsToBlock
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SlotsToBlock);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<EClothingSlot>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SlotsToBlock)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static ClothingDefinition()
	{
		Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Clothing", "ClothingDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_Slot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, "Slot");
		NativeFieldInfoPtr_ApplicationType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, "ApplicationType");
		NativeFieldInfoPtr_ClothingAssetPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, "ClothingAssetPath");
		NativeFieldInfoPtr_Colorable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, "Colorable");
		NativeFieldInfoPtr_DefaultColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, "DefaultColor");
		NativeFieldInfoPtr_SlotsToBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, "SlotsToBlock");
		NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, 100675559);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr, 100675560);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 200234, XrefRangeEnd = 200238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 200238, XrefRangeEnd = 200250, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ClothingDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ClothingDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ClothingDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
