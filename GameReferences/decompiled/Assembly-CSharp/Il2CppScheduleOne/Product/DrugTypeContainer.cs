using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Product;

[System.Serializable]
public class DrugTypeContainer : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DrugType;

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

	static DrugTypeContainer()
	{
		Il2CppClassPointerStore<DrugTypeContainer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "DrugTypeContainer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DrugTypeContainer>.NativeClassPtr);
		NativeFieldInfoPtr_DrugType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DrugTypeContainer>.NativeClassPtr, "DrugType");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DrugTypeContainer>.NativeClassPtr, 100679208);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DrugTypeContainer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DrugTypeContainer>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DrugTypeContainer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
