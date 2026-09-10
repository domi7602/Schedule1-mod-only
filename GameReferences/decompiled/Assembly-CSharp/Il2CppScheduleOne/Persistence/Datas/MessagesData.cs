using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

public class MessagesData : SaveData
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static MessagesData()
	{
		Il2CppClassPointerStore<MessagesData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MessagesData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MessagesData>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MessagesData>.NativeClassPtr, 100669365);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134345, RefRangeEnd = 134346, XrefRangeStart = 134345, XrefRangeEnd = 134346, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MessagesData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MessagesData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MessagesData(IntPtr pointer)
		: base(pointer)
	{
	}
}
