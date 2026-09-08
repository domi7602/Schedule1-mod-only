using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

public class RoadCracksRandomizer : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_Cracks;

	private static readonly IntPtr NativeFieldInfoPtr_MinCount;

	private static readonly IntPtr NativeFieldInfoPtr_MaxCount;

	private static readonly IntPtr NativeMethodInfoPtr_Randomize_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<Transform> Cracks
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cracks);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Transform>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cracks)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int MinCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinCount)) = num;
		}
	}

	public unsafe int MaxCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxCount)) = num;
		}
	}

	static RoadCracksRandomizer()
	{
		Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "RoadCracksRandomizer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr);
		NativeFieldInfoPtr_Cracks = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr, "Cracks");
		NativeFieldInfoPtr_MinCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr, "MinCount");
		NativeFieldInfoPtr_MaxCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr, "MaxCount");
		NativeMethodInfoPtr_Randomize_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr, 100677890);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr, 100677891);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 225037, XrefRangeEnd = 225062, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Randomize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Randomize_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 225062, XrefRangeEnd = 225063, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RoadCracksRandomizer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RoadCracksRandomizer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RoadCracksRandomizer(IntPtr pointer)
		: base(pointer)
	{
	}
}
