using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Quests;

[System.Serializable]
public class QuestWindowConfig : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_IsEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_WindowStartTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_WindowEndTime;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool IsEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsEnabled)) = flag;
		}
	}

	public unsafe int WindowStartTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WindowStartTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WindowStartTime)) = num;
		}
	}

	public unsafe int WindowEndTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WindowEndTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WindowEndTime)) = num;
		}
	}

	static QuestWindowConfig()
	{
		Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "QuestWindowConfig");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr);
		NativeFieldInfoPtr_IsEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr, "IsEnabled");
		NativeFieldInfoPtr_WindowStartTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr, "WindowStartTime");
		NativeFieldInfoPtr_WindowEndTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr, "WindowEndTime");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr, 100667742);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 111672, RefRangeEnd = 111679, XrefRangeStart = 111671, XrefRangeEnd = 111672, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QuestWindowConfig()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QuestWindowConfig>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QuestWindowConfig(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
