using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppScheduleOne.Weather;

public class LensFlareSettings : ScriptableObject
{
	[System.Serializable]
	public class LensFlareSettingsGroup : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_LensFlare;

		private static readonly System.IntPtr NativeFieldInfoPtr_Intensity;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe LensFlareDataSRP LensFlare
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LensFlare);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<LensFlareDataSRP>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LensFlare)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lensFlareDataSRP));
			}
		}

		public unsafe float Intensity
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Intensity);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Intensity)) = num;
			}
		}

		static LensFlareSettingsGroup()
		{
			Il2CppClassPointerStore<LensFlareSettingsGroup>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr, "LensFlareSettingsGroup");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LensFlareSettingsGroup>.NativeClassPtr);
			NativeFieldInfoPtr_LensFlare = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareSettingsGroup>.NativeClassPtr, "LensFlare");
			NativeFieldInfoPtr_Intensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareSettingsGroup>.NativeClassPtr, "Intensity");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareSettingsGroup>.NativeClassPtr, 100685663);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LensFlareSettingsGroup()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LensFlareSettingsGroup>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public LensFlareSettingsGroup(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_lensFlareGroups;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetLensFlareSettings_Public_Boolean_LensFlareDataSRP_byref_LensFlareSettingsGroup_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLensFlareGroups_Public_Il2CppReferenceArray_1_LensFlareSettingsGroup_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<LensFlareSettingsGroup> lensFlareGroups
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lensFlareGroups);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<LensFlareSettingsGroup>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lensFlareGroups)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static LensFlareSettings()
	{
		Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Weather", "LensFlareSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr);
		NativeFieldInfoPtr_lensFlareGroups = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr, "lensFlareGroups");
		NativeMethodInfoPtr_TryGetLensFlareSettings_Public_Boolean_LensFlareDataSRP_byref_LensFlareSettingsGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr, 100685660);
		NativeMethodInfoPtr_GetLensFlareGroups_Public_Il2CppReferenceArray_1_LensFlareSettingsGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr, 100685661);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr, 100685662);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 292522, XrefRangeEnd = 292528, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetLensFlareSettings(LensFlareDataSRP lensFlare, out LensFlareSettingsGroup group)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lensFlare);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetLensFlareSettings_Public_Boolean_LensFlareDataSRP_byref_LensFlareSettingsGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		group = ((num3 == 0) ? null : new LensFlareSettingsGroup(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<LensFlareSettingsGroup> GetLensFlareGroups()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLensFlareGroups_Public_Il2CppReferenceArray_1_LensFlareSettingsGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<LensFlareSettingsGroup>>(intPtr) : null;
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LensFlareSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LensFlareSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LensFlareSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
