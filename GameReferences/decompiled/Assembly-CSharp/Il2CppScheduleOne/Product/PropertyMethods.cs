using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Product;

public static class PropertyMethods : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetName_Public_Static_String_EProperty_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDescription_Public_Static_String_EProperty_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColor_Public_Static_Color_EProperty_0;

	static PropertyMethods()
	{
		Il2CppClassPointerStore<PropertyMethods>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "PropertyMethods");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PropertyMethods>.NativeClassPtr);
		NativeMethodInfoPtr_GetName_Public_Static_String_EProperty_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyMethods>.NativeClassPtr, 100679212);
		NativeMethodInfoPtr_GetDescription_Public_Static_String_EProperty_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyMethods>.NativeClassPtr, 100679213);
		NativeMethodInfoPtr_GetColor_Public_Static_Color_EProperty_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyMethods>.NativeClassPtr, 100679214);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 236137, XrefRangeEnd = 236138, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetName(this EProperty property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetName_Public_Static_String_EProperty_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 236138, XrefRangeEnd = 236139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetDescription(this EProperty property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDescription_Public_Static_String_EProperty_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 236139, XrefRangeEnd = 236140, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetColor(this EProperty property)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&property);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColor_Public_Static_Color_EProperty_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PropertyMethods(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
