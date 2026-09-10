using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[System.Serializable]
public class CGMaterialSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SwapUV;

	private static readonly System.IntPtr NativeFieldInfoPtr_KeepAspect;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVScale;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool SwapUV
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SwapUV);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SwapUV)) = flag;
		}
	}

	public unsafe CGKeepAspectMode KeepAspect
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_KeepAspect);
			return *(CGKeepAspectMode*)num;
		}
		set
		{
			*(CGKeepAspectMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_KeepAspect)) = cGKeepAspectMode;
		}
	}

	public unsafe float UVRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVRotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVRotation)) = num;
		}
	}

	public unsafe Vector2 UVOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVOffset);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVOffset)) = vector;
		}
	}

	public unsafe Vector2 UVScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVScale);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVScale)) = vector;
		}
	}

	static CGMaterialSettings()
	{
		Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGMaterialSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr);
		NativeFieldInfoPtr_SwapUV = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr, "SwapUV");
		NativeFieldInfoPtr_KeepAspect = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr, "KeepAspect");
		NativeFieldInfoPtr_UVRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr, "UVRotation");
		NativeFieldInfoPtr_UVOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr, "UVOffset");
		NativeFieldInfoPtr_UVScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr, "UVScale");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr, 100665843);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 47253, RefRangeEnd = 47260, XrefRangeStart = 47248, XrefRangeEnd = 47253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGMaterialSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGMaterialSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGMaterialSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
