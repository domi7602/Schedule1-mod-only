using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools;

public static class DTTime : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__EditorDeltaTime;

	private static readonly System.IntPtr NativeFieldInfoPtr__EditorLastTime;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TimeSinceStartup_Public_Static_get_Double_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_deltaTime_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitializeEditorTime_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateEditorTime_Public_Static_Void_0;

	public unsafe static float _EditorDeltaTime
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__EditorDeltaTime, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__EditorDeltaTime, (void*)(&num));
		}
	}

	public unsafe static float _EditorLastTime
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__EditorLastTime, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__EditorLastTime, (void*)(&num));
		}
	}

	public unsafe static double TimeSinceStartup
	{
		[CallerCount(19)]
		[CachedScanResults(RefRangeStart = 31576, RefRangeEnd = 31595, XrefRangeStart = 31575, XrefRangeEnd = 31576, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TimeSinceStartup_Public_Static_get_Double_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(double*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static float deltaTime
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31595, XrefRangeEnd = 31602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_deltaTime_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static DTTime()
	{
		Il2CppClassPointerStore<DTTime>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTTime");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTTime>.NativeClassPtr);
		NativeFieldInfoPtr__EditorDeltaTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTTime>.NativeClassPtr, "_EditorDeltaTime");
		NativeFieldInfoPtr__EditorLastTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTTime>.NativeClassPtr, "_EditorLastTime");
		NativeMethodInfoPtr_get_TimeSinceStartup_Public_Static_get_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTTime>.NativeClassPtr, 100663985);
		NativeMethodInfoPtr_get_deltaTime_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTTime>.NativeClassPtr, 100663986);
		NativeMethodInfoPtr_InitializeEditorTime_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTTime>.NativeClassPtr, 100663987);
		NativeMethodInfoPtr_UpdateEditorTime_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTTime>.NativeClassPtr, 100663988);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31602, XrefRangeEnd = 31606, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitializeEditorTime()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeEditorTime_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31606, XrefRangeEnd = 31610, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void UpdateEditorTime()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateEditorTime_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DTTime(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
