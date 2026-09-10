using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Collections.ObjectModel;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

public class SplineController : CurvyController
{
	public class SplineSwitcher : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__StartTime_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__Duration_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__Spline_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__Tf_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__Direction_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__IsSwitching_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_StartTime_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_StartTime_Public_set_Void_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Duration_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_Duration_Public_set_Void_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_Spline_Public_set_Void_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Tf_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_Tf_Public_set_Void_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Direction_Public_get_MovementDirection_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_Direction_Public_set_Void_MovementDirection_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsSwitching_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_IsSwitching_Public_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Progress_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Start_Public_Void_CurvySpline_Single_Single_MovementDirection_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Advance_Public_Void_CurvySpline_MoveModeEnum_Single_CurvyClamping_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Stop_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe float _StartTime_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StartTime_k__BackingField);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StartTime_k__BackingField)) = num;
			}
		}

		public unsafe float _Duration_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Duration_k__BackingField);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Duration_k__BackingField)) = num;
			}
		}

		public unsafe CurvySpline _Spline_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Spline_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Spline_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
			}
		}

		public unsafe float _Tf_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Tf_k__BackingField);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Tf_k__BackingField)) = num;
			}
		}

		public unsafe MovementDirection _Direction_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Direction_k__BackingField);
				return *(MovementDirection*)num;
			}
			set
			{
				*(MovementDirection*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Direction_k__BackingField)) = movementDirection;
			}
		}

		public unsafe bool _IsSwitching_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsSwitching_k__BackingField);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsSwitching_k__BackingField)) = flag;
			}
		}

		public unsafe float StartTime
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartTime_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 29030, RefRangeEnd = 29033, XrefRangeStart = 29030, XrefRangeEnd = 29033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartTime_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe float Duration
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Duration_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 26895, RefRangeEnd = 26896, XrefRangeStart = 26895, XrefRangeEnd = 26896, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Duration_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe CurvySpline Spline
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Spline_Public_set_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe float Tf
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Tf_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 29040, RefRangeEnd = 29041, XrefRangeStart = 29040, XrefRangeEnd = 29041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Tf_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe MovementDirection Direction
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Direction_Public_get_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(2)]
			[CachedScanResults(RefRangeStart = 29041, RefRangeEnd = 29043, XrefRangeStart = 29041, XrefRangeEnd = 29043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Direction_Public_set_Void_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe bool IsSwitching
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsSwitching_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(20)]
			[CachedScanResults(RefRangeStart = 33070, RefRangeEnd = 33090, XrefRangeStart = 33070, XrefRangeEnd = 33090, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsSwitching_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe float Progress
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45031, XrefRangeEnd = 45032, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Progress_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static SplineSwitcher()
		{
			Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "SplineSwitcher");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr);
			NativeFieldInfoPtr__StartTime_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, "<StartTime>k__BackingField");
			NativeFieldInfoPtr__Duration_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, "<Duration>k__BackingField");
			NativeFieldInfoPtr__Spline_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, "<Spline>k__BackingField");
			NativeFieldInfoPtr__Tf_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, "<Tf>k__BackingField");
			NativeFieldInfoPtr__Direction_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, "<Direction>k__BackingField");
			NativeFieldInfoPtr__IsSwitching_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, "<IsSwitching>k__BackingField");
			NativeMethodInfoPtr_get_StartTime_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665577);
			NativeMethodInfoPtr_set_StartTime_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665578);
			NativeMethodInfoPtr_get_Duration_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665579);
			NativeMethodInfoPtr_set_Duration_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665580);
			NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665581);
			NativeMethodInfoPtr_set_Spline_Public_set_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665582);
			NativeMethodInfoPtr_get_Tf_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665583);
			NativeMethodInfoPtr_set_Tf_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665584);
			NativeMethodInfoPtr_get_Direction_Public_get_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665585);
			NativeMethodInfoPtr_set_Direction_Public_set_Void_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665586);
			NativeMethodInfoPtr_get_IsSwitching_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665587);
			NativeMethodInfoPtr_set_IsSwitching_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665588);
			NativeMethodInfoPtr_get_Progress_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665589);
			NativeMethodInfoPtr_Start_Public_Void_CurvySpline_Single_Single_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665590);
			NativeMethodInfoPtr_Advance_Public_Void_CurvySpline_MoveModeEnum_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665591);
			NativeMethodInfoPtr_Stop_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665592);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr, 100665593);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45032, XrefRangeEnd = 45034, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Start(CurvySpline spline, float tf, float duration, MovementDirection direction)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tf;
			*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &duration;
			*(MovementDirection**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &direction;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Public_Void_CurvySpline_Single_Single_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45034, XrefRangeEnd = 45035, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Advance(CurvySpline spline, MoveModeEnum moveMode, float distance, CurvyClamping clamping)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
			*(MoveModeEnum**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &moveMode;
			*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
			*(CurvyClamping**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Advance_Public_Void_CurvySpline_MoveModeEnum_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45035, XrefRangeEnd = 45036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Stop()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Stop_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SplineSwitcher()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SplineSwitcher>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SplineSwitcher(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Spline;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_UseCache;

	private static readonly System.IntPtr NativeFieldInfoPtr_connectionBehavior;

	private static readonly System.IntPtr NativeFieldInfoPtr_allowDirectionChange;

	private static readonly System.IntPtr NativeFieldInfoPtr_rejectCurrentSpline;

	private static readonly System.IntPtr NativeFieldInfoPtr_rejectTooDivergentSplines;

	private static readonly System.IntPtr NativeFieldInfoPtr_maxAllowedDivergenceAngle;

	private static readonly System.IntPtr NativeFieldInfoPtr_connectionCustomSelector;

	private static readonly System.IntPtr NativeFieldInfoPtr_onPositionReachedList;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnControlPointReached;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnEndReached;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnSwitch;

	private static readonly System.IntPtr NativeFieldInfoPtr_Switcher;

	private static readonly System.IntPtr NativeFieldInfoPtr_prePlaySpline;

	private static readonly System.IntPtr NativeFieldInfoPtr_preAllocatedEventArgs;

	private static readonly System.IntPtr NativeFieldInfoPtr_InvalidSegmentErrorMessage;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Spline_Public_Virtual_New_get_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Spline_Public_Virtual_New_set_Void_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UseCache_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UseCache_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ConnectionBehavior_Public_get_SplineControllerConnectionBehavior_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ConnectionBehavior_Public_set_Void_SplineControllerConnectionBehavior_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ConnectionCustomSelector_Public_get_ConnectedControlPointsSelector_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ConnectionCustomSelector_Public_set_Void_ConnectedControlPointsSelector_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllowDirectionChange_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllowDirectionChange_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RejectCurrentSpline_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RejectCurrentSpline_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RejectTooDivergentSplines_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RejectTooDivergentSplines_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxAllowedDivergenceAngle_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_MaxAllowedDivergenceAngle_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnPositionReachedList_Public_get_List_1_OnPositionReachedSettings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnPositionReachedList_Public_set_Void_List_1_OnPositionReachedSettings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnControlPointReached_Public_get_CurvySplineMoveEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnControlPointReached_Public_set_Void_CurvySplineMoveEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnEndReached_Public_get_CurvySplineMoveEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnEndReached_Public_set_Void_CurvySplineMoveEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Length_Public_Virtual_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsSwitching_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SwitchProgress_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnSwitch_Public_get_CurvySplineMoveEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnSwitch_Public_set_Void_CurvySplineMoveEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SwitchTo_Public_Virtual_New_Void_CurvySpline_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FinishCurrentSwitch_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CancelCurrentSwitch_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAngleBetweenConnectedSplines_Public_Static_Single_CurvySplineSegment_MovementDirection_CurvySplineSegment_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsReady_Public_Virtual_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SavePrePlayState_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RestorePrePlayState_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetPrePlayState_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RelativeToAbsolute_Protected_Virtual_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AbsoluteToRelative_Protected_Virtual_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangent_Protected_Virtual_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientation_Protected_Virtual_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Advance_Protected_Virtual_Void_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SimulateAdvance_Protected_Virtual_Void_byref_Single_byref_MovementDirection_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SimulateAdvanceOnSpline_Private_Static_Void_CurvySpline_byref_Single_byref_MovementDirection_Single_MoveModeEnum_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitializedApplyDeltaTime_Protected_Virtual_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeTargetPositionAndRotation_Protected_Virtual_Void_byref_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdvanceSwitching_Private_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSwitchingPositionAndRotation_Private_Void_Vector3_Vector3_Vector3_byref_Vector3_byref_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputePositionAndRotationOnSwitchTarget_Private_Void_byref_Vector3_byref_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MovementCompatibleGetPosition_Private_Static_Single_SplineController_Single_CurvyPositionMode_byref_CurvySplineSegment_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MovementCompatibleSetPosition_Private_Static_Void_SplineController_CurvyPositionMode_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EventAwareMove_Private_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleOnPositionReachedEvents_Private_Void_CurvyPositionMode_Single_Single_Single_byref_Single_Single_CurvySplineSegment_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleOnPositionReachedEvent_Private_Nullable_1_Single_CurvyPositionMode_Single_Single_Single_CurvySplineSegment_byref_Boolean_OnPositionReachedSettings_Nullable_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleReachingNewControlPoint_Private_Void_CurvySplineSegment_Single_CurvyPositionMode_Single_byref_Boolean_byref_CurvySplineSegment_byref_Boolean_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InvokeEventHandler_Private_Void_CurvySplineMoveEvent_CurvySplineMoveEventArgs_CurvyPositionMode_byref_CurvySplineSegment_byref_Boolean_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InvokeEventHandler_Private_Void_CurvySplineMoveEvent_CurvySplineMoveEventArgs_CurvyPositionMode_byref_CurvySplineSegment_byref_Nullable_1_Boolean_byref_Nullable_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleRandomConnectionBehavior_Private_CurvySplineSegment_CurvySplineSegment_MovementDirection_byref_MovementDirection_ReadOnlyCollection_1_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPostConnectionDirection_Private_Static_MovementDirection_CurvySplineSegment_MovementDirection_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleFollowUpConnectionBehavior_Private_CurvySplineSegment_CurvySplineSegment_MovementDirection_byref_MovementDirection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HeadingToDirection_Private_Static_MovementDirection_ConnectionHeadingEnum_CurvySplineSegment_MovementDirection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetControlPointPosition_Private_Static_Single_CurvySplineSegment_CurvyPositionMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ShowRandomConnectionOptions_Private_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SwitchStartTime_Protected_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SwitchDuration_Protected_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SwitchTarget_Protected_get_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TfOnSwitchTarget_Protected_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DirectionOnSwitchTarget_Protected_get_MovementDirection_0;

	public unsafe CurvySpline m_Spline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Spline);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe bool m_UseCache
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseCache);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseCache)) = flag;
		}
	}

	public unsafe SplineControllerConnectionBehavior connectionBehavior
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_connectionBehavior);
			return *(SplineControllerConnectionBehavior*)num;
		}
		set
		{
			*(SplineControllerConnectionBehavior*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_connectionBehavior)) = splineControllerConnectionBehavior;
		}
	}

	public unsafe bool allowDirectionChange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allowDirectionChange);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allowDirectionChange)) = flag;
		}
	}

	public unsafe bool rejectCurrentSpline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rejectCurrentSpline);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rejectCurrentSpline)) = flag;
		}
	}

	public unsafe bool rejectTooDivergentSplines
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rejectTooDivergentSplines);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rejectTooDivergentSplines)) = flag;
		}
	}

	public unsafe float maxAllowedDivergenceAngle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxAllowedDivergenceAngle);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxAllowedDivergenceAngle)) = num;
		}
	}

	public unsafe ConnectedControlPointsSelector connectionCustomSelector
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_connectionCustomSelector);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ConnectedControlPointsSelector>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_connectionCustomSelector)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)connectedControlPointsSelector));
		}
	}

	public unsafe List<OnPositionReachedSettings> onPositionReachedList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onPositionReachedList);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<OnPositionReachedSettings>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onPositionReachedList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe CurvySplineMoveEvent m_OnControlPointReached
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnControlPointReached);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnControlPointReached)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineMoveEvent));
		}
	}

	public unsafe CurvySplineMoveEvent m_OnEndReached
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnEndReached);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnEndReached)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineMoveEvent));
		}
	}

	public unsafe CurvySplineMoveEvent m_OnSwitch
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnSwitch);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnSwitch)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineMoveEvent));
		}
	}

	public unsafe SplineSwitcher Switcher
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Switcher);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SplineSwitcher>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Switcher)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splineSwitcher));
		}
	}

	public unsafe CurvySpline prePlaySpline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_prePlaySpline);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_prePlaySpline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe CurvySplineMoveEventArgs preAllocatedEventArgs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_preAllocatedEventArgs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEventArgs>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_preAllocatedEventArgs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e));
		}
	}

	public unsafe static string InvalidSegmentErrorMessage
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_InvalidSegmentErrorMessage, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_InvalidSegmentErrorMessage, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe virtual CurvySpline Spline
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Spline_Public_Virtual_New_get_CurvySpline_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 38417, RefRangeEnd = 38421, XrefRangeStart = 38417, XrefRangeEnd = 38421, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_set_Spline_Public_Virtual_New_set_Void_CurvySpline_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool UseCache
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UseCache_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UseCache_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SplineControllerConnectionBehavior ConnectionBehavior
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ConnectionBehavior_Public_get_SplineControllerConnectionBehavior_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(SplineControllerConnectionBehavior*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 45077, RefRangeEnd = 45078, XrefRangeStart = 45077, XrefRangeEnd = 45077, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ConnectionBehavior_Public_set_Void_SplineControllerConnectionBehavior_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe ConnectedControlPointsSelector ConnectionCustomSelector
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ConnectionCustomSelector_Public_get_ConnectedControlPointsSelector_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ConnectedControlPointsSelector>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38427, RefRangeEnd = 38428, XrefRangeStart = 38427, XrefRangeEnd = 38428, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ConnectionCustomSelector_Public_set_Void_ConnectedControlPointsSelector_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool AllowDirectionChange
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllowDirectionChange_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllowDirectionChange_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool RejectCurrentSpline
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RejectCurrentSpline_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RejectCurrentSpline_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool RejectTooDivergentSplines
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RejectTooDivergentSplines_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RejectTooDivergentSplines_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float MaxAllowedDivergenceAngle
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxAllowedDivergenceAngle_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MaxAllowedDivergenceAngle_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<OnPositionReachedSettings> OnPositionReachedList
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnPositionReachedList_Public_get_List_1_OnPositionReachedSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<OnPositionReachedSettings>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnPositionReachedList_Public_set_Void_List_1_OnPositionReachedSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineMoveEvent OnControlPointReached
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnControlPointReached_Public_get_CurvySplineMoveEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45078, XrefRangeEnd = 45079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnControlPointReached_Public_set_Void_CurvySplineMoveEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineMoveEvent OnEndReached
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnEndReached_Public_get_CurvySplineMoveEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45079, XrefRangeEnd = 45080, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnEndReached_Public_set_Void_CurvySplineMoveEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override float Length
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45080, XrefRangeEnd = 45081, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Length_Public_Virtual_get_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsSwitching
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsSwitching_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float SwitchProgress
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45081, XrefRangeEnd = 45082, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SwitchProgress_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvySplineMoveEvent OnSwitch
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnSwitch_Public_get_CurvySplineMoveEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineMoveEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45082, XrefRangeEnd = 45083, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnSwitch_Public_set_Void_CurvySplineMoveEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override bool IsReady
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsReady_Public_Virtual_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool ShowRandomConnectionOptions
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowRandomConnectionOptions_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float SwitchStartTime
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SwitchStartTime_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float SwitchDuration
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SwitchDuration_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvySpline SwitchTarget
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SwitchTarget_Protected_get_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
	}

	public unsafe float TfOnSwitchTarget
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TfOnSwitchTarget_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe MovementDirection DirectionOnSwitchTarget
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DirectionOnSwitchTarget_Protected_get_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SplineController()
	{
		Il2CppClassPointerStore<SplineController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "SplineController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SplineController>.NativeClassPtr);
		NativeFieldInfoPtr_m_Spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "m_Spline");
		NativeFieldInfoPtr_m_UseCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "m_UseCache");
		NativeFieldInfoPtr_connectionBehavior = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "connectionBehavior");
		NativeFieldInfoPtr_allowDirectionChange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "allowDirectionChange");
		NativeFieldInfoPtr_rejectCurrentSpline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "rejectCurrentSpline");
		NativeFieldInfoPtr_rejectTooDivergentSplines = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "rejectTooDivergentSplines");
		NativeFieldInfoPtr_maxAllowedDivergenceAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "maxAllowedDivergenceAngle");
		NativeFieldInfoPtr_connectionCustomSelector = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "connectionCustomSelector");
		NativeFieldInfoPtr_onPositionReachedList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "onPositionReachedList");
		NativeFieldInfoPtr_m_OnControlPointReached = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "m_OnControlPointReached");
		NativeFieldInfoPtr_m_OnEndReached = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "m_OnEndReached");
		NativeFieldInfoPtr_m_OnSwitch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "m_OnSwitch");
		NativeFieldInfoPtr_Switcher = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "Switcher");
		NativeFieldInfoPtr_prePlaySpline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "prePlaySpline");
		NativeFieldInfoPtr_preAllocatedEventArgs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "preAllocatedEventArgs");
		NativeFieldInfoPtr_InvalidSegmentErrorMessage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineController>.NativeClassPtr, "InvalidSegmentErrorMessage");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665506);
		NativeMethodInfoPtr_get_Spline_Public_Virtual_New_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665507);
		NativeMethodInfoPtr_set_Spline_Public_Virtual_New_set_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665508);
		NativeMethodInfoPtr_get_UseCache_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665509);
		NativeMethodInfoPtr_set_UseCache_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665510);
		NativeMethodInfoPtr_get_ConnectionBehavior_Public_get_SplineControllerConnectionBehavior_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665511);
		NativeMethodInfoPtr_set_ConnectionBehavior_Public_set_Void_SplineControllerConnectionBehavior_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665512);
		NativeMethodInfoPtr_get_ConnectionCustomSelector_Public_get_ConnectedControlPointsSelector_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665513);
		NativeMethodInfoPtr_set_ConnectionCustomSelector_Public_set_Void_ConnectedControlPointsSelector_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665514);
		NativeMethodInfoPtr_get_AllowDirectionChange_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665515);
		NativeMethodInfoPtr_set_AllowDirectionChange_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665516);
		NativeMethodInfoPtr_get_RejectCurrentSpline_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665517);
		NativeMethodInfoPtr_set_RejectCurrentSpline_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665518);
		NativeMethodInfoPtr_get_RejectTooDivergentSplines_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665519);
		NativeMethodInfoPtr_set_RejectTooDivergentSplines_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665520);
		NativeMethodInfoPtr_get_MaxAllowedDivergenceAngle_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665521);
		NativeMethodInfoPtr_set_MaxAllowedDivergenceAngle_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665522);
		NativeMethodInfoPtr_get_OnPositionReachedList_Public_get_List_1_OnPositionReachedSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665523);
		NativeMethodInfoPtr_set_OnPositionReachedList_Public_set_Void_List_1_OnPositionReachedSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665524);
		NativeMethodInfoPtr_get_OnControlPointReached_Public_get_CurvySplineMoveEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665525);
		NativeMethodInfoPtr_set_OnControlPointReached_Public_set_Void_CurvySplineMoveEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665526);
		NativeMethodInfoPtr_get_OnEndReached_Public_get_CurvySplineMoveEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665527);
		NativeMethodInfoPtr_set_OnEndReached_Public_set_Void_CurvySplineMoveEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665528);
		NativeMethodInfoPtr_get_Length_Public_Virtual_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665529);
		NativeMethodInfoPtr_get_IsSwitching_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665530);
		NativeMethodInfoPtr_get_SwitchProgress_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665531);
		NativeMethodInfoPtr_get_OnSwitch_Public_get_CurvySplineMoveEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665532);
		NativeMethodInfoPtr_set_OnSwitch_Public_set_Void_CurvySplineMoveEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665533);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665534);
		NativeMethodInfoPtr_SwitchTo_Public_Virtual_New_Void_CurvySpline_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665535);
		NativeMethodInfoPtr_FinishCurrentSwitch_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665536);
		NativeMethodInfoPtr_CancelCurrentSwitch_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665537);
		NativeMethodInfoPtr_GetAngleBetweenConnectedSplines_Public_Static_Single_CurvySplineSegment_MovementDirection_CurvySplineSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665538);
		NativeMethodInfoPtr_get_IsReady_Public_Virtual_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665539);
		NativeMethodInfoPtr_SavePrePlayState_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665540);
		NativeMethodInfoPtr_RestorePrePlayState_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665541);
		NativeMethodInfoPtr_ResetPrePlayState_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665542);
		NativeMethodInfoPtr_RelativeToAbsolute_Protected_Virtual_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665543);
		NativeMethodInfoPtr_AbsoluteToRelative_Protected_Virtual_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665544);
		NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665545);
		NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665546);
		NativeMethodInfoPtr_GetTangent_Protected_Virtual_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665547);
		NativeMethodInfoPtr_GetOrientation_Protected_Virtual_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665548);
		NativeMethodInfoPtr_Advance_Protected_Virtual_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665549);
		NativeMethodInfoPtr_SimulateAdvance_Protected_Virtual_Void_byref_Single_byref_MovementDirection_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665550);
		NativeMethodInfoPtr_SimulateAdvanceOnSpline_Private_Static_Void_CurvySpline_byref_Single_byref_MovementDirection_Single_MoveModeEnum_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665551);
		NativeMethodInfoPtr_InitializedApplyDeltaTime_Protected_Virtual_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665552);
		NativeMethodInfoPtr_ComputeTargetPositionAndRotation_Protected_Virtual_Void_byref_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665553);
		NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665554);
		NativeMethodInfoPtr_AdvanceSwitching_Private_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665555);
		NativeMethodInfoPtr_GetSwitchingPositionAndRotation_Private_Void_Vector3_Vector3_Vector3_byref_Vector3_byref_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665556);
		NativeMethodInfoPtr_ComputePositionAndRotationOnSwitchTarget_Private_Void_byref_Vector3_byref_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665557);
		NativeMethodInfoPtr_MovementCompatibleGetPosition_Private_Static_Single_SplineController_Single_CurvyPositionMode_byref_CurvySplineSegment_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665558);
		NativeMethodInfoPtr_MovementCompatibleSetPosition_Private_Static_Void_SplineController_CurvyPositionMode_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665559);
		NativeMethodInfoPtr_EventAwareMove_Private_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665560);
		NativeMethodInfoPtr_HandleOnPositionReachedEvents_Private_Void_CurvyPositionMode_Single_Single_Single_byref_Single_Single_CurvySplineSegment_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665561);
		NativeMethodInfoPtr_HandleOnPositionReachedEvent_Private_Nullable_1_Single_CurvyPositionMode_Single_Single_Single_CurvySplineSegment_byref_Boolean_OnPositionReachedSettings_Nullable_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665562);
		NativeMethodInfoPtr_HandleReachingNewControlPoint_Private_Void_CurvySplineSegment_Single_CurvyPositionMode_Single_byref_Boolean_byref_CurvySplineSegment_byref_Boolean_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665563);
		NativeMethodInfoPtr_InvokeEventHandler_Private_Void_CurvySplineMoveEvent_CurvySplineMoveEventArgs_CurvyPositionMode_byref_CurvySplineSegment_byref_Boolean_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665564);
		NativeMethodInfoPtr_InvokeEventHandler_Private_Void_CurvySplineMoveEvent_CurvySplineMoveEventArgs_CurvyPositionMode_byref_CurvySplineSegment_byref_Nullable_1_Boolean_byref_Nullable_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665565);
		NativeMethodInfoPtr_HandleRandomConnectionBehavior_Private_CurvySplineSegment_CurvySplineSegment_MovementDirection_byref_MovementDirection_ReadOnlyCollection_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665566);
		NativeMethodInfoPtr_GetPostConnectionDirection_Private_Static_MovementDirection_CurvySplineSegment_MovementDirection_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665567);
		NativeMethodInfoPtr_HandleFollowUpConnectionBehavior_Private_CurvySplineSegment_CurvySplineSegment_MovementDirection_byref_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665568);
		NativeMethodInfoPtr_HeadingToDirection_Private_Static_MovementDirection_ConnectionHeadingEnum_CurvySplineSegment_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665569);
		NativeMethodInfoPtr_GetControlPointPosition_Private_Static_Single_CurvySplineSegment_CurvyPositionMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665570);
		NativeMethodInfoPtr_get_ShowRandomConnectionOptions_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665571);
		NativeMethodInfoPtr_get_SwitchStartTime_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665572);
		NativeMethodInfoPtr_get_SwitchDuration_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665573);
		NativeMethodInfoPtr_get_SwitchTarget_Protected_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665574);
		NativeMethodInfoPtr_get_TfOnSwitchTarget_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665575);
		NativeMethodInfoPtr_get_DirectionOnSwitchTarget_Protected_get_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineController>.NativeClassPtr, 100665576);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45076, RefRangeEnd = 45077, XrefRangeStart = 45036, XrefRangeEnd = 45076, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SplineController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SplineController>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45099, RefRangeEnd = 45100, XrefRangeStart = 45083, XrefRangeEnd = 45099, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45100, XrefRangeEnd = 45105, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void SwitchTo(CurvySpline destinationSpline, float destinationTf, float duration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destinationSpline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &destinationTf;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &duration;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SwitchTo_Public_Virtual_New_Void_CurvySpline_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45105, XrefRangeEnd = 45107, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FinishCurrentSwitch()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FinishCurrentSwitch_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45107, XrefRangeEnd = 45108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CancelCurrentSwitch()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CancelCurrentSwitch_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45113, RefRangeEnd = 45114, XrefRangeStart = 45108, XrefRangeEnd = 45113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetAngleBetweenConnectedSplines(CurvySplineSegment before, MovementDirection movementMode, CurvySplineSegment after, bool allowMovementModeChange)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)before);
		*(MovementDirection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &movementMode;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)after);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &allowMovementModeChange;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAngleBetweenConnectedSplines_Public_Static_Single_CurvySplineSegment_MovementDirection_CurvySplineSegment_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45114, XrefRangeEnd = 45115, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SavePrePlayState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SavePrePlayState_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void RestorePrePlayState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_RestorePrePlayState_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45115, XrefRangeEnd = 45116, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ResetPrePlayState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ResetPrePlayState_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45116, XrefRangeEnd = 45118, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override float RelativeToAbsolute(float relativeDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&relativeDistance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_RelativeToAbsolute_Protected_Virtual_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45118, XrefRangeEnd = 45120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override float AbsoluteToRelative(float worldUnitDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&worldUnitDistance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_AbsoluteToRelative_Protected_Virtual_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45120, XrefRangeEnd = 45125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Vector3 GetInterpolatedSourcePosition(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Vector3_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45125, XrefRangeEnd = 45141, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void GetInterpolatedSourcePosition(float tf, out Vector3 interpolatedPosition, out Vector3 tangent, out Vector3 up)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref interpolatedPosition);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref up);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45141, XrefRangeEnd = 45146, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Vector3 GetTangent(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetTangent_Protected_Virtual_Vector3_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45146, XrefRangeEnd = 45149, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Vector3 GetOrientation(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetOrientation_Protected_Virtual_Vector3_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45149, XrefRangeEnd = 45153, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Advance(float speed, float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&speed);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &deltaTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Advance_Protected_Virtual_Void_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45153, XrefRangeEnd = 45154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SimulateAdvance(ref float tf, ref MovementDirection direction, float speed, float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)Unsafe.AsPointer(ref tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref direction);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &speed;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &deltaTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SimulateAdvance_Protected_Virtual_Void_byref_Single_byref_MovementDirection_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 45161, RefRangeEnd = 45164, XrefRangeStart = 45154, XrefRangeEnd = 45161, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SimulateAdvanceOnSpline(CurvySpline spline, ref float tf, ref MovementDirection direction, float distance, MoveModeEnum moveModeEnum, CurvyClamping curvyClamping)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tf);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref direction);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		*(MoveModeEnum**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &moveModeEnum;
		*(CurvyClamping**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &curvyClamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SimulateAdvanceOnSpline_Private_Static_Void_CurvySpline_byref_Single_byref_MovementDirection_Single_MoveModeEnum_CurvyClamping_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45164, XrefRangeEnd = 45171, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void InitializedApplyDeltaTime(float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&deltaTime);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_InitializedApplyDeltaTime_Protected_Virtual_Void_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45171, XrefRangeEnd = 45172, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ComputeTargetPositionAndRotation(out Vector3 targetPosition, out Vector3 targetUp, out Vector3 targetForward)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)Unsafe.AsPointer(ref targetPosition);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref targetUp);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref targetForward);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ComputeTargetPositionAndRotation_Protected_Virtual_Void_byref_Vector3_byref_Vector3_byref_Vector3_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45172, XrefRangeEnd = 45177, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ResetOnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45187, RefRangeEnd = 45188, XrefRangeStart = 45177, XrefRangeEnd = 45187, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AdvanceSwitching(float distance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&distance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdvanceSwitching_Private_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45188, XrefRangeEnd = 45202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetSwitchingPositionAndRotation(Vector3 forwardOnCurrentSpline, Vector3 upOnCurrentSpline, Vector3 positionOnCurrentSpline, out Vector3 interpolatedPosition, out Quaternion interpolatedRotation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&forwardOnCurrentSpline);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &upOnCurrentSpline;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionOnCurrentSpline;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref interpolatedPosition);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref interpolatedRotation);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSwitchingPositionAndRotation_Private_Void_Vector3_Vector3_Vector3_byref_Vector3_byref_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45202, XrefRangeEnd = 45210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputePositionAndRotationOnSwitchTarget(out Vector3 positionOnSwitchToSpline, out Quaternion rotationOnSwitchToSpline)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref positionOnSwitchToSpline);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref rotationOnSwitchToSpline);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputePositionAndRotationOnSwitchTarget_Private_Void_byref_Vector3_byref_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45218, RefRangeEnd = 45219, XrefRangeStart = 45210, XrefRangeEnd = 45218, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float MovementCompatibleGetPosition(SplineController controller, float clampedPosition, CurvyPositionMode positionMode, out CurvySplineSegment controlPoint, out bool isOnControlPoint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controller);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clampedPosition;
		*(CurvyPositionMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionMode;
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isOnControlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MovementCompatibleGetPosition_Private_Static_Single_SplineController_Single_CurvyPositionMode_byref_CurvySplineSegment_byref_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		controlPoint = ((num3 == 0) ? null : new CurvySplineSegment(num3));
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 45220, RefRangeEnd = 45222, XrefRangeStart = 45219, XrefRangeEnd = 45220, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MovementCompatibleSetPosition(SplineController controller, CurvyPositionMode positionMode, float specialClampedPosition)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controller);
		*(CurvyPositionMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionMode;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &specialClampedPosition;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MovementCompatibleSetPosition_Private_Static_Void_SplineController_CurvyPositionMode_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45233, RefRangeEnd = 45234, XrefRangeStart = 45222, XrefRangeEnd = 45233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EventAwareMove(float distance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&distance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EventAwareMove_Private_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45234, XrefRangeEnd = 45258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleOnPositionReachedEvents(CurvyPositionMode positionMode, float startPosition, float endPosition, float endPositionUnclamped, out float postEventsEndPosition, float currentDelta, CurvySplineSegment currentCp, ref bool cancelMovement)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = (nint)(&positionMode);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPosition;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPosition;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPositionUnclamped;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref postEventsEndPosition);
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDelta;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentCp);
		*(void**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref cancelMovement);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleOnPositionReachedEvents_Private_Void_CurvyPositionMode_Single_Single_Single_byref_Single_Single_CurvySplineSegment_byref_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 45272, RefRangeEnd = 45274, XrefRangeStart = 45258, XrefRangeEnd = 45272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppSystem.Nullable<float> HandleOnPositionReachedEvent(CurvyPositionMode positionMode, float startPosition, float endPositionUnclamped, float currentDelta, CurvySplineSegment currentCp, ref bool cancelMovement, OnPositionReachedSettings settings, Il2CppSystem.Nullable<float> postEventEndPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = (nint)(&positionMode);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPosition;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPositionUnclamped;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDelta;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentCp);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref cancelMovement);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)postEventEndPosition));
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleOnPositionReachedEvent_Private_Nullable_1_Single_CurvyPositionMode_Single_Single_Single_CurvySplineSegment_byref_Boolean_OnPositionReachedSettings_Nullable_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new Il2CppSystem.Nullable<float>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45274, XrefRangeEnd = 45289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleReachingNewControlPoint(CurvySplineSegment controlPoint, float controlPointPosition, CurvyPositionMode positionMode, float currentDelta, ref bool cancelMovement, out CurvySplineSegment postEventsControlPoint, out bool postEventsIsControllerOnControlPoint, out float postEventsControlPointPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointPosition;
		*(CurvyPositionMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionMode;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDelta;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref cancelMovement);
		byte* num = (byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		*(void**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref postEventsIsControllerOnControlPoint);
		*(void**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref postEventsControlPointPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleReachingNewControlPoint_Private_Void_CurvySplineSegment_Single_CurvyPositionMode_Single_byref_Boolean_byref_CurvySplineSegment_byref_Boolean_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		postEventsControlPoint = ((num3 == 0) ? null : new CurvySplineSegment(num3));
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 45314, RefRangeEnd = 45316, XrefRangeStart = 45289, XrefRangeEnd = 45314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InvokeEventHandler(CurvySplineMoveEvent @event, CurvySplineMoveEventArgs eventArgument, CurvyPositionMode positionMode, ref CurvySplineSegment postEventsControlPoint, ref bool postEventsIsControllerOnControlPoint, ref float postEventPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@event);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eventArgument);
		*(CurvyPositionMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionMode;
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)postEventsControlPoint);
		*(System.IntPtr**)num = &intPtr;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref postEventsIsControllerOnControlPoint);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref postEventPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InvokeEventHandler_Private_Void_CurvySplineMoveEvent_CurvySplineMoveEventArgs_CurvyPositionMode_byref_CurvySplineSegment_byref_Boolean_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		System.IntPtr intPtr4 = intPtr;
		postEventsControlPoint = ((intPtr4 == (System.IntPtr)0) ? null : new CurvySplineSegment(intPtr4));
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45322, RefRangeEnd = 45323, XrefRangeStart = 45316, XrefRangeEnd = 45322, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InvokeEventHandler(CurvySplineMoveEvent @event, CurvySplineMoveEventArgs eventArgument, CurvyPositionMode positionMode, out CurvySplineSegment postEventsControlPoint, out Il2CppSystem.Nullable<bool> postEventsIsControllerOnControlPoint, out Il2CppSystem.Nullable<float> postEventPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@event);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eventArgument);
		*(CurvyPositionMode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionMode;
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		byte* num5 = (byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num6 = 0;
		*(nint**)num5 = &num6;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InvokeEventHandler_Private_Void_CurvySplineMoveEvent_CurvySplineMoveEventArgs_CurvyPositionMode_byref_CurvySplineSegment_byref_Nullable_1_Boolean_byref_Nullable_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num7 = num2;
		postEventsControlPoint = ((num7 == 0) ? null : new CurvySplineSegment(num7));
		nint num8 = num4;
		postEventsIsControllerOnControlPoint = ((num8 == 0) ? null : new Il2CppSystem.Nullable<bool>(num8));
		nint num9 = num6;
		postEventPosition = ((num9 == 0) ? null : new Il2CppSystem.Nullable<float>(num9));
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45323, XrefRangeEnd = 45352, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment HandleRandomConnectionBehavior(CurvySplineSegment currentControlPoint, MovementDirection currentDirection, out MovementDirection newDirection, ReadOnlyCollection<CurvySplineSegment> connectedControlPoints)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentControlPoint);
		*(MovementDirection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDirection;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref newDirection);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)connectedControlPoints);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRandomConnectionBehavior_Private_CurvySplineSegment_CurvySplineSegment_MovementDirection_byref_MovementDirection_ReadOnlyCollection_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45352, XrefRangeEnd = 45353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MovementDirection GetPostConnectionDirection(CurvySplineSegment connectedControlPoint, MovementDirection currentDirection, bool directionChangeAllowed)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)connectedControlPoint);
		*(MovementDirection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDirection;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &directionChangeAllowed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPostConnectionDirection_Private_Static_MovementDirection_CurvySplineSegment_MovementDirection_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45353, XrefRangeEnd = 45363, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment HandleFollowUpConnectionBehavior(CurvySplineSegment currentControlPoint, MovementDirection currentDirection, out MovementDirection newDirection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentControlPoint);
		*(MovementDirection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDirection;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref newDirection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleFollowUpConnectionBehavior_Private_CurvySplineSegment_CurvySplineSegment_MovementDirection_byref_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 45364, RefRangeEnd = 45368, XrefRangeStart = 45363, XrefRangeEnd = 45364, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MovementDirection HeadingToDirection(ConnectionHeadingEnum heading, CurvySplineSegment controlPoint, MovementDirection currentDirection)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&heading);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(MovementDirection**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentDirection;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HeadingToDirection_Private_Static_MovementDirection_ConnectionHeadingEnum_CurvySplineSegment_MovementDirection_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45368, XrefRangeEnd = 45369, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetControlPointPosition(CurvySplineSegment controlPoint, CurvyPositionMode positionMode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(CurvyPositionMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionMode;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetControlPointPosition_Private_Static_Single_CurvySplineSegment_CurvyPositionMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public SplineController(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
