using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Product;

public static class DrugTypeMethods : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetName_Public_Static_String_EDrugType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColor_Public_Static_Color_EDrugType_0;

	static DrugTypeMethods()
	{
		Il2CppClassPointerStore<DrugTypeMethods>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "DrugTypeMethods");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DrugTypeMethods>.NativeClassPtr);
		NativeMethodInfoPtr_GetName_Public_Static_String_EDrugType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DrugTypeMethods>.NativeClassPtr, 100679209);
		NativeMethodInfoPtr_GetColor_Public_Static_Color_EDrugType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DrugTypeMethods>.NativeClassPtr, 100679210);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 236133, RefRangeEnd = 236134, XrefRangeStart = 236132, XrefRangeEnd = 236133, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetName(this EDrugType property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetName_Public_Static_String_EDrugType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 236135, RefRangeEnd = 236137, XrefRangeStart = 236134, XrefRangeEnd = 236135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetColor(this EDrugType property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColor_Public_Static_Color_EDrugType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public DrugTypeMethods(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
