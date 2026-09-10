using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.NPCs.Responses;
using Il2CppScheduleOne.Noise;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vision;
using UnityEngine;
using UnityEngine.Events;

namespace Il2CppScheduleOne.NPCs;

public class NPCAwareness : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_PLAYER_AIM_DETECTION_RANGE;

	private static readonly IntPtr NativeFieldInfoPtr_AwarenessActiveByDefault;

	private static readonly IntPtr NativeFieldInfoPtr_VisionCone;

	private static readonly IntPtr NativeFieldInfoPtr_Listener;

	private static readonly IntPtr NativeFieldInfoPtr_Responses;

	private static readonly IntPtr NativeFieldInfoPtr_onNoticedGeneralCrime;

	private static readonly IntPtr NativeFieldInfoPtr_onNoticedPettyCrime;

	private static readonly IntPtr NativeFieldInfoPtr_onNoticedDrugDealing;

	private static readonly IntPtr NativeFieldInfoPtr_onNoticedPlayerViolatingCurfew;

	private static readonly IntPtr NativeFieldInfoPtr_onNoticedSuspiciousPlayer;

	private static readonly IntPtr NativeFieldInfoPtr_onGunshotHeard;

	private static readonly IntPtr NativeFieldInfoPtr_onExplosionHeard;

	private static readonly IntPtr NativeFieldInfoPtr_onHitByCar;

	private static readonly IntPtr NativeFieldInfoPtr_npc;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetAwarenessActive_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_VisionEvent_Public_Void_VisionEventReceipt_0;

	private static readonly IntPtr NativeMethodInfoPtr_NoiseEvent_Public_Void_NoiseEvent_0;

	private static readonly IntPtr NativeMethodInfoPtr_HitByCar_Public_Void_LandVehicle_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float PLAYER_AIM_DETECTION_RANGE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PLAYER_AIM_DETECTION_RANGE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PLAYER_AIM_DETECTION_RANGE, (void*)(&num));
		}
	}

	public unsafe bool AwarenessActiveByDefault
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AwarenessActiveByDefault);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AwarenessActiveByDefault)) = flag;
		}
	}

	public unsafe VisionCone VisionCone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VisionCone);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VisionCone>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VisionCone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)visionCone));
		}
	}

	public unsafe Listener Listener
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Listener);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Listener>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Listener)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener));
		}
	}

	public unsafe NPCResponses Responses
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Responses);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NPCResponses>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Responses)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nPCResponses));
		}
	}

	public unsafe UnityEvent<Player> onNoticedGeneralCrime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedGeneralCrime);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<Player>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedGeneralCrime)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<Player> onNoticedPettyCrime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedPettyCrime);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<Player>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedPettyCrime)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<Player> onNoticedDrugDealing
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedDrugDealing);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<Player>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedDrugDealing)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<Player> onNoticedPlayerViolatingCurfew
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedPlayerViolatingCurfew);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<Player>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedPlayerViolatingCurfew)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<Player> onNoticedSuspiciousPlayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedSuspiciousPlayer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<Player>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNoticedSuspiciousPlayer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<NoiseEvent> onGunshotHeard
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onGunshotHeard);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<NoiseEvent>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onGunshotHeard)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<NoiseEvent> onExplosionHeard
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onExplosionHeard);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<NoiseEvent>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onExplosionHeard)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent<LandVehicle> onHitByCar
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onHitByCar);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<LandVehicle>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onHitByCar)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe NPC npc
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_npc);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NPC>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_npc)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nPC));
		}
	}

	static NPCAwareness()
	{
		Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs", "NPCAwareness");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr);
		NativeFieldInfoPtr_PLAYER_AIM_DETECTION_RANGE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "PLAYER_AIM_DETECTION_RANGE");
		NativeFieldInfoPtr_AwarenessActiveByDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "AwarenessActiveByDefault");
		NativeFieldInfoPtr_VisionCone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "VisionCone");
		NativeFieldInfoPtr_Listener = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "Listener");
		NativeFieldInfoPtr_Responses = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "Responses");
		NativeFieldInfoPtr_onNoticedGeneralCrime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onNoticedGeneralCrime");
		NativeFieldInfoPtr_onNoticedPettyCrime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onNoticedPettyCrime");
		NativeFieldInfoPtr_onNoticedDrugDealing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onNoticedDrugDealing");
		NativeFieldInfoPtr_onNoticedPlayerViolatingCurfew = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onNoticedPlayerViolatingCurfew");
		NativeFieldInfoPtr_onNoticedSuspiciousPlayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onNoticedSuspiciousPlayer");
		NativeFieldInfoPtr_onGunshotHeard = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onGunshotHeard");
		NativeFieldInfoPtr_onExplosionHeard = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onExplosionHeard");
		NativeFieldInfoPtr_onHitByCar = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "onHitByCar");
		NativeFieldInfoPtr_npc = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, "npc");
		NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, 100682302);
		NativeMethodInfoPtr_SetAwarenessActive_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, 100682303);
		NativeMethodInfoPtr_VisionEvent_Public_Void_VisionEventReceipt_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, 100682304);
		NativeMethodInfoPtr_NoiseEvent_Public_Void_NoiseEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, 100682305);
		NativeMethodInfoPtr_HitByCar_Public_Void_LandVehicle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, 100682306);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr, 100682307);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 267060, XrefRangeEnd = 267104, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 267108, RefRangeEnd = 267119, XrefRangeStart = 267104, XrefRangeEnd = 267108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetAwarenessActive(bool active)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&active);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAwarenessActive_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 267119, XrefRangeEnd = 267132, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void VisionEvent(VisionEventReceipt vEvent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vEvent);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VisionEvent_Public_Void_VisionEventReceipt_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 267132, XrefRangeEnd = 267145, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void NoiseEvent(NoiseEvent nEvent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nEvent);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NoiseEvent_Public_Void_NoiseEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 267145, XrefRangeEnd = 267152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HitByCar(LandVehicle vehicle)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicle);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HitByCar_Public_Void_LandVehicle_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 141599, RefRangeEnd = 141615, XrefRangeStart = 141599, XrefRangeEnd = 141615, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NPCAwareness()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NPCAwareness>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NPCAwareness(IntPtr pointer)
		: base(pointer)
	{
	}
}
