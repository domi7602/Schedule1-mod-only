using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2Cpp;

public class LandingButtons : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__landingSpotController;

	private static readonly IntPtr NativeFieldInfoPtr__flockController;

	private static readonly IntPtr NativeFieldInfoPtr_hSliderValue;

	private static readonly IntPtr NativeMethodInfoPtr_OnGUI_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe LandingSpotController _landingSpotController
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__landingSpotController);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<LandingSpotController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__landingSpotController)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)landingSpotController));
		}
	}

	public unsafe FlockController _flockController
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flockController);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<FlockController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flockController)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)flockController));
		}
	}

	public unsafe float hSliderValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hSliderValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hSliderValue)) = num;
		}
	}

	static LandingButtons()
	{
		Il2CppClassPointerStore<LandingButtons>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "LandingButtons");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr);
		NativeFieldInfoPtr__landingSpotController = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr, "_landingSpotController");
		NativeFieldInfoPtr__flockController = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr, "_flockController");
		NativeFieldInfoPtr_hSliderValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr, "hSliderValue");
		NativeMethodInfoPtr_OnGUI_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr, 100663616);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr, 100663617);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68086, XrefRangeEnd = 68156, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnGUI()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnGUI_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 68156, XrefRangeEnd = 68157, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LandingButtons()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LandingButtons>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LandingButtons(IntPtr pointer)
		: base(pointer)
	{
	}
}
