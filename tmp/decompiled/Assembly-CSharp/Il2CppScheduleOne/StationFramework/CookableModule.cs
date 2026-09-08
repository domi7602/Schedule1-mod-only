using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace Il2CppScheduleOne.StationFramework;

public class CookableModule : ItemModule
{
	[OriginalName("Assembly-CSharp.dll", "", "ECookableType")]
	public enum ECookableType
	{
		Liquid,
		Solid
	}

	private static readonly IntPtr NativeFieldInfoPtr_CookTime;

	private static readonly IntPtr NativeFieldInfoPtr_CookType;

	private static readonly IntPtr NativeFieldInfoPtr_Product;

	private static readonly IntPtr NativeFieldInfoPtr_ProductQuantity;

	private static readonly IntPtr NativeFieldInfoPtr_ProductShardPrefab;

	private static readonly IntPtr NativeFieldInfoPtr_LiquidColor;

	private static readonly IntPtr NativeFieldInfoPtr_SolidColor;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int CookTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookTime)) = num;
		}
	}

	public unsafe ECookableType CookType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookType);
			return *(ECookableType*)num;
		}
		set
		{
			*(ECookableType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookType)) = eCookableType;
		}
	}

	public unsafe StorableItemDefinition Product
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StorableItemDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storableItemDefinition));
		}
	}

	public unsafe int ProductQuantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuantity)) = num;
		}
	}

	public unsafe Rigidbody ProductShardPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductShardPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductShardPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rigidbody));
		}
	}

	public unsafe Color LiquidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LiquidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LiquidColor)) = color;
		}
	}

	public unsafe Color SolidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SolidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SolidColor)) = color;
		}
	}

	static CookableModule()
	{
		Il2CppClassPointerStore<CookableModule>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.StationFramework", "CookableModule");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CookableModule>.NativeClassPtr);
		NativeFieldInfoPtr_CookTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "CookTime");
		NativeFieldInfoPtr_CookType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "CookType");
		NativeFieldInfoPtr_Product = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "Product");
		NativeFieldInfoPtr_ProductQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "ProductQuantity");
		NativeFieldInfoPtr_ProductShardPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "ProductShardPrefab");
		NativeFieldInfoPtr_LiquidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "LiquidColor");
		NativeFieldInfoPtr_SolidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, "SolidColor");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CookableModule>.NativeClassPtr, 100678907);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233740, XrefRangeEnd = 233741, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CookableModule()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CookableModule>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CookableModule(IntPtr pointer)
		: base(pointer)
	{
	}
}
