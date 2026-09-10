using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.GameTime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class GameDateTimeData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_ElapsedDays;

	private static readonly IntPtr NativeFieldInfoPtr_Time;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_GameDateTime_0;

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

	public unsafe int Time
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Time);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Time)) = num;
		}
	}

	static GameDateTimeData()
	{
		Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "GameDateTimeData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr);
		NativeFieldInfoPtr_ElapsedDays = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr, "ElapsedDays");
		NativeFieldInfoPtr_Time = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr, "Time");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr, 100669335);
		NativeMethodInfoPtr__ctor_Public_Void_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr, 100669336);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 134592, XrefRangeEnd = 134593, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTimeData(int _elapsedDays, int _time)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&_elapsedDays);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &_time;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 134594, RefRangeEnd = 134598, XrefRangeStart = 134593, XrefRangeEnd = 134594, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameDateTimeData(GameDateTime gameDateTime)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameDateTimeData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&gameDateTime);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_GameDateTime_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GameDateTimeData(IntPtr pointer)
		: base(pointer)
	{
	}
}
