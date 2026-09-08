using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class Noise3D : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ms_IsSupportedChecked;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_IsSupported;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_NoiseTexture;

	private static readonly System.IntPtr NativeFieldInfoPtr_kMinShaderLevel;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isSupported_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isProperlyLoaded_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isNotSupportedString_Public_Static_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnStartUp_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadIfNeeded_Public_Static_Void_0;

	public unsafe static bool ms_IsSupportedChecked
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_IsSupportedChecked, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_IsSupportedChecked, (void*)(&flag));
		}
	}

	public unsafe static bool ms_IsSupported
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_IsSupported, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_IsSupported, (void*)(&flag));
		}
	}

	public unsafe static Texture3D ms_NoiseTexture
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_NoiseTexture, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture3D>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_NoiseTexture, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture3D));
		}
	}

	public unsafe static int kMinShaderLevel
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kMinShaderLevel, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kMinShaderLevel, (void*)(&num));
		}
	}

	public unsafe static bool isSupported
	{
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 71984, RefRangeEnd = 71993, XrefRangeStart = 71964, XrefRangeEnd = 71984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isSupported_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static bool isProperlyLoaded
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71993, XrefRangeEnd = 71999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isProperlyLoaded_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static string isNotSupportedString
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71999, XrefRangeEnd = 72008, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isNotSupportedString_Public_Static_get_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static Noise3D()
	{
		Il2CppClassPointerStore<Noise3D>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "Noise3D");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Noise3D>.NativeClassPtr);
		NativeFieldInfoPtr_ms_IsSupportedChecked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, "ms_IsSupportedChecked");
		NativeFieldInfoPtr_ms_IsSupported = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, "ms_IsSupported");
		NativeFieldInfoPtr_ms_NoiseTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, "ms_NoiseTexture");
		NativeFieldInfoPtr_kMinShaderLevel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, "kMinShaderLevel");
		NativeMethodInfoPtr_get_isSupported_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, 100664091);
		NativeMethodInfoPtr_get_isProperlyLoaded_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, 100664092);
		NativeMethodInfoPtr_get_isNotSupportedString_Public_Static_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, 100664093);
		NativeMethodInfoPtr_OnStartUp_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, 100664094);
		NativeMethodInfoPtr_LoadIfNeeded_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Noise3D>.NativeClassPtr, 100664095);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 72008, XrefRangeEnd = 72009, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OnStartUp()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnStartUp_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 72028, RefRangeEnd = 72031, XrefRangeStart = 72009, XrefRangeEnd = 72028, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LoadIfNeeded()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadIfNeeded_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Noise3D(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
