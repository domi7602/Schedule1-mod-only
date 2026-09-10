using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class ShopData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_ShopCode;

	private static readonly IntPtr NativeFieldInfoPtr_ItemStockQuantities;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppReferenceArray_1_StringIntPair_0;

	public unsafe string ShopCode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShopCode);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShopCode)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppReferenceArray<StringIntPair> ItemStockQuantities
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemStockQuantities);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<StringIntPair>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemStockQuantities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ShopData()
	{
		Il2CppClassPointerStore<ShopData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ShopData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShopData>.NativeClassPtr);
		NativeFieldInfoPtr_ShopCode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShopData>.NativeClassPtr, "ShopCode");
		NativeFieldInfoPtr_ItemStockQuantities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShopData>.NativeClassPtr, "ItemStockQuantities");
		NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppReferenceArray_1_StringIntPair_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShopData>.NativeClassPtr, 100669466);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 134800, RefRangeEnd = 134812, XrefRangeStart = 134800, XrefRangeEnd = 134812, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShopData(string shopCode, Il2CppReferenceArray<StringIntPair> itemStockQuantities)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShopData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(shopCode);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemStockQuantities);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppReferenceArray_1_StringIntPair_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ShopData(IntPtr pointer)
		: base(pointer)
	{
	}
}
