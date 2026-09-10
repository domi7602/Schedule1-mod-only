using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public static class Geom : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_IsWindingInside_Public_Static_Boolean_WindingRule_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_VertCCW_Public_Static_Boolean_Vertex_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_VertEq_Public_Static_Boolean_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_VertLeq_Public_Static_Boolean_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EdgeEval_Public_Static_Single_Vertex_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EdgeSign_Public_Static_Single_Vertex_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransLeq_Public_Static_Boolean_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransEval_Public_Static_Single_Vertex_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransSign_Public_Static_Single_Vertex_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EdgeGoesLeft_Public_Static_Boolean_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EdgeGoesRight_Public_Static_Boolean_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_VertL1dist_Public_Static_Single_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddWinding_Public_Static_Void_Edge_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interpolate_Public_Static_Single_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Swap_Private_Static_Void_byref_Vertex_byref_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EdgeIntersect_Public_Static_Void_Vertex_Vertex_Vertex_Vertex_Vertex_0;

	static Geom()
	{
		Il2CppClassPointerStore<Geom>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "Geom");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Geom>.NativeClassPtr);
		NativeMethodInfoPtr_IsWindingInside_Public_Static_Boolean_WindingRule_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667083);
		NativeMethodInfoPtr_VertCCW_Public_Static_Boolean_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667084);
		NativeMethodInfoPtr_VertEq_Public_Static_Boolean_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667085);
		NativeMethodInfoPtr_VertLeq_Public_Static_Boolean_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667086);
		NativeMethodInfoPtr_EdgeEval_Public_Static_Single_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667087);
		NativeMethodInfoPtr_EdgeSign_Public_Static_Single_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667088);
		NativeMethodInfoPtr_TransLeq_Public_Static_Boolean_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667089);
		NativeMethodInfoPtr_TransEval_Public_Static_Single_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667090);
		NativeMethodInfoPtr_TransSign_Public_Static_Single_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667091);
		NativeMethodInfoPtr_EdgeGoesLeft_Public_Static_Boolean_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667092);
		NativeMethodInfoPtr_EdgeGoesRight_Public_Static_Boolean_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667093);
		NativeMethodInfoPtr_VertL1dist_Public_Static_Single_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667094);
		NativeMethodInfoPtr_AddWinding_Public_Static_Void_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667095);
		NativeMethodInfoPtr_Interpolate_Public_Static_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667096);
		NativeMethodInfoPtr_Swap_Private_Static_Void_byref_Vertex_byref_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667097);
		NativeMethodInfoPtr_EdgeIntersect_Public_Static_Void_Vertex_Vertex_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Geom>.NativeClassPtr, 100667098);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 59268, RefRangeEnd = 59275, XrefRangeStart = 59268, XrefRangeEnd = 59268, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsWindingInside(WindingRule rule, int n)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rule);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &n;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsWindingInside_Public_Static_Boolean_WindingRule_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool VertCCW(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)u);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VertCCW_Public_Static_Boolean_Vertex_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool VertEq(MeshUtils.Vertex lhs, MeshUtils.Vertex rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lhs);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rhs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VertEq_Public_Static_Boolean_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool VertLeq(MeshUtils.Vertex lhs, MeshUtils.Vertex rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lhs);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rhs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VertLeq_Public_Static_Boolean_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float EdgeEval(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)u);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EdgeEval_Public_Static_Single_Vertex_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float EdgeSign(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)u);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EdgeSign_Public_Static_Single_Vertex_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool TransLeq(MeshUtils.Vertex lhs, MeshUtils.Vertex rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lhs);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rhs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransLeq_Public_Static_Boolean_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float TransEval(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)u);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransEval_Public_Static_Single_Vertex_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float TransSign(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)u);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransSign_Public_Static_Single_Vertex_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool EdgeGoesLeft(MeshUtils.Edge e)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EdgeGoesLeft_Public_Static_Boolean_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool EdgeGoesRight(MeshUtils.Edge e)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EdgeGoesRight_Public_Static_Boolean_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59275, XrefRangeEnd = 59278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float VertL1dist(MeshUtils.Vertex u, MeshUtils.Vertex v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)u);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VertL1dist_Public_Static_Single_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static void AddWinding(MeshUtils.Edge eDst, MeshUtils.Edge eSrc)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eDst);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eSrc);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddWinding_Public_Static_Void_Edge_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static float Interpolate(float a, float x, float b, float y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Interpolate_Public_Static_Single_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59278, XrefRangeEnd = 59280, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Swap(ref MeshUtils.Vertex a, ref MeshUtils.Vertex b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		*ptr = (nint)(&intPtr);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)b);
		*(System.IntPtr**)num = &intPtr2;
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Swap_Private_Static_Void_byref_Vertex_byref_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		System.IntPtr intPtr5 = intPtr;
		a = ((intPtr5 == (System.IntPtr)0) ? null : new MeshUtils.Vertex(intPtr5));
		System.IntPtr intPtr6 = intPtr2;
		b = ((intPtr6 == (System.IntPtr)0) ? null : new MeshUtils.Vertex(intPtr6));
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59312, RefRangeEnd = 59313, XrefRangeStart = 59280, XrefRangeEnd = 59312, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void EdgeIntersect(MeshUtils.Vertex o1, MeshUtils.Vertex d1, MeshUtils.Vertex o2, MeshUtils.Vertex d2, MeshUtils.Vertex v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o1);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)d1);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o2);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)d2);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EdgeIntersect_Public_Static_Void_Vertex_Vertex_Vertex_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Geom(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
