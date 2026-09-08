using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFishySteamworks;

public class BidirectionalDictionary<T1, T2> : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_t1ToT2Dict;

	private static readonly System.IntPtr NativeFieldInfoPtr_t2ToT1Dict;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FirstTypes_Public_get_IEnumerable_1_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SecondTypes_Public_get_IEnumerable_1_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_First_Public_get_Dictionary_2_T1_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Second_Public_get_Dictionary_2_T2_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Void_T1_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Void_T2_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_T2_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_T1_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetValue_Public_Boolean_T1_byref_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetValue_Public_Boolean_T2_byref_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Contains_Public_Boolean_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Contains_Public_Boolean_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_T1_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Item_Public_set_Void_T2_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_T2_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Item_Public_set_Void_T1_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Dictionary<T1, T2> t1ToT2Dict
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_t1ToT2Dict);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<T1, T2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_t1ToT2Dict)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<T2, T1> t2ToT1Dict
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_t2ToT1Dict);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<T2, T1>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_t2ToT1Dict)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe IEnumerable<T1> FirstTypes
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 78219, RefRangeEnd = 78220, XrefRangeStart = 78215, XrefRangeEnd = 78219, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FirstTypes_Public_get_IEnumerable_1_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<T1>>(intPtr) : null;
		}
	}

	public unsafe IEnumerable<T2> SecondTypes
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78220, XrefRangeEnd = 78226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SecondTypes_Public_get_IEnumerable_1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<T2>>(intPtr) : null;
		}
	}

	public unsafe int Count
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78228, XrefRangeEnd = 78230, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe Dictionary<T1, T2> First
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_First_Public_get_Dictionary_2_T1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<T1, T2>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<T2, T1> Second
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Second_Public_get_Dictionary_2_T2_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<T2, T1>>(intPtr) : null;
		}
	}

	public unsafe T1 this[T2 key]
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			//IL_0061->IL0064: Incompatible stack types: I vs Ref
			//IL_0040->IL0064: Incompatible stack types: I vs Ref
			//IL_004d->IL0064: Incompatible stack types: I vs Ref
			//IL_0054->IL0064: Incompatible stack types: I vs Ref
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			ref T2 reference;
			if (!typeof(T2).IsValueType)
			{
				object obj = key;
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
				reference = ref key;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			Unsafe.SkipInit(out System.IntPtr intPtr3);
			System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_T1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
			Il2CppException.RaiseExceptionIfNecessary(intPtr3);
			return IL2CPP.PointerToValueGeneric<T1>(intPtr2, false, true);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78293, XrefRangeEnd = 78309, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			//IL_0061->IL0064: Incompatible stack types: I vs Ref
			//IL_0040->IL0064: Incompatible stack types: I vs Ref
			//IL_004d->IL0064: Incompatible stack types: I vs Ref
			//IL_00be->IL00c1: Incompatible stack types: I vs Ref
			//IL_009d->IL00c1: Incompatible stack types: I vs Ref
			//IL_0054->IL0064: Incompatible stack types: I vs Ref
			//IL_00aa->IL00c1: Incompatible stack types: I vs Ref
			//IL_00b1->IL00c1: Incompatible stack types: I vs Ref
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			ref T2 reference;
			if (!typeof(T2).IsValueType)
			{
				object obj = key;
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
				reference = ref key;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
			ref T1 reference2;
			if (!typeof(T1).IsValueType)
			{
				object obj2 = value;
				if (obj2 is string)
				{
					reference2 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj2 as string);
				}
				else
				{
					System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj2 is Il2CppObjectBase) ? obj2 : null));
					reference2 = ref *(_003F*)intPtr2;
					if (intPtr2 != (System.IntPtr)0)
					{
						reference2 = ref *(_003F*)intPtr2;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr2)))
						{
							reference2 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr2);
						}
					}
				}
			}
			else
			{
				reference2 = ref value;
			}
			*(void**)num = Unsafe.AsPointer(ref reference2);
			Unsafe.SkipInit(out System.IntPtr intPtr4);
			System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Item_Public_set_Void_T2_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
			Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		}
	}

	public unsafe T2 this[T1 key]
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			//IL_0061->IL0064: Incompatible stack types: I vs Ref
			//IL_0040->IL0064: Incompatible stack types: I vs Ref
			//IL_004d->IL0064: Incompatible stack types: I vs Ref
			//IL_0054->IL0064: Incompatible stack types: I vs Ref
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			ref T1 reference;
			if (!typeof(T1).IsValueType)
			{
				object obj = key;
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
				reference = ref key;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			Unsafe.SkipInit(out System.IntPtr intPtr3);
			System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_T2_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
			Il2CppException.RaiseExceptionIfNecessary(intPtr3);
			return IL2CPP.PointerToValueGeneric<T2>(intPtr2, false, true);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 78329, RefRangeEnd = 78330, XrefRangeStart = 78309, XrefRangeEnd = 78329, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			//IL_0061->IL0064: Incompatible stack types: I vs Ref
			//IL_0040->IL0064: Incompatible stack types: I vs Ref
			//IL_004d->IL0064: Incompatible stack types: I vs Ref
			//IL_00be->IL00c1: Incompatible stack types: I vs Ref
			//IL_009d->IL00c1: Incompatible stack types: I vs Ref
			//IL_0054->IL0064: Incompatible stack types: I vs Ref
			//IL_00aa->IL00c1: Incompatible stack types: I vs Ref
			//IL_00b1->IL00c1: Incompatible stack types: I vs Ref
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			ref T1 reference;
			if (!typeof(T1).IsValueType)
			{
				object obj = key;
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
				reference = ref key;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
			ref T2 reference2;
			if (!typeof(T2).IsValueType)
			{
				object obj2 = value;
				if (obj2 is string)
				{
					reference2 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj2 as string);
				}
				else
				{
					System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj2 is Il2CppObjectBase) ? obj2 : null));
					reference2 = ref *(_003F*)intPtr2;
					if (intPtr2 != (System.IntPtr)0)
					{
						reference2 = ref *(_003F*)intPtr2;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr2)))
						{
							reference2 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr2);
						}
					}
				}
			}
			else
			{
				reference2 = ref value;
			}
			*(void**)num = Unsafe.AsPointer(ref reference2);
			Unsafe.SkipInit(out System.IntPtr intPtr4);
			System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Item_Public_set_Void_T1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
			Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		}
	}

	static BidirectionalDictionary()
	{
		Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishySteamworks", "BidirectionalDictionary`2"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[2]
		{
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T1>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T2>.NativeClassPtr))
		})).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr);
		NativeFieldInfoPtr_t1ToT2Dict = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, "t1ToT2Dict");
		NativeFieldInfoPtr_t2ToT1Dict = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, "t2ToT1Dict");
		NativeMethodInfoPtr_get_FirstTypes_Public_get_IEnumerable_1_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664822);
		NativeMethodInfoPtr_get_SecondTypes_Public_get_IEnumerable_1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664823);
		NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664824);
		NativeMethodInfoPtr_get_Count_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664825);
		NativeMethodInfoPtr_get_First_Public_get_Dictionary_2_T1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664826);
		NativeMethodInfoPtr_get_Second_Public_get_Dictionary_2_T2_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664827);
		NativeMethodInfoPtr_Add_Public_Void_T1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664828);
		NativeMethodInfoPtr_Add_Public_Void_T2_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664829);
		NativeMethodInfoPtr_Get_Public_T2_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664830);
		NativeMethodInfoPtr_Get_Public_T1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664831);
		NativeMethodInfoPtr_TryGetValue_Public_Boolean_T1_byref_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664832);
		NativeMethodInfoPtr_TryGetValue_Public_Boolean_T2_byref_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664833);
		NativeMethodInfoPtr_Contains_Public_Boolean_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664834);
		NativeMethodInfoPtr_Contains_Public_Boolean_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664835);
		NativeMethodInfoPtr_Remove_Public_Void_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664836);
		NativeMethodInfoPtr_Remove_Public_Void_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664837);
		NativeMethodInfoPtr_get_Item_Public_get_T1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664838);
		NativeMethodInfoPtr_set_Item_Public_set_Void_T2_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664839);
		NativeMethodInfoPtr_get_Item_Public_get_T2_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664840);
		NativeMethodInfoPtr_set_Item_Public_set_Void_T1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664841);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr, 100664842);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78226, XrefRangeEnd = 78228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual IEnumerator GetEnumerator()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78250, RefRangeEnd = 78251, XrefRangeStart = 78230, XrefRangeEnd = 78250, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Add(T1 key, T2 value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_00be->IL00c1: Incompatible stack types: I vs Ref
		//IL_009d->IL00c1: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		//IL_00aa->IL00c1: Incompatible stack types: I vs Ref
		//IL_00b1->IL00c1: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		ref T1 reference;
		if (!typeof(T1).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T2 reference2;
		if (!typeof(T2).IsValueType)
		{
			object obj2 = value;
			if (obj2 is string)
			{
				reference2 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj2 as string);
			}
			else
			{
				System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj2 is Il2CppObjectBase) ? obj2 : null));
				reference2 = ref *(_003F*)intPtr2;
				if (intPtr2 != (System.IntPtr)0)
				{
					reference2 = ref *(_003F*)intPtr2;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr2)))
					{
						reference2 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr2);
					}
				}
			}
		}
		else
		{
			reference2 = ref value;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Void_T1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78251, XrefRangeEnd = 78259, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Add(T2 key, T1 value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_00be->IL00c1: Incompatible stack types: I vs Ref
		//IL_009d->IL00c1: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		//IL_00aa->IL00c1: Incompatible stack types: I vs Ref
		//IL_00b1->IL00c1: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		ref T2 reference;
		if (!typeof(T2).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T1 reference2;
		if (!typeof(T1).IsValueType)
		{
			object obj2 = value;
			if (obj2 is string)
			{
				reference2 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj2 as string);
			}
			else
			{
				System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj2 is Il2CppObjectBase) ? obj2 : null));
				reference2 = ref *(_003F*)intPtr2;
				if (intPtr2 != (System.IntPtr)0)
				{
					reference2 = ref *(_003F*)intPtr2;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr2)))
					{
						reference2 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr2);
					}
				}
			}
		}
		else
		{
			reference2 = ref value;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Void_T2_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78259, XrefRangeEnd = 78261, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T2 Get(T1 key)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T1 reference;
		if (!typeof(T1).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_T2_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T2>(intPtr2, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78261, XrefRangeEnd = 78267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T1 Get(T2 key)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T2 reference;
		if (!typeof(T2).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_T1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T1>(intPtr2, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78267, XrefRangeEnd = 78273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetValue(T1 key, out T2 value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_008a->IL008c: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		ref T1 reference;
		if (!typeof(T1).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		Unsafe.SkipInit(out nint num2);
		ref T2 reference2;
		if (!typeof(T2).IsValueType)
		{
			num2 = 0;
			reference2 = ref *(_003F*)(&num2);
		}
		else
		{
			reference2 = ref value;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetValue_Public_Boolean_T1_byref_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		if (!typeof(T2).IsValueType)
		{
			nint num3 = num2;
			Unsafe.As<T2, object>(ref value) = ((num3 == 0) ? ((T2)null) : IL2CPP.PointerToValueGeneric<T2>((System.IntPtr)num3, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 78275, RefRangeEnd = 78277, XrefRangeStart = 78273, XrefRangeEnd = 78275, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetValue(T2 key, out T1 value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_008a->IL008c: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		ref T2 reference;
		if (!typeof(T2).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		Unsafe.SkipInit(out nint num2);
		ref T1 reference2;
		if (!typeof(T1).IsValueType)
		{
			num2 = 0;
			reference2 = ref *(_003F*)(&num2);
		}
		else
		{
			reference2 = ref value;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetValue_Public_Boolean_T2_byref_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		if (!typeof(T1).IsValueType)
		{
			nint num3 = num2;
			Unsafe.As<T1, object>(ref value) = ((num3 == 0) ? ((T1)null) : IL2CPP.PointerToValueGeneric<T1>((System.IntPtr)num3, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78277, XrefRangeEnd = 78279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Contains(T1 key)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T1 reference;
		if (!typeof(T1).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Contains_Public_Boolean_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78279, XrefRangeEnd = 78283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Contains(T2 key)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T2 reference;
		if (!typeof(T2).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Contains_Public_Boolean_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78287, RefRangeEnd = 78288, XrefRangeStart = 78283, XrefRangeEnd = 78287, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Remove(T1 key)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T1 reference;
		if (!typeof(T1).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78292, RefRangeEnd = 78293, XrefRangeStart = 78288, XrefRangeEnd = 78292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Remove(T2 key)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T2 reference;
		if (!typeof(T2).IsValueType)
		{
			object obj = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 78348, RefRangeEnd = 78350, XrefRangeStart = 78330, XrefRangeEnd = 78348, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BidirectionalDictionary()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BidirectionalDictionary<T1, T2>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BidirectionalDictionary(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
