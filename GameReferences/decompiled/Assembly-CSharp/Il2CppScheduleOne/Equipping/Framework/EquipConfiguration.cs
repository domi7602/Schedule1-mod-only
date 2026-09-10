using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Configuration;
using Il2CppScheduleOne.Core.Equipping.Framework;
using Il2CppScheduleOne.Core.Settings;
using Il2CppSystem;

namespace Il2CppScheduleOne.Equipping.Framework;

public class EquipConfiguration : Configuration<EquipSettings>
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Handlers;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetHandlerForData_Public_Boolean_Type_byref_IEquippedItemHandler_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<EquippedItemHandler> Handlers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Handlers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<EquippedItemHandler>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Handlers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static EquipConfiguration()
	{
		Il2CppClassPointerStore<EquipConfiguration>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "EquipConfiguration");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EquipConfiguration>.NativeClassPtr);
		NativeFieldInfoPtr_Handlers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquipConfiguration>.NativeClassPtr, "Handlers");
		NativeMethodInfoPtr_TryGetHandlerForData_Public_Boolean_Type_byref_IEquippedItemHandler_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquipConfiguration>.NativeClassPtr, 100679923);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquipConfiguration>.NativeClassPtr, 100679924);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245038, XrefRangeEnd = 245052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetHandlerForData(Il2CppSystem.Type handlerType, out IEquippedItemHandler handler)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)handlerType);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetHandlerForData_Public_Boolean_Type_byref_IEquippedItemHandler_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		handler = ((num3 == 0) ? null : new IEquippedItemHandler(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245052, XrefRangeEnd = 245055, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EquipConfiguration()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EquipConfiguration>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EquipConfiguration(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
