using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class WateringCanData : ItemData
{
	private static readonly IntPtr NativeFieldInfoPtr_CurrentFillAmount;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Single_0;

	public unsafe float CurrentFillAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentFillAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentFillAmount)) = num;
		}
	}

	static WateringCanData()
	{
		Il2CppClassPointerStore<WateringCanData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "WateringCanData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WateringCanData>.NativeClassPtr);
		NativeFieldInfoPtr_CurrentFillAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WateringCanData>.NativeClassPtr, "CurrentFillAmount");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WateringCanData>.NativeClassPtr, 100669372);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134753, RefRangeEnd = 134755, XrefRangeStart = 134753, XrefRangeEnd = 134755, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WateringCanData(string iD, int quantity, float currentFillLevel)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WateringCanData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iD);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &quantity;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &currentFillLevel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WateringCanData(IntPtr pointer)
		: base(pointer)
	{
	}
}
