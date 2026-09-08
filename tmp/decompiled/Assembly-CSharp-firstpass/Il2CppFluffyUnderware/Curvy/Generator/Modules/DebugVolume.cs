using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class DebugVolume : CGModule
{
	private static readonly IntPtr NativeFieldInfoPtr_InData;

	private static readonly IntPtr NativeFieldInfoPtr_ShowPathSamples;

	private static readonly IntPtr NativeFieldInfoPtr_ShowCrossSamples;

	private static readonly IntPtr NativeFieldInfoPtr_LimitCross;

	private static readonly IntPtr NativeFieldInfoPtr_ShowNormals;

	private static readonly IntPtr NativeFieldInfoPtr_ShowIndex;

	private static readonly IntPtr NativeFieldInfoPtr_ShowMap;

	private static readonly IntPtr NativeFieldInfoPtr_PathColor;

	private static readonly IntPtr NativeFieldInfoPtr_VolumeColor;

	private static readonly IntPtr NativeFieldInfoPtr_NormalColor;

	private static readonly IntPtr NativeFieldInfoPtr_Interpolate;

	private static readonly IntPtr NativeFieldInfoPtr_InterpolatePathF;

	private static readonly IntPtr NativeFieldInfoPtr_InterpolateCrossF;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CGModuleInputSlot InData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe bool ShowPathSamples
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowPathSamples);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowPathSamples)) = flag;
		}
	}

	public unsafe bool ShowCrossSamples
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowCrossSamples);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowCrossSamples)) = flag;
		}
	}

	public unsafe IntRegion LimitCross
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LimitCross);
			return *(IntRegion*)num;
		}
		set
		{
			*(IntRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LimitCross)) = intRegion;
		}
	}

	public unsafe bool ShowNormals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowNormals);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowNormals)) = flag;
		}
	}

	public unsafe bool ShowIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowIndex);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowIndex)) = flag;
		}
	}

	public unsafe bool ShowMap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowMap);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowMap)) = flag;
		}
	}

	public unsafe Color PathColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PathColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PathColor)) = color;
		}
	}

	public unsafe Color VolumeColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VolumeColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VolumeColor)) = color;
		}
	}

	public unsafe Color NormalColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NormalColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NormalColor)) = color;
		}
	}

	public unsafe bool Interpolate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interpolate);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interpolate)) = flag;
		}
	}

	public unsafe float InterpolatePathF
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InterpolatePathF);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InterpolatePathF)) = num;
		}
	}

	public unsafe float InterpolateCrossF
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InterpolateCrossF);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InterpolateCrossF)) = num;
		}
	}

	static DebugVolume()
	{
		Il2CppClassPointerStore<DebugVolume>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "DebugVolume");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr);
		NativeFieldInfoPtr_InData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "InData");
		NativeFieldInfoPtr_ShowPathSamples = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "ShowPathSamples");
		NativeFieldInfoPtr_ShowCrossSamples = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "ShowCrossSamples");
		NativeFieldInfoPtr_LimitCross = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "LimitCross");
		NativeFieldInfoPtr_ShowNormals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "ShowNormals");
		NativeFieldInfoPtr_ShowIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "ShowIndex");
		NativeFieldInfoPtr_ShowMap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "ShowMap");
		NativeFieldInfoPtr_PathColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "PathColor");
		NativeFieldInfoPtr_VolumeColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "VolumeColor");
		NativeFieldInfoPtr_NormalColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "NormalColor");
		NativeFieldInfoPtr_Interpolate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "Interpolate");
		NativeFieldInfoPtr_InterpolatePathF = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "InterpolatePathF");
		NativeFieldInfoPtr_InterpolateCrossF = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, "InterpolateCrossF");
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, 100666909);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr, 100666910);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57498, XrefRangeEnd = 57500, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57500, XrefRangeEnd = 57507, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DebugVolume()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DebugVolume>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DebugVolume(IntPtr pointer)
		: base(pointer)
	{
	}
}
