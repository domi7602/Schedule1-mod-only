using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Graffiti;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct UShort2
{
	private static readonly System.IntPtr NativeFieldInfoPtr_X;

	private static readonly System.IntPtr NativeFieldInfoPtr_Y;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_UInt16_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Addition_Public_Static_UShort2_UShort2_UShort2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Subtraction_Public_Static_UShort2_UShort2_UShort2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Implicit_Public_Static_Vector2_UShort2_0;

	[FieldOffset(0)]
	public ushort X;

	[FieldOffset(2)]
	public ushort Y;

	static UShort2()
	{
		Il2CppClassPointerStore<UShort2>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Graffiti", "UShort2");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UShort2>.NativeClassPtr);
		NativeFieldInfoPtr_X = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UShort2>.NativeClassPtr, "X");
		NativeFieldInfoPtr_Y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UShort2>.NativeClassPtr, "Y");
		NativeMethodInfoPtr__ctor_Public_Void_UInt16_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UShort2>.NativeClassPtr, 100672925);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UShort2>.NativeClassPtr, 100672926);
		NativeMethodInfoPtr_op_Addition_Public_Static_UShort2_UShort2_UShort2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UShort2>.NativeClassPtr, 100672927);
		NativeMethodInfoPtr_op_Subtraction_Public_Static_UShort2_UShort2_UShort2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UShort2>.NativeClassPtr, 100672928);
		NativeMethodInfoPtr_op_Implicit_Public_Static_Vector2_UShort2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UShort2>.NativeClassPtr, 100672929);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 171375, RefRangeEnd = 171384, XrefRangeStart = 171375, XrefRangeEnd = 171375, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UShort2(ushort x, ushort y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&x);
		*(ushort**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_UInt16_UInt16_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 171384, XrefRangeEnd = 171392, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	public unsafe static UShort2 operator +(UShort2 a, UShort2 b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(UShort2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Addition_Public_Static_UShort2_UShort2_UShort2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(UShort2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static UShort2 operator -(UShort2 a, UShort2 b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(UShort2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Subtraction_Public_Static_UShort2_UShort2_UShort2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(UShort2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static implicit operator Vector2(UShort2 uShort2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&uShort2);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Implicit_Public_Static_Vector2_UShort2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<UShort2>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
