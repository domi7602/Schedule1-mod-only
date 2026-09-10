using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGDataRequestRasterization : CGDataRequestParameter
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "ModeEnum")]
	public enum ModeEnum
	{
		Even,
		Optimized
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Start;

	private static readonly System.IntPtr NativeFieldInfoPtr_RasterizedRelativeLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_Resolution;

	private static readonly System.IntPtr NativeFieldInfoPtr_AngleThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_Mode;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Int32_Single_ModeEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGDataRequestRasterization_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe float Start
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start)) = num;
		}
	}

	public unsafe float RasterizedRelativeLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RasterizedRelativeLength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RasterizedRelativeLength)) = num;
		}
	}

	public unsafe int Resolution
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Resolution);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Resolution)) = num;
		}
	}

	public unsafe float AngleThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleThreshold)) = num;
		}
	}

	public unsafe ModeEnum Mode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mode);
			return *(ModeEnum*)num;
		}
		set
		{
			*(ModeEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mode)) = modeEnum;
		}
	}

	static CGDataRequestRasterization()
	{
		Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGDataRequestRasterization");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr);
		NativeFieldInfoPtr_Start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, "Start");
		NativeFieldInfoPtr_RasterizedRelativeLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, "RasterizedRelativeLength");
		NativeFieldInfoPtr_Resolution = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, "Resolution");
		NativeFieldInfoPtr_AngleThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, "AngleThreshold");
		NativeFieldInfoPtr_Mode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, "Mode");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Int32_Single_ModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, 100665803);
		NativeMethodInfoPtr__ctor_Public_Void_CGDataRequestRasterization_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, 100665804);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, 100665805);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, 100665806);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr, 100665807);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 46971, RefRangeEnd = 46974, XrefRangeStart = 46967, XrefRangeEnd = 46971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGDataRequestRasterization(float start, float rasterizedRelativeLength, int resolution, float angle, ModeEnum mode = ModeEnum.Even)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&start);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rasterizedRelativeLength;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &resolution;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &angle;
		*(ModeEnum**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Int32_Single_ModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46974, XrefRangeEnd = 46975, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGDataRequestRasterization(CGDataRequestRasterization source)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGDataRequestRasterization>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGDataRequestRasterization_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46975, XrefRangeEnd = 46977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool Equals(Il2CppSystem.Object obj)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46977, XrefRangeEnd = 46984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 47038, RefRangeEnd = 47039, XrefRangeStart = 46984, XrefRangeEnd = 47038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public CGDataRequestRasterization(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
