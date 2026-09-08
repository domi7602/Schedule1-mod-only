using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public class PolygonHelper : MonoBehaviour
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Plane2D
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_normal;

		private static readonly System.IntPtr NativeFieldInfoPtr_distance;

		private static readonly System.IntPtr NativeMethodInfoPtr_Distance_Public_Single_Vector2_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ClosestPoint_Public_Vector2_Vector2_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Intersect_Public_Vector2_Vector2_Vector2_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetSide_Public_Boolean_Vector2_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_FromPoints_Public_Static_Plane2D_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_FromNormalAndPoint_Public_Static_Plane2D_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Flip_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CutConvex_Public_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

		[FieldOffset(0)]
		public Vector2 normal;

		[FieldOffset(8)]
		public float distance;

		static Plane2D()
		{
			Il2CppClassPointerStore<Plane2D>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PolygonHelper>.NativeClassPtr, "Plane2D");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Plane2D>.NativeClassPtr);
			NativeFieldInfoPtr_normal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, "normal");
			NativeFieldInfoPtr_distance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, "distance");
			NativeMethodInfoPtr_Distance_Public_Single_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664100);
			NativeMethodInfoPtr_ClosestPoint_Public_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664101);
			NativeMethodInfoPtr_Intersect_Public_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664102);
			NativeMethodInfoPtr_GetSide_Public_Boolean_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664103);
			NativeMethodInfoPtr_FromPoints_Public_Static_Plane2D_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664104);
			NativeMethodInfoPtr_FromNormalAndPoint_Public_Static_Plane2D_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664105);
			NativeMethodInfoPtr_Flip_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664106);
			NativeMethodInfoPtr_CutConvex_Public_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664107);
			NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, 100664108);
		}

		[CallerCount(0)]
		public unsafe float Distance(Vector2 point)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&point);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Distance_Public_Single_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Vector2 ClosestPoint(Vector2 pt)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&pt);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClosestPoint_Public_Vector2_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 72043, RefRangeEnd = 72045, XrefRangeStart = 72041, XrefRangeEnd = 72043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Vector2 Intersect(Vector2 p1, Vector2 p2)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&p1);
			*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Intersect_Public_Vector2_Vector2_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe bool GetSide(Vector2 point)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&point);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSide_Public_Boolean_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 72046, RefRangeEnd = 72047, XrefRangeStart = 72045, XrefRangeEnd = 72046, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Plane2D FromPoints(Vector3 p1, Vector3 p2)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&p1);
			*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FromPoints_Public_Static_Plane2D_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Plane2D*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe static Plane2D FromNormalAndPoint(Vector3 normalizedNormal, Vector3 p1)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&normalizedNormal);
			*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FromNormalAndPoint_Public_Static_Plane2D_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Plane2D*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 72047, RefRangeEnd = 72048, XrefRangeStart = 72047, XrefRangeEnd = 72047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Flip()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Flip_Public_Void_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 72069, RefRangeEnd = 72070, XrefRangeStart = 72048, XrefRangeEnd = 72069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Il2CppStructArray<Vector2> CutConvex(Il2CppStructArray<Vector2> poly)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)poly);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CutConvex_Public_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 72070, XrefRangeEnd = 72080, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override string ToString()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Plane2D>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static PolygonHelper()
	{
		Il2CppClassPointerStore<PolygonHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "PolygonHelper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PolygonHelper>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PolygonHelper>.NativeClassPtr, 100664099);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PolygonHelper()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PolygonHelper>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PolygonHelper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
