using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vehicles.AI;

public class SteerPID : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_error_old;

	private static readonly System.IntPtr NativeFieldInfoPtr_error_sum;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNewValue_Public_Single_Single_PID_Parameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddValueToAverage_Public_Static_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float error_old
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_error_old);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_error_old)) = num;
		}
	}

	public unsafe float error_sum
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_error_sum);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_error_sum)) = num;
		}
	}

	static SteerPID()
	{
		Il2CppClassPointerStore<SteerPID>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "SteerPID");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SteerPID>.NativeClassPtr);
		NativeFieldInfoPtr_error_old = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SteerPID>.NativeClassPtr, "error_old");
		NativeFieldInfoPtr_error_sum = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SteerPID>.NativeClassPtr, "error_sum");
		NativeMethodInfoPtr_GetNewValue_Public_Single_Single_PID_Parameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SteerPID>.NativeClassPtr, 100666436);
		NativeMethodInfoPtr_AddValueToAverage_Public_Static_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SteerPID>.NativeClassPtr, 100666437);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SteerPID>.NativeClassPtr, 100666438);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95925, XrefRangeEnd = 95927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNewValue(float error, PID_Parameters pid_parameters)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&error);
		*(PID_Parameters**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pid_parameters;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNewValue_Public_Single_Single_PID_Parameters_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float AddValueToAverage(float oldAverage, float valueToAdd, float count)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&oldAverage);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &valueToAdd;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &count;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddValueToAverage_Public_Static_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SteerPID()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SteerPID>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SteerPID(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
