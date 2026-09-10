using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne;

public static class AchievementManager : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EAchievement")]
	public enum EAchievement
	{
		COMPLETE_PROLOGUE,
		RV_DESTROYED,
		DEALER_RECRUITED,
		MASTER_CHEF,
		BUSINESSMAN,
		BIGWIG,
		MAGNATE,
		UPSTANDING_CITIZEN,
		ROLLING_IN_STYLE,
		LONG_ARM_OF_THE_LAW,
		INDIAN_DEALER,
		URBAN_ARTIST,
		FINISHING_THE_JOB
	}

	[System.Serializable]
	[ObfuscatedName("ScheduleOne.AchievementManager+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__5_1;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__TryPullAchievements_b__5_1_Internal_Boolean_KeyValuePair_2_EAchievement_Boolean_0;

		public unsafe static __c __9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<__c>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c));
			}
		}

		public unsafe static Il2CppSystem.Func<KeyValuePair<EAchievement, bool>, bool> __9__5_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__5_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<KeyValuePair<EAchievement, bool>, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__5_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__5_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__5_1");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665931);
			NativeMethodInfoPtr__TryPullAchievements_b__5_1_Internal_Boolean_KeyValuePair_2_EAchievement_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665932);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90710, XrefRangeEnd = 90711, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _TryPullAchievements_b__5_1(KeyValuePair<EAchievement, bool> kvp)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)kvp));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__TryPullAchievements_b__5_1_Internal_Boolean_KeyValuePair_2_EAchievement_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_achievements;

	private static readonly System.IntPtr NativeFieldInfoPtr_achievementUnlocked;

	private static readonly System.IntPtr NativeFieldInfoPtr__initialized;

	private static readonly System.IntPtr NativeMethodInfoPtr_Init_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryPullAchievements_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnlockAchievement_Public_Static_Void_EAchievement_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Internal_Static_Void_PDM_0;

	public unsafe static Il2CppStructArray<EAchievement> achievements
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_achievements, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<EAchievement>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_achievements, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Dictionary<EAchievement, bool> achievementUnlocked
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_achievementUnlocked, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EAchievement, bool>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_achievementUnlocked, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static bool _initialized
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__initialized, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__initialized, (void*)(&flag));
		}
	}

	static AchievementManager()
	{
		Il2CppClassPointerStore<AchievementManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "AchievementManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr);
		NativeFieldInfoPtr_achievements = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, "achievements");
		NativeFieldInfoPtr_achievementUnlocked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, "achievementUnlocked");
		NativeFieldInfoPtr__initialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, "_initialized");
		NativeMethodInfoPtr_Init_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, 100665925);
		NativeMethodInfoPtr_TryPullAchievements_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, 100665926);
		NativeMethodInfoPtr_UnlockAchievement_Public_Static_Void_EAchievement_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, 100665927);
		NativeMethodInfoPtr_Method_Internal_Static_Void_PDM_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AchievementManager>.NativeClassPtr, 100665929);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90711, XrefRangeEnd = 90747, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Init()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Init_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90797, RefRangeEnd = 90798, XrefRangeStart = 90747, XrefRangeEnd = 90797, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void TryPullAchievements()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryPullAchievements_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 90829, RefRangeEnd = 90841, XrefRangeStart = 90798, XrefRangeEnd = 90829, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void UnlockAchievement(EAchievement achievement)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&achievement);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnlockAchievement_Public_Static_Void_EAchievement_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90841, XrefRangeEnd = 90856, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Method_Internal_Static_Void_PDM_0()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Internal_Static_Void_PDM_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AchievementManager(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
