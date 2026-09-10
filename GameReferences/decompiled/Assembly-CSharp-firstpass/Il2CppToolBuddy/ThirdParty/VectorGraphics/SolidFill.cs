using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SolidFill : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Color_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Opacity;

	private static readonly System.IntPtr NativeFieldInfoPtr__Mode_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Color_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Color_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Color _Color_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Color_k__BackingField);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Color_k__BackingField)) = color;
		}
	}

	public unsafe float m_Opacity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Opacity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Opacity)) = num;
		}
	}

	public unsafe FillMode _Mode_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Mode_k__BackingField);
			return *(FillMode*)num;
		}
		set
		{
			*(FillMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Mode_k__BackingField)) = fillMode;
		}
	}

	public unsafe Color Color
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29039, RefRangeEnd = 29040, XrefRangeStart = 29039, XrefRangeEnd = 29039, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Color_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Color_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe virtual float Opacity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29040, RefRangeEnd = 29041, XrefRangeStart = 29040, XrefRangeEnd = 29040, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe virtual FillMode Mode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FillMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29041, RefRangeEnd = 29043, XrefRangeStart = 29041, XrefRangeEnd = 29041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SolidFill()
	{
		Il2CppClassPointerStore<SolidFill>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SolidFill");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SolidFill>.NativeClassPtr);
		NativeFieldInfoPtr__Color_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, "<Color>k__BackingField");
		NativeFieldInfoPtr_m_Opacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, "m_Opacity");
		NativeFieldInfoPtr__Mode_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, "<Mode>k__BackingField");
		NativeMethodInfoPtr_get_Color_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663674);
		NativeMethodInfoPtr_set_Color_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663675);
		NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663676);
		NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663677);
		NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663678);
		NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663679);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SolidFill>.NativeClassPtr, 100663680);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 29044, RefRangeEnd = 29049, XrefRangeStart = 29043, XrefRangeEnd = 29044, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SolidFill()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SolidFill>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SolidFill(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
