using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class WeedData : ProductItemData
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_String_0;

	static WeedData()
	{
		Il2CppClassPointerStore<WeedData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "WeedData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WeedData>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeedData>.NativeClassPtr, 100669373);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 134763, RefRangeEnd = 134768, XrefRangeStart = 134763, XrefRangeEnd = 134768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WeedData(string iD, int quantity, string quality, string packagingID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WeedData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iD);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &quantity;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(quality);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(packagingID);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WeedData(IntPtr pointer)
		: base(pointer)
	{
	}
}
