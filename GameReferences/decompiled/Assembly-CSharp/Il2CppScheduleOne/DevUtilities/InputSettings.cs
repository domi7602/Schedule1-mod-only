using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class InputSettings : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EActionMode")]
	public enum EActionMode
	{
		Press,
		Hold
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MouseSensitivity;

	private static readonly System.IntPtr NativeFieldInfoPtr_InvertMouse;

	private static readonly System.IntPtr NativeFieldInfoPtr_SprintMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_BindingOverrides;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float MouseSensitivity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MouseSensitivity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MouseSensitivity)) = num;
		}
	}

	public unsafe bool InvertMouse
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InvertMouse);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InvertMouse)) = flag;
		}
	}

	public unsafe EActionMode SprintMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SprintMode);
			return *(EActionMode*)num;
		}
		set
		{
			*(EActionMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SprintMode)) = eActionMode;
		}
	}

	public unsafe string BindingOverrides
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BindingOverrides);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BindingOverrides)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static InputSettings()
	{
		Il2CppClassPointerStore<InputSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "InputSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InputSettings>.NativeClassPtr);
		NativeFieldInfoPtr_MouseSensitivity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSettings>.NativeClassPtr, "MouseSensitivity");
		NativeFieldInfoPtr_InvertMouse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSettings>.NativeClassPtr, "InvertMouse");
		NativeFieldInfoPtr_SprintMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSettings>.NativeClassPtr, "SprintMode");
		NativeFieldInfoPtr_BindingOverrides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSettings>.NativeClassPtr, "BindingOverrides");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputSettings>.NativeClassPtr, 100675253);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InputSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InputSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
