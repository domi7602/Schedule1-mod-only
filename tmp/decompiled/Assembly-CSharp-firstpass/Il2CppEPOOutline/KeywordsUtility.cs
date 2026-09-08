using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppEPOOutline;

public static class KeywordsUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_BlurTypes;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateQualityKeywords;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBackKeyword_Public_Static_String_ComplexMaskingMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTextureArrayCutoutKeyword_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDilateQualityKeyword_Public_Static_String_DilateQuality_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEnabledInfoBufferKeyword_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEdgeMaskKeyword_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInfoBufferStageKeyword_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBlurKeyword_Public_Static_String_BlurType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCutoutKeyword_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllBlurKeywords_Public_Static_Void_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllDilateKeywords_Public_Static_Void_List_1_String_0;

	public unsafe static Dictionary<BlurType, string> BlurTypes
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BlurTypes, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BlurType, string>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BlurTypes, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static Dictionary<DilateQuality, string> DilateQualityKeywords
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DilateQualityKeywords, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DilateQuality, string>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DilateQualityKeywords, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	static KeywordsUtility()
	{
		Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "KeywordsUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr);
		NativeFieldInfoPtr_BlurTypes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, "BlurTypes");
		NativeFieldInfoPtr_DilateQualityKeywords = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, "DilateQualityKeywords");
		NativeMethodInfoPtr_GetBackKeyword_Public_Static_String_ComplexMaskingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667264);
		NativeMethodInfoPtr_GetTextureArrayCutoutKeyword_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667265);
		NativeMethodInfoPtr_GetDilateQualityKeyword_Public_Static_String_DilateQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667266);
		NativeMethodInfoPtr_GetEnabledInfoBufferKeyword_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667267);
		NativeMethodInfoPtr_GetEdgeMaskKeyword_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667268);
		NativeMethodInfoPtr_GetInfoBufferStageKeyword_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667269);
		NativeMethodInfoPtr_GetBlurKeyword_Public_Static_String_BlurType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667270);
		NativeMethodInfoPtr_GetCutoutKeyword_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667271);
		NativeMethodInfoPtr_GetAllBlurKeywords_Public_Static_Void_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667272);
		NativeMethodInfoPtr_GetAllDilateKeywords_Public_Static_Void_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<KeywordsUtility>.NativeClassPtr, 100667273);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 61685, RefRangeEnd = 61688, XrefRangeStart = 61681, XrefRangeEnd = 61685, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetBackKeyword(ComplexMaskingMode mode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBackKeyword_Public_Static_String_ComplexMaskingMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61688, XrefRangeEnd = 61690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetTextureArrayCutoutKeyword()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTextureArrayCutoutKeyword_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61690, XrefRangeEnd = 61694, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetDilateQualityKeyword(DilateQuality quality)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&quality);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDilateQualityKeyword_Public_Static_String_DilateQuality_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61694, XrefRangeEnd = 61696, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetEnabledInfoBufferKeyword()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEnabledInfoBufferKeyword_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61696, XrefRangeEnd = 61698, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetEdgeMaskKeyword()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEdgeMaskKeyword_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61698, XrefRangeEnd = 61700, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetInfoBufferStageKeyword()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInfoBufferStageKeyword_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61700, XrefRangeEnd = 61708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetBlurKeyword(BlurType type)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBlurKeyword_Public_Static_String_BlurType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61708, XrefRangeEnd = 61710, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetCutoutKeyword()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCutoutKeyword_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61736, RefRangeEnd = 61737, XrefRangeStart = 61710, XrefRangeEnd = 61736, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GetAllBlurKeywords(List<string> list)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAllBlurKeywords_Public_Static_Void_List_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61763, RefRangeEnd = 61764, XrefRangeStart = 61737, XrefRangeEnd = 61763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GetAllDilateKeywords(List<string> list)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAllDilateKeywords_Public_Static_Void_List_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public KeywordsUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
