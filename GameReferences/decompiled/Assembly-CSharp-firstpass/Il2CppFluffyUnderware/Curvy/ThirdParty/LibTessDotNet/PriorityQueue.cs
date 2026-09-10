using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public class PriorityQueue<TValue> : Il2CppSystem.Object where TValue : class
{
	public class StackItem : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_p;

		private static readonly System.IntPtr NativeFieldInfoPtr_r;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe int p
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_p);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_p)) = num;
			}
		}

		public unsafe int r
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r)) = num;
			}
		}

		static StackItem()
		{
			Il2CppClassPointerStore<StackItem>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "StackItem"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StackItem>.NativeClassPtr);
			NativeFieldInfoPtr_p = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StackItem>.NativeClassPtr, "p");
			NativeFieldInfoPtr_r = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StackItem>.NativeClassPtr, "r");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StackItem>.NativeClassPtr, 100667182);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe StackItem()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StackItem>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public StackItem(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__leq;

	private static readonly System.IntPtr NativeFieldInfoPtr__heap;

	private static readonly System.IntPtr NativeFieldInfoPtr__keys;

	private static readonly System.IntPtr NativeFieldInfoPtr__order;

	private static readonly System.IntPtr NativeFieldInfoPtr__size;

	private static readonly System.IntPtr NativeFieldInfoPtr__max;

	private static readonly System.IntPtr NativeFieldInfoPtr__initialized;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Empty_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_LessOrEqual_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Swap_Private_Static_Void_byref_Int32_byref_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Init_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Insert_Public_PQHandle_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExtractMin_Public_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Minimum_Public_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_PQHandle_0;

	public unsafe PriorityHeap<TValue>.LessOrEqual _leq
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__leq);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PriorityHeap<TValue>.LessOrEqual>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__leq)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lessOrEqual));
		}
	}

	public unsafe PriorityHeap<TValue> _heap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heap);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PriorityHeap<TValue>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heap)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)priorityHeap));
		}
	}

	public unsafe Il2CppArrayBase<TValue> _keys
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__keys);
			return Il2CppArrayBase<TValue>.WrapNativeGenericArrayPointer(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__keys)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<int> _order
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__order);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__order)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int _size
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__size);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__size)) = num;
		}
	}

	public unsafe int _max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__max);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__max)) = num;
		}
	}

	public unsafe bool _initialized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__initialized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__initialized)) = flag;
		}
	}

	public unsafe bool Empty
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Empty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PriorityQueue()
	{
		Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "PriorityQueue`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr);
		NativeFieldInfoPtr__leq = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_leq");
		NativeFieldInfoPtr__heap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_heap");
		NativeFieldInfoPtr__keys = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_keys");
		NativeFieldInfoPtr__order = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_order");
		NativeFieldInfoPtr__size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_size");
		NativeFieldInfoPtr__max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_max");
		NativeFieldInfoPtr__initialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, "_initialized");
		NativeMethodInfoPtr_get_Empty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667174);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_LessOrEqual_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667175);
		NativeMethodInfoPtr_Swap_Private_Static_Void_byref_Int32_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667176);
		NativeMethodInfoPtr_Init_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667177);
		NativeMethodInfoPtr_Insert_Public_PQHandle_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667178);
		NativeMethodInfoPtr_ExtractMin_Public_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667179);
		NativeMethodInfoPtr_Minimum_Public_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667180);
		NativeMethodInfoPtr_Remove_Public_Void_PQHandle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr, 100667181);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59906, RefRangeEnd = 59907, XrefRangeStart = 59897, XrefRangeEnd = 59906, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PriorityQueue(int initialSize, PriorityHeap<TValue>.LessOrEqual leq)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PriorityQueue<TValue>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&initialSize);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)leq);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_LessOrEqual_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static void Swap(ref int a, ref int b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref a);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Swap_Private_Static_Void_byref_Int32_byref_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59929, RefRangeEnd = 59930, XrefRangeStart = 59907, XrefRangeEnd = 59929, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Init()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Init_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59932, RefRangeEnd = 59933, XrefRangeStart = 59930, XrefRangeEnd = 59932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PQHandle Insert(TValue value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref TValue reference;
		if (!typeof(TValue).IsValueType)
		{
			TValue val = value;
			if (val is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref value;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Insert_Public_PQHandle_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return *(PQHandle*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 59934, RefRangeEnd = 59936, XrefRangeStart = 59933, XrefRangeEnd = 59934, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TValue ExtractMin()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ExtractMin_Public_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<TValue>(intPtr, false, true);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59936, RefRangeEnd = 59937, XrefRangeStart = 59936, XrefRangeEnd = 59936, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TValue Minimum()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Minimum_Public_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<TValue>(intPtr, false, true);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 59938, RefRangeEnd = 59940, XrefRangeStart = 59937, XrefRangeEnd = 59938, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Remove(PQHandle handle)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&handle);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_PQHandle_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PriorityQueue(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
