using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;

namespace Il2CppScheduleOne.ObjectScripts.WateringCan;

[Serializable]
public class TrashGrabberDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static TrashGrabberDefinition()
	{
		Il2CppClassPointerStore<TrashGrabberDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ObjectScripts.WateringCan", "TrashGrabberDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashGrabberDefinition>.NativeClassPtr);
		NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashGrabberDefinition>.NativeClassPtr, 100681895);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashGrabberDefinition>.NativeClassPtr, 100681896);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 263324, XrefRangeEnd = 263333, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override ItemInstance GetDefaultInstance(int quantity = 1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&quantity);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetDefaultInstance_Public_Virtual_ItemInstance_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 166837, RefRangeEnd = 166838, XrefRangeStart = 166837, XrefRangeEnd = 166838, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashGrabberDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashGrabberDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashGrabberDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
