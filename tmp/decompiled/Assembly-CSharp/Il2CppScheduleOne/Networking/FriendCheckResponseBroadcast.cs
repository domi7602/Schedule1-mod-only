using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Networking;

[StructLayout(LayoutKind.Explicit)]
public struct FriendCheckResponseBroadcast
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SenderSteamId;

	private static readonly System.IntPtr NativeFieldInfoPtr_TargetSteamId;

	private static readonly System.IntPtr NativeFieldInfoPtr_IsFriend;

	[FieldOffset(0)]
	public ulong SenderSteamId;

	[FieldOffset(8)]
	public ulong TargetSteamId;

	[FieldOffset(16)]
	[MarshalAs(UnmanagedType.U1)]
	public bool IsFriend;

	static FriendCheckResponseBroadcast()
	{
		Il2CppClassPointerStore<FriendCheckResponseBroadcast>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Networking", "FriendCheckResponseBroadcast");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FriendCheckResponseBroadcast>.NativeClassPtr);
		NativeFieldInfoPtr_SenderSteamId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FriendCheckResponseBroadcast>.NativeClassPtr, "SenderSteamId");
		NativeFieldInfoPtr_TargetSteamId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FriendCheckResponseBroadcast>.NativeClassPtr, "TargetSteamId");
		NativeFieldInfoPtr_IsFriend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FriendCheckResponseBroadcast>.NativeClassPtr, "IsFriend");
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<FriendCheckResponseBroadcast>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
