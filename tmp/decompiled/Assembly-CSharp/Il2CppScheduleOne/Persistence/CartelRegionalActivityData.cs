using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Map;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence;

[System.Serializable]
public class CartelRegionalActivityData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Region;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentActivityIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoursUntilNextActivity;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_EMapRegion_Int32_Int32_0;

	public unsafe EMapRegion Region
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Region);
			return *(EMapRegion*)num;
		}
		set
		{
			*(EMapRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Region)) = eMapRegion;
		}
	}

	public unsafe int CurrentActivityIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentActivityIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentActivityIndex)) = num;
		}
	}

	public unsafe int HoursUntilNextActivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursUntilNextActivity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursUntilNextActivity)) = num;
		}
	}

	static CartelRegionalActivityData()
	{
		Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence", "CartelRegionalActivityData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr);
		NativeFieldInfoPtr_Region = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr, "Region");
		NativeFieldInfoPtr_CurrentActivityIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr, "CurrentActivityIndex");
		NativeFieldInfoPtr_HoursUntilNextActivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr, "HoursUntilNextActivity");
		NativeMethodInfoPtr__ctor_Public_Void_EMapRegion_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr, 100668910);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 127277, RefRangeEnd = 127283, XrefRangeStart = 127276, XrefRangeEnd = 127277, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CartelRegionalActivityData(EMapRegion region, int currentActivityIndex, int hoursUntilNextActivity)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CartelRegionalActivityData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&region);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentActivityIndex;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &hoursUntilNextActivity;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_EMapRegion_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CartelRegionalActivityData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
