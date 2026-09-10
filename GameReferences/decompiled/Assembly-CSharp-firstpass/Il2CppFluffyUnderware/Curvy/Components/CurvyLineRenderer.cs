using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Components;

public class CurvyLineRenderer : SplineProcessor
{
	private static readonly IntPtr NativeFieldInfoPtr_ComponentPath;

	private static readonly IntPtr NativeFieldInfoPtr_cachedLineRenderer;

	private static readonly IntPtr NativeMethodInfoPtr_get_LineRenderer_Private_get_LineRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_EnforceWorldSpaceUsage_Private_Void_0;

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

	public unsafe LineRenderer cachedLineRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedLineRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<LineRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedLineRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lineRenderer));
		}
	}

	public unsafe LineRenderer LineRenderer
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 43581, RefRangeEnd = 43585, XrefRangeStart = 43573, XrefRangeEnd = 43581, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LineRenderer_Private_get_LineRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<LineRenderer>(intPtr) : null;
		}
	}

	static CurvyLineRenderer()
	{
		Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Components", "CurvyLineRenderer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr);
		NativeFieldInfoPtr_ComponentPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, "ComponentPath");
		NativeFieldInfoPtr_cachedLineRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, "cachedLineRenderer");
		NativeMethodInfoPtr_get_LineRenderer_Private_get_LineRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, 100665286);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, 100665287);
		NativeMethodInfoPtr_EnforceWorldSpaceUsage_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, 100665288);
		NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, 100665289);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr, 100665290);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43585, XrefRangeEnd = 43586, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 43593, RefRangeEnd = 43595, XrefRangeStart = 43586, XrefRangeEnd = 43593, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnforceWorldSpaceUsage()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnforceWorldSpaceUsage_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43595, XrefRangeEnd = 43613, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyLineRenderer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyLineRenderer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyLineRenderer(IntPtr pointer)
		: base(pointer)
	{
	}
}
