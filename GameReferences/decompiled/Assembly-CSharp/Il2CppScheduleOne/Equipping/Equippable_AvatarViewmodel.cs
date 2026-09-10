using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace Il2CppScheduleOne.Equipping;

public class Equippable_AvatarViewmodel : Equippable_Viewmodel
{
	private static readonly IntPtr NativeFieldInfoPtr_AnimatorController;

	private static readonly IntPtr NativeFieldInfoPtr_ViewmodelAvatarOffset;

	private static readonly IntPtr NativeFieldInfoPtr_ViewmodelAvatarRotationOffset;

	private static readonly IntPtr NativeFieldInfoPtr_EquipTime;

	private static readonly IntPtr NativeFieldInfoPtr_EquipTrigger;

	private static readonly IntPtr NativeFieldInfoPtr_timeEquipped;

	private static readonly IntPtr NativeMethodInfoPtr_get_equipAnimDone_Protected_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_PlayEquipAnimation_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe RuntimeAnimatorController AnimatorController
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AnimatorController);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RuntimeAnimatorController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AnimatorController)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)runtimeAnimatorController));
		}
	}

	public unsafe Vector3 ViewmodelAvatarOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ViewmodelAvatarOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ViewmodelAvatarOffset)) = vector;
		}
	}

	public unsafe Vector3 ViewmodelAvatarRotationOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ViewmodelAvatarRotationOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ViewmodelAvatarRotationOffset)) = vector;
		}
	}

	public unsafe float EquipTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EquipTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EquipTime)) = num;
		}
	}

	public unsafe string EquipTrigger
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EquipTrigger);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EquipTrigger)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float timeEquipped
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeEquipped);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeEquipped)) = num;
		}
	}

	public unsafe bool equipAnimDone
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_equipAnimDone_Protected_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static Equippable_AvatarViewmodel()
	{
		Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping", "Equippable_AvatarViewmodel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr);
		NativeFieldInfoPtr_AnimatorController = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, "AnimatorController");
		NativeFieldInfoPtr_ViewmodelAvatarOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, "ViewmodelAvatarOffset");
		NativeFieldInfoPtr_ViewmodelAvatarRotationOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, "ViewmodelAvatarRotationOffset");
		NativeFieldInfoPtr_EquipTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, "EquipTime");
		NativeFieldInfoPtr_EquipTrigger = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, "EquipTrigger");
		NativeFieldInfoPtr_timeEquipped = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, "timeEquipped");
		NativeMethodInfoPtr_get_equipAnimDone_Protected_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, 100679761);
		NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, 100679762);
		NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, 100679763);
		NativeMethodInfoPtr_PlayEquipAnimation_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, 100679764);
		NativeMethodInfoPtr_Update_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, 100679765);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr, 100679766);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 243541, RefRangeEnd = 243543, XrefRangeStart = 243513, XrefRangeEnd = 243541, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Equip(ItemInstance item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Equip_Public_Virtual_Void_ItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 243550, RefRangeEnd = 243552, XrefRangeStart = 243543, XrefRangeEnd = 243550, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Unequip()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Unequip_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 243552, XrefRangeEnd = 243569, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void PlayEquipAnimation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_PlayEquipAnimation_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 243569, XrefRangeEnd = 243570, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 243581, RefRangeEnd = 243583, XrefRangeStart = 243570, XrefRangeEnd = 243581, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Equippable_AvatarViewmodel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Equippable_AvatarViewmodel>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Equippable_AvatarViewmodel(IntPtr pointer)
		: base(pointer)
	{
	}
}
