using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Tools;
using UnityEngine;

namespace Il2CppScheduleOne.PlayerScripts;

public class LocalPlayerFootstepGenerator : GenericFootstepDetector
{
	private static readonly IntPtr NativeFieldInfoPtr_DistancePerStep;

	private static readonly IntPtr NativeFieldInfoPtr__movement;

	private static readonly IntPtr NativeFieldInfoPtr__currentDistance;

	private static readonly IntPtr NativeFieldInfoPtr__lastFramePosition;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float DistancePerStep
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DistancePerStep, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DistancePerStep, (void*)(&num));
		}
	}

	public unsafe PlayerMovement _movement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__movement);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PlayerMovement>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__movement)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerMovement));
		}
	}

	public unsafe float _currentDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentDistance)) = num;
		}
	}

	public unsafe Vector3 _lastFramePosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lastFramePosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lastFramePosition)) = vector;
		}
	}

	static LocalPlayerFootstepGenerator()
	{
		Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerScripts", "LocalPlayerFootstepGenerator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr);
		NativeFieldInfoPtr_DistancePerStep = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, "DistancePerStep");
		NativeFieldInfoPtr__movement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, "_movement");
		NativeFieldInfoPtr__currentDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, "_currentDistance");
		NativeFieldInfoPtr__lastFramePosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, "_lastFramePosition");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, 100671321);
		NativeMethodInfoPtr_LateUpdate_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, 100671322);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr, 100671323);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153501, XrefRangeEnd = 153505, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public new unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153505, XrefRangeEnd = 153515, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153515, XrefRangeEnd = 153521, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LocalPlayerFootstepGenerator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LocalPlayerFootstepGenerator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LocalPlayerFootstepGenerator(IntPtr pointer)
		: base(pointer)
	{
	}
}
