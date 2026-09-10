using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Object;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Equipping.Framework;
using Il2CppScheduleOne.Core.Items.Framework;
using UnityEngine;

namespace Il2CppScheduleOne.Equipping.Framework;

public class INetworkedEquippableUser : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_NetworkBehaviour_Public_Abstract_Virtual_New_get_NetworkBehaviour_0;

	private static readonly IntPtr NativeMethodInfoPtr_EquipLocal_Public_Abstract_Virtual_New_IEquippedItemHandler_EquippableData_0;

	private static readonly IntPtr NativeMethodInfoPtr_EquipLocal_Public_Abstract_Virtual_New_IEquippedItemHandler_BaseItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ItemHandlerContainer_Public_Virtual_New_get_Transform_0;

	public unsafe virtual NetworkBehaviour NetworkBehaviour
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_NetworkBehaviour_Public_Abstract_Virtual_New_get_NetworkBehaviour_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NetworkBehaviour>(intPtr) : null;
		}
	}

	public unsafe virtual Transform ItemHandlerContainer
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245313, XrefRangeEnd = 245318, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_ItemHandlerContainer_Public_Virtual_New_get_Transform_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
	}

	static INetworkedEquippableUser()
	{
		Il2CppClassPointerStore<INetworkedEquippableUser>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "INetworkedEquippableUser");
		NativeMethodInfoPtr_get_NetworkBehaviour_Public_Abstract_Virtual_New_get_NetworkBehaviour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<INetworkedEquippableUser>.NativeClassPtr, 100679940);
		NativeMethodInfoPtr_EquipLocal_Public_Abstract_Virtual_New_IEquippedItemHandler_EquippableData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<INetworkedEquippableUser>.NativeClassPtr, 100679941);
		NativeMethodInfoPtr_EquipLocal_Public_Abstract_Virtual_New_IEquippedItemHandler_BaseItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<INetworkedEquippableUser>.NativeClassPtr, 100679942);
		NativeMethodInfoPtr_get_ItemHandlerContainer_Public_Virtual_New_get_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<INetworkedEquippableUser>.NativeClassPtr, 100679943);
	}

	[CallerCount(0)]
	public unsafe virtual IEquippedItemHandler EquipLocal(EquippableData equippable)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equippable);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_EquipLocal_Public_Abstract_Virtual_New_IEquippedItemHandler_EquippableData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IEquippedItemHandler>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual IEquippedItemHandler EquipLocal(BaseItemInstance item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_EquipLocal_Public_Abstract_Virtual_New_IEquippedItemHandler_BaseItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IEquippedItemHandler>(intPtr) : null;
	}

	public INetworkedEquippableUser(IntPtr pointer)
		: base(pointer)
	{
	}
}
