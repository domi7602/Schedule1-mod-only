using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGAttribParser : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "AttribPath")]
	public enum AttribPath
	{
		Path
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "AttribTransform")]
	public enum AttribTransform
	{
		Transform
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "AttribStroke")]
	public enum AttribStroke
	{
		Stroke
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_currentContour;

	private static readonly System.IntPtr NativeFieldInfoPtr_contours;

	private static readonly System.IntPtr NativeFieldInfoPtr_penPos;

	private static readonly System.IntPtr NativeFieldInfoPtr_attribString;

	private static readonly System.IntPtr NativeFieldInfoPtr_pathCommand;

	private static readonly System.IntPtr NativeFieldInfoPtr_transform;

	private static readonly System.IntPtr NativeFieldInfoPtr_fill;

	private static readonly System.IntPtr NativeFieldInfoPtr_attribName;

	private static readonly System.IntPtr NativeFieldInfoPtr_stringPos;

	private static readonly System.IntPtr NativeFieldInfoPtr_namedColors;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParsePath_Public_Static_List_1_BezierContour_Node_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseTransform_Public_Static_Matrix2D_Node_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseTransform_Public_Static_Matrix2D_Node_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseFill_Public_Static_IFill_Node_SVGDictionary_SVGPostponedFills_SVGStyleResolver_Inheritance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseFill_Public_Static_IFill_Node_SVGDictionary_SVGPostponedFills_SVGStyleResolver_Inheritance_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseStrokeAndOpacity_Public_Static_Stroke_Node_SVGDictionary_SVGStyleResolver_Inheritance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseColor_Public_Static_Color_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseURLRef_Public_Static_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseRelativeRef_Public_Static_Object_String_SVGDictionary_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CleanIri_Public_Static_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Private_Void_String_AttribPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Private_Void_String_String_AttribTransform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Private_Void_String_String_Single_FillMode_SVGDictionary_SVGPostponedFills_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConcludePath_Private_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextVector2_Private_Vector2_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextFloat_Private_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseFloat_Internal_Static_Single_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextBool_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextPathCommand_Private_Char_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextStringCommand_Private_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkipSymbol_Private_Void_Char_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PeekSymbol_Private_Boolean_Char_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkipWhitespaces_Private_Void_0;

	public unsafe LinkedList<BezierSegment> currentContour
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentContour);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<LinkedList<BezierSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentContour)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)linkedList));
		}
	}

	public unsafe List<BezierContour> contours
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_contours);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<BezierContour>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_contours)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Vector2 penPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_penPos);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_penPos)) = vector;
		}
	}

	public unsafe string attribString
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attribString);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attribString)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe char pathCommand
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathCommand);
			return *(char*)num;
		}
		set
		{
			*(char*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathCommand)) = c;
		}
	}

	public unsafe Matrix2D transform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transform);
			return *(Matrix2D*)num;
		}
		set
		{
			*(Matrix2D*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transform)) = matrix2D;
		}
	}

	public unsafe IFill fill
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fill);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IFill>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fill)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fill));
		}
	}

	public unsafe string attribName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attribName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attribName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int stringPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stringPos);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stringPos)) = num;
		}
	}

	public unsafe static NamedWebColorDictionary namedColors
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_namedColors, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<NamedWebColorDictionary>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_namedColors, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)namedWebColorDictionary));
		}
	}

	static SVGAttribParser()
	{
		Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGAttribParser");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr);
		NativeFieldInfoPtr_currentContour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "currentContour");
		NativeFieldInfoPtr_contours = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "contours");
		NativeFieldInfoPtr_penPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "penPos");
		NativeFieldInfoPtr_attribString = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "attribString");
		NativeFieldInfoPtr_pathCommand = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "pathCommand");
		NativeFieldInfoPtr_transform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "transform");
		NativeFieldInfoPtr_fill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "fill");
		NativeFieldInfoPtr_attribName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "attribName");
		NativeFieldInfoPtr_stringPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "stringPos");
		NativeFieldInfoPtr_namedColors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, "namedColors");
		NativeMethodInfoPtr_ParsePath_Public_Static_List_1_BezierContour_Node_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663485);
		NativeMethodInfoPtr_ParseTransform_Public_Static_Matrix2D_Node_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663486);
		NativeMethodInfoPtr_ParseTransform_Public_Static_Matrix2D_Node_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663487);
		NativeMethodInfoPtr_ParseFill_Public_Static_IFill_Node_SVGDictionary_SVGPostponedFills_SVGStyleResolver_Inheritance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663488);
		NativeMethodInfoPtr_ParseFill_Public_Static_IFill_Node_SVGDictionary_SVGPostponedFills_SVGStyleResolver_Inheritance_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663489);
		NativeMethodInfoPtr_ParseStrokeAndOpacity_Public_Static_Stroke_Node_SVGDictionary_SVGStyleResolver_Inheritance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663490);
		NativeMethodInfoPtr_ParseColor_Public_Static_Color_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663491);
		NativeMethodInfoPtr_ParseURLRef_Public_Static_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663492);
		NativeMethodInfoPtr_ParseRelativeRef_Public_Static_Object_String_SVGDictionary_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663493);
		NativeMethodInfoPtr_CleanIri_Public_Static_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663494);
		NativeMethodInfoPtr__ctor_Private_Void_String_AttribPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663495);
		NativeMethodInfoPtr__ctor_Private_Void_String_String_AttribTransform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663496);
		NativeMethodInfoPtr__ctor_Private_Void_String_String_Single_FillMode_SVGDictionary_SVGPostponedFills_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663497);
		NativeMethodInfoPtr_ConcludePath_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663498);
		NativeMethodInfoPtr_NextVector2_Private_Vector2_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663499);
		NativeMethodInfoPtr_NextFloat_Private_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663500);
		NativeMethodInfoPtr_ParseFloat_Internal_Static_Single_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663501);
		NativeMethodInfoPtr_NextBool_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663502);
		NativeMethodInfoPtr_NextPathCommand_Private_Char_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663503);
		NativeMethodInfoPtr_NextStringCommand_Private_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663504);
		NativeMethodInfoPtr_SkipSymbol_Private_Void_Char_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663505);
		NativeMethodInfoPtr_PeekSymbol_Private_Boolean_Char_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663506);
		NativeMethodInfoPtr_SkipWhitespaces_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr, 100663507);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 25066, RefRangeEnd = 25067, XrefRangeStart = 25059, XrefRangeEnd = 25066, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<BezierContour> ParsePath(XmlReaderIterator.Node node)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParsePath_Public_Static_List_1_BezierContour_Node_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<BezierContour>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 25067, XrefRangeEnd = 25070, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D ParseTransform(XmlReaderIterator.Node node)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseTransform_Public_Static_Matrix2D_Node_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 25076, RefRangeEnd = 25092, XrefRangeStart = 25070, XrefRangeEnd = 25076, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D ParseTransform(XmlReaderIterator.Node node, string attribName)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attribName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseTransform_Public_Static_Matrix2D_Node_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 25092, XrefRangeEnd = 25093, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IFill ParseFill(XmlReaderIterator.Node node, SVGDictionary dict, SVGPostponedFills postponedFills, SVGStyleResolver styles, Inheritance inheritance = Inheritance.Inherited)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dict);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)postponedFills);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)styles);
		*(Inheritance**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &inheritance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseFill_Public_Static_IFill_Node_SVGDictionary_SVGPostponedFills_SVGStyleResolver_Inheritance_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IFill>(intPtr) : null;
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 25120, RefRangeEnd = 25128, XrefRangeStart = 25093, XrefRangeEnd = 25120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IFill ParseFill(XmlReaderIterator.Node node, SVGDictionary dict, SVGPostponedFills postponedFills, SVGStyleResolver styles, Inheritance inheritance, out bool isDefaultFill)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dict);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)postponedFills);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)styles);
		*(Inheritance**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &inheritance;
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isDefaultFill);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseFill_Public_Static_IFill_Node_SVGDictionary_SVGPostponedFills_SVGStyleResolver_Inheritance_byref_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IFill>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 25151, RefRangeEnd = 25152, XrefRangeStart = 25128, XrefRangeEnd = 25151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Stroke ParseStrokeAndOpacity(XmlReaderIterator.Node node, SVGDictionary dict, SVGStyleResolver styles, Inheritance inheritance = Inheritance.Inherited)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dict);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)styles);
		*(Inheritance**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &inheritance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseStrokeAndOpacity_Public_Static_Stroke_Node_SVGDictionary_SVGStyleResolver_Inheritance_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stroke>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 25209, RefRangeEnd = 25211, XrefRangeStart = 25152, XrefRangeEnd = 25209, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color ParseColor(string colorString)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(colorString);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseColor_Public_Static_Color_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 25218, RefRangeEnd = 25219, XrefRangeStart = 25211, XrefRangeEnd = 25218, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ParseURLRef(string url)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(url);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseURLRef_Public_Static_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 25226, RefRangeEnd = 25232, XrefRangeStart = 25219, XrefRangeEnd = 25226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Object ParseRelativeRef(string iri, SVGDictionary dict)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iri);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dict);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseRelativeRef_Public_Static_Object_String_SVGDictionary_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 25236, RefRangeEnd = 25238, XrefRangeStart = 25232, XrefRangeEnd = 25236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string CleanIri(string iri)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iri);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CleanIri_Public_Static_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 25375, RefRangeEnd = 25376, XrefRangeStart = 25238, XrefRangeEnd = 25375, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGAttribParser(string attrib, AttribPath attribPath)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attrib);
		*(AttribPath**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &attribPath;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Private_Void_String_AttribPath_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 25469, RefRangeEnd = 25470, XrefRangeStart = 25376, XrefRangeEnd = 25469, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGAttribParser(string attrib, string attribNameVal, AttribTransform attribTransform)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attrib);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attribNameVal);
		*(AttribTransform**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &attribTransform;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Private_Void_String_String_AttribTransform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 25561, RefRangeEnd = 25564, XrefRangeStart = 25470, XrefRangeEnd = 25561, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGAttribParser(string attrib, string attribName, float opacity, FillMode mode, SVGDictionary dict, SVGPostponedFills postponedFills, bool allowReference = true)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGAttribParser>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attrib);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attribName);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &opacity;
		*(FillMode**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dict);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)postponedFills);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &allowReference;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Private_Void_String_String_Single_FillMode_SVGDictionary_SVGPostponedFills_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 25598, RefRangeEnd = 25601, XrefRangeStart = 25564, XrefRangeEnd = 25598, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConcludePath(bool joinEnds)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&joinEnds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConcludePath_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 25603, RefRangeEnd = 25611, XrefRangeStart = 25601, XrefRangeEnd = 25603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 NextVector2(bool relative = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&relative);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextVector2_Private_Vector2_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 25625, RefRangeEnd = 25649, XrefRangeStart = 25611, XrefRangeEnd = 25625, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float NextFloat()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextFloat_Private_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 25654, RefRangeEnd = 25657, XrefRangeStart = 25649, XrefRangeEnd = 25654, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ParseFloat(string s)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(s);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseFloat_Internal_Static_Single_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 25662, RefRangeEnd = 25664, XrefRangeStart = 25657, XrefRangeEnd = 25662, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool NextBool()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextBool_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 25670, RefRangeEnd = 25672, XrefRangeStart = 25664, XrefRangeEnd = 25670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe char NextPathCommand(bool noCommandInheritance = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&noCommandInheritance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextPathCommand_Private_Char_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(char*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 25676, RefRangeEnd = 25677, XrefRangeStart = 25672, XrefRangeEnd = 25676, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string NextStringCommand()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextStringCommand_Private_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 25679, RefRangeEnd = 25681, XrefRangeStart = 25677, XrefRangeEnd = 25679, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SkipSymbol(char s)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&s);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkipSymbol_Private_Void_Char_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 25682, RefRangeEnd = 25685, XrefRangeStart = 25681, XrefRangeEnd = 25682, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool PeekSymbol(char s)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&s);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PeekSymbol_Private_Boolean_Char_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 25687, RefRangeEnd = 25692, XrefRangeStart = 25685, XrefRangeEnd = 25687, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SkipWhitespaces()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkipWhitespaces_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SVGAttribParser(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
