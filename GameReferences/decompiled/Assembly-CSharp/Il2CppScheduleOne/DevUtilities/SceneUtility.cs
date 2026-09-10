using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Product;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public class SceneUtility : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_DrugAffinityToAdd;

	private static readonly IntPtr NativeFieldInfoPtr_MinMaxAffinityRange;

	private static readonly IntPtr NativeFieldInfoPtr_UseCurrentHighestAffinityAsMax;

	private static readonly IntPtr NativeFieldInfoPtr_SceneObjects;

	private static readonly IntPtr NativeFieldInfoPtr__rootObject;

	private static readonly IntPtr NativeFieldInfoPtr__showCountOnly;

	private static readonly IntPtr NativeMethodInfoPtr_ScanSceneForShaders_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddAffinityAndRandomise_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RemoveAffinity_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__AddAffinityAndRandomise_b__7_0_Private_Boolean_ProductTypeAffinity_0;

	private static readonly IntPtr NativeMethodInfoPtr__RemoveAffinity_b__8_0_Private_Boolean_ProductTypeAffinity_0;

	private static readonly IntPtr NativeMethodInfoPtr__RemoveAffinity_b__8_1_Private_Boolean_ProductTypeAffinity_0;

	public unsafe EDrugType DrugAffinityToAdd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DrugAffinityToAdd);
			return *(EDrugType*)num;
		}
		set
		{
			*(EDrugType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DrugAffinityToAdd)) = eDrugType;
		}
	}

	public unsafe Vector2 MinMaxAffinityRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinMaxAffinityRange);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinMaxAffinityRange)) = vector;
		}
	}

	public unsafe bool UseCurrentHighestAffinityAsMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseCurrentHighestAffinityAsMax);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseCurrentHighestAffinityAsMax)) = flag;
		}
	}

	public unsafe List<Transform> SceneObjects
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SceneObjects);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Transform>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SceneObjects)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Transform _rootObject
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rootObject);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rootObject)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool _showCountOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__showCountOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__showCountOnly)) = flag;
		}
	}

	static SceneUtility()
	{
		Il2CppClassPointerStore<SceneUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "SceneUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr);
		NativeFieldInfoPtr_DrugAffinityToAdd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, "DrugAffinityToAdd");
		NativeFieldInfoPtr_MinMaxAffinityRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, "MinMaxAffinityRange");
		NativeFieldInfoPtr_UseCurrentHighestAffinityAsMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, "UseCurrentHighestAffinityAsMax");
		NativeFieldInfoPtr_SceneObjects = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, "SceneObjects");
		NativeFieldInfoPtr__rootObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, "_rootObject");
		NativeFieldInfoPtr__showCountOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, "_showCountOnly");
		NativeMethodInfoPtr_ScanSceneForShaders_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675171);
		NativeMethodInfoPtr_AddAffinityAndRandomise_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675172);
		NativeMethodInfoPtr_RemoveAffinity_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675173);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675174);
		NativeMethodInfoPtr__AddAffinityAndRandomise_b__7_0_Private_Boolean_ProductTypeAffinity_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675175);
		NativeMethodInfoPtr__RemoveAffinity_b__8_0_Private_Boolean_ProductTypeAffinity_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675176);
		NativeMethodInfoPtr__RemoveAffinity_b__8_1_Private_Boolean_ProductTypeAffinity_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr, 100675177);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 196247, XrefRangeEnd = 196320, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ScanSceneForShaders()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScanSceneForShaders_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 196320, XrefRangeEnd = 196393, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddAffinityAndRandomise()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddAffinityAndRandomise_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 196393, XrefRangeEnd = 196440, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveAffinity()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveAffinity_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 196440, XrefRangeEnd = 196441, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SceneUtility()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SceneUtility>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool _AddAffinityAndRandomise_b__7_0(ProductTypeAffinity x)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__AddAffinityAndRandomise_b__7_0_Private_Boolean_ProductTypeAffinity_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool _RemoveAffinity_b__8_0(ProductTypeAffinity x)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__RemoveAffinity_b__8_0_Private_Boolean_ProductTypeAffinity_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool _RemoveAffinity_b__8_1(ProductTypeAffinity x)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__RemoveAffinity_b__8_1_Private_Boolean_ProductTypeAffinity_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public SceneUtility(IntPtr pointer)
		: base(pointer)
	{
	}
}
