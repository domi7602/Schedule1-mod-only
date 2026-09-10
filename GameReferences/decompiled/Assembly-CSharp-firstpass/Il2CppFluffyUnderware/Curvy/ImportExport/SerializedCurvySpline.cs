using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.ImportExport;

[System.Serializable]
public class SerializedCurvySpline : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Name;

	private static readonly System.IntPtr NativeFieldInfoPtr_Position;

	private static readonly System.IntPtr NativeFieldInfoPtr_Rotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_Interpolation;

	private static readonly System.IntPtr NativeFieldInfoPtr_RestrictTo2D;

	private static readonly System.IntPtr NativeFieldInfoPtr_Closed;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoEndTangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_Orientation;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoHandleDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_CacheDensity;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxPointsPerUnit;

	private static readonly System.IntPtr NativeFieldInfoPtr_UsePooling;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseThreading;

	private static readonly System.IntPtr NativeFieldInfoPtr_CheckTransform;

	private static readonly System.IntPtr NativeFieldInfoPtr_UpdateIn;

	private static readonly System.IntPtr NativeFieldInfoPtr_IsBSplineClamped;

	private static readonly System.IntPtr NativeFieldInfoPtr_BSplineDegree;

	private static readonly System.IntPtr NativeFieldInfoPtr_ControlPoints;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_CurvySerializationSpace_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WriteIntoSpline_Public_Void_CurvySpline_CurvySerializationSpace_0;

	public unsafe string Name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Vector3 Position
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position)) = vector;
		}
	}

	public unsafe Vector3 Rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation)) = vector;
		}
	}

	public unsafe CurvyInterpolation Interpolation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interpolation);
			return *(CurvyInterpolation*)num;
		}
		set
		{
			*(CurvyInterpolation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interpolation)) = curvyInterpolation;
		}
	}

	public unsafe bool RestrictTo2D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RestrictTo2D);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RestrictTo2D)) = flag;
		}
	}

	public unsafe bool Closed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Closed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Closed)) = flag;
		}
	}

	public unsafe bool AutoEndTangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoEndTangents);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoEndTangents)) = flag;
		}
	}

	public unsafe CurvyOrientation Orientation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Orientation);
			return *(CurvyOrientation*)num;
		}
		set
		{
			*(CurvyOrientation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Orientation)) = curvyOrientation;
		}
	}

	public unsafe float AutoHandleDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoHandleDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoHandleDistance)) = num;
		}
	}

	public unsafe int CacheDensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CacheDensity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CacheDensity)) = num;
		}
	}

	public unsafe float MaxPointsPerUnit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxPointsPerUnit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxPointsPerUnit)) = num;
		}
	}

	public unsafe bool UsePooling
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UsePooling);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UsePooling)) = flag;
		}
	}

	public unsafe bool UseThreading
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseThreading);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseThreading)) = flag;
		}
	}

	public unsafe bool CheckTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CheckTransform);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CheckTransform)) = flag;
		}
	}

	public unsafe CurvyUpdateMethod UpdateIn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateIn);
			return *(CurvyUpdateMethod*)num;
		}
		set
		{
			*(CurvyUpdateMethod*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateIn)) = curvyUpdateMethod;
		}
	}

	public unsafe bool IsBSplineClamped
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsBSplineClamped);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsBSplineClamped)) = flag;
		}
	}

	public unsafe int BSplineDegree
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BSplineDegree);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BSplineDegree)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<SerializedCurvySplineSegment> ControlPoints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPoints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SerializedCurvySplineSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPoints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static SerializedCurvySpline()
	{
		Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ImportExport", "SerializedCurvySpline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr);
		NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "Name");
		NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "Position");
		NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "Rotation");
		NativeFieldInfoPtr_Interpolation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "Interpolation");
		NativeFieldInfoPtr_RestrictTo2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "RestrictTo2D");
		NativeFieldInfoPtr_Closed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "Closed");
		NativeFieldInfoPtr_AutoEndTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "AutoEndTangents");
		NativeFieldInfoPtr_Orientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "Orientation");
		NativeFieldInfoPtr_AutoHandleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "AutoHandleDistance");
		NativeFieldInfoPtr_CacheDensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "CacheDensity");
		NativeFieldInfoPtr_MaxPointsPerUnit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "MaxPointsPerUnit");
		NativeFieldInfoPtr_UsePooling = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "UsePooling");
		NativeFieldInfoPtr_UseThreading = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "UseThreading");
		NativeFieldInfoPtr_CheckTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "CheckTransform");
		NativeFieldInfoPtr_UpdateIn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "UpdateIn");
		NativeFieldInfoPtr_IsBSplineClamped = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "IsBSplineClamped");
		NativeFieldInfoPtr_BSplineDegree = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "BSplineDegree");
		NativeFieldInfoPtr_ControlPoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, "ControlPoints");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, 100665334);
		NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, 100665335);
		NativeMethodInfoPtr_WriteIntoSpline_Public_Void_CurvySpline_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr, 100665336);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 44103, RefRangeEnd = 44104, XrefRangeStart = 44094, XrefRangeEnd = 44103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SerializedCurvySpline()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 44135, RefRangeEnd = 44136, XrefRangeStart = 44104, XrefRangeEnd = 44135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SerializedCurvySpline(CurvySpline spline, CurvySerializationSpace space)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SerializedCurvySpline>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_CurvySerializationSpace_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 44164, RefRangeEnd = 44168, XrefRangeStart = 44136, XrefRangeEnd = 44164, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void WriteIntoSpline(CurvySpline deserializedSpline, CurvySerializationSpace space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deserializedSpline);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteIntoSpline_Public_Void_CurvySpline_CurvySerializationSpace_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SerializedCurvySpline(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
