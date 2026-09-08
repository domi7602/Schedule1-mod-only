using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.ObjectScripts;

[System.Serializable]
public class DryingOperation : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ItemID;

	private static readonly System.IntPtr NativeFieldInfoPtr_Quantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_StartQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_Time;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_EQuality_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IncreaseQuality_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetQualityItemInstance_Public_QualityItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetQuality_Public_EQuality_0;

	public unsafe string ItemID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int Quantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quantity)) = num;
		}
	}

	public unsafe EQuality StartQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartQuality);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartQuality)) = eQuality;
		}
	}

	public unsafe float Time
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Time);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Time)) = num;
		}
	}

	static DryingOperation()
	{
		Il2CppClassPointerStore<DryingOperation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ObjectScripts", "DryingOperation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr);
		NativeFieldInfoPtr_ItemID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, "ItemID");
		NativeFieldInfoPtr_Quantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, "Quantity");
		NativeFieldInfoPtr_StartQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, "StartQuality");
		NativeFieldInfoPtr_Time = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, "Time");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_EQuality_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, 100680632);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, 100680633);
		NativeMethodInfoPtr_IncreaseQuality_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, 100680634);
		NativeMethodInfoPtr_GetQualityItemInstance_Public_QualityItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, 100680635);
		NativeMethodInfoPtr_GetQuality_Public_EQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr, 100680636);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 252022, XrefRangeEnd = 252024, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DryingOperation(string itemID, int quantity, EQuality startQuality, float time)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(itemID);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &quantity;
		*(EQuality**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startQuality;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_EQuality_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DryingOperation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DryingOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void IncreaseQuality()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IncreaseQuality_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 252028, RefRangeEnd = 252029, XrefRangeStart = 252024, XrefRangeEnd = 252028, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe QualityItemInstance GetQualityItemInstance()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetQualityItemInstance_Public_QualityItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<QualityItemInstance>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 252029, RefRangeEnd = 252034, XrefRangeStart = 252029, XrefRangeEnd = 252029, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EQuality GetQuality()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetQuality_Public_EQuality_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EQuality*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public DryingOperation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
