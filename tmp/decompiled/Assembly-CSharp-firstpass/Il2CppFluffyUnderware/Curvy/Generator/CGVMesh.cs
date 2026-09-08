using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGVMesh : CGBounds
{
	private sealed class MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Clone_Public_Virtual_T_0, Il2CppClassPointerStore<CGVMesh>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_SubMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_sortedVertexIndices;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertexIndicesLock;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertices;

	private static readonly System.IntPtr NativeFieldInfoPtr_uvs;

	private static readonly System.IntPtr NativeFieldInfoPtr_uv2s;

	private static readonly System.IntPtr NativeFieldInfoPtr_normals;

	private static readonly System.IntPtr NativeFieldInfoPtr_tangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasPartialNormals;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasPartialTangents;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vertices_Public_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Vertices_Public_set_Void_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UVs_Public_get_SubArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UVs_Public_set_Void_SubArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UV2s_Public_get_SubArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UV2s_Public_set_Void_SubArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NormalsList_Public_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_NormalsList_Public_set_Void_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TangentsList_Public_get_SubArray_1_Vector4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_TangentsList_Public_set_Void_SubArray_1_Vector4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vertex_Public_get_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Vertex_Public_set_Void_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UV_Public_get_Il2CppStructArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UV_Public_set_Void_Il2CppStructArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UV2_Public_get_Il2CppStructArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UV2_Public_set_Void_Il2CppStructArray_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Normals_Public_get_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Normals_Public_set_Void_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Tangents_Public_get_Il2CppStructArray_1_Vector4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Tangents_Public_set_Void_Il2CppStructArray_1_Vector4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_Virtual_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasUV_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasUV2_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasNormals_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasPartialNormals_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HasPartialNormals_Private_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasTangents_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasPartialTangents_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_HasPartialTangents_Private_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGVolume_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGVolume_IntRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGVMesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGMeshProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Mesh_Il2CppReferenceArray_1_Material_Matrix4x4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_Virtual_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_IntRegion_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_IntRegion_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetSubMeshCount_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSubMesh_Public_Void_CGVSubMesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeVMesh_Public_Void_CGVMesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeVMesh_Public_Void_CGVMesh_Matrix4x4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeVMeshes_Public_Void_List_1_CGVMesh_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeUVsNormalsAndTangents_Private_Void_CGVMesh_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaterialSubMesh_Public_CGVSubMesh_Material_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AsMesh_Public_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToMesh_Public_Void_byref_Mesh_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaterials_Public_Il2CppReferenceArray_1_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RecalculateBounds_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RecalculateUV2_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TRS_Public_Void_Matrix4x4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnVerticesChanged_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCachedSortedVertexIndices_Public_SubArray_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearCachedSortedVertexIndices_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Private_Void_List_1_SubArray_1_Int32_Material_0;

	public unsafe Il2CppReferenceArray<CGVSubMesh> SubMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SubMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CGVSubMesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SubMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppSystem.Nullable<SubArray<int>> sortedVertexIndices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sortedVertexIndices);
			return new Il2CppSystem.Nullable<SubArray<int>>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<SubArray<int>>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sortedVertexIndices), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<SubArray<int>>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe Il2CppSystem.Object vertexIndicesLock
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexIndicesLock);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexIndicesLock)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe SubArray<Vector3> vertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertices);
			return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertices), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe SubArray<Vector2> uvs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uvs);
			return new SubArray<Vector2>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector2>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uvs), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector2>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe SubArray<Vector2> uv2s
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uv2s);
			return new SubArray<Vector2>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector2>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uv2s), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector2>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe SubArray<Vector3> normals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_normals);
			return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_normals), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe SubArray<Vector4> tangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tangents);
			return new SubArray<Vector4>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector4>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tangents), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector4>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe bool hasPartialNormals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasPartialNormals);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasPartialNormals)) = flag;
		}
	}

	public unsafe bool hasPartialTangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasPartialTangents);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasPartialTangents)) = flag;
		}
	}

	public unsafe SubArray<Vector3> Vertices
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vertices_Public_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51297, XrefRangeEnd = 51310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Vertices_Public_set_Void_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SubArray<Vector2> UVs
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UVs_Public_get_SubArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector2>(pointer);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51322, RefRangeEnd = 51324, XrefRangeStart = 51310, XrefRangeEnd = 51322, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UVs_Public_set_Void_SubArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SubArray<Vector2> UV2s
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UV2s_Public_get_SubArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector2>(pointer);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51336, RefRangeEnd = 51338, XrefRangeStart = 51324, XrefRangeEnd = 51336, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UV2s_Public_set_Void_SubArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SubArray<Vector3> NormalsList
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NormalsList_Public_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51350, RefRangeEnd = 51352, XrefRangeStart = 51338, XrefRangeEnd = 51350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_NormalsList_Public_set_Void_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SubArray<Vector4> TangentsList
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TangentsList_Public_get_SubArray_1_Vector4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector4>(pointer);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51364, RefRangeEnd = 51366, XrefRangeStart = 51352, XrefRangeEnd = 51364, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TangentsList_Public_set_Void_SubArray_1_Vector4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector3> Vertex
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51366, XrefRangeEnd = 51376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vertex_Public_get_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51376, XrefRangeEnd = 51391, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Vertex_Public_set_Void_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector2> UV
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51391, XrefRangeEnd = 51401, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UV_Public_get_Il2CppStructArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51401, XrefRangeEnd = 51405, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UV_Public_set_Void_Il2CppStructArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector2> UV2
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51405, XrefRangeEnd = 51415, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UV2_Public_get_Il2CppStructArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51415, XrefRangeEnd = 51419, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UV2_Public_set_Void_Il2CppStructArray_1_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector3> Normals
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51419, XrefRangeEnd = 51429, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Normals_Public_get_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51429, XrefRangeEnd = 51433, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Normals_Public_set_Void_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector4> Tangents
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51433, XrefRangeEnd = 51443, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Tangents_Public_get_Il2CppStructArray_1_Vector4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector4>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51443, XrefRangeEnd = 51447, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Tangents_Public_set_Void_Il2CppStructArray_1_Vector4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override int Count
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 51447, RefRangeEnd = 51448, XrefRangeStart = 51447, XrefRangeEnd = 51447, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Count_Public_Virtual_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool HasUV
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51448, RefRangeEnd = 51450, XrefRangeStart = 51448, XrefRangeEnd = 51448, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasUV_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool HasUV2
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 51450, RefRangeEnd = 51453, XrefRangeStart = 51450, XrefRangeEnd = 51450, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasUV2_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool HasNormals
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51453, RefRangeEnd = 51455, XrefRangeStart = 51453, XrefRangeEnd = 51453, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasNormals_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool HasPartialNormals
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasPartialNormals_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HasPartialNormals_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool HasTangents
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51455, RefRangeEnd = 51457, XrefRangeStart = 51455, XrefRangeEnd = 51455, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasTangents_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool HasPartialTangents
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasPartialTangents_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_HasPartialTangents_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int TriangleCount
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51457, XrefRangeEnd = 51458, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CGVMesh()
	{
		Il2CppClassPointerStore<CGVMesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGVMesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr);
		NativeFieldInfoPtr_SubMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "SubMeshes");
		NativeFieldInfoPtr_sortedVertexIndices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "sortedVertexIndices");
		NativeFieldInfoPtr_vertexIndicesLock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "vertexIndicesLock");
		NativeFieldInfoPtr_vertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "vertices");
		NativeFieldInfoPtr_uvs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "uvs");
		NativeFieldInfoPtr_uv2s = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "uv2s");
		NativeFieldInfoPtr_normals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "normals");
		NativeFieldInfoPtr_tangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "tangents");
		NativeFieldInfoPtr_hasPartialNormals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "hasPartialNormals");
		NativeFieldInfoPtr_hasPartialTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, "hasPartialTangents");
		NativeMethodInfoPtr_get_Vertices_Public_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666201);
		NativeMethodInfoPtr_set_Vertices_Public_set_Void_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666202);
		NativeMethodInfoPtr_get_UVs_Public_get_SubArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666203);
		NativeMethodInfoPtr_set_UVs_Public_set_Void_SubArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666204);
		NativeMethodInfoPtr_get_UV2s_Public_get_SubArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666205);
		NativeMethodInfoPtr_set_UV2s_Public_set_Void_SubArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666206);
		NativeMethodInfoPtr_get_NormalsList_Public_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666207);
		NativeMethodInfoPtr_set_NormalsList_Public_set_Void_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666208);
		NativeMethodInfoPtr_get_TangentsList_Public_get_SubArray_1_Vector4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666209);
		NativeMethodInfoPtr_set_TangentsList_Public_set_Void_SubArray_1_Vector4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666210);
		NativeMethodInfoPtr_get_Vertex_Public_get_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666211);
		NativeMethodInfoPtr_set_Vertex_Public_set_Void_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666212);
		NativeMethodInfoPtr_get_UV_Public_get_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666213);
		NativeMethodInfoPtr_set_UV_Public_set_Void_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666214);
		NativeMethodInfoPtr_get_UV2_Public_get_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666215);
		NativeMethodInfoPtr_set_UV2_Public_set_Void_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666216);
		NativeMethodInfoPtr_get_Normals_Public_get_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666217);
		NativeMethodInfoPtr_set_Normals_Public_set_Void_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666218);
		NativeMethodInfoPtr_get_Tangents_Public_get_Il2CppStructArray_1_Vector4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666219);
		NativeMethodInfoPtr_set_Tangents_Public_set_Void_Il2CppStructArray_1_Vector4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666220);
		NativeMethodInfoPtr_get_Count_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666221);
		NativeMethodInfoPtr_get_HasUV_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666222);
		NativeMethodInfoPtr_get_HasUV2_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666223);
		NativeMethodInfoPtr_get_HasNormals_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666224);
		NativeMethodInfoPtr_get_HasPartialNormals_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666225);
		NativeMethodInfoPtr_set_HasPartialNormals_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666226);
		NativeMethodInfoPtr_get_HasTangents_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666227);
		NativeMethodInfoPtr_get_HasPartialTangents_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666228);
		NativeMethodInfoPtr_set_HasPartialTangents_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666229);
		NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666230);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666231);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666232);
		NativeMethodInfoPtr__ctor_Public_Void_CGVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666233);
		NativeMethodInfoPtr__ctor_Public_Void_CGVolume_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666234);
		NativeMethodInfoPtr__ctor_Public_Void_CGVMesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666235);
		NativeMethodInfoPtr__ctor_Public_Void_CGMeshProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666236);
		NativeMethodInfoPtr__ctor_Public_Void_Mesh_Il2CppReferenceArray_1_Material_Matrix4x4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666237);
		NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666238);
		NativeMethodInfoPtr_Clone_Public_Virtual_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666239);
		NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666240);
		NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_IntRegion_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666241);
		NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_IntRegion_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666242);
		NativeMethodInfoPtr_SetSubMeshCount_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666243);
		NativeMethodInfoPtr_AddSubMesh_Public_Void_CGVSubMesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666244);
		NativeMethodInfoPtr_MergeVMesh_Public_Void_CGVMesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666245);
		NativeMethodInfoPtr_MergeVMesh_Public_Void_CGVMesh_Matrix4x4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666246);
		NativeMethodInfoPtr_MergeVMeshes_Public_Void_List_1_CGVMesh_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666247);
		NativeMethodInfoPtr_MergeUVsNormalsAndTangents_Private_Void_CGVMesh_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666248);
		NativeMethodInfoPtr_GetMaterialSubMesh_Public_CGVSubMesh_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666249);
		NativeMethodInfoPtr_AsMesh_Public_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666250);
		NativeMethodInfoPtr_ToMesh_Public_Void_byref_Mesh_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666251);
		NativeMethodInfoPtr_GetMaterials_Public_Il2CppReferenceArray_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666252);
		NativeMethodInfoPtr_RecalculateBounds_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666253);
		NativeMethodInfoPtr_RecalculateUV2_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666254);
		NativeMethodInfoPtr_TRS_Public_Void_Matrix4x4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666255);
		NativeMethodInfoPtr_OnVerticesChanged_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666256);
		NativeMethodInfoPtr_GetCachedSortedVertexIndices_Public_SubArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666257);
		NativeMethodInfoPtr_ClearCachedSortedVertexIndices_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666258);
		NativeMethodInfoPtr_Method_Private_Void_List_1_SubArray_1_Int32_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr, 100666259);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 51459, RefRangeEnd = 51461, XrefRangeStart = 51458, XrefRangeEnd = 51459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 51545, RefRangeEnd = 51550, XrefRangeStart = 51461, XrefRangeEnd = 51545, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh(int vertexCount, bool addUV = false, bool addUV2 = false, bool addNormals = false, bool addTangents = false)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&vertexCount);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &addUV;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &addUV2;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &addNormals;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &addTangents;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51550, XrefRangeEnd = 51552, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh(CGVolume volume)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGVolume_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51552, XrefRangeEnd = 51557, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh(CGVolume volume, IntRegion subset)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		*(IntRegion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &subset;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGVolume_IntRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 51633, RefRangeEnd = 51636, XrefRangeStart = 51557, XrefRangeEnd = 51633, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh(CGVMesh source)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGVMesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 51638, RefRangeEnd = 51639, XrefRangeStart = 51636, XrefRangeEnd = 51638, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh(CGMeshProperties meshProperties)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshProperties);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGMeshProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 51689, RefRangeEnd = 51691, XrefRangeStart = 51639, XrefRangeEnd = 51689, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVMesh(Mesh source, Il2CppReferenceArray<Material> materials, Matrix4x4 trsMatrix)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGVMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materials);
		*(Matrix4x4**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &trsMatrix;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Mesh_Il2CppReferenceArray_1_Material_Matrix4x4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51691, XrefRangeEnd = 51743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool Dispose(bool disposing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&disposing);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51743, XrefRangeEnd = 51751, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override T Clone<T>()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>.Pointer), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51751, XrefRangeEnd = 51753, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CGVMesh Get(CGVMesh data, CGVolume source, bool addUV, bool reverseNormals)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &addUV;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &reverseNormals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGVMesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51753, XrefRangeEnd = 51754, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CGVMesh Get(CGVMesh data, CGVolume source, IntRegion subset, bool addUV, bool reverseNormals)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(IntRegion**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &subset;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &addUV;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &reverseNormals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_IntRegion_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGVMesh>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 51809, RefRangeEnd = 51812, XrefRangeStart = 51754, XrefRangeEnd = 51809, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CGVMesh Get(CGVMesh data, CGVolume source, IntRegion subset, bool addUV, bool addUV2, bool reverseNormals)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(IntRegion**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &subset;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &addUV;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &addUV2;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &reverseNormals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_Static_CGVMesh_CGVMesh_CGVolume_IntRegion_Boolean_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGVMesh>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 51815, RefRangeEnd = 51816, XrefRangeStart = 51812, XrefRangeEnd = 51815, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetSubMeshCount(int count)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&count);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSubMeshCount_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 51820, RefRangeEnd = 51821, XrefRangeStart = 51816, XrefRangeEnd = 51820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddSubMesh(CGVSubMesh submesh = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)submesh);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSubMesh_Public_Void_CGVSubMesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51821, XrefRangeEnd = 51824, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeVMesh(CGVMesh source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeVMesh_Public_Void_CGVMesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 51855, RefRangeEnd = 51857, XrefRangeStart = 51824, XrefRangeEnd = 51855, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeVMesh(CGVMesh source, Matrix4x4 matrix)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(Matrix4x4**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &matrix;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeVMesh_Public_Void_CGVMesh_Matrix4x4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 52002, RefRangeEnd = 52003, XrefRangeStart = 51857, XrefRangeEnd = 52002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeVMeshes(List<CGVMesh> vMeshes, int startIndex, int endIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vMeshes);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startIndex;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &endIndex;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeVMeshes_Public_Void_List_1_CGVMesh_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 52067, RefRangeEnd = 52068, XrefRangeStart = 52003, XrefRangeEnd = 52067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeUVsNormalsAndTangents(CGVMesh source, int preMergeVertexCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &preMergeVertexCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeUVsNormalsAndTangents_Private_Void_CGVMesh_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 52074, RefRangeEnd = 52076, XrefRangeStart = 52068, XrefRangeEnd = 52074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGVSubMesh GetMaterialSubMesh(Material mat, bool createIfMissing = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &createIfMissing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaterialSubMesh_Public_CGVSubMesh_Material_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGVSubMesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52076, XrefRangeEnd = 52081, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Mesh AsMesh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AsMesh_Public_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 52094, RefRangeEnd = 52096, XrefRangeStart = 52081, XrefRangeEnd = 52094, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ToMesh(ref Mesh mesh, bool includeNormals = true, bool includeTangents = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh);
		*ptr = (nint)(&intPtr);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeNormals;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeTangents;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToMesh_Public_Void_byref_Mesh_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		System.IntPtr intPtr4 = intPtr;
		mesh = ((intPtr4 == (System.IntPtr)0) ? null : new Mesh(intPtr4));
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 52113, RefRangeEnd = 52114, XrefRangeStart = 52096, XrefRangeEnd = 52113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<Material> GetMaterials()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaterials_Public_Il2CppReferenceArray_1_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52114, XrefRangeEnd = 52120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void RecalculateBounds()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_RecalculateBounds_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52120, XrefRangeEnd = 52130, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RecalculateUV2()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RecalculateUV2_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 52134, RefRangeEnd = 52138, XrefRangeStart = 52130, XrefRangeEnd = 52134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TRS(Matrix4x4 matrix)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&matrix);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TRS_Public_Void_Matrix4x4_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52138, XrefRangeEnd = 52139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnVerticesChanged()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnVerticesChanged_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 52181, RefRangeEnd = 52184, XrefRangeStart = 52139, XrefRangeEnd = 52181, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<int> GetCachedSortedVertexIndices()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCachedSortedVertexIndices_Public_SubArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<int>(pointer);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 52202, RefRangeEnd = 52209, XrefRangeStart = 52184, XrefRangeEnd = 52202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearCachedSortedVertexIndices()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearCachedSortedVertexIndices_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 52223, RefRangeEnd = 52225, XrefRangeStart = 52209, XrefRangeEnd = 52223, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Method_Private_Void_List_1_SubArray_1_Int32_Material_0(List<SubArray<int>> subArrays, Material material1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArrays);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material1);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Private_Void_List_1_SubArray_1_Int32_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGVMesh(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
