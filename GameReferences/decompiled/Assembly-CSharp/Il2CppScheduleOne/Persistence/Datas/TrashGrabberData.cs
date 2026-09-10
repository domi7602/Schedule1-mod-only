using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class TrashGrabberData : ItemData
{
	private static readonly IntPtr NativeFieldInfoPtr_Content;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_TrashContentData_0;

	public unsafe TrashContentData Content
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Content);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TrashContentData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Content)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashContentData));
		}
	}

	static TrashGrabberData()
	{
		Il2CppClassPointerStore<TrashGrabberData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "TrashGrabberData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashGrabberData>.NativeClassPtr);
		NativeFieldInfoPtr_Content = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashGrabberData>.NativeClassPtr, "Content");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_TrashContentData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashGrabberData>.NativeClassPtr, 100669371);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 134580, RefRangeEnd = 134586, XrefRangeStart = 134580, XrefRangeEnd = 134586, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashGrabberData(string iD, int quantity, TrashContentData content)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashGrabberData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(iD);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &quantity;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)content);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_TrashContentData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashGrabberData(IntPtr pointer)
		: base(pointer)
	{
	}
}
