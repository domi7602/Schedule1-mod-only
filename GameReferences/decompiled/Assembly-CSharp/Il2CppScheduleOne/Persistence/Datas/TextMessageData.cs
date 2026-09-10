using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class TextMessageData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Sender;

	private static readonly System.IntPtr NativeFieldInfoPtr_MessageID;

	private static readonly System.IntPtr NativeFieldInfoPtr_Text;

	private static readonly System.IntPtr NativeFieldInfoPtr_EndOfChain;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int Sender
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Sender);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Sender)) = num;
		}
	}

	public unsafe int MessageID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MessageID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MessageID)) = num;
		}
	}

	public unsafe string Text
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Text);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Text)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool EndOfChain
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EndOfChain);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EndOfChain)) = flag;
		}
	}

	static TextMessageData()
	{
		Il2CppClassPointerStore<TextMessageData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "TextMessageData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr);
		NativeFieldInfoPtr_Sender = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr, "Sender");
		NativeFieldInfoPtr_MessageID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr, "MessageID");
		NativeFieldInfoPtr_Text = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr, "Text");
		NativeFieldInfoPtr_EndOfChain = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr, "EndOfChain");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr, 100669470);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr, 100669471);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 135469, XrefRangeEnd = 135471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TextMessageData(int sender, int messageID, string text, bool endOfChain)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&sender);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &messageID;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(text);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endOfChain;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135476, RefRangeEnd = 135477, XrefRangeStart = 135471, XrefRangeEnd = 135476, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TextMessageData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TextMessageData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TextMessageData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
