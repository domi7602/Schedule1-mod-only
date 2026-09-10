using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Weather;
using UnityEngine;

namespace Il2CppScheduleOne.Weather;

public class WeatherBasedObjectProvider : ScriptableObject
{
	[Serializable]
	[OriginalName("Assembly-CSharp.dll", "", "EvaluationType")]
	public enum EvaluationType
	{
		LessThan,
		Equals,
		GreaterThan,
		Blend
	}

	[OriginalName("Assembly-CSharp.dll", "", "ConditionFlags")]
	[Flags]
	public enum ConditionFlags
	{
		None = 0,
		Sunny = 1,
		Cloudy = 2,
		Rainy = 4,
		Stormy = 8,
		Snowy = 0x10,
		Foggy = 0x20,
		Windy = 0x40,
		Hail = 0x80,
		Sleet = 0x100
	}

	private static readonly IntPtr NativeFieldInfoPtr__selectedConditions;

	private static readonly IntPtr NativeFieldInfoPtr__conditions;

	private static readonly IntPtr NativeFieldInfoPtr__evaluationType;

	private static readonly IntPtr NativeFieldInfoPtr__object;

	private static readonly IntPtr NativeMethodInfoPtr_get_Object_Public_get_Object_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoesSatisfyConditions_Public_Boolean_WeatherConditions_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAverageBlend_Public_Single_WeatherConditions_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetConditionBlendValue_Private_Single_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_EvaluateConditions_Private_Boolean_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ConditionFlags _selectedConditions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectedConditions);
			return *(ConditionFlags*)num;
		}
		set
		{
			*(ConditionFlags*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectedConditions)) = conditionFlags;
		}
	}

	public unsafe WeatherConditions _conditions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__conditions);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WeatherConditions>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__conditions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)weatherConditions));
		}
	}

	public unsafe EvaluationType _evaluationType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__evaluationType);
			return *(EvaluationType*)num;
		}
		set
		{
			*(EvaluationType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__evaluationType)) = evaluationType;
		}
	}

	public unsafe UnityEngine.Object _object
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__object);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEngine.Object>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__object)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe UnityEngine.Object Object
	{
		[CallerCount(13)]
		[CachedScanResults(RefRangeStart = 2964, RefRangeEnd = 2977, XrefRangeStart = 2964, XrefRangeEnd = 2977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Object_Public_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEngine.Object>(intPtr) : null;
		}
	}

	static WeatherBasedObjectProvider()
	{
		Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Weather", "WeatherBasedObjectProvider");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr);
		NativeFieldInfoPtr__selectedConditions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, "_selectedConditions");
		NativeFieldInfoPtr__conditions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, "_conditions");
		NativeFieldInfoPtr__evaluationType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, "_evaluationType");
		NativeFieldInfoPtr__object = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, "_object");
		NativeMethodInfoPtr_get_Object_Public_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, 100685764);
		NativeMethodInfoPtr_DoesSatisfyConditions_Public_Boolean_WeatherConditions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, 100685765);
		NativeMethodInfoPtr_GetAverageBlend_Public_Single_WeatherConditions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, 100685766);
		NativeMethodInfoPtr_GetConditionBlendValue_Private_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, 100685767);
		NativeMethodInfoPtr_EvaluateConditions_Private_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, 100685768);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr, 100685769);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 293365, XrefRangeEnd = 293367, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool DoesSatisfyConditions(WeatherConditions activeConditions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeConditions);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoesSatisfyConditions_Public_Boolean_WeatherConditions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 293367, XrefRangeEnd = 293376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetAverageBlend(WeatherConditions activeConditions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeConditions);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAverageBlend_Public_Single_WeatherConditions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe float GetConditionBlendValue(float activeValue, float condition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&activeValue);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &condition;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConditionBlendValue_Private_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool EvaluateConditions(float conditionValue, float conditionThreshold)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&conditionValue);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &conditionThreshold;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvaluateConditions_Private_Boolean_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WeatherBasedObjectProvider()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WeatherBasedObjectProvider>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WeatherBasedObjectProvider(IntPtr pointer)
		: base(pointer)
	{
	}
}
