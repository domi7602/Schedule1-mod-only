using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class IEnumerableExt : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_ForEach_Public_Static_Void_IEnumerable_1_T_Action_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ForEach_Public_Static_Void_IEnumerable_1_T_Action_1_T_0, Il2CppClassPointerStore<IEnumerableExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_ForEach_Public_Static_Void_IEnumerable_1_T_Action_1_T_0;

	static IEnumerableExt()
	{
		Il2CppClassPointerStore<IEnumerableExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "IEnumerableExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IEnumerableExt>.NativeClassPtr);
		NativeMethodInfoPtr_ForEach_Public_Static_Void_IEnumerable_1_T_Action_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IEnumerableExt>.NativeClassPtr, 100664398);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34170, RefRangeEnd = 34172, XrefRangeStart = 34154, XrefRangeEnd = 34170, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ForEach<T>(this IEnumerable<T> ie, Il2CppSystem.Action<T> action)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ie);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ForEach_Public_Static_Void_IEnumerable_1_T_Action_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IEnumerableExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
