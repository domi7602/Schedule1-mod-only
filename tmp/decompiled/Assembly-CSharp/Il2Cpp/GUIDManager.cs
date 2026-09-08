using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2Cpp;

public static class GUIDManager : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_GetObject_Public_Static_T_Guid_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetObject_Public_Static_T_Guid_0, Il2CppClassPointerStore<GUIDManager>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_registeredGUIDs;

	private static readonly System.IntPtr NativeFieldInfoPtr_guidToObject;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterObject_Public_Static_Void_IGUIDRegisterable_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeregisterObject_Public_Static_Void_IGUIDRegisterable_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetObject_Public_Static_T_Guid_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetObjectType_Public_Static_Type_Guid_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateUniqueGUID_Public_Static_Guid_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsGUIDAlreadyRegistered_Public_Static_Boolean_Guid_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsGUIDValid_Public_Static_Boolean_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Static_Void_0;

	public unsafe static List<Il2CppSystem.Guid> registeredGUIDs
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_registeredGUIDs, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Guid>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_registeredGUIDs, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static Dictionary<Il2CppSystem.Guid, Il2CppSystem.Object> guidToObject
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_guidToObject, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Il2CppSystem.Guid, Il2CppSystem.Object>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_guidToObject, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	static GUIDManager()
	{
		Il2CppClassPointerStore<GUIDManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "GUIDManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr);
		NativeFieldInfoPtr_registeredGUIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, "registeredGUIDs");
		NativeFieldInfoPtr_guidToObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, "guidToObject");
		NativeMethodInfoPtr_RegisterObject_Public_Static_Void_IGUIDRegisterable_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663401);
		NativeMethodInfoPtr_DeregisterObject_Public_Static_Void_IGUIDRegisterable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663402);
		NativeMethodInfoPtr_GetObject_Public_Static_T_Guid_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663403);
		NativeMethodInfoPtr_GetObjectType_Public_Static_Type_Guid_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663404);
		NativeMethodInfoPtr_GenerateUniqueGUID_Public_Static_Guid_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663405);
		NativeMethodInfoPtr_IsGUIDAlreadyRegistered_Public_Static_Boolean_Guid_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663406);
		NativeMethodInfoPtr_IsGUIDValid_Public_Static_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663407);
		NativeMethodInfoPtr_Clear_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUIDManager>.NativeClassPtr, 100663408);
	}

	[CallerCount(37)]
	[CachedScanResults(RefRangeStart = 65884, RefRangeEnd = 65921, XrefRangeStart = 65835, XrefRangeEnd = 65884, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RegisterObject(IGUIDRegisterable obj, GameObject go = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)go);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterObject_Public_Static_Void_IGUIDRegisterable_GameObject_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 65937, RefRangeEnd = 65938, XrefRangeStart = 65921, XrefRangeEnd = 65937, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DeregisterObject(IGUIDRegisterable obj)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeregisterObject_Public_Static_Void_IGUIDRegisterable_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(69)]
	[CachedScanResults(RefRangeStart = 65963, RefRangeEnd = 66032, XrefRangeStart = 65938, XrefRangeEnd = 65963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetObject<T>(Il2CppSystem.Guid guid)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&guid);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetObject_Public_Static_T_Guid_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66032, XrefRangeEnd = 66039, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Type GetObjectType(Il2CppSystem.Guid guid)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&guid);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetObjectType_Public_Static_Type_Guid_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 66047, RefRangeEnd = 66060, XrefRangeStart = 66039, XrefRangeEnd = 66047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Guid GenerateUniqueGUID()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateUniqueGUID_Public_Static_Guid_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Il2CppSystem.Guid*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(25)]
	[CachedScanResults(RefRangeStart = 66067, RefRangeEnd = 66092, XrefRangeStart = 66060, XrefRangeEnd = 66067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsGUIDAlreadyRegistered(Il2CppSystem.Guid guid)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&guid);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsGUIDAlreadyRegistered_Public_Static_Boolean_Guid_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14)]
	[CachedScanResults(RefRangeStart = 66099, RefRangeEnd = 66113, XrefRangeStart = 66092, XrefRangeEnd = 66099, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsGUIDValid(string guid)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(guid);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsGUIDValid_Public_Static_Boolean_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 66129, RefRangeEnd = 66130, XrefRangeStart = 66113, XrefRangeEnd = 66129, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Clear()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GUIDManager(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
