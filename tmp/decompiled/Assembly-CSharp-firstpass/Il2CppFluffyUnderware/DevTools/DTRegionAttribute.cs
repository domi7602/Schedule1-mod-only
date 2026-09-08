using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class DTRegionAttribute : DTPropertyAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_RegionIsOptional;

	private static readonly IntPtr NativeFieldInfoPtr_RegionOptionsPropertyName;

	private static readonly IntPtr NativeFieldInfoPtr_UseSlider;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool RegionIsOptional
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionIsOptional);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionIsOptional)) = flag;
		}
	}

	public unsafe string RegionOptionsPropertyName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionOptionsPropertyName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RegionOptionsPropertyName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool UseSlider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseSlider);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseSlider)) = flag;
		}
	}

	static DTRegionAttribute()
	{
		Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTRegionAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_RegionIsOptional = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr, "RegionIsOptional");
		NativeFieldInfoPtr_RegionOptionsPropertyName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr, "RegionOptionsPropertyName");
		NativeFieldInfoPtr_UseSlider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr, "UseSlider");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr, 100664178);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32282, XrefRangeEnd = 32287, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DTRegionAttribute()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DTRegionAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DTRegionAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
