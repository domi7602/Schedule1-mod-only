using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Quests;
using Il2CppSystem;

namespace Il2CppScheduleOne.Variables;

[System.Serializable]
public class QuestCondition : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_CheckQuestState;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestName;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestState;

	private static readonly System.IntPtr NativeFieldInfoPtr_CheckQuestEntryState;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestEntryIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestEntryState;

	private static readonly System.IntPtr NativeMethodInfoPtr_Evaluate_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool CheckQuestState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CheckQuestState);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CheckQuestState)) = flag;
		}
	}

	public unsafe string QuestName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EQuestState QuestState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestState);
			return *(EQuestState*)num;
		}
		set
		{
			*(EQuestState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestState)) = eQuestState;
		}
	}

	public unsafe bool CheckQuestEntryState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CheckQuestEntryState);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CheckQuestEntryState)) = flag;
		}
	}

	public unsafe int QuestEntryIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestEntryIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestEntryIndex)) = num;
		}
	}

	public unsafe EQuestState QuestEntryState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestEntryState);
			return *(EQuestState*)num;
		}
		set
		{
			*(EQuestState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestEntryState)) = eQuestState;
		}
	}

	static QuestCondition()
	{
		Il2CppClassPointerStore<QuestCondition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Variables", "QuestCondition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr);
		NativeFieldInfoPtr_CheckQuestState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, "CheckQuestState");
		NativeFieldInfoPtr_QuestName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, "QuestName");
		NativeFieldInfoPtr_QuestState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, "QuestState");
		NativeFieldInfoPtr_CheckQuestEntryState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, "CheckQuestEntryState");
		NativeFieldInfoPtr_QuestEntryIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, "QuestEntryIndex");
		NativeFieldInfoPtr_QuestEntryState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, "QuestEntryState");
		NativeMethodInfoPtr_Evaluate_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, 100666524);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr, 100666525);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 97269, XrefRangeEnd = 97285, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Evaluate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Evaluate_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 97285, XrefRangeEnd = 97290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QuestCondition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QuestCondition>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QuestCondition(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
