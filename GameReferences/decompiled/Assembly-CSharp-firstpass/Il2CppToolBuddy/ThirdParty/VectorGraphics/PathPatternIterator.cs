using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class PathPatternIterator : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_pattern;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentSegment;

	private static readonly System.IntPtr NativeFieldInfoPtr_solid;

	private static readonly System.IntPtr NativeFieldInfoPtr_segmentLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_patternLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_patternOffset;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppStructArray_1_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Advance_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsSolidAt_Public_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsSolidAt_Public_Boolean_Single_byref_Int32_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SegmentLength_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsSolid_Public_get_Boolean_0;

	public unsafe Il2CppStructArray<float> pattern
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pattern);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pattern)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int currentSegment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSegment);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSegment)) = num;
		}
	}

	public unsafe bool solid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_solid);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_solid)) = flag;
		}
	}

	public unsafe float segmentLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segmentLength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segmentLength)) = num;
		}
	}

	public unsafe float patternLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_patternLength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_patternLength)) = num;
		}
	}

	public unsafe float patternOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_patternOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_patternOffset)) = num;
		}
	}

	public unsafe float SegmentLength
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SegmentLength_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsSolid
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsSolid_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PathPatternIterator()
	{
		Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "PathPatternIterator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr);
		NativeFieldInfoPtr_pattern = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, "pattern");
		NativeFieldInfoPtr_currentSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, "currentSegment");
		NativeFieldInfoPtr_solid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, "solid");
		NativeFieldInfoPtr_segmentLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, "segmentLength");
		NativeFieldInfoPtr_patternLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, "patternLength");
		NativeFieldInfoPtr_patternOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, "patternOffset");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppStructArray_1_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, 100663784);
		NativeMethodInfoPtr_Advance_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, 100663785);
		NativeMethodInfoPtr_IsSolidAt_Public_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, 100663786);
		NativeMethodInfoPtr_IsSolidAt_Public_Boolean_Single_byref_Int32_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, 100663787);
		NativeMethodInfoPtr_get_SegmentLength_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, 100663788);
		NativeMethodInfoPtr_get_IsSolid_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr, 100663789);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 29414, RefRangeEnd = 29416, XrefRangeStart = 29407, XrefRangeEnd = 29414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PathPatternIterator(Il2CppStructArray<float> pattern, float patternOffset = 0f)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathPatternIterator>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pattern);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &patternOffset;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppStructArray_1_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 29416, RefRangeEnd = 29419, XrefRangeStart = 29416, XrefRangeEnd = 29416, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Advance()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Advance_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 29420, RefRangeEnd = 29423, XrefRangeStart = 29419, XrefRangeEnd = 29420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsSolidAt(float unitsFromPathStart)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&unitsFromPathStart);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsSolidAt_Public_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29427, RefRangeEnd = 29428, XrefRangeStart = 29423, XrefRangeEnd = 29427, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsSolidAt(float unitsFromPathStart, out int patternSegmentIndex, out float patternSegmentLength)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&unitsFromPathStart);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref patternSegmentIndex);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref patternSegmentLength);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsSolidAt_Public_Boolean_Single_byref_Int32_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PathPatternIterator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
