using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Il2CppScheduleOne.FX;

public class PsychedelicFullScreenFeature : ScriptableRendererFeature
{
	[System.Serializable]
	public class Settings : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_profilerTag;

		private static readonly System.IntPtr NativeFieldInfoPtr_renderPassEvent;

		private static readonly System.IntPtr NativeFieldInfoPtr_passMaterial;

		private static readonly System.IntPtr NativeFieldInfoPtr_ActiveProperties;

		private static readonly System.IntPtr NativeFieldInfoPtr_MaterialPresets;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string profilerTag
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_profilerTag);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_profilerTag)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe RenderPassEvent renderPassEvent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderPassEvent);
				return *(RenderPassEvent*)num;
			}
			set
			{
				*(RenderPassEvent*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_renderPassEvent)) = renderPassEvent;
			}
		}

		public unsafe Material passMaterial
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_passMaterial);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_passMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
			}
		}

		public unsafe MaterialProperties ActiveProperties
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveProperties);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MaterialProperties>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveProperties)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialProperties));
			}
		}

		public unsafe List<MaterialPropertyPreset> MaterialPresets
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialPresets);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<MaterialPropertyPreset>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialPresets)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		static Settings()
		{
			Il2CppClassPointerStore<Settings>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "Settings");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Settings>.NativeClassPtr);
			NativeFieldInfoPtr_profilerTag = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "profilerTag");
			NativeFieldInfoPtr_renderPassEvent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "renderPassEvent");
			NativeFieldInfoPtr_passMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "passMaterial");
			NativeFieldInfoPtr_ActiveProperties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "ActiveProperties");
			NativeFieldInfoPtr_MaterialPresets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "MaterialPresets");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Settings>.NativeClassPtr, 100673731);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179068, XrefRangeEnd = 179073, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Settings()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Settings>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Settings(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	public class MaterialPropertyPreset : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Name;

		private static readonly System.IntPtr NativeFieldInfoPtr_Data;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string Name
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe PsychedelicFullScreenData Data
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Data);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PsychedelicFullScreenData>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)psychedelicFullScreenData));
			}
		}

		static MaterialPropertyPreset()
		{
			Il2CppClassPointerStore<MaterialPropertyPreset>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "MaterialPropertyPreset");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialPropertyPreset>.NativeClassPtr);
			NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialPropertyPreset>.NativeClassPtr, "Name");
			NativeFieldInfoPtr_Data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialPropertyPreset>.NativeClassPtr, "Data");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialPropertyPreset>.NativeClassPtr, 100673732);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MaterialPropertyPreset()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialPropertyPreset>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MaterialPropertyPreset(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	public class MaterialProperties : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_NoiseScale;

		private static readonly System.IntPtr NativeFieldInfoPtr_Blend;

		private static readonly System.IntPtr NativeFieldInfoPtr_PanSpeed;

		private static readonly System.IntPtr NativeFieldInfoPtr_DoesBounce;

		private static readonly System.IntPtr NativeFieldInfoPtr_Amplitude;

		private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_MaterialProperties_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe float NoiseScale
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoiseScale);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoiseScale)) = num;
			}
		}

		public unsafe float Blend
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Blend);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Blend)) = num;
			}
		}

		public unsafe Vector2 PanSpeed
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PanSpeed);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PanSpeed)) = vector;
			}
		}

		public unsafe bool DoesBounce
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DoesBounce);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DoesBounce)) = flag;
			}
		}

		public unsafe float Amplitude
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amplitude);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amplitude)) = num;
			}
		}

		static MaterialProperties()
		{
			Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "MaterialProperties");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr);
			NativeFieldInfoPtr_NoiseScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, "NoiseScale");
			NativeFieldInfoPtr_Blend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, "Blend");
			NativeFieldInfoPtr_PanSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, "PanSpeed");
			NativeFieldInfoPtr_DoesBounce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, "DoesBounce");
			NativeFieldInfoPtr_Amplitude = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, "Amplitude");
			NativeMethodInfoPtr_Clone_Public_MaterialProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, 100673733);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr, 100673734);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 179077, RefRangeEnd = 179078, XrefRangeStart = 179073, XrefRangeEnd = 179077, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MaterialProperties Clone()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_MaterialProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MaterialProperties>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 179079, RefRangeEnd = 179080, XrefRangeStart = 179078, XrefRangeEnd = 179079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MaterialProperties()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialProperties>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MaterialProperties(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__settings;

	private static readonly System.IntPtr NativeFieldInfoPtr_BLEND_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr_NOISE_SCALE_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr_PAN_SPEED_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr_DOES_BOUNCE_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr_AMPLITUDE_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr__psychedelicPass;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FeatureSettings_Public_get_Settings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ActiveMaterialProperties_Public_get_MaterialProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetActiveMaterialProperties_Public_Void_MaterialProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrintMaterialValue_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaterialPreset_Public_PsychedelicFullScreenData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Settings _settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Settings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings));
		}
	}

	public unsafe static int BLEND_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BLEND_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BLEND_ID, (void*)(&num));
		}
	}

	public unsafe static int NOISE_SCALE_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NOISE_SCALE_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NOISE_SCALE_ID, (void*)(&num));
		}
	}

	public unsafe static int PAN_SPEED_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PAN_SPEED_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PAN_SPEED_ID, (void*)(&num));
		}
	}

	public unsafe static int DOES_BOUNCE_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DOES_BOUNCE_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DOES_BOUNCE_ID, (void*)(&num));
		}
	}

	public unsafe static int AMPLITUDE_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AMPLITUDE_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AMPLITUDE_ID, (void*)(&num));
		}
	}

	public unsafe PsychedelicFullScreenPass _psychedelicPass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__psychedelicPass);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PsychedelicFullScreenPass>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__psychedelicPass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)psychedelicFullScreenPass));
		}
	}

	public unsafe Settings FeatureSettings
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FeatureSettings_Public_get_Settings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Settings>(intPtr) : null;
		}
	}

	public unsafe MaterialProperties ActiveMaterialProperties
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ActiveMaterialProperties_Public_get_MaterialProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MaterialProperties>(intPtr) : null;
		}
	}

	static PsychedelicFullScreenFeature()
	{
		Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.FX", "PsychedelicFullScreenFeature");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr);
		NativeFieldInfoPtr__settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "_settings");
		NativeFieldInfoPtr_BLEND_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "BLEND_ID");
		NativeFieldInfoPtr_NOISE_SCALE_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "NOISE_SCALE_ID");
		NativeFieldInfoPtr_PAN_SPEED_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "PAN_SPEED_ID");
		NativeFieldInfoPtr_DOES_BOUNCE_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "DOES_BOUNCE_ID");
		NativeFieldInfoPtr_AMPLITUDE_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "AMPLITUDE_ID");
		NativeFieldInfoPtr__psychedelicPass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, "_psychedelicPass");
		NativeMethodInfoPtr_get_FeatureSettings_Public_get_Settings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673722);
		NativeMethodInfoPtr_get_ActiveMaterialProperties_Public_get_MaterialProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673723);
		NativeMethodInfoPtr_Create_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673724);
		NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673725);
		NativeMethodInfoPtr_SetActiveMaterialProperties_Public_Void_MaterialProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673726);
		NativeMethodInfoPtr_PrintMaterialValue_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673727);
		NativeMethodInfoPtr_GetMaterialPreset_Public_PsychedelicFullScreenData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673728);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr, 100673729);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179080, XrefRangeEnd = 179091, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Create()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Create_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179091, XrefRangeEnd = 179103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_AddRenderPasses_Public_Virtual_Void_ScriptableRenderer_byref_RenderingData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void SetActiveMaterialProperties(MaterialProperties properties)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)properties);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetActiveMaterialProperties_Public_Void_MaterialProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179103, XrefRangeEnd = 179116, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PrintMaterialValue()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrintMaterialValue_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 179127, RefRangeEnd = 179129, XrefRangeStart = 179116, XrefRangeEnd = 179127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PsychedelicFullScreenData GetMaterialPreset(string presetName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(presetName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaterialPreset_Public_PsychedelicFullScreenData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PsychedelicFullScreenData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179129, XrefRangeEnd = 179139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PsychedelicFullScreenFeature()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PsychedelicFullScreenFeature>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PsychedelicFullScreenFeature(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
