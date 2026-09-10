using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;

namespace Il2CppScheduleOne.Persistence.Datas;

public class GameData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_OrganisationName;

	private static readonly IntPtr NativeFieldInfoPtr_Seed;

	private static readonly IntPtr NativeFieldInfoPtr_Settings;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_GameSettings_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string OrganisationName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OrganisationName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OrganisationName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int Seed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Seed);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Seed)) = num;
		}
	}

	public unsafe GameSettings Settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Settings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameSettings));
		}
	}

	static GameData()
	{
		Il2CppClassPointerStore<GameData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "GameData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameData>.NativeClassPtr);
		NativeFieldInfoPtr_OrganisationName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameData>.NativeClassPtr, "OrganisationName");
		NativeFieldInfoPtr_Seed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameData>.NativeClassPtr, "Seed");
		NativeFieldInfoPtr_Settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameData>.NativeClassPtr, "Settings");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_GameSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameData>.NativeClassPtr, 100669333);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameData>.NativeClassPtr, 100669334);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 134580, RefRangeEnd = 134586, XrefRangeStart = 134577, XrefRangeEnd = 134580, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameData(string organisationName, int seed, GameSettings settings)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(organisationName);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &seed;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_GameSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134591, RefRangeEnd = 134592, XrefRangeStart = 134586, XrefRangeEnd = 134591, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GameData(IntPtr pointer)
		: base(pointer)
	{
	}
}
