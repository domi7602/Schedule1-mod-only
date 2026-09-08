using System;
using System.Runtime.CompilerServices;
using Il2CppBeautify.Universal;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppScheduleOne.FX;

public class PlayerHealthVisuals : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_PPVolumes;

	private static readonly IntPtr NativeFieldInfoPtr_VignetteAlpha_MaxHealth;

	private static readonly IntPtr NativeFieldInfoPtr_VignetteAlpha_MinHealth;

	private static readonly IntPtr NativeFieldInfoPtr_OuterRingCurve;

	private static readonly IntPtr NativeFieldInfoPtr_Saturation_MaxHealth;

	private static readonly IntPtr NativeFieldInfoPtr_Saturation_MinHealth;

	private static readonly IntPtr NativeFieldInfoPtr_ChromAb_MaxHealth;

	private static readonly IntPtr NativeFieldInfoPtr_ChromAb_MinHealth;

	private static readonly IntPtr NativeFieldInfoPtr_LensDirt_MaxHealth;

	private static readonly IntPtr NativeFieldInfoPtr_LensDirt_MinHealth;

	private static readonly IntPtr NativeFieldInfoPtr__beautifySettings;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Spawned_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_MinPass_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateEffects_Private_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<Volume> PPVolumes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PPVolumes);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Volume>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PPVolumes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float VignetteAlpha_MaxHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VignetteAlpha_MaxHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VignetteAlpha_MaxHealth)) = num;
		}
	}

	public unsafe float VignetteAlpha_MinHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VignetteAlpha_MinHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VignetteAlpha_MinHealth)) = num;
		}
	}

	public unsafe AnimationCurve OuterRingCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OuterRingCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OuterRingCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float Saturation_MaxHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Saturation_MaxHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Saturation_MaxHealth)) = num;
		}
	}

	public unsafe float Saturation_MinHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Saturation_MinHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Saturation_MinHealth)) = num;
		}
	}

	public unsafe float ChromAb_MaxHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChromAb_MaxHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChromAb_MaxHealth)) = num;
		}
	}

	public unsafe float ChromAb_MinHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChromAb_MinHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChromAb_MinHealth)) = num;
		}
	}

	public unsafe float LensDirt_MaxHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LensDirt_MaxHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LensDirt_MaxHealth)) = num;
		}
	}

	public unsafe float LensDirt_MinHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LensDirt_MinHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LensDirt_MinHealth)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<Beautify> _beautifySettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__beautifySettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Beautify>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__beautifySettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static PlayerHealthVisuals()
	{
		Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.FX", "PlayerHealthVisuals");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr);
		NativeFieldInfoPtr_PPVolumes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "PPVolumes");
		NativeFieldInfoPtr_VignetteAlpha_MaxHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "VignetteAlpha_MaxHealth");
		NativeFieldInfoPtr_VignetteAlpha_MinHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "VignetteAlpha_MinHealth");
		NativeFieldInfoPtr_OuterRingCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "OuterRingCurve");
		NativeFieldInfoPtr_Saturation_MaxHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "Saturation_MaxHealth");
		NativeFieldInfoPtr_Saturation_MinHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "Saturation_MinHealth");
		NativeFieldInfoPtr_ChromAb_MaxHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "ChromAb_MaxHealth");
		NativeFieldInfoPtr_ChromAb_MinHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "ChromAb_MinHealth");
		NativeFieldInfoPtr_LensDirt_MaxHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "LensDirt_MaxHealth");
		NativeFieldInfoPtr_LensDirt_MinHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "LensDirt_MinHealth");
		NativeFieldInfoPtr__beautifySettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, "_beautifySettings");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, 100673696);
		NativeMethodInfoPtr_Spawned_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, 100673697);
		NativeMethodInfoPtr_MinPass_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, 100673698);
		NativeMethodInfoPtr_UpdateEffects_Private_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, 100673699);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr, 100673700);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 178821, XrefRangeEnd = 178882, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 178882, XrefRangeEnd = 178902, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Spawned()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Spawned_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 178902, XrefRangeEnd = 178909, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MinPass_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 178915, RefRangeEnd = 178916, XrefRangeStart = 178909, XrefRangeEnd = 178915, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateEffects(float newHealth)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&newHealth);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateEffects_Private_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 178916, XrefRangeEnd = 178917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlayerHealthVisuals()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlayerHealthVisuals>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlayerHealthVisuals(IntPtr pointer)
		: base(pointer)
	{
	}
}
