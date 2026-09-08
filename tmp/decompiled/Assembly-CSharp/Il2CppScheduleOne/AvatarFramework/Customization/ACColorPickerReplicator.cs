using System;
using System.Runtime.CompilerServices;
using Il2CppHSVPicker;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.AvatarFramework.Customization;

public class ACColorPickerReplicator : ACReplicator
{
	private static readonly IntPtr NativeFieldInfoPtr_picker;

	private static readonly IntPtr NativeMethodInfoPtr_AvatarSettingsChanged_Protected_Virtual_Void_AvatarSettings_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ColorPicker picker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_picker);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ColorPicker>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_picker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)colorPicker));
		}
	}

	static ACColorPickerReplicator()
	{
		Il2CppClassPointerStore<ACColorPickerReplicator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Customization", "ACColorPickerReplicator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ACColorPickerReplicator>.NativeClassPtr);
		NativeFieldInfoPtr_picker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ACColorPickerReplicator>.NativeClassPtr, "picker");
		NativeMethodInfoPtr_AvatarSettingsChanged_Protected_Virtual_Void_AvatarSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ACColorPickerReplicator>.NativeClassPtr, 100677607);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ACColorPickerReplicator>.NativeClassPtr, 100677608);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 222217, XrefRangeEnd = 222222, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void AvatarSettingsChanged(AvatarSettings newSettings)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newSettings);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_AvatarSettingsChanged_Protected_Virtual_Void_AvatarSettings_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 222226, RefRangeEnd = 222227, XrefRangeStart = 222222, XrefRangeEnd = 222226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ACColorPickerReplicator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ACColorPickerReplicator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ACColorPickerReplicator(IntPtr pointer)
		: base(pointer)
	{
	}
}
