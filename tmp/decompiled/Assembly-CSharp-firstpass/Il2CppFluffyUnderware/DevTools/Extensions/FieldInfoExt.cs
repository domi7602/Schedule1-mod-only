using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class FieldInfoExt : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_GetCustomAttribute_Public_Static_T_FieldInfo_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetCustomAttribute_Public_Static_T_FieldInfo_0, Il2CppClassPointerStore<FieldInfoExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCustomAttribute_Public_Static_T_FieldInfo_0;

	static FieldInfoExt()
	{
		Il2CppClassPointerStore<FieldInfoExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "FieldInfoExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FieldInfoExt>.NativeClassPtr);
		NativeMethodInfoPtr_GetCustomAttribute_Public_Static_T_FieldInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FieldInfoExt>.NativeClassPtr, 100664428);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 34411, RefRangeEnd = 34416, XrefRangeStart = 34411, XrefRangeEnd = 34416, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetCustomAttribute<T>(this FieldInfo field) where T : Il2CppSystem.Attribute
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)field);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetCustomAttribute_Public_Static_T_FieldInfo_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	public FieldInfoExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
