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

[Serializable]
public class CGBoundsGroup : CGWeightedItem
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "RotationModeEnum")]
	public enum RotationModeEnum
	{
		Full,
		Direction,
		Horizontal,
		Independent
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "DistributionModeEnum")]
	public enum DistributionModeEnum
	{
		Parent,
		Self
	}

	private static readonly IntPtr NativeFieldInfoPtr_m_Name;

	private static readonly IntPtr NativeFieldInfoPtr_m_KeepTogether;

	private static readonly IntPtr NativeFieldInfoPtr_m_SpaceBefore;

	private static readonly IntPtr NativeFieldInfoPtr_m_SpaceAfter;

	private static readonly IntPtr NativeFieldInfoPtr_m_CrossBase;

	private static readonly IntPtr NativeFieldInfoPtr_m_IgnoreModuleCrossBase;

	private static readonly IntPtr NativeFieldInfoPtr_m_RandomizeItems;

	private static readonly IntPtr NativeFieldInfoPtr_m_RepeatingItems;

	private static readonly IntPtr NativeFieldInfoPtr_m_RelativeTranslation;

	private static readonly IntPtr NativeFieldInfoPtr_m_TranslationX;

	private static readonly IntPtr NativeFieldInfoPtr_m_TranslationY;

	private static readonly IntPtr NativeFieldInfoPtr_m_TranslationZ;

	private static readonly IntPtr NativeFieldInfoPtr_m_RotationMode;

	private static readonly IntPtr NativeFieldInfoPtr_m_RotationX;

	private static readonly IntPtr NativeFieldInfoPtr_m_RotationY;

	private static readonly IntPtr NativeFieldInfoPtr_m_RotationZ;

	private static readonly IntPtr NativeFieldInfoPtr_m_UniformScaling;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleX;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleY;

	private static readonly IntPtr NativeFieldInfoPtr_m_ScaleZ;

	private static readonly IntPtr NativeFieldInfoPtr_m_Items;

	private static readonly IntPtr NativeFieldInfoPtr_m_DistributionMode;

	private static readonly IntPtr NativeFieldInfoPtr_m_PositionOffset;

	private static readonly IntPtr NativeFieldInfoPtr_m_Height;

	private static readonly IntPtr NativeFieldInfoPtr_m_RepeatingOrder;

	private static readonly IntPtr NativeFieldInfoPtr_m_RotationOffset;

	private static readonly IntPtr NativeFieldInfoPtr_m_RotationScatter;

	private static readonly IntPtr NativeMethodInfoPtr_get_Name_Public_get_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Name_Public_set_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_KeepTogether_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_KeepTogether_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SpaceBefore_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SpaceBefore_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SpaceAfter_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SpaceAfter_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RandomizeItems_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RandomizeItems_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RepeatingItems_Public_get_IntRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RepeatingItems_Public_set_Void_IntRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossBase_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CrossBase_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IgnoreModuleCrossBase_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IgnoreModuleCrossBase_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RotationMode_Public_get_RotationModeEnum_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RotationMode_Public_set_Void_RotationModeEnum_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RotationX_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RotationX_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RotationY_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RotationY_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RotationZ_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RotationZ_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_UniformScaling_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_UniformScaling_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleX_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleX_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleY_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleY_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ScaleZ_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ScaleZ_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RelativeTranslation_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_RelativeTranslation_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_TranslationX_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_TranslationX_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_TranslationY_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_TranslationY_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_TranslationZ_Public_get_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_TranslationZ_Public_set_Void_FloatRegion_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Items_Public_get_List_1_CGBoundsGroupItem_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_FirstRepeating_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_FirstRepeating_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_LastRepeating_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_LastRepeating_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ItemCount_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_FillItemBag_Public_Static_Void_WeightedRandom_1_Int32_IEnumerable_1_CGWeightedItem_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RepeatingGroupsOptions_Private_get_RegionOptions_1_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PositionRangeOptions_Private_get_RegionOptions_1_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ConvertObsoleteData_Public_Void_0;

	public unsafe string m_Name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Name);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool m_KeepTogether
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_KeepTogether);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_KeepTogether)) = flag;
		}
	}

	public unsafe FloatRegion m_SpaceBefore
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SpaceBefore);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SpaceBefore)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_SpaceAfter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SpaceAfter);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_SpaceAfter)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_CrossBase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossBase);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossBase)) = floatRegion;
		}
	}

	public unsafe bool m_IgnoreModuleCrossBase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_IgnoreModuleCrossBase);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_IgnoreModuleCrossBase)) = flag;
		}
	}

	public unsafe bool m_RandomizeItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RandomizeItems);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RandomizeItems)) = flag;
		}
	}

	public unsafe IntRegion m_RepeatingItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RepeatingItems);
			return *(IntRegion*)num;
		}
		set
		{
			*(IntRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RepeatingItems)) = intRegion;
		}
	}

	public unsafe bool m_RelativeTranslation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RelativeTranslation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RelativeTranslation)) = flag;
		}
	}

	public unsafe FloatRegion m_TranslationX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_TranslationX);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_TranslationX)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_TranslationY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_TranslationY);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_TranslationY)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_TranslationZ
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_TranslationZ);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_TranslationZ)) = floatRegion;
		}
	}

	public unsafe RotationModeEnum m_RotationMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationMode);
			return *(RotationModeEnum*)num;
		}
		set
		{
			*(RotationModeEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationMode)) = rotationModeEnum;
		}
	}

	public unsafe FloatRegion m_RotationX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationX);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationX)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_RotationY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationY);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationY)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_RotationZ
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationZ);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationZ)) = floatRegion;
		}
	}

	public unsafe bool m_UniformScaling
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UniformScaling);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UniformScaling)) = flag;
		}
	}

	public unsafe FloatRegion m_ScaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleX);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleX)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_ScaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleY);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleY)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_ScaleZ
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleZ);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ScaleZ)) = floatRegion;
		}
	}

	public unsafe List<CGBoundsGroupItem> m_Items
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Items);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<CGBoundsGroupItem>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe DistributionModeEnum m_DistributionMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_DistributionMode);
			return *(DistributionModeEnum*)num;
		}
		set
		{
			*(DistributionModeEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_DistributionMode)) = distributionModeEnum;
		}
	}

	public unsafe FloatRegion m_PositionOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_PositionOffset);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_PositionOffset)) = floatRegion;
		}
	}

	public unsafe FloatRegion m_Height
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Height);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Height)) = floatRegion;
		}
	}

	public unsafe CurvyRepeatingOrderEnum m_RepeatingOrder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RepeatingOrder);
			return *(CurvyRepeatingOrderEnum*)num;
		}
		set
		{
			*(CurvyRepeatingOrderEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RepeatingOrder)) = curvyRepeatingOrderEnum;
		}
	}

	public unsafe Vector3 m_RotationOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationOffset)) = vector;
		}
	}

	public unsafe Vector3 m_RotationScatter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationScatter);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RotationScatter)) = vector;
		}
	}

	public unsafe string Name
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Name_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Name_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool KeepTogether
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30481, RefRangeEnd = 30482, XrefRangeStart = 30481, XrefRangeEnd = 30482, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_KeepTogether_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 30482, RefRangeEnd = 30484, XrefRangeStart = 30482, XrefRangeEnd = 30484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_KeepTogether_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion SpaceBefore
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SpaceBefore_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SpaceBefore_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion SpaceAfter
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SpaceAfter_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SpaceAfter_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool RandomizeItems
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RandomizeItems_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 46699, RefRangeEnd = 46708, XrefRangeStart = 46699, XrefRangeEnd = 46699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RandomizeItems_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe IntRegion RepeatingItems
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RepeatingItems_Public_get_IntRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(IntRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 46708, RefRangeEnd = 46711, XrefRangeStart = 46708, XrefRangeEnd = 46708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RepeatingItems_Public_set_Void_IntRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion CrossBase
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossBase_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CrossBase_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IgnoreModuleCrossBase
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IgnoreModuleCrossBase_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 46711, RefRangeEnd = 46714, XrefRangeStart = 46711, XrefRangeEnd = 46711, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IgnoreModuleCrossBase_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe RotationModeEnum RotationMode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RotationMode_Public_get_RotationModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(RotationModeEnum*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RotationMode_Public_set_Void_RotationModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion RotationX
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RotationX_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RotationX_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion RotationY
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RotationY_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RotationY_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion RotationZ
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RotationZ_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RotationZ_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool UniformScaling
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UniformScaling_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UniformScaling_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion ScaleX
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleX_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleX_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion ScaleY
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleY_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleY_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion ScaleZ
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleZ_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleZ_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool RelativeTranslation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RelativeTranslation_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RelativeTranslation_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion TranslationX
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TranslationX_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TranslationX_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion TranslationY
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TranslationY_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TranslationY_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe FloatRegion TranslationZ
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TranslationZ_Public_get_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FloatRegion*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TranslationZ_Public_set_Void_FloatRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<CGBoundsGroupItem> Items
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Items_Public_get_List_1_CGBoundsGroupItem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<CGBoundsGroupItem>>(intPtr) : null;
		}
	}

	public unsafe int FirstRepeating
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 44533, RefRangeEnd = 44544, XrefRangeStart = 44533, XrefRangeEnd = 44544, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FirstRepeating_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46714, XrefRangeEnd = 46715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_FirstRepeating_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int LastRepeating
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38157, RefRangeEnd = 38163, XrefRangeStart = 38157, XrefRangeEnd = 38163, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LastRepeating_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46715, XrefRangeEnd = 46717, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_LastRepeating_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int ItemCount
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 46718, RefRangeEnd = 46722, XrefRangeStart = 46717, XrefRangeEnd = 46718, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ItemCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe RegionOptions<int> RepeatingGroupsOptions
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46759, XrefRangeEnd = 46763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr);
			IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RepeatingGroupsOptions_Private_get_RegionOptions_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new RegionOptions<int>(pointer);
		}
	}

	public unsafe RegionOptions<float> PositionRangeOptions
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46763, XrefRangeEnd = 46766, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr);
			IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PositionRangeOptions_Private_get_RegionOptions_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new RegionOptions<float>(pointer);
		}
	}

	static CGBoundsGroup()
	{
		Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGBoundsGroup");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr);
		NativeFieldInfoPtr_m_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_Name");
		NativeFieldInfoPtr_m_KeepTogether = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_KeepTogether");
		NativeFieldInfoPtr_m_SpaceBefore = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_SpaceBefore");
		NativeFieldInfoPtr_m_SpaceAfter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_SpaceAfter");
		NativeFieldInfoPtr_m_CrossBase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_CrossBase");
		NativeFieldInfoPtr_m_IgnoreModuleCrossBase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_IgnoreModuleCrossBase");
		NativeFieldInfoPtr_m_RandomizeItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RandomizeItems");
		NativeFieldInfoPtr_m_RepeatingItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RepeatingItems");
		NativeFieldInfoPtr_m_RelativeTranslation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RelativeTranslation");
		NativeFieldInfoPtr_m_TranslationX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_TranslationX");
		NativeFieldInfoPtr_m_TranslationY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_TranslationY");
		NativeFieldInfoPtr_m_TranslationZ = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_TranslationZ");
		NativeFieldInfoPtr_m_RotationMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RotationMode");
		NativeFieldInfoPtr_m_RotationX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RotationX");
		NativeFieldInfoPtr_m_RotationY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RotationY");
		NativeFieldInfoPtr_m_RotationZ = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RotationZ");
		NativeFieldInfoPtr_m_UniformScaling = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_UniformScaling");
		NativeFieldInfoPtr_m_ScaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_ScaleX");
		NativeFieldInfoPtr_m_ScaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_ScaleY");
		NativeFieldInfoPtr_m_ScaleZ = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_ScaleZ");
		NativeFieldInfoPtr_m_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_Items");
		NativeFieldInfoPtr_m_DistributionMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_DistributionMode");
		NativeFieldInfoPtr_m_PositionOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_PositionOffset");
		NativeFieldInfoPtr_m_Height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_Height");
		NativeFieldInfoPtr_m_RepeatingOrder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RepeatingOrder");
		NativeFieldInfoPtr_m_RotationOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RotationOffset");
		NativeFieldInfoPtr_m_RotationScatter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, "m_RotationScatter");
		NativeMethodInfoPtr_get_Name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665720);
		NativeMethodInfoPtr_set_Name_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665721);
		NativeMethodInfoPtr_get_KeepTogether_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665722);
		NativeMethodInfoPtr_set_KeepTogether_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665723);
		NativeMethodInfoPtr_get_SpaceBefore_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665724);
		NativeMethodInfoPtr_set_SpaceBefore_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665725);
		NativeMethodInfoPtr_get_SpaceAfter_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665726);
		NativeMethodInfoPtr_set_SpaceAfter_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665727);
		NativeMethodInfoPtr_get_RandomizeItems_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665728);
		NativeMethodInfoPtr_set_RandomizeItems_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665729);
		NativeMethodInfoPtr_get_RepeatingItems_Public_get_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665730);
		NativeMethodInfoPtr_set_RepeatingItems_Public_set_Void_IntRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665731);
		NativeMethodInfoPtr_get_CrossBase_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665732);
		NativeMethodInfoPtr_set_CrossBase_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665733);
		NativeMethodInfoPtr_get_IgnoreModuleCrossBase_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665734);
		NativeMethodInfoPtr_set_IgnoreModuleCrossBase_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665735);
		NativeMethodInfoPtr_get_RotationMode_Public_get_RotationModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665736);
		NativeMethodInfoPtr_set_RotationMode_Public_set_Void_RotationModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665737);
		NativeMethodInfoPtr_get_RotationX_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665738);
		NativeMethodInfoPtr_set_RotationX_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665739);
		NativeMethodInfoPtr_get_RotationY_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665740);
		NativeMethodInfoPtr_set_RotationY_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665741);
		NativeMethodInfoPtr_get_RotationZ_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665742);
		NativeMethodInfoPtr_set_RotationZ_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665743);
		NativeMethodInfoPtr_get_UniformScaling_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665744);
		NativeMethodInfoPtr_set_UniformScaling_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665745);
		NativeMethodInfoPtr_get_ScaleX_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665746);
		NativeMethodInfoPtr_set_ScaleX_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665747);
		NativeMethodInfoPtr_get_ScaleY_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665748);
		NativeMethodInfoPtr_set_ScaleY_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665749);
		NativeMethodInfoPtr_get_ScaleZ_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665750);
		NativeMethodInfoPtr_set_ScaleZ_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665751);
		NativeMethodInfoPtr_get_RelativeTranslation_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665752);
		NativeMethodInfoPtr_set_RelativeTranslation_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665753);
		NativeMethodInfoPtr_get_TranslationX_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665754);
		NativeMethodInfoPtr_set_TranslationX_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665755);
		NativeMethodInfoPtr_get_TranslationY_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665756);
		NativeMethodInfoPtr_set_TranslationY_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665757);
		NativeMethodInfoPtr_get_TranslationZ_Public_get_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665758);
		NativeMethodInfoPtr_set_TranslationZ_Public_set_Void_FloatRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665759);
		NativeMethodInfoPtr_get_Items_Public_get_List_1_CGBoundsGroupItem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665760);
		NativeMethodInfoPtr_get_FirstRepeating_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665761);
		NativeMethodInfoPtr_set_FirstRepeating_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665762);
		NativeMethodInfoPtr_get_LastRepeating_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665763);
		NativeMethodInfoPtr_set_LastRepeating_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665764);
		NativeMethodInfoPtr_get_ItemCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665765);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665766);
		NativeMethodInfoPtr_FillItemBag_Public_Static_Void_WeightedRandom_1_Int32_IEnumerable_1_CGWeightedItem_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665767);
		NativeMethodInfoPtr_get_RepeatingGroupsOptions_Private_get_RegionOptions_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665768);
		NativeMethodInfoPtr_get_PositionRangeOptions_Private_get_RegionOptions_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665769);
		NativeMethodInfoPtr_ConvertObsoleteData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr, 100665770);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 46743, RefRangeEnd = 46745, XrefRangeStart = 46722, XrefRangeEnd = 46743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGBoundsGroup(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGBoundsGroup>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 46757, RefRangeEnd = 46759, XrefRangeStart = 46745, XrefRangeEnd = 46757, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FillItemBag(WeightedRandom<int> bag, IEnumerable<CGWeightedItem> itemsWeights, int firstItem, int lastItem)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bag);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemsWeights);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &firstItem;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &lastItem;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillItemBag_Public_Static_Void_WeightedRandom_1_Int32_IEnumerable_1_CGWeightedItem_Int32_Int32_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 46775, RefRangeEnd = 46776, XrefRangeStart = 46766, XrefRangeEnd = 46775, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConvertObsoleteData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertObsoleteData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGBoundsGroup(IntPtr pointer)
		: base(pointer)
	{
	}
}
