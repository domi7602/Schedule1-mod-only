using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Il2CppMTAssets.EasyMeshCombiner;

public class RuntimeMeshCombiner : MonoBehaviour
{
	public class GameObjectWithMesh : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_gameObject;

		private static readonly System.IntPtr NativeFieldInfoPtr_meshFilter;

		private static readonly System.IntPtr NativeFieldInfoPtr_meshRenderer;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_GameObject_MeshFilter_MeshRenderer_0;

		public unsafe GameObject gameObject
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_gameObject);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_gameObject)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
			}
		}

		public unsafe MeshFilter meshFilter
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter));
			}
		}

		public unsafe MeshRenderer meshRenderer
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
			}
		}

		static GameObjectWithMesh()
		{
			Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "GameObjectWithMesh");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr);
			NativeFieldInfoPtr_gameObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr, "gameObject");
			NativeFieldInfoPtr_meshFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr, "meshFilter");
			NativeFieldInfoPtr_meshRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr, "meshRenderer");
			NativeMethodInfoPtr__ctor_Public_Void_GameObject_MeshFilter_MeshRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr, 100663856);
		}

		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29770, RefRangeEnd = 29775, XrefRangeStart = 29766, XrefRangeEnd = 29770, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe GameObjectWithMesh(GameObject gameObject, MeshFilter meshFilter, MeshRenderer meshRenderer)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameObjectWithMesh>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_GameObject_MeshFilter_MeshRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public GameObjectWithMesh(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class OriginalGameObjectWithMesh : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_gameObject;

		private static readonly System.IntPtr NativeFieldInfoPtr_originalGoState;

		private static readonly System.IntPtr NativeFieldInfoPtr_meshRenderer;

		private static readonly System.IntPtr NativeFieldInfoPtr_originalMrState;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_GameObject_Boolean_MeshRenderer_Boolean_0;

		public unsafe GameObject gameObject
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_gameObject);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_gameObject)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
			}
		}

		public unsafe bool originalGoState
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalGoState);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalGoState)) = flag;
			}
		}

		public unsafe MeshRenderer meshRenderer
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
			}
		}

		public unsafe bool originalMrState
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalMrState);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalMrState)) = flag;
			}
		}

		static OriginalGameObjectWithMesh()
		{
			Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "OriginalGameObjectWithMesh");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr);
			NativeFieldInfoPtr_gameObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr, "gameObject");
			NativeFieldInfoPtr_originalGoState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr, "originalGoState");
			NativeFieldInfoPtr_meshRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr, "meshRenderer");
			NativeFieldInfoPtr_originalMrState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr, "originalMrState");
			NativeMethodInfoPtr__ctor_Public_Void_GameObject_Boolean_MeshRenderer_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr, 100663857);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29775, XrefRangeEnd = 29778, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OriginalGameObjectWithMesh(GameObject gameObject, bool originalGoState, MeshRenderer meshRenderer, bool originalMrState)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OriginalGameObjectWithMesh>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
			*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &originalGoState;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer);
			*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &originalMrState;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_GameObject_Boolean_MeshRenderer_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public OriginalGameObjectWithMesh(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class SubMeshToCombine : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_transform;

		private static readonly System.IntPtr NativeFieldInfoPtr_meshFilter;

		private static readonly System.IntPtr NativeFieldInfoPtr_meshRenderer;

		private static readonly System.IntPtr NativeFieldInfoPtr_subMeshIndex;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Transform_MeshFilter_MeshRenderer_Int32_0;

		public unsafe Transform transform
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transform);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
			}
		}

		public unsafe MeshFilter meshFilter
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter));
			}
		}

		public unsafe MeshRenderer meshRenderer
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
			}
		}

		public unsafe int subMeshIndex
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_subMeshIndex);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_subMeshIndex)) = num;
			}
		}

		static SubMeshToCombine()
		{
			Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "SubMeshToCombine");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr);
			NativeFieldInfoPtr_transform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr, "transform");
			NativeFieldInfoPtr_meshFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr, "meshFilter");
			NativeFieldInfoPtr_meshRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr, "meshRenderer");
			NativeFieldInfoPtr_subMeshIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr, "subMeshIndex");
			NativeMethodInfoPtr__ctor_Public_Void_Transform_MeshFilter_MeshRenderer_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr, 100663858);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29782, RefRangeEnd = 29784, XrefRangeStart = 29778, XrefRangeEnd = 29782, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SubMeshToCombine(Transform transform, MeshFilter meshFilter, MeshRenderer meshRenderer, int subMeshIndex)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SubMeshToCombine>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer);
			*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &subMeshIndex;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Transform_MeshFilter_MeshRenderer_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SubMeshToCombine(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "CombineOnStart")]
	public enum CombineOnStart
	{
		Disabled,
		OnStart,
		OnAwake
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "AfterMerge")]
	public enum AfterMerge
	{
		DisableOriginalMeshes,
		DeactiveOriginalGameObjects,
		DoNothing
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_VERTICES_FOR_16BITS_MESH;

	private static readonly System.IntPtr NativeFieldInfoPtr_originalPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_originalEulerAngles;

	private static readonly System.IntPtr NativeFieldInfoPtr_originalScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_originalGameObjectsWithMeshToRestore;

	private static readonly System.IntPtr NativeFieldInfoPtr_targetMeshesMerged;

	private static readonly System.IntPtr NativeFieldInfoPtr_afterMerge;

	private static readonly System.IntPtr NativeFieldInfoPtr_addMeshColliderAfter;

	private static readonly System.IntPtr NativeFieldInfoPtr_combineMeshesAtStartUp;

	private static readonly System.IntPtr NativeFieldInfoPtr_combineInChildren;

	private static readonly System.IntPtr NativeFieldInfoPtr_combineInactives;

	private static readonly System.IntPtr NativeFieldInfoPtr_recalculateNormals;

	private static readonly System.IntPtr NativeFieldInfoPtr_recalculateTangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_optimizeResultingMesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_targetMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_showDebugLogs;

	private static readonly System.IntPtr NativeFieldInfoPtr_garbageCollectorAfterUndo;

	private static readonly System.IntPtr NativeFieldInfoPtr_onDoneMerge;

	private static readonly System.IntPtr NativeFieldInfoPtr_onDoneUnmerge;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetValidatedTargetGameObjects_Private_Il2CppReferenceArray_1_GameObjectWithMesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CombineMeshes_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UndoMerge_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_isTargetMeshesMerged_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int MAX_VERTICES_FOR_16BITS_MESH
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MAX_VERTICES_FOR_16BITS_MESH);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MAX_VERTICES_FOR_16BITS_MESH)) = num;
		}
	}

	public unsafe Vector3 originalPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalPosition)) = vector;
		}
	}

	public unsafe Vector3 originalEulerAngles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalEulerAngles);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalEulerAngles)) = vector;
		}
	}

	public unsafe Vector3 originalScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalScale);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalScale)) = vector;
		}
	}

	public unsafe List<OriginalGameObjectWithMesh> originalGameObjectsWithMeshToRestore
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalGameObjectsWithMeshToRestore);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<OriginalGameObjectWithMesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalGameObjectsWithMeshToRestore)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool targetMeshesMerged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetMeshesMerged);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetMeshesMerged)) = flag;
		}
	}

	public unsafe AfterMerge afterMerge
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_afterMerge);
			return *(AfterMerge*)num;
		}
		set
		{
			*(AfterMerge*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_afterMerge)) = afterMerge;
		}
	}

	public unsafe bool addMeshColliderAfter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_addMeshColliderAfter);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_addMeshColliderAfter)) = flag;
		}
	}

	public unsafe CombineOnStart combineMeshesAtStartUp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_combineMeshesAtStartUp);
			return *(CombineOnStart*)num;
		}
		set
		{
			*(CombineOnStart*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_combineMeshesAtStartUp)) = combineOnStart;
		}
	}

	public unsafe bool combineInChildren
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_combineInChildren);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_combineInChildren)) = flag;
		}
	}

	public unsafe bool combineInactives
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_combineInactives);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_combineInactives)) = flag;
		}
	}

	public unsafe bool recalculateNormals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_recalculateNormals);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_recalculateNormals)) = flag;
		}
	}

	public unsafe bool recalculateTangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_recalculateTangents);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_recalculateTangents)) = flag;
		}
	}

	public unsafe bool optimizeResultingMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_optimizeResultingMesh);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_optimizeResultingMesh)) = flag;
		}
	}

	public unsafe List<GameObject> targetMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<GameObject>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool showDebugLogs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_showDebugLogs);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_showDebugLogs)) = flag;
		}
	}

	public unsafe bool garbageCollectorAfterUndo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_garbageCollectorAfterUndo);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_garbageCollectorAfterUndo)) = flag;
		}
	}

	public unsafe UnityEvent onDoneMerge
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onDoneMerge);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onDoneMerge)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent onDoneUnmerge
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onDoneUnmerge);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onDoneUnmerge)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	static RuntimeMeshCombiner()
	{
		Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "MTAssets.EasyMeshCombiner", "RuntimeMeshCombiner");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr);
		NativeFieldInfoPtr_MAX_VERTICES_FOR_16BITS_MESH = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "MAX_VERTICES_FOR_16BITS_MESH");
		NativeFieldInfoPtr_originalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "originalPosition");
		NativeFieldInfoPtr_originalEulerAngles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "originalEulerAngles");
		NativeFieldInfoPtr_originalScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "originalScale");
		NativeFieldInfoPtr_originalGameObjectsWithMeshToRestore = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "originalGameObjectsWithMeshToRestore");
		NativeFieldInfoPtr_targetMeshesMerged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "targetMeshesMerged");
		NativeFieldInfoPtr_afterMerge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "afterMerge");
		NativeFieldInfoPtr_addMeshColliderAfter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "addMeshColliderAfter");
		NativeFieldInfoPtr_combineMeshesAtStartUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "combineMeshesAtStartUp");
		NativeFieldInfoPtr_combineInChildren = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "combineInChildren");
		NativeFieldInfoPtr_combineInactives = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "combineInactives");
		NativeFieldInfoPtr_recalculateNormals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "recalculateNormals");
		NativeFieldInfoPtr_recalculateTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "recalculateTangents");
		NativeFieldInfoPtr_optimizeResultingMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "optimizeResultingMesh");
		NativeFieldInfoPtr_targetMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "targetMeshes");
		NativeFieldInfoPtr_showDebugLogs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "showDebugLogs");
		NativeFieldInfoPtr_garbageCollectorAfterUndo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "garbageCollectorAfterUndo");
		NativeFieldInfoPtr_onDoneMerge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "onDoneMerge");
		NativeFieldInfoPtr_onDoneUnmerge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, "onDoneUnmerge");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663849);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663850);
		NativeMethodInfoPtr_GetValidatedTargetGameObjects_Private_Il2CppReferenceArray_1_GameObjectWithMesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663851);
		NativeMethodInfoPtr_CombineMeshes_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663852);
		NativeMethodInfoPtr_UndoMerge_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663853);
		NativeMethodInfoPtr_isTargetMeshesMerged_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663854);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr, 100663855);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29784, XrefRangeEnd = 29796, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29796, XrefRangeEnd = 29808, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 30067, RefRangeEnd = 30068, XrefRangeStart = 29808, XrefRangeEnd = 30067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<GameObjectWithMesh> GetValidatedTargetGameObjects()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetValidatedTargetGameObjects_Private_Il2CppReferenceArray_1_GameObjectWithMesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<GameObjectWithMesh>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 30346, RefRangeEnd = 30349, XrefRangeStart = 30068, XrefRangeEnd = 30346, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CombineMeshes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CombineMeshes_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 30421, RefRangeEnd = 30422, XrefRangeStart = 30349, XrefRangeEnd = 30421, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool UndoMerge()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UndoMerge_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool isTargetMeshesMerged()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_isTargetMeshesMerged_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30422, XrefRangeEnd = 30443, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RuntimeMeshCombiner()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RuntimeMeshCombiner>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RuntimeMeshCombiner(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
