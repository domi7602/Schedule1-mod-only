using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Quests;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class QuestEntryData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Name;

	private static readonly IntPtr NativeFieldInfoPtr_State;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string Name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EQuestState State
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_State);
			return *(EQuestState*)num;
		}
		set
		{
			*(EQuestState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_State)) = eQuestState;
		}
	}

	static QuestEntryData()
	{
		Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "QuestEntryData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr);
		NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr, "Name");
		NativeFieldInfoPtr_State = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr, "State");
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr, 100669457);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr, 100669458);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 134770, RefRangeEnd = 134777, XrefRangeStart = 134770, XrefRangeEnd = 134777, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QuestEntryData(string name, EQuestState state)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(EQuestState**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &state;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134345, RefRangeEnd = 134346, XrefRangeStart = 134345, XrefRangeEnd = 134346, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QuestEntryData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QuestEntryData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QuestEntryData(IntPtr pointer)
		: base(pointer)
	{
	}
}
