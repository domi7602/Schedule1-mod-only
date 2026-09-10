using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Graffiti;

public class PixelData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Coordinate;

	private static readonly System.IntPtr NativeFieldInfoPtr_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StrokeRadiusRoundedUp_Public_get_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StrokeRadiusRoundedDown_Public_get_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_UShort2_ESprayColor_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPixelStrength_Public_Single_Int32_0;

	public unsafe UShort2 Coordinate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Coordinate);
			return *(UShort2*)num;
		}
		set
		{
			*(UShort2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Coordinate)) = uShort;
		}
	}

	public unsafe ESprayColor Color
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Color);
			return *(ESprayColor*)num;
		}
		set
		{
			*(ESprayColor*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Color)) = eSprayColor;
		}
	}

	public unsafe byte StrokeSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StrokeSize);
			return *(byte*)num;
		}
		set
		{
			*(byte*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StrokeSize)) = b;
		}
	}

	public unsafe byte StrokeRadiusRoundedUp
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169511, XrefRangeEnd = 169515, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StrokeRadiusRoundedUp_Public_get_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(byte*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe byte StrokeRadiusRoundedDown
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 169519, RefRangeEnd = 169521, XrefRangeStart = 169515, XrefRangeEnd = 169519, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StrokeRadiusRoundedDown_Public_get_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(byte*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PixelData()
	{
		Il2CppClassPointerStore<PixelData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Graffiti", "PixelData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PixelData>.NativeClassPtr);
		NativeFieldInfoPtr_Coordinate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PixelData>.NativeClassPtr, "Coordinate");
		NativeFieldInfoPtr_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PixelData>.NativeClassPtr, "Color");
		NativeFieldInfoPtr_StrokeSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PixelData>.NativeClassPtr, "StrokeSize");
		NativeMethodInfoPtr_get_StrokeRadiusRoundedUp_Public_get_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PixelData>.NativeClassPtr, 100672773);
		NativeMethodInfoPtr_get_StrokeRadiusRoundedDown_Public_get_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PixelData>.NativeClassPtr, 100672774);
		NativeMethodInfoPtr__ctor_Public_Void_UShort2_ESprayColor_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PixelData>.NativeClassPtr, 100672775);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PixelData>.NativeClassPtr, 100672776);
		NativeMethodInfoPtr_GetPixelStrength_Public_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PixelData>.NativeClassPtr, 100672777);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 169522, RefRangeEnd = 169525, XrefRangeStart = 169521, XrefRangeEnd = 169522, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PixelData(UShort2 coordinate, ESprayColor color, byte strokeSize)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PixelData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&coordinate);
		*(ESprayColor**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &color;
		*(byte**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &strokeSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_UShort2_ESprayColor_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169525, XrefRangeEnd = 169534, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 169540, RefRangeEnd = 169541, XrefRangeStart = 169534, XrefRangeEnd = 169540, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetPixelStrength(int pixelIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&pixelIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPixelStrength_Public_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PixelData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
