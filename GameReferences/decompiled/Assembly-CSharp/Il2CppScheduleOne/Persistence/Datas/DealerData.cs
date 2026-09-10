using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

public class DealerData : NPCData
{
	private static readonly IntPtr NativeFieldInfoPtr_Recruited;

	private static readonly IntPtr NativeFieldInfoPtr_AssignedCustomerIDs;

	private static readonly IntPtr NativeFieldInfoPtr_ActiveContractGUIDs;

	private static readonly IntPtr NativeFieldInfoPtr_Cash;

	private static readonly IntPtr NativeFieldInfoPtr_OverflowItems;

	private static readonly IntPtr NativeFieldInfoPtr_HasBeenRecommended;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStringArray_Il2CppStringArray_Single_ItemSet_Boolean_0;

	public unsafe bool Recruited
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Recruited);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Recruited)) = flag;
		}
	}

	public unsafe Il2CppStringArray AssignedCustomerIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AssignedCustomerIDs);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AssignedCustomerIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStringArray ActiveContractGUIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveContractGUIDs);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveContractGUIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float Cash
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cash);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cash)) = num;
		}
	}

	public unsafe ItemSet OverflowItems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverflowItems);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemSet>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverflowItems)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemSet));
		}
	}

	public unsafe bool HasBeenRecommended
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasBeenRecommended);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasBeenRecommended)) = flag;
		}
	}

	static DealerData()
	{
		Il2CppClassPointerStore<DealerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "DealerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DealerData>.NativeClassPtr);
		NativeFieldInfoPtr_Recruited = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerData>.NativeClassPtr, "Recruited");
		NativeFieldInfoPtr_AssignedCustomerIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerData>.NativeClassPtr, "AssignedCustomerIDs");
		NativeFieldInfoPtr_ActiveContractGUIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerData>.NativeClassPtr, "ActiveContractGUIDs");
		NativeFieldInfoPtr_Cash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerData>.NativeClassPtr, "Cash");
		NativeFieldInfoPtr_OverflowItems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerData>.NativeClassPtr, "OverflowItems");
		NativeFieldInfoPtr_HasBeenRecommended = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DealerData>.NativeClassPtr, "HasBeenRecommended");
		NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStringArray_Il2CppStringArray_Single_ItemSet_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DealerData>.NativeClassPtr, 100669413);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135022, RefRangeEnd = 135023, XrefRangeStart = 135017, XrefRangeEnd = 135022, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DealerData(string id, bool recruited, Il2CppStringArray assignedCustomerIDs, Il2CppStringArray activeContractGUIDs, float cash, ItemSet overflowItems, bool hasBeenRecommended)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DealerData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(id);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &recruited;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)assignedCustomerIDs);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeContractGUIDs);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &cash;
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)overflowItems);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &hasBeenRecommended;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Il2CppStringArray_Il2CppStringArray_Single_ItemSet_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DealerData(IntPtr pointer)
		: base(pointer)
	{
	}
}
