using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public static class SVGStyleSheetUtils : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Parse_Public_Static_SVGStyleSheet_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseInline_Public_Static_SVGPropertySheet_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseSelector_Private_Static_Boolean_List_1_String_SVGStyleSheet_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CombineProperties_Private_Static_Void_SVGPropertySheet_SVGPropertySheet_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyProperties_Private_Static_SVGPropertySheet_SVGPropertySheet_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseProperties_Private_Static_Boolean_List_1_String_SVGPropertySheet_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseProperty_Private_Static_Boolean_List_1_String_byref_String_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseParenValue_Private_Static_String_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Tokenize_Public_Static_List_1_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PeekToken_Private_Static_String_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PopToken_Private_Static_String_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsSeparator_Private_Static_Boolean_Char_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsWhitespace_Private_Static_Boolean_Char_0;

	static SVGStyleSheetUtils()
	{
		Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGStyleSheetUtils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr);
		NativeMethodInfoPtr_Parse_Public_Static_SVGStyleSheet_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663523);
		NativeMethodInfoPtr_ParseInline_Public_Static_SVGPropertySheet_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663524);
		NativeMethodInfoPtr_ParseSelector_Private_Static_Boolean_List_1_String_SVGStyleSheet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663525);
		NativeMethodInfoPtr_CombineProperties_Private_Static_Void_SVGPropertySheet_SVGPropertySheet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663526);
		NativeMethodInfoPtr_CopyProperties_Private_Static_SVGPropertySheet_SVGPropertySheet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663527);
		NativeMethodInfoPtr_ParseProperties_Private_Static_Boolean_List_1_String_SVGPropertySheet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663528);
		NativeMethodInfoPtr_ParseProperty_Private_Static_Boolean_List_1_String_byref_String_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663529);
		NativeMethodInfoPtr_ParseParenValue_Private_Static_String_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663530);
		NativeMethodInfoPtr_Tokenize_Public_Static_List_1_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663531);
		NativeMethodInfoPtr_PeekToken_Private_Static_String_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663532);
		NativeMethodInfoPtr_PopToken_Private_Static_String_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663533);
		NativeMethodInfoPtr_IsSeparator_Private_Static_Boolean_Char_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663534);
		NativeMethodInfoPtr_IsWhitespace_Private_Static_Boolean_Char_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGStyleSheetUtils>.NativeClassPtr, 100663535);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26443, RefRangeEnd = 26444, XrefRangeStart = 26405, XrefRangeEnd = 26443, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SVGStyleSheet Parse(string cssText)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(cssText);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Parse_Public_Static_SVGStyleSheet_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGStyleSheet>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26468, RefRangeEnd = 26469, XrefRangeStart = 26444, XrefRangeEnd = 26468, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SVGPropertySheet ParseInline(string cssText)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(cssText);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseInline_Public_Static_SVGPropertySheet_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGPropertySheet>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26602, RefRangeEnd = 26603, XrefRangeStart = 26469, XrefRangeEnd = 26602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool ParseSelector(List<string> tokens, SVGStyleSheet sheet)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tokens);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sheet);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseSelector_Private_Static_Boolean_List_1_String_SVGStyleSheet_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26626, RefRangeEnd = 26627, XrefRangeStart = 26603, XrefRangeEnd = 26626, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CombineProperties(SVGPropertySheet first, SVGPropertySheet second)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)first);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)second);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CombineProperties_Private_Static_Void_SVGPropertySheet_SVGPropertySheet_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26652, RefRangeEnd = 26653, XrefRangeStart = 26627, XrefRangeEnd = 26652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SVGPropertySheet CopyProperties(SVGPropertySheet props)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)props);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyProperties_Private_Static_SVGPropertySheet_SVGPropertySheet_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGPropertySheet>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26653, XrefRangeEnd = 26670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool ParseProperties(List<string> tokens, SVGPropertySheet props)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tokens);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)props);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseProperties_Private_Static_Boolean_List_1_String_SVGPropertySheet_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 26754, RefRangeEnd = 26760, XrefRangeStart = 26670, XrefRangeEnd = 26754, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool ParseProperty(List<string> tokens, out string name, out string value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tokens);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseProperty_Private_Static_Boolean_List_1_String_byref_String_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		name = IL2CPP.Il2CppStringToManaged((System.IntPtr)num2);
		value = IL2CPP.Il2CppStringToManaged((System.IntPtr)num4);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26833, RefRangeEnd = 26834, XrefRangeStart = 26760, XrefRangeEnd = 26833, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ParseParenValue(List<string> tokens)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tokens);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseParenValue_Private_Static_String_List_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 26872, RefRangeEnd = 26874, XrefRangeStart = 26834, XrefRangeEnd = 26872, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<string> Tokenize(string cssText)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(cssText);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Tokenize_Public_Static_List_1_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26874, XrefRangeEnd = 26880, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string PeekToken(List<string> tokens)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tokens);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PeekToken_Private_Static_String_List_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 26888, RefRangeEnd = 26895, XrefRangeStart = 26880, XrefRangeEnd = 26888, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string PopToken(List<string> tokens)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tokens);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PopToken_Private_Static_String_List_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool IsSeparator(char ch)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&ch);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsSeparator_Private_Static_Boolean_Char_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool IsWhitespace(char ch)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&ch);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsWhitespace_Private_Static_Boolean_Char_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public SVGStyleSheetUtils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
