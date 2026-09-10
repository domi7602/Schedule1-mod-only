using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppTMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Il2CppScheduleOne;

public class UIToggle : UIOption
{
	private static readonly IntPtr NativeFieldInfoPtr_buttonText;

	private static readonly IntPtr NativeFieldInfoPtr_toggleImage;

	private static readonly IntPtr NativeFieldInfoPtr_ONTEXT;

	private static readonly IntPtr NativeFieldInfoPtr_OFFTEXT;

	private static readonly IntPtr NativeFieldInfoPtr_OnChanged;

	private static readonly IntPtr NativeFieldInfoPtr_state;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnUpdate_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetState_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetStateWithoutNotify_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetStateInternal_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetButtonState_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__Awake_b__6_0_Private_Void_0;

	public unsafe TextMeshProUGUI buttonText
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonText);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonText)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe Image toggleImage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_toggleImage);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_toggleImage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe static string ONTEXT
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ONTEXT, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ONTEXT, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string OFFTEXT
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OFFTEXT, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OFFTEXT, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe UnityEvent<bool> OnChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnChanged);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<bool>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe bool state
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state)) = flag;
		}
	}

	static UIToggle()
	{
		Il2CppClassPointerStore<UIToggle>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "UIToggle");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UIToggle>.NativeClassPtr);
		NativeFieldInfoPtr_buttonText = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, "buttonText");
		NativeFieldInfoPtr_toggleImage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, "toggleImage");
		NativeFieldInfoPtr_ONTEXT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, "ONTEXT");
		NativeFieldInfoPtr_OFFTEXT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, "OFFTEXT");
		NativeFieldInfoPtr_OnChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, "OnChanged");
		NativeFieldInfoPtr_state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, "state");
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665399);
		NativeMethodInfoPtr_OnUpdate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665400);
		NativeMethodInfoPtr_SetState_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665401);
		NativeMethodInfoPtr_SetStateWithoutNotify_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665402);
		NativeMethodInfoPtr_SetStateInternal_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665403);
		NativeMethodInfoPtr_SetButtonState_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665404);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665405);
		NativeMethodInfoPtr__Awake_b__6_0_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIToggle>.NativeClassPtr, 100665406);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85175, XrefRangeEnd = 85184, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnUpdate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85184, XrefRangeEnd = 85188, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetState(bool state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetState_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 85189, RefRangeEnd = 85197, XrefRangeStart = 85188, XrefRangeEnd = 85189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetStateWithoutNotify(bool state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStateWithoutNotify_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 85203, RefRangeEnd = 85206, XrefRangeStart = 85197, XrefRangeEnd = 85203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetStateInternal(bool state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStateInternal_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85206, XrefRangeEnd = 85212, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetButtonState(bool state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetButtonState_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85212, XrefRangeEnd = 85213, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UIToggle()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UIToggle>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 85213, XrefRangeEnd = 85217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void _Awake_b__6_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Awake_b__6_0_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UIToggle(IntPtr pointer)
		: base(pointer)
	{
	}
}
