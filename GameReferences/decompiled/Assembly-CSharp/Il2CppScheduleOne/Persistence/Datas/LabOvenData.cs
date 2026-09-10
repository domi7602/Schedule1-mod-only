using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

public class LabOvenData : GridItemData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_InputContents;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutputContents;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentIngredientID;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentIngredientQuantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentIngredientQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentProductID;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentCookProgress;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_ItemSet_ItemSet_String_Int32_EQuality_String_Int32_0;

	public unsafe ItemSet InputContents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InputContents);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InputContents)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemSet));
		}
	}

	public unsafe ItemSet OutputContents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutputContents);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutputContents)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemSet));
		}
	}

	public unsafe string CurrentIngredientID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentIngredientID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentIngredientID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int CurrentIngredientQuantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentIngredientQuantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentIngredientQuantity)) = num;
		}
	}

	public unsafe EQuality CurrentIngredientQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentIngredientQuality);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentIngredientQuality)) = eQuality;
		}
	}

	public unsafe string CurrentProductID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentProductID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentProductID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int CurrentCookProgress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentCookProgress);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentCookProgress)) = num;
		}
	}

	static LabOvenData()
	{
		Il2CppClassPointerStore<LabOvenData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "LabOvenData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr);
		NativeFieldInfoPtr_InputContents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "InputContents");
		NativeFieldInfoPtr_OutputContents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "OutputContents");
		NativeFieldInfoPtr_CurrentIngredientID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "CurrentIngredientID");
		NativeFieldInfoPtr_CurrentIngredientQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "CurrentIngredientQuantity");
		NativeFieldInfoPtr_CurrentIngredientQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "CurrentIngredientQuality");
		NativeFieldInfoPtr_CurrentProductID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "CurrentProductID");
		NativeFieldInfoPtr_CurrentCookProgress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, "CurrentCookProgress");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_ItemSet_ItemSet_String_Int32_EQuality_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr, 100669429);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135170, RefRangeEnd = 135171, XrefRangeStart = 135160, XrefRangeEnd = 135170, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LabOvenData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet inputContents, ItemSet outputContents, string ingredientID, int currentIngredientQuantity, EQuality ingredientQuality, string productID, int currentCookProgress)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LabOvenData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[13];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &originCoordinate;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputContents);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputContents);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(ingredientID);
		*(int**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentIngredientQuantity;
		*(EQuality**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = &ingredientQuality;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(productID);
		*(int**)((byte*)ptr + checked((nuint)12u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentCookProgress;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_ItemSet_ItemSet_String_Int32_EQuality_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LabOvenData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
