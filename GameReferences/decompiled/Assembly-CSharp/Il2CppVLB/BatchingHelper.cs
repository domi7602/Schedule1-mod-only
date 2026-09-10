using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class BatchingHelper : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_IsGpuInstancingEnabled_Public_Static_Boolean_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMaterialProperties_Public_Static_Void_Material_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_forceEnableDepthBlend_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoesRenderingModePreventBatching_Private_Static_Boolean_ShaderMode_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamSD_VolumetricLightBeamSD_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamSD_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamHD_VolumetricLightBeamHD_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamHD_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamAbstractBase_VolumetricLightBeamAbstractBase_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AppendErrorMessage_Private_Static_Void_byref_String_String_0;

	public unsafe static bool forceEnableDepthBlend
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 68662, RefRangeEnd = 68666, XrefRangeStart = 68660, XrefRangeEnd = 68662, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_forceEnableDepthBlend_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static BatchingHelper()
	{
		Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "BatchingHelper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr);
		NativeMethodInfoPtr_IsGpuInstancingEnabled_Public_Static_Boolean_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663673);
		NativeMethodInfoPtr_SetMaterialProperties_Public_Static_Void_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663674);
		NativeMethodInfoPtr_get_forceEnableDepthBlend_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663675);
		NativeMethodInfoPtr_DoesRenderingModePreventBatching_Private_Static_Boolean_ShaderMode_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663676);
		NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamSD_VolumetricLightBeamSD_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663677);
		NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamSD_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663678);
		NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamHD_VolumetricLightBeamHD_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663679);
		NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamHD_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663680);
		NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamAbstractBase_VolumetricLightBeamAbstractBase_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663681);
		NativeMethodInfoPtr_AppendErrorMessage_Private_Static_Void_byref_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BatchingHelper>.NativeClassPtr, 100663682);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68654, XrefRangeEnd = 68656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsGpuInstancingEnabled(Material material)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsGpuInstancingEnabled_Public_Static_Boolean_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 68658, RefRangeEnd = 68660, XrefRangeStart = 68656, XrefRangeEnd = 68658, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetMaterialProperties(Material material, bool enableGpuInstancing)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &enableGpuInstancing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMaterialProperties_Public_Static_Void_Material_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 68671, RefRangeEnd = 68673, XrefRangeStart = 68666, XrefRangeEnd = 68671, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool DoesRenderingModePreventBatching(ShaderMode shaderMode, ref string reasons)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&shaderMode);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(reasons);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoesRenderingModePreventBatching_Private_Static_Boolean_ShaderMode_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		reasons = IL2CPP.Il2CppStringToManaged(intPtr);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 68747, RefRangeEnd = 68748, XrefRangeStart = 68673, XrefRangeEnd = 68747, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanBeBatched(VolumetricLightBeamSD beamA, VolumetricLightBeamSD beamB, ref string reasons)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beamA);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beamB);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(reasons);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamSD_VolumetricLightBeamSD_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		reasons = IL2CPP.Il2CppStringToManaged(intPtr);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 68768, RefRangeEnd = 68770, XrefRangeStart = 68748, XrefRangeEnd = 68768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanBeBatched(VolumetricLightBeamSD beam, ref string reasons)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beam);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(reasons);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamSD_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		reasons = IL2CPP.Il2CppStringToManaged(intPtr);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 68826, RefRangeEnd = 68827, XrefRangeStart = 68770, XrefRangeEnd = 68826, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanBeBatched(VolumetricLightBeamHD beamA, VolumetricLightBeamHD beamB, ref string reasons)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beamA);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beamB);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(reasons);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamHD_VolumetricLightBeamHD_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		reasons = IL2CPP.Il2CppStringToManaged(intPtr);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 68848, RefRangeEnd = 68850, XrefRangeStart = 68827, XrefRangeEnd = 68848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanBeBatched(VolumetricLightBeamHD beam, ref string reasons)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beam);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(reasons);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamHD_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		reasons = IL2CPP.Il2CppStringToManaged(intPtr);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68850, XrefRangeEnd = 68856, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CanBeBatched(VolumetricLightBeamAbstractBase beamA, VolumetricLightBeamAbstractBase beamB, ref string reasons)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beamA);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beamB);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(reasons);
		*(System.IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanBeBatched_Public_Static_Boolean_VolumetricLightBeamAbstractBase_VolumetricLightBeamAbstractBase_byref_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		reasons = IL2CPP.Il2CppStringToManaged(intPtr);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(15)]
	[CachedScanResults(RefRangeStart = 68867, RefRangeEnd = 68882, XrefRangeStart = 68856, XrefRangeEnd = 68867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void AppendErrorMessage(ref string message, string toAppend)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		System.IntPtr intPtr = IL2CPP.ManagedStringToIl2Cpp(message);
		*ptr = (nint)(&intPtr);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(toAppend);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AppendErrorMessage_Private_Static_Void_byref_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		message = IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public BatchingHelper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
