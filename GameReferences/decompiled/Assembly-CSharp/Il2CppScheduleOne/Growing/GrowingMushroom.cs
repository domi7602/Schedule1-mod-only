using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using UnityEngine;

namespace Il2CppScheduleOne.Growing;

public class GrowingMushroom : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_CapExpansionThreshold;

	private static readonly IntPtr NativeFieldInfoPtr_LateralScaleMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_VerticalScaleMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_MaxCapExpansion;

	private static readonly IntPtr NativeFieldInfoPtr__modelContainer;

	private static readonly IntPtr NativeFieldInfoPtr__meshRenderers;

	private static readonly IntPtr NativeFieldInfoPtr__harvestSound;

	private static readonly IntPtr NativeFieldInfoPtr__parentColony;

	private static readonly IntPtr NativeFieldInfoPtr__alignmentIndex;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_ShroomColony_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetGrowthPercent_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Harvest_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float CapExpansionThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CapExpansionThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CapExpansionThreshold, (void*)(&num));
		}
	}

	public unsafe float LateralScaleMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LateralScaleMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LateralScaleMultiplier)) = num;
		}
	}

	public unsafe float VerticalScaleMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VerticalScaleMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VerticalScaleMultiplier)) = num;
		}
	}

	public unsafe float MaxCapExpansion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxCapExpansion);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxCapExpansion)) = num;
		}
	}

	public unsafe Transform _modelContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__modelContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__modelContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Il2CppReferenceArray<SkinnedMeshRenderer> _meshRenderers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__meshRenderers);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SkinnedMeshRenderer>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__meshRenderers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe AudioSourceController _harvestSound
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__harvestSound);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__harvestSound)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe ShroomColony _parentColony
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parentColony);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomColony>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parentColony)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shroomColony));
		}
	}

	public unsafe int _alignmentIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__alignmentIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__alignmentIndex)) = num;
		}
	}

	static GrowingMushroom()
	{
		Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "GrowingMushroom");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr);
		NativeFieldInfoPtr_CapExpansionThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "CapExpansionThreshold");
		NativeFieldInfoPtr_LateralScaleMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "LateralScaleMultiplier");
		NativeFieldInfoPtr_VerticalScaleMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "VerticalScaleMultiplier");
		NativeFieldInfoPtr_MaxCapExpansion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "MaxCapExpansion");
		NativeFieldInfoPtr__modelContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "_modelContainer");
		NativeFieldInfoPtr__meshRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "_meshRenderers");
		NativeFieldInfoPtr__harvestSound = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "_harvestSound");
		NativeFieldInfoPtr__parentColony = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "_parentColony");
		NativeFieldInfoPtr__alignmentIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, "_alignmentIndex");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, 100678521);
		NativeMethodInfoPtr_Initialize_Public_Void_ShroomColony_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, 100678522);
		NativeMethodInfoPtr_SetGrowthPercent_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, 100678523);
		NativeMethodInfoPtr_Harvest_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, 100678524);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr, 100678525);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 229908, RefRangeEnd = 229909, XrefRangeStart = 229907, XrefRangeEnd = 229908, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialize(ShroomColony parentColony, int alignmentIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parentColony);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &alignmentIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_ShroomColony_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 229918, RefRangeEnd = 229920, XrefRangeStart = 229909, XrefRangeEnd = 229918, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetGrowthPercent(float percent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&percent);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetGrowthPercent_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 229953, RefRangeEnd = 229954, XrefRangeStart = 229920, XrefRangeEnd = 229953, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Harvest()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Harvest_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 229954, XrefRangeEnd = 229955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GrowingMushroom()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GrowingMushroom>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GrowingMushroom(IntPtr pointer)
		: base(pointer)
	{
	}
}
