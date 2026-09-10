using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Gamepad;

public class IHapticsManager : Il2CppObjectBase
{
	private static readonly IntPtr NativeFieldInfoPtr_PRESET_ACTION_PUNCH;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_IMPACT_PUNCH;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_ACTION_STAB;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_IMPACT_STAB;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_ACTION_BAT;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_IMPACT_BAT;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_ACTION_BULLET;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_IMPACT_BULLET;

	private static readonly IntPtr NativeFieldInfoPtr_PRESET_EXPLOSION;

	private static readonly IntPtr NativeMethodInfoPtr_Begin_Public_Abstract_Virtual_New_Void_String_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Begin_Public_Abstract_Virtual_New_Void_HapticsData_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_End_Public_Abstract_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Cancel_Public_Abstract_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMultiplier_Public_Abstract_Virtual_New_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ForceToMultiplier_Public_Abstract_Virtual_New_Single_EHapticImpact_Single_0;

	public unsafe static string PRESET_ACTION_PUNCH
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_ACTION_PUNCH, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_ACTION_PUNCH, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_IMPACT_PUNCH
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_IMPACT_PUNCH, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_IMPACT_PUNCH, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_ACTION_STAB
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_ACTION_STAB, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_ACTION_STAB, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_IMPACT_STAB
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_IMPACT_STAB, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_IMPACT_STAB, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_ACTION_BAT
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_ACTION_BAT, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_ACTION_BAT, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_IMPACT_BAT
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_IMPACT_BAT, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_IMPACT_BAT, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_ACTION_BULLET
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_ACTION_BULLET, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_ACTION_BULLET, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_IMPACT_BULLET
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_IMPACT_BULLET, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_IMPACT_BULLET, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string PRESET_EXPLOSION
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PRESET_EXPLOSION, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PRESET_EXPLOSION, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static IHapticsManager()
	{
		Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "IHapticsManager");
		NativeFieldInfoPtr_PRESET_ACTION_PUNCH = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_ACTION_PUNCH");
		NativeFieldInfoPtr_PRESET_IMPACT_PUNCH = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_IMPACT_PUNCH");
		NativeFieldInfoPtr_PRESET_ACTION_STAB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_ACTION_STAB");
		NativeFieldInfoPtr_PRESET_IMPACT_STAB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_IMPACT_STAB");
		NativeFieldInfoPtr_PRESET_ACTION_BAT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_ACTION_BAT");
		NativeFieldInfoPtr_PRESET_IMPACT_BAT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_IMPACT_BAT");
		NativeFieldInfoPtr_PRESET_ACTION_BULLET = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_ACTION_BULLET");
		NativeFieldInfoPtr_PRESET_IMPACT_BULLET = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_IMPACT_BULLET");
		NativeFieldInfoPtr_PRESET_EXPLOSION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, "PRESET_EXPLOSION");
		NativeMethodInfoPtr_Begin_Public_Abstract_Virtual_New_Void_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, 100685953);
		NativeMethodInfoPtr_Begin_Public_Abstract_Virtual_New_Void_HapticsData_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, 100685954);
		NativeMethodInfoPtr_End_Public_Abstract_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, 100685955);
		NativeMethodInfoPtr_Cancel_Public_Abstract_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, 100685956);
		NativeMethodInfoPtr_SetMultiplier_Public_Abstract_Virtual_New_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, 100685957);
		NativeMethodInfoPtr_ForceToMultiplier_Public_Abstract_Virtual_New_Single_EHapticImpact_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHapticsManager>.NativeClassPtr, 100685958);
	}

	[CallerCount(0)]
	public unsafe virtual void Begin(string preset, float intensityMultiplier = 1f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(preset);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &intensityMultiplier;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Begin_Public_Abstract_Virtual_New_Void_String_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void Begin(HapticsData data, float intensityMultiplier = 1f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &intensityMultiplier;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Begin_Public_Abstract_Virtual_New_Void_HapticsData_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void End()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_End_Public_Abstract_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void Cancel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Cancel_Public_Abstract_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void SetMultiplier(float multiplier)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&multiplier);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetMultiplier_Public_Abstract_Virtual_New_Void_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual float ForceToMultiplier(EHapticImpact impact, float force)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&impact);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &force;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ForceToMultiplier_Public_Abstract_Virtual_New_Single_EHapticImpact_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public IHapticsManager(IntPtr pointer)
		: base(pointer)
	{
	}
}
