using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Configuration;
using Il2CppScheduleOne.Core.Deliveries;

namespace Il2CppScheduleOne.Delivery;

public class DeliveryConfiguration : Configuration<DeliverySettings>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static DeliveryConfiguration()
	{
		Il2CppClassPointerStore<DeliveryConfiguration>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Delivery", "DeliveryConfiguration");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeliveryConfiguration>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeliveryConfiguration>.NativeClassPtr, 100675354);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 198457, XrefRangeEnd = 198460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeliveryConfiguration()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeliveryConfiguration>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeliveryConfiguration(IntPtr pointer)
		: base(pointer)
	{
	}
}
