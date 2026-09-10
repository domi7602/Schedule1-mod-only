using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Dialogue;

[System.Serializable]
public sealed class Entry : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Key;

	private static readonly System.IntPtr NativeFieldInfoPtr_Chains;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRandomChain_Public_DialogueChain_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRandomLine_Public_String_0;

	public unsafe string Key
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Key);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Key)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppReferenceArray<DialogueChain> Chains
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Chains);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<DialogueChain>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Chains)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static Entry()
	{
		Il2CppClassPointerStore<Entry>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Dialogue", "Entry");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Entry>.NativeClassPtr);
		NativeFieldInfoPtr_Key = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Entry>.NativeClassPtr, "Key");
		NativeFieldInfoPtr_Chains = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Entry>.NativeClassPtr, "Chains");
		NativeMethodInfoPtr_GetRandomChain_Public_DialogueChain_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Entry>.NativeClassPtr, 100674736);
		NativeMethodInfoPtr_GetRandomLine_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Entry>.NativeClassPtr, 100674737);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 191743, RefRangeEnd = 191744, XrefRangeStart = 191742, XrefRangeEnd = 191743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DialogueChain GetRandomChain()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRandomChain_Public_DialogueChain_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DialogueChain>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 191745, RefRangeEnd = 191746, XrefRangeStart = 191744, XrefRangeEnd = 191745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetRandomLine()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRandomLine_Public_String_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public Entry(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public Entry()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Entry>.NativeClassPtr))
	{
	}
}
