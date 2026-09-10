using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class RectExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Set_Public_Static_Rect_Rect_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBetween_Public_Static_Rect_Rect_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPosition_Public_Static_Rect_Rect_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPosition_Public_Static_Rect_Rect_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSize_Public_Static_Vector2_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetSize_Public_Static_Rect_Rect_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ScaleBy_Public_Static_Rect_Rect_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ScaleBy_Public_Static_Rect_Rect_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShiftBy_Public_Static_Rect_Rect_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Include_Public_Static_Rect_Rect_Rect_0;

	static RectExt()
	{
		Il2CppClassPointerStore<RectExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "RectExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RectExt>.NativeClassPtr);
		NativeMethodInfoPtr_Set_Public_Static_Rect_Rect_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664385);
		NativeMethodInfoPtr_SetBetween_Public_Static_Rect_Rect_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664386);
		NativeMethodInfoPtr_SetPosition_Public_Static_Rect_Rect_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664387);
		NativeMethodInfoPtr_SetPosition_Public_Static_Rect_Rect_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664388);
		NativeMethodInfoPtr_GetSize_Public_Static_Vector2_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664389);
		NativeMethodInfoPtr_SetSize_Public_Static_Rect_Rect_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664390);
		NativeMethodInfoPtr_ScaleBy_Public_Static_Rect_Rect_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664391);
		NativeMethodInfoPtr_ScaleBy_Public_Static_Rect_Rect_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664392);
		NativeMethodInfoPtr_ShiftBy_Public_Static_Rect_Rect_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664393);
		NativeMethodInfoPtr_Include_Public_Static_Rect_Rect_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectExt>.NativeClassPtr, 100664394);
	}

	[CallerCount(0)]
	public unsafe static Rect Set(this Rect rect, Vector2 pos, Vector2 size)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rect);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &size;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Set_Public_Static_Rect_Rect_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect SetBetween(this Rect rect, Vector2 pos, Vector2 pos2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rect);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBetween_Public_Static_Rect_Rect_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect SetPosition(this Rect rect, Vector2 pos)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rect);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPosition_Public_Static_Rect_Rect_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect SetPosition(this Rect rect, float x, float y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rect);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPosition_Public_Static_Rect_Rect_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34123, RefRangeEnd = 34125, XrefRangeStart = 34123, XrefRangeEnd = 34123, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 GetSize(this Rect rect)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&rect);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSize_Public_Static_Vector2_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect SetSize(this Rect rect, Vector2 size)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rect);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &size;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSize_Public_Static_Rect_Rect_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect ScaleBy(this Rect rect, int pixel)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rect);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pixel;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScaleBy_Public_Static_Rect_Rect_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect ScaleBy(this Rect rect, int x, int y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rect);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScaleBy_Public_Static_Rect_Rect_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect ShiftBy(this Rect rect, int x, int y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rect);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShiftBy_Public_Static_Rect_Rect_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Rect Include(this Rect rect, Rect other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rect);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &other;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Include_Public_Static_Rect_Rect_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public RectExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
