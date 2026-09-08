using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class SerializedSaveData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__DataType;

	private static readonly System.IntPtr NativeFieldInfoPtr_DataType;

	private static readonly System.IntPtr NativeFieldInfoPtr__DataVersion;

	private static readonly System.IntPtr NativeFieldInfoPtr_DataVersion;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Version_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static string _DataType
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__DataType, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__DataType, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string DataType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataType);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataType)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static int _DataVersion
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__DataVersion, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__DataVersion, (void*)(&num));
		}
	}

	public unsafe int DataVersion
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataVersion);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataVersion)) = num;
		}
	}

	public unsafe string Version
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 135435, XrefRangeEnd = 135439, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Version_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static SerializedSaveData()
	{
		Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "SerializedSaveData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr);
		NativeFieldInfoPtr__DataType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr, "_DataType");
		NativeFieldInfoPtr_DataType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr, "DataType");
		NativeFieldInfoPtr__DataVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr, "_DataVersion");
		NativeFieldInfoPtr_DataVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr, "DataVersion");
		NativeMethodInfoPtr_get_Version_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr, 100669463);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr, 100669464);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 135439, XrefRangeEnd = 135444, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SerializedSaveData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SerializedSaveData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SerializedSaveData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
