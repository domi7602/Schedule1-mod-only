using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools;

public class DuplicateEditorMesh : DTVersionedMonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_mFilter;

	private static readonly IntPtr NativeMethodInfoPtr_get_Filter_Public_get_MeshFilter_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe MeshFilter mFilter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mFilter);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mFilter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter));
		}
	}

	public unsafe MeshFilter Filter
	{
		[CallerCount(17)]
		[CachedScanResults(RefRangeStart = 33131, RefRangeEnd = 33148, XrefRangeStart = 33127, XrefRangeEnd = 33131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Filter_Public_get_MeshFilter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
		}
	}

	static DuplicateEditorMesh()
	{
		Il2CppClassPointerStore<DuplicateEditorMesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DuplicateEditorMesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DuplicateEditorMesh>.NativeClassPtr);
		NativeFieldInfoPtr_mFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DuplicateEditorMesh>.NativeClassPtr, "mFilter");
		NativeMethodInfoPtr_get_Filter_Public_get_MeshFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateEditorMesh>.NativeClassPtr, 100664280);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DuplicateEditorMesh>.NativeClassPtr, 100664281);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DuplicateEditorMesh()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DuplicateEditorMesh>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DuplicateEditorMesh(IntPtr pointer)
		: base(pointer)
	{
	}
}
