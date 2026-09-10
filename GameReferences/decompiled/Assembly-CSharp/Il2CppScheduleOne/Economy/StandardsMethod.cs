using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.Economy;

public static class StandardsMethod : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetName_Public_Static_String_ECustomerStandard_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCorrespondingQuality_Public_Static_EQuality_ECustomerStandard_0;

	static StandardsMethod()
	{
		Il2CppClassPointerStore<StandardsMethod>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Economy", "StandardsMethod");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StandardsMethod>.NativeClassPtr);
		NativeMethodInfoPtr_GetName_Public_Static_String_ECustomerStandard_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandardsMethod>.NativeClassPtr, 100674257);
		NativeMethodInfoPtr_GetCorrespondingQuality_Public_Static_EQuality_ECustomerStandard_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandardsMethod>.NativeClassPtr, 100674258);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 186530, RefRangeEnd = 186532, XrefRangeStart = 186523, XrefRangeEnd = 186530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetName(this ECustomerStandard property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetName_Public_Static_String_ECustomerStandard_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 186532, RefRangeEnd = 186543, XrefRangeStart = 186532, XrefRangeEnd = 186532, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EQuality GetCorrespondingQuality(this ECustomerStandard property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCorrespondingQuality_Public_Static_EQuality_ECustomerStandard_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public StandardsMethod(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
