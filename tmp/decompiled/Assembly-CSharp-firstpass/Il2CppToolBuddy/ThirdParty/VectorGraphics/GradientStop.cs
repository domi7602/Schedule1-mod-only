using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

[StructLayout(LayoutKind.Explicit)]
public struct GradientStop
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Color_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__StopPercentage_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Color_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Color_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StopPercentage_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StopPercentage_Public_set_Void_Single_0;

	[FieldOffset(0)]
	public Color _Color_k__BackingField;

	[FieldOffset(16)]
	public float _StopPercentage_k__BackingField;

	public unsafe Color Color
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 29004, RefRangeEnd = 29012, XrefRangeStart = 29004, XrefRangeEnd = 29004, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Color_Public_get_Color_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 29012, RefRangeEnd = 29030, XrefRangeStart = 29012, XrefRangeEnd = 29012, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Color_Public_set_Void_Color_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float StopPercentage
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StopPercentage_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 29030, RefRangeEnd = 29033, XrefRangeStart = 29030, XrefRangeEnd = 29030, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StopPercentage_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static GradientStop()
	{
		Il2CppClassPointerStore<GradientStop>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "GradientStop");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GradientStop>.NativeClassPtr);
		NativeFieldInfoPtr__Color_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, "<Color>k__BackingField");
		NativeFieldInfoPtr__StopPercentage_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, "<StopPercentage>k__BackingField");
		NativeMethodInfoPtr_get_Color_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, 100663662);
		NativeMethodInfoPtr_set_Color_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, 100663663);
		NativeMethodInfoPtr_get_StopPercentage_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, 100663664);
		NativeMethodInfoPtr_set_StopPercentage_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, 100663665);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<GradientStop>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
