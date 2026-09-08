using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.UI;

public static class UnitsUtility : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "ERoundingType")]
	public enum ERoundingType
	{
		None,
		Nearest,
		Up,
		Down
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_FormatShortDistance_Public_Static_String_Single_ERoundingType_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FormatSpeed_Public_Static_String_Single_ERoundingType_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RoundValue_Private_Static_Single_Single_ERoundingType_Int32_0;

	static UnitsUtility()
	{
		Il2CppClassPointerStore<UnitsUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "UnitsUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UnitsUtility>.NativeClassPtr);
		NativeMethodInfoPtr_FormatShortDistance_Public_Static_String_Single_ERoundingType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnitsUtility>.NativeClassPtr, 100687496);
		NativeMethodInfoPtr_FormatSpeed_Public_Static_String_Single_ERoundingType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnitsUtility>.NativeClassPtr, 100687497);
		NativeMethodInfoPtr_RoundValue_Private_Static_Single_Single_ERoundingType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnitsUtility>.NativeClassPtr, 100687498);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 309521, RefRangeEnd = 309522, XrefRangeStart = 309501, XrefRangeEnd = 309521, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string FormatShortDistance(float meters, ERoundingType roundingType = ERoundingType.Nearest, int decimalPoints = 0)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&meters);
		*(ERoundingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &roundingType;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimalPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FormatShortDistance_Public_Static_String_Single_ERoundingType_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 309541, RefRangeEnd = 309542, XrefRangeStart = 309522, XrefRangeEnd = 309541, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string FormatSpeed(float metersPerSecond, ERoundingType roundingType = ERoundingType.Nearest, int decimalPoints = 1)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&metersPerSecond);
		*(ERoundingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &roundingType;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimalPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FormatSpeed_Public_Static_String_Single_ERoundingType_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 309544, RefRangeEnd = 309545, XrefRangeStart = 309542, XrefRangeEnd = 309544, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float RoundValue(float value, ERoundingType roundingType, int decimalPoints)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&value);
		*(ERoundingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &roundingType;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimalPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RoundValue_Private_Static_Single_Single_ERoundingType_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public UnitsUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
