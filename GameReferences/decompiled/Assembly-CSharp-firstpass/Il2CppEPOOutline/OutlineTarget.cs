using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppEPOOutline;

[System.Serializable]
public class OutlineTarget : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_IsVisible;

	private static readonly System.IntPtr NativeFieldInfoPtr_CutoutMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_edgeDilateAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr_frontEdgeDilateAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr_backEdgeDilateAmount;

	private static readonly System.IntPtr NativeFieldInfoPtr_renderer;

	private static readonly System.IntPtr NativeFieldInfoPtr_SubmeshIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_BoundsMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_Bounds;

	private static readonly System.IntPtr NativeFieldInfoPtr_CutoutThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_CullMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_cutoutTextureName;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateRenderingMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_cutoutTextureIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_cutoutTextureId;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Renderer_Public_get_Renderer_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UsesCutout_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CutoutTextureIndex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CutoutTextureIndex_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ShiftedSubmeshIndex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CutoutTextureId_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CutoutTextureName_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CutoutTextureName_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EdgeDilateAmount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EdgeDilateAmount_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FrontEdgeDilateAmount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_FrontEdgeDilateAmount_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_BackEdgeDilateAmount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_BackEdgeDilateAmount_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Renderer_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Renderer_String_Single_0;

	public unsafe bool IsVisible
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsVisible);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsVisible)) = flag;
		}
	}

	public unsafe ColorMask CutoutMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CutoutMask);
			return *(ColorMask*)num;
		}
		set
		{
			*(ColorMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CutoutMask)) = colorMask;
		}
	}

	public unsafe float edgeDilateAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_edgeDilateAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_edgeDilateAmount)) = num;
		}
	}

	public unsafe float frontEdgeDilateAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frontEdgeDilateAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frontEdgeDilateAmount)) = num;
		}
	}

	public unsafe float backEdgeDilateAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_backEdgeDilateAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_backEdgeDilateAmount)) = num;
		}
	}

	public unsafe Renderer renderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Renderer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer));
		}
	}

	public unsafe int SubmeshIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SubmeshIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SubmeshIndex)) = num;
		}
	}

	public unsafe BoundsMode BoundsMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BoundsMode);
			return *(BoundsMode*)num;
		}
		set
		{
			*(BoundsMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BoundsMode)) = boundsMode;
		}
	}

	public unsafe Bounds Bounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bounds);
			return *(Bounds*)num;
		}
		set
		{
			*(Bounds*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bounds)) = bounds;
		}
	}

	public unsafe float CutoutThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CutoutThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CutoutThreshold)) = num;
		}
	}

	public unsafe CullMode CullMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CullMode);
			return *(CullMode*)num;
		}
		set
		{
			*(CullMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CullMode)) = cullMode;
		}
	}

	public unsafe string cutoutTextureName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutoutTextureName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutoutTextureName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe DilateRenderMode DilateRenderingMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateRenderingMode);
			return *(DilateRenderMode*)num;
		}
		set
		{
			*(DilateRenderMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateRenderingMode)) = dilateRenderMode;
		}
	}

	public unsafe int cutoutTextureIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutoutTextureIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutoutTextureIndex)) = num;
		}
	}

	public unsafe Il2CppSystem.Nullable<int> cutoutTextureId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutoutTextureId);
			return new Il2CppSystem.Nullable<int>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<int>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cutoutTextureId), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<int>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe Renderer Renderer
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Renderer_Public_get_Renderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Renderer>(intPtr) : null;
		}
	}

	public unsafe bool UsesCutout
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64159, XrefRangeEnd = 64160, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UsesCutout_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int CutoutTextureIndex
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38226, RefRangeEnd = 38232, XrefRangeStart = 38226, XrefRangeEnd = 38232, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CutoutTextureIndex_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64160, XrefRangeEnd = 64166, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CutoutTextureIndex_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int ShiftedSubmeshIndex
	{
		[CallerCount(149)]
		[CachedScanResults(RefRangeStart = 35494, RefRangeEnd = 35643, XrefRangeStart = 35494, XrefRangeEnd = 35643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShiftedSubmeshIndex_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int CutoutTextureId
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 64174, RefRangeEnd = 64178, XrefRangeStart = 64166, XrefRangeEnd = 64174, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CutoutTextureId_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe string CutoutTextureName
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CutoutTextureName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64178, XrefRangeEnd = 64179, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CutoutTextureName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EdgeDilateAmount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EdgeDilateAmount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EdgeDilateAmount_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float FrontEdgeDilateAmount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FrontEdgeDilateAmount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_FrontEdgeDilateAmount_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float BackEdgeDilateAmount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BackEdgeDilateAmount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BackEdgeDilateAmount_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static OutlineTarget()
	{
		Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "OutlineTarget");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr);
		NativeFieldInfoPtr_IsVisible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "IsVisible");
		NativeFieldInfoPtr_CutoutMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "CutoutMask");
		NativeFieldInfoPtr_edgeDilateAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "edgeDilateAmount");
		NativeFieldInfoPtr_frontEdgeDilateAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "frontEdgeDilateAmount");
		NativeFieldInfoPtr_backEdgeDilateAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "backEdgeDilateAmount");
		NativeFieldInfoPtr_renderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "renderer");
		NativeFieldInfoPtr_SubmeshIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "SubmeshIndex");
		NativeFieldInfoPtr_BoundsMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "BoundsMode");
		NativeFieldInfoPtr_Bounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "Bounds");
		NativeFieldInfoPtr_CutoutThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "CutoutThreshold");
		NativeFieldInfoPtr_CullMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "CullMode");
		NativeFieldInfoPtr_cutoutTextureName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "cutoutTextureName");
		NativeFieldInfoPtr_DilateRenderingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "DilateRenderingMode");
		NativeFieldInfoPtr_cutoutTextureIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "cutoutTextureIndex");
		NativeFieldInfoPtr_cutoutTextureId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, "cutoutTextureId");
		NativeMethodInfoPtr_get_Renderer_Public_get_Renderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667427);
		NativeMethodInfoPtr_get_UsesCutout_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667428);
		NativeMethodInfoPtr_get_CutoutTextureIndex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667429);
		NativeMethodInfoPtr_set_CutoutTextureIndex_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667430);
		NativeMethodInfoPtr_get_ShiftedSubmeshIndex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667431);
		NativeMethodInfoPtr_get_CutoutTextureId_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667432);
		NativeMethodInfoPtr_get_CutoutTextureName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667433);
		NativeMethodInfoPtr_set_CutoutTextureName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667434);
		NativeMethodInfoPtr_get_EdgeDilateAmount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667435);
		NativeMethodInfoPtr_set_EdgeDilateAmount_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667436);
		NativeMethodInfoPtr_get_FrontEdgeDilateAmount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667437);
		NativeMethodInfoPtr_set_FrontEdgeDilateAmount_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667438);
		NativeMethodInfoPtr_get_BackEdgeDilateAmount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667439);
		NativeMethodInfoPtr_set_BackEdgeDilateAmount_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667440);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667441);
		NativeMethodInfoPtr__ctor_Public_Void_Renderer_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667442);
		NativeMethodInfoPtr__ctor_Public_Void_Renderer_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr, 100667443);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64179, XrefRangeEnd = 64184, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OutlineTarget()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 64195, RefRangeEnd = 64199, XrefRangeStart = 64184, XrefRangeEnd = 64195, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OutlineTarget(Renderer renderer, int submesh = 0)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &submesh;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Renderer_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64199, XrefRangeEnd = 64212, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OutlineTarget(Renderer renderer, string cutoutTextureName, float cutoutThreshold = 0.5f)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutlineTarget>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(cutoutTextureName);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &cutoutThreshold;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Renderer_String_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public OutlineTarget(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
