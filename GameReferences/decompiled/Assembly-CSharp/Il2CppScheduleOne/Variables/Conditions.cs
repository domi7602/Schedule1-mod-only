using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Variables;

[System.Serializable]
public class Conditions : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EEvaluationType")]
	public enum EEvaluationType
	{
		And,
		Or
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_EvaluationType;

	private static readonly System.IntPtr NativeFieldInfoPtr_ConditionList;

	private static readonly System.IntPtr NativeFieldInfoPtr_QuestConditionList;

	private static readonly System.IntPtr NativeMethodInfoPtr_Evaluate_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EEvaluationType EvaluationType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EvaluationType);
			return *(EEvaluationType*)num;
		}
		set
		{
			*(EEvaluationType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EvaluationType)) = eEvaluationType;
		}
	}

	public unsafe Il2CppReferenceArray<Condition> ConditionList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConditionList);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Condition>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConditionList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<QuestCondition> QuestConditionList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestConditionList);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<QuestCondition>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QuestConditionList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static Conditions()
	{
		Il2CppClassPointerStore<Conditions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Variables", "Conditions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Conditions>.NativeClassPtr);
		NativeFieldInfoPtr_EvaluationType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Conditions>.NativeClassPtr, "EvaluationType");
		NativeFieldInfoPtr_ConditionList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Conditions>.NativeClassPtr, "ConditionList");
		NativeFieldInfoPtr_QuestConditionList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Conditions>.NativeClassPtr, "QuestConditionList");
		NativeMethodInfoPtr_Evaluate_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Conditions>.NativeClassPtr, 100666519);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Conditions>.NativeClassPtr, 100666520);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 97251, RefRangeEnd = 97253, XrefRangeStart = 97240, XrefRangeEnd = 97251, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Evaluate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Evaluate_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Conditions()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Conditions>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Conditions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
