using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class MSGConversationData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_ConversationIndex;

	private static readonly IntPtr NativeFieldInfoPtr_Read;

	private static readonly IntPtr NativeFieldInfoPtr_MessageHistory;

	private static readonly IntPtr NativeFieldInfoPtr_ActiveResponses;

	private static readonly IntPtr NativeFieldInfoPtr_IsHidden;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Il2CppReferenceArray_1_TextMessageData_Il2CppReferenceArray_1_TextResponseData_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int ConversationIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConversationIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConversationIndex)) = num;
		}
	}

	public unsafe bool Read
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Read);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Read)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<TextMessageData> MessageHistory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MessageHistory);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<TextMessageData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MessageHistory)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<TextResponseData> ActiveResponses
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveResponses);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<TextResponseData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveResponses)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool IsHidden
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsHidden);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsHidden)) = flag;
		}
	}

	static MSGConversationData()
	{
		Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MSGConversationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr);
		NativeFieldInfoPtr_ConversationIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, "ConversationIndex");
		NativeFieldInfoPtr_Read = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, "Read");
		NativeFieldInfoPtr_MessageHistory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, "MessageHistory");
		NativeFieldInfoPtr_ActiveResponses = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, "ActiveResponses");
		NativeFieldInfoPtr_IsHidden = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, "IsHidden");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Il2CppReferenceArray_1_TextMessageData_Il2CppReferenceArray_1_TextResponseData_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, 100669405);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr, 100669406);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134992, RefRangeEnd = 134993, XrefRangeStart = 134989, XrefRangeEnd = 134992, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MSGConversationData(int conversationIndex, bool read, Il2CppReferenceArray<TextMessageData> messageHistory, Il2CppReferenceArray<TextResponseData> activeResponses, bool isHidden)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = (nint)(&conversationIndex);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &read;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)messageHistory);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeResponses);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &isHidden;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Boolean_Il2CppReferenceArray_1_TextMessageData_Il2CppReferenceArray_1_TextResponseData_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135002, RefRangeEnd = 135003, XrefRangeStart = 134993, XrefRangeEnd = 135002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MSGConversationData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MSGConversationData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MSGConversationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
