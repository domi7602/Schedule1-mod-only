using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.State;

public static class StatePropertiesTransitionHandler : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__currentProperties;

	private static readonly System.IntPtr NativeMethodInfoPtr_Transition_Public_Static_Void_StateProperties_0;

	public unsafe static StateProperties _currentProperties
	{
		get
		{
			Unsafe.SkipInit(out StateProperties result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__currentProperties, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__currentProperties, (void*)(&stateProperties));
		}
	}

	static StatePropertiesTransitionHandler()
	{
		Il2CppClassPointerStore<StatePropertiesTransitionHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.State", "StatePropertiesTransitionHandler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StatePropertiesTransitionHandler>.NativeClassPtr);
		NativeFieldInfoPtr__currentProperties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StatePropertiesTransitionHandler>.NativeClassPtr, "_currentProperties");
		NativeMethodInfoPtr_Transition_Public_Static_Void_StateProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StatePropertiesTransitionHandler>.NativeClassPtr, 100667125);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 103935, RefRangeEnd = 103937, XrefRangeStart = 103926, XrefRangeEnd = 103935, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Transition(StateProperties newProperties)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&newProperties);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Transition_Public_Static_Void_StateProperties_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StatePropertiesTransitionHandler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
