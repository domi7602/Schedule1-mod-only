using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Effects;

public static class EffectReferences : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_PARTICLE_SYSTEM_EMISSION_RATE_OVER_TIME;

	public unsafe static string PARTICLE_SYSTEM_EMISSION_RATE_OVER_TIME
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PARTICLE_SYSTEM_EMISSION_RATE_OVER_TIME, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PARTICLE_SYSTEM_EMISSION_RATE_OVER_TIME, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static EffectReferences()
	{
		Il2CppClassPointerStore<EffectReferences>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Effects", "EffectReferences");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EffectReferences>.NativeClassPtr);
		NativeFieldInfoPtr_PARTICLE_SYSTEM_EMISSION_RATE_OVER_TIME = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectReferences>.NativeClassPtr, "PARTICLE_SYSTEM_EMISSION_RATE_OVER_TIME");
	}

	public EffectReferences(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
