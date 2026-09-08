using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class BusinessData : PropertyData
{
	private static readonly IntPtr NativeFieldInfoPtr_LaunderingOperations;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStructArray_1_Boolean_Il2CppStructArray_1_Boolean_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_LaunderOperationData_0;

	public unsafe Il2CppReferenceArray<LaunderOperationData> LaunderingOperations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LaunderingOperations);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<LaunderOperationData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LaunderingOperations)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static BusinessData()
	{
		Il2CppClassPointerStore<BusinessData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "BusinessData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BusinessData>.NativeClassPtr);
		NativeFieldInfoPtr_LaunderingOperations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BusinessData>.NativeClassPtr, "LaunderingOperations");
		NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStructArray_1_Boolean_Il2CppStructArray_1_Boolean_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_LaunderOperationData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BusinessData>.NativeClassPtr, 100669314);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134330, RefRangeEnd = 134331, XrefRangeStart = 134323, XrefRangeEnd = 134330, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BusinessData(string propertyCode, bool isOwned, Il2CppStructArray<bool> switchStates, Il2CppStructArray<bool> toggleableStates, Il2CppReferenceArray<DynamicSaveData> employees, Il2CppReferenceArray<DynamicSaveData> objects, Il2CppReferenceArray<LaunderOperationData> launderingOperations)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BusinessData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(propertyCode);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &isOwned;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)switchStates);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toggleableStates);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)employees);
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objects);
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)launderingOperations);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStructArray_1_Boolean_Il2CppStructArray_1_Boolean_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_LaunderOperationData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BusinessData(IntPtr pointer)
		: base(pointer)
	{
	}
}
