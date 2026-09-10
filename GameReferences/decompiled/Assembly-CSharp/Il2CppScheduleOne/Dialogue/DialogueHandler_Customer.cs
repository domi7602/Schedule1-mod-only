using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Dialogue;

public class DialogueHandler_Customer : DialogueHandler
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static DialogueHandler_Customer()
	{
		Il2CppClassPointerStore<DialogueHandler_Customer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Dialogue", "DialogueHandler_Customer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DialogueHandler_Customer>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DialogueHandler_Customer>.NativeClassPtr, 100674801);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 190797, RefRangeEnd = 190799, XrefRangeStart = 190797, XrefRangeEnd = 190799, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DialogueHandler_Customer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DialogueHandler_Customer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DialogueHandler_Customer(IntPtr pointer)
		: base(pointer)
	{
	}
}
