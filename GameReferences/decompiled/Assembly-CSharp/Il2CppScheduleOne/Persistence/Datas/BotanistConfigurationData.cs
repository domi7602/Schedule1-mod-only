using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class BotanistConfigurationData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Bed;

	private static readonly IntPtr NativeFieldInfoPtr_Supplies;

	private static readonly IntPtr NativeFieldInfoPtr_Pots;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_ObjectFieldData_ObjectFieldData_ObjectListFieldData_0;

	public unsafe ObjectFieldData Bed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bed);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ObjectFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectFieldData));
		}
	}

	public unsafe ObjectFieldData Supplies
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Supplies);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ObjectFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Supplies)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectFieldData));
		}
	}

	public unsafe ObjectListFieldData Pots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Pots);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ObjectListFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Pots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectListFieldData));
		}
	}

	static BotanistConfigurationData()
	{
		Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "BotanistConfigurationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr);
		NativeFieldInfoPtr_Bed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr, "Bed");
		NativeFieldInfoPtr_Supplies = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr, "Supplies");
		NativeFieldInfoPtr_Pots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr, "Pots");
		NativeMethodInfoPtr__ctor_Public_Void_ObjectFieldData_ObjectFieldData_ObjectListFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr, 100669378);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 134792, RefRangeEnd = 134797, XrefRangeStart = 134788, XrefRangeEnd = 134792, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BotanistConfigurationData(ObjectFieldData bed, ObjectFieldData supplies, ObjectListFieldData pots)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BotanistConfigurationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bed);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)supplies);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pots);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ObjectFieldData_ObjectFieldData_ObjectListFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BotanistConfigurationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
