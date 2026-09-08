using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Il2CppScheduleOne.NPCs;

public class NPCPathCache : Il2CppSystem.Object
{
	[System.Serializable]
	public class PathCache : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Start;

		private static readonly System.IntPtr NativeFieldInfoPtr_End;

		private static readonly System.IntPtr NativeFieldInfoPtr_Path;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_NavMeshPath_0;

		public unsafe Vector3 Start
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start)) = vector;
			}
		}

		public unsafe Vector3 End
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End)) = vector;
			}
		}

		public unsafe NavMeshPath Path
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Path);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NavMeshPath>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Path)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)navMeshPath));
			}
		}

		static PathCache()
		{
			Il2CppClassPointerStore<PathCache>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, "PathCache");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathCache>.NativeClassPtr);
			NativeFieldInfoPtr_Start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathCache>.NativeClassPtr, "Start");
			NativeFieldInfoPtr_End = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathCache>.NativeClassPtr, "End");
			NativeFieldInfoPtr_Path = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathCache>.NativeClassPtr, "Path");
			NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_NavMeshPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathCache>.NativeClassPtr, 100682596);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 270669, XrefRangeEnd = 270671, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe PathCache(Vector3 start, Vector3 end, NavMeshPath path)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathCache>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&start);
			*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &end;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_NavMeshPath_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public PathCache(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__Paths_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Paths_Public_get_List_1_PathCache_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Paths_Private_set_Void_List_1_PathCache_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPath_Public_NavMeshPath_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddPath_Public_Void_Vector3_Vector3_NavMeshPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<PathCache> _Paths_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Paths_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PathCache>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Paths_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<PathCache> Paths
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Paths_Public_get_List_1_PathCache_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PathCache>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29107, RefRangeEnd = 29109, XrefRangeStart = 29107, XrefRangeEnd = 29109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Paths_Private_set_Void_List_1_PathCache_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static NPCPathCache()
	{
		Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs", "NPCPathCache");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr);
		NativeFieldInfoPtr__Paths_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, "<Paths>k__BackingField");
		NativeMethodInfoPtr_get_Paths_Public_get_List_1_PathCache_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, 100682591);
		NativeMethodInfoPtr_set_Paths_Private_set_Void_List_1_PathCache_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, 100682592);
		NativeMethodInfoPtr_GetPath_Public_NavMeshPath_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, 100682593);
		NativeMethodInfoPtr_AddPath_Public_Void_Vector3_Vector3_NavMeshPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, 100682594);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr, 100682595);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 270681, RefRangeEnd = 270682, XrefRangeStart = 270671, XrefRangeEnd = 270681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NavMeshPath GetPath(Vector3 start, Vector3 end, float sqrMaxDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&start);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &end;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sqrMaxDistance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPath_Public_NavMeshPath_Vector3_Vector3_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NavMeshPath>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 270682, XrefRangeEnd = 270693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddPath(Vector3 start, Vector3 end, NavMeshPath path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&start);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &end;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddPath_Public_Void_Vector3_Vector3_NavMeshPath_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 270693, XrefRangeEnd = 270701, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NPCPathCache()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NPCPathCache>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NPCPathCache(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
