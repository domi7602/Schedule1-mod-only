using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne;

public class UIContentPanel : UIPanel
{
	[System.Serializable]
	public class NavigationSettings : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_NavigationThreshold;

		private static readonly System.IntPtr NativeFieldInfoPtr_DirectionWeight;

		private static readonly System.IntPtr NativeFieldInfoPtr_DistanceWeight;

		private static readonly System.IntPtr NativeFieldInfoPtr_DirectionMatchThreshold;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe float NavigationThreshold
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NavigationThreshold);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NavigationThreshold)) = num;
			}
		}

		public unsafe float DirectionWeight
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DirectionWeight);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DirectionWeight)) = num;
			}
		}

		public unsafe float DistanceWeight
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DistanceWeight);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DistanceWeight)) = num;
			}
		}

		public unsafe float DirectionMatchThreshold
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DirectionMatchThreshold);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DirectionMatchThreshold)) = num;
			}
		}

		static NavigationSettings()
		{
			Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, "NavigationSettings");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr);
			NativeFieldInfoPtr_NavigationThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "NavigationThreshold");
			NativeFieldInfoPtr_DirectionWeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "DirectionWeight");
			NativeFieldInfoPtr_DistanceWeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "DistanceWeight");
			NativeFieldInfoPtr_DirectionMatchThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "DirectionMatchThreshold");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, 100665030);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 80460, RefRangeEnd = 80461, XrefRangeStart = 80459, XrefRangeEnd = 80460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe NavigationSettings()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public NavigationSettings(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[OriginalName("Assembly-CSharp.dll", "", "EContentPanelType")]
	public enum EContentPanelType
	{
		Grid,
		Vertical,
		Horizontal
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_uiPanelNavigationType;

	private static readonly System.IntPtr NativeFieldInfoPtr_contentPanelType;

	private static readonly System.IntPtr NativeFieldInfoPtr_navigationSettings;

	private static readonly System.IntPtr NativeMethodInfoPtr_DetectInput_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Navigate_Protected_Virtual_Boolean_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NavigateToSelectable_Private_Boolean_UISelectable_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe UINavigationType uiPanelNavigationType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uiPanelNavigationType);
			return *(UINavigationType*)num;
		}
		set
		{
			*(UINavigationType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uiPanelNavigationType)) = uINavigationType;
		}
	}

	public unsafe EContentPanelType contentPanelType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_contentPanelType);
			return *(EContentPanelType*)num;
		}
		set
		{
			*(EContentPanelType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_contentPanelType)) = eContentPanelType;
		}
	}

	public unsafe NavigationSettings navigationSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_navigationSettings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NavigationSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_navigationSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)navigationSettings));
		}
	}

	static UIContentPanel()
	{
		Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "UIContentPanel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr);
		NativeFieldInfoPtr_uiPanelNavigationType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, "uiPanelNavigationType");
		NativeFieldInfoPtr_contentPanelType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, "contentPanelType");
		NativeFieldInfoPtr_navigationSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, "navigationSettings");
		NativeMethodInfoPtr_DetectInput_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, 100665026);
		NativeMethodInfoPtr_Navigate_Protected_Virtual_Boolean_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, 100665027);
		NativeMethodInfoPtr_NavigateToSelectable_Private_Boolean_UISelectable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, 100665028);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr, 100665029);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80461, XrefRangeEnd = 80494, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void DetectInput()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_DetectInput_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80494, XrefRangeEnd = 80509, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool Navigate(Vector2 navDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&navDir);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Navigate_Protected_Virtual_Boolean_Vector2_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80509, XrefRangeEnd = 80513, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool NavigateToSelectable(UISelectable target)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NavigateToSelectable_Private_Boolean_UISelectable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 80513, XrefRangeEnd = 80519, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UIContentPanel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UIContentPanel>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UIContentPanel(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
