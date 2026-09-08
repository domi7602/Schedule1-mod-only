using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

public class QuestManagerData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Quests;

	private static readonly IntPtr NativeFieldInfoPtr_Contracts;

	private static readonly IntPtr NativeFieldInfoPtr_DeaddropQuests;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_QuestData_Il2CppReferenceArray_1_ContractData_Il2CppReferenceArray_1_DeaddropQuestData_0;

	public unsafe Il2CppReferenceArray<QuestData> Quests
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quests);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<QuestData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quests)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<ContractData> Contracts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Contracts);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ContractData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Contracts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<DeaddropQuestData> DeaddropQuests
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeaddropQuests);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<DeaddropQuestData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DeaddropQuests)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static QuestManagerData()
	{
		Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "QuestManagerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr);
		NativeFieldInfoPtr_Quests = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr, "Quests");
		NativeFieldInfoPtr_Contracts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr, "Contracts");
		NativeFieldInfoPtr_DeaddropQuests = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr, "DeaddropQuests");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_QuestData_Il2CppReferenceArray_1_ContractData_Il2CppReferenceArray_1_DeaddropQuestData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr, 100669459);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 134792, RefRangeEnd = 134797, XrefRangeStart = 134792, XrefRangeEnd = 134797, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QuestManagerData(Il2CppReferenceArray<QuestData> quests, Il2CppReferenceArray<ContractData> contracts, Il2CppReferenceArray<DeaddropQuestData> deaddropQuests)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QuestManagerData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)quests);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)contracts);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deaddropQuests);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_QuestData_Il2CppReferenceArray_1_ContractData_Il2CppReferenceArray_1_DeaddropQuestData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QuestManagerData(IntPtr pointer)
		: base(pointer)
	{
	}
}
