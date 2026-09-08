using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Networking;

public sealed class HostPasswordBroadcast : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Password;

	public unsafe string Password
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Password);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Password)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static HostPasswordBroadcast()
	{
		Il2CppClassPointerStore<HostPasswordBroadcast>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Networking", "HostPasswordBroadcast");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HostPasswordBroadcast>.NativeClassPtr);
		NativeFieldInfoPtr_Password = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HostPasswordBroadcast>.NativeClassPtr, "Password");
	}

	public HostPasswordBroadcast(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public HostPasswordBroadcast()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HostPasswordBroadcast>.NativeClassPtr))
	{
	}
}
