using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Clothing;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class ClothingData : ItemData
{
	private static readonly IntPtr NativeFieldInfoPtr_Color;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_EClothingColor_0;

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

	static ClothingData()
	{
		Il2CppClassPointerStore<ClothingData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ClothingData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClothingData>.NativeClassPtr);
		NativeFieldInfoPtr_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClothingData>.NativeClassPtr, "Color");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_EClothingColor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingData>.NativeClassPtr, 100669361);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134757, RefRangeEnd = 134759, XrefRangeStart = 134755, XrefRangeEnd = 134757, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ClothingData(string iD, int quantity, EClothingColor color)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ClothingData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iD);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &quantity;
		*(EClothingColor**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &color;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_EClothingColor_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ClothingData(IntPtr pointer)
		: base(pointer)
	{
	}
}
