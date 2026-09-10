using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppScheduleOne.Lighting;

public class LensFlareDisabler : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_RefreshDistance;

	private static readonly IntPtr NativeFieldInfoPtr_lensFlare;

	private static readonly IntPtr NativeFieldInfoPtr_optimizedLight;

	private static readonly IntPtr NativeFieldInfoPtr_threshold;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr_Refresh_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Method_Private_Void_0;

	public unsafe static int RefreshDistance
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RefreshDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RefreshDistance, (void*)(&num));
		}
	}

	public unsafe LensFlareComponentSRP lensFlare
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lensFlare);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<LensFlareComponentSRP>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lensFlare)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lensFlareComponentSRP));
		}
	}

	public unsafe OptimizedLight optimizedLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_optimizedLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<OptimizedLight>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_optimizedLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)optimizedLight));
		}
	}

	public unsafe float threshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threshold)) = num;
		}
	}

	static LensFlareDisabler()
	{
		Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Lighting", "LensFlareDisabler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr);
		NativeFieldInfoPtr_RefreshDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, "RefreshDistance");
		NativeFieldInfoPtr_lensFlare = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, "lensFlare");
		NativeFieldInfoPtr_optimizedLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, "optimizedLight");
		NativeFieldInfoPtr_threshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, "threshold");
		NativeMethodInfoPtr_Awake_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, 100674904);
		NativeMethodInfoPtr_OnDestroy_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, 100674905);
		NativeMethodInfoPtr_Refresh_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, 100674906);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, 100674907);
		NativeMethodInfoPtr_Method_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr, 100674908);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193035, XrefRangeEnd = 193075, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193075, XrefRangeEnd = 193089, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193089, XrefRangeEnd = 193102, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Refresh_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LensFlareDisabler()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LensFlareDisabler>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 193133, RefRangeEnd = 193134, XrefRangeStart = 193102, XrefRangeEnd = 193133, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Method_Private_Void_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LensFlareDisabler(IntPtr pointer)
		: base(pointer)
	{
	}
}
