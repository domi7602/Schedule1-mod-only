using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public class DynamicOcclusionRaycasting : DynamicOcclusionAbstractBase
{
	public sealed class HitResult : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_point;

		private static readonly System.IntPtr NativeFieldInfoPtr_normal;

		private static readonly System.IntPtr NativeFieldInfoPtr_distance;

		private static readonly System.IntPtr NativeFieldInfoPtr_collider2D;

		private static readonly System.IntPtr NativeFieldInfoPtr_collider3D;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_byref_RaycastHit_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_byref_RaycastHit2D_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_hasCollider_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_name_Public_get_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_bounds_Public_get_Bounds_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetNull_Public_Void_0;

		public unsafe Vector3 point
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_point);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_point)) = vector;
			}
		}

		public unsafe Vector3 normal
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_normal);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_normal)) = vector;
			}
		}

		public unsafe float distance
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_distance);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_distance)) = num;
			}
		}

		public unsafe Collider2D collider2D
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_collider2D);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Collider2D>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_collider2D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider2D));
			}
		}

		public unsafe Collider collider3D
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_collider3D);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_collider3D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider));
			}
		}

		public unsafe bool hasCollider
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 72986, RefRangeEnd = 72990, XrefRangeStart = 72982, XrefRangeEnd = 72986, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hasCollider_Public_get_Boolean_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe string name
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 72990, XrefRangeEnd = 72999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_name_Public_get_String_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
		}

		public unsafe Bounds bounds
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 72999, XrefRangeEnd = 73006, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bounds_Public_get_Bounds_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(Bounds*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static HitResult()
		{
			Il2CppClassPointerStore<HitResult>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "HitResult");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HitResult>.NativeClassPtr);
			NativeFieldInfoPtr_point = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HitResult>.NativeClassPtr, "point");
			NativeFieldInfoPtr_normal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HitResult>.NativeClassPtr, "normal");
			NativeFieldInfoPtr_distance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HitResult>.NativeClassPtr, "distance");
			NativeFieldInfoPtr_collider2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HitResult>.NativeClassPtr, "collider2D");
			NativeFieldInfoPtr_collider3D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HitResult>.NativeClassPtr, "collider3D");
			NativeMethodInfoPtr__ctor_Public_Void_byref_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HitResult>.NativeClassPtr, 100664224);
			NativeMethodInfoPtr__ctor_Public_Void_byref_RaycastHit2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HitResult>.NativeClassPtr, 100664225);
			NativeMethodInfoPtr_get_hasCollider_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HitResult>.NativeClassPtr, 100664226);
			NativeMethodInfoPtr_get_name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HitResult>.NativeClassPtr, 100664227);
			NativeMethodInfoPtr_get_bounds_Public_get_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HitResult>.NativeClassPtr, 100664228);
			NativeMethodInfoPtr_SetNull_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HitResult>.NativeClassPtr, 100664229);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 72970, XrefRangeEnd = 72976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe HitResult(ref RaycastHit hit3D)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HitResult>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)Unsafe.AsPointer(ref hit3D);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_byref_RaycastHit_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 72976, XrefRangeEnd = 72982, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe HitResult(ref RaycastHit2D hit2D)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HitResult>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)Unsafe.AsPointer(ref hit2D);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_byref_RaycastHit2D_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73006, XrefRangeEnd = 73008, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetNull()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetNull_Public_Void_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public HitResult(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public HitResult()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HitResult>.NativeClassPtr))
		{
		}
	}

	[OriginalName("Assembly-CSharp.dll", "", "Direction")]
	public enum Direction
	{
		Up = 0,
		Down = 1,
		Left = 2,
		Right = 3,
		Max2D = Down,
		Max3D = Right
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_ClassName;

	private static readonly System.IntPtr NativeFieldInfoPtr_dimensions;

	private static readonly System.IntPtr NativeFieldInfoPtr_layerMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_considerTriggers;

	private static readonly System.IntPtr NativeFieldInfoPtr_minOccluderArea;

	private static readonly System.IntPtr NativeFieldInfoPtr_minSurfaceRatio;

	private static readonly System.IntPtr NativeFieldInfoPtr_maxSurfaceDot;

	private static readonly System.IntPtr NativeFieldInfoPtr_planeAlignment;

	private static readonly System.IntPtr NativeFieldInfoPtr_planeOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_fadeDistanceToSurface;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_CurrentHit;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_RangeMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr__planeEquationWS_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_PrevNonSubHitDirectionId;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_fadeDistanceToPlane_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_fadeDistanceToPlane_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsColliderHiddenByDynamicOccluder_Public_Boolean_Collider_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetShaderKeyword_Protected_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDynamicOcclusionMode_Protected_Virtual_DynamicOcclusion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_planeEquationWS_Public_get_Plane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_planeEquationWS_Private_set_Void_Plane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnValidateProperties_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnablePostValidate_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRandomVectorAround_Private_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_queryTriggerInteraction_Private_get_QueryTriggerInteraction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_raycastMaxDistance_Private_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBestHit_Private_HitResult_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBestHit3D_Private_HitResult_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBestHit2D_Private_HitResult_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDirectionCount_Private_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDirection_Private_Vector3_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsHitValid_Private_Boolean_byref_HitResult_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnProcessOcclusion_Protected_Virtual_Boolean_ProcessOcclusionSource_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetHit_Private_Void_byref_HitResult_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetHitNull_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnModifyMaterialCallback_Protected_Virtual_Void_Interface_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetClippingPlane_Private_Void_Plane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetClippingPlaneOff_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPlaneWS_Private_Void_Plane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public new unsafe static string ClassName
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ClassName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ClassName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Dimensions dimensions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dimensions);
			return *(Dimensions*)num;
		}
		set
		{
			*(Dimensions*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dimensions)) = dimensions;
		}
	}

	public unsafe LayerMask layerMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerMask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerMask)) = layerMask;
		}
	}

	public unsafe bool considerTriggers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_considerTriggers);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_considerTriggers)) = flag;
		}
	}

	public unsafe float minOccluderArea
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minOccluderArea);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minOccluderArea)) = num;
		}
	}

	public unsafe float minSurfaceRatio
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minSurfaceRatio);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minSurfaceRatio)) = num;
		}
	}

	public unsafe float maxSurfaceDot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxSurfaceDot);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxSurfaceDot)) = num;
		}
	}

	public unsafe PlaneAlignment planeAlignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_planeAlignment);
			return *(PlaneAlignment*)num;
		}
		set
		{
			*(PlaneAlignment*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_planeAlignment)) = planeAlignment;
		}
	}

	public unsafe float planeOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_planeOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_planeOffset)) = num;
		}
	}

	public unsafe float fadeDistanceToSurface
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fadeDistanceToSurface);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fadeDistanceToSurface)) = num;
		}
	}

	public unsafe HitResult m_CurrentHit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CurrentHit);
			return new HitResult(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<HitResult>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CurrentHit), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hitResult)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<HitResult>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe float m_RangeMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RangeMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RangeMultiplier)) = num;
		}
	}

	public unsafe Plane _planeEquationWS_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__planeEquationWS_k__BackingField);
			return *(Plane*)num;
		}
		set
		{
			*(Plane*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__planeEquationWS_k__BackingField)) = plane;
		}
	}

	public unsafe uint m_PrevNonSubHitDirectionId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_PrevNonSubHitDirectionId);
			return *(uint*)num;
		}
		set
		{
			*(uint*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_PrevNonSubHitDirectionId)) = num;
		}
	}

	public unsafe float fadeDistanceToPlane
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_fadeDistanceToPlane_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_fadeDistanceToPlane_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Plane planeEquationWS
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_planeEquationWS_Public_get_Plane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Plane*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_planeEquationWS_Private_set_Void_Plane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe QueryTriggerInteraction queryTriggerInteraction
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_queryTriggerInteraction_Private_get_QueryTriggerInteraction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(QueryTriggerInteraction*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float raycastMaxDistance
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73037, XrefRangeEnd = 73038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_raycastMaxDistance_Private_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static DynamicOcclusionRaycasting()
	{
		Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "DynamicOcclusionRaycasting");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr);
		NativeFieldInfoPtr_ClassName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "ClassName");
		NativeFieldInfoPtr_dimensions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "dimensions");
		NativeFieldInfoPtr_layerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "layerMask");
		NativeFieldInfoPtr_considerTriggers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "considerTriggers");
		NativeFieldInfoPtr_minOccluderArea = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "minOccluderArea");
		NativeFieldInfoPtr_minSurfaceRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "minSurfaceRatio");
		NativeFieldInfoPtr_maxSurfaceDot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "maxSurfaceDot");
		NativeFieldInfoPtr_planeAlignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "planeAlignment");
		NativeFieldInfoPtr_planeOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "planeOffset");
		NativeFieldInfoPtr_fadeDistanceToSurface = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "fadeDistanceToSurface");
		NativeFieldInfoPtr_m_CurrentHit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "m_CurrentHit");
		NativeFieldInfoPtr_m_RangeMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "m_RangeMultiplier");
		NativeFieldInfoPtr__planeEquationWS_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "<planeEquationWS>k__BackingField");
		NativeFieldInfoPtr_m_PrevNonSubHitDirectionId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, "m_PrevNonSubHitDirectionId");
		NativeMethodInfoPtr_get_fadeDistanceToPlane_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664196);
		NativeMethodInfoPtr_set_fadeDistanceToPlane_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664197);
		NativeMethodInfoPtr_IsColliderHiddenByDynamicOccluder_Public_Boolean_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664198);
		NativeMethodInfoPtr_GetShaderKeyword_Protected_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664199);
		NativeMethodInfoPtr_GetDynamicOcclusionMode_Protected_Virtual_DynamicOcclusion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664200);
		NativeMethodInfoPtr_get_planeEquationWS_Public_get_Plane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664201);
		NativeMethodInfoPtr_set_planeEquationWS_Private_set_Void_Plane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664202);
		NativeMethodInfoPtr_OnValidateProperties_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664203);
		NativeMethodInfoPtr_OnEnablePostValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664204);
		NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664205);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664206);
		NativeMethodInfoPtr_GetRandomVectorAround_Private_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664207);
		NativeMethodInfoPtr_get_queryTriggerInteraction_Private_get_QueryTriggerInteraction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664208);
		NativeMethodInfoPtr_get_raycastMaxDistance_Private_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664209);
		NativeMethodInfoPtr_GetBestHit_Private_HitResult_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664210);
		NativeMethodInfoPtr_GetBestHit3D_Private_HitResult_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664211);
		NativeMethodInfoPtr_GetBestHit2D_Private_HitResult_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664212);
		NativeMethodInfoPtr_GetDirectionCount_Private_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664213);
		NativeMethodInfoPtr_GetDirection_Private_Vector3_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664214);
		NativeMethodInfoPtr_IsHitValid_Private_Boolean_byref_HitResult_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664215);
		NativeMethodInfoPtr_OnProcessOcclusion_Protected_Virtual_Boolean_ProcessOcclusionSource_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664216);
		NativeMethodInfoPtr_SetHit_Private_Void_byref_HitResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664217);
		NativeMethodInfoPtr_SetHitNull_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664218);
		NativeMethodInfoPtr_OnModifyMaterialCallback_Protected_Virtual_Void_Interface_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664219);
		NativeMethodInfoPtr_SetClippingPlane_Private_Void_Plane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664220);
		NativeMethodInfoPtr_SetClippingPlaneOff_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664221);
		NativeMethodInfoPtr_SetPlaneWS_Private_Void_Plane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664222);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr, 100664223);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73008, XrefRangeEnd = 73014, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsColliderHiddenByDynamicOccluder(Collider collider)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsColliderHiddenByDynamicOccluder_Public_Boolean_Collider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73014, XrefRangeEnd = 73016, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string GetShaderKeyword()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetShaderKeyword_Protected_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 70633, RefRangeEnd = 70636, XrefRangeStart = 70633, XrefRangeEnd = 70636, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDynamicOcclusionMode_Protected_Virtual_DynamicOcclusion_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(MaterialManager.SD.DynamicOcclusion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73016, XrefRangeEnd = 73017, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidateProperties()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidateProperties_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73017, XrefRangeEnd = 73019, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnablePostValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnablePostValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73019, XrefRangeEnd = 73021, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73021, XrefRangeEnd = 73032, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73032, XrefRangeEnd = 73037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetRandomVectorAround(Vector3 direction, float angleDiff)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&direction);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &angleDiff;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRandomVectorAround_Private_Vector3_Vector3_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73038, XrefRangeEnd = 73041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HitResult GetBestHit(Vector3 rayPos, Vector3 rayDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rayPos);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rayDir;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBestHit_Private_HitResult_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new HitResult(pointer);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 73060, RefRangeEnd = 73063, XrefRangeStart = 73041, XrefRangeEnd = 73060, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HitResult GetBestHit3D(Vector3 rayPos, Vector3 rayDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rayPos);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rayDir;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBestHit3D_Private_HitResult_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new HitResult(pointer);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 73084, RefRangeEnd = 73087, XrefRangeStart = 73063, XrefRangeEnd = 73084, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HitResult GetBestHit2D(Vector3 rayPos, Vector3 rayDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rayPos);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rayDir;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBestHit2D_Private_HitResult_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new HitResult(pointer);
	}

	[CallerCount(0)]
	public unsafe uint GetDirectionCount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDirectionCount_Private_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(uint*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 73099, RefRangeEnd = 73100, XrefRangeStart = 73087, XrefRangeEnd = 73099, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetDirection(uint dirInt)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dirInt);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDirection_Private_Vector3_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73100, XrefRangeEnd = 73101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsHitValid(ref HitResult hit, Vector3 forwardVec)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)hit));
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &forwardVec;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsHitValid_Private_Boolean_byref_HitResult_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73101, XrefRangeEnd = 73135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool OnProcessOcclusion(ProcessOcclusionSource source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnProcessOcclusion_Protected_Virtual_Boolean_ProcessOcclusionSource_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 73150, RefRangeEnd = 73151, XrefRangeStart = 73135, XrefRangeEnd = 73150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetHit(ref HitResult hit)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)hit));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetHit_Private_Void_byref_HitResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73166, RefRangeEnd = 73168, XrefRangeStart = 73151, XrefRangeEnd = 73166, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetHitNull()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetHitNull_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73168, XrefRangeEnd = 73181, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)owner);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnModifyMaterialCallback_Protected_Virtual_Void_Interface_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73184, RefRangeEnd = 73186, XrefRangeStart = 73181, XrefRangeEnd = 73184, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetClippingPlane(Plane planeWS)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&planeWS);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetClippingPlane_Private_Void_Plane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73186, XrefRangeEnd = 73196, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetClippingPlaneOff()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetClippingPlaneOff_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void SetPlaneWS(Plane planeWS)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&planeWS);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPlaneWS_Private_Void_Plane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73196, XrefRangeEnd = 73204, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DynamicOcclusionRaycasting()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DynamicOcclusionRaycasting>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DynamicOcclusionRaycasting(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
