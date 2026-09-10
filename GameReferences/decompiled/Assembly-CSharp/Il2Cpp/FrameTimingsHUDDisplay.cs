using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2Cpp;

public class FrameTimingsHUDDisplay : MonoBehaviour
{
	[StructLayout(LayoutKind.Explicit)]
	public struct FrameTimingPoint
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_cpuFrameTime;

		private static readonly System.IntPtr NativeFieldInfoPtr_cpuMainThreadFrameTime;

		private static readonly System.IntPtr NativeFieldInfoPtr_cpuRenderThreadFrameTime;

		private static readonly System.IntPtr NativeFieldInfoPtr_gpuFrameTime;

		[FieldOffset(0)]
		public double cpuFrameTime;

		[FieldOffset(8)]
		public double cpuMainThreadFrameTime;

		[FieldOffset(16)]
		public double cpuRenderThreadFrameTime;

		[FieldOffset(24)]
		public double gpuFrameTime;

		static FrameTimingPoint()
		{
			Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, "FrameTimingPoint");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr);
			NativeFieldInfoPtr_cpuFrameTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr, "cpuFrameTime");
			NativeFieldInfoPtr_cpuMainThreadFrameTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr, "cpuMainThreadFrameTime");
			NativeFieldInfoPtr_cpuRenderThreadFrameTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr, "cpuRenderThreadFrameTime");
			NativeFieldInfoPtr_gpuFrameTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr, "gpuFrameTime");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<FrameTimingPoint>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Style;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_FrameTimings;

	private static readonly System.IntPtr NativeFieldInfoPtr_SAMPLE_SIZE;

	private static readonly System.IntPtr NativeFieldInfoPtr_frameTimingsHistory;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnGUI_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CaptureTimings_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GUIStyle m_Style
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Style);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GUIStyle>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Style)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gUIStyle));
		}
	}

	public unsafe Il2CppStructArray<FrameTiming> m_FrameTimings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FrameTimings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<FrameTiming>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_FrameTimings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static int SAMPLE_SIZE
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SAMPLE_SIZE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SAMPLE_SIZE, (void*)(&num));
		}
	}

	public unsafe List<FrameTimingPoint> frameTimingsHistory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frameTimingsHistory);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<FrameTimingPoint>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frameTimingsHistory)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static FrameTimingsHUDDisplay()
	{
		Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "FrameTimingsHUDDisplay");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr);
		NativeFieldInfoPtr_m_Style = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, "m_Style");
		NativeFieldInfoPtr_m_FrameTimings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, "m_FrameTimings");
		NativeFieldInfoPtr_SAMPLE_SIZE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, "SAMPLE_SIZE");
		NativeFieldInfoPtr_frameTimingsHistory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, "frameTimingsHistory");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, 100663397);
		NativeMethodInfoPtr_OnGUI_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, 100663398);
		NativeMethodInfoPtr_CaptureTimings_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, 100663399);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr, 100663400);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65754, XrefRangeEnd = 65762, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65762, XrefRangeEnd = 65820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnGUI()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnGUI_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65820, XrefRangeEnd = 65823, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CaptureTimings()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CaptureTimings_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65823, XrefRangeEnd = 65835, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FrameTimingsHUDDisplay()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FrameTimingsHUDDisplay>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FrameTimingsHUDDisplay(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
