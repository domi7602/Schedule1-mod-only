using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas.Characters;

public class ThomasData : NPCData
{
	private static readonly IntPtr NativeFieldInfoPtr_MeetingReminderSent;

	private static readonly IntPtr NativeFieldInfoPtr_HandoverReminderSent;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Boolean_0;

	public unsafe bool MeetingReminderSent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MeetingReminderSent);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MeetingReminderSent)) = flag;
		}
	}

	public unsafe bool HandoverReminderSent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandoverReminderSent);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandoverReminderSent)) = flag;
		}
	}

	static ThomasData()
	{
		Il2CppClassPointerStore<ThomasData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas.Characters", "ThomasData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ThomasData>.NativeClassPtr);
		NativeFieldInfoPtr_MeetingReminderSent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ThomasData>.NativeClassPtr, "MeetingReminderSent");
		NativeFieldInfoPtr_HandoverReminderSent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ThomasData>.NativeClassPtr, "HandoverReminderSent");
		NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ThomasData>.NativeClassPtr, 100669487);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 135559, XrefRangeEnd = 135561, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ThomasData(string id, bool meetingReminderSent, bool handoverReminderSent)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ThomasData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(id);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &meetingReminderSent;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &handoverReminderSent;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ThomasData(IntPtr pointer)
		: base(pointer)
	{
	}
}
