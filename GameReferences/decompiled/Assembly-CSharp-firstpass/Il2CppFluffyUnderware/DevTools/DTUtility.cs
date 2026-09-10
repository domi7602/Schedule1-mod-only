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

namespace Il2CppFluffyUnderware.DevTools;

public static class DTUtility : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_SetPlayerPrefs_Public_Static_Void_String_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_SetPlayerPrefs_Public_Static_Void_String_T_0, Il2CppClassPointerStore<DTUtility>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetPlayerPrefs_Public_Static_T_String_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetPlayerPrefs_Public_Static_T_String_T_0, Il2CppClassPointerStore<DTUtility>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_CreateGameObject_Public_Static_T_Transform_String_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_CreateGameObject_Public_Static_T_Transform_String_0, Il2CppClassPointerStore<DTUtility>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_HelpUrlBase;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDefaultMaterial_Public_Static_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsEditorStateChange_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHandleSize_Public_Static_Single_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHandleSize_Public_Static_Single_Vector3_Camera_Single_Single_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPlayerPrefs_Public_Static_Void_String_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPlayerPrefs_Public_Static_T_String_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RandomSign_Public_Static_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHelpUrl_Public_Static_String_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHelpUrl_Public_Static_String_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCenterPosition_Public_Static_Vector3_Vector3_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CreateGameObject_Public_Static_T_Transform_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsInEditMode_Public_Static_get_Boolean_0;

	public unsafe static string HelpUrlBase
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HelpUrlBase, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HelpUrlBase, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static bool IsEditorStateChange
	{
		[CallerCount(170)]
		[CachedScanResults(RefRangeStart = 31151, RefRangeEnd = 31321, XrefRangeStart = 31151, XrefRangeEnd = 31151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsEditorStateChange_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static bool IsInEditMode
	{
		[CallerCount(170)]
		[CachedScanResults(RefRangeStart = 31151, RefRangeEnd = 31321, XrefRangeStart = 31151, XrefRangeEnd = 31321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsInEditMode_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static DTUtility()
	{
		Il2CppClassPointerStore<DTUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTUtility>.NativeClassPtr);
		NativeFieldInfoPtr_HelpUrlBase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, "HelpUrlBase");
		NativeMethodInfoPtr_GetDefaultMaterial_Public_Static_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663973);
		NativeMethodInfoPtr_get_IsEditorStateChange_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663974);
		NativeMethodInfoPtr_GetHandleSize_Public_Static_Single_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663975);
		NativeMethodInfoPtr_GetHandleSize_Public_Static_Single_Vector3_Camera_Single_Single_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663976);
		NativeMethodInfoPtr_SetPlayerPrefs_Public_Static_Void_String_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663977);
		NativeMethodInfoPtr_GetPlayerPrefs_Public_Static_T_String_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663978);
		NativeMethodInfoPtr_RandomSign_Public_Static_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663979);
		NativeMethodInfoPtr_GetHelpUrl_Public_Static_String_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663980);
		NativeMethodInfoPtr_GetHelpUrl_Public_Static_String_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663981);
		NativeMethodInfoPtr_GetCenterPosition_Public_Static_Vector3_Vector3_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663982);
		NativeMethodInfoPtr_CreateGameObject_Public_Static_T_Transform_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663983);
		NativeMethodInfoPtr_get_IsInEditMode_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTUtility>.NativeClassPtr, 100663984);
	}

	[CallerCount(73)]
	[CachedScanResults(RefRangeStart = 31078, RefRangeEnd = 31151, XrefRangeStart = 31078, XrefRangeEnd = 31078, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material GetDefaultMaterial()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDefaultMaterial_Public_Static_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31321, XrefRangeEnd = 31326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetHandleSize(Vector3 position)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&position);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHandleSize_Public_Static_Single_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31326, XrefRangeEnd = 31332, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetHandleSize(Vector3 position, Camera camera, float cameraCenterWidth, float cameraCenterHeight, Vector3 cameraPosition, Vector3 cameraZDirection, Vector3 cameraXDirection)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&position);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraCenterWidth;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraCenterHeight;
		*(Vector3**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraPosition;
		*(Vector3**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraZDirection;
		*(Vector3**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraXDirection;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHandleSize_Public_Static_Single_Vector3_Camera_Single_Single_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 31432, RefRangeEnd = 31434, XrefRangeStart = 31332, XrefRangeEnd = 31432, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetPlayerPrefs<T>(string key, T value)
	{
		//IL_006c->IL006f: Incompatible stack types: I vs Ref
		//IL_004b->IL006f: Incompatible stack types: I vs Ref
		//IL_0058->IL006f: Incompatible stack types: I vs Ref
		//IL_005f->IL006f: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(key);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = value;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref value;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_SetPlayerPrefs_Public_Static_Void_String_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 31540, RefRangeEnd = 31542, XrefRangeStart = 31434, XrefRangeEnd = 31540, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetPlayerPrefs<T>(string key, T defaultValue)
	{
		//IL_006c->IL006f: Incompatible stack types: I vs Ref
		//IL_004b->IL006f: Incompatible stack types: I vs Ref
		//IL_0058->IL006f: Incompatible stack types: I vs Ref
		//IL_005f->IL006f: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(key);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = defaultValue;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref defaultValue;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetPlayerPrefs_Public_Static_T_String_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31542, XrefRangeEnd = 31543, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float RandomSign()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RandomSign_Public_Static_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31543, XrefRangeEnd = 31545, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetHelpUrl(Il2CppSystem.Object forClass)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)forClass);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHelpUrl_Public_Static_String_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31545, XrefRangeEnd = 31557, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetHelpUrl(Il2CppSystem.Type classType)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)classType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHelpUrl_Public_Static_String_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31557, XrefRangeEnd = 31558, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetCenterPosition(Vector3 fallback, [Optional] Il2CppStructArray<Vector3> vectors)
	{
		if (vectors == null)
		{
			vectors = new Il2CppStructArray<Vector3>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&fallback);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vectors);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCenterPosition_Public_Static_Vector3_Vector3_Il2CppStructArray_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31558, XrefRangeEnd = 31575, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T CreateGameObject<T>(Transform parent, string name) where T : MonoBehaviour
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_CreateGameObject_Public_Static_T_Transform_String_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	public static Vector3 GetCenterPosition(Vector3 fallback, params Vector3[] vectors)
	{
		return GetCenterPosition(fallback, new Il2CppStructArray<Vector3>(vectors));
	}

	public DTUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
