using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.UI.WorldspacePopup;

public class WorldspacePopupCanvas : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_WORLDSPACE_ICON_SCALE_MULTIPLIER;

	private static readonly IntPtr NativeFieldInfoPtr_HUDIconMaxOpacityAngle;

	private static readonly IntPtr NativeFieldInfoPtr_HUDIconMinOpacityAngle;

	private static readonly IntPtr NativeFieldInfoPtr_WorldspaceContainer;

	private static readonly IntPtr NativeFieldInfoPtr_HudContainer;

	private static readonly IntPtr NativeFieldInfoPtr_HudIconContainerPrefab;

	private static readonly IntPtr NativeFieldInfoPtr_activeWorldspaceUIs;

	private static readonly IntPtr NativeFieldInfoPtr_activeHUDUIs;

	private static readonly IntPtr NativeFieldInfoPtr_popupsWithUI;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ShouldCreateUI_Private_Boolean_WorldspacePopup_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateWorldspaceIcon_Private_WorldspacePopupUI_WorldspacePopup_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateHUDIcon_Private_RectTransform_WorldspacePopup_0;

	private static readonly IntPtr NativeMethodInfoPtr_DestroyWorldspaceIcon_Private_Void_WorldspacePopup_0;

	private static readonly IntPtr NativeMethodInfoPtr_DestroyHUDIcon_Private_Void_WorldspacePopup_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float WORLDSPACE_ICON_SCALE_MULTIPLIER
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WORLDSPACE_ICON_SCALE_MULTIPLIER, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WORLDSPACE_ICON_SCALE_MULTIPLIER, (void*)(&num));
		}
	}

	public unsafe static float HUDIconMaxOpacityAngle
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HUDIconMaxOpacityAngle, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HUDIconMaxOpacityAngle, (void*)(&num));
		}
	}

	public unsafe static float HUDIconMinOpacityAngle
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HUDIconMinOpacityAngle, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HUDIconMinOpacityAngle, (void*)(&num));
		}
	}

	public unsafe RectTransform WorldspaceContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldspaceContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldspaceContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe RectTransform HudContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HudContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HudContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe GameObject HudIconContainerPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HudIconContainerPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HudIconContainerPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe List<WorldspacePopupUI> activeWorldspaceUIs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeWorldspaceUIs);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<WorldspacePopupUI>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeWorldspaceUIs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<RectTransform> activeHUDUIs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeHUDUIs);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<RectTransform>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_activeHUDUIs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<WorldspacePopup> popupsWithUI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_popupsWithUI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<WorldspacePopup>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_popupsWithUI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static WorldspacePopupCanvas()
	{
		Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.WorldspacePopup", "WorldspacePopupCanvas");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr);
		NativeFieldInfoPtr_WORLDSPACE_ICON_SCALE_MULTIPLIER = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "WORLDSPACE_ICON_SCALE_MULTIPLIER");
		NativeFieldInfoPtr_HUDIconMaxOpacityAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "HUDIconMaxOpacityAngle");
		NativeFieldInfoPtr_HUDIconMinOpacityAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "HUDIconMinOpacityAngle");
		NativeFieldInfoPtr_WorldspaceContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "WorldspaceContainer");
		NativeFieldInfoPtr_HudContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "HudContainer");
		NativeFieldInfoPtr_HudIconContainerPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "HudIconContainerPrefab");
		NativeFieldInfoPtr_activeWorldspaceUIs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "activeWorldspaceUIs");
		NativeFieldInfoPtr_activeHUDUIs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "activeHUDUIs");
		NativeFieldInfoPtr_popupsWithUI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, "popupsWithUI");
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687577);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687578);
		NativeMethodInfoPtr_ShouldCreateUI_Private_Boolean_WorldspacePopup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687579);
		NativeMethodInfoPtr_CreateWorldspaceIcon_Private_WorldspacePopupUI_WorldspacePopup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687580);
		NativeMethodInfoPtr_CreateHUDIcon_Private_RectTransform_WorldspacePopup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687581);
		NativeMethodInfoPtr_DestroyWorldspaceIcon_Private_Void_WorldspacePopup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687582);
		NativeMethodInfoPtr_DestroyHUDIcon_Private_Void_WorldspacePopup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687583);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr, 100687584);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 310228, XrefRangeEnd = 310340, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 310340, XrefRangeEnd = 310437, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 310450, RefRangeEnd = 310452, XrefRangeStart = 310437, XrefRangeEnd = 310450, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool ShouldCreateUI(WorldspacePopup popup)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)popup);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShouldCreateUI_Private_Boolean_WorldspacePopup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 310452, XrefRangeEnd = 310464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WorldspacePopupUI CreateWorldspaceIcon(WorldspacePopup popup)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)popup);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateWorldspaceIcon_Private_WorldspacePopupUI_WorldspacePopup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WorldspacePopupUI>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 310490, RefRangeEnd = 310491, XrefRangeStart = 310464, XrefRangeEnd = 310490, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RectTransform CreateHUDIcon(WorldspacePopup popup)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)popup);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateHUDIcon_Private_RectTransform_WorldspacePopup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 310491, XrefRangeEnd = 310509, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DestroyWorldspaceIcon(WorldspacePopup popup)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)popup);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DestroyWorldspaceIcon_Private_Void_WorldspacePopup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 310535, RefRangeEnd = 310536, XrefRangeStart = 310509, XrefRangeEnd = 310535, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DestroyHUDIcon(WorldspacePopup popup)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)popup);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DestroyHUDIcon_Private_Void_WorldspacePopup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 310536, XrefRangeEnd = 310558, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WorldspacePopupCanvas()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WorldspacePopupCanvas>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WorldspacePopupCanvas(IntPtr pointer)
		: base(pointer)
	{
	}
}
