using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.ObjectScripts;

namespace Il2CppScheduleOne.PlayerTasks;

public class HarvestMushroomBedTask : Task
{
	private static readonly IntPtr NativeFieldInfoPtr__soundLoop;

	private static readonly IntPtr NativeFieldInfoPtr__mushroomBed;

	private static readonly IntPtr NativeFieldInfoPtr__canDrag;

	private static readonly IntPtr NativeFieldInfoPtr__harvestCount;

	private static readonly IntPtr NativeFieldInfoPtr__harvestTotal;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_Boolean_AudioSourceController_0;

	private static readonly IntPtr NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateInstructionText_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateCursor_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetHoveredHarvestable_Private_GrowingMushroom_0;

	public unsafe AudioSourceController _soundLoop
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soundLoop);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soundLoop)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe MushroomBed _mushroomBed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mushroomBed);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MushroomBed>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mushroomBed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mushroomBed));
		}
	}

	public unsafe bool _canDrag
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canDrag);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canDrag)) = flag;
		}
	}

	public unsafe int _harvestCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__harvestCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__harvestCount)) = num;
		}
	}

	public unsafe int _harvestTotal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__harvestTotal);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__harvestTotal)) = num;
		}
	}

	static HarvestMushroomBedTask()
	{
		Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerTasks", "HarvestMushroomBedTask");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr);
		NativeFieldInfoPtr__soundLoop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, "_soundLoop");
		NativeFieldInfoPtr__mushroomBed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, "_mushroomBed");
		NativeFieldInfoPtr__canDrag = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, "_canDrag");
		NativeFieldInfoPtr__harvestCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, "_harvestCount");
		NativeFieldInfoPtr__harvestTotal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, "_harvestTotal");
		NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_Boolean_AudioSourceController_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, 100668345);
		NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, 100668346);
		NativeMethodInfoPtr_Update_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, 100668347);
		NativeMethodInfoPtr_UpdateInstructionText_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, 100668348);
		NativeMethodInfoPtr_UpdateCursor_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, 100668349);
		NativeMethodInfoPtr_GetHoveredHarvestable_Private_GrowingMushroom_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr, 100668350);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 119055, XrefRangeEnd = 119116, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HarvestMushroomBedTask(MushroomBed mushroomBed, bool canDrag, AudioSourceController soundLoopPrefab)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HarvestMushroomBedTask>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mushroomBed);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &canDrag;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)soundLoopPrefab);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_Boolean_AudioSourceController_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 119116, XrefRangeEnd = 119148, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void StopTask()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 119148, XrefRangeEnd = 119220, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 119247, RefRangeEnd = 119249, XrefRangeStart = 119220, XrefRangeEnd = 119247, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateInstructionText()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateInstructionText_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 119249, XrefRangeEnd = 119263, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void UpdateCursor()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_UpdateCursor_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 119280, RefRangeEnd = 119282, XrefRangeStart = 119263, XrefRangeEnd = 119280, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GrowingMushroom GetHoveredHarvestable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHoveredHarvestable_Private_GrowingMushroom_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GrowingMushroom>(intPtr) : null;
	}

	public HarvestMushroomBedTask(IntPtr pointer)
		: base(pointer)
	{
	}
}
