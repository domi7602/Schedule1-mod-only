using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Misc;
using UnityEngine;

namespace Il2CppScheduleOne.Map.Infrastructure;

public class TrafficLight : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "State")]
	public enum State
	{
		Red,
		Orange,
		Green
	}

	private static readonly IntPtr NativeFieldInfoPtr__redLight;

	private static readonly IntPtr NativeFieldInfoPtr__orangeLight;

	private static readonly IntPtr NativeFieldInfoPtr__greenLight;

	private static readonly IntPtr NativeFieldInfoPtr__state;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentState_Public_get_State_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CurrentState_Public_set_Void_State_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyState_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ToggleableLight _redLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__redLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ToggleableLight>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__redLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toggleableLight));
		}
	}

	public unsafe ToggleableLight _orangeLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__orangeLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ToggleableLight>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__orangeLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toggleableLight));
		}
	}

	public unsafe ToggleableLight _greenLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__greenLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ToggleableLight>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__greenLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toggleableLight));
		}
	}

	public unsafe State _state
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__state);
			return *(State*)num;
		}
		set
		{
			*(State*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__state)) = state;
		}
	}

	public unsafe State CurrentState
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentState_Public_get_State_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(State*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentState_Public_set_Void_State_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static TrafficLight()
	{
		Il2CppClassPointerStore<TrafficLight>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map.Infrastructure", "TrafficLight");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr);
		NativeFieldInfoPtr__redLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, "_redLight");
		NativeFieldInfoPtr__orangeLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, "_orangeLight");
		NativeFieldInfoPtr__greenLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, "_greenLight");
		NativeFieldInfoPtr__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, "_state");
		NativeMethodInfoPtr_get_CurrentState_Public_get_State_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, 100670440);
		NativeMethodInfoPtr_set_CurrentState_Public_set_Void_State_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, 100670441);
		NativeMethodInfoPtr_ApplyState_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, 100670442);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr, 100670443);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 144864, XrefRangeEnd = 144868, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void ApplyState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ApplyState_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrafficLight()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrafficLight>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrafficLight(IntPtr pointer)
		: base(pointer)
	{
	}
}
