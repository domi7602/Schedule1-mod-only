using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class EnumExt : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_HasFlag_Public_Static_Boolean_T_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_T_T_0, Il2CppClassPointerStore<EnumExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_Set_Public_Static_T_Enum_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Set_Public_Static_T_Enum_T_0, Il2CppClassPointerStore<EnumExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_Set_Public_Static_T_Enum_T_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Set_Public_Static_T_Enum_T_Boolean_0, Il2CppClassPointerStore<EnumExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_Enum_Il2CppReferenceArray_1_Enum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_T_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Set_Public_Static_T_Enum_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Set_Public_Static_T_Enum_T_Boolean_0;

	static EnumExt()
	{
		Il2CppClassPointerStore<EnumExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "EnumExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EnumExt>.NativeClassPtr);
		NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_Enum_Il2CppReferenceArray_1_Enum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EnumExt>.NativeClassPtr, 100664381);
		NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_T_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EnumExt>.NativeClassPtr, 100664382);
		NativeMethodInfoPtr_Set_Public_Static_T_Enum_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EnumExt>.NativeClassPtr, 100664383);
		NativeMethodInfoPtr_Set_Public_Static_T_Enum_T_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EnumExt>.NativeClassPtr, 100664384);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34066, XrefRangeEnd = 34079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool HasFlag(this Il2CppSystem.Enum variable, [Optional] Il2CppReferenceArray<Il2CppSystem.Enum> flags)
	{
		if (flags == null)
		{
			flags = new Il2CppReferenceArray<Il2CppSystem.Enum>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)variable);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)flags);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasFlag_Public_Static_Boolean_Enum_Il2CppReferenceArray_1_Enum_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34079, XrefRangeEnd = 34091, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool HasFlag<T>(this T value, T flag) where T : new()
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
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference2;
		if (!typeof(T).IsValueType)
		{
			object obj2 = flag;
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
			reference2 = ref flag;
		}
		*(void**)num = Unsafe.AsPointer(ref reference2);
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_HasFlag_Public_Static_Boolean_T_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr3);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34091, XrefRangeEnd = 34096, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T Set<T>(this Il2CppSystem.Enum value, T append)
	{
		//IL_006c->IL006f: Incompatible stack types: I vs Ref
		//IL_004b->IL006f: Incompatible stack types: I vs Ref
		//IL_0058->IL006f: Incompatible stack types: I vs Ref
		//IL_005f->IL006f: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = append;
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
			reference = ref append;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_Set_Public_Static_T_Enum_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34096, XrefRangeEnd = 34123, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T Set<T>(this Il2CppSystem.Enum value, T append, bool OnOff)
	{
		//IL_006c->IL006f: Incompatible stack types: I vs Ref
		//IL_004b->IL006f: Incompatible stack types: I vs Ref
		//IL_0058->IL006f: Incompatible stack types: I vs Ref
		//IL_005f->IL006f: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = append;
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
			reference = ref append;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &OnOff;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_Set_Public_Static_T_Enum_T_Boolean_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	public static bool HasFlag(Il2CppSystem.Enum variable, params Il2CppSystem.Enum[] flags)
	{
		return variable.HasFlag(new Il2CppReferenceArray<Il2CppSystem.Enum>(flags));
	}

	public EnumExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
