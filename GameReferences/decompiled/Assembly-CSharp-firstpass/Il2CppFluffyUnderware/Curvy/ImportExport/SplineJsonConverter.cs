using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFluffyUnderware.Curvy.ImportExport;

public static class SplineJsonConverter : Il2CppSystem.Object
{
	[ObfuscatedName("FluffyUnderware.Curvy.ImportExport.SplineJsonConverter+<>c__DisplayClass0_0")]
	public sealed class __c__DisplayClass0_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_coordinatesSpace;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SplinesToJson_b__0_Internal_SerializedCurvySpline_CurvySpline_0;

		public unsafe CurvySerializationSpace coordinatesSpace
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coordinatesSpace);
				return *(CurvySerializationSpace*)num;
			}
			set
			{
				*(CurvySerializationSpace*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coordinatesSpace)) = curvySerializationSpace;
			}
		}

		static __c__DisplayClass0_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass0_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr, "<>c__DisplayClass0_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass0_0>.NativeClassPtr);
			NativeFieldInfoPtr_coordinatesSpace = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass0_0>.NativeClassPtr, "coordinatesSpace");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass0_0>.NativeClassPtr, 100665345);
			NativeMethodInfoPtr__SplinesToJson_b__0_Internal_SerializedCurvySpline_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass0_0>.NativeClassPtr, 100665346);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass0_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass0_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44225, XrefRangeEnd = 44229, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SerializedCurvySpline _SplinesToJson_b__0(CurvySpline s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SplinesToJson_b__0_Internal_SerializedCurvySpline_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SerializedCurvySpline>(intPtr) : null;
		}

		public __c__DisplayClass0_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_SplinesToJson_Public_Static_String_IEnumerable_1_CurvySpline_CurvySerializationSpace_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SplineToJson_Public_Static_String_CurvySpline_CurvySerializationSpace_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_JsonToSplines_Public_Static_Il2CppReferenceArray_1_CurvySpline_String_CurvySerializationSpace_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_JsonToSpline_Public_Static_CurvySpline_String_CurvySerializationSpace_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_JsonToSerializedSplines_Public_Static_Il2CppReferenceArray_1_SerializedCurvySpline_String_0;

	static SplineJsonConverter()
	{
		Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ImportExport", "SplineJsonConverter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr);
		NativeMethodInfoPtr_SplinesToJson_Public_Static_String_IEnumerable_1_CurvySpline_CurvySerializationSpace_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr, 100665340);
		NativeMethodInfoPtr_SplineToJson_Public_Static_String_CurvySpline_CurvySerializationSpace_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr, 100665341);
		NativeMethodInfoPtr_JsonToSplines_Public_Static_Il2CppReferenceArray_1_CurvySpline_String_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr, 100665342);
		NativeMethodInfoPtr_JsonToSpline_Public_Static_CurvySpline_String_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr, 100665343);
		NativeMethodInfoPtr_JsonToSerializedSplines_Public_Static_Il2CppReferenceArray_1_SerializedCurvySpline_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineJsonConverter>.NativeClassPtr, 100665344);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44229, XrefRangeEnd = 44254, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string SplinesToJson(IEnumerable<CurvySpline> splines, CurvySerializationSpace coordinatesSpace = CurvySerializationSpace.Global, bool prettify = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splines);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coordinatesSpace;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &prettify;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SplinesToJson_Public_Static_String_IEnumerable_1_CurvySpline_CurvySerializationSpace_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44254, XrefRangeEnd = 44287, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string SplineToJson(CurvySpline spline, CurvySerializationSpace coordinatesSpace = CurvySerializationSpace.Global, bool prettify = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coordinatesSpace;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &prettify;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SplineToJson_Public_Static_String_CurvySpline_CurvySerializationSpace_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44287, XrefRangeEnd = 44296, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<CurvySpline> JsonToSplines(string json, CurvySerializationSpace coordinatesSpace = CurvySerializationSpace.Global)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(json);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coordinatesSpace;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_JsonToSplines_Public_Static_Il2CppReferenceArray_1_CurvySpline_String_CurvySerializationSpace_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvySpline>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44296, XrefRangeEnd = 44312, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CurvySpline JsonToSpline(string json, CurvySerializationSpace coordinatesSpace = CurvySerializationSpace.Global)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(json);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coordinatesSpace;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_JsonToSpline_Public_Static_CurvySpline_String_CurvySerializationSpace_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 44343, RefRangeEnd = 44345, XrefRangeStart = 44312, XrefRangeEnd = 44343, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<SerializedCurvySpline> JsonToSerializedSplines(string json)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(json);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_JsonToSerializedSplines_Public_Static_Il2CppReferenceArray_1_SerializedCurvySpline_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SerializedCurvySpline>>(intPtr) : null;
	}

	public SplineJsonConverter(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
