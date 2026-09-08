using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Core.Equipping.Framework;

namespace Il2CppScheduleOne.Equipping.Framework;

public class EquippableItemDefinition : GenericEquippableItemDefinition<EquippableData>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static EquippableItemDefinition()
	{
		Il2CppClassPointerStore<EquippableItemDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "EquippableItemDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EquippableItemDefinition>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableItemDefinition>.NativeClassPtr, 100679935);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245297, XrefRangeEnd = 245300, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EquippableItemDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EquippableItemDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EquippableItemDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
