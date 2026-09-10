using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.AvatarFramework;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class AvatarAppearanceData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_AvatarSettings;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_AvatarSettings_0;

	public unsafe AvatarSettings AvatarSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvatarSettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AvatarSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvatarSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatarSettings));
		}
	}

	static AvatarAppearanceData()
	{
		Il2CppClassPointerStore<AvatarAppearanceData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "AvatarAppearanceData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AvatarAppearanceData>.NativeClassPtr);
		NativeFieldInfoPtr_AvatarSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarAppearanceData>.NativeClassPtr, "AvatarSettings");
		NativeMethodInfoPtr__ctor_Public_Void_AvatarSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarAppearanceData>.NativeClassPtr, 100669313);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 134314, RefRangeEnd = 134323, XrefRangeStart = 134312, XrefRangeEnd = 134314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AvatarAppearanceData(AvatarSettings avatarSettings)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AvatarAppearanceData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatarSettings);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_AvatarSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AvatarAppearanceData(IntPtr pointer)
		: base(pointer)
	{
	}
}
