using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Effects;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Product;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.ObjectScripts;

[System.Serializable]
public class MixOperation : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ProductID;

	private static readonly System.IntPtr NativeFieldInfoPtr_ProductQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_IngredientID;

	private static readonly System.IntPtr NativeFieldInfoPtr_Quantity;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOutput_Public_EDrugType_byref_List_1_Effect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsOutputKnown_Public_Boolean_byref_ProductDefinition_0;

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

	public unsafe EQuality ProductQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuality);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuality)) = eQuality;
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

	static MixOperation()
	{
		Il2CppClassPointerStore<MixOperation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ObjectScripts", "MixOperation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MixOperation>.NativeClassPtr);
		NativeFieldInfoPtr_ProductID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, "ProductID");
		NativeFieldInfoPtr_ProductQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, "ProductQuality");
		NativeFieldInfoPtr_IngredientID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, "IngredientID");
		NativeFieldInfoPtr_Quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, "Quantity");
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, 100681189);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, 100681190);
		NativeMethodInfoPtr_GetOutput_Public_EDrugType_byref_List_1_Effect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, 100681191);
		NativeMethodInfoPtr_IsOutputKnown_Public_Boolean_byref_ProductDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MixOperation>.NativeClassPtr, 100681192);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 257372, RefRangeEnd = 257374, XrefRangeStart = 257369, XrefRangeEnd = 257372, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MixOperation(string productID, EQuality productQuality, string ingredientID, int quantity)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MixOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(productID);
		*(EQuality**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &productQuality;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(ingredientID);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &quantity;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MixOperation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MixOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 257400, RefRangeEnd = 257403, XrefRangeStart = 257374, XrefRangeEnd = 257400, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EDrugType GetOutput(out List<Effect> properties)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		nint num = 0;
		*ptr = (nint)(&num);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutput_Public_EDrugType_byref_List_1_Effect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num2 = num;
		properties = ((num2 == 0) ? null : new List<Effect>(num2));
		return *(EDrugType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 257414, RefRangeEnd = 257417, XrefRangeStart = 257403, XrefRangeEnd = 257414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsOutputKnown(out ProductDefinition knownProduct)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		nint num = 0;
		*ptr = (nint)(&num);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsOutputKnown_Public_Boolean_byref_ProductDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num2 = num;
		knownProduct = ((num2 == 0) ? null : new ProductDefinition(num2));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public MixOperation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
