using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Dialogue;

[System.Serializable]
public class DialogueChoiceData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Guid;

	private static readonly System.IntPtr NativeFieldInfoPtr_ChoiceText;

	private static readonly System.IntPtr NativeFieldInfoPtr_ChoiceLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr_ShowWorldspaceDialogue;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_DialogueChoiceData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string Guid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Guid);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Guid)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string ChoiceText
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChoiceText);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChoiceText)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string ChoiceLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChoiceLabel);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChoiceLabel)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool ShowWorldspaceDialogue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowWorldspaceDialogue);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowWorldspaceDialogue)) = flag;
		}
	}

	static DialogueChoiceData()
	{
		Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Dialogue", "DialogueChoiceData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr);
		NativeFieldInfoPtr_Guid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr, "Guid");
		NativeFieldInfoPtr_ChoiceText = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr, "ChoiceText");
		NativeFieldInfoPtr_ChoiceLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr, "ChoiceLabel");
		NativeFieldInfoPtr_ShowWorldspaceDialogue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr, "ShowWorldspaceDialogue");
		NativeMethodInfoPtr_GetCopy_Public_DialogueChoiceData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr, 100674853);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr, 100674854);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 192703, RefRangeEnd = 192704, XrefRangeStart = 192696, XrefRangeEnd = 192703, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DialogueChoiceData GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_DialogueChoiceData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DialogueChoiceData>(intPtr) : null;
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 192705, RefRangeEnd = 192713, XrefRangeStart = 192704, XrefRangeEnd = 192705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DialogueChoiceData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DialogueChoiceData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DialogueChoiceData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
