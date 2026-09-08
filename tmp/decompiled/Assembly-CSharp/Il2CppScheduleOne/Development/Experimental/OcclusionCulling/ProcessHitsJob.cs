using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Unity.Collections;
using UnityEngine;

namespace Il2CppScheduleOne.Development.Experimental.OcclusionCulling;

public sealed class ProcessHitsJob : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ObjectCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_RaysPerObject;

	private static readonly System.IntPtr NativeFieldInfoPtr_Hits;

	private static readonly System.IntPtr NativeFieldInfoPtr_VisibilityData;

	private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Final_New_Void_Int32_0;

	public unsafe int ObjectCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectCount)) = num;
		}
	}

	public unsafe int RaysPerObject
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RaysPerObject);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RaysPerObject)) = num;
		}
	}

	public unsafe NativeArray<RaycastHit> Hits
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hits);
			return new NativeArray<RaycastHit>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<NativeArray<RaycastHit>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Hits), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nativeArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<NativeArray<RaycastHit>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe NativeArray<byte> VisibilityData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VisibilityData);
			return new NativeArray<byte>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<NativeArray<byte>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VisibilityData), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nativeArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<NativeArray<byte>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	static ProcessHitsJob()
	{
		Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Development.Experimental.OcclusionCulling", "ProcessHitsJob");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr);
		NativeFieldInfoPtr_ObjectCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr, "ObjectCount");
		NativeFieldInfoPtr_RaysPerObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr, "RaysPerObject");
		NativeFieldInfoPtr_Hits = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr, "Hits");
		NativeFieldInfoPtr_VisibilityData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr, "VisibilityData");
		NativeMethodInfoPtr_Execute_Public_Virtual_Final_New_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr, 100686215);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 297614, XrefRangeEnd = 297619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Execute(int cellIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&cellIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Execute_Public_Virtual_Final_New_Void_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ProcessHitsJob(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public ProcessHitsJob()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ProcessHitsJob>.NativeClassPtr))
	{
	}
}
