using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class NumberFieldData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Value;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_0;

	public unsafe float Value
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Value);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Value)) = num;
		}
	}

	static NumberFieldData()
	{
		Il2CppClassPointerStore<NumberFieldData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "NumberFieldData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NumberFieldData>.NativeClassPtr);
		NativeFieldInfoPtr_Value = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NumberFieldData>.NativeClassPtr, "Value");
		NativeMethodInfoPtr__ctor_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NumberFieldData>.NativeClassPtr, 100669390);
	}

	[CallerCount(149)]
	[CachedScanResults(RefRangeStart = 134822, RefRangeEnd = 134971, XrefRangeStart = 134821, XrefRangeEnd = 134822, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NumberFieldData(float value)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NumberFieldData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NumberFieldData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
