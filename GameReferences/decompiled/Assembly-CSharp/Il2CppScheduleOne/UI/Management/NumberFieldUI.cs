using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Management;
using Il2CppSystem.Collections.Generic;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne.UI.Management;

public class NumberFieldUI : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__Fields_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_FieldLabel;

	private static readonly IntPtr NativeFieldInfoPtr_Slider;

	private static readonly IntPtr NativeFieldInfoPtr_ValueLabel;

	private static readonly IntPtr NativeFieldInfoPtr_MinValueLabel;

	private static readonly IntPtr NativeFieldInfoPtr_MaxValueLabel;

	private static readonly IntPtr NativeMethodInfoPtr_get_Fields_Public_get_List_1_NumberField_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Fields_Protected_set_Void_List_1_NumberField_0;

	private static readonly IntPtr NativeMethodInfoPtr_Bind_Public_Void_List_1_NumberField_0;

	private static readonly IntPtr NativeMethodInfoPtr_IncrementValue_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_Refresh_Private_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_AreFieldsUniform_Private_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_ValueChanged_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<NumberField> _Fields_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Fields_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<NumberField>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Fields_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe TextMeshProUGUI FieldLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FieldLabel);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FieldLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe Slider Slider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Slider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Slider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Slider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slider));
		}
	}

	public unsafe TextMeshProUGUI ValueLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ValueLabel);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ValueLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe TextMeshProUGUI MinValueLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinValueLabel);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinValueLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe TextMeshProUGUI MaxValueLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxValueLabel);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxValueLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe List<NumberField> Fields
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Fields_Public_get_List_1_NumberField_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<NumberField>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Fields_Protected_set_Void_List_1_NumberField_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static NumberFieldUI()
	{
		Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Management", "NumberFieldUI");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr);
		NativeFieldInfoPtr__Fields_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, "<Fields>k__BackingField");
		NativeFieldInfoPtr_FieldLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, "FieldLabel");
		NativeFieldInfoPtr_Slider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, "Slider");
		NativeFieldInfoPtr_ValueLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, "ValueLabel");
		NativeFieldInfoPtr_MinValueLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, "MinValueLabel");
		NativeFieldInfoPtr_MaxValueLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, "MaxValueLabel");
		NativeMethodInfoPtr_get_Fields_Public_get_List_1_NumberField_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688679);
		NativeMethodInfoPtr_set_Fields_Protected_set_Void_List_1_NumberField_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688680);
		NativeMethodInfoPtr_Bind_Public_Void_List_1_NumberField_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688681);
		NativeMethodInfoPtr_IncrementValue_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688682);
		NativeMethodInfoPtr_Refresh_Private_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688683);
		NativeMethodInfoPtr_AreFieldsUniform_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688684);
		NativeMethodInfoPtr_ValueChanged_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688685);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr, 100688686);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 323827, RefRangeEnd = 323829, XrefRangeStart = 323769, XrefRangeEnd = 323827, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Bind(List<NumberField> field)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)field);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Bind_Public_Void_List_1_NumberField_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 323829, XrefRangeEnd = 323830, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void IncrementValue(int amt)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&amt);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IncrementValue_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 323830, XrefRangeEnd = 323841, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Refresh(float newVal)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&newVal);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Refresh_Private_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 323841, XrefRangeEnd = 323848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool AreFieldsUniform()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AreFieldsUniform_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 323848, XrefRangeEnd = 323854, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ValueChanged(float value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ValueChanged_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 323854, XrefRangeEnd = 323862, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NumberFieldUI()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NumberFieldUI>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NumberFieldUI(IntPtr pointer)
		: base(pointer)
	{
	}
}
