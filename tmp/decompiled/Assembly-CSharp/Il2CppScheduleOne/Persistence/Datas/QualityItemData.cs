using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class QualityItemData : ItemData
{
	private static readonly IntPtr NativeFieldInfoPtr_Quality;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_0;

	public unsafe string Quality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quality);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quality)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static QualityItemData()
	{
		Il2CppClassPointerStore<QualityItemData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "QualityItemData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QualityItemData>.NativeClassPtr);
		NativeFieldInfoPtr_Quality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QualityItemData>.NativeClassPtr, "Quality");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityItemData>.NativeClassPtr, 100669368);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 134580, RefRangeEnd = 134586, XrefRangeStart = 134580, XrefRangeEnd = 134586, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QualityItemData(string iD, int quantity, string quality)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QualityItemData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iD);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &quantity;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(quality);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QualityItemData(IntPtr pointer)
		: base(pointer)
	{
	}
}
