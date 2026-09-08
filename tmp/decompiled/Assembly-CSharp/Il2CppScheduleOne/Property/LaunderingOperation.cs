using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Property;

public class LaunderingOperation : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_business;

	private static readonly System.IntPtr NativeFieldInfoPtr_amount;

	private static readonly System.IntPtr NativeFieldInfoPtr_minutesSinceStarted;

	private static readonly System.IntPtr NativeFieldInfoPtr_completionTime_Minutes;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Business_Single_Int32_0;

	public unsafe Business business
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_business);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Business>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_business)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)business));
		}
	}

	public unsafe float amount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_amount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_amount)) = num;
		}
	}

	public unsafe int minutesSinceStarted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minutesSinceStarted);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minutesSinceStarted)) = num;
		}
	}

	public unsafe int completionTime_Minutes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_completionTime_Minutes);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_completionTime_Minutes)) = num;
		}
	}

	static LaunderingOperation()
	{
		Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Property", "LaunderingOperation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr);
		NativeFieldInfoPtr_business = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr, "business");
		NativeFieldInfoPtr_amount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr, "amount");
		NativeFieldInfoPtr_minutesSinceStarted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr, "minutesSinceStarted");
		NativeFieldInfoPtr_completionTime_Minutes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr, "completionTime_Minutes");
		NativeMethodInfoPtr__ctor_Public_Void_Business_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr, 100667819);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 112432, XrefRangeEnd = 112434, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LaunderingOperation(Business _business, float _amount, int _minutesSinceStarted)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LaunderingOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_business);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &_amount;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &_minutesSinceStarted;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Business_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LaunderingOperation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
