using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2Cpp;

public class RollingAverage<T> : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_buffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_add;

	private static readonly System.IntPtr NativeFieldInfoPtr_sub;

	private static readonly System.IntPtr NativeFieldInfoPtr_div;

	private static readonly System.IntPtr NativeFieldInfoPtr_head;

	private static readonly System.IntPtr NativeFieldInfoPtr_count;

	private static readonly System.IntPtr NativeFieldInfoPtr_sum;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Func_3_T_T_T_Func_3_T_T_T_Func_3_T_Single_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Void_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Average_Public_get_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Capacity_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	public unsafe Il2CppArrayBase<T> buffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buffer);
			return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppSystem.Func<T, T, T> add
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_add);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<T, T, T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_add)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
		}
	}

	public unsafe Il2CppSystem.Func<T, T, T> sub
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sub);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<T, T, T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sub)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
		}
	}

	public unsafe Il2CppSystem.Func<T, float, T> div
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_div);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<T, float, T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_div)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
		}
	}

	public unsafe int head
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_head);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_head)) = num;
		}
	}

	public unsafe int count
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_count);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_count)) = num;
		}
	}

	public unsafe T sum
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sum);
			return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sum);
			System.Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsValueType)
			{
				if (!string.Equals(typeFromHandle.FullName, "System.String"))
				{
					System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((((object)val) is Il2CppObjectBase) ? ((object)val) : null));
					if (intPtr != (System.IntPtr)0 && IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						System.IntPtr intPtr2 = intPtr;
						// IL cpblk instruction
						Unsafe.CopyBlock(num2, IL2CPP.il2cpp_object_unbox(intPtr), IL2CPP.il2cpp_class_value_size(IL2CPP.il2cpp_object_get_class(intPtr2), ref *(uint*)null));
					}
					else
					{
						IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, intPtr);
					}
				}
				else
				{
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, IL2CPP.ManagedStringToIl2Cpp(val as string));
				}
			}
			else
			{
				Unsafe.Write((void*)num2, val);
			}
		}
	}

	public unsafe T Average
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 66685, RefRangeEnd = 66687, XrefRangeStart = 66685, XrefRangeEnd = 66685, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Average_Public_get_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
		}
	}

	public unsafe int Count
	{
		[CallerCount(126)]
		[CachedScanResults(RefRangeStart = 41326, RefRangeEnd = 41452, XrefRangeStart = 41326, XrefRangeEnd = 41452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Count_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int Capacity
	{
		[CallerCount(57)]
		[CachedScanResults(RefRangeStart = 32354, RefRangeEnd = 32411, XrefRangeStart = 32354, XrefRangeEnd = 32411, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Capacity_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static RollingAverage()
	{
		Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "RollingAverage`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr);
		NativeFieldInfoPtr_buffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "buffer");
		NativeFieldInfoPtr_add = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "add");
		NativeFieldInfoPtr_sub = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "sub");
		NativeFieldInfoPtr_div = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "div");
		NativeFieldInfoPtr_head = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "head");
		NativeFieldInfoPtr_count = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "count");
		NativeFieldInfoPtr_sum = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, "sum");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Func_3_T_T_T_Func_3_T_T_T_Func_3_T_Single_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, 100663480);
		NativeMethodInfoPtr_Add_Public_Void_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, 100663481);
		NativeMethodInfoPtr_get_Average_Public_get_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, 100663482);
		NativeMethodInfoPtr_get_Count_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, 100663483);
		NativeMethodInfoPtr_get_Capacity_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, 100663484);
		NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr, 100663485);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 66682, RefRangeEnd = 66683, XrefRangeStart = 66675, XrefRangeEnd = 66682, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RollingAverage(int capacity, Il2CppSystem.Func<T, T, T> add, Il2CppSystem.Func<T, T, T> sub, Il2CppSystem.Func<T, float, T> div)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RollingAverage<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&capacity);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)add);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sub);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)div);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Func_3_T_T_T_Func_3_T_T_T_Func_3_T_Single_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 66684, RefRangeEnd = 66685, XrefRangeStart = 66683, XrefRangeEnd = 66684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Add(T value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = value;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
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
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Void_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66687, XrefRangeEnd = 66688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RollingAverage(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
