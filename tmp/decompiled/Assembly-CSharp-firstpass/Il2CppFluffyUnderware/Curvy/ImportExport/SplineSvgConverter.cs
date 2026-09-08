using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppToolBuddy.ThirdParty.VectorGraphics;

namespace Il2CppFluffyUnderware.Curvy.ImportExport;

public static class SplineSvgConverter : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_SvgToSplines_Public_Static_Il2CppReferenceArray_1_CurvySpline_String_CurvySerializationSpace_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SvgToSpline_Public_Static_CurvySpline_String_CurvySerializationSpace_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SvgToSerializedSplines_Public_Static_List_1_SerializedCurvySpline_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DrawNode_Private_Static_Void_SceneNode_Matrix2D_List_1_SerializedCurvySpline_0;

	static SplineSvgConverter()
	{
		Il2CppClassPointerStore<SplineSvgConverter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ImportExport", "SplineSvgConverter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SplineSvgConverter>.NativeClassPtr);
		NativeMethodInfoPtr_SvgToSplines_Public_Static_Il2CppReferenceArray_1_CurvySpline_String_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSvgConverter>.NativeClassPtr, 100665347);
		NativeMethodInfoPtr_SvgToSpline_Public_Static_CurvySpline_String_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSvgConverter>.NativeClassPtr, 100665348);
		NativeMethodInfoPtr_SvgToSerializedSplines_Public_Static_List_1_SerializedCurvySpline_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSvgConverter>.NativeClassPtr, 100665349);
		NativeMethodInfoPtr_DrawNode_Private_Static_Void_SceneNode_Matrix2D_List_1_SerializedCurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSvgConverter>.NativeClassPtr, 100665350);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44345, XrefRangeEnd = 44358, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<CurvySpline> SvgToSplines(string svg, CurvySerializationSpace coordinatesSpace = CurvySerializationSpace.Global)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(svg);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coordinatesSpace;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SvgToSplines_Public_Static_Il2CppReferenceArray_1_CurvySpline_String_CurvySerializationSpace_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvySpline>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44358, XrefRangeEnd = 44378, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CurvySpline SvgToSpline(string svg, CurvySerializationSpace coordinatesSpace = CurvySerializationSpace.Global)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(svg);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &coordinatesSpace;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SvgToSpline_Public_Static_CurvySpline_String_CurvySerializationSpace_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 44412, RefRangeEnd = 44414, XrefRangeStart = 44378, XrefRangeEnd = 44412, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<SerializedCurvySpline> SvgToSerializedSplines(string svg, bool invertY = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(svg);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &invertY;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SvgToSerializedSplines_Public_Static_List_1_SerializedCurvySpline_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SerializedCurvySpline>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 44511, RefRangeEnd = 44513, XrefRangeStart = 44414, XrefRangeEnd = 44511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DrawNode(SceneNode node, Matrix2D rootTransform, List<SerializedCurvySpline> splines)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rootTransform;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splines);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawNode_Private_Static_Void_SceneNode_Matrix2D_List_1_SerializedCurvySpline_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SplineSvgConverter(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
