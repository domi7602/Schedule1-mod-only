using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Delivery;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.UI.Items;
using Il2CppScheduleOne.UI.Tooltips;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne.UI.Phone.Delivery;

public class DeliveryReceiptDisplay : MonoBehaviour
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ItemEntryPrefab;

	private static readonly System.IntPtr NativeFieldInfoPtr__DestinationLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr__loadingDockLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr__shopLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr__shopDescriptionLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr__ItemEntryContainer;

	private static readonly System.IntPtr NativeFieldInfoPtr__ReorderButton;

	private static readonly System.IntPtr NativeFieldInfoPtr__ReorderTooltip;

	private static readonly System.IntPtr NativeFieldInfoPtr__reorderPriceLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr__maxItemsShown;

	private static readonly System.IntPtr NativeFieldInfoPtr__generalColorFont;

	private static readonly System.IntPtr NativeFieldInfoPtr__shopTextColorFont;

	private static readonly System.IntPtr NativeFieldInfoPtr__selectable;

	private static readonly System.IntPtr NativeFieldInfoPtr__receipt;

	private static readonly System.IntPtr NativeFieldInfoPtr__itemEntries;

	private static readonly System.IntPtr NativeFieldInfoPtr__onSelect;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ReorderButton_Public_get_Button_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Receipt_Public_get_DeliveryReceipt_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Selectable_Public_get_UISelectable_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialise_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Set_Public_Void_DeliveryReceipt_Single_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetTooltip_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetActiveTooltip_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ForceActiveTooltip_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SubscribeToOnSelect_Public_Void_Action_1_DeliveryReceipt_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnsubscribeFromOnSelect_Public_Void_Action_1_DeliveryReceipt_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__Initialise_b__22_0_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__Initialise_b__22_1_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__Initialise_b__22_2_Private_Void_0;

	public unsafe ItemEntryUI ItemEntryPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemEntryPrefab);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemEntryUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemEntryPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemEntryUI));
		}
	}

	public unsafe Text _DestinationLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DestinationLabel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Text>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DestinationLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)text));
		}
	}

	public unsafe Text _loadingDockLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__loadingDockLabel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Text>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__loadingDockLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)text));
		}
	}

	public unsafe Text _shopLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shopLabel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Text>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shopLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)text));
		}
	}

	public unsafe Text _shopDescriptionLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shopDescriptionLabel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Text>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shopDescriptionLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)text));
		}
	}

	public unsafe RectTransform _ItemEntryContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ItemEntryContainer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ItemEntryContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe Button _ReorderButton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ReorderButton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ReorderButton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe Tooltip _ReorderTooltip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ReorderTooltip);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Tooltip>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ReorderTooltip)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tooltip));
		}
	}

	public unsafe Text _reorderPriceLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reorderPriceLabel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Text>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reorderPriceLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)text));
		}
	}

	public unsafe int _maxItemsShown
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxItemsShown);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxItemsShown)) = num;
		}
	}

	public unsafe ColorFont _generalColorFont
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__generalColorFont);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ColorFont>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__generalColorFont)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)colorFont));
		}
	}

	public unsafe ColorFont _shopTextColorFont
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shopTextColorFont);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ColorFont>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shopTextColorFont)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)colorFont));
		}
	}

	public unsafe UISelectable _selectable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectable);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UISelectable>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uISelectable));
		}
	}

	public unsafe DeliveryReceipt _receipt
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__receipt);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DeliveryReceipt>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__receipt)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deliveryReceipt));
		}
	}

	public unsafe Il2CppReferenceArray<ItemEntryUI> _itemEntries
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemEntries);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ItemEntryUI>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemEntries)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppSystem.Action<DeliveryReceipt> _onSelect
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__onSelect);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<DeliveryReceipt>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__onSelect)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe Button ReorderButton
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 32794, RefRangeEnd = 32795, XrefRangeStart = 32794, XrefRangeEnd = 32795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ReorderButton_Public_get_Button_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
	}

	public unsafe DeliveryReceipt Receipt
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 41637, RefRangeEnd = 41647, XrefRangeStart = 41637, XrefRangeEnd = 41647, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Receipt_Public_get_DeliveryReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DeliveryReceipt>(intPtr) : null;
		}
	}

	public unsafe UISelectable Selectable
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Selectable_Public_get_UISelectable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UISelectable>(intPtr) : null;
		}
	}

	static DeliveryReceiptDisplay()
	{
		Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Phone.Delivery", "DeliveryReceiptDisplay");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr);
		NativeFieldInfoPtr_ItemEntryPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "ItemEntryPrefab");
		NativeFieldInfoPtr__DestinationLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_DestinationLabel");
		NativeFieldInfoPtr__loadingDockLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_loadingDockLabel");
		NativeFieldInfoPtr__shopLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_shopLabel");
		NativeFieldInfoPtr__shopDescriptionLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_shopDescriptionLabel");
		NativeFieldInfoPtr__ItemEntryContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_ItemEntryContainer");
		NativeFieldInfoPtr__ReorderButton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_ReorderButton");
		NativeFieldInfoPtr__ReorderTooltip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_ReorderTooltip");
		NativeFieldInfoPtr__reorderPriceLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_reorderPriceLabel");
		NativeFieldInfoPtr__maxItemsShown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_maxItemsShown");
		NativeFieldInfoPtr__generalColorFont = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_generalColorFont");
		NativeFieldInfoPtr__shopTextColorFont = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_shopTextColorFont");
		NativeFieldInfoPtr__selectable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_selectable");
		NativeFieldInfoPtr__receipt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_receipt");
		NativeFieldInfoPtr__itemEntries = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_itemEntries");
		NativeFieldInfoPtr__onSelect = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, "_onSelect");
		NativeMethodInfoPtr_get_ReorderButton_Public_get_Button_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688322);
		NativeMethodInfoPtr_get_Receipt_Public_get_DeliveryReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688323);
		NativeMethodInfoPtr_get_Selectable_Public_get_UISelectable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688324);
		NativeMethodInfoPtr_Initialise_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688325);
		NativeMethodInfoPtr_Set_Public_Void_DeliveryReceipt_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688326);
		NativeMethodInfoPtr_SetTooltip_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688327);
		NativeMethodInfoPtr_SetActiveTooltip_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688328);
		NativeMethodInfoPtr_ForceActiveTooltip_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688329);
		NativeMethodInfoPtr_SubscribeToOnSelect_Public_Void_Action_1_DeliveryReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688330);
		NativeMethodInfoPtr_UnsubscribeFromOnSelect_Public_Void_Action_1_DeliveryReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688331);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688332);
		NativeMethodInfoPtr__Initialise_b__22_0_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688333);
		NativeMethodInfoPtr__Initialise_b__22_1_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688334);
		NativeMethodInfoPtr__Initialise_b__22_2_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr, 100688335);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 319113, RefRangeEnd = 319114, XrefRangeStart = 319076, XrefRangeEnd = 319113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialise()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialise_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 319145, RefRangeEnd = 319146, XrefRangeStart = 319114, XrefRangeEnd = 319145, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Set(DeliveryReceipt receipt, float deliveryCost, bool canAfford)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)receipt);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &deliveryCost;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &canAfford;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Set_Public_Void_DeliveryReceipt_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 319148, RefRangeEnd = 319149, XrefRangeStart = 319146, XrefRangeEnd = 319148, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetTooltip(string tooltip)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetTooltip_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 319151, RefRangeEnd = 319152, XrefRangeStart = 319149, XrefRangeEnd = 319151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetActiveTooltip(bool active)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&active);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetActiveTooltip_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 319161, RefRangeEnd = 319162, XrefRangeStart = 319152, XrefRangeEnd = 319161, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ForceActiveTooltip(bool active)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&active);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForceActiveTooltip_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 319172, RefRangeEnd = 319173, XrefRangeStart = 319162, XrefRangeEnd = 319172, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SubscribeToOnSelect(Il2CppSystem.Action<DeliveryReceipt> callback)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SubscribeToOnSelect_Public_Void_Action_1_DeliveryReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 319173, XrefRangeEnd = 319183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnsubscribeFromOnSelect(Il2CppSystem.Action<DeliveryReceipt> callback)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnsubscribeFromOnSelect_Public_Void_Action_1_DeliveryReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 319183, XrefRangeEnd = 319184, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeliveryReceiptDisplay()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeliveryReceiptDisplay>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void _Initialise_b__22_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Initialise_b__22_0_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 319184, XrefRangeEnd = 319186, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void _Initialise_b__22_1()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Initialise_b__22_1_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 319186, XrefRangeEnd = 319192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void _Initialise_b__22_2()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Initialise_b__22_2_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeliveryReceiptDisplay(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
