using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Economy;

[StructLayout(LayoutKind.Explicit)]
public struct DealWindowInfo
{
	private static readonly System.IntPtr NativeFieldInfoPtr_WINDOW_DURATION_MINS;

	private static readonly System.IntPtr NativeFieldInfoPtr_WINDOW_COUNT;

	private static readonly System.IntPtr NativeFieldInfoPtr_StartTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_EndTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_Morning;

	private static readonly System.IntPtr NativeFieldInfoPtr_Afternoon;

	private static readonly System.IntPtr NativeFieldInfoPtr_Night;

	private static readonly System.IntPtr NativeFieldInfoPtr_LateNight;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetWindowInfo_Public_Static_DealWindowInfo_EDealWindow_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetWindow_Public_Static_EDealWindow_Int32_0;

	[FieldOffset(0)]
	public int StartTime;

	[FieldOffset(4)]
	public int EndTime;

	public unsafe static int WINDOW_DURATION_MINS
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WINDOW_DURATION_MINS, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WINDOW_DURATION_MINS, (void*)(&num));
		}
	}

	public unsafe static int WINDOW_COUNT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WINDOW_COUNT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WINDOW_COUNT, (void*)(&num));
		}
	}

	public unsafe static DealWindowInfo Morning
	{
		get
		{
			Unsafe.SkipInit(out DealWindowInfo result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Morning, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Morning, (void*)(&dealWindowInfo));
		}
	}

	public unsafe static DealWindowInfo Afternoon
	{
		get
		{
			Unsafe.SkipInit(out DealWindowInfo result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Afternoon, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Afternoon, (void*)(&dealWindowInfo));
		}
	}

	public unsafe static DealWindowInfo Night
	{
		get
		{
			Unsafe.SkipInit(out DealWindowInfo result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Night, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Night, (void*)(&dealWindowInfo));
		}
	}

	public unsafe static DealWindowInfo LateNight
	{
		get
		{
			Unsafe.SkipInit(out DealWindowInfo result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LateNight, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LateNight, (void*)(&dealWindowInfo));
		}
	}

	static DealWindowInfo()
	{
		Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Economy", "DealWindowInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr);
		NativeFieldInfoPtr_WINDOW_DURATION_MINS = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "WINDOW_DURATION_MINS");
		NativeFieldInfoPtr_WINDOW_COUNT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "WINDOW_COUNT");
		NativeFieldInfoPtr_StartTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "StartTime");
		NativeFieldInfoPtr_EndTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "EndTime");
		NativeFieldInfoPtr_Morning = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "Morning");
		NativeFieldInfoPtr_Afternoon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "Afternoon");
		NativeFieldInfoPtr_Night = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "Night");
		NativeFieldInfoPtr_LateNight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, "LateNight");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, 100674244);
		NativeMethodInfoPtr_GetWindowInfo_Public_Static_DealWindowInfo_EDealWindow_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, 100674245);
		NativeMethodInfoPtr_GetWindow_Public_Static_EDealWindow_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, 100674246);
	}

	[CallerCount(494)]
	[CachedScanResults(RefRangeStart = 60743, RefRangeEnd = 61237, XrefRangeStart = 60743, XrefRangeEnd = 61237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DealWindowInfo(int startTime, int endTime)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&startTime);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &endTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 186472, RefRangeEnd = 186475, XrefRangeStart = 186468, XrefRangeEnd = 186472, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static DealWindowInfo GetWindowInfo(EDealWindow window)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&window);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWindowInfo_Public_Static_DealWindowInfo_EDealWindow_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(DealWindowInfo*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 186481, RefRangeEnd = 186485, XrefRangeStart = 186475, XrefRangeEnd = 186481, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EDealWindow GetWindow(int time)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&time);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWindow_Public_Static_EDealWindow_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EDealWindow*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<DealWindowInfo>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
