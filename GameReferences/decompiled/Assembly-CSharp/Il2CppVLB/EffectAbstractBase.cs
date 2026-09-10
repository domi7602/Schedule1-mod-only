using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppVLB;

public class EffectAbstractBase : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "ComponentsToChange")]
	[Flags]
	public enum ComponentsToChange
	{
		UnityLight = 1,
		VolumetricLightBeam = 2,
		VolumetricDustParticles = 4
	}

	private static readonly IntPtr NativeFieldInfoPtr_ClassName;

	private static readonly IntPtr NativeFieldInfoPtr_componentsToChange;

	private static readonly IntPtr NativeFieldInfoPtr_restoreIntensityOnDisable;

	private static readonly IntPtr NativeFieldInfoPtr_m_Beam;

	private static readonly IntPtr NativeFieldInfoPtr_m_Light;

	private static readonly IntPtr NativeFieldInfoPtr_m_Particles;

	private static readonly IntPtr NativeFieldInfoPtr_m_BaseIntensityBeamInside;

	private static readonly IntPtr NativeFieldInfoPtr_m_BaseIntensityBeamOutside;

	private static readonly IntPtr NativeFieldInfoPtr_m_BaseIntensityLight;

	private static readonly IntPtr NativeMethodInfoPtr_get_restoreBaseIntensity_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_restoreBaseIntensity_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_InitFrom_Public_Virtual_New_Void_EffectAbstractBase_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetIntensity_Private_Void_VolumetricLightBeamSD_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetIntensity_Private_Void_VolumetricLightBeamHD_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetIntensity_Private_Void_VolumetricLightBeamSD_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetIntensity_Private_Void_VolumetricLightBeamHD_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetAdditiveIntensity_Protected_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static string ClassName
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ClassName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ClassName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe ComponentsToChange componentsToChange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_componentsToChange);
			return *(ComponentsToChange*)num;
		}
		set
		{
			*(ComponentsToChange*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_componentsToChange)) = componentsToChange;
		}
	}

	public unsafe bool restoreIntensityOnDisable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_restoreIntensityOnDisable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_restoreIntensityOnDisable)) = flag;
		}
	}

	public unsafe VolumetricLightBeamAbstractBase m_Beam
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Beam);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VolumetricLightBeamAbstractBase>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Beam)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volumetricLightBeamAbstractBase));
		}
	}

	public unsafe Light m_Light
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Light);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Light>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Light)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light));
		}
	}

	public unsafe VolumetricDustParticles m_Particles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Particles);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VolumetricDustParticles>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Particles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volumetricDustParticles));
		}
	}

	public unsafe float m_BaseIntensityBeamInside
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_BaseIntensityBeamInside);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_BaseIntensityBeamInside)) = num;
		}
	}

	public unsafe float m_BaseIntensityBeamOutside
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_BaseIntensityBeamOutside);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_BaseIntensityBeamOutside)) = num;
		}
	}

	public unsafe float m_BaseIntensityLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_BaseIntensityLight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_BaseIntensityLight)) = num;
		}
	}

	public unsafe bool restoreBaseIntensity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_restoreBaseIntensity_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_restoreBaseIntensity_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static EffectAbstractBase()
	{
		Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "EffectAbstractBase");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr);
		NativeFieldInfoPtr_ClassName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "ClassName");
		NativeFieldInfoPtr_componentsToChange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "componentsToChange");
		NativeFieldInfoPtr_restoreIntensityOnDisable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "restoreIntensityOnDisable");
		NativeFieldInfoPtr_m_Beam = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "m_Beam");
		NativeFieldInfoPtr_m_Light = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "m_Light");
		NativeFieldInfoPtr_m_Particles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "m_Particles");
		NativeFieldInfoPtr_m_BaseIntensityBeamInside = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "m_BaseIntensityBeamInside");
		NativeFieldInfoPtr_m_BaseIntensityBeamOutside = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "m_BaseIntensityBeamOutside");
		NativeFieldInfoPtr_m_BaseIntensityLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, "m_BaseIntensityLight");
		NativeMethodInfoPtr_get_restoreBaseIntensity_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663741);
		NativeMethodInfoPtr_set_restoreBaseIntensity_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663742);
		NativeMethodInfoPtr_InitFrom_Public_Virtual_New_Void_EffectAbstractBase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663743);
		NativeMethodInfoPtr_GetIntensity_Private_Void_VolumetricLightBeamSD_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663744);
		NativeMethodInfoPtr_GetIntensity_Private_Void_VolumetricLightBeamHD_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663745);
		NativeMethodInfoPtr_SetIntensity_Private_Void_VolumetricLightBeamSD_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663746);
		NativeMethodInfoPtr_SetIntensity_Private_Void_VolumetricLightBeamHD_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663747);
		NativeMethodInfoPtr_SetAdditiveIntensity_Protected_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663748);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663749);
		NativeMethodInfoPtr_OnEnable_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663750);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663751);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr, 100663752);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69376, XrefRangeEnd = 69380, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void InitFrom(EffectAbstractBase Source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)Source);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_InitFrom_Public_Virtual_New_Void_EffectAbstractBase_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69380, XrefRangeEnd = 69384, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetIntensity(VolumetricLightBeamSD beam)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beam);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIntensity_Private_Void_VolumetricLightBeamSD_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69384, XrefRangeEnd = 69388, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetIntensity(VolumetricLightBeamHD beam)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beam);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIntensity_Private_Void_VolumetricLightBeamHD_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69388, XrefRangeEnd = 69392, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetIntensity(VolumetricLightBeamSD beam, float additive)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beam);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &additive;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetIntensity_Private_Void_VolumetricLightBeamSD_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69392, XrefRangeEnd = 69399, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetIntensity(VolumetricLightBeamHD beam, float additive)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beam);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &additive;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetIntensity_Private_Void_VolumetricLightBeamHD_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 69427, RefRangeEnd = 69430, XrefRangeStart = 69399, XrefRangeEnd = 69427, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetAdditiveIntensity(float additive)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&additive);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAdditiveIntensity_Protected_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69430, XrefRangeEnd = 69461, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69461, XrefRangeEnd = 69462, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnable_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69462, XrefRangeEnd = 69464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 69464, XrefRangeEnd = 69465, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EffectAbstractBase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EffectAbstractBase>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EffectAbstractBase(IntPtr pointer)
		: base(pointer)
	{
	}
}
