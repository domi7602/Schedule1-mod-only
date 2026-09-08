using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppToolBuddy.Pooling.Pools;

[StructLayout(LayoutKind.Explicit)]
public struct ArrayPoolUsageData
{
	private static readonly System.IntPtr NativeFieldInfoPtr__ElementsCount_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__ArraysCount_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__ElementsCapacity_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ElementsCount_Public_get_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ArraysCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ElementsCapacity_Public_get_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int64_Int32_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Boolean_ArrayPoolUsageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ArrayPoolUsageData_ArrayPoolUsageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ArrayPoolUsageData_ArrayPoolUsageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	[FieldOffset(0)]
	public readonly long _ElementsCount_k__BackingField;

	[FieldOffset(8)]
	public readonly int _ArraysCount_k__BackingField;

	[FieldOffset(16)]
	public readonly long _ElementsCapacity_k__BackingField;

	public unsafe long ElementsCount
	{
		[CallerCount(155)]
		[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ElementsCount_Public_get_Int64_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(long*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int ArraysCount
	{
		[CallerCount(28)]
		[CachedScanResults(RefRangeStart = 29148, RefRangeEnd = 29176, XrefRangeStart = 29148, XrefRangeEnd = 29176, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ArraysCount_Public_get_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe long ElementsCapacity
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ElementsCapacity_Public_get_Int64_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(long*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static ArrayPoolUsageData()
	{
		Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.Pooling.Pools", "ArrayPoolUsageData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr);
		NativeFieldInfoPtr__ElementsCount_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, "<ElementsCount>k__BackingField");
		NativeFieldInfoPtr__ArraysCount_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, "<ArraysCount>k__BackingField");
		NativeFieldInfoPtr__ElementsCapacity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, "<ElementsCapacity>k__BackingField");
		NativeMethodInfoPtr_get_ElementsCount_Public_get_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663812);
		NativeMethodInfoPtr_get_ArraysCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663813);
		NativeMethodInfoPtr_get_ElementsCapacity_Public_get_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663814);
		NativeMethodInfoPtr__ctor_Public_Void_Int64_Int32_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663815);
		NativeMethodInfoPtr_Equals_Public_Boolean_ArrayPoolUsageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663816);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663817);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663818);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ArrayPoolUsageData_ArrayPoolUsageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663819);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ArrayPoolUsageData_ArrayPoolUsageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663820);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, 100663821);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29609, RefRangeEnd = 29610, XrefRangeStart = 29609, XrefRangeEnd = 29609, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ArrayPoolUsageData(long elementsCount, int arraysCount, long elementsCapacity)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&elementsCount);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &arraysCount;
		*(long**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementsCapacity;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int64_Int32_Int64_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool Equals(ArrayPoolUsageData other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Boolean_ArrayPoolUsageData_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29610, XrefRangeEnd = 29613, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29613, XrefRangeEnd = 29615, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator ==(ArrayPoolUsageData a, ArrayPoolUsageData b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(ArrayPoolUsageData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_ArrayPoolUsageData_ArrayPoolUsageData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool operator !=(ArrayPoolUsageData a, ArrayPoolUsageData b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(ArrayPoolUsageData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_ArrayPoolUsageData_ArrayPoolUsageData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29615, XrefRangeEnd = 29650, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ArrayPoolUsageData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
