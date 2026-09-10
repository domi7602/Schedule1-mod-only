using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Product;
using Il2CppSystem;

namespace Il2CppScheduleOne.Economy;

[System.Serializable]
public class ProductTypeAffinity : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DrugType;

	private static readonly System.IntPtr NativeFieldInfoPtr_Affinity;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EDrugType DrugType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DrugType);
			return *(EDrugType*)num;
		}
		set
		{
			*(EDrugType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DrugType)) = eDrugType;
		}
	}

	public unsafe float Affinity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Affinity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Affinity)) = num;
		}
	}

	static ProductTypeAffinity()
	{
		Il2CppClassPointerStore<ProductTypeAffinity>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Economy", "ProductTypeAffinity");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ProductTypeAffinity>.NativeClassPtr);
		NativeFieldInfoPtr_DrugType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductTypeAffinity>.NativeClassPtr, "DrugType");
		NativeFieldInfoPtr_Affinity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductTypeAffinity>.NativeClassPtr, "Affinity");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProductTypeAffinity>.NativeClassPtr, 100674027);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ProductTypeAffinity()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ProductTypeAffinity>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ProductTypeAffinity(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
