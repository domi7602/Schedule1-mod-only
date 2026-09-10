using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.PlayerTasks;

public class RotateRigidbodyToTarget : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_Rigidbody;

	private static readonly IntPtr NativeFieldInfoPtr_TargetRotation;

	private static readonly IntPtr NativeFieldInfoPtr_RotationForce;

	private static readonly IntPtr NativeFieldInfoPtr_Bitch;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Rigidbody Rigidbody
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rigidbody);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rigidbody)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rigidbody));
		}
	}

	public unsafe Vector3 TargetRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetRotation);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetRotation)) = vector;
		}
	}

	public unsafe float RotationForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RotationForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RotationForce)) = num;
		}
	}

	public unsafe Transform Bitch
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bitch);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Bitch)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	static RotateRigidbodyToTarget()
	{
		Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerTasks", "RotateRigidbodyToTarget");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr);
		NativeFieldInfoPtr_Rigidbody = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr, "Rigidbody");
		NativeFieldInfoPtr_TargetRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr, "TargetRotation");
		NativeFieldInfoPtr_RotationForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr, "RotationForce");
		NativeFieldInfoPtr_Bitch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr, "Bitch");
		NativeMethodInfoPtr_FixedUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr, 100668222);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr, 100668223);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 117417, XrefRangeEnd = 117430, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 117430, XrefRangeEnd = 117431, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RotateRigidbodyToTarget()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RotateRigidbodyToTarget>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RotateRigidbodyToTarget(IntPtr pointer)
		: base(pointer)
	{
	}
}
