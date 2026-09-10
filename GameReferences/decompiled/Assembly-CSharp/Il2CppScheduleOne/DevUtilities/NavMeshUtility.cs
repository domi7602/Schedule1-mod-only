using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Management;
using Il2CppScheduleOne.NPCs;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Il2CppScheduleOne.DevUtilities;

public static class NavMeshUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SAMPLE_MAX_DISTANCE;

	private static readonly System.IntPtr NativeFieldInfoPtr_SAMPLE_CACHE_MAX_DIST;

	private static readonly System.IntPtr NativeFieldInfoPtr_SAMPLE_CACHE_MAX_SQR_DIST;

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_CACHE_SIZE;

	private static readonly System.IntPtr NativeFieldInfoPtr_SampleCache;

	private static readonly System.IntPtr NativeFieldInfoPtr_sampleCacheKeys;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPathLength_Public_Static_Single_NavMeshPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetReachableAccessPoint_Public_Static_Transform_ITransitEntity_NPC_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsAtTransitEntity_Public_Static_Boolean_ITransitEntity_NPC_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNavMeshAgentID_Public_Static_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SamplePosition_Public_Static_Boolean_Vector3_byref_NavMeshHit_Single_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CacheSampleResult_Private_Static_Void_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Quantize_Private_Static_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearCache_Public_Static_Void_0;

	public unsafe static float SAMPLE_MAX_DISTANCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SAMPLE_MAX_DISTANCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SAMPLE_MAX_DISTANCE, (void*)(&num));
		}
	}

	public unsafe static float SAMPLE_CACHE_MAX_DIST
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SAMPLE_CACHE_MAX_DIST, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SAMPLE_CACHE_MAX_DIST, (void*)(&num));
		}
	}

	public unsafe static float SAMPLE_CACHE_MAX_SQR_DIST
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SAMPLE_CACHE_MAX_SQR_DIST, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SAMPLE_CACHE_MAX_SQR_DIST, (void*)(&num));
		}
	}

	public unsafe static float MAX_CACHE_SIZE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_CACHE_SIZE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_CACHE_SIZE, (void*)(&num));
		}
	}

	public unsafe static Dictionary<Vector3, Vector3> SampleCache
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SampleCache, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Vector3, Vector3>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SampleCache, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static List<Vector3> sampleCacheKeys
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_sampleCacheKeys, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_sampleCacheKeys, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static NavMeshUtility()
	{
		Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "NavMeshUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr);
		NativeFieldInfoPtr_SAMPLE_MAX_DISTANCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, "SAMPLE_MAX_DISTANCE");
		NativeFieldInfoPtr_SAMPLE_CACHE_MAX_DIST = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, "SAMPLE_CACHE_MAX_DIST");
		NativeFieldInfoPtr_SAMPLE_CACHE_MAX_SQR_DIST = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, "SAMPLE_CACHE_MAX_SQR_DIST");
		NativeFieldInfoPtr_MAX_CACHE_SIZE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, "MAX_CACHE_SIZE");
		NativeFieldInfoPtr_SampleCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, "SampleCache");
		NativeFieldInfoPtr_sampleCacheKeys = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, "sampleCacheKeys");
		NativeMethodInfoPtr_GetPathLength_Public_Static_Single_NavMeshPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675097);
		NativeMethodInfoPtr_GetReachableAccessPoint_Public_Static_Transform_ITransitEntity_NPC_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675098);
		NativeMethodInfoPtr_IsAtTransitEntity_Public_Static_Boolean_ITransitEntity_NPC_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675099);
		NativeMethodInfoPtr_GetNavMeshAgentID_Public_Static_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675100);
		NativeMethodInfoPtr_SamplePosition_Public_Static_Boolean_Vector3_byref_NavMeshHit_Single_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675101);
		NativeMethodInfoPtr_CacheSampleResult_Private_Static_Void_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675102);
		NativeMethodInfoPtr_Quantize_Private_Static_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675103);
		NativeMethodInfoPtr_ClearCache_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshUtility>.NativeClassPtr, 100675104);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194852, XrefRangeEnd = 194861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetPathLength(NavMeshPath path)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPathLength_Public_Static_Single_NavMeshPath_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(26)]
	[CachedScanResults(RefRangeStart = 194916, RefRangeEnd = 194942, XrefRangeStart = 194861, XrefRangeEnd = 194916, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Transform GetReachableAccessPoint(ITransitEntity entity, NPC npc)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entity);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)npc);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetReachableAccessPoint_Public_Static_Transform_ITransitEntity_NPC_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 194963, RefRangeEnd = 194968, XrefRangeStart = 194942, XrefRangeEnd = 194963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsAtTransitEntity(ITransitEntity entity, NPC npc, float distanceThreshold = 0.4f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entity);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)npc);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distanceThreshold;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsAtTransitEntity_Public_Static_Boolean_ITransitEntity_NPC_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 194974, RefRangeEnd = 194977, XrefRangeStart = 194968, XrefRangeEnd = 194974, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetNavMeshAgentID(string name)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNavMeshAgentID_Public_Static_Int32_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 195003, RefRangeEnd = 195016, XrefRangeStart = 194977, XrefRangeEnd = 195003, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask, bool useCache = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&sourcePosition);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hit);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxDistance;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &areaMask;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &useCache;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SamplePosition_Public_Static_Boolean_Vector3_byref_NavMeshHit_Single_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 195065, RefRangeEnd = 195066, XrefRangeStart = 195016, XrefRangeEnd = 195065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CacheSampleResult(Vector3 sourcePosition, Vector3 hitPosition)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&sourcePosition);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &hitPosition;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CacheSampleResult_Private_Static_Void_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 195066, XrefRangeEnd = 195069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Quantize(Vector3 position, float precision = 0.1f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&position);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &precision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Quantize_Private_Static_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 195078, RefRangeEnd = 195079, XrefRangeStart = 195069, XrefRangeEnd = 195078, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ClearCache()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearCache_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NavMeshUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
