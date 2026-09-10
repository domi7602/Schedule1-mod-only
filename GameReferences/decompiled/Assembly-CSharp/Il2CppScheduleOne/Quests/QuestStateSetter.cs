using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Quests;

[System.Serializable]
public class QuestStateSetter : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_QuestName;

	private static readonly System.IntPtr NativeFieldInfoPtr_SetQuestState;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestState;

	private static readonly System.IntPtr NativeFieldInfoPtr_SetQuestEntryState;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestEntryIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestEntryState;

	private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe bool SetQuestState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetQuestState);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetQuestState)) = flag;
		}
	}

	public unsafe QuestManager.EQuestAction QuestState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestState);
			return *(QuestManager.EQuestAction*)num;
		}
		set
		{
			*(QuestManager.EQuestAction*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestState)) = eQuestAction;
		}
	}

	public unsafe bool SetQuestEntryState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetQuestEntryState);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetQuestEntryState)) = flag;
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

	static QuestStateSetter()
	{
		Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "QuestStateSetter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr);
		NativeFieldInfoPtr_QuestName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, "QuestName");
		NativeFieldInfoPtr_SetQuestState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, "SetQuestState");
		NativeFieldInfoPtr_QuestState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, "QuestState");
		NativeFieldInfoPtr_SetQuestEntryState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, "SetQuestEntryState");
		NativeFieldInfoPtr_QuestEntryIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, "QuestEntryIndex");
		NativeFieldInfoPtr_QuestEntryState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, "QuestEntryState");
		NativeMethodInfoPtr_Execute_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, 100667740);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr, 100667741);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 111670, RefRangeEnd = 111671, XrefRangeStart = 111651, XrefRangeEnd = 111670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Execute()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Execute_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QuestStateSetter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QuestStateSetter>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QuestStateSetter(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
