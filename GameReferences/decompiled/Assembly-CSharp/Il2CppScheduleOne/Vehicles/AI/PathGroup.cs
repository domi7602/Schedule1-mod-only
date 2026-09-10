using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppPathfinding;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Vehicles.AI;

public class PathGroup : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_entryPoint;

	private static readonly System.IntPtr NativeFieldInfoPtr_startToEntryPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_entryToExitPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_exitToDestinationPath;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Vector3 entryPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_entryPoint);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_entryPoint)) = vector;
		}
	}

	public unsafe Path startToEntryPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startToEntryPath);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Path>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startToEntryPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path));
		}
	}

	public unsafe Path entryToExitPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_entryToExitPath);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Path>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_entryToExitPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path));
		}
	}

	public unsafe Path exitToDestinationPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_exitToDestinationPath);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Path>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_exitToDestinationPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path));
		}
	}

	static PathGroup()
	{
		Il2CppClassPointerStore<PathGroup>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "PathGroup");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathGroup>.NativeClassPtr);
		NativeFieldInfoPtr_entryPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathGroup>.NativeClassPtr, "entryPoint");
		NativeFieldInfoPtr_startToEntryPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathGroup>.NativeClassPtr, "startToEntryPath");
		NativeFieldInfoPtr_entryToExitPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathGroup>.NativeClassPtr, "entryToExitPath");
		NativeFieldInfoPtr_exitToDestinationPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathGroup>.NativeClassPtr, "exitToDestinationPath");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathGroup>.NativeClassPtr, 100666421);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PathGroup()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathGroup>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PathGroup(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
