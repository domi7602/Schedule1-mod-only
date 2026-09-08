using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class ScalingModule : CGModule
{
	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleMode;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleReference;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleOffset;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleUniform;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleX;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleCurveX;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleY;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleCurveY;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleMode_Public_get_ScaleMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleMode_Public_set_Void_ScaleMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleReference_Public_get_CGReferenceMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleReference_Public_set_Void_CGReferenceMode_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleUniform_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleUniform_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleOffset_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleOffset_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleX_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleX_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleMultiplierX_Public_get_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleMultiplierX_Public_set_Void_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleY_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleY_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleMultiplierY_Public_get_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleMultiplierY_Public_set_Void_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetScale_Public_Vector2_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetScale_Protected_Vector2_Int32_SubArray_1_Single_SubArray_1_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetScale_Protected_Static_Vector2_Single_ScaleMode_Single_Boolean_Single_AnimationCurve_Single_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRelativeDistance_Protected_Static_Single_Int32_CGReferenceMode_SubArray_1_Single_SubArray_1_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAdvancedScale_Protected_Static_Vector2_Single_Single_Boolean_Single_AnimationCurve_Single_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetSimpleScale_Protected_Static_Vector2_Boolean_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe ScaleMode m_ScaleMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleMode);
			return *(ScaleMode*)num;
		}
		set
		{
			*(ScaleMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleMode)) = scaleMode;
		}
	}

	public unsafe CGReferenceMode m_ScaleReference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleReference);
			return *(CGReferenceMode*)num;
		}
		set
		{
			*(CGReferenceMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleReference)) = cGReferenceMode;
		}
	}

	public unsafe float m_ScaleOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleOffset)) = num;
		}
	}

	public unsafe bool m_ScaleUniform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleUniform);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleUniform)) = flag;
		}
	}

	public unsafe float m_ScaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleX)) = num;
		}
	}

	public unsafe AnimationCurve m_ScaleCurveX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleCurveX);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleCurveX)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float m_ScaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleY)) = num;
		}
	}

	public unsafe AnimationCurve m_ScaleCurveY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleCurveY);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleCurveY)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe ScaleMode ScaleMode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleMode_Public_get_ScaleMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ScaleMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54797, XrefRangeEnd = 54798, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleMode_Public_set_Void_ScaleMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CGReferenceMode ScaleReference
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleReference_Public_get_CGReferenceMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CGReferenceMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54798, XrefRangeEnd = 54799, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleReference_Public_set_Void_CGReferenceMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ScaleUniform
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleUniform_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54799, XrefRangeEnd = 54800, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleUniform_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float ScaleOffset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleOffset_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54800, XrefRangeEnd = 54801, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleOffset_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float ScaleX
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleX_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54801, XrefRangeEnd = 54802, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleX_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe AnimationCurve ScaleMultiplierX
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleMultiplierX_Public_get_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54802, XrefRangeEnd = 54804, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleMultiplierX_Public_set_Void_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float ScaleY
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleY_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54804, XrefRangeEnd = 54805, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleY_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe AnimationCurve ScaleMultiplierY
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleMultiplierY_Public_get_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54805, XrefRangeEnd = 54807, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleMultiplierY_Public_set_Void_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ScalingModule()
	{
		Il2CppClassPointerStore<ScalingModule>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "ScalingModule");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr);
		NativeFieldInfoPtr_m_ScaleMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleMode");
		NativeFieldInfoPtr_m_ScaleReference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleReference");
		NativeFieldInfoPtr_m_ScaleOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleOffset");
		NativeFieldInfoPtr_m_ScaleUniform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleUniform");
		NativeFieldInfoPtr_m_ScaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleX");
		NativeFieldInfoPtr_m_ScaleCurveX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleCurveX");
		NativeFieldInfoPtr_m_ScaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleY");
		NativeFieldInfoPtr_m_ScaleCurveY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, "m_ScaleCurveY");
		NativeMethodInfoPtr_get_ScaleMode_Public_get_ScaleMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666511);
		NativeMethodInfoPtr_set_ScaleMode_Public_set_Void_ScaleMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666512);
		NativeMethodInfoPtr_get_ScaleReference_Public_get_CGReferenceMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666513);
		NativeMethodInfoPtr_set_ScaleReference_Public_set_Void_CGReferenceMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666514);
		NativeMethodInfoPtr_get_ScaleUniform_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666515);
		NativeMethodInfoPtr_set_ScaleUniform_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666516);
		NativeMethodInfoPtr_get_ScaleOffset_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666517);
		NativeMethodInfoPtr_set_ScaleOffset_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666518);
		NativeMethodInfoPtr_get_ScaleX_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666519);
		NativeMethodInfoPtr_set_ScaleX_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666520);
		NativeMethodInfoPtr_get_ScaleMultiplierX_Public_get_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666521);
		NativeMethodInfoPtr_set_ScaleMultiplierX_Public_set_Void_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666522);
		NativeMethodInfoPtr_get_ScaleY_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666523);
		NativeMethodInfoPtr_set_ScaleY_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666524);
		NativeMethodInfoPtr_get_ScaleMultiplierY_Public_get_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666525);
		NativeMethodInfoPtr_set_ScaleMultiplierY_Public_set_Void_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666526);
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666527);
		NativeMethodInfoPtr_GetScale_Public_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666528);
		NativeMethodInfoPtr_GetScale_Protected_Vector2_Int32_SubArray_1_Single_SubArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666529);
		NativeMethodInfoPtr_GetScale_Protected_Static_Vector2_Single_ScaleMode_Single_Boolean_Single_AnimationCurve_Single_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666530);
		NativeMethodInfoPtr_GetRelativeDistance_Protected_Static_Single_Int32_CGReferenceMode_SubArray_1_Single_SubArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666531);
		NativeMethodInfoPtr_GetAdvancedScale_Protected_Static_Vector2_Single_Single_Boolean_Single_AnimationCurve_Single_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666532);
		NativeMethodInfoPtr_GetSimpleScale_Protected_Static_Vector2_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666533);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr, 100666534);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 54820, RefRangeEnd = 54822, XrefRangeStart = 54807, XrefRangeEnd = 54820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54822, XrefRangeEnd = 54826, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 GetScale(float relativeDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&relativeDistance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetScale_Public_Vector2_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54826, XrefRangeEnd = 54830, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 GetScale(int sampleIndex, SubArray<float> relativeDistances, SubArray<float> sourceRelativeDistances)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&sampleIndex);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)relativeDistances));
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)sourceRelativeDistances));
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetScale_Protected_Vector2_Int32_SubArray_1_Single_SubArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54830, XrefRangeEnd = 54834, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 GetScale(float relativeDistance, ScaleMode mode, float offset, bool isUniform, float scaleX, AnimationCurve scaleMultiplierX, float scaleY, AnimationCurve scaleMultiplierY)
	{
		IntPtr* ptr = stackalloc IntPtr[8];
		*ptr = (nint)(&relativeDistance);
		*(ScaleMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &mode;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &offset;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &isUniform;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &scaleX;
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleMultiplierX);
		*(float**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &scaleY;
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleMultiplierY);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetScale_Protected_Static_Vector2_Single_ScaleMode_Single_Boolean_Single_AnimationCurve_Single_AnimationCurve_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 54834, RefRangeEnd = 54835, XrefRangeStart = 54834, XrefRangeEnd = 54834, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetRelativeDistance(int sampleIndex, CGReferenceMode cgReferenceMode, SubArray<float> relativeDistances, SubArray<float> sourceRelativeDistances)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)(&sampleIndex);
		*(CGReferenceMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &cgReferenceMode;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)relativeDistances));
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)sourceRelativeDistances));
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRelativeDistance_Protected_Static_Single_Int32_CGReferenceMode_SubArray_1_Single_SubArray_1_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54835, XrefRangeEnd = 54838, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 GetAdvancedScale(float relativeDistance, float scaleOffset, bool isUniform, float scaleX, AnimationCurve scaleMultiplierX, float scaleY, AnimationCurve scaleMultiplierY)
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = (nint)(&relativeDistance);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &scaleOffset;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &isUniform;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &scaleX;
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleMultiplierX);
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &scaleY;
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleMultiplierY);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAdvancedScale_Protected_Static_Vector2_Single_Single_Boolean_Single_AnimationCurve_Single_AnimationCurve_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 GetSimpleScale(bool isUniform, float scaleX, float scaleY)
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&isUniform);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &scaleX;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &scaleY;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSimpleScale_Protected_Static_Vector2_Boolean_Single_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 54843, RefRangeEnd = 54845, XrefRangeStart = 54838, XrefRangeEnd = 54843, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ScalingModule()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ScalingModule>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ScalingModule(IntPtr pointer)
		: base(pointer)
	{
	}
}
