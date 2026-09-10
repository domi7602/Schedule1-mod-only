using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.DevUtilities;

public static class Extensions : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_ShiftLeft_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ShiftLeft_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0, Il2CppClassPointerStore<Extensions>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ShiftRight_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ShiftRight_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0, Il2CppClassPointerStore<Extensions>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ShiftLeft_Public_Static_List_1_T_List_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ShiftLeft_Public_Static_List_1_T_List_1_T_0, Il2CppClassPointerStore<Extensions>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ShiftRight_Public_Static_List_1_T_List_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ShiftRight_Public_Static_List_1_T_List_1_T_0, Il2CppClassPointerStore<Extensions>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_ShiftLeft_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShiftRight_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShiftLeft_Public_Static_List_1_T_List_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShiftRight_Public_Static_List_1_T_List_1_T_0;

	static Extensions()
	{
		Il2CppClassPointerStore<Extensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "Extensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Extensions>.NativeClassPtr);
		NativeMethodInfoPtr_ShiftLeft_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Extensions>.NativeClassPtr, 100675007);
		NativeMethodInfoPtr_ShiftRight_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Extensions>.NativeClassPtr, 100675008);
		NativeMethodInfoPtr_ShiftLeft_Public_Static_List_1_T_List_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Extensions>.NativeClassPtr, 100675009);
		NativeMethodInfoPtr_ShiftRight_Public_Static_List_1_T_List_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Extensions>.NativeClassPtr, 100675010);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194051, XrefRangeEnd = 194066, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppArrayBase<T> ShiftLeft<T>(this Il2CppArrayBase<T> array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ShiftLeft_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194066, XrefRangeEnd = 194080, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppArrayBase<T> ShiftRight<T>(this Il2CppArrayBase<T> array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ShiftRight_Public_Static_Il2CppArrayBase_1_T_Il2CppArrayBase_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194080, XrefRangeEnd = 194087, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<T> ShiftLeft<T>(this List<T> array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ShiftLeft_Public_Static_List_1_T_List_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194087, XrefRangeEnd = 194093, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<T> ShiftRight<T>(this List<T> array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ShiftRight_Public_Static_List_1_T_List_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	public Extensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
