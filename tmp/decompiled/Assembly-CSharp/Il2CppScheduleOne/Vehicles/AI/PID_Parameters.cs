using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vehicles.AI;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct PID_Parameters
{
	private static readonly System.IntPtr NativeFieldInfoPtr_P;

	private static readonly System.IntPtr NativeFieldInfoPtr_I;

	private static readonly System.IntPtr NativeFieldInfoPtr_D;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0;

	[FieldOffset(0)]
	public float P;

	[FieldOffset(4)]
	public float I;

	[FieldOffset(8)]
	public float D;

	static PID_Parameters()
	{
		Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "PID_Parameters");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr);
		NativeFieldInfoPtr_P = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr, "P");
		NativeFieldInfoPtr_I = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr, "I");
		NativeFieldInfoPtr_D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr, "D");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr, 100666439);
	}

	[CallerCount(0)]
	public unsafe PID_Parameters(float P, float I, float D)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&P);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &I;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &D;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<PID_Parameters>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
