using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class GradientFill : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Type_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Stops_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Mode_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Opacity;

	private static readonly System.IntPtr NativeFieldInfoPtr__Addressing_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__RadialFocus_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Type_Public_get_GradientFillType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Type_Public_set_Void_GradientFillType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Stops_Public_get_Il2CppStructArray_1_GradientStop_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Stops_Public_set_Void_Il2CppStructArray_1_GradientStop_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Addressing_Public_get_AddressMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Addressing_Public_set_Void_AddressMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RadialFocus_Public_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RadialFocus_Public_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GradientFillType _Type_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Type_k__BackingField);
			return *(GradientFillType*)num;
		}
		set
		{
			*(GradientFillType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Type_k__BackingField)) = gradientFillType;
		}
	}

	public unsafe Il2CppStructArray<GradientStop> _Stops_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stops_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<GradientStop>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stops_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
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

	public unsafe AddressMode _Addressing_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Addressing_k__BackingField);
			return *(AddressMode*)num;
		}
		set
		{
			*(AddressMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Addressing_k__BackingField)) = addressMode;
		}
	}

	public unsafe Vector2 _RadialFocus_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__RadialFocus_k__BackingField);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__RadialFocus_k__BackingField)) = vector;
		}
	}

	public unsafe GradientFillType Type
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29049, RefRangeEnd = 29051, XrefRangeStart = 29049, XrefRangeEnd = 29049, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Type_Public_get_GradientFillType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(GradientFillType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Type_Public_set_Void_GradientFillType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<GradientStop> Stops
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Stops_Public_get_Il2CppStructArray_1_GradientStop_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<GradientStop>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29056, XrefRangeEnd = 29057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Stops_Public_set_Void_Il2CppStructArray_1_GradientStop_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe virtual FillMode Mode
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 3894, RefRangeEnd = 3895, XrefRangeStart = 3894, XrefRangeEnd = 3895, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(FillMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29057, RefRangeEnd = 29058, XrefRangeStart = 29057, XrefRangeEnd = 29057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CallerCount(14)]
		[CachedScanResults(RefRangeStart = 29058, RefRangeEnd = 29072, XrefRangeStart = 29058, XrefRangeEnd = 29058, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe AddressMode Addressing
	{
		[CallerCount(29)]
		[CachedScanResults(RefRangeStart = 29072, RefRangeEnd = 29101, XrefRangeStart = 29072, XrefRangeEnd = 29072, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Addressing_Public_get_AddressMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(AddressMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29101, RefRangeEnd = 29102, XrefRangeStart = 29101, XrefRangeEnd = 29101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Addressing_Public_set_Void_AddressMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector2 RadialFocus
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RadialFocus_Public_get_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RadialFocus_Public_set_Void_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static GradientFill()
	{
		Il2CppClassPointerStore<GradientFill>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "GradientFill");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GradientFill>.NativeClassPtr);
		NativeFieldInfoPtr__Type_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, "<Type>k__BackingField");
		NativeFieldInfoPtr__Stops_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, "<Stops>k__BackingField");
		NativeFieldInfoPtr__Mode_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, "<Mode>k__BackingField");
		NativeFieldInfoPtr_m_Opacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, "m_Opacity");
		NativeFieldInfoPtr__Addressing_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, "<Addressing>k__BackingField");
		NativeFieldInfoPtr__RadialFocus_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, "<RadialFocus>k__BackingField");
		NativeMethodInfoPtr_get_Type_Public_get_GradientFillType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663681);
		NativeMethodInfoPtr_set_Type_Public_set_Void_GradientFillType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663682);
		NativeMethodInfoPtr_get_Stops_Public_get_Il2CppStructArray_1_GradientStop_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663683);
		NativeMethodInfoPtr_set_Stops_Public_set_Void_Il2CppStructArray_1_GradientStop_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663684);
		NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663685);
		NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663686);
		NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663687);
		NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663688);
		NativeMethodInfoPtr_get_Addressing_Public_get_AddressMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663689);
		NativeMethodInfoPtr_set_Addressing_Public_set_Void_AddressMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663690);
		NativeMethodInfoPtr_get_RadialFocus_Public_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663691);
		NativeMethodInfoPtr_set_RadialFocus_Public_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663692);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientFill>.NativeClassPtr, 100663693);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 29103, RefRangeEnd = 29106, XrefRangeStart = 29102, XrefRangeEnd = 29103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GradientFill()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GradientFill>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GradientFill(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
