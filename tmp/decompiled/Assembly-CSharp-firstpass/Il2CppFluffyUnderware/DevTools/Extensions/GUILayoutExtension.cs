using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class GUILayoutExtension : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Area_Public_Static_Void_Action_Rect_GUIStyle_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Area_Public_Static_Void_Action_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Horizontal_Public_Static_Void_Action_GUIStyle_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Horizontal_Public_Static_Void_Action_Il2CppReferenceArray_1_GUILayoutOption_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ScrollView_Public_Static_Vector2_Action_Vector2_Il2CppReferenceArray_1_GUILayoutOption_0;

	static GUILayoutExtension()
	{
		Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "GUILayoutExtension");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr);
		NativeMethodInfoPtr_Area_Public_Static_Void_Action_Rect_GUIStyle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr, 100664429);
		NativeMethodInfoPtr_Area_Public_Static_Void_Action_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr, 100664430);
		NativeMethodInfoPtr_Horizontal_Public_Static_Void_Action_GUIStyle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr, 100664431);
		NativeMethodInfoPtr_Horizontal_Public_Static_Void_Action_Il2CppReferenceArray_1_GUILayoutOption_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr, 100664432);
		NativeMethodInfoPtr_ScrollView_Public_Static_Vector2_Action_Vector2_Il2CppReferenceArray_1_GUILayoutOption_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GUILayoutExtension>.NativeClassPtr, 100664433);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34599, XrefRangeEnd = 34603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Area(Il2CppSystem.Action action, Rect screenRectangle, GUIStyle skinBox)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &screenRectangle;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skinBox);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Area_Public_Static_Void_Action_Rect_GUIStyle_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34603, XrefRangeEnd = 34607, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Area(Il2CppSystem.Action action, Rect screenRectangle)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &screenRectangle;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Area_Public_Static_Void_Action_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34607, XrefRangeEnd = 34617, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Horizontal(Il2CppSystem.Action action, GUIStyle style)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)style);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Horizontal_Public_Static_Void_Action_GUIStyle_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34617, XrefRangeEnd = 34621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Horizontal(Il2CppSystem.Action action, [Optional] Il2CppReferenceArray<GUILayoutOption> layoutOptions)
	{
		if (layoutOptions == null)
		{
			layoutOptions = new Il2CppReferenceArray<GUILayoutOption>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)layoutOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Horizontal_Public_Static_Void_Action_Il2CppReferenceArray_1_GUILayoutOption_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34621, XrefRangeEnd = 34625, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 ScrollView(Il2CppSystem.Action action, Vector2 scrollPosition, [Optional] Il2CppReferenceArray<GUILayoutOption> layoutOptions)
	{
		if (layoutOptions == null)
		{
			layoutOptions = new Il2CppReferenceArray<GUILayoutOption>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &scrollPosition;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)layoutOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScrollView_Public_Static_Vector2_Action_Vector2_Il2CppReferenceArray_1_GUILayoutOption_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public static void Horizontal(Il2CppSystem.Action action, params GUILayoutOption[] layoutOptions)
	{
		Horizontal(action, new Il2CppReferenceArray<GUILayoutOption>(layoutOptions));
	}

	public static Vector2 ScrollView(Il2CppSystem.Action action, Vector2 scrollPosition, params GUILayoutOption[] layoutOptions)
	{
		return ScrollView(action, scrollPosition, new Il2CppReferenceArray<GUILayoutOption>(layoutOptions));
	}

	public GUILayoutExtension(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
