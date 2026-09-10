using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class GlobalMeshHD : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ms_Mesh;

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

	static GlobalMeshHD()
	{
		Il2CppClassPointerStore<GlobalMeshHD>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "GlobalMeshHD");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GlobalMeshHD>.NativeClassPtr);
		NativeFieldInfoPtr_ms_Mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GlobalMeshHD>.NativeClassPtr, "ms_Mesh");
		NativeMethodInfoPtr_Get_Public_Static_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GlobalMeshHD>.NativeClassPtr, 100663824);
		NativeMethodInfoPtr_Destroy_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GlobalMeshHD>.NativeClassPtr, 100663825);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 70079, RefRangeEnd = 70081, XrefRangeStart = 70045, XrefRangeEnd = 70079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh Get()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_Static_Mesh_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70081, XrefRangeEnd = 70094, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Destroy()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Destroy_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GlobalMeshHD(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
