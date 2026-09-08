using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Clothing;

namespace Il2CppScheduleOne.UI.Shop;

public class ClothingShopListing : ShopListing
{
	private static readonly IntPtr NativeFieldInfoPtr_Color;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EClothingColor Color
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Color);
			return *(EClothingColor*)num;
		}
		set
		{
			*(EClothingColor*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Color)) = eClothingColor;
		}
	}

	static ClothingShopListing()
	{
		Il2CppClassPointerStore<ClothingShopListing>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Shop", "ClothingShopListing");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClothingShopListing>.NativeClassPtr);
		NativeFieldInfoPtr_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingShopListing>.NativeClassPtr, "Color");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingShopListing>.NativeClassPtr, 100689649);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 335382, RefRangeEnd = 335383, XrefRangeStart = 335371, XrefRangeEnd = 335382, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ClothingShopListing()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ClothingShopListing>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ClothingShopListing(IntPtr pointer)
		: base(pointer)
	{
	}
}
