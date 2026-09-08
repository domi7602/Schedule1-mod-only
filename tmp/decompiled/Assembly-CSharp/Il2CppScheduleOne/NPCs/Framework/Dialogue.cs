using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Dialogue;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Dialogue : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DialogueDatabase;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Dialogue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe DialogueDatabase DialogueDatabase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DialogueDatabase);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DialogueDatabase>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DialogueDatabase)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dialogueDatabase));
		}
	}

	static Dialogue()
	{
		Il2CppClassPointerStore<Dialogue>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Dialogue");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Dialogue>.NativeClassPtr);
		NativeFieldInfoPtr_DialogueDatabase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Dialogue>.NativeClassPtr, "DialogueDatabase");
		NativeMethodInfoPtr_GetCopy_Public_Dialogue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dialogue>.NativeClassPtr, 100682805);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dialogue>.NativeClassPtr, 100682806);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272232, XrefRangeEnd = 272237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dialogue GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Dialogue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dialogue>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dialogue()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Dialogue>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Dialogue(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
