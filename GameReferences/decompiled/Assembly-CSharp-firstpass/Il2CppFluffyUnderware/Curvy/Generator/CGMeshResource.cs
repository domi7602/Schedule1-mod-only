using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGMeshResource : DuplicateEditorMesh
{
	private static readonly IntPtr NativeFieldInfoPtr_EverMeshColliderCookingOptions;

	private static readonly IntPtr NativeFieldInfoPtr_mRenderer;

	private static readonly IntPtr NativeFieldInfoPtr_mCollider;

	private static readonly IntPtr NativeFieldInfoPtr_UsedMeshes;

	private static readonly IntPtr NativeMethodInfoPtr_get_Renderer_Public_get_MeshRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Collider_Public_get_Collider_0;

	private static readonly IntPtr NativeMethodInfoPtr_Prepare_Public_Mesh_0;

	private static readonly IntPtr NativeMethodInfoPtr_ColliderMatches_Public_Boolean_CGColliderEnum_0;

	private static readonly IntPtr NativeMethodInfoPtr_RemoveCollider_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateCollider_Public_Boolean_CGColliderEnum_Boolean_Boolean_PhysicMaterial_MeshColliderCookingOptions_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnBeforePush_Public_Virtual_Final_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnAfterPop_Public_Virtual_Final_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetNewMesh_Private_Static_Mesh_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetNewMesh_Private_Static_Mesh_Mesh_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UsesSharedMesh_Private_Static_Boolean_CGMeshResource_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static MeshColliderCookingOptions EverMeshColliderCookingOptions
	{
		get
		{
			Unsafe.SkipInit(out MeshColliderCookingOptions result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EverMeshColliderCookingOptions, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EverMeshColliderCookingOptions, (void*)(&meshColliderCookingOptions));
		}
	}

	public unsafe MeshRenderer mRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	public unsafe Collider mCollider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCollider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCollider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider));
		}
	}

	public unsafe static HashSet<Mesh> UsedMeshes
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UsedMeshes, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != (IntPtr)0) ? Il2CppObjectPool.Get<HashSet<Mesh>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UsedMeshes, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
		}
	}

	public unsafe MeshRenderer Renderer
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 47310, RefRangeEnd = 47317, XrefRangeStart = 47302, XrefRangeEnd = 47310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Renderer_Public_get_MeshRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
	}

	public unsafe Collider Collider
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 47325, RefRangeEnd = 47337, XrefRangeStart = 47317, XrefRangeEnd = 47325, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Collider_Public_get_Collider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
		}
	}

	static CGMeshResource()
	{
		Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGMeshResource");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr);
		NativeFieldInfoPtr_EverMeshColliderCookingOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, "EverMeshColliderCookingOptions");
		NativeFieldInfoPtr_mRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, "mRenderer");
		NativeFieldInfoPtr_mCollider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, "mCollider");
		NativeFieldInfoPtr_UsedMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, "UsedMeshes");
		NativeMethodInfoPtr_get_Renderer_Public_get_MeshRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665859);
		NativeMethodInfoPtr_get_Collider_Public_get_Collider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665860);
		NativeMethodInfoPtr_Prepare_Public_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665861);
		NativeMethodInfoPtr_ColliderMatches_Public_Boolean_CGColliderEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665862);
		NativeMethodInfoPtr_RemoveCollider_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665863);
		NativeMethodInfoPtr_UpdateCollider_Public_Boolean_CGColliderEnum_Boolean_Boolean_PhysicMaterial_MeshColliderCookingOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665864);
		NativeMethodInfoPtr_OnBeforePush_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665865);
		NativeMethodInfoPtr_OnAfterPop_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665866);
		NativeMethodInfoPtr_GetNewMesh_Private_Static_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665867);
		NativeMethodInfoPtr_GetNewMesh_Private_Static_Mesh_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665868);
		NativeMethodInfoPtr_Awake_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665869);
		NativeMethodInfoPtr_OnDestroy_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665870);
		NativeMethodInfoPtr_UsesSharedMesh_Private_Static_Boolean_CGMeshResource_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665871);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr, 100665872);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47337, XrefRangeEnd = 47341, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Mesh Prepare()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Prepare_Public_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 47358, RefRangeEnd = 47359, XrefRangeStart = 47341, XrefRangeEnd = 47358, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool ColliderMatches(CGColliderEnum type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ColliderMatches_Public_Boolean_CGColliderEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 47366, RefRangeEnd = 47367, XrefRangeStart = 47359, XrefRangeEnd = 47366, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveCollider()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveCollider_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 47473, RefRangeEnd = 47474, XrefRangeStart = 47367, XrefRangeEnd = 47473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool UpdateCollider(CGColliderEnum mode, bool convex, bool isTrigger, PhysicMaterial material, MeshColliderCookingOptions meshCookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = (nint)(&mode);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &convex;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &isTrigger;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		*(MeshColliderCookingOptions**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &meshCookingOptions;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateCollider_Public_Boolean_CGColliderEnum_Boolean_Boolean_PhysicMaterial_MeshColliderCookingOptions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47474, XrefRangeEnd = 47481, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnBeforePush()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBeforePush_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47481, XrefRangeEnd = 47499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnAfterPop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnAfterPop_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47499, XrefRangeEnd = 47511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh GetNewMesh()
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNewMesh_Private_Static_Mesh_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47511, XrefRangeEnd = 47525, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mesh GetNewMesh(Mesh oldMesh)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)oldMesh);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNewMesh_Private_Static_Mesh_Mesh_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47525, XrefRangeEnd = 47542, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47542, XrefRangeEnd = 47551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47551, XrefRangeEnd = 47572, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool UsesSharedMesh(CGMeshResource meshResource)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshResource);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UsesSharedMesh_Private_Static_Boolean_CGMeshResource_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 34977, RefRangeEnd = 34989, XrefRangeStart = 34977, XrefRangeEnd = 34989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGMeshResource()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGMeshResource>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGMeshResource(IntPtr pointer)
		: base(pointer)
	{
	}
}
