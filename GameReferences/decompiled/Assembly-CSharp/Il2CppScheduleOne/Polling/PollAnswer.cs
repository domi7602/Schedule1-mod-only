using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Polling;

[System.Serializable]
public class PollAnswer : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_pollId;

	private static readonly System.IntPtr NativeFieldInfoPtr_answer;

	private static readonly System.IntPtr NativeFieldInfoPtr_ticket;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_String_0;

	public unsafe int pollId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pollId);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pollId)) = num;
		}
	}

	public unsafe int answer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_answer);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_answer)) = num;
		}
	}

	public unsafe string ticket
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ticket);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ticket)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static PollAnswer()
	{
		Il2CppClassPointerStore<PollAnswer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Polling", "PollAnswer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PollAnswer>.NativeClassPtr);
		NativeFieldInfoPtr_pollId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PollAnswer>.NativeClassPtr, "pollId");
		NativeFieldInfoPtr_answer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PollAnswer>.NativeClassPtr, "answer");
		NativeFieldInfoPtr_ticket = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PollAnswer>.NativeClassPtr, "ticket");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PollAnswer>.NativeClassPtr, 100668113);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 115210, XrefRangeEnd = 115212, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PollAnswer(int _pollId, int _answer, string _ticket)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PollAnswer>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&_pollId);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &_answer;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(_ticket);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PollAnswer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
