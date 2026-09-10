using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Map;

public class MedicalCentre : NPCEnterableBuilding
{
	private static readonly IntPtr NativeFieldInfoPtr_RespawnPoint;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Transform RespawnPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RespawnPoint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RespawnPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	static MedicalCentre()
	{
		Il2CppClassPointerStore<MedicalCentre>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map", "MedicalCentre");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MedicalCentre>.NativeClassPtr);
		NativeFieldInfoPtr_RespawnPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MedicalCentre>.NativeClassPtr, "RespawnPoint");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MedicalCentre>.NativeClassPtr, 100670178);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 142240, RefRangeEnd = 142241, XrefRangeStart = 142229, XrefRangeEnd = 142240, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MedicalCentre()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MedicalCentre>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MedicalCentre(IntPtr pointer)
		: base(pointer)
	{
	}
}
