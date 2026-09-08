using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Vision;

public class VisionObscurer : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_ObscuranceAmount;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float ObscuranceAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObscuranceAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObscuranceAmount)) = num;
		}
	}

	static VisionObscurer()
	{
		Il2CppClassPointerStore<VisionObscurer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vision", "VisionObscurer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VisionObscurer>.NativeClassPtr);
		NativeFieldInfoPtr_ObscuranceAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionObscurer>.NativeClassPtr, "ObscuranceAmount");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionObscurer>.NativeClassPtr, 100668662);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123677, XrefRangeEnd = 123678, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VisionObscurer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VisionObscurer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VisionObscurer(IntPtr pointer)
		: base(pointer)
	{
	}
}
