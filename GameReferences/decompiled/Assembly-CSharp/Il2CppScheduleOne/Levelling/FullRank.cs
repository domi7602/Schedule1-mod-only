using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Levelling;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct FullRank
{
	private static readonly System.IntPtr NativeFieldInfoPtr_TIER_COUNT;

	private static readonly System.IntPtr NativeFieldInfoPtr_Rank;

	private static readonly System.IntPtr NativeFieldInfoPtr_Tier;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_ERank_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextRank_Public_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToFloat_Public_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRankIndex_Public_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetString_Public_Static_String_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_GreaterThan_Public_Static_Boolean_FullRank_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_LessThan_Public_Static_Boolean_FullRank_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_LessThanOrEqual_Public_Static_Boolean_FullRank_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_GreaterThanOrEqual_Public_Static_Boolean_FullRank_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_FullRank_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_FullRank_FullRank_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CompareTo_Public_Int32_FullRank_0;

	[FieldOffset(0)]
	public ERank Rank;

	[FieldOffset(4)]
	public int Tier;

	public unsafe static int TIER_COUNT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TIER_COUNT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TIER_COUNT, (void*)(&num));
		}
	}

	static FullRank()
	{
		Il2CppClassPointerStore<FullRank>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Levelling", "FullRank");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FullRank>.NativeClassPtr);
		NativeFieldInfoPtr_TIER_COUNT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FullRank>.NativeClassPtr, "TIER_COUNT");
		NativeFieldInfoPtr_Rank = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FullRank>.NativeClassPtr, "Rank");
		NativeFieldInfoPtr_Tier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FullRank>.NativeClassPtr, "Tier");
		NativeMethodInfoPtr__ctor_Public_Void_ERank_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671047);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671048);
		NativeMethodInfoPtr_NextRank_Public_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671049);
		NativeMethodInfoPtr_ToFloat_Public_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671050);
		NativeMethodInfoPtr_GetRankIndex_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671051);
		NativeMethodInfoPtr_GetString_Public_Static_String_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671052);
		NativeMethodInfoPtr_op_GreaterThan_Public_Static_Boolean_FullRank_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671053);
		NativeMethodInfoPtr_op_LessThan_Public_Static_Boolean_FullRank_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671054);
		NativeMethodInfoPtr_op_LessThanOrEqual_Public_Static_Boolean_FullRank_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671055);
		NativeMethodInfoPtr_op_GreaterThanOrEqual_Public_Static_Boolean_FullRank_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671056);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_FullRank_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671057);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_FullRank_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671058);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671059);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671060);
		NativeMethodInfoPtr_CompareTo_Public_Int32_FullRank_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FullRank>.NativeClassPtr, 100671061);
	}

	[CallerCount(494)]
	[CachedScanResults(RefRangeStart = 60743, RefRangeEnd = 61237, XrefRangeStart = 60743, XrefRangeEnd = 61237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FullRank(ERank rank, int tier)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rank);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tier;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ERank_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 151411, RefRangeEnd = 151419, XrefRangeStart = 151395, XrefRangeEnd = 151411, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 151419, RefRangeEnd = 151421, XrefRangeStart = 151419, XrefRangeEnd = 151419, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FullRank NextRank()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextRank_Public_FullRank_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(FullRank*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 151421, RefRangeEnd = 151426, XrefRangeStart = 151421, XrefRangeEnd = 151421, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float ToFloat()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToFloat_Public_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 151426, RefRangeEnd = 151427, XrefRangeStart = 151426, XrefRangeEnd = 151426, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetRankIndex()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRankIndex_Public_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 151443, RefRangeEnd = 151446, XrefRangeStart = 151427, XrefRangeEnd = 151443, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetString(FullRank rank)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&rank);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetString_Public_Static_String_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 151446, RefRangeEnd = 151447, XrefRangeStart = 151446, XrefRangeEnd = 151446, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator >(FullRank a, FullRank b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FullRank**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_GreaterThan_Public_Static_Boolean_FullRank_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 151447, RefRangeEnd = 151448, XrefRangeStart = 151447, XrefRangeEnd = 151447, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator <(FullRank a, FullRank b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FullRank**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_LessThan_Public_Static_Boolean_FullRank_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator <=(FullRank a, FullRank b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FullRank**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_LessThanOrEqual_Public_Static_Boolean_FullRank_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 151448, RefRangeEnd = 151452, XrefRangeStart = 151448, XrefRangeEnd = 151448, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator >=(FullRank a, FullRank b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FullRank**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_GreaterThanOrEqual_Public_Static_Boolean_FullRank_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 54644, RefRangeEnd = 54657, XrefRangeStart = 54644, XrefRangeEnd = 54657, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(FullRank a, FullRank b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FullRank**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_FullRank_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator !=(FullRank a, FullRank b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(FullRank**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_FullRank_FullRank_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 151452, XrefRangeEnd = 151455, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 151455, XrefRangeEnd = 151459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 151459, RefRangeEnd = 151461, XrefRangeStart = 151459, XrefRangeEnd = 151459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int CompareTo(FullRank other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CompareTo_Public_Int32_FullRank_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<FullRank>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
