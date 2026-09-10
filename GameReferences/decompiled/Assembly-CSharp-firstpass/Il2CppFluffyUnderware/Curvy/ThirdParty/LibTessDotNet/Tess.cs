using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppToolBuddy.Pooling.Collections;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public class Tess : Il2CppSystem.Object
{
	public class ActiveRegion : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__eUp;

		private static readonly System.IntPtr NativeFieldInfoPtr__nodeUp;

		private static readonly System.IntPtr NativeFieldInfoPtr__windingNumber;

		private static readonly System.IntPtr NativeFieldInfoPtr__inside;

		private static readonly System.IntPtr NativeFieldInfoPtr__sentinel;

		private static readonly System.IntPtr NativeFieldInfoPtr__dirty;

		private static readonly System.IntPtr NativeFieldInfoPtr__fixUpperEdge;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe MeshUtils.Edge _eUp
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eUp);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eUp)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Dict<ActiveRegion>.Node _nodeUp
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__nodeUp);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dict<ActiveRegion>.Node>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__nodeUp)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node));
			}
		}

		public unsafe int _windingNumber
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windingNumber);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windingNumber)) = num;
			}
		}

		public unsafe bool _inside
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inside);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inside)) = flag;
			}
		}

		public unsafe bool _sentinel
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sentinel);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sentinel)) = flag;
			}
		}

		public unsafe bool _dirty
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dirty);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dirty)) = flag;
			}
		}

		public unsafe bool _fixUpperEdge
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fixUpperEdge);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fixUpperEdge)) = flag;
			}
		}

		static ActiveRegion()
		{
			Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Tess>.NativeClassPtr, "ActiveRegion");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr);
			NativeFieldInfoPtr__eUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_eUp");
			NativeFieldInfoPtr__nodeUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_nodeUp");
			NativeFieldInfoPtr__windingNumber = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_windingNumber");
			NativeFieldInfoPtr__inside = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_inside");
			NativeFieldInfoPtr__sentinel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_sentinel");
			NativeFieldInfoPtr__dirty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_dirty");
			NativeFieldInfoPtr__fixUpperEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, "_fixUpperEdge");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr, 100667238);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ActiveRegion()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ActiveRegion>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ActiveRegion(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__mesh;

	private static readonly System.IntPtr NativeFieldInfoPtr__normal;

	private static readonly System.IntPtr NativeFieldInfoPtr__sUnit;

	private static readonly System.IntPtr NativeFieldInfoPtr__tUnit;

	private static readonly System.IntPtr NativeFieldInfoPtr__bminX;

	private static readonly System.IntPtr NativeFieldInfoPtr__bminY;

	private static readonly System.IntPtr NativeFieldInfoPtr__bmaxX;

	private static readonly System.IntPtr NativeFieldInfoPtr__bmaxY;

	private static readonly System.IntPtr NativeFieldInfoPtr__windingRule;

	private static readonly System.IntPtr NativeFieldInfoPtr__dict;

	private static readonly System.IntPtr NativeFieldInfoPtr__pq;

	private static readonly System.IntPtr NativeFieldInfoPtr__event;

	private static readonly System.IntPtr NativeFieldInfoPtr__combineCallback;

	private static readonly System.IntPtr NativeFieldInfoPtr__vertices;

	private static readonly System.IntPtr NativeFieldInfoPtr__vertexCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_elementsList;

	private static readonly System.IntPtr NativeFieldInfoPtr__elementCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_SUnitX;

	private static readonly System.IntPtr NativeFieldInfoPtr_SUnitY;

	private static readonly System.IntPtr NativeFieldInfoPtr_SentinelCoord;

	private static readonly System.IntPtr NativeFieldInfoPtr_NoEmptyPolygons;

	private static readonly System.IntPtr NativeFieldInfoPtr_UsePooling;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegionBelow_Private_ActiveRegion_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegionAbove_Private_ActiveRegion_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EdgeLeq_Private_Boolean_ActiveRegion_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteRegion_Private_Void_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixUpperEdge_Private_Void_ActiveRegion_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TopLeftRegion_Private_ActiveRegion_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TopRightRegion_Private_ActiveRegion_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRegionBelow_Private_ActiveRegion_ActiveRegion_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeWinding_Private_Void_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FinishRegion_Private_Void_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FinishLeftRegions_Private_Edge_ActiveRegion_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRightEdges_Private_Void_ActiveRegion_Edge_Edge_Edge_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpliceMergeVertices_Private_Void_Edge_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_VertexWeights_Private_Void_Vertex_Vertex_Vertex_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetIntersectData_Private_Void_Vertex_Vertex_Vertex_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckForRightSplice_Private_Boolean_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckForLeftSplice_Private_Boolean_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckForIntersect_Private_Boolean_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WalkDirtyRegions_Private_Void_ActiveRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConnectRightVertex_Private_Void_ActiveRegion_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConnectLeftDegenerate_Private_Void_ActiveRegion_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConnectLeftVertex_Private_Void_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SweepEvent_Private_Void_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSentinel_Private_Void_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitEdgeDict_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoneEdgeDict_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveDegenerateEdges_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitPriorityQ_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DonePriorityQ_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveDegenerateFaces_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeInterior_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Normal_Public_get_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Normal_Public_set_Void_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vertices_Public_get_Il2CppReferenceArray_1_ContourVertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Elements_Public_get_Il2CppStructArray_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ElementsList_Public_get_IList_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ElementsArray_Public_get_Nullable_1_SubArray_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ElementCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeNormal_Private_Void_byref_Vec3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckOrientation_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ProjectPolygon_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TessellateMonoRegion_Private_Void_Face_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TessellateInterior_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DiscardExterior_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetWindingNumber_Private_Void_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNeighbourFace_Private_Int32_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OutputPolymesh_Private_Void_ElementType_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OutputContours_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SignedArea_Private_Single_Il2CppReferenceArray_1_ContourVertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddContour_Public_Void_Il2CppReferenceArray_1_ContourVertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddContour_Public_Void_Il2CppReferenceArray_1_ContourVertex_ContourOrientation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Tessellate_Public_Void_WindingRule_ElementType_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Tessellate_Public_Void_WindingRule_ElementType_Int32_CombineCallback_0;

	public unsafe LTMesh _mesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<LTMesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lTMesh));
		}
	}

	public unsafe Vec3 _normal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__normal);
			return *(Vec3*)num;
		}
		set
		{
			*(Vec3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__normal)) = vec;
		}
	}

	public unsafe Vec3 _sUnit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sUnit);
			return *(Vec3*)num;
		}
		set
		{
			*(Vec3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sUnit)) = vec;
		}
	}

	public unsafe Vec3 _tUnit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tUnit);
			return *(Vec3*)num;
		}
		set
		{
			*(Vec3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tUnit)) = vec;
		}
	}

	public unsafe float _bminX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bminX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bminX)) = num;
		}
	}

	public unsafe float _bminY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bminY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bminY)) = num;
		}
	}

	public unsafe float _bmaxX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bmaxX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bmaxX)) = num;
		}
	}

	public unsafe float _bmaxY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bmaxY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bmaxY)) = num;
		}
	}

	public unsafe WindingRule _windingRule
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windingRule);
			return *(WindingRule*)num;
		}
		set
		{
			*(WindingRule*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windingRule)) = windingRule;
		}
	}

	public unsafe Dict<ActiveRegion> _dict
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dict);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dict<ActiveRegion>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dict)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dict));
		}
	}

	public unsafe PriorityQueue<MeshUtils.Vertex> _pq
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pq);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PriorityQueue<MeshUtils.Vertex>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pq)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)priorityQueue));
		}
	}

	public unsafe MeshUtils.Vertex _event
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__event);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Vertex>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__event)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertex));
		}
	}

	public unsafe CombineCallback _combineCallback
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__combineCallback);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CombineCallback>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__combineCallback)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)combineCallback));
		}
	}

	public unsafe Il2CppReferenceArray<ContourVertex> _vertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__vertices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ContourVertex>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__vertices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int _vertexCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__vertexCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__vertexCount)) = num;
		}
	}

	public unsafe Il2CppSystem.Nullable<SubArray<int>> elementsList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elementsList);
			return new Il2CppSystem.Nullable<SubArray<int>>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<SubArray<int>>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elementsList), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<SubArray<int>>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe int _elementCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__elementCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__elementCount)) = num;
		}
	}

	public unsafe float SUnitX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SUnitX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SUnitX)) = num;
		}
	}

	public unsafe float SUnitY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SUnitY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SUnitY)) = num;
		}
	}

	public unsafe float SentinelCoord
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SentinelCoord);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SentinelCoord)) = num;
		}
	}

	public unsafe bool NoEmptyPolygons
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoEmptyPolygons);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoEmptyPolygons)) = flag;
		}
	}

	public unsafe bool UsePooling
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UsePooling);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UsePooling)) = flag;
		}
	}

	public unsafe Vec3 Normal
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Normal_Public_get_Vec3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vec3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Normal_Public_set_Void_Vec3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppReferenceArray<ContourVertex> Vertices
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 22015, RefRangeEnd = 22016, XrefRangeStart = 22015, XrefRangeEnd = 22016, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vertices_Public_get_Il2CppReferenceArray_1_ContourVertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ContourVertex>>(intPtr) : null;
		}
	}

	public unsafe int VertexCount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Il2CppStructArray<int> Elements
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60414, XrefRangeEnd = 60425, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Elements_Public_get_Il2CppStructArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
	}

	public unsafe IList<int> ElementsList
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ElementsList_Public_get_IList_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IList<int>>(intPtr) : null;
		}
	}

	public unsafe Il2CppSystem.Nullable<SubArray<int>> ElementsArray
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ElementsArray_Public_get_Nullable_1_SubArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<SubArray<int>>(pointer);
		}
	}

	public unsafe int ElementCount
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38177, RefRangeEnd = 38183, XrefRangeStart = 38177, XrefRangeEnd = 38183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ElementCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static Tess()
	{
		Il2CppClassPointerStore<Tess>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "Tess");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Tess>.NativeClassPtr);
		NativeFieldInfoPtr__mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_mesh");
		NativeFieldInfoPtr__normal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_normal");
		NativeFieldInfoPtr__sUnit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_sUnit");
		NativeFieldInfoPtr__tUnit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_tUnit");
		NativeFieldInfoPtr__bminX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_bminX");
		NativeFieldInfoPtr__bminY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_bminY");
		NativeFieldInfoPtr__bmaxX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_bmaxX");
		NativeFieldInfoPtr__bmaxY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_bmaxY");
		NativeFieldInfoPtr__windingRule = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_windingRule");
		NativeFieldInfoPtr__dict = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_dict");
		NativeFieldInfoPtr__pq = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_pq");
		NativeFieldInfoPtr__event = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_event");
		NativeFieldInfoPtr__combineCallback = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_combineCallback");
		NativeFieldInfoPtr__vertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_vertices");
		NativeFieldInfoPtr__vertexCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_vertexCount");
		NativeFieldInfoPtr_elementsList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "elementsList");
		NativeFieldInfoPtr__elementCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "_elementCount");
		NativeFieldInfoPtr_SUnitX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "SUnitX");
		NativeFieldInfoPtr_SUnitY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "SUnitY");
		NativeFieldInfoPtr_SentinelCoord = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "SentinelCoord");
		NativeFieldInfoPtr_NoEmptyPolygons = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "NoEmptyPolygons");
		NativeFieldInfoPtr_UsePooling = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Tess>.NativeClassPtr, "UsePooling");
		NativeMethodInfoPtr_RegionBelow_Private_ActiveRegion_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667183);
		NativeMethodInfoPtr_RegionAbove_Private_ActiveRegion_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667184);
		NativeMethodInfoPtr_EdgeLeq_Private_Boolean_ActiveRegion_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667185);
		NativeMethodInfoPtr_DeleteRegion_Private_Void_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667186);
		NativeMethodInfoPtr_FixUpperEdge_Private_Void_ActiveRegion_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667187);
		NativeMethodInfoPtr_TopLeftRegion_Private_ActiveRegion_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667188);
		NativeMethodInfoPtr_TopRightRegion_Private_ActiveRegion_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667189);
		NativeMethodInfoPtr_AddRegionBelow_Private_ActiveRegion_ActiveRegion_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667190);
		NativeMethodInfoPtr_ComputeWinding_Private_Void_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667191);
		NativeMethodInfoPtr_FinishRegion_Private_Void_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667192);
		NativeMethodInfoPtr_FinishLeftRegions_Private_Edge_ActiveRegion_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667193);
		NativeMethodInfoPtr_AddRightEdges_Private_Void_ActiveRegion_Edge_Edge_Edge_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667194);
		NativeMethodInfoPtr_SpliceMergeVertices_Private_Void_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667195);
		NativeMethodInfoPtr_VertexWeights_Private_Void_Vertex_Vertex_Vertex_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667196);
		NativeMethodInfoPtr_GetIntersectData_Private_Void_Vertex_Vertex_Vertex_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667197);
		NativeMethodInfoPtr_CheckForRightSplice_Private_Boolean_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667198);
		NativeMethodInfoPtr_CheckForLeftSplice_Private_Boolean_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667199);
		NativeMethodInfoPtr_CheckForIntersect_Private_Boolean_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667200);
		NativeMethodInfoPtr_WalkDirtyRegions_Private_Void_ActiveRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667201);
		NativeMethodInfoPtr_ConnectRightVertex_Private_Void_ActiveRegion_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667202);
		NativeMethodInfoPtr_ConnectLeftDegenerate_Private_Void_ActiveRegion_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667203);
		NativeMethodInfoPtr_ConnectLeftVertex_Private_Void_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667204);
		NativeMethodInfoPtr_SweepEvent_Private_Void_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667205);
		NativeMethodInfoPtr_AddSentinel_Private_Void_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667206);
		NativeMethodInfoPtr_InitEdgeDict_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667207);
		NativeMethodInfoPtr_DoneEdgeDict_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667208);
		NativeMethodInfoPtr_RemoveDegenerateEdges_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667209);
		NativeMethodInfoPtr_InitPriorityQ_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667210);
		NativeMethodInfoPtr_DonePriorityQ_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667211);
		NativeMethodInfoPtr_RemoveDegenerateFaces_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667212);
		NativeMethodInfoPtr_ComputeInterior_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667213);
		NativeMethodInfoPtr_get_Normal_Public_get_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667214);
		NativeMethodInfoPtr_set_Normal_Public_set_Void_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667215);
		NativeMethodInfoPtr_get_Vertices_Public_get_Il2CppReferenceArray_1_ContourVertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667216);
		NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667217);
		NativeMethodInfoPtr_get_Elements_Public_get_Il2CppStructArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667218);
		NativeMethodInfoPtr_get_ElementsList_Public_get_IList_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667219);
		NativeMethodInfoPtr_get_ElementsArray_Public_get_Nullable_1_SubArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667220);
		NativeMethodInfoPtr_get_ElementCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667221);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667222);
		NativeMethodInfoPtr_ComputeNormal_Private_Void_byref_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667223);
		NativeMethodInfoPtr_CheckOrientation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667224);
		NativeMethodInfoPtr_ProjectPolygon_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667225);
		NativeMethodInfoPtr_TessellateMonoRegion_Private_Void_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667226);
		NativeMethodInfoPtr_TessellateInterior_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667227);
		NativeMethodInfoPtr_DiscardExterior_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667228);
		NativeMethodInfoPtr_SetWindingNumber_Private_Void_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667229);
		NativeMethodInfoPtr_GetNeighbourFace_Private_Int32_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667230);
		NativeMethodInfoPtr_OutputPolymesh_Private_Void_ElementType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667231);
		NativeMethodInfoPtr_OutputContours_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667232);
		NativeMethodInfoPtr_SignedArea_Private_Single_Il2CppReferenceArray_1_ContourVertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667233);
		NativeMethodInfoPtr_AddContour_Public_Void_Il2CppReferenceArray_1_ContourVertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667234);
		NativeMethodInfoPtr_AddContour_Public_Void_Il2CppReferenceArray_1_ContourVertex_ContourOrientation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667235);
		NativeMethodInfoPtr_Tessellate_Public_Void_WindingRule_ElementType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667236);
		NativeMethodInfoPtr_Tessellate_Public_Void_WindingRule_ElementType_Int32_CombineCallback_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Tess>.NativeClassPtr, 100667237);
	}

	[CallerCount(0)]
	public unsafe ActiveRegion RegionBelow(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegionBelow_Private_ActiveRegion_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ActiveRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe ActiveRegion RegionAbove(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegionAbove_Private_ActiveRegion_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ActiveRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59940, XrefRangeEnd = 59942, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool EdgeLeq(ActiveRegion reg1, ActiveRegion reg2)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg1);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg2);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EdgeLeq_Private_Boolean_ActiveRegion_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59942, XrefRangeEnd = 59947, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DeleteRegion(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteRegion_Private_Void_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59947, XrefRangeEnd = 59951, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixUpperEdge(ActiveRegion reg, MeshUtils.Edge newEdge)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newEdge);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixUpperEdge_Private_Void_ActiveRegion_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 59955, RefRangeEnd = 59959, XrefRangeStart = 59951, XrefRangeEnd = 59955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ActiveRegion TopLeftRegion(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TopLeftRegion_Private_ActiveRegion_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ActiveRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe ActiveRegion TopRightRegion(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TopRightRegion_Private_ActiveRegion_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ActiveRegion>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59969, RefRangeEnd = 59970, XrefRangeStart = 59959, XrefRangeEnd = 59969, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ActiveRegion AddRegionBelow(ActiveRegion regAbove, MeshUtils.Edge eNewUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regAbove);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eNewUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddRegionBelow_Private_ActiveRegion_ActiveRegion_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ActiveRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59970, XrefRangeEnd = 59971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeWinding(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeWinding_Private_Void_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59977, RefRangeEnd = 59978, XrefRangeStart = 59971, XrefRangeEnd = 59977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FinishRegion(ActiveRegion reg)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FinishRegion_Private_Void_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 59985, RefRangeEnd = 59991, XrefRangeStart = 59978, XrefRangeEnd = 59985, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshUtils.Edge FinishLeftRegions(ActiveRegion regFirst, ActiveRegion regLast)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regFirst);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regLast);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FinishLeftRegions_Private_Edge_ActiveRegion_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshUtils.Edge>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 60012, RefRangeEnd = 60016, XrefRangeStart = 59991, XrefRangeEnd = 60012, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddRightEdges(ActiveRegion regUp, MeshUtils.Edge eFirst, MeshUtils.Edge eLast, MeshUtils.Edge eTopLeft, bool cleanUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eFirst);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eLast);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eTopLeft);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &cleanUp;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddRightEdges_Private_Void_ActiveRegion_Edge_Edge_Edge_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60016, XrefRangeEnd = 60018, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SpliceMergeVertices(MeshUtils.Edge e1, MeshUtils.Edge e2)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e1);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e2);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpliceMergeVertices_Private_Void_Edge_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 60024, RefRangeEnd = 60026, XrefRangeStart = 60018, XrefRangeEnd = 60024, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void VertexWeights(MeshUtils.Vertex isect, MeshUtils.Vertex org, MeshUtils.Vertex dst, out float w0, out float w1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)isect);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)org);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dst);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref w0);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref w1);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VertexWeights_Private_Void_Vertex_Vertex_Vertex_byref_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60026, XrefRangeEnd = 60047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetIntersectData(MeshUtils.Vertex isect, MeshUtils.Vertex orgUp, MeshUtils.Vertex dstUp, MeshUtils.Vertex orgLo, MeshUtils.Vertex dstLo)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)isect);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)orgUp);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dstUp);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)orgLo);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dstLo);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIntersectData_Private_Void_Vertex_Vertex_Vertex_Vertex_Vertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 60057, RefRangeEnd = 60062, XrefRangeStart = 60047, XrefRangeEnd = 60057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CheckForRightSplice(ActiveRegion regUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckForRightSplice_Private_Boolean_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60065, RefRangeEnd = 60066, XrefRangeStart = 60062, XrefRangeEnd = 60065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CheckForLeftSplice(ActiveRegion regUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckForLeftSplice_Private_Boolean_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 60081, RefRangeEnd = 60084, XrefRangeStart = 60066, XrefRangeEnd = 60081, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CheckForIntersect(ActiveRegion regUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckForIntersect_Private_Boolean_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 60098, RefRangeEnd = 60101, XrefRangeStart = 60084, XrefRangeEnd = 60098, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void WalkDirtyRegions(ActiveRegion regUp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WalkDirtyRegions_Private_Void_ActiveRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60101, XrefRangeEnd = 60109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConnectRightVertex(ActiveRegion regUp, MeshUtils.Edge eBottomLeft)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eBottomLeft);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConnectRightVertex_Private_Void_ActiveRegion_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60109, XrefRangeEnd = 60126, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConnectLeftDegenerate(ActiveRegion regUp, MeshUtils.Vertex vEvent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regUp);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vEvent);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConnectLeftDegenerate_Private_Void_ActiveRegion_Vertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60145, RefRangeEnd = 60146, XrefRangeStart = 60126, XrefRangeEnd = 60145, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConnectLeftVertex(MeshUtils.Vertex vEvent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vEvent);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConnectLeftVertex_Private_Void_Vertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 60151, RefRangeEnd = 60156, XrefRangeStart = 60146, XrefRangeEnd = 60151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SweepEvent(MeshUtils.Vertex vEvent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vEvent);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SweepEvent_Private_Void_Vertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 60171, RefRangeEnd = 60175, XrefRangeStart = 60156, XrefRangeEnd = 60171, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddSentinel(float smin, float smax, float t)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&smin);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &smax;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSentinel_Private_Void_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60175, XrefRangeEnd = 60190, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InitEdgeDict()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitEdgeDict_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60190, XrefRangeEnd = 60201, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoneEdgeDict()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoneEdgeDict_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60206, RefRangeEnd = 60207, XrefRangeStart = 60201, XrefRangeEnd = 60206, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveDegenerateEdges()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveDegenerateEdges_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60237, RefRangeEnd = 60238, XrefRangeStart = 60207, XrefRangeEnd = 60237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InitPriorityQ()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitPriorityQ_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60239, RefRangeEnd = 60240, XrefRangeStart = 60238, XrefRangeEnd = 60239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DonePriorityQ()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DonePriorityQ_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 60242, RefRangeEnd = 60244, XrefRangeStart = 60240, XrefRangeEnd = 60242, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveDegenerateFaces()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveDegenerateFaces_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60413, RefRangeEnd = 60414, XrefRangeStart = 60244, XrefRangeEnd = 60413, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeInterior()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeInterior_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 60432, RefRangeEnd = 60434, XrefRangeStart = 60425, XrefRangeEnd = 60432, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Tess()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Tess>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60480, RefRangeEnd = 60481, XrefRangeStart = 60434, XrefRangeEnd = 60480, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeNormal(ref Vec3 norm)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref norm);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeNormal_Private_Void_byref_Vec3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60481, XrefRangeEnd = 60487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckOrientation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckOrientation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60520, RefRangeEnd = 60521, XrefRangeStart = 60487, XrefRangeEnd = 60520, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ProjectPolygon()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProjectPolygon_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 60531, RefRangeEnd = 60533, XrefRangeStart = 60521, XrefRangeEnd = 60531, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TessellateMonoRegion(MeshUtils.Face face)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)face);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TessellateMonoRegion_Private_Void_Face_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60533, XrefRangeEnd = 60535, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TessellateInterior()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TessellateInterior_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60535, XrefRangeEnd = 60537, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DiscardExterior()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DiscardExterior_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60537, XrefRangeEnd = 60541, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetWindingNumber(int value, bool keepOnlyBoundary)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&value);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &keepOnlyBoundary;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetWindingNumber_Private_Void_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe int GetNeighbourFace(MeshUtils.Edge edge)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNeighbourFace_Private_Int32_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60587, RefRangeEnd = 60588, XrefRangeStart = 60541, XrefRangeEnd = 60587, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OutputPolymesh(ElementType elementType, int polySize)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&elementType);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &polySize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OutputPolymesh_Private_Void_ElementType_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60619, RefRangeEnd = 60620, XrefRangeStart = 60588, XrefRangeEnd = 60619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OutputContours()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OutputContours_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60620, XrefRangeEnd = 60621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float SignedArea(Il2CppReferenceArray<ContourVertex> vertices)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SignedArea_Private_Single_Il2CppReferenceArray_1_ContourVertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 60622, RefRangeEnd = 60624, XrefRangeStart = 60621, XrefRangeEnd = 60622, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddContour(Il2CppReferenceArray<ContourVertex> vertices)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddContour_Public_Void_Il2CppReferenceArray_1_ContourVertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 60640, RefRangeEnd = 60642, XrefRangeStart = 60624, XrefRangeEnd = 60640, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddContour(Il2CppReferenceArray<ContourVertex> vertices, ContourOrientation forceOrientation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(ContourOrientation**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &forceOrientation;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddContour_Public_Void_Il2CppReferenceArray_1_ContourVertex_ContourOrientation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60642, XrefRangeEnd = 60643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Tessellate(WindingRule windingRule, ElementType elementType, int polySize)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&windingRule);
		*(ElementType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementType;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &polySize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Tessellate_Public_Void_WindingRule_ElementType_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60663, RefRangeEnd = 60664, XrefRangeStart = 60643, XrefRangeEnd = 60663, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Tessellate(WindingRule windingRule, ElementType elementType, int polySize, CombineCallback combineCallback)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&windingRule);
		*(ElementType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementType;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &polySize;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)combineCallback);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Tessellate_Public_Void_WindingRule_ElementType_Int32_CombineCallback_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Tess(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
