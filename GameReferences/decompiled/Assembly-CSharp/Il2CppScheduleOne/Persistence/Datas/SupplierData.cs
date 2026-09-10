using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class SupplierData : NPCData
{
	private static readonly IntPtr NativeFieldInfoPtr_timeSinceMeetingStart;

	private static readonly IntPtr NativeFieldInfoPtr_timeSinceLastMeetingEnd;

	private static readonly IntPtr NativeFieldInfoPtr_debt;

	private static readonly IntPtr NativeFieldInfoPtr_minsUntilDeadDropReady;

	private static readonly IntPtr NativeFieldInfoPtr_deaddropItems;

	private static readonly IntPtr NativeFieldInfoPtr_debtReminderSent;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_Single_Int32_Il2CppReferenceArray_1_StringIntPair_Boolean_0;

	public unsafe int timeSinceMeetingStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceMeetingStart);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceMeetingStart)) = num;
		}
	}

	public unsafe int timeSinceLastMeetingEnd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceLastMeetingEnd);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeSinceLastMeetingEnd)) = num;
		}
	}

	public unsafe float debt
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_debt);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_debt)) = num;
		}
	}

	public unsafe int minsUntilDeadDropReady
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minsUntilDeadDropReady);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minsUntilDeadDropReady)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<StringIntPair> deaddropItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_deaddropItems);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<StringIntPair>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_deaddropItems)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool debtReminderSent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_debtReminderSent);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_debtReminderSent)) = flag;
		}
	}

	static SupplierData()
	{
		Il2CppClassPointerStore<SupplierData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "SupplierData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SupplierData>.NativeClassPtr);
		NativeFieldInfoPtr_timeSinceMeetingStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, "timeSinceMeetingStart");
		NativeFieldInfoPtr_timeSinceLastMeetingEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, "timeSinceLastMeetingEnd");
		NativeFieldInfoPtr_debt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, "debt");
		NativeFieldInfoPtr_minsUntilDeadDropReady = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, "minsUntilDeadDropReady");
		NativeFieldInfoPtr_deaddropItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, "deaddropItems");
		NativeFieldInfoPtr_debtReminderSent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, "debtReminderSent");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_Single_Int32_Il2CppReferenceArray_1_StringIntPair_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SupplierData>.NativeClassPtr, 100669417);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135044, RefRangeEnd = 135045, XrefRangeStart = 135041, XrefRangeEnd = 135044, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SupplierData(string id, int _timeSinceMeetingStart, int _timeSinceLastMeetingEnd, float _debt, int _minsUntilDeadDropReady, Il2CppReferenceArray<StringIntPair> _deaddropItems, bool _debtReminderSent)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SupplierData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(id);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &_timeSinceMeetingStart;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &_timeSinceLastMeetingEnd;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &_debt;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &_minsUntilDeadDropReady;
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_deaddropItems);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &_debtReminderSent;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_Single_Int32_Il2CppReferenceArray_1_StringIntPair_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SupplierData(IntPtr pointer)
		: base(pointer)
	{
	}
}
