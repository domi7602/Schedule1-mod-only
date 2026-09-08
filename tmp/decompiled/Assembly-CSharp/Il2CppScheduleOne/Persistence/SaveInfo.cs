using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence;

public class SaveInfo : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_SavePath;

	private static readonly System.IntPtr NativeFieldInfoPtr_SaveSlotNumber;

	private static readonly System.IntPtr NativeFieldInfoPtr_OrganisationName;

	private static readonly System.IntPtr NativeFieldInfoPtr_DateCreated;

	private static readonly System.IntPtr NativeFieldInfoPtr_DateLastPlayed;

	private static readonly System.IntPtr NativeFieldInfoPtr_Networth;

	private static readonly System.IntPtr NativeFieldInfoPtr_SaveVersion;

	private static readonly System.IntPtr NativeFieldInfoPtr_MetaData;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_DateTime_DateTime_Single_String_MetaData_0;

	public unsafe string SavePath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SavePath);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SavePath)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int SaveSlotNumber
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SaveSlotNumber);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SaveSlotNumber)) = num;
		}
	}

	public unsafe string OrganisationName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OrganisationName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OrganisationName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppSystem.DateTime DateCreated
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DateCreated);
			return *(Il2CppSystem.DateTime*)num;
		}
		set
		{
			*(Il2CppSystem.DateTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DateCreated)) = dateTime;
		}
	}

	public unsafe Il2CppSystem.DateTime DateLastPlayed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DateLastPlayed);
			return *(Il2CppSystem.DateTime*)num;
		}
		set
		{
			*(Il2CppSystem.DateTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DateLastPlayed)) = dateTime;
		}
	}

	public unsafe float Networth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Networth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Networth)) = num;
		}
	}

	public unsafe string SaveVersion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SaveVersion);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SaveVersion)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe MetaData MetaData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MetaData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MetaData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MetaData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)metaData));
		}
	}

	static SaveInfo()
	{
		Il2CppClassPointerStore<SaveInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence", "SaveInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr);
		NativeFieldInfoPtr_SavePath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "SavePath");
		NativeFieldInfoPtr_SaveSlotNumber = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "SaveSlotNumber");
		NativeFieldInfoPtr_OrganisationName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "OrganisationName");
		NativeFieldInfoPtr_DateCreated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "DateCreated");
		NativeFieldInfoPtr_DateLastPlayed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "DateLastPlayed");
		NativeFieldInfoPtr_Networth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "Networth");
		NativeFieldInfoPtr_SaveVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "SaveVersion");
		NativeFieldInfoPtr_MetaData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, "MetaData");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_DateTime_DateTime_Single_String_MetaData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr, 100668914);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 127363, RefRangeEnd = 127365, XrefRangeStart = 127358, XrefRangeEnd = 127363, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SaveInfo(string savePath, int saveSlotNumber, string organisationName, Il2CppSystem.DateTime dateCreated, Il2CppSystem.DateTime dateLastPlayed, float networth, string saveVersion, MetaData metaData)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SaveInfo>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(savePath);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &saveSlotNumber;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(organisationName);
		*(Il2CppSystem.DateTime**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &dateCreated;
		*(Il2CppSystem.DateTime**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &dateLastPlayed;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &networth;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(saveVersion);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)metaData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_String_DateTime_DateTime_Single_String_MetaData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SaveInfo(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
