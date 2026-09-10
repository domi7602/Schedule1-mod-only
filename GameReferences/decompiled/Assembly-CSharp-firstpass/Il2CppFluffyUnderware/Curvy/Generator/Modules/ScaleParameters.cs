using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class ScaleParameters : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleReference;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleUniform;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleX;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleY;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleMultiplierX;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleMultiplierY;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_ScaleMode_CGReferenceMode_Boolean_Single_Single_Single_AnimationCurve_AnimationCurve_0;

	public unsafe ScaleMode ScaleMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleMode);
			return *(ScaleMode*)num;
		}
		set
		{
			*(ScaleMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleMode)) = scaleMode;
		}
	}

	public unsafe CGReferenceMode ScaleReference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleReference);
			return *(CGReferenceMode*)num;
		}
		set
		{
			*(CGReferenceMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleReference)) = cGReferenceMode;
		}
	}

	public unsafe bool ScaleUniform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleUniform);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleUniform)) = flag;
		}
	}

	public unsafe float ScaleOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleOffset)) = num;
		}
	}

	public unsafe float ScaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleX)) = num;
		}
	}

	public unsafe float ScaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleY)) = num;
		}
	}

	public unsafe AnimationCurve ScaleMultiplierX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleMultiplierX);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleMultiplierX)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe AnimationCurve ScaleMultiplierY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleMultiplierY);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ScaleMultiplierY)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	static ScaleParameters()
	{
		Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "ScaleParameters");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr);
		NativeFieldInfoPtr_ScaleMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleMode");
		NativeFieldInfoPtr_ScaleReference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleReference");
		NativeFieldInfoPtr_ScaleUniform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleUniform");
		NativeFieldInfoPtr_ScaleOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleOffset");
		NativeFieldInfoPtr_ScaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleX");
		NativeFieldInfoPtr_ScaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleY");
		NativeFieldInfoPtr_ScaleMultiplierX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleMultiplierX");
		NativeFieldInfoPtr_ScaleMultiplierY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, "ScaleMultiplierY");
		NativeMethodInfoPtr__ctor_Public_Void_ScaleMode_CGReferenceMode_Boolean_Single_Single_Single_AnimationCurve_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr, 100666510);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 54796, RefRangeEnd = 54797, XrefRangeStart = 54793, XrefRangeEnd = 54796, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ScaleParameters(ScaleMode scaleMode, CGReferenceMode scaleReference, bool scaleUniform, float scaleOffset, float scaleX, float scaleY, AnimationCurve scaleMultiplierX, AnimationCurve scaleMultiplierY)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ScaleParameters>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = (nint)(&scaleMode);
		*(CGReferenceMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaleReference;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaleUniform;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaleOffset;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaleX;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaleY;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleMultiplierX);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleMultiplierY);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ScaleMode_CGReferenceMode_Boolean_Single_Single_Single_AnimationCurve_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ScaleParameters(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
