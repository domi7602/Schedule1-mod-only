using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class RenamableConfigurationData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Name;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_0;

	public unsafe StringFieldData Name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StringFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stringFieldData));
		}
	}

	static RenamableConfigurationData()
	{
		Il2CppClassPointerStore<RenamableConfigurationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "RenamableConfigurationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RenamableConfigurationData>.NativeClassPtr);
		NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RenamableConfigurationData>.NativeClassPtr, "Name");
		NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RenamableConfigurationData>.NativeClassPtr, 100669397);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 134314, RefRangeEnd = 134323, XrefRangeStart = 134314, XrefRangeEnd = 134323, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RenamableConfigurationData(StringFieldData name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RenamableConfigurationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RenamableConfigurationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
