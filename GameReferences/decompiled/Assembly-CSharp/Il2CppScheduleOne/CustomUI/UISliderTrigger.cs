using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Il2CppScheduleOne.CustomUI;

public class UISliderTrigger : UITrigger
{
	[OriginalName("Assembly-CSharp.dll", "", "ESliderDirection")]
	public enum ESliderDirection
	{
		Horizontal,
		Vertical
	}

	private static readonly IntPtr NativeFieldInfoPtr_Deadzone;

	private static readonly IntPtr NativeFieldInfoPtr__slider;

	private static readonly IntPtr NativeFieldInfoPtr__sliderDirection;

	private static readonly IntPtr NativeFieldInfoPtr__sliderSpeed;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DetectTriggerInput_Internal_Virtual_Void_InputActionReference_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float Deadzone
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Deadzone, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Deadzone, (void*)(&num));
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

	public unsafe ESliderDirection _sliderDirection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sliderDirection);
			return *(ESliderDirection*)num;
		}
		set
		{
			*(ESliderDirection*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sliderDirection)) = eSliderDirection;
		}
	}

	public unsafe float _sliderSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sliderSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sliderSpeed)) = num;
		}
	}

	static UISliderTrigger()
	{
		Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.CustomUI", "UISliderTrigger");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr);
		NativeFieldInfoPtr_Deadzone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, "Deadzone");
		NativeFieldInfoPtr__slider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, "_slider");
		NativeFieldInfoPtr__sliderDirection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, "_sliderDirection");
		NativeFieldInfoPtr__sliderSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, "_sliderSpeed");
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, 100689840);
		NativeMethodInfoPtr_DetectTriggerInput_Internal_Virtual_Void_InputActionReference_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, 100689841);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr, 100689842);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337238, XrefRangeEnd = 337239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337239, XrefRangeEnd = 337253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void DetectTriggerInput(InputActionReference inputAction)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputAction);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_DetectTriggerInput_Internal_Virtual_Void_InputActionReference_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 337253, XrefRangeEnd = 337254, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UISliderTrigger()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UISliderTrigger>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UISliderTrigger(IntPtr pointer)
		: base(pointer)
	{
	}
}
