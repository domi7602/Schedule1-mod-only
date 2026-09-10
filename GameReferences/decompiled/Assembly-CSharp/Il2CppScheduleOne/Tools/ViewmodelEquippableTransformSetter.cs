using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

public class ViewmodelEquippableTransformSetter : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_lastRecordedLocalPosition;

	private static readonly IntPtr NativeFieldInfoPtr_lastRecordedLocalEulerAngles;

	private static readonly IntPtr NativeFieldInfoPtr_lastRecordedLocalScale;

	private static readonly IntPtr NativeFieldInfoPtr_transformChangedApplied;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Vector3 lastRecordedLocalPosition
	{
		get
		{
			Unsafe.SkipInit(out Vector3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lastRecordedLocalPosition, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lastRecordedLocalPosition, (void*)(&vector));
		}
	}

	public unsafe static Vector3 lastRecordedLocalEulerAngles
	{
		get
		{
			Unsafe.SkipInit(out Vector3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lastRecordedLocalEulerAngles, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lastRecordedLocalEulerAngles, (void*)(&vector));
		}
	}

	public unsafe static Vector3 lastRecordedLocalScale
	{
		get
		{
			Unsafe.SkipInit(out Vector3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lastRecordedLocalScale, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lastRecordedLocalScale, (void*)(&vector));
		}
	}

	public unsafe static bool transformChangedApplied
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_transformChangedApplied, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_transformChangedApplied, (void*)(&flag));
		}
	}

	static ViewmodelEquippableTransformSetter()
	{
		Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "ViewmodelEquippableTransformSetter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr);
		NativeFieldInfoPtr_lastRecordedLocalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr, "lastRecordedLocalPosition");
		NativeFieldInfoPtr_lastRecordedLocalEulerAngles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr, "lastRecordedLocalEulerAngles");
		NativeFieldInfoPtr_lastRecordedLocalScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr, "lastRecordedLocalScale");
		NativeFieldInfoPtr_transformChangedApplied = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr, "transformChangedApplied");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr, 100678227);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ViewmodelEquippableTransformSetter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ViewmodelEquippableTransformSetter>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ViewmodelEquippableTransformSetter(IntPtr pointer)
		: base(pointer)
	{
	}
}
