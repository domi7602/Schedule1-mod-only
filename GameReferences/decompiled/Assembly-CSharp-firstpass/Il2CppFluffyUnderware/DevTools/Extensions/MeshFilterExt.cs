using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class MeshFilterExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareNewShared_Public_Static_Mesh_MeshFilter_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateTangents_Public_Static_Void_MeshFilter_0;

	static MeshFilterExt()
	{
		Il2CppClassPointerStore<MeshFilterExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "MeshFilterExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshFilterExt>.NativeClassPtr);
		NativeMethodInfoPtr_PrepareNewShared_Public_Static_Mesh_MeshFilter_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshFilterExt>.NativeClassPtr, 100664408);
		NativeMethodInfoPtr_CalculateTangents_Public_Static_Void_MeshFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshFilterExt>.NativeClassPtr, 100664409);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 34290, RefRangeEnd = 34291, XrefRangeStart = 34267, XrefRangeEnd = 34290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh PrepareNewShared(this MeshFilter m, string name = "Mesh")
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareNewShared_Public_Static_Mesh_MeshFilter_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34291, XrefRangeEnd = 34312, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CalculateTangents(this MeshFilter m)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateTangents_Public_Static_Void_MeshFilter_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MeshFilterExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
