using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Police;

public class Offense : Il2CppSystem.Object
{
	public class Charge : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_chargeName;

		private static readonly System.IntPtr NativeFieldInfoPtr_crimeIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_quantity;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_0;

		public unsafe string chargeName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_chargeName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_chargeName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe int crimeIndex
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_crimeIndex);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_crimeIndex)) = num;
			}
		}

		public unsafe int quantity
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_quantity);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_quantity)) = num;
			}
		}

		static Charge()
		{
			Il2CppClassPointerStore<Charge>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Offense>.NativeClassPtr, "Charge");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Charge>.NativeClassPtr);
			NativeFieldInfoPtr_chargeName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Charge>.NativeClassPtr, "chargeName");
			NativeFieldInfoPtr_crimeIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Charge>.NativeClassPtr, "crimeIndex");
			NativeFieldInfoPtr_quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Charge>.NativeClassPtr, "quantity");
			NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Charge>.NativeClassPtr, 100676095);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 206476, XrefRangeEnd = 206482, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Charge(string _chargeName, int _crimeIndex, int _quantity)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Charge>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(_chargeName);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &_crimeIndex;
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &_quantity;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Charge(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_charges;

	private static readonly System.IntPtr NativeFieldInfoPtr_penalties;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_Charge_0;

	public unsafe List<Charge> charges
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_charges);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Charge>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_charges)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<string> penalties
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_penalties);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_penalties)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static Offense()
	{
		Il2CppClassPointerStore<Offense>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Police", "Offense");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Offense>.NativeClassPtr);
		NativeFieldInfoPtr_charges = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Offense>.NativeClassPtr, "charges");
		NativeFieldInfoPtr_penalties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Offense>.NativeClassPtr, "penalties");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_Charge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Offense>.NativeClassPtr, 100676094);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 206482, XrefRangeEnd = 206501, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Offense(List<Charge> _charges)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Offense>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_charges);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_Charge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Offense(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
