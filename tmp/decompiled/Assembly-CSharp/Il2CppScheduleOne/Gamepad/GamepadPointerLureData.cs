using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Gamepad;

public class GamepadPointerLureData : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_Id;

	private static readonly IntPtr NativeFieldInfoPtr_Radius;

	private static readonly IntPtr NativeFieldInfoPtr_InteractionRadius;

	private static readonly IntPtr NativeFieldInfoPtr_Strength;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string Id
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Id);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Id)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float Radius
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Radius);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Radius)) = num;
		}
	}

	public unsafe float InteractionRadius
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InteractionRadius);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InteractionRadius)) = num;
		}
	}

	public unsafe float Strength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Strength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Strength)) = num;
		}
	}

	static GamepadPointerLureData()
	{
		Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "GamepadPointerLureData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr);
		NativeFieldInfoPtr_Id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr, "Id");
		NativeFieldInfoPtr_Radius = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr, "Radius");
		NativeFieldInfoPtr_InteractionRadius = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr, "InteractionRadius");
		NativeFieldInfoPtr_Strength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr, "Strength");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr, 100685921);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 294617, XrefRangeEnd = 294618, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GamepadPointerLureData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GamepadPointerLureData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GamepadPointerLureData(IntPtr pointer)
		: base(pointer)
	{
	}
}
