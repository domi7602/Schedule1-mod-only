using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.DevUtilities;

public class CoroutineService : PersistentSingleton<CoroutineService>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static CoroutineService()
	{
		Il2CppClassPointerStore<CoroutineService>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "CoroutineService");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CoroutineService>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CoroutineService>.NativeClassPtr, 100675006);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194048, XrefRangeEnd = 194051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CoroutineService()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CoroutineService>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CoroutineService(IntPtr pointer)
		: base(pointer)
	{
	}
}
