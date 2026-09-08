using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

[StructLayout(LayoutKind.Explicit)]
public struct Matrix2D
{
	private static readonly System.IntPtr NativeFieldInfoPtr_m00;

	private static readonly System.IntPtr NativeFieldInfoPtr_m10;

	private static readonly System.IntPtr NativeFieldInfoPtr_m01;

	private static readonly System.IntPtr NativeFieldInfoPtr_m11;

	private static readonly System.IntPtr NativeFieldInfoPtr_m02;

	private static readonly System.IntPtr NativeFieldInfoPtr_m12;

	private static readonly System.IntPtr NativeFieldInfoPtr_zeroMatrix;

	private static readonly System.IntPtr NativeFieldInfoPtr_identityMatrix;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Multiply_Public_Static_Matrix2D_Matrix2D_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Multiply_Public_Static_Vector2_Matrix2D_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_Matrix2D_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_Matrix2D_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColumn_Public_Vector2_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRow_Public_Vector3_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetColumn_Public_Void_Int32_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRow_Public_Void_Int32_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MultiplyPoint_Public_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MultiplyVector_Public_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Inverse_Public_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Scale_Public_Static_Matrix2D_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Translate_Public_Static_Matrix2D_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RotateRH_Public_Static_Matrix2D_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RotateLH_Public_Static_Matrix2D_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkewX_Public_Static_Matrix2D_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkewY_Public_Static_Matrix2D_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_zero_Public_Static_get_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_identity_Public_Static_get_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_String_String_0;

	[FieldOffset(0)]
	public float m00;

	[FieldOffset(4)]
	public float m10;

	[FieldOffset(8)]
	public float m01;

	[FieldOffset(12)]
	public float m11;

	[FieldOffset(16)]
	public float m02;

	[FieldOffset(20)]
	public float m12;

	public unsafe static Matrix2D zeroMatrix
	{
		get
		{
			Unsafe.SkipInit(out Matrix2D result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_zeroMatrix, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_zeroMatrix, (void*)(&matrix2D));
		}
	}

	public unsafe static Matrix2D identityMatrix
	{
		get
		{
			Unsafe.SkipInit(out Matrix2D result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_identityMatrix, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_identityMatrix, (void*)(&matrix2D));
		}
	}

	public unsafe float this[int row, int column]
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19182, XrefRangeEnd = 19186, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&row);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &column;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19186, XrefRangeEnd = 19190, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&row);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &column;
			*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Int32_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float this[int index]
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 19190, RefRangeEnd = 19191, XrefRangeStart = 19190, XrefRangeEnd = 19190, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&index);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 19191, RefRangeEnd = 19197, XrefRangeStart = 19191, XrefRangeEnd = 19191, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe static Matrix2D zero
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19384, XrefRangeEnd = 19388, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_zero_Public_Static_get_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static Matrix2D identity
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19388, XrefRangeEnd = 19392, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_identity_Public_Static_get_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static Matrix2D()
	{
		Il2CppClassPointerStore<Matrix2D>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "Matrix2D");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr);
		NativeFieldInfoPtr_m00 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "m00");
		NativeFieldInfoPtr_m10 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "m10");
		NativeFieldInfoPtr_m01 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "m01");
		NativeFieldInfoPtr_m11 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "m11");
		NativeFieldInfoPtr_m02 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "m02");
		NativeFieldInfoPtr_m12 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "m12");
		NativeFieldInfoPtr_zeroMatrix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "zeroMatrix");
		NativeFieldInfoPtr_identityMatrix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, "identityMatrix");
		NativeMethodInfoPtr__ctor_Public_Void_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663329);
		NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663330);
		NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663331);
		NativeMethodInfoPtr_get_Item_Public_get_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663332);
		NativeMethodInfoPtr_set_Item_Public_set_Void_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663333);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663334);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663335);
		NativeMethodInfoPtr_op_Multiply_Public_Static_Matrix2D_Matrix2D_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663336);
		NativeMethodInfoPtr_op_Multiply_Public_Static_Vector2_Matrix2D_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663337);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_Matrix2D_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663338);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_Matrix2D_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663339);
		NativeMethodInfoPtr_GetColumn_Public_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663340);
		NativeMethodInfoPtr_GetRow_Public_Vector3_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663341);
		NativeMethodInfoPtr_SetColumn_Public_Void_Int32_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663342);
		NativeMethodInfoPtr_SetRow_Public_Void_Int32_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663343);
		NativeMethodInfoPtr_MultiplyPoint_Public_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663344);
		NativeMethodInfoPtr_MultiplyVector_Public_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663345);
		NativeMethodInfoPtr_Inverse_Public_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663346);
		NativeMethodInfoPtr_Scale_Public_Static_Matrix2D_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663347);
		NativeMethodInfoPtr_Translate_Public_Static_Matrix2D_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663348);
		NativeMethodInfoPtr_RotateRH_Public_Static_Matrix2D_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663349);
		NativeMethodInfoPtr_RotateLH_Public_Static_Matrix2D_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663350);
		NativeMethodInfoPtr_SkewX_Public_Static_Matrix2D_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663351);
		NativeMethodInfoPtr_SkewY_Public_Static_Matrix2D_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663352);
		NativeMethodInfoPtr_get_zero_Public_Static_get_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663353);
		NativeMethodInfoPtr_get_identity_Public_Static_get_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663354);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663355);
		NativeMethodInfoPtr_ToString_Public_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, 100663356);
	}

	[CallerCount(0)]
	public unsafe Matrix2D(Vector2 column0, Vector2 column1, Vector2 column2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&column0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &column1;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &column2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Vector2_Vector2_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19197, XrefRangeEnd = 19206, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19206, XrefRangeEnd = 19216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Equals(Il2CppSystem.Object other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 19216, RefRangeEnd = 19219, XrefRangeStart = 19216, XrefRangeEnd = 19216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D operator *(Matrix2D lhs, Matrix2D rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Multiply_Public_Static_Matrix2D_Matrix2D_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 19219, RefRangeEnd = 19242, XrefRangeStart = 19219, XrefRangeEnd = 19219, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 operator *(Matrix2D lhs, Vector2 vector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &vector;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Multiply_Public_Static_Vector2_Matrix2D_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19242, XrefRangeEnd = 19249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(Matrix2D lhs, Matrix2D rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_Matrix2D_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19249, XrefRangeEnd = 19260, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator !=(Matrix2D lhs, Matrix2D rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_Matrix2D_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe Vector2 GetColumn(int index)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColumn_Public_Vector2_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe Vector3 GetRow(int index)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRow_Public_Vector3_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19260, XrefRangeEnd = 19271, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetColumn(int index, Vector2 column)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&index);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &column;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetColumn_Public_Void_Int32_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19271, XrefRangeEnd = 19286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRow(int index, Vector3 row)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&index);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &row;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRow_Public_Void_Int32_Vector3_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 19219, RefRangeEnd = 19242, XrefRangeStart = 19219, XrefRangeEnd = 19242, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 MultiplyPoint(Vector2 point)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&point);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MultiplyPoint_Public_Vector2_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 19286, RefRangeEnd = 19290, XrefRangeStart = 19286, XrefRangeEnd = 19286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 MultiplyVector(Vector2 vector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&vector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MultiplyVector_Public_Vector2_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 19369, RefRangeEnd = 19371, XrefRangeStart = 19290, XrefRangeEnd = 19369, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Matrix2D Inverse()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Inverse_Public_Matrix2D_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Matrix2D Scale(Vector2 vector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&vector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Scale_Public_Static_Matrix2D_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Matrix2D Translate(Vector2 vector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&vector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Translate_Public_Static_Matrix2D_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19371, XrefRangeEnd = 19376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D RotateRH(float angleRadians)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&angleRadians);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RotateRH_Public_Static_Matrix2D_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 19378, RefRangeEnd = 19382, XrefRangeStart = 19376, XrefRangeEnd = 19378, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D RotateLH(float angleRadians)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&angleRadians);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RotateLH_Public_Static_Matrix2D_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19382, XrefRangeEnd = 19383, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D SkewX(float angleRadians)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&angleRadians);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkewX_Public_Static_Matrix2D_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19383, XrefRangeEnd = 19384, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Matrix2D SkewY(float angleRadians)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&angleRadians);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkewY_Public_Static_Matrix2D_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Matrix2D*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19392, XrefRangeEnd = 19423, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19423, XrefRangeEnd = 19461, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string ToString(string format)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(format);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_String_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Matrix2D>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
