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

public static class GameObjectExt : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_UndoableAddComponent_Public_Static_T_GameObject_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_UndoableAddComponent_Public_Static_T_GameObject_0, Il2CppClassPointerStore<GameObjectExt>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_DuplicateGameObject_Public_Static_GameObject_GameObject_Transform_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StripComponents_Public_Static_Void_GameObject_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UndoableAddComponent_Public_Static_T_GameObject_0;

	static GameObjectExt()
	{
		Il2CppClassPointerStore<GameObjectExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "GameObjectExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameObjectExt>.NativeClassPtr);
		NativeMethodInfoPtr_DuplicateGameObject_Public_Static_GameObject_GameObject_Transform_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExt>.NativeClassPtr, 100664371);
		NativeMethodInfoPtr_StripComponents_Public_Static_Void_GameObject_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExt>.NativeClassPtr, 100664372);
		NativeMethodInfoPtr_UndoableAddComponent_Public_Static_T_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectExt>.NativeClassPtr, 100664373);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 33860, RefRangeEnd = 33862, XrefRangeStart = 33847, XrefRangeEnd = 33860, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static GameObject DuplicateGameObject(this GameObject source, Transform newParent, bool keepPrefabReference = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newParent);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &keepPrefabReference;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DuplicateGameObject_Public_Static_GameObject_GameObject_Transform_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 33895, RefRangeEnd = 33896, XrefRangeStart = 33862, XrefRangeEnd = 33895, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void StripComponents(this GameObject go, [Optional] Il2CppReferenceArray<Il2CppSystem.Type> toKeep)
	{
		if (toKeep == null)
		{
			toKeep = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)go);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toKeep);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StripComponents_Public_Static_Void_GameObject_Il2CppReferenceArray_1_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 33906, RefRangeEnd = 33907, XrefRangeStart = 33896, XrefRangeEnd = 33906, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T UndoableAddComponent<T>(this GameObject gameObject) where T : Component
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_UndoableAddComponent_Public_Static_T_GameObject_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	public static void StripComponents(GameObject go, params Il2CppSystem.Type[] toKeep)
	{
		go.StripComponents(new Il2CppReferenceArray<Il2CppSystem.Type>(toKeep));
	}

	public GameObjectExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
