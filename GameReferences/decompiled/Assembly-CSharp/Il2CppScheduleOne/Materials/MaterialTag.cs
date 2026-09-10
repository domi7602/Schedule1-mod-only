using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Core;
using UnityEngine;

namespace Il2CppScheduleOne.Materials;

public class MaterialTag : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_MaterialType;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EMaterialType MaterialType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialType);
			return *(EMaterialType*)num;
		}
		set
		{
			*(EMaterialType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialType)) = eMaterialType;
		}
	}

	static MaterialTag()
	{
		Il2CppClassPointerStore<MaterialTag>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Materials", "MaterialTag");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialTag>.NativeClassPtr);
		NativeFieldInfoPtr_MaterialType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialTag>.NativeClassPtr, "MaterialType");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialTag>.NativeClassPtr, 100669912);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MaterialTag()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialTag>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MaterialTag(IntPtr pointer)
		: base(pointer)
	{
	}
}
