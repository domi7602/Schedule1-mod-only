using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.UI.Phone.Messages;
using Il2CppSystem;

namespace Il2CppScheduleOne.Dialogue;

[System.Serializable]
public class DialogueChain : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Lines;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMessageChain_Public_MessageChain_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppStringArray Lines
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Lines);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Lines)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static DialogueChain()
	{
		Il2CppClassPointerStore<DialogueChain>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Dialogue", "DialogueChain");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DialogueChain>.NativeClassPtr);
		NativeFieldInfoPtr_Lines = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueChain>.NativeClassPtr, "Lines");
		NativeMethodInfoPtr_GetMessageChain_Public_MessageChain_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DialogueChain>.NativeClassPtr, 100674724);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DialogueChain>.NativeClassPtr, 100674725);
	}

	[CallerCount(25)]
	[CachedScanResults(RefRangeStart = 191551, RefRangeEnd = 191576, XrefRangeStart = 191543, XrefRangeEnd = 191551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MessageChain GetMessageChain()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMessageChain_Public_MessageChain_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MessageChain>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DialogueChain()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DialogueChain>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DialogueChain(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
