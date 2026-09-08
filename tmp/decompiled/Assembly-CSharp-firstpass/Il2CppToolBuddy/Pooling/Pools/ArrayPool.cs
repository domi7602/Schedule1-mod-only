using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppToolBuddy.Pooling.Collections;

namespace Il2CppToolBuddy.Pooling.Pools;

public class ArrayPool<T> : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_emptySubArray;

	private static readonly System.IntPtr NativeFieldInfoPtr_random;

	private static readonly System.IntPtr NativeFieldInfoPtr_keysInitialCapacity;

	private static readonly System.IntPtr NativeFieldInfoPtr_poolKeys;

	private static readonly System.IntPtr NativeFieldInfoPtr_poolValues;

	private static readonly System.IntPtr NativeFieldInfoPtr_arraysCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_elementsCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_elementsCapacity;

	private static readonly System.IntPtr NativeFieldInfoPtr__LogAllocations_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ElementsCapacity_Public_get_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ElementsCapacity_Public_set_Void_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LogAllocations_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_LogAllocations_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UsageData_Public_get_ArrayPoolUsageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Allocate_Public_SubArray_1_T_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AllocateExactSize_Public_SubArray_1_T_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Free_Public_Void_SubArray_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Free_Public_Void_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Resize_Public_Void_byref_SubArray_1_T_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResizeAndClear_Public_Void_byref_SubArray_1_T_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResizeCopyless_Public_Void_byref_SubArray_1_T_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_SubArray_1_T_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_SubArray_1_T_SubArray_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Allocate_Private_SubArray_1_T_Int32_Boolean_Boolean_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyCapacity_Private_Void_Int64_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveElementAt_Private_Il2CppArrayBase_1_T_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BinarySearch_Private_Static_Int32_Il2CppStructArray_1_Int32_Int32_Int32_0;

	public unsafe SubArray<T> emptySubArray
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_emptySubArray);
			return new SubArray<T>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<T>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_emptySubArray), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<T>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe Il2CppSystem.Random random
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_random);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Random>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_random)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)random));
		}
	}

	public unsafe static int keysInitialCapacity
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_keysInitialCapacity, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_keysInitialCapacity, (void*)(&num));
		}
	}

	public unsafe Il2CppStructArray<int> poolKeys
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolKeys);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolKeys)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<Il2CppArrayBase<T>> poolValues
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolValues);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Il2CppArrayBase<T>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolValues)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int arraysCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_arraysCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_arraysCount)) = num;
		}
	}

	public unsafe long elementsCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elementsCount);
			return *(long*)num;
		}
		set
		{
			*(long*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elementsCount)) = num;
		}
	}

	public unsafe long elementsCapacity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elementsCapacity);
			return *(long*)num;
		}
		set
		{
			*(long*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elementsCapacity)) = num;
		}
	}

	public unsafe bool _LogAllocations_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LogAllocations_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LogAllocations_k__BackingField)) = flag;
		}
	}

	public unsafe long ElementsCapacity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ElementsCapacity_Public_get_Int64_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(long*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 29467, RefRangeEnd = 29479, XrefRangeStart = 29462, XrefRangeEnd = 29467, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ElementsCapacity_Public_set_Void_Int64_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool LogAllocations
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LogAllocations_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_LogAllocations_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe ArrayPoolUsageData UsageData
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29479, XrefRangeEnd = 29480, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UsageData_Public_get_ArrayPoolUsageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ArrayPoolUsageData*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static ArrayPool()
	{
		Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.Pooling.Pools", "ArrayPool`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr);
		NativeFieldInfoPtr_emptySubArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "emptySubArray");
		NativeFieldInfoPtr_random = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "random");
		NativeFieldInfoPtr_keysInitialCapacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "keysInitialCapacity");
		NativeFieldInfoPtr_poolKeys = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "poolKeys");
		NativeFieldInfoPtr_poolValues = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "poolValues");
		NativeFieldInfoPtr_arraysCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "arraysCount");
		NativeFieldInfoPtr_elementsCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "elementsCount");
		NativeFieldInfoPtr_elementsCapacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "elementsCapacity");
		NativeFieldInfoPtr__LogAllocations_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, "<LogAllocations>k__BackingField");
		NativeMethodInfoPtr_get_ElementsCapacity_Public_get_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663793);
		NativeMethodInfoPtr_set_ElementsCapacity_Public_set_Void_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663794);
		NativeMethodInfoPtr_get_LogAllocations_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663795);
		NativeMethodInfoPtr_set_LogAllocations_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663796);
		NativeMethodInfoPtr_get_UsageData_Public_get_ArrayPoolUsageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663797);
		NativeMethodInfoPtr__ctor_Public_Void_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663798);
		NativeMethodInfoPtr_Allocate_Public_SubArray_1_T_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663799);
		NativeMethodInfoPtr_AllocateExactSize_Public_SubArray_1_T_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663800);
		NativeMethodInfoPtr_Free_Public_Void_SubArray_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663801);
		NativeMethodInfoPtr_Free_Public_Void_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663802);
		NativeMethodInfoPtr_Resize_Public_Void_byref_SubArray_1_T_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663803);
		NativeMethodInfoPtr_ResizeAndClear_Public_Void_byref_SubArray_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663804);
		NativeMethodInfoPtr_ResizeCopyless_Public_Void_byref_SubArray_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663805);
		NativeMethodInfoPtr_Clone_Public_SubArray_1_T_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663806);
		NativeMethodInfoPtr_Clone_Public_SubArray_1_T_SubArray_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663807);
		NativeMethodInfoPtr_Allocate_Private_SubArray_1_T_Int32_Boolean_Boolean_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663808);
		NativeMethodInfoPtr_ApplyCapacity_Private_Void_Int64_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663809);
		NativeMethodInfoPtr_RemoveElementAt_Private_Il2CppArrayBase_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663810);
		NativeMethodInfoPtr_BinarySearch_Private_Static_Int32_Il2CppStructArray_1_Int32_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr, 100663811);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29497, RefRangeEnd = 29498, XrefRangeStart = 29480, XrefRangeEnd = 29497, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ArrayPool(long elementsCapacity)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ArrayPool<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&elementsCapacity);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int64_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29498, XrefRangeEnd = 29499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<T> Allocate(int minimalSize, bool clearArray = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&minimalSize);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearArray;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Allocate_Public_SubArray_1_T_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<T>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29499, XrefRangeEnd = 29500, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<T> AllocateExactSize(int exactSize, bool clearArray = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&exactSize);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearArray;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AllocateExactSize_Public_SubArray_1_T_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<T>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29500, XrefRangeEnd = 29501, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Free(SubArray<T> subArray)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)subArray));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Free_Public_Void_SubArray_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 29515, RefRangeEnd = 29522, XrefRangeStart = 29501, XrefRangeEnd = 29515, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Free(Il2CppArrayBase<T> array)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Free_Public_Void_Il2CppArrayBase_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29530, RefRangeEnd = 29531, XrefRangeStart = 29522, XrefRangeEnd = 29530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Resize(ref SubArray<T> subArray, int newMinimalSize, bool clearNewSpace = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)subArray));
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &newMinimalSize;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearNewSpace;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Resize_Public_Void_byref_SubArray_1_T_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29531, XrefRangeEnd = 29534, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResizeAndClear(ref SubArray<T> subArray, int newMinimalSize)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)subArray));
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &newMinimalSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResizeAndClear_Public_Void_byref_SubArray_1_T_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29534, XrefRangeEnd = 29536, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResizeCopyless(ref SubArray<T> subArray, int newMinimalSize)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)subArray));
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &newMinimalSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResizeCopyless_Public_Void_byref_SubArray_1_T_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29536, XrefRangeEnd = 29538, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<T> Clone(Il2CppArrayBase<T> source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_SubArray_1_T_Il2CppArrayBase_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<T>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29538, XrefRangeEnd = 29540, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<T> Clone(SubArray<T> source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)source));
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_SubArray_1_T_SubArray_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<T>(pointer);
	}

	[CallerCount(15)]
	[CachedScanResults(RefRangeStart = 29590, RefRangeEnd = 29605, XrefRangeStart = 29540, XrefRangeEnd = 29590, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<T> Allocate(int size, bool exactSize, bool clearArray, out bool isArrayCleared)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&size);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &exactSize;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearArray;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isArrayCleared);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Allocate_Private_SubArray_1_T_Int32_Boolean_Boolean_byref_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<T>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29605, XrefRangeEnd = 29607, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyCapacity(long capacity)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&capacity);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyCapacity_Private_Void_Int64_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29607, XrefRangeEnd = 29609, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppArrayBase<T> RemoveElementAt(int elementIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&elementIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveElementAt_Private_Il2CppArrayBase_1_T_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
	}

	[CallerCount(0)]
	public unsafe static int BinarySearch(Il2CppStructArray<int> array, int length, int value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BinarySearch_Private_Static_Int32_Il2CppStructArray_1_Int32_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ArrayPool(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
