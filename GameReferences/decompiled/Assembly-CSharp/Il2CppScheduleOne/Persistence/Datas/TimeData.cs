using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class TimeData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_TimeOfDay;

	private static readonly IntPtr NativeFieldInfoPtr_ElapsedDays;

	private static readonly IntPtr NativeFieldInfoPtr_Playtime;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Int32_0;

	public unsafe int TimeOfDay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeOfDay);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TimeOfDay)) = num;
		}
	}

	public unsafe int ElapsedDays
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ElapsedDays);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ElapsedDays)) = num;
		}
	}

	public unsafe int Playtime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Playtime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Playtime)) = num;
		}
	}

	static TimeData()
	{
		Il2CppClassPointerStore<TimeData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "TimeData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TimeData>.NativeClassPtr);
		NativeFieldInfoPtr_TimeOfDay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeData>.NativeClassPtr, "TimeOfDay");
		NativeFieldInfoPtr_ElapsedDays = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeData>.NativeClassPtr, "ElapsedDays");
		NativeFieldInfoPtr_Playtime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeData>.NativeClassPtr, "Playtime");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TimeData>.NativeClassPtr, 100669474);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135487, RefRangeEnd = 135488, XrefRangeStart = 135486, XrefRangeEnd = 135487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TimeData(int timeOfDay, int elapsedDays, int playtime)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TimeData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&timeOfDay);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &elapsedDays;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &playtime;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TimeData(IntPtr pointer)
		: base(pointer)
	{
	}
}
