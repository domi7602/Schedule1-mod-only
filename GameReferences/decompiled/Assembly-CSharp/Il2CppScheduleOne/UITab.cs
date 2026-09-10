using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppTMPro;

namespace Il2CppScheduleOne;

public class UITab : UIPanel
{
	[OriginalName("Assembly-CSharp.dll", "", "CycleInputActionType")]
	public enum CycleInputActionType
	{
		Primary,
		Secondary,
		Tertiary
	}

	[OriginalName("Assembly-CSharp.dll", "", "CycleDirection")]
	public enum CycleDirection
	{
		Horizontal,
		Vertical
	}

	private static readonly IntPtr NativeFieldInfoPtr_allowLooping;

	private static readonly IntPtr NativeFieldInfoPtr_cycleInputActionType;

	private static readonly IntPtr NativeFieldInfoPtr_cycleDirection;

	private static readonly IntPtr NativeFieldInfoPtr_reverseCycleDirection;

	private static readonly IntPtr NativeFieldInfoPtr_cycleLeftVisual;

	private static readonly IntPtr NativeFieldInfoPtr_cycleRightVisual;

	private static readonly IntPtr NativeFieldInfoPtr_cycleTabTimer;

	private static readonly IntPtr NativeFieldInfoPtr_wasCycleTabPressedLastFrame;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_EarlyUpdate_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetCycleTabInputValue_Private_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CycleTab_Private_Void_Single_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CycleTabWithoutEvent_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Navigate_Private_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CanNavigate_Protected_Virtual_New_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleInputDeviceChanged_Protected_Virtual_Void_InputDeviceType_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool allowLooping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allowLooping);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allowLooping)) = flag;
		}
	}

	public unsafe CycleInputActionType cycleInputActionType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleInputActionType);
			return *(CycleInputActionType*)num;
		}
		set
		{
			*(CycleInputActionType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleInputActionType)) = cycleInputActionType;
		}
	}

	public unsafe CycleDirection cycleDirection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleDirection);
			return *(CycleDirection*)num;
		}
		set
		{
			*(CycleDirection*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleDirection)) = cycleDirection;
		}
	}

	public unsafe bool reverseCycleDirection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reverseCycleDirection);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reverseCycleDirection)) = flag;
		}
	}

	public unsafe TextMeshProUGUI cycleLeftVisual
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleLeftVisual);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleLeftVisual)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe TextMeshProUGUI cycleRightVisual
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleRightVisual);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleRightVisual)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe float cycleTabTimer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleTabTimer);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cycleTabTimer)) = num;
		}
	}

	public unsafe bool wasCycleTabPressedLastFrame
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasCycleTabPressedLastFrame);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasCycleTabPressedLastFrame)) = flag;
		}
	}

	static UITab()
	{
		Il2CppClassPointerStore<UITab>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "UITab");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UITab>.NativeClassPtr);
		NativeFieldInfoPtr_allowLooping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "allowLooping");
		NativeFieldInfoPtr_cycleInputActionType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "cycleInputActionType");
		NativeFieldInfoPtr_cycleDirection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "cycleDirection");
		NativeFieldInfoPtr_reverseCycleDirection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "reverseCycleDirection");
		NativeFieldInfoPtr_cycleLeftVisual = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "cycleLeftVisual");
		NativeFieldInfoPtr_cycleRightVisual = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "cycleRightVisual");
		NativeFieldInfoPtr_cycleTabTimer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "cycleTabTimer");
		NativeFieldInfoPtr_wasCycleTabPressedLastFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UITab>.NativeClassPtr, "wasCycleTabPressedLastFrame");
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665390);
		NativeMethodInfoPtr_EarlyUpdate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665391);
		NativeMethodInfoPtr_GetCycleTabInputValue_Private_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665392);
		NativeMethodInfoPtr_CycleTab_Private_Void_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665393);
		NativeMethodInfoPtr_CycleTabWithoutEvent_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665394);
		NativeMethodInfoPtr_Navigate_Private_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665395);
		NativeMethodInfoPtr_CanNavigate_Protected_Virtual_New_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665396);
		NativeMethodInfoPtr_HandleInputDeviceChanged_Protected_Virtual_Void_InputDeviceType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665397);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UITab>.NativeClassPtr, 100665398);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85035, XrefRangeEnd = 85050, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85050, XrefRangeEnd = 85079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void EarlyUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_EarlyUpdate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85079, XrefRangeEnd = 85086, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetCycleTabInputValue()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCycleTabInputValue_Private_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 85088, RefRangeEnd = 85089, XrefRangeStart = 85086, XrefRangeEnd = 85088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CycleTab(float navDir, float delay, float speed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&navDir);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &delay;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &speed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CycleTab_Private_Void_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 85090, RefRangeEnd = 85091, XrefRangeStart = 85089, XrefRangeEnd = 85090, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CycleTabWithoutEvent(float navDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&navDir);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CycleTabWithoutEvent_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 85156, RefRangeEnd = 85159, XrefRangeStart = 85091, XrefRangeEnd = 85156, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Navigate(float navDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&navDir);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Navigate_Private_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(18)]
	[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool CanNavigate(float navDir)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&navDir);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_CanNavigate_Protected_Virtual_New_Boolean_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85159, XrefRangeEnd = 85173, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void HandleInputDeviceChanged(GameInput.InputDeviceType type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleInputDeviceChanged_Protected_Virtual_Void_InputDeviceType_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 85174, RefRangeEnd = 85175, XrefRangeStart = 85173, XrefRangeEnd = 85174, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UITab()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UITab>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UITab(IntPtr pointer)
		: base(pointer)
	{
	}
}
