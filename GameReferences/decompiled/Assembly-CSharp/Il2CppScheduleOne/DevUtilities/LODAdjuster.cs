using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public class LODAdjuster : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__lodGroup;

	private static readonly IntPtr NativeFieldInfoPtr__rendererName;

	private static readonly IntPtr NativeFieldInfoPtr__lodLevel;

	private static readonly IntPtr NativeMethodInfoPtr_AddToLodGroup_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__AddToLodGroup_b__3_0_Private_Boolean_MeshRenderer_0;

	public unsafe LODGroup _lodGroup
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lodGroup);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<LODGroup>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lodGroup)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lODGroup));
		}
	}

	public unsafe string _rendererName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rendererName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rendererName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int _lodLevel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lodLevel);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lodLevel)) = num;
		}
	}

	static LODAdjuster()
	{
		Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "LODAdjuster");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr);
		NativeFieldInfoPtr__lodGroup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr, "_lodGroup");
		NativeFieldInfoPtr__rendererName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr, "_rendererName");
		NativeFieldInfoPtr__lodLevel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr, "_lodLevel");
		NativeMethodInfoPtr_AddToLodGroup_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr, 100675071);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr, 100675072);
		NativeMethodInfoPtr__AddToLodGroup_b__3_0_Private_Boolean_MeshRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr, 100675073);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194699, XrefRangeEnd = 194759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddToLodGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddToLodGroup_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LODAdjuster()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LODAdjuster>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194759, XrefRangeEnd = 194763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool _AddToLodGroup_b__3_0(MeshRenderer r)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)r);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__AddToLodGroup_b__3_0_Private_Boolean_MeshRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public LODAdjuster(IntPtr pointer)
		: base(pointer)
	{
	}
}
