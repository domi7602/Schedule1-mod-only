using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[StructLayout(LayoutKind.Explicit)]
public struct DuplicateSamplePoint
{
	private static readonly System.IntPtr NativeFieldInfoPtr__StartIndex_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__EndIndex_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__IsHardEdge_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartIndex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndIndex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsHardEdge_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_DuplicateSamplePoint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_DuplicateSamplePoint_DuplicateSamplePoint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_DuplicateSamplePoint_DuplicateSamplePoint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	[FieldOffset(0)]
	public readonly int _StartIndex_k__BackingField;

	[FieldOffset(4)]
	public readonly int _EndIndex_k__BackingField;

	[FieldOffset(8)]
	[MarshalAs(UnmanagedType.U1)]
	public readonly bool _IsHardEdge_k__BackingField;

	public unsafe int StartIndex
	{
		[CallerCount(501)]
		[CachedScanResults(RefRangeStart = 40619, RefRangeEnd = 41120, XrefRangeStart = 40619, XrefRangeEnd = 41120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartIndex_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int EndIndex
	{
		[CallerCount(120)]
		[CachedScanResults(RefRangeStart = 54296, RefRangeEnd = 54416, XrefRangeStart = 54296, XrefRangeEnd = 54296, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndIndex_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsHardEdge
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsHardEdge_Public_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static DuplicateSamplePoint()
	{
		Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "DuplicateSamplePoint");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr);
		NativeFieldInfoPtr__StartIndex_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, "<StartIndex>k__BackingField");
		NativeFieldInfoPtr__EndIndex_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, "<EndIndex>k__BackingField");
		NativeFieldInfoPtr__IsHardEdge_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, "<IsHardEdge>k__BackingField");
		NativeMethodInfoPtr_get_StartIndex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666442);
		NativeMethodInfoPtr_get_EndIndex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666443);
		NativeMethodInfoPtr_get_IsHardEdge_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666444);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666445);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_DuplicateSamplePoint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666446);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666447);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666448);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_DuplicateSamplePoint_DuplicateSamplePoint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666449);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_DuplicateSamplePoint_DuplicateSamplePoint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666450);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, 100666451);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 54416, RefRangeEnd = 54419, XrefRangeStart = 54416, XrefRangeEnd = 54416, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DuplicateSamplePoint(int startIndex, int endIndex, bool isHardEdge)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&startIndex);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &endIndex;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &isHardEdge;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual bool Equals(DuplicateSamplePoint other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_DuplicateSamplePoint_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54419, XrefRangeEnd = 54422, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54422, XrefRangeEnd = 54426, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator ==(DuplicateSamplePoint left, DuplicateSamplePoint right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(DuplicateSamplePoint**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_DuplicateSamplePoint_DuplicateSamplePoint_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator !=(DuplicateSamplePoint left, DuplicateSamplePoint right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(DuplicateSamplePoint**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_DuplicateSamplePoint_DuplicateSamplePoint_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54426, XrefRangeEnd = 54461, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<DuplicateSamplePoint>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
