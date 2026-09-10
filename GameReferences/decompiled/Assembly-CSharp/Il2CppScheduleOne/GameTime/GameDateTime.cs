using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppSystem;

namespace Il2CppScheduleOne.GameTime;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct GameDateTime
{
	private static readonly System.IntPtr NativeFieldInfoPtr_elapsedDays;

	private static readonly System.IntPtr NativeFieldInfoPtr_time;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_GameDateTimeData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMinSum_Public_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddMins_Public_GameDateTime_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_GameDateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Addition_Public_Static_GameDateTime_GameDateTime_GameDateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Subtraction_Public_Static_GameDateTime_GameDateTime_GameDateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_GreaterThan_Public_Static_Boolean_GameDateTime_GameDateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_GreaterThanOrEqual_Public_Static_Boolean_GameDateTime_GameDateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_LessThan_Public_Static_Boolean_GameDateTime_GameDateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_LessThanOrEqual_Public_Static_Boolean_GameDateTime_GameDateTime_0;

	[FieldOffset(0)]
	public int elapsedDays;

	[FieldOffset(4)]
	public int time;

	static GameDateTime()
	{
		Il2CppClassPointerStore<GameDateTime>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.GameTime", "GameDateTime");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr);
		NativeFieldInfoPtr_elapsedDays = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, "elapsedDays");
		NativeFieldInfoPtr_time = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, "time");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666707);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666708);
		NativeMethodInfoPtr__ctor_Public_Void_GameDateTimeData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666709);
		NativeMethodInfoPtr_GetMinSum_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666710);
		NativeMethodInfoPtr_AddMins_Public_GameDateTime_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666711);
		NativeMethodInfoPtr_GetCopy_Public_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666712);
		NativeMethodInfoPtr_op_Addition_Public_Static_GameDateTime_GameDateTime_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666713);
		NativeMethodInfoPtr_op_Subtraction_Public_Static_GameDateTime_GameDateTime_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666714);
		NativeMethodInfoPtr_op_GreaterThan_Public_Static_Boolean_GameDateTime_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666715);
		NativeMethodInfoPtr_op_GreaterThanOrEqual_Public_Static_Boolean_GameDateTime_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666716);
		NativeMethodInfoPtr_op_LessThan_Public_Static_Boolean_GameDateTime_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666717);
		NativeMethodInfoPtr_op_LessThanOrEqual_Public_Static_Boolean_GameDateTime_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, 100666718);
	}

	[CallerCount(494)]
	[CachedScanResults(RefRangeStart = 60743, RefRangeEnd = 61237, XrefRangeStart = 60743, XrefRangeEnd = 61237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTime(int _elapsedDays, int _time)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&_elapsedDays);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &_time;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 99228, RefRangeEnd = 99231, XrefRangeStart = 99224, XrefRangeEnd = 99228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTime(int _minSum)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&_minSum);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 99231, RefRangeEnd = 99235, XrefRangeStart = 99231, XrefRangeEnd = 99231, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTime(GameDateTimeData data)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_GameDateTimeData_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 99238, RefRangeEnd = 99251, XrefRangeStart = 99235, XrefRangeEnd = 99238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetMinSum()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMinSum_Public_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 99251, XrefRangeEnd = 99255, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTime AddMins(int mins)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mins);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddMins_Public_GameDateTime_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(GameDateTime*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(155)]
	[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTime GetCopy()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_GameDateTime_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(GameDateTime*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 99255, XrefRangeEnd = 99262, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static GameDateTime operator +(GameDateTime a, GameDateTime b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(GameDateTime**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Addition_Public_Static_GameDateTime_GameDateTime_GameDateTime_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(GameDateTime*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 99269, RefRangeEnd = 99271, XrefRangeStart = 99262, XrefRangeEnd = 99269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static GameDateTime operator -(GameDateTime a, GameDateTime b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(GameDateTime**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Subtraction_Public_Static_GameDateTime_GameDateTime_GameDateTime_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(GameDateTime*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 99271, XrefRangeEnd = 99277, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator >(GameDateTime a, GameDateTime b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(GameDateTime**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_GreaterThan_Public_Static_Boolean_GameDateTime_GameDateTime_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 99277, XrefRangeEnd = 99283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator >=(GameDateTime a, GameDateTime b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(GameDateTime**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_GreaterThanOrEqual_Public_Static_Boolean_GameDateTime_GameDateTime_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 99283, XrefRangeEnd = 99289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator <(GameDateTime a, GameDateTime b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(GameDateTime**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_LessThan_Public_Static_Boolean_GameDateTime_GameDateTime_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 99289, XrefRangeEnd = 99295, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator <=(GameDateTime a, GameDateTime b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(GameDateTime**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_LessThanOrEqual_Public_Static_Boolean_GameDateTime_GameDateTime_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<GameDateTime>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
