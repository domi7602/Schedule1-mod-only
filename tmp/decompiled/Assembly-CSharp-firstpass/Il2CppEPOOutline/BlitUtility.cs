using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppEPOOutline;

public static class BlitUtility : Il2CppSystem.Object
{
	[StructLayout(LayoutKind.Explicit)]
	public struct MeshSetupResult
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_VertexIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_TriangleIndex;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0;

		[FieldOffset(0)]
		public readonly int VertexIndex;

		[FieldOffset(4)]
		public readonly int TriangleIndex;

		static MeshSetupResult()
		{
			Il2CppClassPointerStore<MeshSetupResult>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "MeshSetupResult");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshSetupResult>.NativeClassPtr);
			NativeFieldInfoPtr_VertexIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshSetupResult>.NativeClassPtr, "VertexIndex");
			NativeFieldInfoPtr_TriangleIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshSetupResult>.NativeClassPtr, "TriangleIndex");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshSetupResult>.NativeClassPtr, 100667261);
		}

		[CallerCount(494)]
		[CachedScanResults(RefRangeStart = 60743, RefRangeEnd = 61237, XrefRangeStart = 60743, XrefRangeEnd = 60743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MeshSetupResult(int vertexIndex, int triangleIndex)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&vertexIndex);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &triangleIndex;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<MeshSetupResult>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Vertex
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Position;

		private static readonly System.IntPtr NativeFieldInfoPtr_Normal;

		[FieldOffset(0)]
		public Vector4 Position;

		[FieldOffset(16)]
		public Vector3 Normal;

		static Vertex()
		{
			Il2CppClassPointerStore<Vertex>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "Vertex");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Vertex>.NativeClassPtr);
			NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "Position");
			NativeFieldInfoPtr_Normal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "Normal");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Vertex>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MainTexHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_normals;

	private static readonly System.IntPtr NativeFieldInfoPtr_tempVertecies;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertexParams;

	private static readonly System.IntPtr NativeFieldInfoPtr_indecies;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertices;

	private static readonly System.IntPtr NativeFieldInfoPtr_matrices;

	private static readonly System.IntPtr NativeFieldInfoPtr_itemsToDraw;

	private static readonly System.IntPtr NativeFieldInfoPtr_supportsInstancing;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SupportsInstancing_Private_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateBounds_Private_Static_Void_Renderer_OutlineTarget_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareForRendering_Public_Static_Void_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupForInstancing_Private_Static_Nullable_1_MeshSetupResult_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupForBruteForce_Private_Static_Nullable_1_MeshSetupResult_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Blit_Public_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_RenderTargetIdentifier_Material_CommandBuffer_Int32_Nullable_1_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Draw_Public_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_Material_Nullable_1_Rect_0;

	public unsafe static int MainTexHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MainTexHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MainTexHash, (void*)(&num));
		}
	}

	public unsafe static Il2CppStructArray<Vector4> normals
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_normals, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector4>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_normals, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<Vector4> tempVertecies
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_tempVertecies, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector4>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_tempVertecies, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<VertexAttributeDescriptor> vertexParams
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_vertexParams, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<VertexAttributeDescriptor>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_vertexParams, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<ushort> indecies
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_indecies, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<ushort>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_indecies, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<Vertex> vertices
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_vertices, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vertex>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_vertices, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static Il2CppStructArray<Matrix4x4> matrices
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_matrices, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Matrix4x4>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_matrices, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static int itemsToDraw
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_itemsToDraw, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_itemsToDraw, (void*)(&num));
		}
	}

	public unsafe static Il2CppSystem.Nullable<bool> supportsInstancing
	{
		get
		{
			System.IntPtr intPtr = (nint)stackalloc byte[(int)(uint)IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<bool>>.NativeClassPtr, ref *(uint*)null)];
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_supportsInstancing, (void*)intPtr);
			return new Il2CppSystem.Nullable<bool>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<bool>>.NativeClassPtr, intPtr));
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_supportsInstancing, (void*)IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)nullable)));
		}
	}

	public unsafe static bool SupportsInstancing
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 61256, RefRangeEnd = 61259, XrefRangeStart = 61237, XrefRangeEnd = 61256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SupportsInstancing_Private_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static BlitUtility()
	{
		Il2CppClassPointerStore<BlitUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "BlitUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr);
		NativeFieldInfoPtr_MainTexHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "MainTexHash");
		NativeFieldInfoPtr_normals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "normals");
		NativeFieldInfoPtr_tempVertecies = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "tempVertecies");
		NativeFieldInfoPtr_vertexParams = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "vertexParams");
		NativeFieldInfoPtr_indecies = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "indecies");
		NativeFieldInfoPtr_vertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "vertices");
		NativeFieldInfoPtr_matrices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "matrices");
		NativeFieldInfoPtr_itemsToDraw = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "itemsToDraw");
		NativeFieldInfoPtr_supportsInstancing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, "supportsInstancing");
		NativeMethodInfoPtr_get_SupportsInstancing_Private_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667253);
		NativeMethodInfoPtr_UpdateBounds_Private_Static_Void_Renderer_OutlineTarget_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667254);
		NativeMethodInfoPtr_PrepareForRendering_Public_Static_Void_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667255);
		NativeMethodInfoPtr_SetupForInstancing_Private_Static_Nullable_1_MeshSetupResult_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667256);
		NativeMethodInfoPtr_SetupForBruteForce_Private_Static_Nullable_1_MeshSetupResult_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667257);
		NativeMethodInfoPtr_Blit_Public_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_RenderTargetIdentifier_Material_CommandBuffer_Int32_Nullable_1_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667258);
		NativeMethodInfoPtr_Draw_Public_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_Material_Nullable_1_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlitUtility>.NativeClassPtr, 100667259);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 61272, RefRangeEnd = 61274, XrefRangeStart = 61259, XrefRangeEnd = 61272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void UpdateBounds(Renderer renderer, OutlineTarget target)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateBounds_Private_Static_Void_Renderer_OutlineTarget_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61318, RefRangeEnd = 61319, XrefRangeStart = 61274, XrefRangeEnd = 61318, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void PrepareForRendering(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareForRendering_Public_Static_Void_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61460, RefRangeEnd = 61461, XrefRangeStart = 61319, XrefRangeEnd = 61460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Nullable<MeshSetupResult> SetupForInstancing(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupForInstancing_Private_Static_Nullable_1_MeshSetupResult_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new Il2CppSystem.Nullable<MeshSetupResult>(pointer);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61611, RefRangeEnd = 61612, XrefRangeStart = 61461, XrefRangeEnd = 61611, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Nullable<MeshSetupResult> SetupForBruteForce(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupForBruteForce_Private_Static_Nullable_1_MeshSetupResult_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new Il2CppSystem.Nullable<MeshSetupResult>(pointer);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 61627, RefRangeEnd = 61636, XrefRangeStart = 61612, XrefRangeEnd = 61627, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Blit(OutlineParameters parameters, RenderTargetIdentifier source, RenderTargetIdentifier destination, RenderTargetIdentifier destinationDepth, Material material, CommandBuffer targetBuffer, int pass = -1, Il2CppSystem.Nullable<Rect> viewport = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &source;
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &destination;
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &destinationDepth;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)targetBuffer);
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &pass;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)viewport));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Blit_Public_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_RenderTargetIdentifier_Material_CommandBuffer_Int32_Nullable_1_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61649, RefRangeEnd = 61650, XrefRangeStart = 61636, XrefRangeEnd = 61649, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Draw(OutlineParameters parameters, RenderTargetIdentifier target, RenderTargetIdentifier depth, Material material, Il2CppSystem.Nullable<Rect> viewport = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &target;
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &depth;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)viewport));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Draw_Public_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_Material_Nullable_1_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BlitUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
