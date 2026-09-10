using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

[StructLayout(LayoutKind.Explicit)]
public struct ApproximateVector3
{
	private static readonly System.IntPtr NativeFieldInfoPtr_X;

	private static readonly System.IntPtr NativeFieldInfoPtr_Y;

	private static readonly System.IntPtr NativeFieldInfoPtr_Z;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToVector3_Public_Vector3_0;

	[FieldOffset(0)]
	public short X;

	[FieldOffset(2)]
	public short Y;

	[FieldOffset(4)]
	public short Z;

	static ApproximateVector3()
	{
		Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "ApproximateVector3");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr);
		NativeFieldInfoPtr_X = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, "X");
		NativeFieldInfoPtr_Y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, "Y");
		NativeFieldInfoPtr_Z = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, "Z");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, 100677896);
		NativeMethodInfoPtr__ctor_Public_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, 100677897);
		NativeMethodInfoPtr_ToVector3_Public_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, 100677898);
	}

	[CallerCount(0)]
	public unsafe ApproximateVector3(float x, float y, float z)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&x);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &z;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe ApproximateVector3(Vector3 vector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&vector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Vector3_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe Vector3 ToVector3()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToVector3_Public_Vector3_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ApproximateVector3>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
