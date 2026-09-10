using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class TypeExt : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_GetAllTypesWithAttribute_Public_Static_Dictionary_2_U_Type_Type_0<U>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetAllTypesWithAttribute_Public_Static_Dictionary_2_U_Type_Type_0, Il2CppClassPointerStore<TypeExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<U>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetFieldsWithAttribute_Public_Static_List_1_FieldInfo_Type_Boolean_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetFieldsWithAttribute_Public_Static_List_1_FieldInfo_Type_Boolean_Boolean_0, Il2CppClassPointerStore<TypeExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetCustomAttribute_Public_Static_T_Type_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetCustomAttribute_Public_Static_T_Type_0, Il2CppClassPointerStore<TypeExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLoadedTypes_Public_Static_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLoadedAssemblies_Public_Static_IEnumerable_1_Assembly_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllTypesWithAttribute_Public_Static_Dictionary_2_U_Type_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFieldsWithAttribute_Public_Static_List_1_FieldInfo_Type_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCustomAttribute_Public_Static_T_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MethodByName_Public_Static_MethodInfo_Type_String_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FieldByName_Public_Static_FieldInfo_Type_String_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PropertyByName_Public_Static_PropertyInfo_Type_String_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllFields_Public_Static_Il2CppReferenceArray_1_FieldInfo_Type_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllProperties_Public_Static_Il2CppReferenceArray_1_PropertyInfo_Type_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsFrameworkType_Public_Static_Boolean_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsArrayOrList_Public_Static_Boolean_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEnumerableType_Public_Static_Type_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindIEnumerable_Private_Static_Type_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMethodIncludingBaseClasses_Private_Static_MethodInfo_Type_String_BindingFlags_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFieldIncludingBaseClasses_Private_Static_FieldInfo_Type_String_BindingFlags_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPropertyIncludingBaseClasses_Private_Static_PropertyInfo_Type_String_BindingFlags_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Matches_Public_Static_Boolean_Type_Il2CppReferenceArray_1_Type_0;

	static TypeExt()
	{
		Il2CppClassPointerStore<TypeExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "TypeExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TypeExt>.NativeClassPtr);
		NativeMethodInfoPtr_GetLoadedTypes_Public_Static_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664410);
		NativeMethodInfoPtr_GetLoadedAssemblies_Public_Static_IEnumerable_1_Assembly_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664411);
		NativeMethodInfoPtr_GetAllTypesWithAttribute_Public_Static_Dictionary_2_U_Type_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664412);
		NativeMethodInfoPtr_GetFieldsWithAttribute_Public_Static_List_1_FieldInfo_Type_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664413);
		NativeMethodInfoPtr_GetCustomAttribute_Public_Static_T_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664414);
		NativeMethodInfoPtr_MethodByName_Public_Static_MethodInfo_Type_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664415);
		NativeMethodInfoPtr_FieldByName_Public_Static_FieldInfo_Type_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664416);
		NativeMethodInfoPtr_PropertyByName_Public_Static_PropertyInfo_Type_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664417);
		NativeMethodInfoPtr_GetAllFields_Public_Static_Il2CppReferenceArray_1_FieldInfo_Type_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664418);
		NativeMethodInfoPtr_GetAllProperties_Public_Static_Il2CppReferenceArray_1_PropertyInfo_Type_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664419);
		NativeMethodInfoPtr_IsFrameworkType_Public_Static_Boolean_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664420);
		NativeMethodInfoPtr_IsArrayOrList_Public_Static_Boolean_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664421);
		NativeMethodInfoPtr_GetEnumerableType_Public_Static_Type_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664422);
		NativeMethodInfoPtr_FindIEnumerable_Private_Static_Type_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664423);
		NativeMethodInfoPtr_GetMethodIncludingBaseClasses_Private_Static_MethodInfo_Type_String_BindingFlags_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664424);
		NativeMethodInfoPtr_GetFieldIncludingBaseClasses_Private_Static_FieldInfo_Type_String_BindingFlags_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664425);
		NativeMethodInfoPtr_GetPropertyIncludingBaseClasses_Private_Static_PropertyInfo_Type_String_BindingFlags_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664426);
		NativeMethodInfoPtr_Matches_Public_Static_Boolean_Type_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TypeExt>.NativeClassPtr, 100664427);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34353, RefRangeEnd = 34355, XrefRangeStart = 34312, XrefRangeEnd = 34353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<Il2CppSystem.Type> GetLoadedTypes()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLoadedTypes_Public_Static_Il2CppReferenceArray_1_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Il2CppSystem.Type>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34355, XrefRangeEnd = 34358, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerable<Assembly> GetLoadedAssemblies()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLoadedAssemblies_Public_Static_IEnumerable_1_Assembly_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<Assembly>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 34389, RefRangeEnd = 34390, XrefRangeStart = 34358, XrefRangeEnd = 34389, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<U, Il2CppSystem.Type> GetAllTypesWithAttribute<U>(this Il2CppSystem.Type type)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetAllTypesWithAttribute_Public_Static_Dictionary_2_U_Type_Type_0<U>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<U, Il2CppSystem.Type>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34390, XrefRangeEnd = 34403, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<FieldInfo> GetFieldsWithAttribute<T>(this Il2CppSystem.Type type, bool includeInherited = false, bool includePrivate = false) where T : Il2CppSystem.Attribute
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInherited;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includePrivate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetFieldsWithAttribute_Public_Static_List_1_FieldInfo_Type_Boolean_Boolean_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<FieldInfo>>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 34411, RefRangeEnd = 34416, XrefRangeStart = 34403, XrefRangeEnd = 34411, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetCustomAttribute<T>(this Il2CppSystem.Type type) where T : Il2CppSystem.Attribute
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetCustomAttribute_Public_Static_T_Type_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 34417, RefRangeEnd = 34418, XrefRangeStart = 34416, XrefRangeEnd = 34417, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MethodInfo MethodByName(this Il2CppSystem.Type type, string name, bool includeInherited = false, bool includePrivate = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInherited;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &includePrivate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MethodByName_Public_Static_MethodInfo_Type_String_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MethodInfo>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 34418, RefRangeEnd = 34422, XrefRangeStart = 34418, XrefRangeEnd = 34418, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static FieldInfo FieldByName(this Il2CppSystem.Type type, string name, bool includeInherited = false, bool includePrivate = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInherited;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &includePrivate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FieldByName_Public_Static_FieldInfo_Type_String_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<FieldInfo>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 34423, RefRangeEnd = 34426, XrefRangeStart = 34422, XrefRangeEnd = 34423, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static PropertyInfo PropertyByName(this Il2CppSystem.Type type, string name, bool includeInherited = false, bool includePrivate = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInherited;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &includePrivate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PropertyByName_Public_Static_PropertyInfo_Type_String_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PropertyInfo>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 34447, RefRangeEnd = 34453, XrefRangeStart = 34426, XrefRangeEnd = 34447, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<FieldInfo> GetAllFields(this Il2CppSystem.Type type, bool includeInherited = false, bool includePrivate = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInherited;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includePrivate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAllFields_Public_Static_Il2CppReferenceArray_1_FieldInfo_Type_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<FieldInfo>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34453, XrefRangeEnd = 34474, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<PropertyInfo> GetAllProperties(this Il2CppSystem.Type type, bool includeInherited = false, bool includePrivate = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeInherited;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includePrivate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAllProperties_Public_Static_Il2CppReferenceArray_1_PropertyInfo_Type_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<PropertyInfo>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34474, XrefRangeEnd = 34486, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsFrameworkType(this Il2CppSystem.Type type)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsFrameworkType_Public_Static_Boolean_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34486, XrefRangeEnd = 34489, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsArrayOrList(this Il2CppSystem.Type type)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsArrayOrList_Public_Static_Boolean_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34489, XrefRangeEnd = 34494, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Type GetEnumerableType(this Il2CppSystem.Type t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEnumerableType_Public_Static_Type_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 34543, RefRangeEnd = 34546, XrefRangeStart = 34494, XrefRangeEnd = 34543, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Type FindIEnumerable(Il2CppSystem.Type seqType)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)seqType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindIEnumerable_Private_Static_Type_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34546, XrefRangeEnd = 34562, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MethodInfo GetMethodIncludingBaseClasses(this Il2CppSystem.Type type, string name, BindingFlags bindingFlags)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(BindingFlags**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &bindingFlags;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMethodIncludingBaseClasses_Private_Static_MethodInfo_Type_String_BindingFlags_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MethodInfo>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34562, XrefRangeEnd = 34576, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static FieldInfo GetFieldIncludingBaseClasses(this Il2CppSystem.Type type, string name, BindingFlags bindingFlags)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(BindingFlags**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &bindingFlags;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFieldIncludingBaseClasses_Private_Static_FieldInfo_Type_String_BindingFlags_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<FieldInfo>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34576, XrefRangeEnd = 34592, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static PropertyInfo GetPropertyIncludingBaseClasses(this Il2CppSystem.Type type, string name, BindingFlags bindingFlags)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(BindingFlags**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &bindingFlags;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPropertyIncludingBaseClasses_Private_Static_PropertyInfo_Type_String_BindingFlags_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PropertyInfo>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34597, RefRangeEnd = 34599, XrefRangeStart = 34592, XrefRangeEnd = 34597, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Matches(this Il2CppSystem.Type type, [Optional] Il2CppReferenceArray<Il2CppSystem.Type> types)
	{
		if (types == null)
		{
			types = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)types);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Matches_Public_Static_Boolean_Type_Il2CppReferenceArray_1_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public static bool Matches(Il2CppSystem.Type type, params Il2CppSystem.Type[] types)
	{
		return type.Matches(new Il2CppReferenceArray<Il2CppSystem.Type>(types));
	}

	public TypeExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
