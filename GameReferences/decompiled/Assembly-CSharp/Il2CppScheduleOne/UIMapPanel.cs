using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne;

public class UIMapPanel : UIPanel
{
	private static readonly IntPtr NativeFieldInfoPtr_rightStickDeadzone;

	private static readonly IntPtr NativeFieldInfoPtr_mapScrollRect;

	private static readonly IntPtr NativeFieldInfoPtr_scrollSensitivity;

	private static readonly IntPtr NativeFieldInfoPtr_rightStickSensitivity;

	private static readonly IntPtr NativeFieldInfoPtr_minZoomScrollSpeedMult;

	private static readonly IntPtr NativeFieldInfoPtr_maxZoomScrollSpeedMult;

	private static readonly IntPtr NativeFieldInfoPtr_zoomSensitivity;

	private static readonly IntPtr NativeFieldInfoPtr_centerPoint;

	private static readonly IntPtr NativeFieldInfoPtr_initialHoldThreshold;

	private static readonly IntPtr NativeFieldInfoPtr_repeatInterval;

	private static readonly IntPtr NativeFieldInfoPtr_mapItems;

	private static readonly IntPtr NativeFieldInfoPtr_snappedItem;

	private static readonly IntPtr NativeFieldInfoPtr_lockMapInput;

	private static readonly IntPtr NativeMethodInfoPtr_get_LockMapInput_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_LockMapInput_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Navigate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Zoom_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RegisterMapItem_Public_Void_UIMapItem_0;

	private static readonly IntPtr NativeMethodInfoPtr_DeregisterMapItem_Public_Void_UIMapItem_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetSnappedItem_Public_Void_UIMapItem_0;

	private static readonly IntPtr NativeMethodInfoPtr_ResetSnappedItem_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SnapToNearestMapItem_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SnapMapToItem_Private_Void_UIMapItem_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleInputDeviceChanged_Protected_Virtual_Void_InputDeviceType_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float rightStickDeadzone
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_rightStickDeadzone, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_rightStickDeadzone, (void*)(&num));
		}
	}

	public unsafe PinchableScrollRect mapScrollRect
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mapScrollRect);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PinchableScrollRect>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mapScrollRect)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pinchableScrollRect));
		}
	}

	public unsafe float scrollSensitivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scrollSensitivity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scrollSensitivity)) = num;
		}
	}

	public unsafe float rightStickSensitivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rightStickSensitivity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rightStickSensitivity)) = num;
		}
	}

	public unsafe float minZoomScrollSpeedMult
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minZoomScrollSpeedMult);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minZoomScrollSpeedMult)) = num;
		}
	}

	public unsafe float maxZoomScrollSpeedMult
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxZoomScrollSpeedMult);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxZoomScrollSpeedMult)) = num;
		}
	}

	public unsafe float zoomSensitivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_zoomSensitivity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_zoomSensitivity)) = num;
		}
	}

	public unsafe RectTransform centerPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_centerPoint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_centerPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe static float initialHoldThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_initialHoldThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_initialHoldThreshold, (void*)(&num));
		}
	}

	public unsafe static float repeatInterval
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_repeatInterval, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_repeatInterval, (void*)(&num));
		}
	}

	public unsafe List<UIMapItem> mapItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mapItems);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<UIMapItem>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mapItems)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe UIMapItem snappedItem
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_snappedItem);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UIMapItem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_snappedItem)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uIMapItem));
		}
	}

	public unsafe bool lockMapInput
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lockMapInput);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lockMapInput)) = flag;
		}
	}

	public unsafe bool LockMapInput
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LockMapInput_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 80794, RefRangeEnd = 80795, XrefRangeStart = 80786, XrefRangeEnd = 80794, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_LockMapInput_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static UIMapPanel()
	{
		Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "UIMapPanel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr);
		NativeFieldInfoPtr_rightStickDeadzone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "rightStickDeadzone");
		NativeFieldInfoPtr_mapScrollRect = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "mapScrollRect");
		NativeFieldInfoPtr_scrollSensitivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "scrollSensitivity");
		NativeFieldInfoPtr_rightStickSensitivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "rightStickSensitivity");
		NativeFieldInfoPtr_minZoomScrollSpeedMult = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "minZoomScrollSpeedMult");
		NativeFieldInfoPtr_maxZoomScrollSpeedMult = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "maxZoomScrollSpeedMult");
		NativeFieldInfoPtr_zoomSensitivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "zoomSensitivity");
		NativeFieldInfoPtr_centerPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "centerPoint");
		NativeFieldInfoPtr_initialHoldThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "initialHoldThreshold");
		NativeFieldInfoPtr_repeatInterval = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "repeatInterval");
		NativeFieldInfoPtr_mapItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "mapItems");
		NativeFieldInfoPtr_snappedItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "snappedItem");
		NativeFieldInfoPtr_lockMapInput = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, "lockMapInput");
		NativeMethodInfoPtr_get_LockMapInput_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665059);
		NativeMethodInfoPtr_set_LockMapInput_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665060);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665061);
		NativeMethodInfoPtr_Update_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665062);
		NativeMethodInfoPtr_Navigate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665063);
		NativeMethodInfoPtr_Zoom_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665064);
		NativeMethodInfoPtr_RegisterMapItem_Public_Void_UIMapItem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665065);
		NativeMethodInfoPtr_DeregisterMapItem_Public_Void_UIMapItem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665066);
		NativeMethodInfoPtr_SetSnappedItem_Public_Void_UIMapItem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665067);
		NativeMethodInfoPtr_ResetSnappedItem_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665068);
		NativeMethodInfoPtr_SnapToNearestMapItem_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665069);
		NativeMethodInfoPtr_SnapMapToItem_Private_Void_UIMapItem_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665070);
		NativeMethodInfoPtr_HandleInputDeviceChanged_Protected_Virtual_Void_InputDeviceType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665071);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr, 100665072);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80795, XrefRangeEnd = 80803, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80803, XrefRangeEnd = 80819, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 80851, RefRangeEnd = 80852, XrefRangeStart = 80819, XrefRangeEnd = 80851, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Navigate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Navigate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80852, XrefRangeEnd = 80861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Zoom()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Zoom_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 80867, RefRangeEnd = 80869, XrefRangeStart = 80861, XrefRangeEnd = 80867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RegisterMapItem(UIMapItem item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterMapItem_Public_Void_UIMapItem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 80875, RefRangeEnd = 80876, XrefRangeStart = 80869, XrefRangeEnd = 80875, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DeregisterMapItem(UIMapItem item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeregisterMapItem_Public_Void_UIMapItem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80876, XrefRangeEnd = 80882, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetSnappedItem(UIMapItem newItem)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newItem);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSnappedItem_Public_Void_UIMapItem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 80888, RefRangeEnd = 80890, XrefRangeStart = 80882, XrefRangeEnd = 80888, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetSnappedItem()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetSnappedItem_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 80964, RefRangeEnd = 80965, XrefRangeStart = 80890, XrefRangeEnd = 80964, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SnapToNearestMapItem()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapToNearestMapItem_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SnapMapToItem(UIMapItem item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapMapToItem_Private_Void_UIMapItem_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80965, XrefRangeEnd = 80968, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void HandleInputDeviceChanged(GameInput.InputDeviceType type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleInputDeviceChanged_Protected_Virtual_Void_InputDeviceType_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80968, XrefRangeEnd = 80976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UIMapPanel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UIMapPanel>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UIMapPanel(IntPtr pointer)
		: base(pointer)
	{
	}
}
