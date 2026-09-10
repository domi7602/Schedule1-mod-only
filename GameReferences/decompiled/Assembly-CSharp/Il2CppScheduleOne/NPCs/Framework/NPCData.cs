using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class NPCData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__basicInfo;

	private static readonly System.IntPtr NativeFieldInfoPtr__appearance;

	private static readonly System.IntPtr NativeFieldInfoPtr__health;

	private static readonly System.IntPtr NativeFieldInfoPtr__movement;

	private static readonly System.IntPtr NativeFieldInfoPtr__interaction;

	private static readonly System.IntPtr NativeFieldInfoPtr__relationship;

	private static readonly System.IntPtr NativeFieldInfoPtr__messaging;

	private static readonly System.IntPtr NativeFieldInfoPtr__dialogue;

	private static readonly System.IntPtr NativeFieldInfoPtr__voice;

	private static readonly System.IntPtr NativeFieldInfoPtr__inventory;

	private static readonly System.IntPtr NativeFieldInfoPtr__behaviour;

	private static readonly System.IntPtr NativeFieldInfoPtr__weatherBehaviour;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_BasicInfo_Public_get_BasicInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Appearance_Public_get_Appearance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Health_Public_get_Health_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Movement_Public_get_Movement_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Interaction_Public_get_Interaction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Relationship_Public_get_Relationship_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Messaging_Public_get_Messaging_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Dialogue_Public_get_Dialogue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Voice_Public_get_Voice_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Inventory_Public_get_Inventory_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Behaviour_Public_get_Behaviour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_WeatherBehaviour_Public_get_WeatherBehaviour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_New_NPCData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PopulateNPCData_Protected_Void_NPCData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ValueOrReference<BasicInfo, BasicInfoPreset> _basicInfo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__basicInfo);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<BasicInfo, BasicInfoPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__basicInfo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Appearance, AppearancePreset> _appearance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__appearance);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Appearance, AppearancePreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__appearance)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Health, HealthPreset> _health
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__health);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Health, HealthPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__health)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Movement, MovementPreset> _movement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__movement);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Movement, MovementPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__movement)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Interaction, InteractionPreset> _interaction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__interaction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Interaction, InteractionPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__interaction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Relationship, RelationshipPreset> _relationship
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__relationship);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Relationship, RelationshipPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__relationship)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Messaging, MessagingPreset> _messaging
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__messaging);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Messaging, MessagingPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__messaging)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Dialogue, DialoguePreset> _dialogue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dialogue);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Dialogue, DialoguePreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dialogue)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Voice, VoicePreset> _voice
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__voice);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Voice, VoicePreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__voice)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Inventory, InventoryPreset> _inventory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inventory);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Inventory, InventoryPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inventory)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<Behaviour, BehaviourPreset> _behaviour
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__behaviour);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<Behaviour, BehaviourPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__behaviour)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe ValueOrReference<WeatherBehaviour, WeatherBehaviourPreset> _weatherBehaviour
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weatherBehaviour);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueOrReference<WeatherBehaviour, WeatherBehaviourPreset>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weatherBehaviour)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueOrReference));
		}
	}

	public unsafe BasicInfo BasicInfo
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 272366, RefRangeEnd = 272376, XrefRangeStart = 272362, XrefRangeEnd = 272366, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BasicInfo_Public_get_BasicInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BasicInfo>(intPtr) : null;
		}
	}

	public unsafe Appearance Appearance
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 272380, RefRangeEnd = 272386, XrefRangeStart = 272376, XrefRangeEnd = 272380, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Appearance_Public_get_Appearance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Appearance>(intPtr) : null;
		}
	}

	public unsafe Health Health
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 272390, RefRangeEnd = 272402, XrefRangeStart = 272386, XrefRangeEnd = 272390, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Health_Public_get_Health_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Health>(intPtr) : null;
		}
	}

	public unsafe Movement Movement
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 272406, RefRangeEnd = 272411, XrefRangeStart = 272402, XrefRangeEnd = 272406, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Movement_Public_get_Movement_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Movement>(intPtr) : null;
		}
	}

	public unsafe Interaction Interaction
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 272415, RefRangeEnd = 272416, XrefRangeStart = 272411, XrefRangeEnd = 272415, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Interaction_Public_get_Interaction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Interaction>(intPtr) : null;
		}
	}

	public unsafe Relationship Relationship
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 272420, RefRangeEnd = 272423, XrefRangeStart = 272416, XrefRangeEnd = 272420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Relationship_Public_get_Relationship_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Relationship>(intPtr) : null;
		}
	}

	public unsafe Messaging Messaging
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 272427, RefRangeEnd = 272431, XrefRangeStart = 272423, XrefRangeEnd = 272427, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Messaging_Public_get_Messaging_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Messaging>(intPtr) : null;
		}
	}

	public unsafe Dialogue Dialogue
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 272435, RefRangeEnd = 272436, XrefRangeStart = 272431, XrefRangeEnd = 272435, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Dialogue_Public_get_Dialogue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dialogue>(intPtr) : null;
		}
	}

	public unsafe Voice Voice
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 272440, RefRangeEnd = 272442, XrefRangeStart = 272436, XrefRangeEnd = 272440, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Voice_Public_get_Voice_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Voice>(intPtr) : null;
		}
	}

	public unsafe Inventory Inventory
	{
		[CallerCount(19)]
		[CachedScanResults(RefRangeStart = 272446, RefRangeEnd = 272465, XrefRangeStart = 272442, XrefRangeEnd = 272446, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Inventory_Public_get_Inventory_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Inventory>(intPtr) : null;
		}
	}

	public unsafe Behaviour Behaviour
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 272469, RefRangeEnd = 272472, XrefRangeStart = 272465, XrefRangeEnd = 272469, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Behaviour_Public_get_Behaviour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Behaviour>(intPtr) : null;
		}
	}

	public unsafe WeatherBehaviour WeatherBehaviour
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 272476, RefRangeEnd = 272479, XrefRangeStart = 272472, XrefRangeEnd = 272476, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_WeatherBehaviour_Public_get_WeatherBehaviour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<WeatherBehaviour>(intPtr) : null;
		}
	}

	static NPCData()
	{
		Il2CppClassPointerStore<NPCData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "NPCData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NPCData>.NativeClassPtr);
		NativeFieldInfoPtr__basicInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_basicInfo");
		NativeFieldInfoPtr__appearance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_appearance");
		NativeFieldInfoPtr__health = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_health");
		NativeFieldInfoPtr__movement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_movement");
		NativeFieldInfoPtr__interaction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_interaction");
		NativeFieldInfoPtr__relationship = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_relationship");
		NativeFieldInfoPtr__messaging = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_messaging");
		NativeFieldInfoPtr__dialogue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_dialogue");
		NativeFieldInfoPtr__voice = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_voice");
		NativeFieldInfoPtr__inventory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_inventory");
		NativeFieldInfoPtr__behaviour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_behaviour");
		NativeFieldInfoPtr__weatherBehaviour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCData>.NativeClassPtr, "_weatherBehaviour");
		NativeMethodInfoPtr_get_BasicInfo_Public_get_BasicInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682850);
		NativeMethodInfoPtr_get_Appearance_Public_get_Appearance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682851);
		NativeMethodInfoPtr_get_Health_Public_get_Health_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682852);
		NativeMethodInfoPtr_get_Movement_Public_get_Movement_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682853);
		NativeMethodInfoPtr_get_Interaction_Public_get_Interaction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682854);
		NativeMethodInfoPtr_get_Relationship_Public_get_Relationship_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682855);
		NativeMethodInfoPtr_get_Messaging_Public_get_Messaging_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682856);
		NativeMethodInfoPtr_get_Dialogue_Public_get_Dialogue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682857);
		NativeMethodInfoPtr_get_Voice_Public_get_Voice_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682858);
		NativeMethodInfoPtr_get_Inventory_Public_get_Inventory_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682859);
		NativeMethodInfoPtr_get_Behaviour_Public_get_Behaviour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682860);
		NativeMethodInfoPtr_get_WeatherBehaviour_Public_get_WeatherBehaviour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682861);
		NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_New_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682862);
		NativeMethodInfoPtr_PopulateNPCData_Protected_Void_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682863);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCData>.NativeClassPtr, 100682864);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272479, XrefRangeEnd = 272484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual NPCData GetDeepCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_New_NPCData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NPCData>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 272667, RefRangeEnd = 272672, XrefRangeStart = 272484, XrefRangeEnd = 272667, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PopulateNPCData(NPCData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PopulateNPCData_Protected_Void_NPCData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 272805, RefRangeEnd = 272811, XrefRangeStart = 272672, XrefRangeEnd = 272805, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NPCData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NPCData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NPCData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
