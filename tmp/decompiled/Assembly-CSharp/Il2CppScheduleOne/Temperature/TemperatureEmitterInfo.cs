using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Temperature;

[StructLayout(LayoutKind.Explicit)]
public struct TemperatureEmitterInfo
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Temperature;

	private static readonly System.IntPtr NativeFieldInfoPtr_SqrRange;

	private static readonly System.IntPtr NativeFieldInfoPtr_Position;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SizeOf_Public_Static_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Vector3_0;

	[FieldOffset(0)]
	public float Temperature;

	[FieldOffset(4)]
	public float SqrRange;

	[FieldOffset(8)]
	public Vector3 Position;

	public unsafe static int SizeOf
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 102411, RefRangeEnd = 102421, XrefRangeStart = 102411, XrefRangeEnd = 102411, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SizeOf_Public_Static_get_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static TemperatureEmitterInfo()
	{
		Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Temperature", "TemperatureEmitterInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr);
		NativeFieldInfoPtr_Temperature = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr, "Temperature");
		NativeFieldInfoPtr_SqrRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr, "SqrRange");
		NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr, "Position");
		NativeMethodInfoPtr_get_SizeOf_Public_Static_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr, 100666985);
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr, 100666986);
	}

	[CallerCount(0)]
	public unsafe TemperatureEmitterInfo(float temperature, float sqrRange, Vector3 position)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&temperature);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &sqrRange;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Vector3_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<TemperatureEmitterInfo>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
