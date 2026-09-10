using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Quests;

public class Quest_ExpandingOperations : Quest
{
	private static readonly IntPtr NativeFieldInfoPtr_SetUpGrowTentsEntry;

	private static readonly IntPtr NativeFieldInfoPtr_ReachCustomersEntry;

	private static readonly IntPtr NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe QuestEntry SetUpGrowTentsEntry
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetUpGrowTentsEntry);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QuestEntry>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetUpGrowTentsEntry)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)questEntry));
		}
	}

	public unsafe QuestEntry ReachCustomersEntry
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReachCustomersEntry);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QuestEntry>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReachCustomersEntry)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)questEntry));
		}
	}

	static Quest_ExpandingOperations()
	{
		Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "Quest_ExpandingOperations");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr);
		NativeFieldInfoPtr_SetUpGrowTentsEntry = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr, "SetUpGrowTentsEntry");
		NativeFieldInfoPtr_ReachCustomersEntry = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr, "ReachCustomersEntry");
		NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr, 100667662);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr, 100667663);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 110875, XrefRangeEnd = 110908, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnUncappedMinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnUncappedMinPass_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 110908, XrefRangeEnd = 110912, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Quest_ExpandingOperations()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Quest_ExpandingOperations>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Quest_ExpandingOperations(IntPtr pointer)
		: base(pointer)
	{
	}
}
