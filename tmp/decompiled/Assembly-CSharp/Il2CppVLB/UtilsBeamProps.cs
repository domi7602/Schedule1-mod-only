using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class UtilsBeamProps : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_CanChangeDuringPlaytime_Public_Static_Boolean_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInternalLocalRotation_Public_Static_Quaternion_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetThickness_Public_Static_Single_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFallOffEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColorMode_Public_Static_ColorMode_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColorFlat_Public_Static_Color_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColorGradient_Public_Static_Gradient_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConeAngle_Public_Static_Single_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConeRadiusStart_Public_Static_Single_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConeRadiusEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSortingLayerID_Public_Static_Int32_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSortingOrder_Public_Static_Int32_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFadeOutEnabled_Public_Static_Boolean_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFadeOutEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDimensions_Public_Static_Dimensions_VolumetricLightBeamAbstractBase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetGeomSides_Public_Static_Int32_VolumetricLightBeamAbstractBase_0;

	static UtilsBeamProps()
	{
		Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "UtilsBeamProps");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr);
		NativeMethodInfoPtr_CanChangeDuringPlaytime_Public_Static_Boolean_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664421);
		NativeMethodInfoPtr_GetInternalLocalRotation_Public_Static_Quaternion_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664422);
		NativeMethodInfoPtr_GetThickness_Public_Static_Single_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664423);
		NativeMethodInfoPtr_GetFallOffEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664424);
		NativeMethodInfoPtr_GetColorMode_Public_Static_ColorMode_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664425);
		NativeMethodInfoPtr_GetColorFlat_Public_Static_Color_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664426);
		NativeMethodInfoPtr_GetColorGradient_Public_Static_Gradient_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664427);
		NativeMethodInfoPtr_GetConeAngle_Public_Static_Single_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664428);
		NativeMethodInfoPtr_GetConeRadiusStart_Public_Static_Single_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664429);
		NativeMethodInfoPtr_GetConeRadiusEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664430);
		NativeMethodInfoPtr_GetSortingLayerID_Public_Static_Int32_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664431);
		NativeMethodInfoPtr_GetSortingOrder_Public_Static_Int32_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664432);
		NativeMethodInfoPtr_GetFadeOutEnabled_Public_Static_Boolean_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664433);
		NativeMethodInfoPtr_GetFadeOutEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664434);
		NativeMethodInfoPtr_GetDimensions_Public_Static_Dimensions_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664435);
		NativeMethodInfoPtr_GetGeomSides_Public_Static_Int32_VolumetricLightBeamAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilsBeamProps>.NativeClassPtr, 100664436);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74227, XrefRangeEnd = 74233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanChangeDuringPlaytime(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanChangeDuringPlaytime_Public_Static_Boolean_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74233, XrefRangeEnd = 74249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Quaternion GetInternalLocalRotation(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInternalLocalRotation_Public_Static_Quaternion_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Quaternion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74261, RefRangeEnd = 74262, XrefRangeStart = 74249, XrefRangeEnd = 74261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetThickness(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetThickness_Public_Static_Single_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 74274, RefRangeEnd = 74277, XrefRangeStart = 74262, XrefRangeEnd = 74274, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetFallOffEnd(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFallOffEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74277, XrefRangeEnd = 74289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ColorMode GetColorMode(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColorMode_Public_Static_ColorMode_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ColorMode*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74289, XrefRangeEnd = 74302, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetColorFlat(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColorFlat_Public_Static_Color_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74302, XrefRangeEnd = 74314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Gradient GetColorGradient(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColorGradient_Public_Static_Gradient_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Gradient>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74314, XrefRangeEnd = 74326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetConeAngle(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConeAngle_Public_Static_Single_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 74338, RefRangeEnd = 74340, XrefRangeStart = 74326, XrefRangeEnd = 74338, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetConeRadiusStart(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConeRadiusStart_Public_Static_Single_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74340, XrefRangeEnd = 74352, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetConeRadiusEnd(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConeRadiusEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74352, XrefRangeEnd = 74364, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetSortingLayerID(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSortingLayerID_Public_Static_Int32_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74364, XrefRangeEnd = 74376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetSortingOrder(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSortingOrder_Public_Static_Int32_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74376, XrefRangeEnd = 74382, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetFadeOutEnabled(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFadeOutEnabled_Public_Static_Boolean_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74382, XrefRangeEnd = 74388, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetFadeOutEnd(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFadeOutEnd_Public_Static_Single_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 74400, RefRangeEnd = 74403, XrefRangeStart = 74388, XrefRangeEnd = 74400, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dimensions GetDimensions(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDimensions_Public_Static_Dimensions_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Dimensions*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74403, XrefRangeEnd = 74410, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetGeomSides(VolumetricLightBeamAbstractBase self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetGeomSides_Public_Static_Int32_VolumetricLightBeamAbstractBase_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public UtilsBeamProps(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
