using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne;

public static class RigidbodyExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetWorldTransformData_Public_Static_TransformData_Rigidbody_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLocalTransformData_Public_Static_Void_Rigidbody_TransformData_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetWorldTransformData_Public_Static_Void_Rigidbody_TransformData_0;

	static RigidbodyExtensions()
	{
		Il2CppClassPointerStore<RigidbodyExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "RigidbodyExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RigidbodyExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_GetWorldTransformData_Public_Static_TransformData_Rigidbody_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RigidbodyExtensions>.NativeClassPtr, 100665937);
		NativeMethodInfoPtr_SetLocalTransformData_Public_Static_Void_Rigidbody_TransformData_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RigidbodyExtensions>.NativeClassPtr, 100665938);
		NativeMethodInfoPtr_SetWorldTransformData_Public_Static_Void_Rigidbody_TransformData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RigidbodyExtensions>.NativeClassPtr, 100665939);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90874, XrefRangeEnd = 90878, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static TransformData GetWorldTransformData(this Rigidbody rb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWorldTransformData_Public_Static_TransformData_Rigidbody_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(TransformData*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90878, XrefRangeEnd = 90879, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetLocalTransformData(this Rigidbody rb, TransformData data, bool setScale = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rb);
		*(TransformData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &data;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &setScale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLocalTransformData_Public_Static_Void_Rigidbody_TransformData_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90879, XrefRangeEnd = 90881, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetWorldTransformData(this Rigidbody rb, TransformData data)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rb);
		*(TransformData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &data;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetWorldTransformData_Public_Static_Void_Rigidbody_TransformData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RigidbodyExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
