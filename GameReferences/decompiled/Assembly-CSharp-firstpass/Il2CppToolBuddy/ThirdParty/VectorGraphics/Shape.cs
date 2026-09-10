using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class Shape : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Contours_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Fill_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_FillTransform;

	private static readonly System.IntPtr NativeFieldInfoPtr__PathProps_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__IsConvex_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Contours_Public_get_Il2CppReferenceArray_1_BezierContour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Contours_Public_set_Void_Il2CppReferenceArray_1_BezierContour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Fill_Public_get_IFill_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Fill_Public_set_Void_IFill_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FillTransform_Public_get_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_FillTransform_Public_set_Void_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PathProps_Public_get_PathProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_PathProps_Public_set_Void_PathProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsConvex_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_IsConvex_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<BezierContour> _Contours_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Contours_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<BezierContour>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Contours_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe IFill _Fill_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Fill_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IFill>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Fill_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fill));
		}
	}

	public unsafe Matrix2D m_FillTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FillTransform);
			return *(Matrix2D*)num;
		}
		set
		{
			*(Matrix2D*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FillTransform)) = matrix2D;
		}
	}

	public unsafe PathProperties _PathProps_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PathProps_k__BackingField);
			return new PathProperties(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PathProps_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathProperties)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe bool _IsConvex_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsConvex_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsConvex_k__BackingField)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<BezierContour> Contours
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Contours_Public_get_Il2CppReferenceArray_1_BezierContour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<BezierContour>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29107, RefRangeEnd = 29109, XrefRangeStart = 29107, XrefRangeEnd = 29109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Contours_Public_set_Void_Il2CppReferenceArray_1_BezierContour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe IFill Fill
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Fill_Public_get_IFill_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IFill>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Fill_Public_set_Void_IFill_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Matrix2D FillTransform
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FillTransform_Public_get_Matrix2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_FillTransform_Public_set_Void_Matrix2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PathProperties PathProps
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PathProps_Public_get_PathProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new PathProperties(pointer);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29194, XrefRangeEnd = 29195, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_PathProps_Public_set_Void_PathProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsConvex
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsConvex_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsConvex_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Shape()
	{
		Il2CppClassPointerStore<Shape>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "Shape");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Shape>.NativeClassPtr);
		NativeFieldInfoPtr__Contours_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shape>.NativeClassPtr, "<Contours>k__BackingField");
		NativeFieldInfoPtr__Fill_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shape>.NativeClassPtr, "<Fill>k__BackingField");
		NativeFieldInfoPtr_m_FillTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shape>.NativeClassPtr, "m_FillTransform");
		NativeFieldInfoPtr__PathProps_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shape>.NativeClassPtr, "<PathProps>k__BackingField");
		NativeFieldInfoPtr__IsConvex_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Shape>.NativeClassPtr, "<IsConvex>k__BackingField");
		NativeMethodInfoPtr_get_Contours_Public_get_Il2CppReferenceArray_1_BezierContour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663735);
		NativeMethodInfoPtr_set_Contours_Public_set_Void_Il2CppReferenceArray_1_BezierContour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663736);
		NativeMethodInfoPtr_get_Fill_Public_get_IFill_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663737);
		NativeMethodInfoPtr_set_Fill_Public_set_Void_IFill_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663738);
		NativeMethodInfoPtr_get_FillTransform_Public_get_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663739);
		NativeMethodInfoPtr_set_FillTransform_Public_set_Void_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663740);
		NativeMethodInfoPtr_get_PathProps_Public_get_PathProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663741);
		NativeMethodInfoPtr_set_PathProps_Public_set_Void_PathProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663742);
		NativeMethodInfoPtr_get_IsConvex_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663743);
		NativeMethodInfoPtr_set_IsConvex_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663744);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Shape>.NativeClassPtr, 100663745);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 29203, RefRangeEnd = 29214, XrefRangeStart = 29195, XrefRangeEnd = 29203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Shape()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Shape>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Shape(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
