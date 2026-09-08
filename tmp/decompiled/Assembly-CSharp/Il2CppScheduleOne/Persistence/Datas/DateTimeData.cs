using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class DateTimeData : SaveData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Year;

	private static readonly System.IntPtr NativeFieldInfoPtr_Month;

	private static readonly System.IntPtr NativeFieldInfoPtr_Day;

	private static readonly System.IntPtr NativeFieldInfoPtr_Hour;

	private static readonly System.IntPtr NativeFieldInfoPtr_Minute;

	private static readonly System.IntPtr NativeFieldInfoPtr_Second;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_DateTime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDateTime_Public_DateTime_0;

	public unsafe int Year
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Year);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Year)) = num;
		}
	}

	public unsafe int Month
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Month);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Month)) = num;
		}
	}

	public unsafe int Day
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Day);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Day)) = num;
		}
	}

	public unsafe int Hour
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hour);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hour)) = num;
		}
	}

	public unsafe int Minute
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Minute);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Minute)) = num;
		}
	}

	public unsafe int Second
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Second);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Second)) = num;
		}
	}

	static DateTimeData()
	{
		Il2CppClassPointerStore<DateTimeData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "DateTimeData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr);
		NativeFieldInfoPtr_Year = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, "Year");
		NativeFieldInfoPtr_Month = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, "Month");
		NativeFieldInfoPtr_Day = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, "Day");
		NativeFieldInfoPtr_Hour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, "Hour");
		NativeFieldInfoPtr_Minute = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, "Minute");
		NativeFieldInfoPtr_Second = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, "Second");
		NativeMethodInfoPtr__ctor_Public_Void_DateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, 100669319);
		NativeMethodInfoPtr_GetDateTime_Public_DateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr, 100669320);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 134356, RefRangeEnd = 134362, XrefRangeStart = 134346, XrefRangeEnd = 134356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DateTimeData(Il2CppSystem.DateTime date)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DateTimeData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&date);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_DateTime_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 134363, RefRangeEnd = 134366, XrefRangeStart = 134362, XrefRangeEnd = 134363, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppSystem.DateTime GetDateTime()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDateTime_Public_DateTime_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Il2CppSystem.DateTime*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public DateTimeData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
