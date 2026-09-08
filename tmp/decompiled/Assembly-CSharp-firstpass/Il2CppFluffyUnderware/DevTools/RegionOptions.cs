using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools;

public sealed class RegionOptions<T> : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_LabelFrom;

	private static readonly System.IntPtr NativeFieldInfoPtr_LabelTo;

	private static readonly System.IntPtr NativeFieldInfoPtr_OptionalTooltip;

	private static readonly System.IntPtr NativeFieldInfoPtr_ClampFrom;

	private static readonly System.IntPtr NativeFieldInfoPtr_ClampTo;

	private static readonly System.IntPtr NativeFieldInfoPtr_FromMin;

	private static readonly System.IntPtr NativeFieldInfoPtr_FromMax;

	private static readonly System.IntPtr NativeFieldInfoPtr_ToMin;

	private static readonly System.IntPtr NativeFieldInfoPtr_ToMax;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Default_Public_Static_get_RegionOptions_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MinMax_Public_Static_RegionOptions_1_T_T_T_0;

	public unsafe string LabelFrom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelFrom);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelFrom)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string LabelTo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelTo);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelTo)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string OptionalTooltip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OptionalTooltip);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OptionalTooltip)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe DTValueClamping ClampFrom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClampFrom);
			return *(DTValueClamping*)num;
		}
		set
		{
			*(DTValueClamping*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClampFrom)) = dTValueClamping;
		}
	}

	public unsafe DTValueClamping ClampTo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClampTo);
			return *(DTValueClamping*)num;
		}
		set
		{
			*(DTValueClamping*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ClampTo)) = dTValueClamping;
		}
	}

	public unsafe T FromMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FromMin);
			return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FromMin);
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

	public unsafe T FromMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FromMax);
			return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FromMax);
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

	public unsafe T ToMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ToMin);
			return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ToMin);
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

	public unsafe T ToMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ToMax);
			return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ToMax);
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

	public unsafe static RegionOptions<T> Default
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32287, XrefRangeEnd = 32299, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Default_Public_Static_get_RegionOptions_1_T_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new RegionOptions<T>(pointer);
		}
	}

	static RegionOptions()
	{
		Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "RegionOptions`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr);
		NativeFieldInfoPtr_LabelFrom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "LabelFrom");
		NativeFieldInfoPtr_LabelTo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "LabelTo");
		NativeFieldInfoPtr_OptionalTooltip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "OptionalTooltip");
		NativeFieldInfoPtr_ClampFrom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "ClampFrom");
		NativeFieldInfoPtr_ClampTo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "ClampTo");
		NativeFieldInfoPtr_FromMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "FromMin");
		NativeFieldInfoPtr_FromMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "FromMax");
		NativeFieldInfoPtr_ToMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "ToMin");
		NativeFieldInfoPtr_ToMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, "ToMax");
		NativeMethodInfoPtr_get_Default_Public_Static_get_RegionOptions_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, 100664181);
		NativeMethodInfoPtr_MinMax_Public_Static_RegionOptions_1_T_T_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr, 100664182);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 32307, RefRangeEnd = 32309, XrefRangeStart = 32299, XrefRangeEnd = 32307, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RegionOptions<T> MinMax(T min, T max)
	{
		//IL_005a->IL005d: Incompatible stack types: I vs Ref
		//IL_0039->IL005d: Incompatible stack types: I vs Ref
		//IL_0046->IL005d: Incompatible stack types: I vs Ref
		//IL_00b7->IL00ba: Incompatible stack types: I vs Ref
		//IL_0096->IL00ba: Incompatible stack types: I vs Ref
		//IL_004d->IL005d: Incompatible stack types: I vs Ref
		//IL_00a3->IL00ba: Incompatible stack types: I vs Ref
		//IL_00aa->IL00ba: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = min;
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
			reference = ref min;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference2;
		if (!typeof(T).IsValueType)
		{
			object obj2 = max;
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
			reference2 = ref max;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MinMax_Public_Static_RegionOptions_1_T_T_T_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return new RegionOptions<T>(pointer);
	}

	public RegionOptions(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public RegionOptions()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RegionOptions<T>>.NativeClassPtr))
	{
	}
}
