using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.DevUtilities;

public static class ListExtensions : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_Shuffle_Public_Static_Void_IList_1_T_Int32_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Shuffle_Public_Static_Void_IList_1_T_Int32_0, Il2CppClassPointerStore<ListExtensions>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_Shuffle_Public_Static_Void_IList_1_T_Int32_0;

	static ListExtensions()
	{
		Il2CppClassPointerStore<ListExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "ListExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ListExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_Shuffle_Public_Static_Void_IList_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ListExtensions>.NativeClassPtr, 100675070);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 194696, RefRangeEnd = 194699, XrefRangeStart = 194679, XrefRangeEnd = 194696, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Shuffle<T>(this IList<T> list, int seed = -1)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &seed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_Shuffle_Public_Static_Void_IList_1_T_Int32_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ListExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
