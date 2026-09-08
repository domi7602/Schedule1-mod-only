using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Product;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class MethProductData : ProductData
{
	private static readonly IntPtr NativeFieldInfoPtr_AppearanceSettings;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_EDrugType_Il2CppStringArray_MethAppearanceSettings_0;

	public unsafe MethAppearanceSettings AppearanceSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AppearanceSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MethAppearanceSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AppearanceSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)methAppearanceSettings));
		}
	}

	static MethProductData()
	{
		Il2CppClassPointerStore<MethProductData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MethProductData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MethProductData>.NativeClassPtr);
		NativeFieldInfoPtr_AppearanceSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MethProductData>.NativeClassPtr, "AppearanceSettings");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_EDrugType_Il2CppStringArray_MethAppearanceSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MethProductData>.NativeClassPtr, 100669448);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 135233, RefRangeEnd = 135237, XrefRangeStart = 135233, XrefRangeEnd = 135237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MethProductData(string name, string id, EDrugType drugType, Il2CppStringArray properties, MethAppearanceSettings appearanceSettings)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MethProductData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(id);
		*(EDrugType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &drugType;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)properties);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)appearanceSettings);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_EDrugType_Il2CppStringArray_MethAppearanceSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MethProductData(IntPtr pointer)
		: base(pointer)
	{
	}
}
