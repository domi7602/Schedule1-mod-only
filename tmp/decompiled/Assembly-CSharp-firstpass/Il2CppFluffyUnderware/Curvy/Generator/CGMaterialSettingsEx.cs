using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[Serializable]
public class CGMaterialSettingsEx : CGMaterialSettings
{
	private static readonly IntPtr NativeFieldInfoPtr_MaterialID;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int MaterialID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialID)) = num;
		}
	}

	static CGMaterialSettingsEx()
	{
		Il2CppClassPointerStore<CGMaterialSettingsEx>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGMaterialSettingsEx");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGMaterialSettingsEx>.NativeClassPtr);
		NativeFieldInfoPtr_MaterialID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGMaterialSettingsEx>.NativeClassPtr, "MaterialID");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGMaterialSettingsEx>.NativeClassPtr, 100665844);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 47253, RefRangeEnd = 47260, XrefRangeStart = 47253, XrefRangeEnd = 47260, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGMaterialSettingsEx()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGMaterialSettingsEx>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGMaterialSettingsEx(IntPtr pointer)
		: base(pointer)
	{
	}
}
