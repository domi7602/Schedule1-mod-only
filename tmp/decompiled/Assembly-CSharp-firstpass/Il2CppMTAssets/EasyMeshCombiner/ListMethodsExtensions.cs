using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppMTAssets.EasyMeshCombiner;

public static class ListMethodsExtensions : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_RemoveAllNullItems_Public_Static_Void_List_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_RemoveAllNullItems_Public_Static_Void_List_1_T_0, Il2CppClassPointerStore<ListMethodsExtensions>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveAllNullItems_Public_Static_Void_List_1_T_0;

	static ListMethodsExtensions()
	{
		Il2CppClassPointerStore<ListMethodsExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "MTAssets.EasyMeshCombiner", "ListMethodsExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ListMethodsExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_RemoveAllNullItems_Public_Static_Void_List_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ListMethodsExtensions>.NativeClassPtr, 100663847);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29763, XrefRangeEnd = 29766, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RemoveAllNullItems<T>(this List<T> list)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_RemoveAllNullItems_Public_Static_Void_List_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ListMethodsExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
