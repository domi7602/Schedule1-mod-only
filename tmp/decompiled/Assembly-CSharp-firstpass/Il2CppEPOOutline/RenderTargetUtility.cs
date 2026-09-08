using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppEPOOutline;

public static class RenderTargetUtility : Il2CppSystem.Object
{
	[StructLayout(LayoutKind.Explicit)]
	public struct RenderTextureInfo
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Descriptor;

		private static readonly System.IntPtr NativeFieldInfoPtr_FilterMode;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_RenderTextureDescriptor_FilterMode_0;

		[FieldOffset(0)]
		public readonly RenderTextureDescriptor Descriptor;

		[FieldOffset(52)]
		public readonly FilterMode FilterMode;

		static RenderTextureInfo()
		{
			Il2CppClassPointerStore<RenderTextureInfo>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, "RenderTextureInfo");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RenderTextureInfo>.NativeClassPtr);
			NativeFieldInfoPtr_Descriptor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderTextureInfo>.NativeClassPtr, "Descriptor");
			NativeFieldInfoPtr_FilterMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderTextureInfo>.NativeClassPtr, "FilterMode");
			NativeMethodInfoPtr__ctor_Public_Void_RenderTextureDescriptor_FilterMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTextureInfo>.NativeClassPtr, 100667453);
		}

		[CallerCount(0)]
		public unsafe RenderTextureInfo(RenderTextureDescriptor descriptor, FilterMode filterMode)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&descriptor);
			*(FilterMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &filterMode;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_RenderTextureDescriptor_FilterMode_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<RenderTextureInfo>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_hdrFormat;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDepthSliceForEye_Public_Static_Int32_StereoTargetEyeMask_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComposeTarget_Public_Static_RenderTargetIdentifier_OutlineParameters_RenderTargetIdentifier_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsUsingVR_Public_Static_Boolean_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTargetInfo_Public_Static_RenderTextureInfo_OutlineParameters_Int32_Int32_Int32_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTemporaryRT_Public_Static_Void_OutlineParameters_Int32_Int32_Int32_Int32_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHDRFormat_Private_Static_RenderTextureFormat_0;

	public unsafe static Il2CppSystem.Nullable<RenderTextureFormat> hdrFormat
	{
		get
		{
			System.IntPtr intPtr = (nint)stackalloc byte[(int)(uint)IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<RenderTextureFormat>>.NativeClassPtr, ref *(uint*)null)];
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_hdrFormat, (void*)intPtr);
			return new Il2CppSystem.Nullable<RenderTextureFormat>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<RenderTextureFormat>>.NativeClassPtr, intPtr));
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_hdrFormat, (void*)IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)nullable)));
		}
	}

	static RenderTargetUtility()
	{
		Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "RenderTargetUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr);
		NativeFieldInfoPtr_hdrFormat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, "hdrFormat");
		NativeMethodInfoPtr_GetDepthSliceForEye_Public_Static_Int32_StereoTargetEyeMask_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, 100667447);
		NativeMethodInfoPtr_ComposeTarget_Public_Static_RenderTargetIdentifier_OutlineParameters_RenderTargetIdentifier_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, 100667448);
		NativeMethodInfoPtr_IsUsingVR_Public_Static_Boolean_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, 100667449);
		NativeMethodInfoPtr_GetTargetInfo_Public_Static_RenderTextureInfo_OutlineParameters_Int32_Int32_Int32_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, 100667450);
		NativeMethodInfoPtr_GetTemporaryRT_Public_Static_Void_OutlineParameters_Int32_Int32_Int32_Int32_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, 100667451);
		NativeMethodInfoPtr_GetHDRFormat_Private_Static_RenderTextureFormat_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenderTargetUtility>.NativeClassPtr, 100667452);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 64260, RefRangeEnd = 64266, XrefRangeStart = 64260, XrefRangeEnd = 64260, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetDepthSliceForEye(StereoTargetEyeMask mask)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mask);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDepthSliceForEye_Public_Static_Int32_StereoTargetEyeMask_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 64269, RefRangeEnd = 64277, XrefRangeStart = 64266, XrefRangeEnd = 64269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RenderTargetIdentifier ComposeTarget(OutlineParameters parameters, RenderTargetIdentifier target)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &target;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComposeTarget_Public_Static_RenderTargetIdentifier_OutlineParameters_RenderTargetIdentifier_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RenderTargetIdentifier*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64277, XrefRangeEnd = 64278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsUsingVR(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsUsingVR_Public_Static_Boolean_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 64304, RefRangeEnd = 64305, XrefRangeStart = 64278, XrefRangeEnd = 64304, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RenderTextureInfo GetTargetInfo(OutlineParameters parameters, int width, int height, int depthBuffer, bool forceNoAA, bool noFiltering)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &width;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &height;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &depthBuffer;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &forceNoAA;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &noFiltering;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTargetInfo_Public_Static_RenderTextureInfo_OutlineParameters_Int32_Int32_Int32_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RenderTextureInfo*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 64313, RefRangeEnd = 64319, XrefRangeStart = 64305, XrefRangeEnd = 64313, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GetTemporaryRT(OutlineParameters parameters, int id, int width, int height, int depthBuffer, bool clear, bool forceNoAA, bool noFiltering)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &id;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &width;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &height;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &depthBuffer;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &clear;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &forceNoAA;
		*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &noFiltering;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTemporaryRT_Public_Static_Void_OutlineParameters_Int32_Int32_Int32_Int32_Boolean_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64319, XrefRangeEnd = 64339, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RenderTextureFormat GetHDRFormat()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHDRFormat_Private_Static_RenderTextureFormat_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RenderTextureFormat*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public RenderTargetUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
