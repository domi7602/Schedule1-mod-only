using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Quests;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class DeaddropQuestData : QuestData
{
	private static readonly IntPtr NativeFieldInfoPtr_DeaddropGUID;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_Boolean_String_String_Boolean_GameDateTimeData_Il2CppReferenceArray_1_QuestEntryData_String_0;

	public unsafe string DeaddropGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeaddropGUID);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeaddropGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static DeaddropQuestData()
	{
		Il2CppClassPointerStore<DeaddropQuestData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "DeaddropQuestData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeaddropQuestData>.NativeClassPtr);
		NativeFieldInfoPtr_DeaddropGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeaddropQuestData>.NativeClassPtr, "DeaddropGUID");
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_Boolean_String_String_Boolean_GameDateTimeData_Il2CppReferenceArray_1_QuestEntryData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeaddropQuestData>.NativeClassPtr, 100669455);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135281, RefRangeEnd = 135282, XrefRangeStart = 135274, XrefRangeEnd = 135281, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeaddropQuestData(string guid, EQuestState state, bool isTracked, string title, string desc, bool isTimed, GameDateTimeData expiry, Il2CppReferenceArray<QuestEntryData> entries, string deaddropGUID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeaddropQuestData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[9];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(guid);
		*(EQuestState**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &state;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &isTracked;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(title);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(desc);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &isTimed;
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)expiry);
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entries);
		*(IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(deaddropGUID);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuestState_Boolean_String_String_Boolean_GameDateTimeData_Il2CppReferenceArray_1_QuestEntryData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeaddropQuestData(IntPtr pointer)
		: base(pointer)
	{
	}
}
