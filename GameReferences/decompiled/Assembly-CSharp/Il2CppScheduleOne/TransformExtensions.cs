using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne;

public static class TransformExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetWorldTransformData_Public_Static_TransformData_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalTransformData_Public_Static_TransformData_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLocalTransformData_Public_Static_Void_Transform_TransformData_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetWorldTransformData_Public_Static_Void_Transform_TransformData_0;

	static TransformExtensions()
	{
		Il2CppClassPointerStore<TransformExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "TransformExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_GetWorldTransformData_Public_Static_TransformData_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformExtensions>.NativeClassPtr, 100665933);
		NativeMethodInfoPtr_GetLocalTransformData_Public_Static_TransformData_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformExtensions>.NativeClassPtr, 100665934);
		NativeMethodInfoPtr_SetLocalTransformData_Public_Static_Void_Transform_TransformData_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformExtensions>.NativeClassPtr, 100665935);
		NativeMethodInfoPtr_SetWorldTransformData_Public_Static_Void_Transform_TransformData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformExtensions>.NativeClassPtr, 100665936);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 90859, RefRangeEnd = 90861, XrefRangeStart = 90856, XrefRangeEnd = 90859, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static TransformData GetWorldTransformData(this Transform transform)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWorldTransformData_Public_Static_TransformData_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(TransformData*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 90864, RefRangeEnd = 90866, XrefRangeStart = 90861, XrefRangeEnd = 90864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static TransformData GetLocalTransformData(this Transform transform)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalTransformData_Public_Static_TransformData_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(TransformData*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90869, RefRangeEnd = 90870, XrefRangeStart = 90866, XrefRangeEnd = 90869, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetLocalTransformData(this Transform transform, TransformData data, bool setScale = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		*(TransformData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &data;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &setScale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLocalTransformData_Public_Static_Void_Transform_TransformData_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 90872, RefRangeEnd = 90874, XrefRangeStart = 90870, XrefRangeEnd = 90872, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetWorldTransformData(this Transform transform, TransformData data)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		*(TransformData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &data;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetWorldTransformData_Public_Static_Void_Transform_TransformData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TransformExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
