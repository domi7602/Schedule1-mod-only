using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyControlPointEventArgs : CurvySplineEventArgs
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "ModeEnum")]
	public enum ModeEnum
	{
		None,
		AddBefore,
		AddAfter,
		Delete
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Mode;

	private static readonly System.IntPtr NativeFieldInfoPtr_ControlPoint;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_CurvySpline_CurvySplineSegment_ModeEnum_Object_0;

	public unsafe ModeEnum Mode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mode);
			return *(ModeEnum*)num;
		}
		set
		{
			*(ModeEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mode)) = modeEnum;
		}
	}

	public unsafe CurvySplineSegment ControlPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPoint);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
		}
	}

	static CurvyControlPointEventArgs()
	{
		Il2CppClassPointerStore<CurvyControlPointEventArgs>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyControlPointEventArgs");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyControlPointEventArgs>.NativeClassPtr);
		NativeFieldInfoPtr_Mode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyControlPointEventArgs>.NativeClassPtr, "Mode");
		NativeFieldInfoPtr_ControlPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyControlPointEventArgs>.NativeClassPtr, "ControlPoint");
		NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_CurvySpline_CurvySplineSegment_ModeEnum_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyControlPointEventArgs>.NativeClassPtr, 100664631);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36886, XrefRangeEnd = 36888, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyControlPointEventArgs(MonoBehaviour sender, CurvySpline spline, CurvySplineSegment cp, ModeEnum mode = ModeEnum.None, Il2CppSystem.Object data = null)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyControlPointEventArgs>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sender);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
		*(ModeEnum**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_CurvySpline_CurvySplineSegment_ModeEnum_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyControlPointEventArgs(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
