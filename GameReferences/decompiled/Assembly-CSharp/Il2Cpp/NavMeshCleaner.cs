using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2Cpp;

public class NavMeshCleaner : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_m_WalkablePoint;

	private static readonly IntPtr NativeFieldInfoPtr_m_Height;

	private static readonly IntPtr NativeFieldInfoPtr_m_Offset;

	private static readonly IntPtr NativeFieldInfoPtr_m_MidLayerCount;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<Vector3> m_WalkablePoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_WalkablePoint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_WalkablePoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe float m_Height
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Height);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Height)) = num;
		}
	}

	public unsafe float m_Offset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Offset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Offset)) = num;
		}
	}

	public unsafe int m_MidLayerCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MidLayerCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MidLayerCount)) = num;
		}
	}

	static NavMeshCleaner()
	{
		Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "NavMeshCleaner");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr);
		NativeFieldInfoPtr_m_WalkablePoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr, "m_WalkablePoint");
		NativeFieldInfoPtr_m_Height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr, "m_Height");
		NativeFieldInfoPtr_m_Offset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr, "m_Offset");
		NativeFieldInfoPtr_m_MidLayerCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr, "m_MidLayerCount");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr, 100663502);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66837, XrefRangeEnd = 66845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NavMeshCleaner()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NavMeshCleaner>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NavMeshCleaner(IntPtr pointer)
		: base(pointer)
	{
	}
}
