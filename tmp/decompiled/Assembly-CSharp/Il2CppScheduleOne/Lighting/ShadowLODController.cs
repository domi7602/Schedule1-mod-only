using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Lighting;

public class ShadowLODController : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_RefreshMovementThreshold;

	private static readonly IntPtr NativeFieldInfoPtr__softShadowEnabled;

	private static readonly IntPtr NativeFieldInfoPtr__lights;

	private static readonly IntPtr NativeFieldInfoPtr__softShadowDistance;

	private static readonly IntPtr NativeFieldInfoPtr__hardShadowDistance;

	private static readonly IntPtr NativeFieldInfoPtr__softShadowDistanceSqr;

	private static readonly IntPtr NativeFieldInfoPtr__hardShadowDistanceSqr;

	private static readonly IntPtr NativeFieldInfoPtr__appliedShadowsMode;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateShadows_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RecalculateDistances_Private_Void_1;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Method_Private_Void_0;

	public unsafe static int RefreshMovementThreshold
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RefreshMovementThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RefreshMovementThreshold, (void*)(&num));
		}
	}

	public unsafe bool _softShadowEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__softShadowEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__softShadowEnabled)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<Light> _lights
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lights);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Light>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lights)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float _softShadowDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__softShadowDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__softShadowDistance)) = num;
		}
	}

	public unsafe float _hardShadowDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hardShadowDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hardShadowDistance)) = num;
		}
	}

	public unsafe float _softShadowDistanceSqr
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__softShadowDistanceSqr);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__softShadowDistanceSqr)) = num;
		}
	}

	public unsafe float _hardShadowDistanceSqr
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hardShadowDistanceSqr);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hardShadowDistanceSqr)) = num;
		}
	}

	public unsafe LightShadows _appliedShadowsMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__appliedShadowsMode);
			return *(LightShadows*)num;
		}
		set
		{
			*(LightShadows*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__appliedShadowsMode)) = lightShadows;
		}
	}

	static ShadowLODController()
	{
		Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Lighting", "ShadowLODController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr);
		NativeFieldInfoPtr_RefreshMovementThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "RefreshMovementThreshold");
		NativeFieldInfoPtr__softShadowEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_softShadowEnabled");
		NativeFieldInfoPtr__lights = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_lights");
		NativeFieldInfoPtr__softShadowDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_softShadowDistance");
		NativeFieldInfoPtr__hardShadowDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_hardShadowDistance");
		NativeFieldInfoPtr__softShadowDistanceSqr = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_softShadowDistanceSqr");
		NativeFieldInfoPtr__hardShadowDistanceSqr = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_hardShadowDistanceSqr");
		NativeFieldInfoPtr__appliedShadowsMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, "_appliedShadowsMode");
		NativeMethodInfoPtr_Awake_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674883);
		NativeMethodInfoPtr_OnEnable_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674884);
		NativeMethodInfoPtr_OnDestroy_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674885);
		NativeMethodInfoPtr_UpdateShadows_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674886);
		NativeMethodInfoPtr_RecalculateDistances_Private_Void_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674887);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674888);
		NativeMethodInfoPtr_Method_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr, 100674889);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 192897, XrefRangeEnd = 192925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 192925, XrefRangeEnd = 192929, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 192929, XrefRangeEnd = 192943, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 192957, RefRangeEnd = 192958, XrefRangeStart = 192943, XrefRangeEnd = 192957, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateShadows()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateShadows_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void RecalculateDistances()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RecalculateDistances_Private_Void_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 192958, XrefRangeEnd = 192959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShadowLODController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShadowLODController>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 192990, RefRangeEnd = 192991, XrefRangeStart = 192959, XrefRangeEnd = 192990, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Method_Private_Void_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ShadowLODController(IntPtr pointer)
		: base(pointer)
	{
	}
}
