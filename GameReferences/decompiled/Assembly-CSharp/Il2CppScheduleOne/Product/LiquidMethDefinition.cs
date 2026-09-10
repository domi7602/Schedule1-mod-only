using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace Il2CppScheduleOne.Product;

[Serializable]
public class LiquidMethDefinition : QualityItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr_StaticLiquidColor;

	private static readonly IntPtr NativeFieldInfoPtr_LiquidVolumeColor;

	private static readonly IntPtr NativeFieldInfoPtr_PourParticlesColor;

	private static readonly IntPtr NativeFieldInfoPtr_CookableLiquidColor;

	private static readonly IntPtr NativeFieldInfoPtr_CookableSolidColor;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Color StaticLiquidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StaticLiquidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StaticLiquidColor)) = color;
		}
	}

	public unsafe Color LiquidVolumeColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LiquidVolumeColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LiquidVolumeColor)) = color;
		}
	}

	public unsafe Color PourParticlesColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PourParticlesColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PourParticlesColor)) = color;
		}
	}

	public unsafe Color CookableLiquidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookableLiquidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookableLiquidColor)) = color;
		}
	}

	public unsafe Color CookableSolidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookableSolidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CookableSolidColor)) = color;
		}
	}

	static LiquidMethDefinition()
	{
		Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "LiquidMethDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_StaticLiquidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr, "StaticLiquidColor");
		NativeFieldInfoPtr_LiquidVolumeColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr, "LiquidVolumeColor");
		NativeFieldInfoPtr_PourParticlesColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr, "PourParticlesColor");
		NativeFieldInfoPtr_CookableLiquidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr, "CookableLiquidColor");
		NativeFieldInfoPtr_CookableSolidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr, "CookableSolidColor");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr, 100679183);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235874, XrefRangeEnd = 235875, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LiquidMethDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LiquidMethDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LiquidMethDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
