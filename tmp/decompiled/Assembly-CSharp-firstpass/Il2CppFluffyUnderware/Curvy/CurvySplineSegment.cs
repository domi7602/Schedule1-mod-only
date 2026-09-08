using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Collections.ObjectModel;
using Il2CppSystem.Reflection;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvySplineSegment : DTVersionedMonoBehaviour
{
	public class Approximations : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Positions;

		private static readonly System.IntPtr NativeFieldInfoPtr_Tangents;

		private static readonly System.IntPtr NativeFieldInfoPtr_Ups;

		private static readonly System.IntPtr NativeFieldInfoPtr_Distances;

		private static readonly System.IntPtr NativeMethodInfoPtr_ResizePositions_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ResizeTangents_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ResizeUps_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ResizeDistances_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Free_Private_Void_0;

		public unsafe SubArray<Vector3> Positions
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Positions);
				return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Positions), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe SubArray<Vector3> Tangents
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tangents);
				return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tangents), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe SubArray<Vector3> Ups
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Ups);
				return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Ups), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe SubArray<float> Distances
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Distances);
				return new SubArray<float>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<float>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Distances), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<float>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		static Approximations()
		{
			Il2CppClassPointerStore<Approximations>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "Approximations");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Approximations>.NativeClassPtr);
			NativeFieldInfoPtr_Positions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Approximations>.NativeClassPtr, "Positions");
			NativeFieldInfoPtr_Tangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Approximations>.NativeClassPtr, "Tangents");
			NativeFieldInfoPtr_Ups = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Approximations>.NativeClassPtr, "Ups");
			NativeFieldInfoPtr_Distances = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Approximations>.NativeClassPtr, "Distances");
			NativeMethodInfoPtr_ResizePositions_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665108);
			NativeMethodInfoPtr_ResizeTangents_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665109);
			NativeMethodInfoPtr_ResizeUps_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665110);
			NativeMethodInfoPtr_ResizeDistances_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665111);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665112);
			NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665113);
			NativeMethodInfoPtr_Initialize_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665114);
			NativeMethodInfoPtr_Free_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Approximations>.NativeClassPtr, 100665115);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40415, XrefRangeEnd = 40426, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ResizePositions(int size)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&size);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResizePositions_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40426, XrefRangeEnd = 40437, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ResizeTangents(int size)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&size);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResizeTangents_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40437, XrefRangeEnd = 40448, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ResizeUps(int size)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&size);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResizeUps_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40448, XrefRangeEnd = 40459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ResizeDistances(int size)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&size);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResizeDistances_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40459, XrefRangeEnd = 40461, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Approximations()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Approximations>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40461, XrefRangeEnd = 40463, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Clear()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 40490, RefRangeEnd = 40495, XrefRangeStart = 40463, XrefRangeEnd = 40490, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Initialize()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 40524, RefRangeEnd = 40527, XrefRangeStart = 40495, XrefRangeEnd = 40524, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Free()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Free_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Approximations(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public static class ApproximationsSetter : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeMethodInfoPtr_SetPositionsToPoint_Public_Static_Void_Approximations_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetPositionsToLinear_Public_Static_Void_Approximations_Int32_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetPositionsToCatmullRom_Public_Static_Void_Approximations_Int32_Vector3_Vector3_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetPositionsToTCB_Public_Static_Void_Approximations_Int32_TcbParameters_Vector3_Vector3_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetPositionsToBezier_Public_Static_Void_Approximations_Int32_Vector3_Vector3_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetPositionsToBSpline_Public_Static_Void_Approximations_Int32_SubArray_1_Vector3_BSplineApproximationParameters_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetOrientationToNone_Public_Static_Void_Approximations_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetOrientationToStatic_Public_Static_Void_Approximations_Int32_Vector3_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetOrientationToDynamic_Public_Static_Void_Approximations_Int32_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetPointTangentsAndDistances_Public_Static_Single_Approximations_Vector3_Vector3_Vector3_Quaternion_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetSegmentTangentsAnDistances_Public_Static_Single_Approximations_Int32_0;

		static ApproximationsSetter()
		{
			Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "ApproximationsSetter");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr);
			NativeMethodInfoPtr_SetPositionsToPoint_Public_Static_Void_Approximations_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665116);
			NativeMethodInfoPtr_SetPositionsToLinear_Public_Static_Void_Approximations_Int32_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665117);
			NativeMethodInfoPtr_SetPositionsToCatmullRom_Public_Static_Void_Approximations_Int32_Vector3_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665118);
			NativeMethodInfoPtr_SetPositionsToTCB_Public_Static_Void_Approximations_Int32_TcbParameters_Vector3_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665119);
			NativeMethodInfoPtr_SetPositionsToBezier_Public_Static_Void_Approximations_Int32_Vector3_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665120);
			NativeMethodInfoPtr_SetPositionsToBSpline_Public_Static_Void_Approximations_Int32_SubArray_1_Vector3_BSplineApproximationParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665121);
			NativeMethodInfoPtr_SetOrientationToNone_Public_Static_Void_Approximations_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665122);
			NativeMethodInfoPtr_SetOrientationToStatic_Public_Static_Void_Approximations_Int32_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665123);
			NativeMethodInfoPtr_SetOrientationToDynamic_Public_Static_Void_Approximations_Int32_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665124);
			NativeMethodInfoPtr_SetPointTangentsAndDistances_Public_Static_Single_Approximations_Vector3_Vector3_Vector3_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665125);
			NativeMethodInfoPtr_SetSegmentTangentsAnDistances_Public_Static_Single_Approximations_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximationsSetter>.NativeClassPtr, 100665126);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40527, XrefRangeEnd = 40528, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetPositionsToPoint(Approximations approximations, Vector3 currentPosition)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentPosition;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPositionsToPoint_Public_Static_Void_Approximations_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40528, XrefRangeEnd = 40529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetPositionsToLinear(Approximations approximations, int elementCount, Vector3 startPosition, Vector3 endPosition)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPosition;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPositionsToLinear_Public_Static_Void_Approximations_Int32_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40530, RefRangeEnd = 40531, XrefRangeStart = 40529, XrefRangeEnd = 40530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetPositionsToCatmullRom(Approximations approximations, int elementCount, Vector3 startPosition, Vector3 endPosition, Vector3 preSegmentPosition, Vector3 postSegmentPosition)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[6];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &preSegmentPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &postSegmentPosition;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPositionsToCatmullRom_Public_Static_Void_Approximations_Int32_Vector3_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40532, RefRangeEnd = 40533, XrefRangeStart = 40531, XrefRangeEnd = 40532, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetPositionsToTCB(Approximations approximations, int elementCount, TcbParameters tcbParameters, Vector3 startPosition, Vector3 endPosition, Vector3 preSegmentPosition, Vector3 postSegmentPosition)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[7];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(TcbParameters**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &tcbParameters;
			*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &preSegmentPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &postSegmentPosition;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPositionsToTCB_Public_Static_Void_Approximations_Int32_TcbParameters_Vector3_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40534, RefRangeEnd = 40535, XrefRangeStart = 40533, XrefRangeEnd = 40534, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetPositionsToBezier(Approximations approximations, int elementCount, Vector3 startPosition, Vector3 startTangent, Vector3 endPosition, Vector3 endTangent)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[6];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &startTangent;
			*(Vector3**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &endTangent;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPositionsToBezier_Public_Static_Void_Approximations_Int32_Vector3_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40588, RefRangeEnd = 40589, XrefRangeStart = 40535, XrefRangeEnd = 40588, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetPositionsToBSpline(Approximations approximations, int elementCount, SubArray<Vector3> splineP0Array, BSplineApproximationParameters bSplineParameters)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)splineP0Array));
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)bSplineParameters));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPositionsToBSpline_Public_Static_Void_Approximations_Int32_SubArray_1_Vector3_BSplineApproximationParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40589, XrefRangeEnd = 40592, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetOrientationToNone(Approximations approximations, int elementCount)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetOrientationToNone_Public_Static_Void_Approximations_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40592, XrefRangeEnd = 40594, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetOrientationToStatic(Approximations approximations, int elementCount, Vector3 startUp, Vector3 endUp)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startUp;
			*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endUp;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetOrientationToStatic_Public_Static_Void_Approximations_Int32_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40603, RefRangeEnd = 40604, XrefRangeStart = 40594, XrefRangeEnd = 40603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SetOrientationToDynamic(Approximations approximations, int elementCount, Vector3 startUp)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startUp;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetOrientationToDynamic_Public_Static_Void_Approximations_Int32_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40612, RefRangeEnd = 40613, XrefRangeStart = 40604, XrefRangeEnd = 40612, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static float SetPointTangentsAndDistances(Approximations approximations, Vector3 previousPosition, Vector3 currentPosition, Vector3 nextPosition, Quaternion currentRotation)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[5];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &previousPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentPosition;
			*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &nextPosition;
			*(Quaternion**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentRotation;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPointTangentsAndDistances_Public_Static_Single_Approximations_Vector3_Vector3_Vector3_Quaternion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40618, RefRangeEnd = 40619, XrefRangeStart = 40613, XrefRangeEnd = 40618, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static float SetSegmentTangentsAnDistances(Approximations approximations, int elementCount)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementCount;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSegmentTangentsAnDistances_Public_Static_Single_Approximations_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public ApproximationsSetter(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class BSplineApproximationParameters : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__Degree_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__IsClamped_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__IsClosed_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__StartTf_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__EndTf_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__ControlPoints_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__SegmentsCount_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Degree_Public_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsClamped_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsClosed_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_StartTf_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_EndTf_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_ControlPoints_Public_get_ReadOnlyCollection_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_SegmentsCount_Public_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_BSplineApproximationParameters_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_BSplineApproximationParameters_BSplineApproximationParameters_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_BSplineApproximationParameters_BSplineApproximationParameters_0;

		public unsafe int _Degree_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Degree_k__BackingField);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Degree_k__BackingField)) = num;
			}
		}

		public unsafe bool _IsClamped_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsClamped_k__BackingField);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsClamped_k__BackingField)) = flag;
			}
		}

		public unsafe bool _IsClosed_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsClosed_k__BackingField);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsClosed_k__BackingField)) = flag;
			}
		}

		public unsafe float _StartTf_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StartTf_k__BackingField);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StartTf_k__BackingField)) = num;
			}
		}

		public unsafe float _EndTf_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__EndTf_k__BackingField);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__EndTf_k__BackingField)) = num;
			}
		}

		public unsafe ReadOnlyCollection<CurvySplineSegment> _ControlPoints_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ControlPoints_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ReadOnlyCollection<CurvySplineSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ControlPoints_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)readOnlyCollection));
			}
		}

		public unsafe int _SegmentsCount_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SegmentsCount_k__BackingField);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SegmentsCount_k__BackingField)) = num;
			}
		}

		public unsafe int Degree
		{
			[CallerCount(501)]
			[CachedScanResults(RefRangeStart = 40619, RefRangeEnd = 41120, XrefRangeStart = 40619, XrefRangeEnd = 40619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Degree_Public_get_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe bool IsClamped
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsClamped_Public_get_Boolean_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe bool IsClosed
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsClosed_Public_get_Boolean_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe float StartTf
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartTf_Public_get_Single_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe float EndTf
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndTf_Public_get_Single_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe ReadOnlyCollection<CurvySplineSegment> ControlPoints
		{
			[CallerCount(12)]
			[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ControlPoints_Public_get_ReadOnlyCollection_1_CurvySplineSegment_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ReadOnlyCollection<CurvySplineSegment>>(intPtr) : null;
			}
		}

		public unsafe int SegmentsCount
		{
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 3891, RefRangeEnd = 3894, XrefRangeStart = 3891, XrefRangeEnd = 3894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SegmentsCount_Public_get_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static BSplineApproximationParameters()
		{
			Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "BSplineApproximationParameters");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr);
			NativeFieldInfoPtr__Degree_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<Degree>k__BackingField");
			NativeFieldInfoPtr__IsClamped_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<IsClamped>k__BackingField");
			NativeFieldInfoPtr__IsClosed_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<IsClosed>k__BackingField");
			NativeFieldInfoPtr__StartTf_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<StartTf>k__BackingField");
			NativeFieldInfoPtr__EndTf_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<EndTf>k__BackingField");
			NativeFieldInfoPtr__ControlPoints_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<ControlPoints>k__BackingField");
			NativeFieldInfoPtr__SegmentsCount_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, "<SegmentsCount>k__BackingField");
			NativeMethodInfoPtr_get_Degree_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665127);
			NativeMethodInfoPtr_get_IsClamped_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665128);
			NativeMethodInfoPtr_get_IsClosed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665129);
			NativeMethodInfoPtr_get_StartTf_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665130);
			NativeMethodInfoPtr_get_EndTf_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665131);
			NativeMethodInfoPtr_get_ControlPoints_Public_get_ReadOnlyCollection_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665132);
			NativeMethodInfoPtr_get_SegmentsCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665133);
			NativeMethodInfoPtr__ctor_Public_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665134);
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_BSplineApproximationParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665135);
			NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665136);
			NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665137);
			NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_BSplineApproximationParameters_BSplineApproximationParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665138);
			NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_BSplineApproximationParameters_BSplineApproximationParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr, 100665139);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41120, XrefRangeEnd = 41126, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe BSplineApproximationParameters(CurvySplineSegment segment)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySplineSegment_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 41128, RefRangeEnd = 41131, XrefRangeStart = 41126, XrefRangeEnd = 41128, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool Equals(BSplineApproximationParameters other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)other));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_BSplineApproximationParameters_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41131, XrefRangeEnd = 41135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override bool Equals(Il2CppSystem.Object obj)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41135, XrefRangeEnd = 41142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override int GetHashCode()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41142, XrefRangeEnd = 41143, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool operator ==(BSplineApproximationParameters left, BSplineApproximationParameters right)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)left));
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)right));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_BSplineApproximationParameters_BSplineApproximationParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41143, XrefRangeEnd = 41144, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool operator !=(BSplineApproximationParameters left, BSplineApproximationParameters right)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)left));
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)right));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_BSplineApproximationParameters_BSplineApproximationParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public BSplineApproximationParameters(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public BSplineApproximationParameters()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BSplineApproximationParameters>.NativeClassPtr))
		{
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct ControlPointExtrinsicProperties
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_isVisible;

		private static readonly System.IntPtr NativeFieldInfoPtr_tf;

		private static readonly System.IntPtr NativeFieldInfoPtr_segmentIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_controlPointIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_nextControlPointIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_previousControlPointIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_previousControlPointIsSegment;

		private static readonly System.IntPtr NativeFieldInfoPtr_nextControlPointIsSegment;

		private static readonly System.IntPtr NativeFieldInfoPtr_canHaveFollowUp;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsVisible_Internal_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_TF_Internal_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_SegmentIndex_Internal_get_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_ControlPointIndex_Internal_get_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_NextControlPointIndex_Internal_get_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_PreviousControlPointIndex_Internal_get_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_PreviousControlPointIsSegment_Internal_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_NextControlPointIsSegment_Internal_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_CanHaveFollowUp_Internal_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsSegment_Internal_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_OrientationAnchorIndex_Internal_get_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Internal_Void_Boolean_Single_Int16_Int16_Int16_Int16_Boolean_Boolean_Boolean_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Internal_Void_Boolean_Single_Int16_Int16_Int16_Int16_Boolean_Boolean_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_ControlPointExtrinsicProperties_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ControlPointExtrinsicProperties_ControlPointExtrinsicProperties_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ControlPointExtrinsicProperties_ControlPointExtrinsicProperties_0;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool isVisible;

		[FieldOffset(4)]
		public readonly float tf;

		[FieldOffset(8)]
		public readonly short segmentIndex;

		[FieldOffset(10)]
		public readonly short controlPointIndex;

		[FieldOffset(12)]
		public readonly short nextControlPointIndex;

		[FieldOffset(14)]
		public readonly short previousControlPointIndex;

		[FieldOffset(16)]
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool previousControlPointIsSegment;

		[FieldOffset(17)]
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool nextControlPointIsSegment;

		[FieldOffset(18)]
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool canHaveFollowUp;

		public unsafe bool IsVisible
		{
			[CallerCount(42)]
			[CachedScanResults(RefRangeStart = 41144, RefRangeEnd = 41186, XrefRangeStart = 41144, XrefRangeEnd = 41144, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsVisible_Internal_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe float TF
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TF_Internal_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe short SegmentIndex
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SegmentIndex_Internal_get_Int16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe short ControlPointIndex
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ControlPointIndex_Internal_get_Int16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe short NextControlPointIndex
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NextControlPointIndex_Internal_get_Int16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe short PreviousControlPointIndex
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PreviousControlPointIndex_Internal_get_Int16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe bool PreviousControlPointIsSegment
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PreviousControlPointIsSegment_Internal_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe bool NextControlPointIsSegment
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NextControlPointIsSegment_Internal_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe bool CanHaveFollowUp
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CanHaveFollowUp_Internal_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe bool IsSegment
		{
			[CallerCount(6)]
			[CachedScanResults(RefRangeStart = 41186, RefRangeEnd = 41192, XrefRangeStart = 41186, XrefRangeEnd = 41186, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsSegment_Internal_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe short OrientationAnchorIndex
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41192, XrefRangeEnd = 41198, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OrientationAnchorIndex_Internal_get_Int16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static ControlPointExtrinsicProperties()
		{
			Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "ControlPointExtrinsicProperties");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr);
			NativeFieldInfoPtr_isVisible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "isVisible");
			NativeFieldInfoPtr_tf = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "tf");
			NativeFieldInfoPtr_segmentIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "segmentIndex");
			NativeFieldInfoPtr_controlPointIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "controlPointIndex");
			NativeFieldInfoPtr_nextControlPointIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "nextControlPointIndex");
			NativeFieldInfoPtr_previousControlPointIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "previousControlPointIndex");
			NativeFieldInfoPtr_previousControlPointIsSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "previousControlPointIsSegment");
			NativeFieldInfoPtr_nextControlPointIsSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "nextControlPointIsSegment");
			NativeFieldInfoPtr_canHaveFollowUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, "canHaveFollowUp");
			NativeMethodInfoPtr_get_IsVisible_Internal_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665140);
			NativeMethodInfoPtr_get_TF_Internal_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665141);
			NativeMethodInfoPtr_get_SegmentIndex_Internal_get_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665142);
			NativeMethodInfoPtr_get_ControlPointIndex_Internal_get_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665143);
			NativeMethodInfoPtr_get_NextControlPointIndex_Internal_get_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665144);
			NativeMethodInfoPtr_get_PreviousControlPointIndex_Internal_get_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665145);
			NativeMethodInfoPtr_get_PreviousControlPointIsSegment_Internal_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665146);
			NativeMethodInfoPtr_get_NextControlPointIsSegment_Internal_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665147);
			NativeMethodInfoPtr_get_CanHaveFollowUp_Internal_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665148);
			NativeMethodInfoPtr_get_IsSegment_Internal_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665149);
			NativeMethodInfoPtr_get_OrientationAnchorIndex_Internal_get_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665150);
			NativeMethodInfoPtr__ctor_Internal_Void_Boolean_Single_Int16_Int16_Int16_Int16_Boolean_Boolean_Boolean_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665151);
			NativeMethodInfoPtr__ctor_Internal_Void_Boolean_Single_Int16_Int16_Int16_Int16_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665152);
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_ControlPointExtrinsicProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665153);
			NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665154);
			NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665155);
			NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ControlPointExtrinsicProperties_ControlPointExtrinsicProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665156);
			NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ControlPointExtrinsicProperties_ControlPointExtrinsicProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, 100665157);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41198, RefRangeEnd = 41200, XrefRangeStart = 41198, XrefRangeEnd = 41198, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ControlPointExtrinsicProperties(bool isVisible, float tf, short segmentIndex, short controlPointIndex, short previousControlPointIndex, short nextControlPointIndex, bool previousControlPointIsSegment, bool nextControlPointIsSegment, bool canHaveFollowUp, short orientationAnchorIndex)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[10];
			*ptr = (nint)(&isVisible);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tf;
			*(short**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &segmentIndex;
			*(short**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointIndex;
			*(short**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &previousControlPointIndex;
			*(short**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &nextControlPointIndex;
			*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &previousControlPointIsSegment;
			*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &nextControlPointIsSegment;
			*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &canHaveFollowUp;
			*(short**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &orientationAnchorIndex;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Internal_Void_Boolean_Single_Int16_Int16_Int16_Int16_Boolean_Boolean_Boolean_Int16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41198, RefRangeEnd = 41200, XrefRangeStart = 41198, XrefRangeEnd = 41200, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ControlPointExtrinsicProperties(bool isVisible, float tf, short segmentIndex, short controlPointIndex, short previousControlPointIndex, short nextControlPointIndex, bool previousControlPointIsSegment, bool nextControlPointIsSegment, bool canHaveFollowUp)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[9];
			*ptr = (nint)(&isVisible);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tf;
			*(short**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &segmentIndex;
			*(short**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointIndex;
			*(short**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &previousControlPointIndex;
			*(short**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &nextControlPointIndex;
			*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &previousControlPointIsSegment;
			*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &nextControlPointIsSegment;
			*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &canHaveFollowUp;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Internal_Void_Boolean_Single_Int16_Int16_Int16_Int16_Boolean_Boolean_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual bool Equals(ControlPointExtrinsicProperties other)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&other);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_ControlPointExtrinsicProperties_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41200, XrefRangeEnd = 41203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool Equals(Il2CppSystem.Object obj)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41203, XrefRangeEnd = 41215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override int GetHashCode()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe static bool operator ==(ControlPointExtrinsicProperties left, ControlPointExtrinsicProperties right)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&left);
			*(ControlPointExtrinsicProperties**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ControlPointExtrinsicProperties_ControlPointExtrinsicProperties_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe static bool operator !=(ControlPointExtrinsicProperties left, ControlPointExtrinsicProperties right)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&left);
			*(ControlPointExtrinsicProperties**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ControlPointExtrinsicProperties_ControlPointExtrinsicProperties_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ControlPointExtrinsicProperties>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public class ThreadSafeData : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ThreadSafeLocalPosition;

		private static readonly System.IntPtr NativeFieldInfoPtr_ThreadSafeNextCpLocalPosition;

		private static readonly System.IntPtr NativeFieldInfoPtr_ThreadSafePreviousCpLocalPosition;

		private static readonly System.IntPtr NativeFieldInfoPtr_ThreadSafeLocalRotation;

		private static readonly System.IntPtr NativeMethodInfoPtr_Set_Internal_Void_Boolean_CurvySplineSegment_byref_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Vector3 ThreadSafeLocalPosition
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafeLocalPosition);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafeLocalPosition)) = vector;
			}
		}

		public unsafe Vector3 ThreadSafeNextCpLocalPosition
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafeNextCpLocalPosition);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafeNextCpLocalPosition)) = vector;
			}
		}

		public unsafe Vector3 ThreadSafePreviousCpLocalPosition
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafePreviousCpLocalPosition);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafePreviousCpLocalPosition)) = vector;
			}
		}

		public unsafe Quaternion ThreadSafeLocalRotation
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafeLocalRotation);
				return *(Quaternion*)num;
			}
			set
			{
				*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ThreadSafeLocalRotation)) = quaternion;
			}
		}

		static ThreadSafeData()
		{
			Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "ThreadSafeData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr);
			NativeFieldInfoPtr_ThreadSafeLocalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr, "ThreadSafeLocalPosition");
			NativeFieldInfoPtr_ThreadSafeNextCpLocalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr, "ThreadSafeNextCpLocalPosition");
			NativeFieldInfoPtr_ThreadSafePreviousCpLocalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr, "ThreadSafePreviousCpLocalPosition");
			NativeFieldInfoPtr_ThreadSafeLocalRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr, "ThreadSafeLocalRotation");
			NativeMethodInfoPtr_Set_Internal_Void_Boolean_CurvySplineSegment_byref_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr, 100665158);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr, 100665159);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41224, RefRangeEnd = 41225, XrefRangeStart = 41215, XrefRangeEnd = 41224, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Set(bool useFollowUp, CurvySplineSegment curvySplineSegment, out CurvySplineSegment nextCP)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&useFollowUp);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment);
			byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
			nint num2 = 0;
			*(nint**)num = &num2;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Set_Internal_Void_Boolean_CurvySplineSegment_byref_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			nint num3 = num2;
			nextCP = ((num3 == 0) ? null : new CurvySplineSegment(num3));
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ThreadSafeData()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ThreadSafeData>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ThreadSafeData(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_GetMetadata_Public_T_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetMetadata_Public_T_Boolean_0, Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetInterpolatedMetadata_Public_U_Single_0<T, U>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetInterpolatedMetadata_Public_U_Single_0, Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[2]
		{
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<U>.NativeClassPtr))
		}))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_GizmoTangentColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_AutoBakeOrientation;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OrientationAnchor;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Swirl;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_SwirlTurns;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_AutoHandles;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_AutoHandleDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_HandleIn;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_HandleOut;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OverrideGlobalTension;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OverrideGlobalContinuity;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OverrideGlobalBias;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_SynchronizeTCB;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_StartTension;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_EndTension;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_StartContinuity;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_EndContinuity;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_StartBias;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_EndBias;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_FollowUp;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_FollowUpHeading;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_ConnectionSyncPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_ConnectionSyncRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Connection;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedTransform;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedNextControlPoint;

	private static readonly System.IntPtr NativeFieldInfoPtr_threadSafeData;

	private static readonly System.IntPtr NativeFieldInfoPtr_mSpline;

	private static readonly System.IntPtr NativeFieldInfoPtr_mBounds;

	private static readonly System.IntPtr NativeFieldInfoPtr_mMetadata;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastProcessedLocalPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastProcessedLocalRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_distance;

	private static readonly System.IntPtr NativeFieldInfoPtr_length;

	private static readonly System.IntPtr NativeFieldInfoPtr_bSplineP0Array;

	private static readonly System.IntPtr NativeFieldInfoPtr_extrinsicProperties;

	private static readonly System.IntPtr NativeFieldInfoPtr_approximations;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Approximation_Public_get_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Approximation_Public_set_Void_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ApproximationDistances_Public_get_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ApproximationDistances_Public_set_Void_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ApproximationUp_Public_get_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ApproximationUp_Public_set_Void_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ApproximationT_Public_get_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ApproximationT_Public_set_Void_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoBakeOrientation_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoBakeOrientation_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SerializedOrientationAnchor_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_SerializedOrientationAnchor_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Swirl_Public_get_CurvyOrientationSwirl_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Swirl_Public_set_Void_CurvyOrientationSwirl_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SwirlTurns_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_SwirlTurns_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HandleIn_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HandleIn_Public_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HandleOut_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HandleOut_Public_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HandleInPosition_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HandleInPosition_Public_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HandleOutPosition_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HandleOutPosition_Public_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoHandles_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoHandles_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoHandleDistance_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoHandleDistance_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SynchronizeTCB_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_SynchronizeTCB_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OverrideGlobalTension_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OverrideGlobalTension_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OverrideGlobalContinuity_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OverrideGlobalContinuity_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OverrideGlobalBias_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OverrideGlobalBias_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartTension_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StartTension_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartContinuity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StartContinuity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartBias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StartBias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndTension_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EndTension_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndContinuity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EndContinuity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndBias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EndBias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EffectiveTcbParameters_Public_get_TcbParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FollowUp_Public_get_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_FollowUp_Private_set_Void_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FollowUpHeading_Public_get_ConnectionHeadingEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_FollowUpHeading_Public_set_Void_ConnectionHeadingEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ConnectionSyncPosition_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ConnectionSyncPosition_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ConnectionSyncRotation_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ConnectionSyncRotation_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Connection_Public_get_CurvyConnection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Connection_Internal_set_Void_CurvyConnection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PositionsApproximation_Public_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TangentsApproximation_Public_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UpsApproximation_Public_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DistancesApproximation_Public_get_SubArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CacheSize_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Length_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Length_Private_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Distance_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Distance_Internal_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TF_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_TF_Internal_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsFirstControlPoint_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsLastControlPoint_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Metadata_Public_get_HashSet_1_CurvyMetadataBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasUnprocessedLocalPosition_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasUnprocessedLocalOrientation_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OrientatinInfluencesSpline_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OrientationInfluencesSpline_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandleIn_Public_Void_Vector3_Space_CurvyBezierModeEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandleOut_Public_Void_Vector3_Space_CurvyBezierModeEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandles_Public_Void_Single_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandles_Public_Void_Single_Vector3_Vector3_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetFollowUp_Public_Void_CurvySplineSegment_ConnectionHeadingEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetConnectionUnrelatedProperties_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Disconnect_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Disconnect_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interpolate_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangentFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateAndGetTangent_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateAndGetTangentFast_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientationUpFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientationFast_Public_Quaternion_Single_Boolean_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReloadMetaData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterMetaData_Public_Void_CurvyMetadataBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnregisterMetaData_Public_Void_CurvyMetadataBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMetadata_Public_T_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInterpolatedMetadata_Public_U_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteMetadata_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointF_Public_Single_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DistanceToLocalF_Public_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LocalFToDistance_Public_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LocalFToTF_Public_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BakeOrientationToTransform_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_getApproximationIndexINTERNAL_Public_Int32_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LinkToSpline_Public_Void_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnlinkFromSpline_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnlinkFromSpline_Public_Void_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLocalPosition_Public_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPosition_Public_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLocalRotation_Public_Void_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRotation_Public_Void_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanFollowUpHeadToStart_Public_Static_Boolean_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanFollowUpHeadToEnd_Public_Static_Boolean_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BSpline_Public_Static_Vector3_ReadOnlyCollection_1_CurvySplineSegment_Single_Boolean_Boolean_Int32_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBeforePush_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnAfterPop_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Interpolation_Private_get_CurvyInterpolation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsDynamicOrientation_Private_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsOrientationAnchorEditable_Private_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CanHaveSwirl_Private_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_BSplineP0Array_Private_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBSplineP0s_Private_Static_Void_ReadOnlyCollection_1_CurvySplineSegment_Int32_Int32_Int32_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetExtrinsicPropertiesINTERNAL_Internal_Void_ControlPointExtrinsicProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetExtrinsicPropertiesINTERNAL_Internal_byref_ControlPointExtrinsicProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CanTouchItsSpline_Private_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoInitialValidations_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckAgainstMetaDataDuplication_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetConnection_Private_Boolean_CurvyConnection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetValidateConnectionHeading_Private_Static_ConnectionHeadingEnum_ConnectionHeadingEnum_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetAutoHandles_Private_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareThreadCompatibleDataINTERNAL_Internal_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_refreshCurveINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSegmentCacheSize_Private_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_refreshOrientationNoneINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_refreshOrientationStaticINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_refreshOrientationDynamicINTERNAL_Internal_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateLasProcessedLocalPosition_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateLasProcessedLocalRotation_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearBoundsINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_getOrthoUp0INTERNAL_Internal_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_getOrthoUp1INTERNAL_Private_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnsetFollowUpWithoutDirtyingINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoSanityChecks_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSelectionIfNeeded_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ForceHierarchyDrawing_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Color GizmoTangentColor
	{
		get
		{
			Unsafe.SkipInit(out Color result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GizmoTangentColor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GizmoTangentColor, (void*)(&color));
		}
	}

	public unsafe bool m_AutoBakeOrientation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoBakeOrientation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoBakeOrientation)) = flag;
		}
	}

	public unsafe bool m_OrientationAnchor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OrientationAnchor);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OrientationAnchor)) = flag;
		}
	}

	public unsafe CurvyOrientationSwirl m_Swirl
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Swirl);
			return *(CurvyOrientationSwirl*)num;
		}
		set
		{
			*(CurvyOrientationSwirl*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Swirl)) = curvyOrientationSwirl;
		}
	}

	public unsafe float m_SwirlTurns
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SwirlTurns);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SwirlTurns)) = num;
		}
	}

	public unsafe bool m_AutoHandles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoHandles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoHandles)) = flag;
		}
	}

	public unsafe float m_AutoHandleDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoHandleDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoHandleDistance)) = num;
		}
	}

	public unsafe Vector3 m_HandleIn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_HandleIn);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_HandleIn)) = vector;
		}
	}

	public unsafe Vector3 m_HandleOut
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_HandleOut);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_HandleOut)) = vector;
		}
	}

	public unsafe bool m_OverrideGlobalTension
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OverrideGlobalTension);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OverrideGlobalTension)) = flag;
		}
	}

	public unsafe bool m_OverrideGlobalContinuity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OverrideGlobalContinuity);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OverrideGlobalContinuity)) = flag;
		}
	}

	public unsafe bool m_OverrideGlobalBias
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OverrideGlobalBias);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OverrideGlobalBias)) = flag;
		}
	}

	public unsafe bool m_SynchronizeTCB
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SynchronizeTCB);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SynchronizeTCB)) = flag;
		}
	}

	public unsafe float m_StartTension
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartTension);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartTension)) = num;
		}
	}

	public unsafe float m_EndTension
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndTension);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndTension)) = num;
		}
	}

	public unsafe float m_StartContinuity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartContinuity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartContinuity)) = num;
		}
	}

	public unsafe float m_EndContinuity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndContinuity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndContinuity)) = num;
		}
	}

	public unsafe float m_StartBias
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartBias);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartBias)) = num;
		}
	}

	public unsafe float m_EndBias
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndBias);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndBias)) = num;
		}
	}

	public unsafe CurvySplineSegment m_FollowUp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FollowUp);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FollowUp)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
		}
	}

	public unsafe ConnectionHeadingEnum m_FollowUpHeading
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FollowUpHeading);
			return *(ConnectionHeadingEnum*)num;
		}
		set
		{
			*(ConnectionHeadingEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FollowUpHeading)) = connectionHeadingEnum;
		}
	}

	public unsafe bool m_ConnectionSyncPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ConnectionSyncPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ConnectionSyncPosition)) = flag;
		}
	}

	public unsafe bool m_ConnectionSyncRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ConnectionSyncRotation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ConnectionSyncRotation)) = flag;
		}
	}

	public unsafe CurvyConnection m_Connection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Connection);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyConnection>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Connection)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyConnection));
		}
	}

	public unsafe Transform cachedTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedTransform);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedTransform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe CurvySplineSegment cachedNextControlPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedNextControlPoint);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedNextControlPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
		}
	}

	public unsafe ThreadSafeData threadSafeData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threadSafeData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ThreadSafeData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threadSafeData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)threadSafeData));
		}
	}

	public unsafe CurvySpline mSpline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSpline);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSpline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe Il2CppSystem.Nullable<Bounds> mBounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mBounds);
			return new Il2CppSystem.Nullable<Bounds>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<Bounds>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mBounds), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<Bounds>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe HashSet<CurvyMetadataBase> mMetadata
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mMetadata);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<CurvyMetadataBase>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mMetadata)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
		}
	}

	public unsafe Il2CppSystem.Nullable<Vector3> lastProcessedLocalPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastProcessedLocalPosition);
			return new Il2CppSystem.Nullable<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<Vector3>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastProcessedLocalPosition), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<Vector3>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe Il2CppSystem.Nullable<Quaternion> lastProcessedLocalRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastProcessedLocalRotation);
			return new Il2CppSystem.Nullable<Quaternion>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<Quaternion>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastProcessedLocalRotation), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<Quaternion>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe float distance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_distance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_distance)) = num;
		}
	}

	public unsafe float length
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_length);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_length)) = num;
		}
	}

	public unsafe SubArray<Vector3> bSplineP0Array
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bSplineP0Array);
			return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bSplineP0Array), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe ControlPointExtrinsicProperties extrinsicProperties
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_extrinsicProperties);
			return *(ControlPointExtrinsicProperties*)num;
		}
		set
		{
			*(ControlPointExtrinsicProperties*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_extrinsicProperties)) = controlPointExtrinsicProperties;
		}
	}

	public unsafe Approximations approximations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_approximations);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Approximations>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_approximations)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximations));
		}
	}

	public unsafe Il2CppStructArray<Vector3> Approximation
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41225, XrefRangeEnd = 41235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Approximation_Public_get_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41235, XrefRangeEnd = 41249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Approximation_Public_set_Void_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<float> ApproximationDistances
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41249, XrefRangeEnd = 41259, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ApproximationDistances_Public_get_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41259, XrefRangeEnd = 41273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ApproximationDistances_Public_set_Void_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector3> ApproximationUp
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41273, XrefRangeEnd = 41283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ApproximationUp_Public_get_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41283, XrefRangeEnd = 41297, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ApproximationUp_Public_set_Void_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector3> ApproximationT
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41297, XrefRangeEnd = 41307, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ApproximationT_Public_get_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41307, XrefRangeEnd = 41321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ApproximationT_Public_set_Void_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool AutoBakeOrientation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoBakeOrientation_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoBakeOrientation_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool SerializedOrientationAnchor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SerializedOrientationAnchor_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41321, XrefRangeEnd = 41326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SerializedOrientationAnchor_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyOrientationSwirl Swirl
	{
		[CallerCount(126)]
		[CachedScanResults(RefRangeStart = 41326, RefRangeEnd = 41452, XrefRangeStart = 41326, XrefRangeEnd = 41326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Swirl_Public_get_CurvyOrientationSwirl_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyOrientationSwirl*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41452, XrefRangeEnd = 41457, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Swirl_Public_set_Void_CurvyOrientationSwirl_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float SwirlTurns
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SwirlTurns_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41457, XrefRangeEnd = 41462, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SwirlTurns_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector3 HandleIn
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HandleIn_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 41467, RefRangeEnd = 41479, XrefRangeStart = 41462, XrefRangeEnd = 41467, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HandleIn_Public_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector3 HandleOut
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HandleOut_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 41484, RefRangeEnd = 41496, XrefRangeStart = 41479, XrefRangeEnd = 41484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HandleOut_Public_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector3 HandleInPosition
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41500, RefRangeEnd = 41501, XrefRangeStart = 41496, XrefRangeEnd = 41500, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HandleInPosition_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41505, RefRangeEnd = 41507, XrefRangeStart = 41501, XrefRangeEnd = 41505, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HandleInPosition_Public_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector3 HandleOutPosition
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41511, RefRangeEnd = 41512, XrefRangeStart = 41507, XrefRangeEnd = 41511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HandleOutPosition_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41516, RefRangeEnd = 41518, XrefRangeStart = 41512, XrefRangeEnd = 41516, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HandleOutPosition_Public_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool AutoHandles
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoHandles_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 41529, RefRangeEnd = 41539, XrefRangeStart = 41518, XrefRangeEnd = 41529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoHandles_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float AutoHandleDistance
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29133, RefRangeEnd = 29134, XrefRangeStart = 29133, XrefRangeEnd = 29134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoHandleDistance_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 41542, RefRangeEnd = 41545, XrefRangeStart = 41539, XrefRangeEnd = 41542, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoHandleDistance_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool SynchronizeTCB
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SynchronizeTCB_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41550, RefRangeEnd = 41551, XrefRangeStart = 41545, XrefRangeEnd = 41550, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SynchronizeTCB_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool OverrideGlobalTension
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OverrideGlobalTension_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41556, RefRangeEnd = 41557, XrefRangeStart = 41551, XrefRangeEnd = 41556, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OverrideGlobalTension_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool OverrideGlobalContinuity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OverrideGlobalContinuity_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41562, RefRangeEnd = 41563, XrefRangeStart = 41557, XrefRangeEnd = 41562, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OverrideGlobalContinuity_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool OverrideGlobalBias
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OverrideGlobalBias_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41568, RefRangeEnd = 41569, XrefRangeStart = 41563, XrefRangeEnd = 41568, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OverrideGlobalBias_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float StartTension
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartTension_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41574, RefRangeEnd = 41575, XrefRangeStart = 41569, XrefRangeEnd = 41574, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartTension_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float StartContinuity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartContinuity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41580, RefRangeEnd = 41581, XrefRangeStart = 41575, XrefRangeEnd = 41580, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartContinuity_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float StartBias
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartBias_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41586, RefRangeEnd = 41587, XrefRangeStart = 41581, XrefRangeEnd = 41586, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartBias_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EndTension
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndTension_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41592, RefRangeEnd = 41593, XrefRangeStart = 41587, XrefRangeEnd = 41592, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndTension_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EndContinuity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndContinuity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41598, RefRangeEnd = 41599, XrefRangeStart = 41593, XrefRangeEnd = 41598, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndContinuity_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EndBias
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndBias_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41604, RefRangeEnd = 41605, XrefRangeStart = 41599, XrefRangeEnd = 41604, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndBias_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe TcbParameters EffectiveTcbParameters
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41607, RefRangeEnd = 41608, XrefRangeStart = 41605, XrefRangeEnd = 41607, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EffectiveTcbParameters_Public_get_TcbParameters_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(TcbParameters*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvySplineSegment FollowUp
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41608, RefRangeEnd = 41609, XrefRangeStart = 41608, XrefRangeEnd = 41608, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FollowUp_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41615, RefRangeEnd = 41617, XrefRangeStart = 41609, XrefRangeEnd = 41615, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_FollowUp_Private_set_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe ConnectionHeadingEnum FollowUpHeading
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 41621, RefRangeEnd = 41626, XrefRangeStart = 41617, XrefRangeEnd = 41621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FollowUpHeading_Public_get_ConnectionHeadingEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ConnectionHeadingEnum*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41635, RefRangeEnd = 41637, XrefRangeStart = 41626, XrefRangeEnd = 41635, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_FollowUpHeading_Public_set_Void_ConnectionHeadingEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ConnectionSyncPosition
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ConnectionSyncPosition_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ConnectionSyncPosition_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ConnectionSyncRotation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ConnectionSyncRotation_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ConnectionSyncRotation_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyConnection Connection
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 41637, RefRangeEnd = 41647, XrefRangeStart = 41637, XrefRangeEnd = 41637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Connection_Public_get_CurvyConnection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyConnection>(intPtr) : null;
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 41653, RefRangeEnd = 41656, XrefRangeStart = 41647, XrefRangeEnd = 41653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Connection_Internal_set_Void_CurvyConnection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SubArray<Vector3> PositionsApproximation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PositionsApproximation_Public_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
	}

	public unsafe SubArray<Vector3> TangentsApproximation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TangentsApproximation_Public_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
	}

	public unsafe SubArray<Vector3> UpsApproximation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UpsApproximation_Public_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
	}

	public unsafe SubArray<float> DistancesApproximation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DistancesApproximation_Public_get_SubArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<float>(pointer);
		}
	}

	public unsafe int CacheSize
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 41656, RefRangeEnd = 41661, XrefRangeStart = 41656, XrefRangeEnd = 41656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CacheSize_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Bounds Bounds
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41678, RefRangeEnd = 41680, XrefRangeStart = 41661, XrefRangeEnd = 41678, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Bounds*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float Length
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Length_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Length_Private_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Distance
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Distance_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Distance_Internal_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float TF
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 41682, RefRangeEnd = 41684, XrefRangeStart = 41680, XrefRangeEnd = 41682, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TF_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41684, XrefRangeEnd = 41690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TF_Internal_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsFirstControlPoint
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41690, XrefRangeEnd = 41691, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsFirstControlPoint_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsLastControlPoint
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41691, XrefRangeEnd = 41693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsLastControlPoint_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe HashSet<CurvyMetadataBase> Metadata
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Metadata_Public_get_HashSet_1_CurvyMetadataBase_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<CurvyMetadataBase>>(intPtr) : null;
		}
	}

	public unsafe CurvySpline Spline
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
	}

	public unsafe bool HasUnprocessedLocalPosition
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41695, RefRangeEnd = 41696, XrefRangeStart = 41693, XrefRangeEnd = 41695, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasUnprocessedLocalPosition_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool HasUnprocessedLocalOrientation
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 41698, RefRangeEnd = 41699, XrefRangeStart = 41696, XrefRangeEnd = 41698, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasUnprocessedLocalOrientation_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool OrientatinInfluencesSpline
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41699, XrefRangeEnd = 41700, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OrientatinInfluencesSpline_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool OrientationInfluencesSpline
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 41704, RefRangeEnd = 41709, XrefRangeStart = 41700, XrefRangeEnd = 41704, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OrientationInfluencesSpline_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvyInterpolation Interpolation
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42261, XrefRangeEnd = 42265, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Interpolation_Private_get_CurvyInterpolation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyInterpolation*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsDynamicOrientation
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42265, XrefRangeEnd = 42269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsDynamicOrientation_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsOrientationAnchorEditable
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42269, XrefRangeEnd = 42283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsOrientationAnchorEditable_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool CanHaveSwirl
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42283, XrefRangeEnd = 42292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CanHaveSwirl_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe SubArray<Vector3> BSplineP0Array
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 42306, RefRangeEnd = 42307, XrefRangeStart = 42292, XrefRangeEnd = 42306, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BSplineP0Array_Private_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
	}

	public unsafe bool CanTouchItsSpline
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 42368, RefRangeEnd = 42372, XrefRangeStart = 42367, XrefRangeEnd = 42368, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CanTouchItsSpline_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CurvySplineSegment()
	{
		Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySplineSegment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr);
		NativeFieldInfoPtr_GizmoTangentColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "GizmoTangentColor");
		NativeFieldInfoPtr_m_AutoBakeOrientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_AutoBakeOrientation");
		NativeFieldInfoPtr_m_OrientationAnchor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_OrientationAnchor");
		NativeFieldInfoPtr_m_Swirl = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_Swirl");
		NativeFieldInfoPtr_m_SwirlTurns = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_SwirlTurns");
		NativeFieldInfoPtr_m_AutoHandles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_AutoHandles");
		NativeFieldInfoPtr_m_AutoHandleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_AutoHandleDistance");
		NativeFieldInfoPtr_m_HandleIn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_HandleIn");
		NativeFieldInfoPtr_m_HandleOut = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_HandleOut");
		NativeFieldInfoPtr_m_OverrideGlobalTension = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_OverrideGlobalTension");
		NativeFieldInfoPtr_m_OverrideGlobalContinuity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_OverrideGlobalContinuity");
		NativeFieldInfoPtr_m_OverrideGlobalBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_OverrideGlobalBias");
		NativeFieldInfoPtr_m_SynchronizeTCB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_SynchronizeTCB");
		NativeFieldInfoPtr_m_StartTension = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_StartTension");
		NativeFieldInfoPtr_m_EndTension = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_EndTension");
		NativeFieldInfoPtr_m_StartContinuity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_StartContinuity");
		NativeFieldInfoPtr_m_EndContinuity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_EndContinuity");
		NativeFieldInfoPtr_m_StartBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_StartBias");
		NativeFieldInfoPtr_m_EndBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_EndBias");
		NativeFieldInfoPtr_m_FollowUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_FollowUp");
		NativeFieldInfoPtr_m_FollowUpHeading = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_FollowUpHeading");
		NativeFieldInfoPtr_m_ConnectionSyncPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_ConnectionSyncPosition");
		NativeFieldInfoPtr_m_ConnectionSyncRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_ConnectionSyncRotation");
		NativeFieldInfoPtr_m_Connection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "m_Connection");
		NativeFieldInfoPtr_cachedTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "cachedTransform");
		NativeFieldInfoPtr_cachedNextControlPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "cachedNextControlPoint");
		NativeFieldInfoPtr_threadSafeData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "threadSafeData");
		NativeFieldInfoPtr_mSpline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "mSpline");
		NativeFieldInfoPtr_mBounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "mBounds");
		NativeFieldInfoPtr_mMetadata = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "mMetadata");
		NativeFieldInfoPtr_lastProcessedLocalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "lastProcessedLocalPosition");
		NativeFieldInfoPtr_lastProcessedLocalRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "lastProcessedLocalRotation");
		NativeFieldInfoPtr_distance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "distance");
		NativeFieldInfoPtr_length = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "length");
		NativeFieldInfoPtr_bSplineP0Array = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "bSplineP0Array");
		NativeFieldInfoPtr_extrinsicProperties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "extrinsicProperties");
		NativeFieldInfoPtr_approximations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, "approximations");
		NativeMethodInfoPtr_get_Approximation_Public_get_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664950);
		NativeMethodInfoPtr_set_Approximation_Public_set_Void_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664951);
		NativeMethodInfoPtr_get_ApproximationDistances_Public_get_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664952);
		NativeMethodInfoPtr_set_ApproximationDistances_Public_set_Void_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664953);
		NativeMethodInfoPtr_get_ApproximationUp_Public_get_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664954);
		NativeMethodInfoPtr_set_ApproximationUp_Public_set_Void_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664955);
		NativeMethodInfoPtr_get_ApproximationT_Public_get_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664956);
		NativeMethodInfoPtr_set_ApproximationT_Public_set_Void_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664957);
		NativeMethodInfoPtr_get_AutoBakeOrientation_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664958);
		NativeMethodInfoPtr_set_AutoBakeOrientation_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664959);
		NativeMethodInfoPtr_get_SerializedOrientationAnchor_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664960);
		NativeMethodInfoPtr_set_SerializedOrientationAnchor_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664961);
		NativeMethodInfoPtr_get_Swirl_Public_get_CurvyOrientationSwirl_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664962);
		NativeMethodInfoPtr_set_Swirl_Public_set_Void_CurvyOrientationSwirl_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664963);
		NativeMethodInfoPtr_get_SwirlTurns_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664964);
		NativeMethodInfoPtr_set_SwirlTurns_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664965);
		NativeMethodInfoPtr_get_HandleIn_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664966);
		NativeMethodInfoPtr_set_HandleIn_Public_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664967);
		NativeMethodInfoPtr_get_HandleOut_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664968);
		NativeMethodInfoPtr_set_HandleOut_Public_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664969);
		NativeMethodInfoPtr_get_HandleInPosition_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664970);
		NativeMethodInfoPtr_set_HandleInPosition_Public_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664971);
		NativeMethodInfoPtr_get_HandleOutPosition_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664972);
		NativeMethodInfoPtr_set_HandleOutPosition_Public_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664973);
		NativeMethodInfoPtr_get_AutoHandles_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664974);
		NativeMethodInfoPtr_set_AutoHandles_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664975);
		NativeMethodInfoPtr_get_AutoHandleDistance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664976);
		NativeMethodInfoPtr_set_AutoHandleDistance_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664977);
		NativeMethodInfoPtr_get_SynchronizeTCB_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664978);
		NativeMethodInfoPtr_set_SynchronizeTCB_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664979);
		NativeMethodInfoPtr_get_OverrideGlobalTension_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664980);
		NativeMethodInfoPtr_set_OverrideGlobalTension_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664981);
		NativeMethodInfoPtr_get_OverrideGlobalContinuity_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664982);
		NativeMethodInfoPtr_set_OverrideGlobalContinuity_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664983);
		NativeMethodInfoPtr_get_OverrideGlobalBias_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664984);
		NativeMethodInfoPtr_set_OverrideGlobalBias_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664985);
		NativeMethodInfoPtr_get_StartTension_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664986);
		NativeMethodInfoPtr_set_StartTension_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664987);
		NativeMethodInfoPtr_get_StartContinuity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664988);
		NativeMethodInfoPtr_set_StartContinuity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664989);
		NativeMethodInfoPtr_get_StartBias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664990);
		NativeMethodInfoPtr_set_StartBias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664991);
		NativeMethodInfoPtr_get_EndTension_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664992);
		NativeMethodInfoPtr_set_EndTension_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664993);
		NativeMethodInfoPtr_get_EndContinuity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664994);
		NativeMethodInfoPtr_set_EndContinuity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664995);
		NativeMethodInfoPtr_get_EndBias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664996);
		NativeMethodInfoPtr_set_EndBias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664997);
		NativeMethodInfoPtr_get_EffectiveTcbParameters_Public_get_TcbParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664998);
		NativeMethodInfoPtr_get_FollowUp_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100664999);
		NativeMethodInfoPtr_set_FollowUp_Private_set_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665000);
		NativeMethodInfoPtr_get_FollowUpHeading_Public_get_ConnectionHeadingEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665001);
		NativeMethodInfoPtr_set_FollowUpHeading_Public_set_Void_ConnectionHeadingEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665002);
		NativeMethodInfoPtr_get_ConnectionSyncPosition_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665003);
		NativeMethodInfoPtr_set_ConnectionSyncPosition_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665004);
		NativeMethodInfoPtr_get_ConnectionSyncRotation_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665005);
		NativeMethodInfoPtr_set_ConnectionSyncRotation_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665006);
		NativeMethodInfoPtr_get_Connection_Public_get_CurvyConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665007);
		NativeMethodInfoPtr_set_Connection_Internal_set_Void_CurvyConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665008);
		NativeMethodInfoPtr_get_PositionsApproximation_Public_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665009);
		NativeMethodInfoPtr_get_TangentsApproximation_Public_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665010);
		NativeMethodInfoPtr_get_UpsApproximation_Public_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665011);
		NativeMethodInfoPtr_get_DistancesApproximation_Public_get_SubArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665012);
		NativeMethodInfoPtr_get_CacheSize_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665013);
		NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665014);
		NativeMethodInfoPtr_get_Length_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665015);
		NativeMethodInfoPtr_set_Length_Private_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665016);
		NativeMethodInfoPtr_get_Distance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665017);
		NativeMethodInfoPtr_set_Distance_Internal_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665018);
		NativeMethodInfoPtr_get_TF_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665019);
		NativeMethodInfoPtr_set_TF_Internal_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665020);
		NativeMethodInfoPtr_get_IsFirstControlPoint_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665021);
		NativeMethodInfoPtr_get_IsLastControlPoint_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665022);
		NativeMethodInfoPtr_get_Metadata_Public_get_HashSet_1_CurvyMetadataBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665023);
		NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665024);
		NativeMethodInfoPtr_get_HasUnprocessedLocalPosition_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665025);
		NativeMethodInfoPtr_get_HasUnprocessedLocalOrientation_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665026);
		NativeMethodInfoPtr_get_OrientatinInfluencesSpline_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665027);
		NativeMethodInfoPtr_get_OrientationInfluencesSpline_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665028);
		NativeMethodInfoPtr_SetBezierHandleIn_Public_Void_Vector3_Space_CurvyBezierModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665029);
		NativeMethodInfoPtr_SetBezierHandleOut_Public_Void_Vector3_Space_CurvyBezierModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665030);
		NativeMethodInfoPtr_SetBezierHandles_Public_Void_Single_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665031);
		NativeMethodInfoPtr_SetBezierHandles_Public_Void_Single_Vector3_Vector3_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665032);
		NativeMethodInfoPtr_SetFollowUp_Public_Void_CurvySplineSegment_ConnectionHeadingEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665033);
		NativeMethodInfoPtr_ResetConnectionUnrelatedProperties_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665034);
		NativeMethodInfoPtr_Disconnect_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665035);
		NativeMethodInfoPtr_Disconnect_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665036);
		NativeMethodInfoPtr_Interpolate_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665037);
		NativeMethodInfoPtr_InterpolateFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665038);
		NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665039);
		NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665040);
		NativeMethodInfoPtr_GetTangentFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665041);
		NativeMethodInfoPtr_InterpolateAndGetTangent_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665042);
		NativeMethodInfoPtr_InterpolateAndGetTangentFast_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665043);
		NativeMethodInfoPtr_GetOrientationUpFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665044);
		NativeMethodInfoPtr_GetOrientationFast_Public_Quaternion_Single_Boolean_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665045);
		NativeMethodInfoPtr_ReloadMetaData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665046);
		NativeMethodInfoPtr_RegisterMetaData_Public_Void_CurvyMetadataBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665047);
		NativeMethodInfoPtr_UnregisterMetaData_Public_Void_CurvyMetadataBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665048);
		NativeMethodInfoPtr_GetMetadata_Public_T_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665049);
		NativeMethodInfoPtr_GetInterpolatedMetadata_Public_U_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665050);
		NativeMethodInfoPtr_DeleteMetadata_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665051);
		NativeMethodInfoPtr_GetNearestPointF_Public_Single_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665052);
		NativeMethodInfoPtr_DistanceToLocalF_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665053);
		NativeMethodInfoPtr_LocalFToDistance_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665054);
		NativeMethodInfoPtr_LocalFToTF_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665055);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665056);
		NativeMethodInfoPtr_BakeOrientationToTransform_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665057);
		NativeMethodInfoPtr_getApproximationIndexINTERNAL_Public_Int32_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665058);
		NativeMethodInfoPtr_LinkToSpline_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665059);
		NativeMethodInfoPtr_UnlinkFromSpline_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665060);
		NativeMethodInfoPtr_UnlinkFromSpline_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665061);
		NativeMethodInfoPtr_SetLocalPosition_Public_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665062);
		NativeMethodInfoPtr_SetPosition_Public_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665063);
		NativeMethodInfoPtr_SetLocalRotation_Public_Void_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665064);
		NativeMethodInfoPtr_SetRotation_Public_Void_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665065);
		NativeMethodInfoPtr_CanFollowUpHeadToStart_Public_Static_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665066);
		NativeMethodInfoPtr_CanFollowUpHeadToEnd_Public_Static_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665067);
		NativeMethodInfoPtr_BSpline_Public_Static_Vector3_ReadOnlyCollection_1_CurvySplineSegment_Single_Boolean_Boolean_Int32_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665068);
		NativeMethodInfoPtr_OnBeforePush_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665069);
		NativeMethodInfoPtr_OnAfterPop_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665070);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665071);
		NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665072);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665073);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665074);
		NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665075);
		NativeMethodInfoPtr_get_Interpolation_Private_get_CurvyInterpolation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665076);
		NativeMethodInfoPtr_get_IsDynamicOrientation_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665077);
		NativeMethodInfoPtr_get_IsOrientationAnchorEditable_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665078);
		NativeMethodInfoPtr_get_CanHaveSwirl_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665079);
		NativeMethodInfoPtr_get_BSplineP0Array_Private_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665080);
		NativeMethodInfoPtr_GetBSplineP0s_Private_Static_Void_ReadOnlyCollection_1_CurvySplineSegment_Int32_Int32_Int32_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665081);
		NativeMethodInfoPtr_SetExtrinsicPropertiesINTERNAL_Internal_Void_ControlPointExtrinsicProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665082);
		NativeMethodInfoPtr_GetExtrinsicPropertiesINTERNAL_Internal_byref_ControlPointExtrinsicProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665083);
		NativeMethodInfoPtr_get_CanTouchItsSpline_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665084);
		NativeMethodInfoPtr_DoInitialValidations_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665085);
		NativeMethodInfoPtr_CheckAgainstMetaDataDuplication_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665086);
		NativeMethodInfoPtr_SetConnection_Private_Boolean_CurvyConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665087);
		NativeMethodInfoPtr_GetValidateConnectionHeading_Private_Static_ConnectionHeadingEnum_ConnectionHeadingEnum_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665088);
		NativeMethodInfoPtr_SetAutoHandles_Private_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665089);
		NativeMethodInfoPtr_PrepareThreadCompatibleDataINTERNAL_Internal_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665090);
		NativeMethodInfoPtr_refreshCurveINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665091);
		NativeMethodInfoPtr_GetSegmentCacheSize_Private_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665092);
		NativeMethodInfoPtr_refreshOrientationNoneINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665093);
		NativeMethodInfoPtr_refreshOrientationStaticINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665094);
		NativeMethodInfoPtr_refreshOrientationDynamicINTERNAL_Internal_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665095);
		NativeMethodInfoPtr_UpdateLasProcessedLocalPosition_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665096);
		NativeMethodInfoPtr_UpdateLasProcessedLocalRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665097);
		NativeMethodInfoPtr_ClearBoundsINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665098);
		NativeMethodInfoPtr_getOrthoUp0INTERNAL_Internal_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665099);
		NativeMethodInfoPtr_getOrthoUp1INTERNAL_Private_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665100);
		NativeMethodInfoPtr_UnsetFollowUpWithoutDirtyingINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665101);
		NativeMethodInfoPtr_DoSanityChecks_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665102);
		NativeMethodInfoPtr_UpdateSelectionIfNeeded_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665103);
		NativeMethodInfoPtr_ForceHierarchyDrawing_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665104);
		NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665105);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr, 100665106);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41748, RefRangeEnd = 41751, XrefRangeStart = 41709, XrefRangeEnd = 41748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandleIn(Vector3 position, Space space = Space.Self, CurvyBezierModeEnum mode = CurvyBezierModeEnum.None)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&position);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		*(CurvyBezierModeEnum**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandleIn_Public_Void_Vector3_Space_CurvyBezierModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41791, RefRangeEnd = 41794, XrefRangeStart = 41751, XrefRangeEnd = 41791, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandleOut(Vector3 position, Space space = Space.Self, CurvyBezierModeEnum mode = CurvyBezierModeEnum.None)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&position);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		*(CurvyBezierModeEnum**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandleOut_Public_Void_Vector3_Space_CurvyBezierModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 41820, RefRangeEnd = 41822, XrefRangeStart = 41794, XrefRangeEnd = 41820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandles(float distanceFrag = -1f, bool setIn = true, bool setOut = true, bool noDirtying = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&distanceFrag);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &setIn;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &setOut;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &noDirtying;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandles_Public_Void_Single_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41833, RefRangeEnd = 41836, XrefRangeStart = 41822, XrefRangeEnd = 41833, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandles(float distanceFrag, Vector3 p, Vector3 n, bool setIn = true, bool setOut = true, bool noDirtying = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&distanceFrag);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &n;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &setIn;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &setOut;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &noDirtying;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandles_Public_Void_Single_Vector3_Vector3_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41845, RefRangeEnd = 41848, XrefRangeStart = 41836, XrefRangeEnd = 41845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFollowUp(CurvySplineSegment target, ConnectionHeadingEnum heading = ConnectionHeadingEnum.Auto)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		*(ConnectionHeadingEnum**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &heading;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFollowUp_Public_Void_CurvySplineSegment_ConnectionHeadingEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41858, RefRangeEnd = 41861, XrefRangeStart = 41848, XrefRangeEnd = 41858, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetConnectionUnrelatedProperties()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetConnectionUnrelatedProperties_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41868, RefRangeEnd = 41871, XrefRangeStart = 41861, XrefRangeEnd = 41868, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Disconnect()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Disconnect_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 41878, RefRangeEnd = 41879, XrefRangeStart = 41871, XrefRangeEnd = 41878, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Disconnect(bool destroyEmptyConnection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&destroyEmptyConnection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Disconnect_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 41890, RefRangeEnd = 41901, XrefRangeStart = 41879, XrefRangeEnd = 41890, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 Interpolate(float localF, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localF);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Interpolate_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41905, RefRangeEnd = 41908, XrefRangeStart = 41901, XrefRangeEnd = 41905, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 InterpolateFast(float localF, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localF);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 41911, RefRangeEnd = 41913, XrefRangeStart = 41908, XrefRangeEnd = 41911, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangent(float localF, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localF);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41922, RefRangeEnd = 41925, XrefRangeStart = 41913, XrefRangeEnd = 41922, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangent(float localF, Vector3 position, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&localF);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		*(Space**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 41929, RefRangeEnd = 41933, XrefRangeStart = 41925, XrefRangeEnd = 41929, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangentFast(float localF, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localF);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangentFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 41936, RefRangeEnd = 41940, XrefRangeStart = 41933, XrefRangeEnd = 41936, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InterpolateAndGetTangent(float localF, out Vector3 position, out Vector3 tangent, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&localF);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref position);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateAndGetTangent_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 41946, RefRangeEnd = 41950, XrefRangeStart = 41940, XrefRangeEnd = 41946, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InterpolateAndGetTangentFast(float localF, out Vector3 position, out Vector3 tangent, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&localF);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref position);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateAndGetTangentFast_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 41954, RefRangeEnd = 41957, XrefRangeStart = 41950, XrefRangeEnd = 41954, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetOrientationUpFast(float localF, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localF);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrientationUpFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 41973, RefRangeEnd = 41975, XrefRangeStart = 41957, XrefRangeEnd = 41973, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Quaternion GetOrientationFast(float localF, bool inverse = false, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&localF);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &inverse;
		*(Space**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrientationFast_Public_Quaternion_Single_Boolean_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Quaternion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41975, XrefRangeEnd = 41988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReloadMetaData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReloadMetaData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41988, XrefRangeEnd = 41993, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RegisterMetaData(CurvyMetadataBase metaData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)metaData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterMetaData_Public_Void_CurvyMetadataBase_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 41993, XrefRangeEnd = 41997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnregisterMetaData(CurvyMetadataBase metaData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)metaData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnregisterMetaData_Public_Void_CurvyMetadataBase_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(18)]
	[CachedScanResults(RefRangeStart = 42036, RefRangeEnd = 42054, XrefRangeStart = 41997, XrefRangeEnd = 42036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetMetadata<T>(bool autoCreate = false) where T : CurvyMetadataBase
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&autoCreate);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetMetadata_Public_T_Boolean_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42054, XrefRangeEnd = 42066, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe U GetInterpolatedMetadata<T, U>(float f) where T : CurvyInterpolatableMetadataBase<U>
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&f);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetInterpolatedMetadata_Public_U_Single_0<T, U>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<U>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42066, XrefRangeEnd = 42074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DeleteMetadata()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteMetadata_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 42076, RefRangeEnd = 42077, XrefRangeStart = 42074, XrefRangeEnd = 42076, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointF(Vector3 position, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&position);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointF_Public_Single_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42078, RefRangeEnd = 42080, XrefRangeStart = 42077, XrefRangeEnd = 42078, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float DistanceToLocalF(float localDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localDistance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DistanceToLocalF_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 42080, RefRangeEnd = 42081, XrefRangeStart = 42080, XrefRangeEnd = 42080, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float LocalFToDistance(float localF)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localF);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LocalFToDistance_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 42083, RefRangeEnd = 42084, XrefRangeStart = 42081, XrefRangeEnd = 42083, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float LocalFToTF(float localF)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localF);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LocalFToTF_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42084, XrefRangeEnd = 42095, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 42099, RefRangeEnd = 42100, XrefRangeStart = 42095, XrefRangeEnd = 42099, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BakeOrientationToTransform()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BakeOrientationToTransform_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42101, RefRangeEnd = 42103, XrefRangeStart = 42100, XrefRangeEnd = 42101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int getApproximationIndexINTERNAL(float localF, out float frag)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localF);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref frag);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getApproximationIndexINTERNAL_Public_Int32_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LinkToSpline(CurvySpline spline)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LinkToSpline_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42103, XrefRangeEnd = 42104, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnlinkFromSpline()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnlinkFromSpline_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42109, RefRangeEnd = 42111, XrefRangeStart = 42104, XrefRangeEnd = 42109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnlinkFromSpline(CurvySpline spline)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnlinkFromSpline_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 42125, RefRangeEnd = 42132, XrefRangeStart = 42111, XrefRangeEnd = 42125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLocalPosition(Vector3 newPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&newPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLocalPosition_Public_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42132, XrefRangeEnd = 42146, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetPosition(Vector3 value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPosition_Public_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 42161, RefRangeEnd = 42164, XrefRangeStart = 42146, XrefRangeEnd = 42161, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLocalRotation(Quaternion value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLocalRotation_Public_Void_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42164, XrefRangeEnd = 42179, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRotation(Quaternion value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRotation_Public_Void_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42180, RefRangeEnd = 42182, XrefRangeStart = 42179, XrefRangeEnd = 42180, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanFollowUpHeadToStart(CurvySplineSegment followUp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)followUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanFollowUpHeadToStart_Public_Static_Boolean_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42183, RefRangeEnd = 42185, XrefRangeStart = 42182, XrefRangeEnd = 42183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanFollowUpHeadToEnd(CurvySplineSegment followUp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)followUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanFollowUpHeadToEnd_Public_Static_Boolean_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42185, XrefRangeEnd = 42198, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 BSpline(ReadOnlyCollection<CurvySplineSegment> controlPoints, float tf, bool isClamped, bool isClosed, int degree, Il2CppStructArray<Vector3> p0Array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoints);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tf;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &isClamped;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &isClosed;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &degree;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)p0Array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BSpline_Public_Static_Vector3_ReadOnlyCollection_1_CurvySplineSegment_Single_Boolean_Boolean_Int32_Il2CppStructArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42198, XrefRangeEnd = 42217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnBeforePush()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBeforePush_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42217, XrefRangeEnd = 42218, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnAfterPop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnAfterPop_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42218, XrefRangeEnd = 42221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42221, XrefRangeEnd = 42232, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42232, XrefRangeEnd = 42246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42246, XrefRangeEnd = 42259, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42259, XrefRangeEnd = 42261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public new unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42307, XrefRangeEnd = 42310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GetBSplineP0s(ReadOnlyCollection<CurvySplineSegment> controlPoints, int controlPointsCount, int degree, int k, Il2CppStructArray<Vector3> pArray)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoints);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointsCount;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &degree;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &k;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pArray);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBSplineP0s_Private_Static_Void_ReadOnlyCollection_1_CurvySplineSegment_Int32_Int32_Int32_Il2CppStructArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void SetExtrinsicPropertiesINTERNAL(ControlPointExtrinsicProperties value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetExtrinsicPropertiesINTERNAL_Internal_Void_ControlPointExtrinsicProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(57)]
	[CachedScanResults(RefRangeStart = 42310, RefRangeEnd = 42367, XrefRangeStart = 42310, XrefRangeEnd = 42310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ref ControlPointExtrinsicProperties GetExtrinsicPropertiesINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetExtrinsicPropertiesINTERNAL_Internal_byref_ControlPointExtrinsicProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return ref *(ControlPointExtrinsicProperties*)intPtr;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42394, RefRangeEnd = 42396, XrefRangeStart = 42372, XrefRangeEnd = 42394, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoInitialValidations()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoInitialValidations_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 42429, RefRangeEnd = 42433, XrefRangeStart = 42396, XrefRangeEnd = 42429, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckAgainstMetaDataDuplication()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckAgainstMetaDataDuplication_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 42445, RefRangeEnd = 42448, XrefRangeStart = 42433, XrefRangeEnd = 42445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool SetConnection(CurvyConnection newConnection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newConnection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetConnection_Private_Boolean_CurvyConnection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42456, RefRangeEnd = 42458, XrefRangeStart = 42448, XrefRangeEnd = 42456, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ConnectionHeadingEnum GetValidateConnectionHeading(ConnectionHeadingEnum connectionHeading, CurvySplineSegment followUp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&connectionHeading);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)followUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetValidateConnectionHeading_Private_Static_ConnectionHeadingEnum_ConnectionHeadingEnum_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ConnectionHeadingEnum*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42458, XrefRangeEnd = 42464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool SetAutoHandles(bool newValue)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&newValue);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAutoHandles_Private_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 42471, RefRangeEnd = 42474, XrefRangeStart = 42464, XrefRangeEnd = 42471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PrepareThreadCompatibleDataINTERNAL(bool useFollowUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&useFollowUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareThreadCompatibleDataINTERNAL_Internal_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42501, RefRangeEnd = 42503, XrefRangeStart = 42474, XrefRangeEnd = 42501, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void refreshCurveINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_refreshCurveINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42503, XrefRangeEnd = 42506, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetSegmentCacheSize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSegmentCacheSize_Private_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 42510, RefRangeEnd = 42511, XrefRangeStart = 42506, XrefRangeEnd = 42510, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void refreshOrientationNoneINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_refreshOrientationNoneINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42534, RefRangeEnd = 42536, XrefRangeStart = 42511, XrefRangeEnd = 42534, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void refreshOrientationStaticINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_refreshOrientationStaticINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 42539, RefRangeEnd = 42543, XrefRangeStart = 42536, XrefRangeEnd = 42539, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void refreshOrientationDynamicINTERNAL(Vector3 initialUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&initialUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_refreshOrientationDynamicINTERNAL_Internal_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42543, XrefRangeEnd = 42546, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateLasProcessedLocalPosition()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateLasProcessedLocalPosition_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 42549, RefRangeEnd = 42552, XrefRangeStart = 42546, XrefRangeEnd = 42549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateLasProcessedLocalRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateLasProcessedLocalRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 42552, RefRangeEnd = 42554, XrefRangeStart = 42552, XrefRangeEnd = 42552, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearBoundsINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearBoundsINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 42558, RefRangeEnd = 42564, XrefRangeStart = 42554, XrefRangeEnd = 42558, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 getOrthoUp0INTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getOrthoUp0INTERNAL_Internal_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42564, XrefRangeEnd = 42574, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 getOrthoUp1INTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getOrthoUp1INTERNAL_Private_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 42575, RefRangeEnd = 42576, XrefRangeStart = 42574, XrefRangeEnd = 42575, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnsetFollowUpWithoutDirtyingINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnsetFollowUpWithoutDirtyingINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42576, XrefRangeEnd = 42598, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoSanityChecks()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoSanityChecks_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSelectionIfNeeded()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSelectionIfNeeded_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ForceHierarchyDrawing()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForceHierarchyDrawing_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42598, XrefRangeEnd = 42603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ResetOnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42603, XrefRangeEnd = 42622, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvySplineSegment>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvySplineSegment(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
