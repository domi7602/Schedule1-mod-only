using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Property;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class ManorData : PropertyData
{
	private static readonly IntPtr NativeFieldInfoPtr_ManorState;

	private static readonly IntPtr NativeFieldInfoPtr_DaysSinceStateChange;

	private static readonly IntPtr NativeFieldInfoPtr_TunnelDug;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStructArray_1_Boolean_Il2CppStructArray_1_Boolean_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_DynamicSaveData_EManorState_Int32_Boolean_0;

	public unsafe Manor.EManorState ManorState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ManorState);
			return *(Manor.EManorState*)num;
		}
		set
		{
			*(Manor.EManorState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ManorState)) = eManorState;
		}
	}

	public unsafe int DaysSinceStateChange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DaysSinceStateChange);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DaysSinceStateChange)) = num;
		}
	}

	public unsafe bool TunnelDug
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TunnelDug);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TunnelDug)) = flag;
		}
	}

	static ManorData()
	{
		Il2CppClassPointerStore<ManorData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ManorData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ManorData>.NativeClassPtr);
		NativeFieldInfoPtr_ManorState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ManorData>.NativeClassPtr, "ManorState");
		NativeFieldInfoPtr_DaysSinceStateChange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ManorData>.NativeClassPtr, "DaysSinceStateChange");
		NativeFieldInfoPtr_TunnelDug = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ManorData>.NativeClassPtr, "TunnelDug");
		NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStructArray_1_Boolean_Il2CppStructArray_1_Boolean_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_DynamicSaveData_EManorState_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ManorData>.NativeClassPtr, 100669402);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134977, RefRangeEnd = 134978, XrefRangeStart = 134971, XrefRangeEnd = 134977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ManorData(string propertyCode, bool isOwned, Il2CppStructArray<bool> switchStates, Il2CppStructArray<bool> toggleableStates, Il2CppReferenceArray<DynamicSaveData> employees, Il2CppReferenceArray<DynamicSaveData> objects, Manor.EManorState state, int daysSinceStateChange, bool tunnelDug)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ManorData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[9];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(propertyCode);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &isOwned;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)switchStates);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toggleableStates);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)employees);
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objects);
		*(Manor.EManorState**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &state;
		*(int**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = &daysSinceStateChange;
		*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = &tunnelDug;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStructArray_1_Boolean_Il2CppStructArray_1_Boolean_Il2CppReferenceArray_1_DynamicSaveData_Il2CppReferenceArray_1_DynamicSaveData_EManorState_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ManorData(IntPtr pointer)
		: base(pointer)
	{
	}
}
