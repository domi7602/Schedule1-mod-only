using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Money;

public class Transaction : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_transaction_Name;

	private static readonly System.IntPtr NativeFieldInfoPtr_unit_Amount;

	private static readonly System.IntPtr NativeFieldInfoPtr_quantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_transaction_Note;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_total_Amount_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Single_Single_String_0;

	public unsafe string transaction_Name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transaction_Name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transaction_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float unit_Amount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unit_Amount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unit_Amount)) = num;
		}
	}

	public unsafe float quantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_quantity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_quantity)) = num;
		}
	}

	public unsafe string transaction_Note
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transaction_Note);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transaction_Note)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float total_Amount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_total_Amount_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static Transaction()
	{
		Il2CppClassPointerStore<Transaction>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Money", "Transaction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Transaction>.NativeClassPtr);
		NativeFieldInfoPtr_transaction_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Transaction>.NativeClassPtr, "transaction_Name");
		NativeFieldInfoPtr_unit_Amount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Transaction>.NativeClassPtr, "unit_Amount");
		NativeFieldInfoPtr_quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Transaction>.NativeClassPtr, "quantity");
		NativeFieldInfoPtr_transaction_Note = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Transaction>.NativeClassPtr, "transaction_Note");
		NativeMethodInfoPtr_get_total_Amount_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Transaction>.NativeClassPtr, 100670047);
		NativeMethodInfoPtr__ctor_Public_Void_String_Single_Single_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Transaction>.NativeClassPtr, 100670048);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141201, XrefRangeEnd = 141209, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Transaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Transaction>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(_transaction_Name);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &_unit_Amount;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &_quantity;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(_transaction_Note);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Single_Single_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Transaction(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
