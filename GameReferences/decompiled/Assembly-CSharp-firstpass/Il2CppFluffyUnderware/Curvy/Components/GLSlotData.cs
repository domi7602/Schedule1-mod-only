using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Components;

[System.Serializable]
public class GLSlotData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Spline;

	private static readonly System.IntPtr NativeFieldInfoPtr_LineColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_VertexData;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetVertexData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Render_Public_Void_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CurvySpline Spline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Spline);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe Color LineColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LineColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LineColor)) = color;
		}
	}

	public unsafe List<Il2CppStructArray<Vector3>> VertexData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VertexData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppStructArray<Vector3>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VertexData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static GLSlotData()
	{
		Il2CppClassPointerStore<GLSlotData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Components", "GLSlotData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr);
		NativeFieldInfoPtr_Spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr, "Spline");
		NativeFieldInfoPtr_LineColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr, "LineColor");
		NativeFieldInfoPtr_VertexData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr, "VertexData");
		NativeMethodInfoPtr_GetVertexData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr, 100665291);
		NativeMethodInfoPtr_Render_Public_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr, 100665292);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr, 100665293);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43636, RefRangeEnd = 43637, XrefRangeStart = 43613, XrefRangeEnd = 43636, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetVertexData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVertexData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43655, RefRangeEnd = 43656, XrefRangeStart = 43637, XrefRangeEnd = 43655, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Render(Material mat)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Render_Public_Void_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43656, XrefRangeEnd = 43668, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GLSlotData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GLSlotData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GLSlotData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
