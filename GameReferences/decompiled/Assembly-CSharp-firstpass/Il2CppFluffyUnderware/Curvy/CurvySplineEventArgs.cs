using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvySplineEventArgs : CurvyEventArgs
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Spline;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_CurvySpline_Object_0;

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

	static CurvySplineEventArgs()
	{
		Il2CppClassPointerStore<CurvySplineEventArgs>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySplineEventArgs");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineEventArgs>.NativeClassPtr);
		NativeFieldInfoPtr_Spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineEventArgs>.NativeClassPtr, "Spline");
		NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_CurvySpline_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineEventArgs>.NativeClassPtr, 100664949);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 40408, RefRangeEnd = 40415, XrefRangeStart = 40406, XrefRangeEnd = 40408, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineEventArgs(MonoBehaviour sender, CurvySpline spline, Il2CppSystem.Object data = null)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvySplineEventArgs>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sender);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_CurvySpline_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvySplineEventArgs(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
