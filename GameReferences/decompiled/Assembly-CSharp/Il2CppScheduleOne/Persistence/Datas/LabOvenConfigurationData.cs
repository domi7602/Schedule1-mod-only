using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class LabOvenConfigurationData : RenamableConfigurationData
{
	private static readonly IntPtr NativeFieldInfoPtr_Destination;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_ObjectFieldData_0;

	public unsafe ObjectFieldData Destination
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Destination);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ObjectFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Destination)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectFieldData));
		}
	}

	static LabOvenConfigurationData()
	{
		Il2CppClassPointerStore<LabOvenConfigurationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "LabOvenConfigurationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LabOvenConfigurationData>.NativeClassPtr);
		NativeFieldInfoPtr_Destination = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LabOvenConfigurationData>.NativeClassPtr, "Destination");
		NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_ObjectFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LabOvenConfigurationData>.NativeClassPtr, 100669386);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 134800, RefRangeEnd = 134812, XrefRangeStart = 134800, XrefRangeEnd = 134812, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LabOvenConfigurationData(StringFieldData name, ObjectFieldData destination)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LabOvenConfigurationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)name);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_ObjectFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LabOvenConfigurationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
