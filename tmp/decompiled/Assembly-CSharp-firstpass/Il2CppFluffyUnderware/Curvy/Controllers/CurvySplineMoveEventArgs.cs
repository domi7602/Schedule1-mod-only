using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.ComponentModel;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

public class CurvySplineMoveEventArgs : CancelEventArgs
{
	private static readonly IntPtr NativeFieldInfoPtr__Sender_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__Spline_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__ControlPoint_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__WorldUnits_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__MovementDirection_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__Delta_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__Position_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_Sender_Public_get_SplineController_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Sender_Private_set_Void_SplineController_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Spline_Private_set_Void_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ControlPoint_Public_get_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ControlPoint_Private_set_Void_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_WorldUnits_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_WorldUnits_Private_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MovementDirection_Public_get_MovementDirection_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MovementDirection_Private_set_Void_MovementDirection_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Delta_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Delta_Private_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Position_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Position_Private_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_SplineController_CurvySpline_CurvySplineSegment_Single_Boolean_Single_MovementDirection_0;

	private static readonly IntPtr NativeMethodInfoPtr_Set_INTERNAL_Internal_Void_SplineController_CurvySpline_CurvySplineSegment_Single_Single_MovementDirection_Boolean_0;

	public unsafe SplineController _Sender_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Sender_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SplineController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Sender_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splineController));
		}
	}

	public unsafe CurvySpline _Spline_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Spline_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Spline_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe CurvySplineSegment _ControlPoint_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ControlPoint_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ControlPoint_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
		}
	}

	public unsafe bool _WorldUnits_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__WorldUnits_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__WorldUnits_k__BackingField)) = flag;
		}
	}

	public unsafe MovementDirection _MovementDirection_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MovementDirection_k__BackingField);
			return *(MovementDirection*)num;
		}
		set
		{
			*(MovementDirection*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MovementDirection_k__BackingField)) = movementDirection;
		}
	}

	public unsafe float _Delta_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Delta_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Delta_k__BackingField)) = num;
		}
	}

	public unsafe float _Position_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Position_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Position_k__BackingField)) = num;
		}
	}

	public unsafe SplineController Sender
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Sender_Public_get_SplineController_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SplineController>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Sender_Private_set_Void_SplineController_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySpline Spline
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Spline_Private_set_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineSegment ControlPoint
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ControlPoint_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ControlPoint_Private_set_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool WorldUnits
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_WorldUnits_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_WorldUnits_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe MovementDirection MovementDirection
	{
		[CallerCount(126)]
		[CachedScanResults(RefRangeStart = 41326, RefRangeEnd = 41452, XrefRangeStart = 41326, XrefRangeEnd = 41452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MovementDirection_Public_get_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MovementDirection_Private_set_Void_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Delta
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Delta_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 44937, RefRangeEnd = 44940, XrefRangeStart = 44937, XrefRangeEnd = 44937, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Delta_Private_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Position
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Position_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Position_Private_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static CurvySplineMoveEventArgs()
	{
		Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "CurvySplineMoveEventArgs");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr);
		NativeFieldInfoPtr__Sender_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<Sender>k__BackingField");
		NativeFieldInfoPtr__Spline_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<Spline>k__BackingField");
		NativeFieldInfoPtr__ControlPoint_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<ControlPoint>k__BackingField");
		NativeFieldInfoPtr__WorldUnits_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<WorldUnits>k__BackingField");
		NativeFieldInfoPtr__MovementDirection_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<MovementDirection>k__BackingField");
		NativeFieldInfoPtr__Delta_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<Delta>k__BackingField");
		NativeFieldInfoPtr__Position_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, "<Position>k__BackingField");
		NativeMethodInfoPtr_get_Sender_Public_get_SplineController_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665468);
		NativeMethodInfoPtr_set_Sender_Private_set_Void_SplineController_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665469);
		NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665470);
		NativeMethodInfoPtr_set_Spline_Private_set_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665471);
		NativeMethodInfoPtr_get_ControlPoint_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665472);
		NativeMethodInfoPtr_set_ControlPoint_Private_set_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665473);
		NativeMethodInfoPtr_get_WorldUnits_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665474);
		NativeMethodInfoPtr_set_WorldUnits_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665475);
		NativeMethodInfoPtr_get_MovementDirection_Public_get_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665476);
		NativeMethodInfoPtr_set_MovementDirection_Private_set_Void_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665477);
		NativeMethodInfoPtr_get_Delta_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665478);
		NativeMethodInfoPtr_set_Delta_Private_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665479);
		NativeMethodInfoPtr_get_Position_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665480);
		NativeMethodInfoPtr_set_Position_Private_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665481);
		NativeMethodInfoPtr__ctor_Public_Void_SplineController_CurvySpline_CurvySplineSegment_Single_Boolean_Single_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665482);
		NativeMethodInfoPtr_Set_INTERNAL_Internal_Void_SplineController_CurvySpline_CurvySplineSegment_Single_Single_MovementDirection_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr, 100665483);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44940, XrefRangeEnd = 44944, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineMoveEventArgs(SplineController sender, CurvySpline spline, CurvySplineSegment controlPoint, float position, bool usingWorldUnits, float delta, MovementDirection direction)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvySplineMoveEventArgs>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sender);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &position;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &usingWorldUnits;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &delta;
		*(MovementDirection**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &direction;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SplineController_CurvySpline_CurvySplineSegment_Single_Boolean_Single_MovementDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 44944, XrefRangeEnd = 44947, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Set_INTERNAL(SplineController sender, CurvySpline spline, CurvySplineSegment controlPoint, float position, float delta, MovementDirection direction, bool usingWorldUnits)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sender);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &position;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &delta;
		*(MovementDirection**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &direction;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &usingWorldUnits;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Set_INTERNAL_Internal_Void_SplineController_CurvySpline_CurvySplineSegment_Single_Single_MovementDirection_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvySplineMoveEventArgs(IntPtr pointer)
		: base(pointer)
	{
	}
}
