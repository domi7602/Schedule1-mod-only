using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Management;

public class QualityField : ConfigField
{
	private static readonly IntPtr NativeFieldInfoPtr__Value_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_onValueChanged;

	private static readonly IntPtr NativeMethodInfoPtr_get_Value_Public_get_EQuality_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Value_Protected_set_Void_EQuality_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_EntityConfiguration_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetValue_Public_Void_EQuality_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsValueDefault_Public_Virtual_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetData_Public_QualityFieldData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Load_Public_Void_QualityFieldData_0;

	public unsafe EQuality _Value_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Value_k__BackingField);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Value_k__BackingField)) = eQuality;
		}
	}

	public unsafe UnityEvent<EQuality> onValueChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onValueChanged);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent<EQuality>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onValueChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe EQuality Value
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 3891, RefRangeEnd = 3894, XrefRangeStart = 3891, XrefRangeEnd = 3894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Value_Public_get_EQuality_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29109, RefRangeEnd = 29110, XrefRangeStart = 29109, XrefRangeEnd = 29110, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Value_Protected_set_Void_EQuality_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static QualityField()
	{
		Il2CppClassPointerStore<QualityField>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Management", "QualityField");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QualityField>.NativeClassPtr);
		NativeFieldInfoPtr__Value_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QualityField>.NativeClassPtr, "<Value>k__BackingField");
		NativeFieldInfoPtr_onValueChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<QualityField>.NativeClassPtr, "onValueChanged");
		NativeMethodInfoPtr_get_Value_Public_get_EQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670847);
		NativeMethodInfoPtr_set_Value_Protected_set_Void_EQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670848);
		NativeMethodInfoPtr__ctor_Public_Void_EntityConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670849);
		NativeMethodInfoPtr_SetValue_Public_Void_EQuality_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670850);
		NativeMethodInfoPtr_IsValueDefault_Public_Virtual_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670851);
		NativeMethodInfoPtr_GetData_Public_QualityFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670852);
		NativeMethodInfoPtr_Load_Public_Void_QualityFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QualityField>.NativeClassPtr, 100670853);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 149632, RefRangeEnd = 149633, XrefRangeStart = 149624, XrefRangeEnd = 149632, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QualityField(EntityConfiguration parentConfig)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<QualityField>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parentConfig);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_EntityConfiguration_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 149637, RefRangeEnd = 149642, XrefRangeStart = 149633, XrefRangeEnd = 149637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetValue(EQuality value, bool network)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&value);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &network;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetValue_Public_Void_EQuality_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override bool IsValueDefault()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_IsValueDefault_Public_Virtual_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 149646, RefRangeEnd = 149647, XrefRangeStart = 149642, XrefRangeEnd = 149646, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QualityFieldData GetData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetData_Public_QualityFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QualityFieldData>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 149651, RefRangeEnd = 149653, XrefRangeStart = 149647, XrefRangeEnd = 149651, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Load(QualityFieldData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Load_Public_Void_QualityFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public QualityField(IntPtr pointer)
		: base(pointer)
	{
	}
}
