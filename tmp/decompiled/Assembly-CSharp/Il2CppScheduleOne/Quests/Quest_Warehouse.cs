using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Quests;

public class Quest_Warehouse : Quest
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static Quest_Warehouse()
	{
		Il2CppClassPointerStore<Quest_Warehouse>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "Quest_Warehouse");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Quest_Warehouse>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_Warehouse>.NativeClassPtr, 100667731);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 111578, XrefRangeEnd = 111582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Quest_Warehouse()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Quest_Warehouse>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Quest_Warehouse(IntPtr pointer)
		: base(pointer)
	{
	}
}
