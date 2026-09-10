using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class StringExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ColorFromHtml_Public_Static_Color_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TrimStart_Public_Static_String_String_String_StringComparison_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TrimEnd_Public_Static_String_String_String_StringComparison_0;

	static StringExt()
	{
		Il2CppClassPointerStore<StringExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "StringExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringExt>.NativeClassPtr);
		NativeMethodInfoPtr_ColorFromHtml_Public_Static_Color_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExt>.NativeClassPtr, 100664395);
		NativeMethodInfoPtr_TrimStart_Public_Static_String_String_String_StringComparison_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExt>.NativeClassPtr, 100664396);
		NativeMethodInfoPtr_TrimEnd_Public_Static_String_String_String_StringComparison_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringExt>.NativeClassPtr, 100664397);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34146, RefRangeEnd = 34148, XrefRangeStart = 34125, XrefRangeEnd = 34146, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color ColorFromHtml(this string hexString)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(hexString);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ColorFromHtml_Public_Static_Color_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34150, RefRangeEnd = 34152, XrefRangeStart = 34148, XrefRangeEnd = 34150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string TrimStart(this string s, string trim, Il2CppSystem.StringComparison compare = Il2CppSystem.StringComparison.CurrentCultureIgnoreCase)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(s);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(trim);
		*(Il2CppSystem.StringComparison**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &compare;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TrimStart_Public_Static_String_String_String_StringComparison_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34152, XrefRangeEnd = 34154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string TrimEnd(this string s, string trim, Il2CppSystem.StringComparison compare = Il2CppSystem.StringComparison.CurrentCultureIgnoreCase)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(s);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(trim);
		*(Il2CppSystem.StringComparison**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &compare;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TrimEnd_Public_Static_String_String_String_StringComparison_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public StringExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
