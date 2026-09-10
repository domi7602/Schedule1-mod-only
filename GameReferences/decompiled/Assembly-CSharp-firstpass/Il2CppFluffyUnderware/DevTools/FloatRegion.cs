using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct FloatRegion
{
	private static readonly System.IntPtr NativeFieldInfoPtr_From;

	private static readonly System.IntPtr NativeFieldInfoPtr_To;

	private static readonly System.IntPtr NativeFieldInfoPtr_SimpleValue;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ZeroOne_Public_Static_get_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakePositive_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clamp_Public_Void_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Positive_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Low_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Low_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_High_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_High_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Random_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Next_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Length_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LengthPositive_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Addition_Public_Static_FloatRegion_FloatRegion_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Subtraction_Public_Static_FloatRegion_FloatRegion_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_UnaryNegation_Public_Static_FloatRegion_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Multiply_Public_Static_FloatRegion_FloatRegion_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Multiply_Public_Static_FloatRegion_Single_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Division_Public_Static_FloatRegion_FloatRegion_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_FloatRegion_FloatRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_FloatRegion_FloatRegion_0;

	[FieldOffset(0)]
	public float From;

	[FieldOffset(4)]
	public float To;

	[FieldOffset(8)]
	[MarshalAs(UnmanagedType.U1)]
	public bool SimpleValue;

	public unsafe static FloatRegion ZeroOne
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 31988, RefRangeEnd = 31994, XrefRangeStart = 31988, XrefRangeEnd = 31988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ZeroOne_Public_Static_get_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool Positive
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Positive_Public_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float Low
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 31997, RefRangeEnd = 32001, XrefRangeStart = 31997, XrefRangeEnd = 31997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Low_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Low_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float High
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 32001, RefRangeEnd = 32004, XrefRangeStart = 32001, XrefRangeEnd = 32001, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_High_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_High_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Random
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32004, XrefRangeEnd = 32005, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Random_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float Next
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 32006, RefRangeEnd = 32011, XrefRangeStart = 32005, XrefRangeEnd = 32006, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Next_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float Length
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 32011, RefRangeEnd = 32013, XrefRangeStart = 32011, XrefRangeEnd = 32011, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Length_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float LengthPositive
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LengthPositive_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static FloatRegion()
	{
		Il2CppClassPointerStore<FloatRegion>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "FloatRegion");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr);
		NativeFieldInfoPtr_From = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, "From");
		NativeFieldInfoPtr_To = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, "To");
		NativeFieldInfoPtr_SimpleValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, "SimpleValue");
		NativeMethodInfoPtr__ctor_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664040);
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664041);
		NativeMethodInfoPtr_get_ZeroOne_Public_Static_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664042);
		NativeMethodInfoPtr_MakePositive_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664043);
		NativeMethodInfoPtr_Clamp_Public_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664044);
		NativeMethodInfoPtr_get_Positive_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664045);
		NativeMethodInfoPtr_get_Low_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664046);
		NativeMethodInfoPtr_set_Low_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664047);
		NativeMethodInfoPtr_get_High_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664048);
		NativeMethodInfoPtr_set_High_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664049);
		NativeMethodInfoPtr_get_Random_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664050);
		NativeMethodInfoPtr_get_Next_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664051);
		NativeMethodInfoPtr_get_Length_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664052);
		NativeMethodInfoPtr_get_LengthPositive_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664053);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664054);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664055);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664056);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664057);
		NativeMethodInfoPtr_op_Addition_Public_Static_FloatRegion_FloatRegion_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664058);
		NativeMethodInfoPtr_op_Subtraction_Public_Static_FloatRegion_FloatRegion_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664059);
		NativeMethodInfoPtr_op_UnaryNegation_Public_Static_FloatRegion_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664060);
		NativeMethodInfoPtr_op_Multiply_Public_Static_FloatRegion_FloatRegion_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664061);
		NativeMethodInfoPtr_op_Multiply_Public_Static_FloatRegion_Single_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664062);
		NativeMethodInfoPtr_op_Division_Public_Static_FloatRegion_FloatRegion_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664063);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_FloatRegion_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664064);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_FloatRegion_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, 100664065);
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 31960, RefRangeEnd = 31984, XrefRangeStart = 31960, XrefRangeEnd = 31960, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FloatRegion(float value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 31984, RefRangeEnd = 31988, XrefRangeStart = 31984, XrefRangeEnd = 31984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FloatRegion(float A, float B)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&A);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &B;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void MakePositive()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakePositive_Public_Void_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31994, XrefRangeEnd = 31997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clamp(float low, float high)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&low);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &high;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clamp_Public_Void_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32013, XrefRangeEnd = 32021, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32021, XrefRangeEnd = 32023, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32023, XrefRangeEnd = 32024, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool Equals(FloatRegion other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_FloatRegion_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32024, XrefRangeEnd = 32028, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Equals(Il2CppSystem.Object other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static FloatRegion operator +(FloatRegion a, FloatRegion b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FloatRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Addition_Public_Static_FloatRegion_FloatRegion_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static FloatRegion operator -(FloatRegion a, FloatRegion b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FloatRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Subtraction_Public_Static_FloatRegion_FloatRegion_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static FloatRegion operator -(FloatRegion a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&a);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_UnaryNegation_Public_Static_FloatRegion_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static FloatRegion operator *(FloatRegion a, float v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Multiply_Public_Static_FloatRegion_FloatRegion_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static FloatRegion operator *(float v, FloatRegion a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&v);
		*(FloatRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Multiply_Public_Static_FloatRegion_Single_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static FloatRegion operator /(FloatRegion a, float v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Division_Public_Static_FloatRegion_FloatRegion_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32028, XrefRangeEnd = 32030, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(FloatRegion lhs, FloatRegion rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(FloatRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_FloatRegion_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 32032, RefRangeEnd = 32034, XrefRangeStart = 32030, XrefRangeEnd = 32032, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator !=(FloatRegion lhs, FloatRegion rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(FloatRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_FloatRegion_FloatRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<FloatRegion>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
