using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class CashData : ItemData
{
	private static readonly IntPtr NativeFieldInfoPtr_CashBalance;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Single_0;

	public unsafe float CashBalance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CashBalance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CashBalance)) = num;
		}
	}

	static CashData()
	{
		Il2CppClassPointerStore<CashData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "CashData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CashData>.NativeClassPtr);
		NativeFieldInfoPtr_CashBalance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CashData>.NativeClassPtr, "CashBalance");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CashData>.NativeClassPtr, 100669360);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134753, RefRangeEnd = 134755, XrefRangeStart = 134751, XrefRangeEnd = 134753, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CashData(string iD, int quantity, float cashBalance)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CashData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iD);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &quantity;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &cashBalance;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CashData(IntPtr pointer)
		: base(pointer)
	{
	}
}
