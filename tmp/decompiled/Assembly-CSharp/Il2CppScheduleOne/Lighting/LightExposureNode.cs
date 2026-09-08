using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Lighting;

public class LightExposureNode : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_ambientExposure;

	private static readonly IntPtr NativeFieldInfoPtr_sources;

	private static readonly IntPtr NativeMethodInfoPtr_GetTotalExposure_Public_Single_byref_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddSource_Public_Void_UsableLightSource_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_RemoveSource_Public_Void_UsableLightSource_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDrawGizmos_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float ambientExposure
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ambientExposure);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ambientExposure)) = num;
		}
	}

	public unsafe Dictionary<UsableLightSource, float> sources
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sources);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<UsableLightSource, float>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sources)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	static LightExposureNode()
	{
		Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Lighting", "LightExposureNode");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr);
		NativeFieldInfoPtr_ambientExposure = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, "ambientExposure");
		NativeFieldInfoPtr_sources = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, "sources");
		NativeMethodInfoPtr_GetTotalExposure_Public_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, 100674909);
		NativeMethodInfoPtr_AddSource_Public_Void_UsableLightSource_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, 100674910);
		NativeMethodInfoPtr_RemoveSource_Public_Void_UsableLightSource_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, 100674911);
		NativeMethodInfoPtr_OnDrawGizmos_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, 100674912);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr, 100674913);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 193158, RefRangeEnd = 193160, XrefRangeStart = 193134, XrefRangeEnd = 193158, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetTotalExposure(out float growSpeedMultiplier)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref growSpeedMultiplier);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTotalExposure_Public_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 193170, RefRangeEnd = 193171, XrefRangeStart = 193160, XrefRangeEnd = 193170, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddSource(UsableLightSource source, float lightAmount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &lightAmount;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSource_Public_Void_UsableLightSource_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 193175, RefRangeEnd = 193176, XrefRangeStart = 193171, XrefRangeEnd = 193175, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveSource(UsableLightSource source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveSource_Public_Void_UsableLightSource_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193176, XrefRangeEnd = 193181, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDrawGizmos()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDrawGizmos_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193181, XrefRangeEnd = 193189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LightExposureNode()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LightExposureNode>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LightExposureNode(IntPtr pointer)
		: base(pointer)
	{
	}
}
