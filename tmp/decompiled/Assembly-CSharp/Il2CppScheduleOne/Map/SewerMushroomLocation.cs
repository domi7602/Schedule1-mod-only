using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Map;

public class SewerMushroomLocation : MonoBehaviour
{
	[System.Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MushroomLocationData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_isActive;

		private static readonly System.IntPtr NativeFieldInfoPtr_location;

		private static readonly System.IntPtr NativeFieldInfoPtr_rotation;

		private static readonly System.IntPtr NativeFieldInfoPtr_scale;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.U1)]
		public bool isActive;

		[FieldOffset(4)]
		public Vector3 location;

		[FieldOffset(16)]
		public Quaternion rotation;

		[FieldOffset(32)]
		public float scale;

		static MushroomLocationData()
		{
			Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, "MushroomLocationData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr);
			NativeFieldInfoPtr_isActive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr, "isActive");
			NativeFieldInfoPtr_location = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr, "location");
			NativeFieldInfoPtr_rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr, "rotation");
			NativeFieldInfoPtr_scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr, "scale");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<MushroomLocationData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__data;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMushroomsFromData_Public_Void_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMushroomFromData_Private_Void_Transform_MushroomLocationData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMushroomLocationData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<MushroomLocationData> _data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<MushroomLocationData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static SewerMushroomLocation()
	{
		Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map", "SewerMushroomLocation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr);
		NativeFieldInfoPtr__data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, "_data");
		NativeMethodInfoPtr_SetMushroomsFromData_Public_Void_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, 100670348);
		NativeMethodInfoPtr_SetMushroomFromData_Private_Void_Transform_MushroomLocationData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, 100670349);
		NativeMethodInfoPtr_ClearData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, 100670350);
		NativeMethodInfoPtr_SetMushroomLocationData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, 100670351);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr, 100670352);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 143841, RefRangeEnd = 143842, XrefRangeStart = 143824, XrefRangeEnd = 143841, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMushroomsFromData(GameObject mushroomObject)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mushroomObject);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMushroomsFromData_Public_Void_GameObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 143847, RefRangeEnd = 143848, XrefRangeStart = 143842, XrefRangeEnd = 143847, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMushroomFromData(Transform childMushroomObj, MushroomLocationData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)childMushroomObj);
		*(MushroomLocationData**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &data;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMushroomFromData_Private_Void_Transform_MushroomLocationData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 143848, XrefRangeEnd = 143849, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 143849, XrefRangeEnd = 143864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMushroomLocationData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMushroomLocationData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 143864, XrefRangeEnd = 143872, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SewerMushroomLocation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SewerMushroomLocation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SewerMushroomLocation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
