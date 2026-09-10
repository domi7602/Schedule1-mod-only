using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Il2CppScheduleOne.Trash;

public class TrashBag_Equippable : Equippable_Viewmodel
{
	private static readonly IntPtr NativeFieldInfoPtr_TRASH_CONTAINER_INTERACT_DISTANCE;

	private static readonly IntPtr NativeFieldInfoPtr_BAG_TRASH_TIME;

	private static readonly IntPtr NativeFieldInfoPtr_PICKUP_RANGE;

	private static readonly IntPtr NativeFieldInfoPtr_PICKUP_AREA_RADIUS;

	private static readonly IntPtr NativeFieldInfoPtr__IsBaggingTrash_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__IsPickingUpTrash_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_PickupLookMask;

	private static readonly IntPtr NativeFieldInfoPtr_PickupAreaProjector;

	private static readonly IntPtr NativeFieldInfoPtr_RustleSound;

	private static readonly IntPtr NativeFieldInfoPtr_BagSound;

	private static readonly IntPtr NativeFieldInfoPtr__bagTrashTime;

	private static readonly IntPtr NativeFieldInfoPtr__baggedContainer;

	private static readonly IntPtr NativeFieldInfoPtr__pickupTrashTime;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsBaggingTrash_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IsBaggingTrash_Private_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsPickingUpTrash_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IsPickingUpTrash_Private_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetHoveredTrashContainer_Private_TrashContainer_0;

	private static readonly IntPtr NativeMethodInfoPtr_RaycastLook_Private_Boolean_byref_RaycastHit_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsPickupLocationValid_Private_Boolean_RaycastHit_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetTrashItemsAtPoint_Private_List_1_TrashItem_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartBagTrash_Private_Void_TrashContainer_0;

	private static readonly IntPtr NativeMethodInfoPtr_StopBagTrash_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartPickup_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StopPickup_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_ShowPrompt_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HidePrompt_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float TRASH_CONTAINER_INTERACT_DISTANCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TRASH_CONTAINER_INTERACT_DISTANCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TRASH_CONTAINER_INTERACT_DISTANCE, (void*)(&num));
		}
	}

	public unsafe static float BAG_TRASH_TIME
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BAG_TRASH_TIME, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BAG_TRASH_TIME, (void*)(&num));
		}
	}

	public unsafe static float PICKUP_RANGE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PICKUP_RANGE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PICKUP_RANGE, (void*)(&num));
		}
	}

	public unsafe static float PICKUP_AREA_RADIUS
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PICKUP_AREA_RADIUS, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PICKUP_AREA_RADIUS, (void*)(&num));
		}
	}

	public unsafe bool _IsBaggingTrash_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsBaggingTrash_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsBaggingTrash_k__BackingField)) = flag;
		}
	}

	public unsafe bool _IsPickingUpTrash_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsPickingUpTrash_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsPickingUpTrash_k__BackingField)) = flag;
		}
	}

	public unsafe LayerMask PickupLookMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickupLookMask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickupLookMask)) = layerMask;
		}
	}

	public unsafe DecalProjector PickupAreaProjector
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickupAreaProjector);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DecalProjector>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PickupAreaProjector)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)decalProjector));
		}
	}

	public unsafe AudioSourceController RustleSound
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RustleSound);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RustleSound)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe AudioSourceController BagSound
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BagSound);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BagSound)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe float _bagTrashTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bagTrashTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bagTrashTime)) = num;
		}
	}

	public unsafe TrashContainer _baggedContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__baggedContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TrashContainer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__baggedContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashContainer));
		}
	}

	public unsafe float _pickupTrashTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pickupTrashTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pickupTrashTime)) = num;
		}
	}

	public unsafe bool IsBaggingTrash
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsBaggingTrash_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsBaggingTrash_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsPickingUpTrash
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsPickingUpTrash_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsPickingUpTrash_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static TrashBag_Equippable()
	{
		Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Trash", "TrashBag_Equippable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr);
		NativeFieldInfoPtr_TRASH_CONTAINER_INTERACT_DISTANCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "TRASH_CONTAINER_INTERACT_DISTANCE");
		NativeFieldInfoPtr_BAG_TRASH_TIME = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "BAG_TRASH_TIME");
		NativeFieldInfoPtr_PICKUP_RANGE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "PICKUP_RANGE");
		NativeFieldInfoPtr_PICKUP_AREA_RADIUS = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "PICKUP_AREA_RADIUS");
		NativeFieldInfoPtr__IsBaggingTrash_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "<IsBaggingTrash>k__BackingField");
		NativeFieldInfoPtr__IsPickingUpTrash_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "<IsPickingUpTrash>k__BackingField");
		NativeFieldInfoPtr_PickupLookMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "PickupLookMask");
		NativeFieldInfoPtr_PickupAreaProjector = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "PickupAreaProjector");
		NativeFieldInfoPtr_RustleSound = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "RustleSound");
		NativeFieldInfoPtr_BagSound = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "BagSound");
		NativeFieldInfoPtr__bagTrashTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "_bagTrashTime");
		NativeFieldInfoPtr__baggedContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "_baggedContainer");
		NativeFieldInfoPtr__pickupTrashTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, "_pickupTrashTime");
		NativeMethodInfoPtr_get_IsBaggingTrash_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676953);
		NativeMethodInfoPtr_set_IsBaggingTrash_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676954);
		NativeMethodInfoPtr_get_IsPickingUpTrash_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676955);
		NativeMethodInfoPtr_set_IsPickingUpTrash_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676956);
		NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676957);
		NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676958);
		NativeMethodInfoPtr_Update_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676959);
		NativeMethodInfoPtr_GetHoveredTrashContainer_Private_TrashContainer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676960);
		NativeMethodInfoPtr_RaycastLook_Private_Boolean_byref_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676961);
		NativeMethodInfoPtr_IsPickupLocationValid_Private_Boolean_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676962);
		NativeMethodInfoPtr_GetTrashItemsAtPoint_Private_List_1_TrashItem_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676963);
		NativeMethodInfoPtr_StartBagTrash_Private_Void_TrashContainer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676964);
		NativeMethodInfoPtr_StopBagTrash_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676965);
		NativeMethodInfoPtr_StartPickup_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676966);
		NativeMethodInfoPtr_StopPickup_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676967);
		NativeMethodInfoPtr_ShowPrompt_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676968);
		NativeMethodInfoPtr_HidePrompt_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676969);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr, 100676970);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216169, XrefRangeEnd = 216200, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Equip(ItemInstance item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216200, XrefRangeEnd = 216214, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Unequip()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216214, XrefRangeEnd = 216288, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 216305, RefRangeEnd = 216306, XrefRangeStart = 216288, XrefRangeEnd = 216305, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashContainer GetHoveredTrashContainer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHoveredTrashContainer_Private_TrashContainer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TrashContainer>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 216311, RefRangeEnd = 216313, XrefRangeStart = 216306, XrefRangeEnd = 216311, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool RaycastLook(out RaycastHit hit)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref hit);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RaycastLook_Private_Boolean_byref_RaycastHit_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 216317, RefRangeEnd = 216319, XrefRangeStart = 216313, XrefRangeEnd = 216317, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsPickupLocationValid(RaycastHit hit)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&hit);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPickupLocationValid_Private_Boolean_RaycastHit_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 216345, RefRangeEnd = 216348, XrefRangeStart = 216319, XrefRangeEnd = 216345, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<TrashItem> GetTrashItemsAtPoint(Vector3 pos)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&pos);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTrashItemsAtPoint_Private_List_1_TrashItem_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<TrashItem>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216348, XrefRangeEnd = 216350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartBagTrash(TrashContainer container)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)container);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartBagTrash_Private_Void_TrashContainer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216350, XrefRangeEnd = 216355, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StopBagTrash(bool complete)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&complete);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopBagTrash_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216355, XrefRangeEnd = 216356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartPickup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartPickup_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 216402, RefRangeEnd = 216403, XrefRangeStart = 216356, XrefRangeEnd = 216402, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StopPickup(bool complete)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&complete);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopPickup_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 216411, RefRangeEnd = 216414, XrefRangeStart = 216403, XrefRangeEnd = 216411, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ShowPrompt()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShowPrompt_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216414, XrefRangeEnd = 216422, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HidePrompt()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HidePrompt_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashBag_Equippable()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashBag_Equippable>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashBag_Equippable(IntPtr pointer)
		: base(pointer)
	{
	}
}
