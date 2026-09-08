using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class GlobalMeshSD : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ms_Mesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_DoubleSided;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_Static_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Destroy_Public_Static_Void_0;

	public unsafe static Mesh ms_Mesh
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_Mesh, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_Mesh, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe static bool ms_DoubleSided
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_DoubleSided, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_DoubleSided, (void*)(&flag));
		}
	}

	static GlobalMeshSD()
	{
		Il2CppClassPointerStore<GlobalMeshSD>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "GlobalMeshSD");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GlobalMeshSD>.NativeClassPtr);
		NativeFieldInfoPtr_ms_Mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GlobalMeshSD>.NativeClassPtr, "ms_Mesh");
		NativeFieldInfoPtr_ms_DoubleSided = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GlobalMeshSD>.NativeClassPtr, "ms_DoubleSided");
		NativeMethodInfoPtr_Get_Public_Static_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GlobalMeshSD>.NativeClassPtr, 100664230);
		NativeMethodInfoPtr_Destroy_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GlobalMeshSD>.NativeClassPtr, 100664231);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 73244, RefRangeEnd = 73245, XrefRangeStart = 73204, XrefRangeEnd = 73244, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh Get()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_Static_Mesh_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73245, XrefRangeEnd = 73258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Destroy()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Destroy_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GlobalMeshSD(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
