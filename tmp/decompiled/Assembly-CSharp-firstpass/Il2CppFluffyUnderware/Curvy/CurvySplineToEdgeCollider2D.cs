using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvySplineToEdgeCollider2D : SplineProcessor
{
	private static readonly IntPtr NativeFieldInfoPtr_ComponentPath;

	private static readonly IntPtr NativeFieldInfoPtr_cachedEdgeCollider2D;

	private static readonly IntPtr NativeMethodInfoPtr_get_EdgeCollider_Private_get_EdgeCollider2D_0;

	private static readonly IntPtr NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static string ComponentPath
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ComponentPath, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ComponentPath, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EdgeCollider2D cachedEdgeCollider2D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedEdgeCollider2D);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EdgeCollider2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedEdgeCollider2D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edgeCollider2D));
		}
	}

	public unsafe EdgeCollider2D EdgeCollider
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35412, XrefRangeEnd = 35420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EdgeCollider_Private_get_EdgeCollider2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EdgeCollider2D>(intPtr) : null;
		}
	}

	static CurvySplineToEdgeCollider2D()
	{
		Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySplineToEdgeCollider2D");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr);
		NativeFieldInfoPtr_ComponentPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr, "ComponentPath");
		NativeFieldInfoPtr_cachedEdgeCollider2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr, "cachedEdgeCollider2D");
		NativeMethodInfoPtr_get_EdgeCollider_Private_get_EdgeCollider2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr, 100664542);
		NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr, 100664543);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr, 100664544);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35420, XrefRangeEnd = 35459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 34977, RefRangeEnd = 34989, XrefRangeStart = 34977, XrefRangeEnd = 34989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineToEdgeCollider2D()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvySplineToEdgeCollider2D>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvySplineToEdgeCollider2D(IntPtr pointer)
		: base(pointer)
	{
	}
}
