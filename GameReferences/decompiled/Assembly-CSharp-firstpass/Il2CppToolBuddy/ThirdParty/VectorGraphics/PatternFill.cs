using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class PatternFill : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Mode_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Opacity;

	private static readonly System.IntPtr NativeFieldInfoPtr__Pattern_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Rect_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Pattern_Public_get_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Pattern_Public_set_Void_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Rect_Public_get_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Rect_Public_set_Void_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe SceneNode _Pattern_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Pattern_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Pattern_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
		}
	}

	public unsafe Rect _Rect_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Rect_k__BackingField);
			return *(Rect*)num;
		}
		set
		{
			*(Rect*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Rect_k__BackingField)) = rect;
		}
	}

	public unsafe virtual FillMode Mode
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29049, RefRangeEnd = 29051, XrefRangeStart = 29049, XrefRangeEnd = 29051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FillMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CachedScanResults(RefRangeStart = 26895, RefRangeEnd = 26896, XrefRangeStart = 26895, XrefRangeEnd = 26896, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe SceneNode Pattern
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Pattern_Public_get_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Pattern_Public_set_Void_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Rect Rect
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Rect_Public_get_Rect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Rect_Public_set_Void_Rect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static PatternFill()
	{
		Il2CppClassPointerStore<PatternFill>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "PatternFill");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PatternFill>.NativeClassPtr);
		NativeFieldInfoPtr__Mode_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, "<Mode>k__BackingField");
		NativeFieldInfoPtr_m_Opacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, "m_Opacity");
		NativeFieldInfoPtr__Pattern_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, "<Pattern>k__BackingField");
		NativeFieldInfoPtr__Rect_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, "<Rect>k__BackingField");
		NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663703);
		NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663704);
		NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663705);
		NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663706);
		NativeMethodInfoPtr_get_Pattern_Public_get_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663707);
		NativeMethodInfoPtr_set_Pattern_Public_set_Void_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663708);
		NativeMethodInfoPtr_get_Rect_Public_get_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663709);
		NativeMethodInfoPtr_set_Rect_Public_set_Void_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663710);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PatternFill>.NativeClassPtr, 100663711);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29122, RefRangeEnd = 29123, XrefRangeStart = 29121, XrefRangeEnd = 29122, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PatternFill()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PatternFill>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PatternFill(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
