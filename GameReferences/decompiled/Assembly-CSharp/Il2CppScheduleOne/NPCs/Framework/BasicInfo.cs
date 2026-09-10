using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class BasicInfo : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_FirstName;

	private static readonly System.IntPtr NativeFieldInfoPtr_HasLastName;

	private static readonly System.IntPtr NativeFieldInfoPtr_LastName;

	private static readonly System.IntPtr NativeFieldInfoPtr_ID;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_BasicInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string FirstName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FirstName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FirstName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool HasLastName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasLastName);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasLastName)) = flag;
		}
	}

	public unsafe string LastName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string ID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static BasicInfo()
	{
		Il2CppClassPointerStore<BasicInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "BasicInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr);
		NativeFieldInfoPtr_FirstName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr, "FirstName");
		NativeFieldInfoPtr_HasLastName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr, "HasLastName");
		NativeFieldInfoPtr_LastName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr, "LastName");
		NativeFieldInfoPtr_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr, "ID");
		NativeMethodInfoPtr_GetCopy_Public_BasicInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr, 100682797);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr, 100682798);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272214, XrefRangeEnd = 272221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BasicInfo GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_BasicInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BasicInfo>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BasicInfo()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BasicInfo>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BasicInfo(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
