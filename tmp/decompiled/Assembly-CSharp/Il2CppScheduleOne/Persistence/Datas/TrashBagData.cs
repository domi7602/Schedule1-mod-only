using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

public class TrashBagData : TrashItemData
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_Vector3_Quaternion_TrashContentData_0;

	static TrashBagData()
	{
		Il2CppClassPointerStore<TrashBagData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "TrashBagData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashBagData>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_String_Vector3_Quaternion_TrashContentData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBagData>.NativeClassPtr, 100669475);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135503, RefRangeEnd = 135504, XrefRangeStart = 135488, XrefRangeEnd = 135503, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashBagData(string trashID, string guid, Vector3 position, Quaternion rotation, TrashContentData contents)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashBagData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(trashID);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(guid);
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &position;
		*(Quaternion**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &rotation;
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)contents);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_Vector3_Quaternion_TrashContentData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashBagData(IntPtr pointer)
		: base(pointer)
	{
	}
}
