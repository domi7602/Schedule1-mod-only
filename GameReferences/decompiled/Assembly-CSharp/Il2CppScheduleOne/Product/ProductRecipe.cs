using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne.Product;

public class ProductRecipe : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__productIcon;

	private static readonly IntPtr NativeFieldInfoPtr__productTooltip;

	private static readonly IntPtr NativeFieldInfoPtr__mixerIcon;

	private static readonly IntPtr NativeFieldInfoPtr__mixerTooltip;

	private static readonly IntPtr NativeFieldInfoPtr__outputIcon;

	private static readonly IntPtr NativeFieldInfoPtr__outputTooltip;

	private static readonly IntPtr NativeMethodInfoPtr_SetProduct_Public_Void_Sprite_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMixer_Public_Void_Sprite_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetOutput_Public_Void_Sprite_String_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Image _productIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__productIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__productIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Tooltip _productTooltip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__productTooltip);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Tooltip>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__productTooltip)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tooltip));
		}
	}

	public unsafe Image _mixerIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixerIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixerIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Tooltip _mixerTooltip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixerTooltip);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Tooltip>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixerTooltip)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tooltip));
		}
	}

	public unsafe Image _outputIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__outputIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__outputIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Tooltip _outputTooltip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__outputTooltip);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Tooltip>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__outputTooltip)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tooltip));
		}
	}

	static ProductRecipe()
	{
		Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "ProductRecipe");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr);
		NativeFieldInfoPtr__productIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, "_productIcon");
		NativeFieldInfoPtr__productTooltip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, "_productTooltip");
		NativeFieldInfoPtr__mixerIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, "_mixerIcon");
		NativeFieldInfoPtr__mixerTooltip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, "_mixerTooltip");
		NativeFieldInfoPtr__outputIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, "_outputIcon");
		NativeFieldInfoPtr__outputTooltip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, "_outputTooltip");
		NativeMethodInfoPtr_SetProduct_Public_Void_Sprite_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, 100679711);
		NativeMethodInfoPtr_SetMixer_Public_Void_Sprite_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, 100679712);
		NativeMethodInfoPtr_SetOutput_Public_Void_Sprite_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, 100679713);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr, 100679714);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 243087, RefRangeEnd = 243088, XrefRangeStart = 243084, XrefRangeEnd = 243087, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetProduct(Sprite sprite, string tooltip)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetProduct_Public_Void_Sprite_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 243091, RefRangeEnd = 243092, XrefRangeStart = 243088, XrefRangeEnd = 243091, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMixer(Sprite sprite, string tooltip)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMixer_Public_Void_Sprite_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 243095, RefRangeEnd = 243096, XrefRangeStart = 243092, XrefRangeEnd = 243095, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetOutput(Sprite sprite, string tooltip)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetOutput_Public_Void_Sprite_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ProductRecipe()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ProductRecipe>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ProductRecipe(IntPtr pointer)
		: base(pointer)
	{
	}
}
