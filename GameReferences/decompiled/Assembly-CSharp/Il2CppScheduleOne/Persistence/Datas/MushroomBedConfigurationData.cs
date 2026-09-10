using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class MushroomBedConfigurationData : RenamableConfigurationData
{
	private static readonly IntPtr NativeFieldInfoPtr_Spawn;

	private static readonly IntPtr NativeFieldInfoPtr_Additive1;

	private static readonly IntPtr NativeFieldInfoPtr_Additive2;

	private static readonly IntPtr NativeFieldInfoPtr_Additive3;

	private static readonly IntPtr NativeFieldInfoPtr_Destination;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_ItemFieldData_ItemFieldData_ItemFieldData_ItemFieldData_ObjectFieldData_0;

	public unsafe ItemFieldData Spawn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Spawn);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Spawn)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldData));
		}
	}

	public unsafe ItemFieldData Additive1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive1);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive1)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldData));
		}
	}

	public unsafe ItemFieldData Additive2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive2);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldData));
		}
	}

	public unsafe ItemFieldData Additive3
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive3);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive3)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldData));
		}
	}

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

	static MushroomBedConfigurationData()
	{
		Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MushroomBedConfigurationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr);
		NativeFieldInfoPtr_Spawn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr, "Spawn");
		NativeFieldInfoPtr_Additive1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr, "Additive1");
		NativeFieldInfoPtr_Additive2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr, "Additive2");
		NativeFieldInfoPtr_Additive3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr, "Additive3");
		NativeFieldInfoPtr_Destination = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr, "Destination");
		NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_ItemFieldData_ItemFieldData_ItemFieldData_ItemFieldData_ObjectFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr, 100669388);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134819, RefRangeEnd = 134821, XrefRangeStart = 134812, XrefRangeEnd = 134819, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MushroomBedConfigurationData(StringFieldData name, ItemFieldData spawn, ItemFieldData additive1, ItemFieldData additive2, ItemFieldData additive3, ObjectFieldData destination)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MushroomBedConfigurationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)name);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spawn);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additive1);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additive2);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additive3);
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_ItemFieldData_ItemFieldData_ItemFieldData_ItemFieldData_ObjectFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MushroomBedConfigurationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
