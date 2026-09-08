using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.ImportExport;

[System.Serializable]
public class SerializedCurvySplineSegment : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Position;

	private static readonly System.IntPtr NativeFieldInfoPtr_Rotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoBakeOrientation;

	private static readonly System.IntPtr NativeFieldInfoPtr_OrientationAnchor;

	private static readonly System.IntPtr NativeFieldInfoPtr_Swirl;

	private static readonly System.IntPtr NativeFieldInfoPtr_SwirlTurns;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoHandles;

	private static readonly System.IntPtr NativeFieldInfoPtr_SynchronizeTCB;

	private static readonly System.IntPtr NativeFieldInfoPtr_AutoHandleDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_HandleOut;

	private static readonly System.IntPtr NativeFieldInfoPtr_HandleIn;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySplineSegment_CurvySerializationSpace_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WriteIntoControlPoint_Public_Void_CurvySplineSegment_CurvySerializationSpace_0;

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

	public unsafe bool AutoBakeOrientation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoBakeOrientation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoBakeOrientation)) = flag;
		}
	}

	public unsafe bool OrientationAnchor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OrientationAnchor);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OrientationAnchor)) = flag;
		}
	}

	public unsafe CurvyOrientationSwirl Swirl
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Swirl);
			return *(CurvyOrientationSwirl*)num;
		}
		set
		{
			*(CurvyOrientationSwirl*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Swirl)) = curvyOrientationSwirl;
		}
	}

	public unsafe float SwirlTurns
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SwirlTurns);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SwirlTurns)) = num;
		}
	}

	public unsafe bool AutoHandles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoHandles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AutoHandles)) = flag;
		}
	}

	public unsafe bool SynchronizeTCB
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SynchronizeTCB);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SynchronizeTCB)) = flag;
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

	public unsafe Vector3 HandleOut
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandleOut);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandleOut)) = vector;
		}
	}

	public unsafe Vector3 HandleIn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandleIn);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandleIn)) = vector;
		}
	}

	static SerializedCurvySplineSegment()
	{
		Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ImportExport", "SerializedCurvySplineSegment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr);
		NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "Position");
		NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "Rotation");
		NativeFieldInfoPtr_AutoBakeOrientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "AutoBakeOrientation");
		NativeFieldInfoPtr_OrientationAnchor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "OrientationAnchor");
		NativeFieldInfoPtr_Swirl = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "Swirl");
		NativeFieldInfoPtr_SwirlTurns = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "SwirlTurns");
		NativeFieldInfoPtr_AutoHandles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "AutoHandles");
		NativeFieldInfoPtr_SynchronizeTCB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "SynchronizeTCB");
		NativeFieldInfoPtr_AutoHandleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "AutoHandleDistance");
		NativeFieldInfoPtr_HandleOut = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "HandleOut");
		NativeFieldInfoPtr_HandleIn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, "HandleIn");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, 100665337);
		NativeMethodInfoPtr__ctor_Public_Void_CurvySplineSegment_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, 100665338);
		NativeMethodInfoPtr_WriteIntoControlPoint_Public_Void_CurvySplineSegment_CurvySerializationSpace_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr, 100665339);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 44174, RefRangeEnd = 44177, XrefRangeStart = 44168, XrefRangeEnd = 44174, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SerializedCurvySplineSegment()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 44188, RefRangeEnd = 44189, XrefRangeStart = 44177, XrefRangeEnd = 44188, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SerializedCurvySplineSegment(CurvySplineSegment segment, CurvySerializationSpace space)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SerializedCurvySplineSegment>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySplineSegment_CurvySerializationSpace_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 44224, RefRangeEnd = 44225, XrefRangeStart = 44189, XrefRangeEnd = 44224, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void WriteIntoControlPoint(CurvySplineSegment controlPoint, CurvySerializationSpace space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(CurvySerializationSpace**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteIntoControlPoint_Public_Void_CurvySplineSegment_CurvySerializationSpace_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SerializedCurvySplineSegment(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
