using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Equipping.Framework;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Equipping;

public class TPEquippedUmbrella : TPEquippedItem
{
	private static readonly System.IntPtr NativeFieldInfoPtr_CanopyMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_CanopySkinnedMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr__random;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equip_Public_Virtual_Void_IEquippedItemHandler_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<MeshRenderer> CanopyMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<MeshRenderer>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<SkinnedMeshRenderer> CanopySkinnedMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopySkinnedMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SkinnedMeshRenderer>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopySkinnedMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppSystem.Random _random
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__random);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Random>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__random)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)random));
		}
	}

	static TPEquippedUmbrella()
	{
		Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping", "TPEquippedUmbrella");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr);
		NativeFieldInfoPtr_CanopyMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr, "CanopyMeshes");
		NativeFieldInfoPtr_CanopySkinnedMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr, "CanopySkinnedMeshes");
		NativeFieldInfoPtr__random = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr, "_random");
		NativeMethodInfoPtr_Equip_Public_Virtual_Void_IEquippedItemHandler_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr, 100679919);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr, 100679920);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 244987, XrefRangeEnd = 245025, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Equip(IEquippedItemHandler handler)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)handler);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Equip_Public_Virtual_Void_IEquippedItemHandler_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245025, XrefRangeEnd = 245026, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TPEquippedUmbrella()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TPEquippedUmbrella>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TPEquippedUmbrella(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
