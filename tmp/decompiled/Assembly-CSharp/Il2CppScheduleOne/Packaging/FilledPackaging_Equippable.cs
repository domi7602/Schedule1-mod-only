using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Product;

namespace Il2CppScheduleOne.Packaging;

public class FilledPackaging_Equippable : Product_Equippable
{
	private static readonly IntPtr NativeFieldInfoPtr_MultiTypeVisuals;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyProductVisuals_Protected_Virtual_Void_ProductItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe MultiTypeVisualsSetter MultiTypeVisuals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MultiTypeVisuals);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MultiTypeVisualsSetter>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MultiTypeVisuals)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)multiTypeVisualsSetter));
		}
	}

	static FilledPackaging_Equippable()
	{
		Il2CppClassPointerStore<FilledPackaging_Equippable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Packaging", "FilledPackaging_Equippable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FilledPackaging_Equippable>.NativeClassPtr);
		NativeFieldInfoPtr_MultiTypeVisuals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FilledPackaging_Equippable>.NativeClassPtr, "MultiTypeVisuals");
		NativeMethodInfoPtr_ApplyProductVisuals_Protected_Virtual_Void_ProductItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FilledPackaging_Equippable>.NativeClassPtr, 100678286);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FilledPackaging_Equippable>.NativeClassPtr, 100678287);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 228219, XrefRangeEnd = 228221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ApplyProductVisuals(ProductItemInstance product)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)product);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ApplyProductVisuals_Protected_Virtual_Void_ProductItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 228221, XrefRangeEnd = 228222, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FilledPackaging_Equippable()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FilledPackaging_Equippable>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FilledPackaging_Equippable(IntPtr pointer)
		: base(pointer)
	{
	}
}
