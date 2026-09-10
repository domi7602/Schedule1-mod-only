using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Equipping.Framework;

namespace Il2CppScheduleOne.Equipping.Framework;

public class CustomHandlerEquippableData : EquippableData
{
	private static readonly IntPtr NativeFieldInfoPtr_Handler;

	private static readonly IntPtr NativeMethodInfoPtr_OnValidate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EquippedItemHandler Handler
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Handler);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EquippedItemHandler>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Handler)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equippedItemHandler));
		}
	}

	static CustomHandlerEquippableData()
	{
		Il2CppClassPointerStore<CustomHandlerEquippableData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "CustomHandlerEquippableData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CustomHandlerEquippableData>.NativeClassPtr);
		NativeFieldInfoPtr_Handler = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CustomHandlerEquippableData>.NativeClassPtr, "Handler");
		NativeMethodInfoPtr_OnValidate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomHandlerEquippableData>.NativeClassPtr, 100679921);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomHandlerEquippableData>.NativeClassPtr, 100679922);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245026, XrefRangeEnd = 245038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnValidate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 199973, RefRangeEnd = 199979, XrefRangeStart = 199973, XrefRangeEnd = 199979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CustomHandlerEquippableData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CustomHandlerEquippableData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CustomHandlerEquippableData(IntPtr pointer)
		: base(pointer)
	{
	}
}
