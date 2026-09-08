using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class LaunderOperationData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Amount;

	private static readonly IntPtr NativeFieldInfoPtr_MinutesSinceStarted;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Int32_0;

	public unsafe float Amount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Amount)) = num;
		}
	}

	public unsafe int MinutesSinceStarted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinutesSinceStarted);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinutesSinceStarted)) = num;
		}
	}

	static LaunderOperationData()
	{
		Il2CppClassPointerStore<LaunderOperationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "LaunderOperationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LaunderOperationData>.NativeClassPtr);
		NativeFieldInfoPtr_Amount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LaunderOperationData>.NativeClassPtr, "Amount");
		NativeFieldInfoPtr_MinutesSinceStarted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LaunderOperationData>.NativeClassPtr, "MinutesSinceStarted");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LaunderOperationData>.NativeClassPtr, 100669374);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134780, RefRangeEnd = 134781, XrefRangeStart = 134779, XrefRangeEnd = 134780, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LaunderOperationData(float amount, int minutesSinceStarted)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LaunderOperationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&amount);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &minutesSinceStarted;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LaunderOperationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
