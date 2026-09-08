using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class ObjectListFieldData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ObjectGUIDs;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_String_0;

	public unsafe List<string> ObjectGUIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectGUIDs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectGUIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static ObjectListFieldData()
	{
		Il2CppClassPointerStore<ObjectListFieldData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ObjectListFieldData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectListFieldData>.NativeClassPtr);
		NativeFieldInfoPtr_ObjectGUIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectListFieldData>.NativeClassPtr, "ObjectGUIDs");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectListFieldData>.NativeClassPtr, 100669392);
	}

	[CallerCount(203)]
	[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ObjectListFieldData(List<string> objectGUIDs)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ObjectListFieldData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectGUIDs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ObjectListFieldData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
