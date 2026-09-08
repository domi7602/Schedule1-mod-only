using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace Il2Cpp;

[ObfuscatedName("<>f__AnonymousType1`5")]
public sealed class __f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar> : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__A_i__Field;

	private static readonly System.IntPtr NativeFieldInfoPtr__B_i__Field;

	private static readonly System.IntPtr NativeFieldInfoPtr__C_i__Field;

	private static readonly System.IntPtr NativeFieldInfoPtr__D_i__Field;

	private static readonly System.IntPtr NativeFieldInfoPtr__E_i__Field;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_A_Public_get__A_j__TPar_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_B_Public_get__B_j__TPar_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_C_Public_get__C_j__TPar_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_D_Public_get__D_j__TPar_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_E_Public_get__E_j__TPar_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void__A_j__TPar__B_j__TPar__C_j__TPar__D_j__TPar__E_j__TPar_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe _A_j__TPar _A_i__Field
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__A_i__Field);
			return IL2CPP.PointerToValueGeneric<_A_j__TPar>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__A_i__Field);
			System.Type typeFromHandle = typeof(_A_j__TPar);
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

	public unsafe _B_j__TPar _B_i__Field
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__B_i__Field);
			return IL2CPP.PointerToValueGeneric<_B_j__TPar>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__B_i__Field);
			System.Type typeFromHandle = typeof(_B_j__TPar);
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

	public unsafe _C_j__TPar _C_i__Field
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__C_i__Field);
			return IL2CPP.PointerToValueGeneric<_C_j__TPar>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__C_i__Field);
			System.Type typeFromHandle = typeof(_C_j__TPar);
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

	public unsafe _D_j__TPar _D_i__Field
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__D_i__Field);
			return IL2CPP.PointerToValueGeneric<_D_j__TPar>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__D_i__Field);
			System.Type typeFromHandle = typeof(_D_j__TPar);
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

	public unsafe _E_j__TPar _E_i__Field
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__E_i__Field);
			return IL2CPP.PointerToValueGeneric<_E_j__TPar>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__E_i__Field);
			System.Type typeFromHandle = typeof(_E_j__TPar);
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

	public unsafe _A_j__TPar A
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_A_Public_get__A_j__TPar_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<_A_j__TPar>(intPtr, false, true);
		}
	}

	public unsafe _B_j__TPar B
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_B_Public_get__B_j__TPar_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<_B_j__TPar>(intPtr, false, true);
		}
	}

	public unsafe _C_j__TPar C
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 3891, RefRangeEnd = 3894, XrefRangeStart = 3891, XrefRangeEnd = 3891, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_C_Public_get__C_j__TPar_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<_C_j__TPar>(intPtr, false, true);
		}
	}

	public unsafe _D_j__TPar D
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_D_Public_get__D_j__TPar_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<_D_j__TPar>(intPtr, false, true);
		}
	}

	public unsafe _E_j__TPar E
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 3894, RefRangeEnd = 3895, XrefRangeStart = 3894, XrefRangeEnd = 3894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_E_Public_get__E_j__TPar_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<_E_j__TPar>(intPtr, false, true);
		}
	}

	static __f__AnonymousType1()
	{
		Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "", "<>f__AnonymousType1`5"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[5]
		{
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<_A_j__TPar>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<_B_j__TPar>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<_C_j__TPar>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<_D_j__TPar>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<_E_j__TPar>.NativeClassPtr))
		})).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr);
		NativeFieldInfoPtr__A_i__Field = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, "<A>i__Field");
		NativeFieldInfoPtr__B_i__Field = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, "<B>i__Field");
		NativeFieldInfoPtr__C_i__Field = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, "<C>i__Field");
		NativeFieldInfoPtr__D_i__Field = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, "<D>i__Field");
		NativeFieldInfoPtr__E_i__Field = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, "<E>i__Field");
		NativeMethodInfoPtr_get_A_Public_get__A_j__TPar_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663305);
		NativeMethodInfoPtr_get_B_Public_get__B_j__TPar_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663306);
		NativeMethodInfoPtr_get_C_Public_get__C_j__TPar_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663307);
		NativeMethodInfoPtr_get_D_Public_get__D_j__TPar_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663308);
		NativeMethodInfoPtr_get_E_Public_get__E_j__TPar_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663309);
		NativeMethodInfoPtr__ctor_Public_Void__A_j__TPar__B_j__TPar__C_j__TPar__D_j__TPar__E_j__TPar_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663310);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663311);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663312);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr, 100663313);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 3896, RefRangeEnd = 3898, XrefRangeStart = 3895, XrefRangeEnd = 3896, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe __f__AnonymousType1(_A_j__TPar A, _B_j__TPar B, _C_j__TPar C, _D_j__TPar D, _E_j__TPar E)
	{
		//IL_006a->IL006d: Incompatible stack types: I vs Ref
		//IL_0049->IL006d: Incompatible stack types: I vs Ref
		//IL_0056->IL006d: Incompatible stack types: I vs Ref
		//IL_00c7->IL00ca: Incompatible stack types: I vs Ref
		//IL_00a6->IL00ca: Incompatible stack types: I vs Ref
		//IL_005d->IL006d: Incompatible stack types: I vs Ref
		//IL_00b3->IL00ca: Incompatible stack types: I vs Ref
		//IL_0124->IL0127: Incompatible stack types: I vs Ref
		//IL_0103->IL0127: Incompatible stack types: I vs Ref
		//IL_00ba->IL00ca: Incompatible stack types: I vs Ref
		//IL_0110->IL0127: Incompatible stack types: I vs Ref
		//IL_0182->IL0185: Incompatible stack types: I vs Ref
		//IL_0161->IL0185: Incompatible stack types: I vs Ref
		//IL_0117->IL0127: Incompatible stack types: I vs Ref
		//IL_016e->IL0185: Incompatible stack types: I vs Ref
		//IL_01e0->IL01e3: Incompatible stack types: I vs Ref
		//IL_01bf->IL01e3: Incompatible stack types: I vs Ref
		//IL_0175->IL0185: Incompatible stack types: I vs Ref
		//IL_01cc->IL01e3: Incompatible stack types: I vs Ref
		//IL_01d3->IL01e3: Incompatible stack types: I vs Ref
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__f__AnonymousType1<_A_j__TPar, _B_j__TPar, _C_j__TPar, _D_j__TPar, _E_j__TPar>>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		ref _A_j__TPar reference;
		if (!typeof(_A_j__TPar).IsValueType)
		{
			object obj = A;
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
			reference = ref A;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref _B_j__TPar reference2;
		if (!typeof(_B_j__TPar).IsValueType)
		{
			object obj2 = B;
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
			reference2 = ref B;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		byte* num2 = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		ref _C_j__TPar reference3;
		if (!typeof(_C_j__TPar).IsValueType)
		{
			object obj3 = C;
			if (obj3 is string)
			{
				reference3 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj3 as string);
			}
			else
			{
				System.IntPtr intPtr3 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj3 is Il2CppObjectBase) ? obj3 : null));
				reference3 = ref *(_003F*)intPtr3;
				if (intPtr3 != (System.IntPtr)0)
				{
					reference3 = ref *(_003F*)intPtr3;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr3)))
					{
						reference3 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr3);
					}
				}
			}
		}
		else
		{
			reference3 = ref C;
		}
		*(void**)num2 = Unsafe.AsPointer(ref reference3);
		byte* num3 = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		ref _D_j__TPar reference4;
		if (!typeof(_D_j__TPar).IsValueType)
		{
			object obj4 = D;
			if (obj4 is string)
			{
				reference4 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj4 as string);
			}
			else
			{
				System.IntPtr intPtr4 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj4 is Il2CppObjectBase) ? obj4 : null));
				reference4 = ref *(_003F*)intPtr4;
				if (intPtr4 != (System.IntPtr)0)
				{
					reference4 = ref *(_003F*)intPtr4;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr4)))
					{
						reference4 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr4);
					}
				}
			}
		}
		else
		{
			reference4 = ref D;
		}
		*(void**)num3 = Unsafe.AsPointer(ref reference4);
		byte* num4 = (byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)));
		ref _E_j__TPar reference5;
		if (!typeof(_E_j__TPar).IsValueType)
		{
			object obj5 = E;
			if (obj5 is string)
			{
				reference5 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj5 as string);
			}
			else
			{
				System.IntPtr intPtr5 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj5 is Il2CppObjectBase) ? obj5 : null));
				reference5 = ref *(_003F*)intPtr5;
				if (intPtr5 != (System.IntPtr)0)
				{
					reference5 = ref *(_003F*)intPtr5;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr5)))
					{
						reference5 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr5);
					}
				}
			}
		}
		else
		{
			reference5 = ref E;
		}
		*(void**)num4 = Unsafe.AsPointer(ref reference5);
		Unsafe.SkipInit(out System.IntPtr intPtr7);
		System.IntPtr intPtr6 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void__A_j__TPar__B_j__TPar__C_j__TPar__D_j__TPar__E_j__TPar_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr7);
		Il2CppException.RaiseExceptionIfNecessary(intPtr7);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3898, XrefRangeEnd = 3899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool Equals(Il2CppSystem.Object value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3899, XrefRangeEnd = 3904, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 3904, XrefRangeEnd = 3926, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public __f__AnonymousType1(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
