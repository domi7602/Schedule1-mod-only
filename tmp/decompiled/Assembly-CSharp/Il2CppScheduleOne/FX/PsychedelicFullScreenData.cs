using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.FX;

public class PsychedelicFullScreenData : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_NoiseScale;

	private static readonly IntPtr NativeFieldInfoPtr_Blend;

	private static readonly IntPtr NativeFieldInfoPtr_PanSpeed;

	private static readonly IntPtr NativeFieldInfoPtr_DoesBounce;

	private static readonly IntPtr NativeFieldInfoPtr_Amplitude;

	private static readonly IntPtr NativeMethodInfoPtr_ConvertToMaterialProperties_Public_MaterialProperties_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float NoiseScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoiseScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoiseScale)) = num;
		}
	}

	public unsafe float Blend
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Blend);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Blend)) = num;
		}
	}

	public unsafe Vector2 PanSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PanSpeed);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PanSpeed)) = vector;
		}
	}

	public unsafe bool DoesBounce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DoesBounce);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DoesBounce)) = flag;
		}
	}

	public unsafe float Amplitude
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amplitude);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amplitude)) = num;
		}
	}

	static PsychedelicFullScreenData()
	{
		Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.FX", "PsychedelicFullScreenData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr);
		NativeFieldInfoPtr_NoiseScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, "NoiseScale");
		NativeFieldInfoPtr_Blend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, "Blend");
		NativeFieldInfoPtr_PanSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, "PanSpeed");
		NativeFieldInfoPtr_DoesBounce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, "DoesBounce");
		NativeFieldInfoPtr_Amplitude = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, "Amplitude");
		NativeMethodInfoPtr_ConvertToMaterialProperties_Public_MaterialProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, 100673651);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr, 100673652);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 178398, RefRangeEnd = 178402, XrefRangeStart = 178394, XrefRangeEnd = 178398, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PsychedelicFullScreenFeature.MaterialProperties ConvertToMaterialProperties()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertToMaterialProperties_Public_MaterialProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PsychedelicFullScreenFeature.MaterialProperties>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 178402, XrefRangeEnd = 178403, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PsychedelicFullScreenData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PsychedelicFullScreenData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PsychedelicFullScreenData(IntPtr pointer)
		: base(pointer)
	{
	}
}
