using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

[StructLayout(LayoutKind.Explicit)]
public struct Vec3
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Zero;

	private static readonly System.IntPtr NativeFieldInfoPtr_X;

	private static readonly System.IntPtr NativeFieldInfoPtr_Y;

	private static readonly System.IntPtr NativeFieldInfoPtr_Z;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Sub_Public_Static_Void_byref_Vec3_byref_Vec3_byref_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Neg_Public_Static_Void_byref_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dot_Public_Static_Void_byref_Vec3_byref_Vec3_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Normalize_Public_Static_Void_byref_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LongAxis_Public_Static_Int32_byref_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	[FieldOffset(0)]
	public float X;

	[FieldOffset(4)]
	public float Y;

	[FieldOffset(8)]
	public float Z;

	public unsafe static Vec3 Zero
	{
		get
		{
			Unsafe.SkipInit(out Vec3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Zero, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Zero, (void*)(&vec));
		}
	}

	public unsafe float this[int index]
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&index);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&index);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Vec3()
	{
		Il2CppClassPointerStore<Vec3>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "Vec3");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Vec3>.NativeClassPtr);
		NativeFieldInfoPtr_Zero = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vec3>.NativeClassPtr, "Zero");
		NativeFieldInfoPtr_X = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vec3>.NativeClassPtr, "X");
		NativeFieldInfoPtr_Y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vec3>.NativeClassPtr, "Y");
		NativeFieldInfoPtr_Z = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vec3>.NativeClassPtr, "Z");
		NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667111);
		NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667112);
		NativeMethodInfoPtr_Sub_Public_Static_Void_byref_Vec3_byref_Vec3_byref_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667113);
		NativeMethodInfoPtr_Neg_Public_Static_Void_byref_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667114);
		NativeMethodInfoPtr_Dot_Public_Static_Void_byref_Vec3_byref_Vec3_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667115);
		NativeMethodInfoPtr_Normalize_Public_Static_Void_byref_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667116);
		NativeMethodInfoPtr_LongAxis_Public_Static_Int32_byref_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667117);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vec3>.NativeClassPtr, 100667118);
	}

	[CallerCount(0)]
	public unsafe static void Sub(ref Vec3 lhs, ref Vec3 rhs, out Vec3 result)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)Unsafe.AsPointer(ref lhs);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref rhs);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Sub_Public_Static_Void_byref_Vec3_byref_Vec3_byref_Vec3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static void Neg(ref Vec3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Neg_Public_Static_Void_byref_Vec3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static void Dot(ref Vec3 u, ref Vec3 v, out float dot)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)Unsafe.AsPointer(ref u);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref v);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dot_Public_Static_Void_byref_Vec3_byref_Vec3_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59553, XrefRangeEnd = 59558, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Normalize(ref Vec3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Normalize_Public_Static_Void_byref_Vec3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 59565, RefRangeEnd = 59567, XrefRangeStart = 59558, XrefRangeEnd = 59565, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int LongAxis(ref Vec3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LongAxis_Public_Static_Int32_byref_Vec3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59567, XrefRangeEnd = 59577, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Vec3>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
