using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using UnityEngine;

namespace Il2CppScheduleOne.PlayerTasks.Tasks;

public class MistMushroomBedTask : Task
{
	private static readonly IntPtr NativeFieldInfoPtr__TaskName_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_TARGET_SPRAY_RADIUS;

	private static readonly IntPtr NativeFieldInfoPtr_TARGET_SPRAY_DISTANCE;

	private static readonly IntPtr NativeFieldInfoPtr__mushroomBed;

	private static readonly IntPtr NativeFieldInfoPtr__sprayable;

	private static readonly IntPtr NativeFieldInfoPtr__sprayableObj;

	private static readonly IntPtr NativeFieldInfoPtr__waterContainerInstance;

	private static readonly IntPtr NativeMethodInfoPtr_get_TaskName_Public_Virtual_get_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_TaskName_Protected_Virtual_set_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_ItemInstance_GameObject_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSuccessfulSpray_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSpray_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0;

	public new unsafe string _TaskName_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__TaskName_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__TaskName_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static float TARGET_SPRAY_RADIUS
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TARGET_SPRAY_RADIUS, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TARGET_SPRAY_RADIUS, (void*)(&num));
		}
	}

	public unsafe static float TARGET_SPRAY_DISTANCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TARGET_SPRAY_DISTANCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TARGET_SPRAY_DISTANCE, (void*)(&num));
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

	public unsafe Sprayable _sprayable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sprayable);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Sprayable>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sprayable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprayable));
		}
	}

	public unsafe GameObject _sprayableObj
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sprayableObj);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sprayableObj)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe WaterContainerInstance _waterContainerInstance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__waterContainerInstance);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WaterContainerInstance>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__waterContainerInstance)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)waterContainerInstance));
		}
	}

	public unsafe override string TaskName
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_TaskName_Public_Virtual_get_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_set_TaskName_Protected_Virtual_set_Void_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static MistMushroomBedTask()
	{
		Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerTasks.Tasks", "MistMushroomBedTask");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr);
		NativeFieldInfoPtr__TaskName_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "<TaskName>k__BackingField");
		NativeFieldInfoPtr_TARGET_SPRAY_RADIUS = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "TARGET_SPRAY_RADIUS");
		NativeFieldInfoPtr_TARGET_SPRAY_DISTANCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "TARGET_SPRAY_DISTANCE");
		NativeFieldInfoPtr__mushroomBed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "_mushroomBed");
		NativeFieldInfoPtr__sprayable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "_sprayable");
		NativeFieldInfoPtr__sprayableObj = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "_sprayableObj");
		NativeFieldInfoPtr__waterContainerInstance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, "_waterContainerInstance");
		NativeMethodInfoPtr_get_TaskName_Public_Virtual_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, 100668481);
		NativeMethodInfoPtr_set_TaskName_Protected_Virtual_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, 100668482);
		NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_ItemInstance_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, 100668483);
		NativeMethodInfoPtr_OnSuccessfulSpray_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, 100668484);
		NativeMethodInfoPtr_OnSpray_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, 100668485);
		NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr, 100668486);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 121395, RefRangeEnd = 121396, XrefRangeStart = 121294, XrefRangeEnd = 121395, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MistMushroomBedTask(MushroomBed mushroomBed, ItemInstance item, GameObject sprayablePrefab)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MistMushroomBedTask>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mushroomBed);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprayablePrefab);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_ItemInstance_GameObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 121396, XrefRangeEnd = 121408, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSuccessfulSpray()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSuccessfulSpray_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 121408, XrefRangeEnd = 121410, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSpray()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSpray_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 121410, XrefRangeEnd = 121444, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void StopTask()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MistMushroomBedTask(IntPtr pointer)
		: base(pointer)
	{
	}
}
