using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Clothing;

public static class ClothingColorExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetActualColor_Public_Static_Color_EClothingColor_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLabelColor_Public_Static_Color_EClothingColor_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLabel_Public_Static_String_EClothingColor_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetClothingColor_Public_Static_EClothingColor_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ColorEquals_Public_Static_Boolean_Color_Color_Single_0;

	static ClothingColorExtensions()
	{
		Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Clothing", "ClothingColorExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_GetActualColor_Public_Static_Color_EClothingColor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr, 100675584);
		NativeMethodInfoPtr_GetLabelColor_Public_Static_Color_EClothingColor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr, 100675585);
		NativeMethodInfoPtr_GetLabel_Public_Static_String_EClothingColor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr, 100675586);
		NativeMethodInfoPtr_GetClothingColor_Public_Static_EClothingColor_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr, 100675587);
		NativeMethodInfoPtr_ColorEquals_Public_Static_Boolean_Color_Color_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClothingColorExtensions>.NativeClassPtr, 100675588);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 200484, RefRangeEnd = 200492, XrefRangeStart = 200479, XrefRangeEnd = 200484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetActualColor(this EClothingColor color)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&color);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetActualColor_Public_Static_Color_EClothingColor_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 200492, XrefRangeEnd = 200497, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetLabelColor(this EClothingColor color)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&color);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLabelColor_Public_Static_Color_EClothingColor_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 200500, RefRangeEnd = 200502, XrefRangeStart = 200497, XrefRangeEnd = 200500, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetLabel(this EClothingColor color)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&color);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLabel_Public_Static_String_EClothingColor_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 200530, RefRangeEnd = 200533, XrefRangeStart = 200502, XrefRangeEnd = 200530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EClothingColor GetClothingColor(Color color)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&color);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClothingColor_Public_Static_EClothingColor_Color_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EClothingColor*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 200533, RefRangeEnd = 200534, XrefRangeStart = 200533, XrefRangeEnd = 200533, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool ColorEquals(Color a, Color b, float tolerance = 0.004f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(Color**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &tolerance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ColorEquals_Public_Static_Boolean_Color_Color_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ClothingColorExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
