using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class TextureFill : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Texture_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Mode_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Opacity;

	private static readonly System.IntPtr NativeFieldInfoPtr__Addressing_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Texture_Public_get_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Texture_Public_set_Void_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Addressing_Public_get_AddressMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Addressing_Public_set_Void_AddressMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Texture2D _Texture_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Texture_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Texture_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
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

	public unsafe Texture2D Texture
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Texture_Public_get_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29107, RefRangeEnd = 29109, XrefRangeStart = 29106, XrefRangeEnd = 29107, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Texture_Public_set_Void_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe virtual FillMode Mode
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 3891, RefRangeEnd = 3894, XrefRangeStart = 3891, XrefRangeEnd = 3894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CachedScanResults(RefRangeStart = 29109, RefRangeEnd = 29110, XrefRangeStart = 29109, XrefRangeEnd = 29109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 29110, RefRangeEnd = 29119, XrefRangeStart = 29110, XrefRangeEnd = 29110, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 3894, RefRangeEnd = 3895, XrefRangeStart = 3894, XrefRangeEnd = 3895, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CachedScanResults(RefRangeStart = 29057, RefRangeEnd = 29058, XrefRangeStart = 29057, XrefRangeEnd = 29058, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	static TextureFill()
	{
		Il2CppClassPointerStore<TextureFill>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "TextureFill");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TextureFill>.NativeClassPtr);
		NativeFieldInfoPtr__Texture_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, "<Texture>k__BackingField");
		NativeFieldInfoPtr__Mode_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, "<Mode>k__BackingField");
		NativeFieldInfoPtr_m_Opacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, "m_Opacity");
		NativeFieldInfoPtr__Addressing_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, "<Addressing>k__BackingField");
		NativeMethodInfoPtr_get_Texture_Public_get_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663694);
		NativeMethodInfoPtr_set_Texture_Public_set_Void_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663695);
		NativeMethodInfoPtr_get_Mode_Public_Virtual_Final_New_get_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663696);
		NativeMethodInfoPtr_set_Mode_Public_Virtual_Final_New_set_Void_FillMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663697);
		NativeMethodInfoPtr_get_Opacity_Public_Virtual_Final_New_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663698);
		NativeMethodInfoPtr_set_Opacity_Public_Virtual_Final_New_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663699);
		NativeMethodInfoPtr_get_Addressing_Public_get_AddressMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663700);
		NativeMethodInfoPtr_set_Addressing_Public_set_Void_AddressMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663701);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureFill>.NativeClassPtr, 100663702);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29120, RefRangeEnd = 29121, XrefRangeStart = 29119, XrefRangeEnd = 29120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TextureFill()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TextureFill>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TextureFill(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
