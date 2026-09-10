using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.NPCs.CharacterClasses;
using Il2CppScheduleOne.Property;

namespace Il2CppScheduleOne.Quests;

public class Quest_WelcomeToHylandPoint : Quest
{
	private static readonly IntPtr NativeFieldInfoPtr_ReturnToRVQuest;

	private static readonly IntPtr NativeFieldInfoPtr_ReadMessagesQuest;

	private static readonly IntPtr NativeFieldInfoPtr_RV;

	private static readonly IntPtr NativeFieldInfoPtr_Nelson;

	private static readonly IntPtr NativeFieldInfoPtr_ExplosionMaxDist;

	private static readonly IntPtr NativeFieldInfoPtr_ExplosionMinDist;

	private static readonly IntPtr NativeFieldInfoPtr_cameraLookTime;

	private static readonly IntPtr NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetQuestState_Public_Virtual_Void_EQuestState_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_BlowupRV_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRVDestroyed_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe QuestEntry ReturnToRVQuest
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReturnToRVQuest);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QuestEntry>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReturnToRVQuest)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)questEntry));
		}
	}

	public unsafe QuestEntry ReadMessagesQuest
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReadMessagesQuest);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QuestEntry>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReadMessagesQuest)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)questEntry));
		}
	}

	public unsafe RV RV
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RV);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RV>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RV)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rV));
		}
	}

	public unsafe UncleNelson Nelson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Nelson);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UncleNelson>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Nelson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uncleNelson));
		}
	}

	public unsafe float ExplosionMaxDist
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExplosionMaxDist);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExplosionMaxDist)) = num;
		}
	}

	public unsafe float ExplosionMinDist
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExplosionMinDist);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExplosionMinDist)) = num;
		}
	}

	public unsafe float cameraLookTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cameraLookTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cameraLookTime)) = num;
		}
	}

	static Quest_WelcomeToHylandPoint()
	{
		Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "Quest_WelcomeToHylandPoint");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr);
		NativeFieldInfoPtr_ReturnToRVQuest = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "ReturnToRVQuest");
		NativeFieldInfoPtr_ReadMessagesQuest = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "ReadMessagesQuest");
		NativeFieldInfoPtr_RV = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "RV");
		NativeFieldInfoPtr_Nelson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "Nelson");
		NativeFieldInfoPtr_ExplosionMaxDist = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "ExplosionMaxDist");
		NativeFieldInfoPtr_ExplosionMinDist = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "ExplosionMinDist");
		NativeFieldInfoPtr_cameraLookTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, "cameraLookTime");
		NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, 100667732);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, 100667733);
		NativeMethodInfoPtr_SetQuestState_Public_Virtual_Void_EQuestState_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, 100667734);
		NativeMethodInfoPtr_BlowupRV_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, 100667735);
		NativeMethodInfoPtr_SetRVDestroyed_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, 100667736);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr, 100667737);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111582, XrefRangeEnd = 111584, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnUncappedMinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111584, XrefRangeEnd = 111593, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111593, XrefRangeEnd = 111597, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SetQuestState(EQuestState state, bool network = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&state);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &network;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetQuestState_Public_Virtual_Void_EQuestState_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111597, XrefRangeEnd = 111630, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BlowupRV()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BlowupRV_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111630, XrefRangeEnd = 111640, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRVDestroyed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRVDestroyed_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111640, XrefRangeEnd = 111644, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Quest_WelcomeToHylandPoint()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Quest_WelcomeToHylandPoint>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Quest_WelcomeToHylandPoint(IntPtr pointer)
		: base(pointer)
	{
	}
}
