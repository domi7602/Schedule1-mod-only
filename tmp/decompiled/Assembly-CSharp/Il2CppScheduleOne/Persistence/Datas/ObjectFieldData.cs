using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class ObjectFieldData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ObjectGUID;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	public unsafe string ObjectGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ObjectFieldData()
	{
		Il2CppClassPointerStore<ObjectFieldData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ObjectFieldData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectFieldData>.NativeClassPtr);
		NativeFieldInfoPtr_ObjectGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectFieldData>.NativeClassPtr, "ObjectGUID");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectFieldData>.NativeClassPtr, 100669391);
	}

	[CallerCount(203)]
	[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ObjectFieldData(string objectGUID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ObjectFieldData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(objectGUID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ObjectFieldData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
