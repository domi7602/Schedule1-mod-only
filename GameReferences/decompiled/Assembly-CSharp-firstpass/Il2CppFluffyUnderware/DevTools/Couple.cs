using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools;

public class Couple<T1, T2> : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__First_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Second_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_First_Public_get_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_First_Public_set_Void_T1_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Second_Public_get_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Second_Public_set_Void_T2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_T1_T2_0;

	public unsafe T1 _First_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__First_k__BackingField);
			return IL2CPP.PointerToValueGeneric<T1>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__First_k__BackingField);
			System.Type typeFromHandle = typeof(T1);
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

	public unsafe T2 _Second_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Second_k__BackingField);
			return IL2CPP.PointerToValueGeneric<T2>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Second_k__BackingField);
			System.Type typeFromHandle = typeof(T2);
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

	public unsafe T1 First
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30720, XrefRangeEnd = 30724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_First_Public_get_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<T1>(intPtr, false, true);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30724, XrefRangeEnd = 30727, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
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
			System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_First_Public_set_Void_T1_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
			Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		}
	}

	public unsafe T2 Second
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30727, XrefRangeEnd = 30731, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Second_Public_get_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<T2>(intPtr, false, true);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30731, XrefRangeEnd = 30734, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
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
			System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Second_Public_set_Void_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
			Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		}
	}

	static Couple()
	{
		Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "Couple`2"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[2]
		{
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T1>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T2>.NativeClassPtr))
		})).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr);
		NativeFieldInfoPtr__First_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, "<First>k__BackingField");
		NativeFieldInfoPtr__Second_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, "<Second>k__BackingField");
		NativeMethodInfoPtr_get_First_Public_get_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, 100663904);
		NativeMethodInfoPtr_set_First_Public_set_Void_T1_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, 100663905);
		NativeMethodInfoPtr_get_Second_Public_get_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, 100663906);
		NativeMethodInfoPtr_set_Second_Public_set_Void_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, 100663907);
		NativeMethodInfoPtr__ctor_Public_Void_T1_T2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr, 100663908);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30734, XrefRangeEnd = 30739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Couple(T1 first, T2 second)
	{
		//IL_006a->IL006d: Incompatible stack types: I vs Ref
		//IL_0049->IL006d: Incompatible stack types: I vs Ref
		//IL_0056->IL006d: Incompatible stack types: I vs Ref
		//IL_00c7->IL00ca: Incompatible stack types: I vs Ref
		//IL_00a6->IL00ca: Incompatible stack types: I vs Ref
		//IL_005d->IL006d: Incompatible stack types: I vs Ref
		//IL_00b3->IL00ca: Incompatible stack types: I vs Ref
		//IL_00ba->IL00ca: Incompatible stack types: I vs Ref
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Couple<T1, T2>>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		ref T1 reference;
		if (!typeof(T1).IsValueType)
		{
			object obj = first;
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
			reference = ref first;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T2 reference2;
		if (!typeof(T2).IsValueType)
		{
			object obj2 = second;
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
			reference2 = ref second;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_T1_T2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
	}

	public Couple(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
