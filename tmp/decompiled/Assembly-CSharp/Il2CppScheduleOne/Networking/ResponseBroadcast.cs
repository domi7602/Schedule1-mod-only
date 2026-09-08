using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Networking;

[StructLayout(LayoutKind.Explicit)]
public struct ResponseBroadcast
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Passed;

	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.U1)]
	public bool Passed;

	static ResponseBroadcast()
	{
		Il2CppClassPointerStore<ResponseBroadcast>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Networking", "ResponseBroadcast");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ResponseBroadcast>.NativeClassPtr);
		NativeFieldInfoPtr_Passed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResponseBroadcast>.NativeClassPtr, "Passed");
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ResponseBroadcast>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
