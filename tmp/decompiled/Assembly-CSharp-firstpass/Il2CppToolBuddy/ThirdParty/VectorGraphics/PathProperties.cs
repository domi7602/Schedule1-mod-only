using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public sealed class PathProperties : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Stroke_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Head_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Tail_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Corners_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Stroke_Public_get_Stroke_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Stroke_Public_set_Void_Stroke_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Head_Public_get_PathEnding_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Head_Public_set_Void_PathEnding_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Tail_Public_get_PathEnding_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Tail_Public_set_Void_PathEnding_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Corners_Public_get_PathCorner_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Corners_Public_set_Void_PathCorner_0;

	public unsafe Stroke _Stroke_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stroke_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stroke>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stroke_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stroke));
		}
	}

	public unsafe PathEnding _Head_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Head_k__BackingField);
			return *(PathEnding*)num;
		}
		set
		{
			*(PathEnding*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Head_k__BackingField)) = pathEnding;
		}
	}

	public unsafe PathEnding _Tail_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Tail_k__BackingField);
			return *(PathEnding*)num;
		}
		set
		{
			*(PathEnding*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Tail_k__BackingField)) = pathEnding;
		}
	}

	public unsafe PathCorner _Corners_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Corners_k__BackingField);
			return *(PathCorner*)num;
		}
		set
		{
			*(PathCorner*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Corners_k__BackingField)) = pathCorner;
		}
	}

	public unsafe Stroke Stroke
	{
		[CallerCount(155)]
		[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Stroke_Public_get_Stroke_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stroke>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 29034, RefRangeEnd = 29038, XrefRangeStart = 29034, XrefRangeEnd = 29038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Stroke_Public_set_Void_Stroke_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PathEnding Head
	{
		[CallerCount(28)]
		[CachedScanResults(RefRangeStart = 29148, RefRangeEnd = 29176, XrefRangeStart = 29148, XrefRangeEnd = 29148, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Head_Public_get_PathEnding_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(PathEnding*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 29176, RefRangeEnd = 29187, XrefRangeStart = 29176, XrefRangeEnd = 29176, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Head_Public_set_Void_PathEnding_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PathEnding Tail
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 29187, RefRangeEnd = 29190, XrefRangeStart = 29187, XrefRangeEnd = 29187, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Tail_Public_get_PathEnding_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(PathEnding*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 29190, RefRangeEnd = 29194, XrefRangeStart = 29190, XrefRangeEnd = 29190, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Tail_Public_set_Void_PathEnding_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PathCorner Corners
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29049, RefRangeEnd = 29051, XrefRangeStart = 29049, XrefRangeEnd = 29051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Corners_Public_get_PathCorner_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(PathCorner*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Corners_Public_set_Void_PathCorner_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static PathProperties()
	{
		Il2CppClassPointerStore<PathProperties>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "PathProperties");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathProperties>.NativeClassPtr);
		NativeFieldInfoPtr__Stroke_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, "<Stroke>k__BackingField");
		NativeFieldInfoPtr__Head_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, "<Head>k__BackingField");
		NativeFieldInfoPtr__Tail_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, "<Tail>k__BackingField");
		NativeFieldInfoPtr__Corners_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, "<Corners>k__BackingField");
		NativeMethodInfoPtr_get_Stroke_Public_get_Stroke_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663727);
		NativeMethodInfoPtr_set_Stroke_Public_set_Void_Stroke_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663728);
		NativeMethodInfoPtr_get_Head_Public_get_PathEnding_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663729);
		NativeMethodInfoPtr_set_Head_Public_set_Void_PathEnding_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663730);
		NativeMethodInfoPtr_get_Tail_Public_get_PathEnding_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663731);
		NativeMethodInfoPtr_set_Tail_Public_set_Void_PathEnding_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663732);
		NativeMethodInfoPtr_get_Corners_Public_get_PathCorner_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663733);
		NativeMethodInfoPtr_set_Corners_Public_set_Void_PathCorner_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathProperties>.NativeClassPtr, 100663734);
	}

	public PathProperties(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public PathProperties()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathProperties>.NativeClassPtr))
	{
	}
}
