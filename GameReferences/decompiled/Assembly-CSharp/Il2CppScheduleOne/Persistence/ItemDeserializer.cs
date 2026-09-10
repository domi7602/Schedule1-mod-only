using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence;

public static class ItemDeserializer : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_LoadItem_Public_Static_ItemInstance_String_0;

	static ItemDeserializer()
	{
		Il2CppClassPointerStore<ItemDeserializer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence", "ItemDeserializer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemDeserializer>.NativeClassPtr);
		NativeMethodInfoPtr_LoadItem_Public_Static_ItemInstance_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemDeserializer>.NativeClassPtr, 100668781);
	}

	[CallerCount(34)]
	[CachedScanResults(RefRangeStart = 124949, RefRangeEnd = 124983, XrefRangeStart = 124913, XrefRangeEnd = 124949, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ItemInstance LoadItem(string itemString)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(itemString);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadItem_Public_Static_ItemInstance_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	public ItemDeserializer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
