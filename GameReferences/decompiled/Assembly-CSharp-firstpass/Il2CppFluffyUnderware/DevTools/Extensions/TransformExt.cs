using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class TransformExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_UndoableSetParent_Public_Static_Void_Transform_Transform_Boolean_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteChildren_Public_Static_Void_Transform_Boolean_Boolean_0;

	static TransformExt()
	{
		Il2CppClassPointerStore<TransformExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "TransformExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformExt>.NativeClassPtr);
		NativeMethodInfoPtr_UndoableSetParent_Public_Static_Void_Transform_Transform_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformExt>.NativeClassPtr, 100664349);
		NativeMethodInfoPtr_DeleteChildren_Public_Static_Void_Transform_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformExt>.NativeClassPtr, 100664350);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 33706, RefRangeEnd = 33710, XrefRangeStart = 33704, XrefRangeEnd = 33706, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void UndoableSetParent(this Transform child, Transform newParent, bool worldPositionStays, string undoOperationName)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)child);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newParent);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &worldPositionStays;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(undoOperationName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UndoableSetParent_Public_Static_Void_Transform_Transform_Boolean_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 33749, RefRangeEnd = 33751, XrefRangeStart = 33710, XrefRangeEnd = 33749, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DeleteChildren(this Transform transform, bool isUndoable, bool doPrefabCheck)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isUndoable;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &doPrefabCheck;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteChildren_Public_Static_Void_Transform_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TransformExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
