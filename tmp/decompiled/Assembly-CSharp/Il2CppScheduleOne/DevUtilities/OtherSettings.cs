using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class OtherSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_AutoBackupSaves;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool AutoBackupSaves
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoBackupSaves);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoBackupSaves)) = flag;
		}
	}

	static OtherSettings()
	{
		Il2CppClassPointerStore<OtherSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "OtherSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OtherSettings>.NativeClassPtr);
		NativeFieldInfoPtr_AutoBackupSaves = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OtherSettings>.NativeClassPtr, "AutoBackupSaves");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OtherSettings>.NativeClassPtr, 100675254);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 197152, RefRangeEnd = 197154, XrefRangeStart = 197151, XrefRangeEnd = 197152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OtherSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OtherSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public OtherSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
