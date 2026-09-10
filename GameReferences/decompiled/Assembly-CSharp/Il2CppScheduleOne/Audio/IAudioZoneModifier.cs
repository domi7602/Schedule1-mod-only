using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Audio;

public class IAudioZoneModifier : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_VolumeMultiplier_Public_Abstract_Virtual_New_get_Single_0;

	public unsafe virtual float VolumeMultiplier
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_VolumeMultiplier_Public_Abstract_Virtual_New_get_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static IAudioZoneModifier()
	{
		Il2CppClassPointerStore<IAudioZoneModifier>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "IAudioZoneModifier");
		NativeMethodInfoPtr_get_VolumeMultiplier_Public_Abstract_Virtual_New_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IAudioZoneModifier>.NativeClassPtr, 100676808);
	}

	public IAudioZoneModifier(IntPtr pointer)
		: base(pointer)
	{
	}
}
