using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Map;

public class DarkMarketAccessZone : TimedAccessZone
{
	private static readonly IntPtr NativeMethodInfoPtr_GetIsOpen_Protected_Virtual_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static DarkMarketAccessZone()
	{
		Il2CppClassPointerStore<DarkMarketAccessZone>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map", "DarkMarketAccessZone");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DarkMarketAccessZone>.NativeClassPtr);
		NativeMethodInfoPtr_GetIsOpen_Protected_Virtual_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DarkMarketAccessZone>.NativeClassPtr, 100670083);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DarkMarketAccessZone>.NativeClassPtr, 100670084);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141475, XrefRangeEnd = 141483, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool GetIsOpen()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetIsOpen_Protected_Virtual_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141483, XrefRangeEnd = 141484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DarkMarketAccessZone()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DarkMarketAccessZone>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DarkMarketAccessZone(IntPtr pointer)
		: base(pointer)
	{
	}
}
