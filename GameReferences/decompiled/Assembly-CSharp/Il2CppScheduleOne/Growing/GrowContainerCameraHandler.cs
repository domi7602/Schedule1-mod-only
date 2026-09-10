using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Growing;

public class GrowContainerCameraHandler : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "ECameraPosition")]
	public enum ECameraPosition
	{
		Closeup,
		Midshot,
		Fullshot,
		BirdsEye
	}

	private static readonly IntPtr NativeFieldInfoPtr_RotateCameraContainerToFacePlayer;

	private static readonly IntPtr NativeFieldInfoPtr_SnapRotationToRightAngles;

	private static readonly IntPtr NativeFieldInfoPtr__midshotCamera;

	private static readonly IntPtr NativeFieldInfoPtr__closeupCamera;

	private static readonly IntPtr NativeFieldInfoPtr__fullshotContainer;

	private static readonly IntPtr NativeFieldInfoPtr__birdsEyeCamera;

	private static readonly IntPtr NativeFieldInfoPtr__debugCameraPosition;

	private static readonly IntPtr NativeMethodInfoPtr_PositionCameraContainer_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetCameraPosition_Public_Transform_ECameraPosition_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetCameraPosition_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool RotateCameraContainerToFacePlayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RotateCameraContainerToFacePlayer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RotateCameraContainerToFacePlayer)) = flag;
		}
	}

	public unsafe bool SnapRotationToRightAngles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SnapRotationToRightAngles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SnapRotationToRightAngles)) = flag;
		}
	}

	public unsafe Transform _midshotCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__midshotCamera);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__midshotCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform _closeupCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__closeupCamera);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__closeupCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform _fullshotContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fullshotContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fullshotContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform _birdsEyeCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdsEyeCamera);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdsEyeCamera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe ECameraPosition _debugCameraPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugCameraPosition);
			return *(ECameraPosition*)num;
		}
		set
		{
			*(ECameraPosition*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugCameraPosition)) = eCameraPosition;
		}
	}

	static GrowContainerCameraHandler()
	{
		Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "GrowContainerCameraHandler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr);
		NativeFieldInfoPtr_RotateCameraContainerToFacePlayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "RotateCameraContainerToFacePlayer");
		NativeFieldInfoPtr_SnapRotationToRightAngles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "SnapRotationToRightAngles");
		NativeFieldInfoPtr__midshotCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "_midshotCamera");
		NativeFieldInfoPtr__closeupCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "_closeupCamera");
		NativeFieldInfoPtr__fullshotContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "_fullshotContainer");
		NativeFieldInfoPtr__birdsEyeCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "_birdsEyeCamera");
		NativeFieldInfoPtr__debugCameraPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, "_debugCameraPosition");
		NativeMethodInfoPtr_PositionCameraContainer_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, 100678466);
		NativeMethodInfoPtr_GetCameraPosition_Public_Transform_ECameraPosition_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, 100678467);
		NativeMethodInfoPtr_SetCameraPosition_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, 100678468);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr, 100678469);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 229623, RefRangeEnd = 229625, XrefRangeStart = 229599, XrefRangeEnd = 229623, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PositionCameraContainer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PositionCameraContainer_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 229626, RefRangeEnd = 229632, XrefRangeStart = 229625, XrefRangeEnd = 229626, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Transform GetCameraPosition(ECameraPosition pos, bool autoPosition = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&pos);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &autoPosition;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCameraPosition_Public_Transform_ECameraPosition_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 229632, XrefRangeEnd = 229644, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetCameraPosition()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCameraPosition_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 229644, XrefRangeEnd = 229645, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GrowContainerCameraHandler()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GrowContainerCameraHandler>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GrowContainerCameraHandler(IntPtr pointer)
		: base(pointer)
	{
	}
}
