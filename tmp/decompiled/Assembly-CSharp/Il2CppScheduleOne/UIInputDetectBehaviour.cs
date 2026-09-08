using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne;

public class UIInputDetectBehaviour : MonoBehaviour
{
	private static readonly System.IntPtr NativeFieldInfoPtr_initialHoldThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_repeatInterval;

	private static readonly System.IntPtr NativeFieldInfoPtr_timer;

	private static readonly System.IntPtr NativeFieldInfoPtr_wasPressedLastFrame;

	private static readonly System.IntPtr NativeFieldInfoPtr_onAction;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Void_Action_1_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoUpdate_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float initialHoldThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialHoldThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialHoldThreshold)) = num;
		}
	}

	public unsafe float repeatInterval
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_repeatInterval);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_repeatInterval)) = num;
		}
	}

	public unsafe float timer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timer);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timer)) = num;
		}
	}

	public unsafe bool wasPressedLastFrame
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasPressedLastFrame);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasPressedLastFrame)) = flag;
		}
	}

	public unsafe Il2CppSystem.Action<float> onAction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onAction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onAction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	static UIInputDetectBehaviour()
	{
		Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "UIInputDetectBehaviour");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr);
		NativeFieldInfoPtr_initialHoldThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, "initialHoldThreshold");
		NativeFieldInfoPtr_repeatInterval = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, "repeatInterval");
		NativeFieldInfoPtr_timer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, "timer");
		NativeFieldInfoPtr_wasPressedLastFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, "wasPressedLastFrame");
		NativeFieldInfoPtr_onAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, "onAction");
		NativeMethodInfoPtr_Initialize_Public_Void_Action_1_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, 100665047);
		NativeMethodInfoPtr_ResetData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, 100665048);
		NativeMethodInfoPtr_DoUpdate_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, 100665049);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr, 100665050);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 80681, RefRangeEnd = 80684, XrefRangeStart = 80680, XrefRangeEnd = 80681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialize(Il2CppSystem.Action<float> action, float holdThreshold, float repeat)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &holdThreshold;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &repeat;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_Action_1_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 80684, RefRangeEnd = 80687, XrefRangeStart = 80684, XrefRangeEnd = 80684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 80687, RefRangeEnd = 80690, XrefRangeStart = 80687, XrefRangeEnd = 80687, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoUpdate(float value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoUpdate_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 80691, RefRangeEnd = 80694, XrefRangeStart = 80690, XrefRangeEnd = 80691, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UIInputDetectBehaviour()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UIInputDetectBehaviour>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UIInputDetectBehaviour(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
