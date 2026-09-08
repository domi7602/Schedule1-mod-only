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
public struct IntRegion
{
	private static readonly System.IntPtr NativeFieldInfoPtr_From;

	private static readonly System.IntPtr NativeFieldInfoPtr_To;

	private static readonly System.IntPtr NativeFieldInfoPtr_SimpleValue;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ZeroOne_Public_Static_get_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakePositive_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clamp_Public_Void_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Positive_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Low_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Low_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_High_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_High_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Random_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Length_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LengthPositive_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Addition_Public_Static_IntRegion_IntRegion_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Subtraction_Public_Static_IntRegion_IntRegion_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_UnaryNegation_Public_Static_IntRegion_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Multiply_Public_Static_IntRegion_IntRegion_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Multiply_Public_Static_IntRegion_Int32_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Division_Public_Static_IntRegion_IntRegion_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_IntRegion_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_IntRegion_IntRegion_0;

	[FieldOffset(0)]
	public int From;

	[FieldOffset(4)]
	public int To;

	[FieldOffset(8)]
	[MarshalAs(UnmanagedType.U1)]
	public bool SimpleValue;

	public unsafe static IntRegion ZeroOne
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ZeroOne_Public_Static_get_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
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

	public unsafe int Low
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 32047, RefRangeEnd = 32049, XrefRangeStart = 32047, XrefRangeEnd = 32047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Low_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Low_Public_set_Void_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int High
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_High_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_High_Public_set_Void_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int Random
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32049, XrefRangeEnd = 32050, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Random_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int Length
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 32050, RefRangeEnd = 32051, XrefRangeStart = 32050, XrefRangeEnd = 32050, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Length_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int LengthPositive
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 32051, RefRangeEnd = 32053, XrefRangeStart = 32051, XrefRangeEnd = 32051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LengthPositive_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static IntRegion()
	{
		Il2CppClassPointerStore<IntRegion>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "IntRegion");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IntRegion>.NativeClassPtr);
		NativeFieldInfoPtr_From = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, "From");
		NativeFieldInfoPtr_To = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, "To");
		NativeFieldInfoPtr_SimpleValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, "SimpleValue");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664066);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664067);
		NativeMethodInfoPtr_get_ZeroOne_Public_Static_get_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664068);
		NativeMethodInfoPtr_MakePositive_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664069);
		NativeMethodInfoPtr_Clamp_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664070);
		NativeMethodInfoPtr_get_Positive_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664071);
		NativeMethodInfoPtr_get_Low_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664072);
		NativeMethodInfoPtr_set_Low_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664073);
		NativeMethodInfoPtr_get_High_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664074);
		NativeMethodInfoPtr_set_High_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664075);
		NativeMethodInfoPtr_get_Random_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664076);
		NativeMethodInfoPtr_get_Length_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664077);
		NativeMethodInfoPtr_get_LengthPositive_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664078);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664079);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664080);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664081);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664082);
		NativeMethodInfoPtr_op_Addition_Public_Static_IntRegion_IntRegion_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664083);
		NativeMethodInfoPtr_op_Subtraction_Public_Static_IntRegion_IntRegion_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664084);
		NativeMethodInfoPtr_op_UnaryNegation_Public_Static_IntRegion_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664085);
		NativeMethodInfoPtr_op_Multiply_Public_Static_IntRegion_IntRegion_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664086);
		NativeMethodInfoPtr_op_Multiply_Public_Static_IntRegion_Int32_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664087);
		NativeMethodInfoPtr_op_Division_Public_Static_IntRegion_IntRegion_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664088);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_IntRegion_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664089);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_IntRegion_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, 100664090);
	}

	[CallerCount(0)]
	public unsafe IntRegion(int value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 32034, RefRangeEnd = 32040, XrefRangeStart = 32034, XrefRangeEnd = 32034, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IntRegion(int A, int B)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&A);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &B;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 32040, RefRangeEnd = 32042, XrefRangeStart = 32040, XrefRangeEnd = 32040, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MakePositive()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakePositive_Public_Void_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 32045, RefRangeEnd = 32047, XrefRangeStart = 32042, XrefRangeEnd = 32045, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clamp(int low, int high)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&low);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &high;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clamp_Public_Void_Int32_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32053, XrefRangeEnd = 32061, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32061, XrefRangeEnd = 32063, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32063, XrefRangeEnd = 32064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool Equals(IntRegion other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_IntRegion_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32064, XrefRangeEnd = 32069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	public unsafe static IntRegion operator +(IntRegion a, IntRegion b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(IntRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Addition_Public_Static_IntRegion_IntRegion_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static IntRegion operator -(IntRegion a, IntRegion b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(IntRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Subtraction_Public_Static_IntRegion_IntRegion_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static IntRegion operator -(IntRegion a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&a);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_UnaryNegation_Public_Static_IntRegion_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static IntRegion operator *(IntRegion a, int v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Multiply_Public_Static_IntRegion_IntRegion_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static IntRegion operator *(int v, IntRegion a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&v);
		*(IntRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Multiply_Public_Static_IntRegion_Int32_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static IntRegion operator /(IntRegion a, int v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Division_Public_Static_IntRegion_IntRegion_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator ==(IntRegion lhs, IntRegion rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(IntRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_IntRegion_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator !=(IntRegion lhs, IntRegion rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(IntRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_IntRegion_IntRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<IntRegion>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
