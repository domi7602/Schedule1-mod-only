using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[StructLayout(LayoutKind.Explicit)]
public struct ControlPointOption
{
	private static readonly System.IntPtr NativeFieldInfoPtr_TF;

	private static readonly System.IntPtr NativeFieldInfoPtr_Distance;

	private static readonly System.IntPtr NativeFieldInfoPtr_Include;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaterialID;

	private static readonly System.IntPtr NativeFieldInfoPtr_HardEdge;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxStepDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVEdge;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVShift;

	private static readonly System.IntPtr NativeFieldInfoPtr_FirstU;

	private static readonly System.IntPtr NativeFieldInfoPtr_SecondU;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Boolean_Int32_Boolean_Single_Boolean_Boolean_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_ControlPointOption_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ControlPointOption_ControlPointOption_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ControlPointOption_ControlPointOption_0;

	[FieldOffset(0)]
	public float TF;

	[FieldOffset(4)]
	public float Distance;

	[FieldOffset(8)]
	[MarshalAs(UnmanagedType.U1)]
	public bool Include;

	[FieldOffset(12)]
	public int MaterialID;

	[FieldOffset(16)]
	[MarshalAs(UnmanagedType.U1)]
	public bool HardEdge;

	[FieldOffset(20)]
	public float MaxStepDistance;

	[FieldOffset(24)]
	[MarshalAs(UnmanagedType.U1)]
	public bool UVEdge;

	[FieldOffset(25)]
	[MarshalAs(UnmanagedType.U1)]
	public bool UVShift;

	[FieldOffset(28)]
	public float FirstU;

	[FieldOffset(32)]
	public float SecondU;

	static ControlPointOption()
	{
		Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "ControlPointOption");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr);
		NativeFieldInfoPtr_TF = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "TF");
		NativeFieldInfoPtr_Distance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "Distance");
		NativeFieldInfoPtr_Include = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "Include");
		NativeFieldInfoPtr_MaterialID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "MaterialID");
		NativeFieldInfoPtr_HardEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "HardEdge");
		NativeFieldInfoPtr_MaxStepDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "MaxStepDistance");
		NativeFieldInfoPtr_UVEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "UVEdge");
		NativeFieldInfoPtr_UVShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "UVShift");
		NativeFieldInfoPtr_FirstU = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "FirstU");
		NativeFieldInfoPtr_SecondU = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, "SecondU");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Boolean_Int32_Boolean_Single_Boolean_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, 100666321);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_ControlPointOption_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, 100666322);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, 100666323);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, 100666324);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ControlPointOption_ControlPointOption_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, 100666325);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ControlPointOption_ControlPointOption_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, 100666326);
	}

	[CallerCount(0)]
	public unsafe ControlPointOption(float tf, float dist, bool includeAnyways, int materialID, bool hardEdge, float maxStepDistance, bool uvEdge, bool uvShift, float firstU, float secondU)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[10];
		*ptr = (nint)(&tf);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dist;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeAnyways;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &materialID;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &hardEdge;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxStepDistance;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &uvEdge;
		*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &uvShift;
		*(float**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &firstU;
		*(float**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &secondU;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Boolean_Int32_Boolean_Single_Boolean_Boolean_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 52909, RefRangeEnd = 52912, XrefRangeStart = 52904, XrefRangeEnd = 52909, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool Equals(ControlPointOption other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_ControlPointOption_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52912, XrefRangeEnd = 52916, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52916, XrefRangeEnd = 52928, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52928, XrefRangeEnd = 52929, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(ControlPointOption left, ControlPointOption right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(ControlPointOption**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ControlPointOption_ControlPointOption_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52929, XrefRangeEnd = 52930, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator !=(ControlPointOption left, ControlPointOption right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(ControlPointOption**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ControlPointOption_ControlPointOption_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ControlPointOption>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
