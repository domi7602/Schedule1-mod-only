using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppMTAssets.EasyMeshCombiner;

public class MTAssetsMathematics : MonoBehaviour
{
	private sealed class MethodInfoStoreGeneric_RandomizeThisList_Public_Static_List_1_T_List_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_RandomizeThisList_Public_Static_List_1_T_List_1_T_0, Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_RandomizeThisList_Public_Static_List_1_T_List_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHalfPositionBetweenTwoPoints_Public_Static_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static MTAssetsMathematics()
	{
		Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "MTAssets.EasyMeshCombiner", "MTAssetsMathematics");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr);
		NativeMethodInfoPtr_RandomizeThisList_Public_Static_List_1_T_List_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr, 100663844);
		NativeMethodInfoPtr_GetHalfPositionBetweenTwoPoints_Public_Static_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr, 100663845);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr, 100663846);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29754, XrefRangeEnd = 29763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<T> RandomizeThisList<T>(List<T> list)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_RandomizeThisList_Public_Static_List_1_T_List_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe static Vector3 GetHalfPositionBetweenTwoPoints(Vector3 pointA, Vector3 pointB)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&pointA);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointB;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHalfPositionBetweenTwoPoints_Public_Static_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MTAssetsMathematics()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MTAssetsMathematics>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MTAssetsMathematics(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
