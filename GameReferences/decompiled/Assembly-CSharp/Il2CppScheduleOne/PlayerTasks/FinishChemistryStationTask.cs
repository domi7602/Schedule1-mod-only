using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.PlayerTasks;

public class FinishChemistryStationTask : Task
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static FinishChemistryStationTask()
	{
		Il2CppClassPointerStore<FinishChemistryStationTask>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerTasks", "FinishChemistryStationTask");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FinishChemistryStationTask>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FinishChemistryStationTask>.NativeClassPtr, 100668344);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 119054, XrefRangeEnd = 119055, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FinishChemistryStationTask()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FinishChemistryStationTask>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FinishChemistryStationTask(IntPtr pointer)
		: base(pointer)
	{
	}
}
