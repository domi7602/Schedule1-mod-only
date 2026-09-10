using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;

namespace Il2CppScheduleOne.Graffiti;

[System.Serializable]
public class SprayStroke : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MinStrokeLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_AngleThreshold_Degrees;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxStrokeDeviation;

	private static readonly System.IntPtr NativeFieldInfoPtr_ForwardSampleCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_LegacyDefault;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_Small;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_Medium;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_Large;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_ExtraLarge;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSizePresets;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_Min;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize_Max;

	private static readonly System.IntPtr NativeFieldInfoPtr_Start;

	private static readonly System.IntPtr NativeFieldInfoPtr_End;

	private static readonly System.IntPtr NativeFieldInfoPtr_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_StrokeSize;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_UShort2_UShort2_ESprayColor_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_SprayStroke_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPixelsFromStroke_Public_List_1_PixelData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetStrokesFromPixels_Public_Static_List_1_SprayStroke_List_1_UShort2_ESprayColor_Byte_SpraySurface_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Serialize_Public_Void_BinaryWriter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Deserialize_Public_Static_SprayStroke_BinaryReader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyAndShiftStrokes_Public_Static_List_1_SprayStroke_List_1_SprayStroke_UShort2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBounds_Public_Static_Void_List_1_SprayStroke_byref_UShort2_byref_UShort2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Internal_Static_Void_SprayStroke_byref_UShort2_byref_UShort2_0;

	public unsafe static int MinStrokeLength
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinStrokeLength, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinStrokeLength, (void*)(&num));
		}
	}

	public unsafe static int AngleThreshold_Degrees
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AngleThreshold_Degrees, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AngleThreshold_Degrees, (void*)(&num));
		}
	}

	public unsafe static float MaxStrokeDeviation
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxStrokeDeviation, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxStrokeDeviation, (void*)(&num));
		}
	}

	public unsafe static int ForwardSampleCount
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ForwardSampleCount, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ForwardSampleCount, (void*)(&num));
		}
	}

	public unsafe static byte StrokeSize_LegacyDefault
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_LegacyDefault, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_LegacyDefault, (void*)(&b));
		}
	}

	public unsafe static byte StrokeSize_Small
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_Small, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_Small, (void*)(&b));
		}
	}

	public unsafe static byte StrokeSize_Medium
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_Medium, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_Medium, (void*)(&b));
		}
	}

	public unsafe static byte StrokeSize_Large
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_Large, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_Large, (void*)(&b));
		}
	}

	public unsafe static byte StrokeSize_ExtraLarge
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_ExtraLarge, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_ExtraLarge, (void*)(&b));
		}
	}

	public unsafe static Il2CppStructArray<byte> StrokeSizePresets
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSizePresets, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSizePresets, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static byte StrokeSize_Min
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_Min, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_Min, (void*)(&b));
		}
	}

	public unsafe static byte StrokeSize_Max
	{
		get
		{
			Unsafe.SkipInit(out byte result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_StrokeSize_Max, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_StrokeSize_Max, (void*)(&b));
		}
	}

	public unsafe UShort2 Start
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start);
			return *(UShort2*)num;
		}
		set
		{
			*(UShort2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start)) = uShort;
		}
	}

	public unsafe UShort2 End
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End);
			return *(UShort2*)num;
		}
		set
		{
			*(UShort2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End)) = uShort;
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

	static SprayStroke()
	{
		Il2CppClassPointerStore<SprayStroke>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Graffiti", "SprayStroke");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr);
		NativeFieldInfoPtr_MinStrokeLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "MinStrokeLength");
		NativeFieldInfoPtr_AngleThreshold_Degrees = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "AngleThreshold_Degrees");
		NativeFieldInfoPtr_MaxStrokeDeviation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "MaxStrokeDeviation");
		NativeFieldInfoPtr_ForwardSampleCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "ForwardSampleCount");
		NativeFieldInfoPtr_StrokeSize_LegacyDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_LegacyDefault");
		NativeFieldInfoPtr_StrokeSize_Small = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_Small");
		NativeFieldInfoPtr_StrokeSize_Medium = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_Medium");
		NativeFieldInfoPtr_StrokeSize_Large = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_Large");
		NativeFieldInfoPtr_StrokeSize_ExtraLarge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_ExtraLarge");
		NativeFieldInfoPtr_StrokeSizePresets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSizePresets");
		NativeFieldInfoPtr_StrokeSize_Min = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_Min");
		NativeFieldInfoPtr_StrokeSize_Max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize_Max");
		NativeFieldInfoPtr_Start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "Start");
		NativeFieldInfoPtr_End = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "End");
		NativeFieldInfoPtr_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "Color");
		NativeFieldInfoPtr_StrokeSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, "StrokeSize");
		NativeMethodInfoPtr__ctor_Public_Void_UShort2_UShort2_ESprayColor_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672794);
		NativeMethodInfoPtr_GetCopy_Public_SprayStroke_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672795);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672796);
		NativeMethodInfoPtr_GetPixelsFromStroke_Public_List_1_PixelData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672797);
		NativeMethodInfoPtr_GetStrokesFromPixels_Public_Static_List_1_SprayStroke_List_1_UShort2_ESprayColor_Byte_SpraySurface_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672798);
		NativeMethodInfoPtr_Serialize_Public_Void_BinaryWriter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672799);
		NativeMethodInfoPtr_Deserialize_Public_Static_SprayStroke_BinaryReader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672800);
		NativeMethodInfoPtr_CopyAndShiftStrokes_Public_Static_List_1_SprayStroke_List_1_SprayStroke_UShort2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672801);
		NativeMethodInfoPtr_GetBounds_Public_Static_Void_List_1_SprayStroke_byref_UShort2_byref_UShort2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672802);
		NativeMethodInfoPtr_Method_Internal_Static_Void_SprayStroke_byref_UShort2_byref_UShort2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr, 100672804);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169634, XrefRangeEnd = 169635, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SprayStroke(UShort2 start, UShort2 end, ESprayColor color, byte strokeSize)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&start);
		*(UShort2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &end;
		*(ESprayColor**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &color;
		*(byte**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &strokeSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_UShort2_UShort2_ESprayColor_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169635, XrefRangeEnd = 169639, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SprayStroke GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_SprayStroke_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SprayStroke>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SprayStroke()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SprayStroke>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 169679, RefRangeEnd = 169681, XrefRangeStart = 169639, XrefRangeEnd = 169679, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<PixelData> GetPixelsFromStroke()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPixelsFromStroke_Public_List_1_PixelData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PixelData>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 169728, RefRangeEnd = 169729, XrefRangeStart = 169681, XrefRangeEnd = 169728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<SprayStroke> GetStrokesFromPixels(List<UShort2> coords, ESprayColor color, byte strokeSize, SpraySurface surface)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)coords);
		*(ESprayColor**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &color;
		*(byte**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &strokeSize;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)surface);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetStrokesFromPixels_Public_Static_List_1_SprayStroke_List_1_UShort2_ESprayColor_Byte_SpraySurface_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SprayStroke>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169729, XrefRangeEnd = 169740, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Serialize(BinaryWriter writer)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)writer);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Serialize_Public_Void_BinaryWriter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169740, XrefRangeEnd = 169744, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SprayStroke Deserialize(BinaryReader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Deserialize_Public_Static_SprayStroke_BinaryReader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SprayStroke>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 169773, RefRangeEnd = 169775, XrefRangeStart = 169744, XrefRangeEnd = 169773, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<SprayStroke> CopyAndShiftStrokes(List<SprayStroke> strokes, UShort2 shift)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)strokes);
		*(UShort2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &shift;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyAndShiftStrokes_Public_Static_List_1_SprayStroke_List_1_SprayStroke_UShort2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SprayStroke>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 169793, RefRangeEnd = 169795, XrefRangeStart = 169775, XrefRangeEnd = 169793, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GetBounds(List<SprayStroke> strokes, out UShort2 min, out UShort2 max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)strokes);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref min);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref max);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBounds_Public_Static_Void_List_1_SprayStroke_byref_UShort2_byref_UShort2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 169592, RefRangeEnd = 169594, XrefRangeStart = 169592, XrefRangeEnd = 169594, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Method_Internal_Static_Void_SprayStroke_byref_UShort2_byref_UShort2_0(SprayStroke stroke, out UShort2 min, out UShort2 max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stroke);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref min);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref max);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Internal_Static_Void_SprayStroke_byref_UShort2_byref_UShort2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SprayStroke(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
