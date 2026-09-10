using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.ItemFramework;

[Serializable]
public class SoilDefinition : StorableItemDefinition
{
	[OriginalName("Assembly-CSharp.dll", "", "ESoilQuality")]
	public enum ESoilQuality
	{
		Basic,
		Premium
	}

	private static readonly IntPtr NativeFieldInfoPtr_SoilQuality;

	private static readonly IntPtr NativeFieldInfoPtr_DrySoilMat;

	private static readonly IntPtr NativeFieldInfoPtr_WetSoilMat;

	private static readonly IntPtr NativeFieldInfoPtr_ParticleColor;

	private static readonly IntPtr NativeFieldInfoPtr_Uses;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ESoilQuality SoilQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SoilQuality);
			return *(ESoilQuality*)num;
		}
		set
		{
			*(ESoilQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SoilQuality)) = eSoilQuality;
		}
	}

	public unsafe Material DrySoilMat
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DrySoilMat);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DrySoilMat)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material WetSoilMat
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WetSoilMat);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WetSoilMat)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Color ParticleColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ParticleColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ParticleColor)) = color;
		}
	}

	public unsafe int Uses
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Uses);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Uses)) = num;
		}
	}

	static SoilDefinition()
	{
		Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "SoilDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_SoilQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr, "SoilQuality");
		NativeFieldInfoPtr_DrySoilMat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr, "DrySoilMat");
		NativeFieldInfoPtr_WetSoilMat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr, "WetSoilMat");
		NativeFieldInfoPtr_ParticleColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr, "ParticleColor");
		NativeFieldInfoPtr_Uses = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr, "Uses");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr, 100672450);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166835, XrefRangeEnd = 166836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SoilDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SoilDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SoilDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
