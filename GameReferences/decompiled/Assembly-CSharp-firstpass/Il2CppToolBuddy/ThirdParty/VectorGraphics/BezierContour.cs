using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public sealed class BezierContour : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Segments_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Closed_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Segments_Public_get_Il2CppStructArray_1_BezierPathSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Segments_Public_set_Void_Il2CppStructArray_1_BezierPathSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Closed_Public_set_Void_Boolean_0;

	public unsafe Il2CppStructArray<BezierPathSegment> _Segments_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Segments_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Segments_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool _Closed_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Closed_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Closed_k__BackingField)) = flag;
		}
	}

	public unsafe Il2CppStructArray<BezierPathSegment> Segments
	{
		[CallerCount(155)]
		[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Segments_Public_get_Il2CppStructArray_1_BezierPathSegment_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 29034, RefRangeEnd = 29038, XrefRangeStart = 29033, XrefRangeEnd = 29034, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Segments_Public_set_Void_Il2CppStructArray_1_BezierPathSegment_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool Closed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29038, RefRangeEnd = 29039, XrefRangeStart = 29038, XrefRangeEnd = 29038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Closed_Public_set_Void_Boolean_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static BezierContour()
	{
		Il2CppClassPointerStore<BezierContour>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "BezierContour");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BezierContour>.NativeClassPtr);
		NativeFieldInfoPtr__Segments_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BezierContour>.NativeClassPtr, "<Segments>k__BackingField");
		NativeFieldInfoPtr__Closed_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BezierContour>.NativeClassPtr, "<Closed>k__BackingField");
		NativeMethodInfoPtr_get_Segments_Public_get_Il2CppStructArray_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierContour>.NativeClassPtr, 100663666);
		NativeMethodInfoPtr_set_Segments_Public_set_Void_Il2CppStructArray_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierContour>.NativeClassPtr, 100663667);
		NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierContour>.NativeClassPtr, 100663668);
		NativeMethodInfoPtr_set_Closed_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierContour>.NativeClassPtr, 100663669);
	}

	public BezierContour(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public BezierContour()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BezierContour>.NativeClassPtr))
	{
	}
}
