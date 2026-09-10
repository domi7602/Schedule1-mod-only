using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class ComponentExt : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_AddChildGameObject_Public_Static_T_Component_String_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_AddChildGameObject_Public_Static_T_Component_String_0, Il2CppClassPointerStore<ComponentExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_DuplicateGameObject_Public_Static_T_Component_Transform_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_DuplicateGameObject_Public_Static_T_Component_Transform_0, Il2CppClassPointerStore<ComponentExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_DuplicateGameObject_Public_Static_T_Component_Transform_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_DuplicateGameObject_Public_Static_T_Component_Transform_Boolean_0, Il2CppClassPointerStore<ComponentExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_StripComponents_Public_Static_Void_Component_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddChildGameObject_Public_Static_GameObject_Component_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddChildGameObject_Public_Static_T_Component_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DuplicateGameObject_Public_Static_T_Component_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DuplicateGameObject_Public_Static_T_Component_Transform_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DuplicateGameObject_Public_Static_Component_Component_Transform_Boolean_0;

	static ComponentExt()
	{
		Il2CppClassPointerStore<ComponentExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "ComponentExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr);
		NativeMethodInfoPtr_StripComponents_Public_Static_Void_Component_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr, 100664374);
		NativeMethodInfoPtr_AddChildGameObject_Public_Static_GameObject_Component_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr, 100664375);
		NativeMethodInfoPtr_AddChildGameObject_Public_Static_T_Component_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr, 100664376);
		NativeMethodInfoPtr_DuplicateGameObject_Public_Static_T_Component_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr, 100664377);
		NativeMethodInfoPtr_DuplicateGameObject_Public_Static_T_Component_Transform_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr, 100664378);
		NativeMethodInfoPtr_DuplicateGameObject_Public_Static_Component_Component_Transform_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ComponentExt>.NativeClassPtr, 100664379);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 33921, RefRangeEnd = 33923, XrefRangeStart = 33907, XrefRangeEnd = 33921, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void StripComponents(this Component c, [Optional] Il2CppReferenceArray<Il2CppSystem.Type> toKeep)
	{
		if (toKeep == null)
		{
			toKeep = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)c);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toKeep);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StripComponents_Public_Static_Void_Component_Il2CppReferenceArray_1_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33923, XrefRangeEnd = 33930, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static GameObject AddChildGameObject(this Component c, string name)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)c);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddChildGameObject_Public_Static_GameObject_Component_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33930, XrefRangeEnd = 33940, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T AddChildGameObject<T>(this Component c, string name) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)c);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_AddChildGameObject_Public_Static_T_Component_String_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 33967, RefRangeEnd = 33968, XrefRangeStart = 33940, XrefRangeEnd = 33967, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T DuplicateGameObject<T>(this Component source, Transform newParent) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newParent);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_DuplicateGameObject_Public_Static_T_Component_Transform_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33968, XrefRangeEnd = 34004, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T DuplicateGameObject<T>(this Component source, Transform newParent, bool keepPrefabConnection) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newParent);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &keepPrefabConnection;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_DuplicateGameObject_Public_Static_T_Component_Transform_Boolean_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34004, XrefRangeEnd = 34040, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Component DuplicateGameObject(this Component source, Transform newParent, bool keepPrefabConnection = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newParent);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &keepPrefabConnection;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DuplicateGameObject_Public_Static_Component_Component_Transform_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Component>(intPtr) : null;
	}

	public static void StripComponents(Component c, params Il2CppSystem.Type[] toKeep)
	{
		c.StripComponents(new Il2CppReferenceArray<Il2CppSystem.Type>(toKeep));
	}

	public ComponentExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
