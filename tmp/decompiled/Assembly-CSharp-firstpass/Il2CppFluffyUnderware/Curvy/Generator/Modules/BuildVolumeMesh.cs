using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class BuildVolumeMesh : CGModule
{
	private static readonly IntPtr NativeFieldInfoPtr_DefaultUnscalingOrigin;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultSplitLength;

	private static readonly IntPtr NativeFieldInfoPtr_InVolume;

	private static readonly IntPtr NativeFieldInfoPtr_OutVMesh;

	private static readonly IntPtr NativeFieldInfoPtr_m_ReverseTriOrder;

	private static readonly IntPtr NativeFieldInfoPtr_m_GenerateUV;

	private static readonly IntPtr NativeFieldInfoPtr_unscaleU;

	private static readonly IntPtr NativeFieldInfoPtr_unscalingOrigin;

	private static readonly IntPtr NativeFieldInfoPtr_m_GenerateUV2;

	private static readonly IntPtr NativeFieldInfoPtr_m_Split;

	private static readonly IntPtr NativeFieldInfoPtr_m_SplitLength;

	private static readonly IntPtr NativeFieldInfoPtr_splitUV2;

	private static readonly IntPtr NativeFieldInfoPtr_m_MaterialSettings;

	private static readonly IntPtr NativeFieldInfoPtr_m_Material;

	private static readonly IntPtr NativeMethodInfoPtr_get_GenerateUV_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_GenerateUV_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_GenerateUV2_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_GenerateUV2_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_UnscaleU_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_UnscaleU_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_UnscalingOrigin_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_UnscalingOrigin_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ReverseTriOrder_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ReverseTriOrder_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Split_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Split_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SplitLength_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SplitLength_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SplitUV2_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SplitUV2_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MaterialSetttings_Public_get_List_1_CGMaterialSettingsEx_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MaterialSettings_Public_get_List_1_CGMaterialSettingsEx_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MaterialCount_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddMaterial_Public_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_RemoveMaterial_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMaterial_Public_Void_Int32_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetMaterial_Public_Material_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_build_Private_Void_CGVMesh_CGVolume_IntRegion_List_1_SamplePointsMaterialGroupCollection_0;

	private static readonly IntPtr NativeMethodInfoPtr_prepareSubMeshes_Private_Static_Void_CGVMesh_List_1_SamplePointsMaterialGroupCollection_Int32_byref_Il2CppReferenceArray_1_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_createMaterialGroupUV_Private_Void_CGVMesh_CGVolume_SamplePointsMaterialGroup_Int32_Single_Single_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_createMaterialGroupUV2_Private_Void_CGVMesh_CGVolume_SamplePointsMaterialGroup_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_createPatchTriangles_Private_Static_Void_Il2CppStructArray_1_Int32_byref_Int32_Int32_Int32_Int32_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_getMaterialIDGroups_Private_List_1_SamplePointsMaterialGroupCollection_CGVolume_0;

	private static readonly IntPtr NativeMethodInfoPtr_validateMaterialIndex_Private_Boolean_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsSplitUV2Togglable_Private_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float DefaultUnscalingOrigin
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultUnscalingOrigin, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultUnscalingOrigin, (void*)(&num));
		}
	}

	public unsafe static int DefaultSplitLength
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultSplitLength, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultSplitLength, (void*)(&num));
		}
	}

	public unsafe CGModuleInputSlot InVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InVolume);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InVolume)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleOutputSlot OutVMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutVMesh);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutVMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleOutputSlot));
		}
	}

	public unsafe bool m_ReverseTriOrder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ReverseTriOrder);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ReverseTriOrder)) = flag;
		}
	}

	public unsafe bool m_GenerateUV
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GenerateUV);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GenerateUV)) = flag;
		}
	}

	public unsafe bool unscaleU
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unscaleU);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unscaleU)) = flag;
		}
	}

	public unsafe float unscalingOrigin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unscalingOrigin);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unscalingOrigin)) = num;
		}
	}

	public unsafe bool m_GenerateUV2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GenerateUV2);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GenerateUV2)) = flag;
		}
	}

	public unsafe bool m_Split
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Split);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Split)) = flag;
		}
	}

	public unsafe float m_SplitLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SplitLength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SplitLength)) = num;
		}
	}

	public unsafe bool splitUV2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_splitUV2);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_splitUV2)) = flag;
		}
	}

	public unsafe List<CGMaterialSettingsEx> m_MaterialSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MaterialSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<CGMaterialSettingsEx>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MaterialSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppReferenceArray<Material> m_Material
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Material);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Material)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool GenerateUV
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GenerateUV_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55372, XrefRangeEnd = 55373, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_GenerateUV_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool GenerateUV2
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GenerateUV2_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55373, XrefRangeEnd = 55374, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_GenerateUV2_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool UnscaleU
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UnscaleU_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55374, XrefRangeEnd = 55375, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UnscaleU_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float UnscalingOrigin
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UnscalingOrigin_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55375, XrefRangeEnd = 55376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UnscalingOrigin_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ReverseTriOrder
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ReverseTriOrder_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55376, XrefRangeEnd = 55377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ReverseTriOrder_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool Split
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Split_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55377, XrefRangeEnd = 55378, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Split_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float SplitLength
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SplitLength_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55378, XrefRangeEnd = 55379, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SplitLength_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool SplitUV2
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SplitUV2_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SplitUV2_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<CGMaterialSettingsEx> MaterialSetttings
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaterialSetttings_Public_get_List_1_CGMaterialSettingsEx_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<CGMaterialSettingsEx>>(intPtr) : null;
		}
	}

	public unsafe List<CGMaterialSettingsEx> MaterialSettings
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaterialSettings_Public_get_List_1_CGMaterialSettingsEx_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<CGMaterialSettingsEx>>(intPtr) : null;
		}
	}

	public unsafe int MaterialCount
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 55380, RefRangeEnd = 55381, XrefRangeStart = 55379, XrefRangeEnd = 55380, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaterialCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsSplitUV2Togglable
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsSplitUV2Togglable_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static BuildVolumeMesh()
	{
		Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "BuildVolumeMesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultUnscalingOrigin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "DefaultUnscalingOrigin");
		NativeFieldInfoPtr_DefaultSplitLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "DefaultSplitLength");
		NativeFieldInfoPtr_InVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "InVolume");
		NativeFieldInfoPtr_OutVMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "OutVMesh");
		NativeFieldInfoPtr_m_ReverseTriOrder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_ReverseTriOrder");
		NativeFieldInfoPtr_m_GenerateUV = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_GenerateUV");
		NativeFieldInfoPtr_unscaleU = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "unscaleU");
		NativeFieldInfoPtr_unscalingOrigin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "unscalingOrigin");
		NativeFieldInfoPtr_m_GenerateUV2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_GenerateUV2");
		NativeFieldInfoPtr_m_Split = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_Split");
		NativeFieldInfoPtr_m_SplitLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_SplitLength");
		NativeFieldInfoPtr_splitUV2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "splitUV2");
		NativeFieldInfoPtr_m_MaterialSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_MaterialSettings");
		NativeFieldInfoPtr_m_Material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, "m_Material");
		NativeMethodInfoPtr_get_GenerateUV_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666672);
		NativeMethodInfoPtr_set_GenerateUV_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666673);
		NativeMethodInfoPtr_get_GenerateUV2_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666674);
		NativeMethodInfoPtr_set_GenerateUV2_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666675);
		NativeMethodInfoPtr_get_UnscaleU_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666676);
		NativeMethodInfoPtr_set_UnscaleU_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666677);
		NativeMethodInfoPtr_get_UnscalingOrigin_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666678);
		NativeMethodInfoPtr_set_UnscalingOrigin_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666679);
		NativeMethodInfoPtr_get_ReverseTriOrder_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666680);
		NativeMethodInfoPtr_set_ReverseTriOrder_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666681);
		NativeMethodInfoPtr_get_Split_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666682);
		NativeMethodInfoPtr_set_Split_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666683);
		NativeMethodInfoPtr_get_SplitLength_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666684);
		NativeMethodInfoPtr_set_SplitLength_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666685);
		NativeMethodInfoPtr_get_SplitUV2_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666686);
		NativeMethodInfoPtr_set_SplitUV2_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666687);
		NativeMethodInfoPtr_get_MaterialSetttings_Public_get_List_1_CGMaterialSettingsEx_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666688);
		NativeMethodInfoPtr_get_MaterialSettings_Public_get_List_1_CGMaterialSettingsEx_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666689);
		NativeMethodInfoPtr_get_MaterialCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666690);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666691);
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666692);
		NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666693);
		NativeMethodInfoPtr_AddMaterial_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666694);
		NativeMethodInfoPtr_RemoveMaterial_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666695);
		NativeMethodInfoPtr_SetMaterial_Public_Void_Int32_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666696);
		NativeMethodInfoPtr_GetMaterial_Public_Material_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666697);
		NativeMethodInfoPtr_build_Private_Void_CGVMesh_CGVolume_IntRegion_List_1_SamplePointsMaterialGroupCollection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666698);
		NativeMethodInfoPtr_prepareSubMeshes_Private_Static_Void_CGVMesh_List_1_SamplePointsMaterialGroupCollection_Int32_byref_Il2CppReferenceArray_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666699);
		NativeMethodInfoPtr_createMaterialGroupUV_Private_Void_CGVMesh_CGVolume_SamplePointsMaterialGroup_Int32_Single_Single_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666700);
		NativeMethodInfoPtr_createMaterialGroupUV2_Private_Void_CGVMesh_CGVolume_SamplePointsMaterialGroup_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666701);
		NativeMethodInfoPtr_createPatchTriangles_Private_Static_Void_Il2CppStructArray_1_Int32_byref_Int32_Int32_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666702);
		NativeMethodInfoPtr_getMaterialIDGroups_Private_List_1_SamplePointsMaterialGroupCollection_CGVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666703);
		NativeMethodInfoPtr_validateMaterialIndex_Private_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666704);
		NativeMethodInfoPtr_get_IsSplitUV2Togglable_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666705);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr, 100666706);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55381, XrefRangeEnd = 55399, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55399, XrefRangeEnd = 55437, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55437, XrefRangeEnd = 55489, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55489, XrefRangeEnd = 55505, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int AddMaterial()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddMaterial_Public_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55505, XrefRangeEnd = 55514, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveMaterial(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveMaterial_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55514, XrefRangeEnd = 55525, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMaterial(int index, Material mat)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&index);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMaterial_Public_Void_Int32_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55525, XrefRangeEnd = 55526, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Material GetMaterial(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaterial_Public_Material_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 55581, RefRangeEnd = 55582, XrefRangeStart = 55526, XrefRangeEnd = 55581, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void build(CGVMesh vmesh, CGVolume vol, IntRegion subset, List<SamplePointsMaterialGroupCollection> materialIdGroups)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vmesh);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vol);
		*(IntRegion**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &subset;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialIdGroups);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_build_Private_Void_CGVMesh_CGVolume_IntRegion_List_1_SamplePointsMaterialGroupCollection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 55594, RefRangeEnd = 55595, XrefRangeStart = 55582, XrefRangeEnd = 55594, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void prepareSubMeshes(CGVMesh vmesh, List<SamplePointsMaterialGroupCollection> groupsBySubMeshes, int extrusions, ref Il2CppReferenceArray<Material> materials)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vmesh);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)groupsBySubMeshes);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &extrusions;
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)));
		IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materials);
		*(IntPtr**)num = &intPtr;
		Unsafe.SkipInit(out IntPtr intPtr3);
		IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_prepareSubMeshes_Private_Static_Void_CGVMesh_List_1_SamplePointsMaterialGroupCollection_Int32_byref_Il2CppReferenceArray_1_Material_0, (IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		IntPtr intPtr4 = intPtr;
		materials = ((intPtr4 == (IntPtr)0) ? null : new Il2CppReferenceArray<Material>(intPtr4));
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 55602, RefRangeEnd = 55604, XrefRangeStart = 55595, XrefRangeEnd = 55602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void createMaterialGroupUV(CGVMesh vmesh, CGVolume volume, SamplePointsMaterialGroup materialGroup, int matIndex, float aspectCorrectionV, float aspectCorrectionU, int sample, int baseVertex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vmesh);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialGroup);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &matIndex;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &aspectCorrectionV;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &aspectCorrectionU;
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &sample;
		*(int**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = &baseVertex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_createMaterialGroupUV_Private_Void_CGVMesh_CGVolume_SamplePointsMaterialGroup_Int32_Single_Single_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55604, XrefRangeEnd = 55606, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void createMaterialGroupUV2(CGVMesh vmesh, CGVolume volume, SamplePointsMaterialGroup materialGroup, int sample, int baseVertex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vmesh);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialGroup);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &sample;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &baseVertex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_createMaterialGroupUV2_Private_Void_CGVMesh_CGVolume_SamplePointsMaterialGroup_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 55606, RefRangeEnd = 55607, XrefRangeStart = 55606, XrefRangeEnd = 55606, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void createPatchTriangles(Il2CppStructArray<int> triangles, ref int triIdx, int curVTIndex, int patchSize, int crossSize, bool reverse)
	{
		IntPtr* ptr = stackalloc IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)triangles);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref triIdx);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &curVTIndex;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &patchSize;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &crossSize;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &reverse;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_createPatchTriangles_Private_Static_Void_Il2CppStructArray_1_Int32_byref_Int32_Int32_Int32_Int32_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 55681, RefRangeEnd = 55682, XrefRangeStart = 55607, XrefRangeEnd = 55681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<SamplePointsMaterialGroupCollection> getMaterialIDGroups(CGVolume volume)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getMaterialIDGroups_Private_List_1_SamplePointsMaterialGroupCollection_CGVolume_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<SamplePointsMaterialGroupCollection>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 55685, RefRangeEnd = 55688, XrefRangeStart = 55682, XrefRangeEnd = 55685, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool validateMaterialIndex(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_validateMaterialIndex_Private_Boolean_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 55688, XrefRangeEnd = 55710, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BuildVolumeMesh()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BuildVolumeMesh>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BuildVolumeMesh(IntPtr pointer)
		: base(pointer)
	{
	}
}
