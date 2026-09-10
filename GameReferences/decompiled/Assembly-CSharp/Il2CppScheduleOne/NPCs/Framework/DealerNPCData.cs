using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.Economy;

namespace Il2CppScheduleOne.NPCs.Framework;

[Serializable]
public class DealerNPCData : NPCData
{
	private static readonly IntPtr NativeFieldInfoPtr_DealerType;

	private static readonly IntPtr NativeFieldInfoPtr_HomeName;

	private static readonly IntPtr NativeFieldInfoPtr_SigningFee;

	private static readonly IntPtr NativeFieldInfoPtr_SalesCutPercentage;

	private static readonly IntPtr NativeFieldInfoPtr_RecruitDialogue;

	private static readonly IntPtr NativeFieldInfoPtr_CollectCashDialogue;

	private static readonly IntPtr NativeFieldInfoPtr_AssignCustomersDialogue;

	private static readonly IntPtr NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_NPCData_0;

	private static readonly IntPtr NativeMethodInfoPtr_PopulateDealerData_Private_Void_DealerNPCData_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EDealerType DealerType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DealerType);
			return *(EDealerType*)num;
		}
		set
		{
			*(EDealerType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DealerType)) = eDealerType;
		}
	}

	public unsafe string HomeName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HomeName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HomeName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float SigningFee
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SigningFee);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SigningFee)) = num;
		}
	}

	public unsafe float SalesCutPercentage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SalesCutPercentage);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SalesCutPercentage)) = num;
		}
	}

	public unsafe DialogueContainer RecruitDialogue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RecruitDialogue);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DialogueContainer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RecruitDialogue)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dialogueContainer));
		}
	}

	public unsafe DialogueContainer CollectCashDialogue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CollectCashDialogue);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DialogueContainer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CollectCashDialogue)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dialogueContainer));
		}
	}

	public unsafe DialogueContainer AssignCustomersDialogue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AssignCustomersDialogue);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DialogueContainer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AssignCustomersDialogue)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dialogueContainer));
		}
	}

	static DealerNPCData()
	{
		Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "DealerNPCData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr);
		NativeFieldInfoPtr_DealerType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "DealerType");
		NativeFieldInfoPtr_HomeName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "HomeName");
		NativeFieldInfoPtr_SigningFee = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "SigningFee");
		NativeFieldInfoPtr_SalesCutPercentage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "SalesCutPercentage");
		NativeFieldInfoPtr_RecruitDialogue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "RecruitDialogue");
		NativeFieldInfoPtr_CollectCashDialogue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "CollectCashDialogue");
		NativeFieldInfoPtr_AssignCustomersDialogue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, "AssignCustomersDialogue");
		NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, 100682842);
		NativeMethodInfoPtr_PopulateDealerData_Private_Void_DealerNPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, 100682843);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr, 100682844);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272325, XrefRangeEnd = 272338, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override NPCData GetDeepCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDeepCopy_Public_Virtual_NPCData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NPCData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272338, XrefRangeEnd = 272344, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PopulateDealerData(DealerNPCData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PopulateDealerData_Private_Void_DealerNPCData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272344, XrefRangeEnd = 272349, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DealerNPCData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DealerNPCData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DealerNPCData(IntPtr pointer)
		: base(pointer)
	{
	}
}
