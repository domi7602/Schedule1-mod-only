using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public class LTMesh : MeshUtils.Pooled<LTMesh>
{
	private static readonly IntPtr NativeFieldInfoPtr__vHead;

	private static readonly IntPtr NativeFieldInfoPtr__fHead;

	private static readonly IntPtr NativeFieldInfoPtr__eHead;

	private static readonly IntPtr NativeFieldInfoPtr__eHeadSym;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnFree_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_MakeEdge_Public_Edge_0;

	private static readonly IntPtr NativeMethodInfoPtr_Splice_Public_Void_Edge_Edge_0;

	private static readonly IntPtr NativeMethodInfoPtr_Delete_Public_Void_Edge_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddEdgeVertex_Public_Edge_Edge_0;

	private static readonly IntPtr NativeMethodInfoPtr_SplitEdge_Public_Edge_Edge_0;

	private static readonly IntPtr NativeMethodInfoPtr_Connect_Public_Edge_Edge_Edge_0;

	private static readonly IntPtr NativeMethodInfoPtr_ZapFace_Public_Void_Face_0;

	private static readonly IntPtr NativeMethodInfoPtr_MergeConvexFaces_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_Check_Public_Void_0;

	public unsafe MeshUtils.Vertex _vHead
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__vHead);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Vertex>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__vHead)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertex));
		}
	}

	public unsafe MeshUtils.Face _fHead
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fHead);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Face>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fHead)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)face));
		}
	}

	public unsafe MeshUtils.Edge _eHead
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eHead);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eHead)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
		}
	}

	public unsafe MeshUtils.Edge _eHeadSym
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eHeadSym);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eHeadSym)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
		}
	}

	static LTMesh()
	{
		Il2CppClassPointerStore<LTMesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "LTMesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LTMesh>.NativeClassPtr);
		NativeFieldInfoPtr__vHead = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, "_vHead");
		NativeFieldInfoPtr__fHead = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, "_fHead");
		NativeFieldInfoPtr__eHead = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, "_eHead");
		NativeFieldInfoPtr__eHeadSym = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, "_eHeadSym");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667099);
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667100);
		NativeMethodInfoPtr_OnFree_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667101);
		NativeMethodInfoPtr_MakeEdge_Public_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667102);
		NativeMethodInfoPtr_Splice_Public_Void_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667103);
		NativeMethodInfoPtr_Delete_Public_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667104);
		NativeMethodInfoPtr_AddEdgeVertex_Public_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667105);
		NativeMethodInfoPtr_SplitEdge_Public_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667106);
		NativeMethodInfoPtr_Connect_Public_Edge_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667107);
		NativeMethodInfoPtr_ZapFace_Public_Void_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667108);
		NativeMethodInfoPtr_MergeConvexFaces_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667109);
		NativeMethodInfoPtr_Check_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LTMesh>.NativeClassPtr, 100667110);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59358, RefRangeEnd = 59359, XrefRangeStart = 59313, XrefRangeEnd = 59358, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LTMesh()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LTMesh>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59359, XrefRangeEnd = 59363, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59363, XrefRangeEnd = 59372, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnFree()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnFree_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59376, RefRangeEnd = 59377, XrefRangeStart = 59372, XrefRangeEnd = 59376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshUtils.Edge MakeEdge()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeEdge_Public_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 59387, RefRangeEnd = 59411, XrefRangeStart = 59377, XrefRangeEnd = 59387, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Splice(MeshUtils.Edge eOrg, MeshUtils.Edge eDst)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eOrg);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eDst);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Splice_Public_Void_Edge_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(19)]
	[CachedScanResults(RefRangeStart = 59432, RefRangeEnd = 59451, XrefRangeStart = 59411, XrefRangeEnd = 59432, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Delete(MeshUtils.Edge eDel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eDel);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Delete_Public_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59451, XrefRangeEnd = 59460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshUtils.Edge AddEdgeVertex(MeshUtils.Edge eOrg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eOrg);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddEdgeVertex_Public_Edge_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 59480, RefRangeEnd = 59486, XrefRangeStart = 59460, XrefRangeEnd = 59480, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshUtils.Edge SplitEdge(MeshUtils.Edge eOrg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eOrg);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SplitEdge_Public_Edge_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 59502, RefRangeEnd = 59512, XrefRangeStart = 59486, XrefRangeEnd = 59502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshUtils.Edge Connect(MeshUtils.Edge eOrg, MeshUtils.Edge eDst)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eOrg);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eDst);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Connect_Public_Edge_Edge_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59545, RefRangeEnd = 59546, XrefRangeStart = 59512, XrefRangeEnd = 59545, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ZapFace(MeshUtils.Face fZap)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fZap);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ZapFace_Public_Void_Face_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59549, RefRangeEnd = 59550, XrefRangeStart = 59546, XrefRangeEnd = 59549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeConvexFaces(int maxVertsPerFace)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&maxVertsPerFace);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeConvexFaces_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59550, XrefRangeEnd = 59553, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Check()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Check_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LTMesh(IntPtr pointer)
		: base(pointer)
	{
	}
}
