using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne;

[StructLayout(LayoutKind.Explicit)]
public struct TransformData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Position;

	private static readonly System.IntPtr NativeFieldInfoPtr_Rotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_Scale;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Vector3_Quaternion_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToWorldTransform_Public_Void_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToLocalTransform_Public_Void_Transform_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToWorldTransform_Public_Void_Rigidbody_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyToLocalTransform_Public_Void_Rigidbody_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FromTransform_Public_Static_TransformData_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Lerp_Public_Static_TransformData_TransformData_TransformData_Single_0;

	[FieldOffset(0)]
	public Vector3 Position;

	[FieldOffset(12)]
	public Quaternion Rotation;

	[FieldOffset(28)]
	public Vector3 Scale;

	static TransformData()
	{
		Il2CppClassPointerStore<TransformData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "TransformData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformData>.NativeClassPtr);
		NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformData>.NativeClassPtr, "Position");
		NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformData>.NativeClassPtr, "Rotation");
		NativeFieldInfoPtr_Scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformData>.NativeClassPtr, "Scale");
		NativeMethodInfoPtr__ctor_Public_Void_Vector3_Quaternion_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665940);
		NativeMethodInfoPtr_ApplyToWorldTransform_Public_Void_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665941);
		NativeMethodInfoPtr_ApplyToLocalTransform_Public_Void_Transform_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665942);
		NativeMethodInfoPtr_ApplyToWorldTransform_Public_Void_Rigidbody_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665943);
		NativeMethodInfoPtr_ApplyToLocalTransform_Public_Void_Rigidbody_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665944);
		NativeMethodInfoPtr_FromTransform_Public_Static_TransformData_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665945);
		NativeMethodInfoPtr_Lerp_Public_Static_TransformData_TransformData_TransformData_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformData>.NativeClassPtr, 100665946);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 90881, RefRangeEnd = 90883, XrefRangeStart = 90881, XrefRangeEnd = 90881, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&position);
		*(Quaternion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Vector3_Quaternion_Vector3_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90883, XrefRangeEnd = 90885, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyToWorldTransform(Transform transform)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyToWorldTransform_Public_Void_Transform_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90885, XrefRangeEnd = 90888, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyToLocalTransform(Transform transform, bool setScale = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &setScale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyToLocalTransform_Public_Void_Transform_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90888, XrefRangeEnd = 90890, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyToWorldTransform(Rigidbody rb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyToWorldTransform_Public_Void_Rigidbody_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90896, RefRangeEnd = 90897, XrefRangeStart = 90890, XrefRangeEnd = 90896, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyToLocalTransform(Rigidbody rb, bool setScale = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rb);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &setScale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyToLocalTransform_Public_Void_Rigidbody_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90897, XrefRangeEnd = 90900, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static TransformData FromTransform(Transform transform)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FromTransform_Public_Static_TransformData_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(TransformData*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90904, RefRangeEnd = 90905, XrefRangeStart = 90900, XrefRangeEnd = 90904, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static TransformData Lerp(TransformData a, TransformData b, float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(TransformData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Lerp_Public_Static_TransformData_TransformData_TransformData_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(TransformData*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<TransformData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
