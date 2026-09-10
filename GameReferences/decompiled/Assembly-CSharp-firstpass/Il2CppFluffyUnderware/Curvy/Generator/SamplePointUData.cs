using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[StructLayout(LayoutKind.Explicit)]
public struct SamplePointUData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Vertex;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVEdge;

	private static readonly System.IntPtr NativeFieldInfoPtr_HardEdge;

	private static readonly System.IntPtr NativeFieldInfoPtr_FirstU;

	private static readonly System.IntPtr NativeFieldInfoPtr_SecondU;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Boolean_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_ControlPointOption_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SamplePointUData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_SamplePointUData_SamplePointUData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_SamplePointUData_SamplePointUData_0;

	[FieldOffset(0)]
	public int Vertex;

	[FieldOffset(4)]
	[MarshalAs(UnmanagedType.U1)]
	public bool UVEdge;

	[FieldOffset(5)]
	[MarshalAs(UnmanagedType.U1)]
	public bool HardEdge;

	[FieldOffset(8)]
	public float FirstU;

	[FieldOffset(12)]
	public float SecondU;

	static SamplePointUData()
	{
		Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "SamplePointUData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr);
		NativeFieldInfoPtr_Vertex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, "Vertex");
		NativeFieldInfoPtr_UVEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, "UVEdge");
		NativeFieldInfoPtr_HardEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, "HardEdge");
		NativeFieldInfoPtr_FirstU = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, "FirstU");
		NativeFieldInfoPtr_SecondU = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, "SecondU");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666493);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666494);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_ControlPointOption_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666495);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666496);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SamplePointUData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666497);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666498);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666499);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_SamplePointUData_SamplePointUData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666500);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_SamplePointUData_SamplePointUData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, 100666501);
	}

	[CallerCount(0)]
	public unsafe SamplePointUData(int vertexIndex, bool uvEdge, float firstU, float secondU)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&vertexIndex);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &uvEdge;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &firstU;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &secondU;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 54688, RefRangeEnd = 54690, XrefRangeStart = 54688, XrefRangeEnd = 54688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointUData(int vertexIndex, bool uvEdge, bool hardEdge, float firstU, float secondU)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&vertexIndex);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &uvEdge;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &hardEdge;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &firstU;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &secondU;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Boolean_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 54690, RefRangeEnd = 54693, XrefRangeStart = 54690, XrefRangeEnd = 54690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointUData(int vertexIndex, ControlPointOption controlPointsOption)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&vertexIndex);
		*(ControlPointOption**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointsOption;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_ControlPointOption_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54693, XrefRangeEnd = 54726, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54726, XrefRangeEnd = 54728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool Equals(SamplePointUData other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SamplePointUData_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54728, XrefRangeEnd = 54733, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Equals(Il2CppSystem.Object obj)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54733, XrefRangeEnd = 54740, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54740, XrefRangeEnd = 54742, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(SamplePointUData left, SamplePointUData right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(SamplePointUData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_SamplePointUData_SamplePointUData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54742, XrefRangeEnd = 54744, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator !=(SamplePointUData left, SamplePointUData right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(SamplePointUData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_SamplePointUData_SamplePointUData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SamplePointUData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
