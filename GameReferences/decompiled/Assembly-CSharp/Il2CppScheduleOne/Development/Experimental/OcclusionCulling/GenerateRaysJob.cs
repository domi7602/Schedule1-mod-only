using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Il2CppScheduleOne.Development.Experimental.OcclusionCulling;

public sealed class GenerateRaysJob : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Origin;

	private static readonly System.IntPtr NativeFieldInfoPtr_CellSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_CellsX;

	private static readonly System.IntPtr NativeFieldInfoPtr_CellsY;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_ObjectCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_RaysPerObject;

	private static readonly System.IntPtr NativeFieldInfoPtr_ObjectCenters;

	private static readonly System.IntPtr NativeFieldInfoPtr_LayerMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_Commands;

	private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Final_New_Void_Int32_0;

	public unsafe float3 Origin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Origin);
			return *(float3*)num;
		}
		set
		{
			*(float3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Origin)) = float5;
		}
	}

	public unsafe float CellSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CellSize);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CellSize)) = num;
		}
	}

	public unsafe int CellsX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CellsX);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CellsX)) = num;
		}
	}

	public unsafe int CellsY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CellsY);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CellsY)) = num;
		}
	}

	public unsafe float MaxDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxDistance)) = num;
		}
	}

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

	public unsafe NativeArray<float3> ObjectCenters
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectCenters);
			return new NativeArray<float3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<NativeArray<float3>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ObjectCenters), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nativeArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<NativeArray<float3>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe int LayerMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LayerMask);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LayerMask)) = num;
		}
	}

	public unsafe NativeArray<RaycastCommand> Commands
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Commands);
			return new NativeArray<RaycastCommand>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<NativeArray<RaycastCommand>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Commands), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nativeArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<NativeArray<RaycastCommand>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	static GenerateRaysJob()
	{
		Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Development.Experimental.OcclusionCulling", "GenerateRaysJob");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr);
		NativeFieldInfoPtr_Origin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "Origin");
		NativeFieldInfoPtr_CellSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "CellSize");
		NativeFieldInfoPtr_CellsX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "CellsX");
		NativeFieldInfoPtr_CellsY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "CellsY");
		NativeFieldInfoPtr_MaxDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "MaxDistance");
		NativeFieldInfoPtr_ObjectCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "ObjectCount");
		NativeFieldInfoPtr_RaysPerObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "RaysPerObject");
		NativeFieldInfoPtr_ObjectCenters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "ObjectCenters");
		NativeFieldInfoPtr_LayerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "LayerMask");
		NativeFieldInfoPtr_Commands = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, "Commands");
		NativeMethodInfoPtr_Execute_Public_Virtual_Final_New_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr, 100686214);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 297613, RefRangeEnd = 297614, XrefRangeStart = 297600, XrefRangeEnd = 297613, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Execute(int cellIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&cellIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Execute_Public_Virtual_Final_New_Void_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GenerateRaysJob(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public GenerateRaysJob()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GenerateRaysJob>.NativeClassPtr))
	{
	}
}
