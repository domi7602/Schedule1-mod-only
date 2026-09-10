using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Il2CppScheduleOne.CustomUI;

public class UISelectable_Slider : UISelectable
{
	private static readonly IntPtr NativeFieldInfoPtr_DelayBeforeRepeat;

	private static readonly IntPtr NativeFieldInfoPtr_SlowestRepeatDelay;

	private static readonly IntPtr NativeFieldInfoPtr_FastestRepeatDelay;

	private static readonly IntPtr NativeFieldInfoPtr_VerticalMoveThreshold;

	private static readonly IntPtr NativeFieldInfoPtr_SliderMoveThreshold;

	private static readonly IntPtr NativeFieldInfoPtr_TimeToReachFastestRepeat;

	private static readonly IntPtr NativeFieldInfoPtr__increment;

	private static readonly IntPtr NativeFieldInfoPtr__bigIncrements;

	private static readonly IntPtr NativeFieldInfoPtr__bigIncrement;

	private static readonly IntPtr NativeFieldInfoPtr__wasNavPressedLastFrame;

	private static readonly IntPtr NativeFieldInfoPtr__timeBeforeNextRepeat;

	private static readonly IntPtr NativeFieldInfoPtr__timeSinceNavStart;

	private static readonly IntPtr NativeFieldInfoPtr__slider;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DetectInput_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSelect_Public_Virtual_Void_BaseEventData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Increment_Private_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float DelayBeforeRepeat
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DelayBeforeRepeat, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DelayBeforeRepeat, (void*)(&num));
		}
	}

	public unsafe static float SlowestRepeatDelay
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SlowestRepeatDelay, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SlowestRepeatDelay, (void*)(&num));
		}
	}

	public unsafe static float FastestRepeatDelay
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FastestRepeatDelay, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FastestRepeatDelay, (void*)(&num));
		}
	}

	public unsafe static float VerticalMoveThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_VerticalMoveThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_VerticalMoveThreshold, (void*)(&num));
		}
	}

	public unsafe static float SliderMoveThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SliderMoveThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SliderMoveThreshold, (void*)(&num));
		}
	}

	public unsafe static float TimeToReachFastestRepeat
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TimeToReachFastestRepeat, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TimeToReachFastestRepeat, (void*)(&num));
		}
	}

	public unsafe float _increment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__increment);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__increment)) = num;
		}
	}

	public unsafe bool _bigIncrements
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bigIncrements);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bigIncrements)) = flag;
		}
	}

	public unsafe float _bigIncrement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bigIncrement);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bigIncrement)) = num;
		}
	}

	public unsafe bool _wasNavPressedLastFrame
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__wasNavPressedLastFrame);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__wasNavPressedLastFrame)) = flag;
		}
	}

	public unsafe float _timeBeforeNextRepeat
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeBeforeNextRepeat);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeBeforeNextRepeat)) = num;
		}
	}

	public unsafe float _timeSinceNavStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeSinceNavStart);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeSinceNavStart)) = num;
		}
	}

	public unsafe Slider _slider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__slider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Slider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__slider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slider));
		}
	}

	static UISelectable_Slider()
	{
		Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.CustomUI", "UISelectable_Slider");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr);
		NativeFieldInfoPtr_DelayBeforeRepeat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "DelayBeforeRepeat");
		NativeFieldInfoPtr_SlowestRepeatDelay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "SlowestRepeatDelay");
		NativeFieldInfoPtr_FastestRepeatDelay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "FastestRepeatDelay");
		NativeFieldInfoPtr_VerticalMoveThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "VerticalMoveThreshold");
		NativeFieldInfoPtr_SliderMoveThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "SliderMoveThreshold");
		NativeFieldInfoPtr_TimeToReachFastestRepeat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "TimeToReachFastestRepeat");
		NativeFieldInfoPtr__increment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_increment");
		NativeFieldInfoPtr__bigIncrements = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_bigIncrements");
		NativeFieldInfoPtr__bigIncrement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_bigIncrement");
		NativeFieldInfoPtr__wasNavPressedLastFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_wasNavPressedLastFrame");
		NativeFieldInfoPtr__timeBeforeNextRepeat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_timeBeforeNextRepeat");
		NativeFieldInfoPtr__timeSinceNavStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_timeSinceNavStart");
		NativeFieldInfoPtr__slider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, "_slider");
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, 100689834);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, 100689835);
		NativeMethodInfoPtr_DetectInput_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, 100689836);
		NativeMethodInfoPtr_OnSelect_Public_Virtual_Void_BaseEventData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, 100689837);
		NativeMethodInfoPtr_Increment_Private_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, 100689838);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr, 100689839);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337187, XrefRangeEnd = 337192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337192, XrefRangeEnd = 337193, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public new unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337193, XrefRangeEnd = 337233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void DetectInput()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_DetectInput_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337233, XrefRangeEnd = 337234, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnSelect(BaseEventData eventData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eventData);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnSelect_Public_Virtual_Void_BaseEventData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 337235, RefRangeEnd = 337237, XrefRangeStart = 337234, XrefRangeEnd = 337235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Increment(float amount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&amount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Increment_Private_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337237, XrefRangeEnd = 337238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UISelectable_Slider()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UISelectable_Slider>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UISelectable_Slider(IntPtr pointer)
		: base(pointer)
	{
	}
}
