using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppSystem;

namespace Il2CppScheduleOne.Messaging;

[System.Serializable]
public class Message : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "ESenderType")]
	public enum ESenderType
	{
		Player,
		Other
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_messageId;

	private static readonly System.IntPtr NativeFieldInfoPtr_text;

	private static readonly System.IntPtr NativeFieldInfoPtr_sender;

	private static readonly System.IntPtr NativeFieldInfoPtr_endOfGroup;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_ESenderType_Boolean_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_TextMessageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSaveData_Public_TextMessageData_0;

	public unsafe int messageId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_messageId);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_messageId)) = num;
		}
	}

	public unsafe string text
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_text);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_text)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe ESenderType sender
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sender);
			return *(ESenderType*)num;
		}
		set
		{
			*(ESenderType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sender)) = eSenderType;
		}
	}

	public unsafe bool endOfGroup
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endOfGroup);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endOfGroup)) = flag;
		}
	}

	static Message()
	{
		Il2CppClassPointerStore<Message>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Messaging", "Message");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Message>.NativeClassPtr);
		NativeFieldInfoPtr_messageId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Message>.NativeClassPtr, "messageId");
		NativeFieldInfoPtr_text = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Message>.NativeClassPtr, "text");
		NativeFieldInfoPtr_sender = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Message>.NativeClassPtr, "sender");
		NativeFieldInfoPtr_endOfGroup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Message>.NativeClassPtr, "endOfGroup");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Message>.NativeClassPtr, 100669727);
		NativeMethodInfoPtr__ctor_Public_Void_String_ESenderType_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Message>.NativeClassPtr, 100669728);
		NativeMethodInfoPtr__ctor_Public_Void_TextMessageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Message>.NativeClassPtr, 100669729);
		NativeMethodInfoPtr_GetSaveData_Public_TextMessageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Message>.NativeClassPtr, 100669730);
	}

	[CallerCount(53)]
	[CachedScanResults(RefRangeStart = 137626, RefRangeEnd = 137679, XrefRangeStart = 137625, XrefRangeEnd = 137626, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Message()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Message>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 137682, RefRangeEnd = 137693, XrefRangeStart = 137679, XrefRangeEnd = 137682, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Message(string _text, ESenderType _type, bool _endOfGroup = false, int _messageId = -1)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Message>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(_text);
		*(ESenderType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &_type;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &_endOfGroup;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &_messageId;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_ESenderType_Boolean_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 137695, RefRangeEnd = 137696, XrefRangeStart = 137693, XrefRangeEnd = 137695, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Message(TextMessageData data)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Message>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_TextMessageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 137701, RefRangeEnd = 137702, XrefRangeStart = 137696, XrefRangeEnd = 137701, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TextMessageData GetSaveData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSaveData_Public_TextMessageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TextMessageData>(intPtr) : null;
	}

	public Message(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
