using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Variables;
using Il2CppSystem;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Quests;

[System.Serializable]
public class SystemTrigger : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Conditions;

	private static readonly System.IntPtr NativeFieldInfoPtr_onEvaluateTrueVariableSetters;

	private static readonly System.IntPtr NativeFieldInfoPtr_onEvaluateTrueQuestSetters;

	private static readonly System.IntPtr NativeFieldInfoPtr_onEvaluateTrue;

	private static readonly System.IntPtr NativeFieldInfoPtr_onEvaluateFalseVariableSetters;

	private static readonly System.IntPtr NativeFieldInfoPtr_onEvaluateFalseQuestSetters;

	private static readonly System.IntPtr NativeFieldInfoPtr_onEvaluateFalse;

	private static readonly System.IntPtr NativeMethodInfoPtr_Trigger_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Conditions Conditions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Conditions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Conditions>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Conditions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conditions));
		}
	}

	public unsafe Il2CppReferenceArray<VariableSetter> onEvaluateTrueVariableSetters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateTrueVariableSetters);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<VariableSetter>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateTrueVariableSetters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<QuestStateSetter> onEvaluateTrueQuestSetters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateTrueQuestSetters);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<QuestStateSetter>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateTrueQuestSetters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe UnityEvent onEvaluateTrue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateTrue);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateTrue)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe Il2CppReferenceArray<VariableSetter> onEvaluateFalseVariableSetters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateFalseVariableSetters);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<VariableSetter>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateFalseVariableSetters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<QuestStateSetter> onEvaluateFalseQuestSetters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateFalseQuestSetters);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<QuestStateSetter>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateFalseQuestSetters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe UnityEvent onEvaluateFalse
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateFalse);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEvaluateFalse)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	static SystemTrigger()
	{
		Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "SystemTrigger");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr);
		NativeFieldInfoPtr_Conditions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "Conditions");
		NativeFieldInfoPtr_onEvaluateTrueVariableSetters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "onEvaluateTrueVariableSetters");
		NativeFieldInfoPtr_onEvaluateTrueQuestSetters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "onEvaluateTrueQuestSetters");
		NativeFieldInfoPtr_onEvaluateTrue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "onEvaluateTrue");
		NativeFieldInfoPtr_onEvaluateFalseVariableSetters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "onEvaluateFalseVariableSetters");
		NativeFieldInfoPtr_onEvaluateFalseQuestSetters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "onEvaluateFalseQuestSetters");
		NativeFieldInfoPtr_onEvaluateFalse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, "onEvaluateFalse");
		NativeMethodInfoPtr_Trigger_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, 100667748);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr, 100667749);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 111715, RefRangeEnd = 111719, XrefRangeStart = 111709, XrefRangeEnd = 111715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Trigger()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Trigger_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SystemTrigger()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SystemTrigger>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SystemTrigger(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
