using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Utils;

[System.Serializable]
public class SplinePolyLine : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "VertexCalculation")]
	public enum VertexCalculation
	{
		ByApproximation,
		ByAngle
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Utils.SplinePolyLine+<>c__DisplayClass14_0")]
	public sealed class __c__DisplayClass14_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_vPos;

		private static readonly System.IntPtr NativeFieldInfoPtr_angleFromLast;

		private static readonly System.IntPtr NativeFieldInfoPtr_distAccu;

		private static readonly System.IntPtr NativeFieldInfoPtr_linearSteps;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetPolygon_b__0_Internal_Void_Vector3_0;

		public unsafe SubArrayList<Vector3> vPos
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vPos);
				return new SubArrayList<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArrayList<Vector3>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vPos), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArrayList)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArrayList<Vector3>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe float angleFromLast
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_angleFromLast);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_angleFromLast)) = num;
			}
		}

		public unsafe float distAccu
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_distAccu);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_distAccu)) = num;
			}
		}

		public unsafe int linearSteps
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_linearSteps);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_linearSteps)) = num;
			}
		}

		static __c__DisplayClass14_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "<>c__DisplayClass14_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr);
			NativeFieldInfoPtr_vPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "vPos");
			NativeFieldInfoPtr_angleFromLast = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "angleFromLast");
			NativeFieldInfoPtr_distAccu = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "distAccu");
			NativeFieldInfoPtr_linearSteps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "linearSteps");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, 100665332);
			NativeMethodInfoPtr__GetPolygon_b__0_Internal_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, 100665333);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass14_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44002, XrefRangeEnd = 44005, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _GetPolygon_b__0(Vector3 position)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&position);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetPolygon_b__0_Internal_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public __c__DisplayClass14_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Orientation;

	private static readonly System.IntPtr NativeFieldInfoPtr_Spline;

	private static readonly System.IntPtr NativeFieldInfoPtr_VertexMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_Angle;

	private static readonly System.IntPtr NativeFieldInfoPtr_Distance;

	private static readonly System.IntPtr NativeFieldInfoPtr_Space;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Private_Void_CurvySpline_VertexCalculation_Single_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsClosed_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetVertices_Public_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetVertexList_Public_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPolygon_Private_Static_SubArrayList_1_Vector3_CurvySpline_Single_Single_Single_Single_Single_Boolean_Single_0;

	public unsafe ContourOrientation Orientation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Orientation);
			return *(ContourOrientation*)num;
		}
		set
		{
			*(ContourOrientation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Orientation)) = contourOrientation;
		}
	}

	public unsafe CurvySpline Spline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Spline);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe VertexCalculation VertexMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VertexMode);
			return *(VertexCalculation*)num;
		}
		set
		{
			*(VertexCalculation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VertexMode)) = vertexCalculation;
		}
	}

	public unsafe float Angle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Angle);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Angle)) = num;
		}
	}

	public unsafe float Distance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Distance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Distance)) = num;
		}
	}

	public unsafe Space Space
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Space);
			return *(Space*)num;
		}
		set
		{
			*(Space*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Space)) = space;
		}
	}

	public unsafe bool IsClosed
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44011, XrefRangeEnd = 44015, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsClosed_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SplinePolyLine()
	{
		Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Utils", "SplinePolyLine");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr);
		NativeFieldInfoPtr_Orientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "Orientation");
		NativeFieldInfoPtr_Spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "Spline");
		NativeFieldInfoPtr_VertexMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "VertexMode");
		NativeFieldInfoPtr_Angle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "Angle");
		NativeFieldInfoPtr_Distance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "Distance");
		NativeFieldInfoPtr_Space = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, "Space");
		NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665325);
		NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665326);
		NativeMethodInfoPtr__ctor_Private_Void_CurvySpline_VertexCalculation_Single_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665327);
		NativeMethodInfoPtr_get_IsClosed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665328);
		NativeMethodInfoPtr_GetVertices_Public_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665329);
		NativeMethodInfoPtr_GetVertexList_Public_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665330);
		NativeMethodInfoPtr_GetPolygon_Private_Static_SubArrayList_1_Vector3_CurvySpline_Single_Single_Single_Single_Single_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr, 100665331);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44005, XrefRangeEnd = 44007, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SplinePolyLine(CurvySpline spline)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44007, XrefRangeEnd = 44009, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SplinePolyLine(CurvySpline spline, float angle, float distance)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &angle;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44009, XrefRangeEnd = 44011, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SplinePolyLine(CurvySpline spline, VertexCalculation vertexMode, float angle, float distance, Space space = Space.World)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SplinePolyLine>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(VertexCalculation**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexMode;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &angle;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		*(Space**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Private_Void_CurvySpline_VertexCalculation_Single_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44015, XrefRangeEnd = 44033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<Vector3> GetVertices()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVertices_Public_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 44041, RefRangeEnd = 44044, XrefRangeStart = 44033, XrefRangeEnd = 44041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<Vector3> GetVertexList()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVertexList_Public_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 44093, RefRangeEnd = 44094, XrefRangeStart = 44044, XrefRangeEnd = 44093, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SubArrayList<Vector3> GetPolygon(CurvySpline spline, float fromTF, float toTF, float maxAngle, float minDistance, float maxDistance, bool includeEndPoint = true, float stepSize = 0.01f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &fromTF;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &toTF;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxAngle;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &minDistance;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxDistance;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeEndPoint;
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &stepSize;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPolygon_Private_Static_SubArrayList_1_Vector3_CurvySpline_Single_Single_Single_Single_Single_Boolean_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArrayList<Vector3>(pointer);
	}

	public SplinePolyLine(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
