using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class MetaData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_CreationDate;

	private static readonly IntPtr NativeFieldInfoPtr_LastPlayedDate;

	private static readonly IntPtr NativeFieldInfoPtr_CreationVersion;

	private static readonly IntPtr NativeFieldInfoPtr_LastSaveVersion;

	private static readonly IntPtr NativeFieldInfoPtr_PlayTutorial;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_DateTimeData_DateTimeData_String_String_Boolean_0;

	public unsafe DateTimeData CreationDate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreationDate);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DateTimeData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreationDate)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dateTimeData));
		}
	}

	public unsafe DateTimeData LastPlayedDate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastPlayedDate);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DateTimeData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastPlayedDate)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dateTimeData));
		}
	}

	public unsafe string CreationVersion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreationVersion);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CreationVersion)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string LastSaveVersion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastSaveVersion);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastSaveVersion)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool PlayTutorial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlayTutorial);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlayTutorial)) = flag;
		}
	}

	static MetaData()
	{
		Il2CppClassPointerStore<MetaData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MetaData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MetaData>.NativeClassPtr);
		NativeFieldInfoPtr_CreationDate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MetaData>.NativeClassPtr, "CreationDate");
		NativeFieldInfoPtr_LastPlayedDate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MetaData>.NativeClassPtr, "LastPlayedDate");
		NativeFieldInfoPtr_CreationVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MetaData>.NativeClassPtr, "CreationVersion");
		NativeFieldInfoPtr_LastSaveVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MetaData>.NativeClassPtr, "LastSaveVersion");
		NativeFieldInfoPtr_PlayTutorial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MetaData>.NativeClassPtr, "PlayTutorial");
		NativeMethodInfoPtr__ctor_Public_Void_DateTimeData_DateTimeData_String_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MetaData>.NativeClassPtr, 100669403);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 134983, RefRangeEnd = 134987, XrefRangeStart = 134978, XrefRangeEnd = 134983, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MetaData(DateTimeData creationDate, DateTimeData lastPlayedDate, string creationVersion, string lastSaveVersion, bool playTutorial)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MetaData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)creationDate);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lastPlayedDate);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(creationVersion);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(lastSaveVersion);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &playTutorial;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_DateTimeData_DateTimeData_String_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MetaData(IntPtr pointer)
		: base(pointer)
	{
	}
}
