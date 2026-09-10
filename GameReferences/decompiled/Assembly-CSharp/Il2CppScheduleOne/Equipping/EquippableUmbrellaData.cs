using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Equipping.Framework;
using UnityEngine;

namespace Il2CppScheduleOne.Equipping;

public class EquippableUmbrellaData : EquippableData
{
	private static readonly IntPtr NativeFieldInfoPtr_CanopyColor;

	private static readonly IntPtr NativeFieldInfoPtr_CanopyDecal;

	private static readonly IntPtr NativeFieldInfoPtr_CanopyDecalColor;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Gradient CanopyColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyColor);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Gradient>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyColor)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gradient));
		}
	}

	public unsafe Texture2D CanopyDecal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyDecal);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyDecal)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Color CanopyDecalColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyDecalColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanopyDecalColor)) = color;
		}
	}

	static EquippableUmbrellaData()
	{
		Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping", "EquippableUmbrellaData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr);
		NativeFieldInfoPtr_CanopyColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr, "CanopyColor");
		NativeFieldInfoPtr_CanopyDecal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr, "CanopyDecal");
		NativeFieldInfoPtr_CanopyDecalColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr, "CanopyDecalColor");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr, 100679756);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 199973, RefRangeEnd = 199979, XrefRangeStart = 199973, XrefRangeEnd = 199979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EquippableUmbrellaData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EquippableUmbrellaData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EquippableUmbrellaData(IntPtr pointer)
		: base(pointer)
	{
	}
}
