using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class ObjectExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Destroy_Public_Static_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Destroy_Public_Static_Boolean_Object_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToDumpString_Public_Static_String_Object_0;

	static ObjectExt()
	{
		Il2CppClassPointerStore<ObjectExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "ObjectExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectExt>.NativeClassPtr);
		NativeMethodInfoPtr_Destroy_Public_Static_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectExt>.NativeClassPtr, 100664352);
		NativeMethodInfoPtr_Destroy_Public_Static_Boolean_Object_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectExt>.NativeClassPtr, 100664353);
		NativeMethodInfoPtr_ToDumpString_Public_Static_String_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectExt>.NativeClassPtr, 100664354);
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 33768, RefRangeEnd = 33791, XrefRangeStart = 33762, XrefRangeEnd = 33768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Destroy(this UnityEngine.Object @object)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Destroy_Public_Static_Boolean_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 33768, RefRangeEnd = 33791, XrefRangeStart = 33768, XrefRangeEnd = 33791, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Destroy(this UnityEngine.Object @object, bool isUndoable, bool doPrefabCheck)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isUndoable;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &doPrefabCheck;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Destroy_Public_Static_Boolean_Object_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33791, XrefRangeEnd = 33796, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ToDumpString(this Il2CppSystem.Object o)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToDumpString_Public_Static_String_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public ObjectExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
