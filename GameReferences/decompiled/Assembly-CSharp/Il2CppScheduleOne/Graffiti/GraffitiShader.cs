using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Il2CppScheduleOne.Graffiti;

public class GraffitiShader : Il2CppSystem.Object
{
	[StructLayout(LayoutKind.Explicit)]
	public struct StrokeData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Start;

		private static readonly System.IntPtr NativeFieldInfoPtr_End;

		private static readonly System.IntPtr NativeFieldInfoPtr_Color;

		private static readonly System.IntPtr NativeFieldInfoPtr_Size;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Stride_Public_Static_get_Int32_0;

		[FieldOffset(0)]
		public uint2 Start;

		[FieldOffset(8)]
		public uint2 End;

		[FieldOffset(16)]
		public uint Color;

		[FieldOffset(20)]
		public uint Size;

		public unsafe static int Stride
		{
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 169405, RefRangeEnd = 169406, XrefRangeStart = 169405, XrefRangeEnd = 169405, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Stride_Public_Static_get_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static StrokeData()
		{
			Il2CppClassPointerStore<StrokeData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "StrokeData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StrokeData>.NativeClassPtr);
			NativeFieldInfoPtr_Start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StrokeData>.NativeClassPtr, "Start");
			NativeFieldInfoPtr_End = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StrokeData>.NativeClassPtr, "End");
			NativeFieldInfoPtr_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StrokeData>.NativeClassPtr, "Color");
			NativeFieldInfoPtr_Size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StrokeData>.NativeClassPtr, "Size");
			NativeMethodInfoPtr_get_Stride_Public_Static_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StrokeData>.NativeClassPtr, 100672772);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<StrokeData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__kernal;

	private static readonly System.IntPtr NativeFieldInfoPtr__shader;

	private static readonly System.IntPtr NativeFieldInfoPtr__texture;

	private static readonly System.IntPtr NativeFieldInfoPtr__width;

	private static readonly System.IntPtr NativeFieldInfoPtr__height;

	private static readonly System.IntPtr NativeFieldInfoPtr__strokes;

	private static readonly System.IntPtr NativeFieldInfoPtr__falloffTable;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialise_Public_Void_Texture2D_Int32_Int32_AnimationCurve_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Draw_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearStrokes_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddStrokes_Public_Void_List_1_SprayStroke_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveStrokes_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CreateFalloffTables_Private_Void_Int32_Int32_AnimationCurve_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int _kernal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__kernal);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__kernal)) = num;
		}
	}

	public unsafe ComputeShader _shader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shader);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ComputeShader>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)computeShader));
		}
	}

	public unsafe Texture2D _texture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__texture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__texture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe int _width
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__width);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__width)) = num;
		}
	}

	public unsafe int _height
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__height);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__height)) = num;
		}
	}

	public unsafe List<StrokeData> _strokes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__strokes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<StrokeData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__strokes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppStructArray<float> _falloffTable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__falloffTable);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__falloffTable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static GraffitiShader()
	{
		Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Graffiti", "GraffitiShader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr);
		NativeFieldInfoPtr__kernal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_kernal");
		NativeFieldInfoPtr__shader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_shader");
		NativeFieldInfoPtr__texture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_texture");
		NativeFieldInfoPtr__width = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_width");
		NativeFieldInfoPtr__height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_height");
		NativeFieldInfoPtr__strokes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_strokes");
		NativeFieldInfoPtr__falloffTable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, "_falloffTable");
		NativeMethodInfoPtr_Initialise_Public_Void_Texture2D_Int32_Int32_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672765);
		NativeMethodInfoPtr_Draw_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672766);
		NativeMethodInfoPtr_ClearStrokes_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672767);
		NativeMethodInfoPtr_AddStrokes_Public_Void_List_1_SprayStroke_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672768);
		NativeMethodInfoPtr_RemoveStrokes_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672769);
		NativeMethodInfoPtr_CreateFalloffTables_Private_Void_Int32_Int32_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672770);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr, 100672771);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169406, XrefRangeEnd = 169448, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialise(Texture2D texture, int minStrokeSize, int maxStrokeSize, AnimationCurve falloffCurve)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &minStrokeSize;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxStrokeSize;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)falloffCurve);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialise_Public_Void_Texture2D_Int32_Int32_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169448, XrefRangeEnd = 169465, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Draw()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Draw_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169465, XrefRangeEnd = 169466, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearStrokes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearStrokes_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169466, XrefRangeEnd = 169491, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddStrokes(List<SprayStroke> strokes)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)strokes);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddStrokes_Public_Void_List_1_SprayStroke_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169491, XrefRangeEnd = 169495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveStrokes(int count)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&count);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveStrokes_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169495, XrefRangeEnd = 169503, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CreateFalloffTables(int minFalloff, int maxFalloff, AnimationCurve falloffCurve)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&minFalloff);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxFalloff;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)falloffCurve);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateFalloffTables_Private_Void_Int32_Int32_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 169503, XrefRangeEnd = 169511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GraffitiShader()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GraffitiShader>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GraffitiShader(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
