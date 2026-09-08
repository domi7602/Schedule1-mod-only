using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Networking;

[StructLayout(LayoutKind.Explicit)]
public struct CheckIfUserIsFriendBroadcast
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SteamId;

	[FieldOffset(0)]
	public ulong SteamId;

	static CheckIfUserIsFriendBroadcast()
	{
		Il2CppClassPointerStore<CheckIfUserIsFriendBroadcast>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Networking", "CheckIfUserIsFriendBroadcast");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CheckIfUserIsFriendBroadcast>.NativeClassPtr);
		NativeFieldInfoPtr_SteamId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CheckIfUserIsFriendBroadcast>.NativeClassPtr, "SteamId");
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<CheckIfUserIsFriendBroadcast>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
