using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

public class LawData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_InternalLawIntensity;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_0;

	public unsafe float InternalLawIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InternalLawIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InternalLawIntensity)) = num;
		}
	}

	static LawData()
	{
		Il2CppClassPointerStore<LawData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "LawData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LawData>.NativeClassPtr);
		NativeFieldInfoPtr_InternalLawIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LawData>.NativeClassPtr, "InternalLawIntensity");
		NativeMethodInfoPtr__ctor_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LawData>.NativeClassPtr, 100669375);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134782, RefRangeEnd = 134783, XrefRangeStart = 134781, XrefRangeEnd = 134782, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LawData(float internalLawIntensity)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LawData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&internalLawIntensity);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LawData(IntPtr pointer)
		: base(pointer)
	{
	}
}
