using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

[System.Serializable]
public class TransformLerp : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__transform;

	private static readonly System.IntPtr NativeFieldInfoPtr__min;

	private static readonly System.IntPtr NativeFieldInfoPtr__max;

	private static readonly System.IntPtr NativeFieldInfoPtr__lerpPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr__lerpRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr__lerpScale;

	private static readonly System.IntPtr NativeFieldInfoPtr__disableOnZero;

	private static readonly System.IntPtr NativeFieldInfoPtr__currentLerpValue;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLerpValue_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Transform _transform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__transform);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__transform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform _min
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__min);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__min)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform _max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__max);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__max)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool _lerpPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpPosition)) = flag;
		}
	}

	public unsafe bool _lerpRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpRotation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpRotation)) = flag;
		}
	}

	public unsafe bool _lerpScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpScale);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpScale)) = flag;
		}
	}

	public unsafe bool _disableOnZero
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__disableOnZero);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__disableOnZero)) = flag;
		}
	}

	public unsafe float _currentLerpValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentLerpValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentLerpValue)) = num;
		}
	}

	static TransformLerp()
	{
		Il2CppClassPointerStore<TransformLerp>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "TransformLerp");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr);
		NativeFieldInfoPtr__transform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_transform");
		NativeFieldInfoPtr__min = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_min");
		NativeFieldInfoPtr__max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_max");
		NativeFieldInfoPtr__lerpPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_lerpPosition");
		NativeFieldInfoPtr__lerpRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_lerpRotation");
		NativeFieldInfoPtr__lerpScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_lerpScale");
		NativeFieldInfoPtr__disableOnZero = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_disableOnZero");
		NativeFieldInfoPtr__currentLerpValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, "_currentLerpValue");
		NativeMethodInfoPtr_SetLerpValue_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, 100678220);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr, 100678221);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 227548, RefRangeEnd = 227552, XrefRangeStart = 227533, XrefRangeEnd = 227548, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLerpValue(float lerpValue)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&lerpValue);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLerpValue_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 227552, XrefRangeEnd = 227553, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TransformLerp()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TransformLerp>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TransformLerp(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
