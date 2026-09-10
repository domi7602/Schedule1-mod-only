using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class WeatherBehaviour : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_UseUmbrellaChance;

	private static readonly System.IntPtr NativeFieldInfoPtr_RainTolerance;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxWalkSpeedInRainMultiplier;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_WeatherBehaviour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float UseUmbrellaChance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseUmbrellaChance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseUmbrellaChance)) = num;
		}
	}

	public unsafe float RainTolerance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RainTolerance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RainTolerance)) = num;
		}
	}

	public unsafe float MaxWalkSpeedInRainMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxWalkSpeedInRainMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxWalkSpeedInRainMultiplier)) = num;
		}
	}

	static WeatherBehaviour()
	{
		Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "WeatherBehaviour");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr);
		NativeFieldInfoPtr_UseUmbrellaChance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr, "UseUmbrellaChance");
		NativeFieldInfoPtr_RainTolerance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr, "RainTolerance");
		NativeFieldInfoPtr_MaxWalkSpeedInRainMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr, "MaxWalkSpeedInRainMultiplier");
		NativeMethodInfoPtr_GetCopy_Public_WeatherBehaviour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr, 100682838);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr, 100682839);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272317, XrefRangeEnd = 272321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WeatherBehaviour GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_WeatherBehaviour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<WeatherBehaviour>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272321, XrefRangeEnd = 272322, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WeatherBehaviour()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WeatherBehaviour>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WeatherBehaviour(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
