using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Schedules;

public class NPCActionOrderByDescending : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Compare_Public_Virtual_Final_New_Int32_NPCAction_NPCAction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static NPCActionOrderByDescending()
	{
		Il2CppClassPointerStore<NPCActionOrderByDescending>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Schedules", "NPCActionOrderByDescending");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NPCActionOrderByDescending>.NativeClassPtr);
		NativeMethodInfoPtr_Compare_Public_Virtual_Final_New_Int32_NPCAction_NPCAction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCActionOrderByDescending>.NativeClassPtr, 100684869);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCActionOrderByDescending>.NativeClassPtr, 100684870);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 286971, XrefRangeEnd = 286972, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual int Compare(NPCAction x, NPCAction y)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)y);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Compare_Public_Virtual_Final_New_Int32_NPCAction_NPCAction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NPCActionOrderByDescending()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NPCActionOrderByDescending>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NPCActionOrderByDescending(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
