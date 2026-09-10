using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppVLB;

public class TriggerZone : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "TriggerZoneUpdateRate")]
	public enum TriggerZoneUpdateRate
	{
		OnEnable,
		OnOcclusionChange
	}

	private static readonly IntPtr NativeFieldInfoPtr_ClassName;

	private static readonly IntPtr NativeFieldInfoPtr_setIsTrigger;

	private static readonly IntPtr NativeFieldInfoPtr_rangeMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_kMeshColliderNumSides;

	private static readonly IntPtr NativeFieldInfoPtr_m_Beam;

	private static readonly IntPtr NativeFieldInfoPtr_m_DynamicOcclusionRaycasting;

	private static readonly IntPtr NativeFieldInfoPtr_m_PolygonCollider2D;

	private static readonly IntPtr NativeMethodInfoPtr_get_updateRate_Private_get_TriggerZoneUpdateRate_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnOcclusionProcessed_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ComputeZone_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static string ClassName
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ClassName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ClassName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool setIsTrigger
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_setIsTrigger);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_setIsTrigger)) = flag;
		}
	}

	public unsafe float rangeMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rangeMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rangeMultiplier)) = num;
		}
	}

	public unsafe static int kMeshColliderNumSides
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kMeshColliderNumSides, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kMeshColliderNumSides, (void*)(&num));
		}
	}

	public unsafe VolumetricLightBeamAbstractBase m_Beam
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Beam);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VolumetricLightBeamAbstractBase>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Beam)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volumetricLightBeamAbstractBase));
		}
	}

	public unsafe DynamicOcclusionRaycasting m_DynamicOcclusionRaycasting
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_DynamicOcclusionRaycasting);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DynamicOcclusionRaycasting>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_DynamicOcclusionRaycasting)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dynamicOcclusionRaycasting));
		}
	}

	public unsafe PolygonCollider2D m_PolygonCollider2D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_PolygonCollider2D);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PolygonCollider2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_PolygonCollider2D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)polygonCollider2D));
		}
	}

	public unsafe TriggerZoneUpdateRate updateRate
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73862, XrefRangeEnd = 73867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_updateRate_Private_get_TriggerZoneUpdateRate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(TriggerZoneUpdateRate*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static TriggerZone()
	{
		Il2CppClassPointerStore<TriggerZone>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "TriggerZone");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr);
		NativeFieldInfoPtr_ClassName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "ClassName");
		NativeFieldInfoPtr_setIsTrigger = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "setIsTrigger");
		NativeFieldInfoPtr_rangeMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "rangeMultiplier");
		NativeFieldInfoPtr_kMeshColliderNumSides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "kMeshColliderNumSides");
		NativeFieldInfoPtr_m_Beam = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "m_Beam");
		NativeFieldInfoPtr_m_DynamicOcclusionRaycasting = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "m_DynamicOcclusionRaycasting");
		NativeFieldInfoPtr_m_PolygonCollider2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, "m_PolygonCollider2D");
		NativeMethodInfoPtr_get_updateRate_Private_get_TriggerZoneUpdateRate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, 100664374);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, 100664375);
		NativeMethodInfoPtr_OnOcclusionProcessed_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, 100664376);
		NativeMethodInfoPtr_ComputeZone_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, 100664377);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr, 100664378);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73867, XrefRangeEnd = 73891, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73891, XrefRangeEnd = 73892, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnOcclusionProcessed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnOcclusionProcessed_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 73947, RefRangeEnd = 73948, XrefRangeStart = 73892, XrefRangeEnd = 73947, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeZone()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeZone_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 73948, XrefRangeEnd = 73949, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TriggerZone()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TriggerZone>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TriggerZone(IntPtr pointer)
		: base(pointer)
	{
	}
}
