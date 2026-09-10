using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Il2CppEPOOutline;

public class OutlineParameters : Il2CppSystem.Object
{
	[System.Serializable]
	[ObfuscatedName("EPOOutline.OutlineParameters+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__30_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__Prepare_b__30_0_Internal_Boolean_Outlinable_0;

		public unsafe static __c __9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<__c>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c));
			}
		}

		public unsafe static Il2CppSystem.Predicate<Outlinable> __9__30_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__30_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Predicate<Outlinable>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__30_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)predicate));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__30_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__30_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667381);
			NativeMethodInfoPtr__Prepare_b__30_0_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667382);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63828, XrefRangeEnd = 63832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _Prepare_b__30_0(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Prepare_b__30_0_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MeshPool;

	private static readonly System.IntPtr NativeFieldInfoPtr_Camera;

	private static readonly System.IntPtr NativeFieldInfoPtr_Target;

	private static readonly System.IntPtr NativeFieldInfoPtr_DepthTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr_Buffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateIterations;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlurIterations;

	private static readonly System.IntPtr NativeFieldInfoPtr_Scale;

	private static readonly System.IntPtr NativeFieldInfoPtr_CustomViewport;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutlineLayerMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_TargetWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr_TargetHeight;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlurShift;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateShift;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseHDR;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseInfoBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_IsEditorCamera;

	private static readonly System.IntPtr NativeFieldInfoPtr_PrimaryBufferSizeMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_PrimaryBufferSizeReference;

	private static readonly System.IntPtr NativeFieldInfoPtr_PrimaryBufferScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_EyeMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_Antialiasing;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlurType;

	private static readonly System.IntPtr NativeFieldInfoPtr_Mask;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlitMesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutlinablesToRender;

	private static readonly System.IntPtr NativeFieldInfoPtr_isInitialized;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeScaledVector_Public_Vector2Int_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckInitialization_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Prepare_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckDiffers_Private_Static_Boolean_Outlinable_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckIfNonOne_Private_Static_Boolean_OutlineProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe MeshPool MeshPool
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MeshPool);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshPool>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MeshPool)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshPool));
		}
	}

	public unsafe Camera Camera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Camera);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Camera>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Camera)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera));
		}
	}

	public unsafe RenderTargetIdentifier Target
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Target);
			return *(RenderTargetIdentifier*)num;
		}
		set
		{
			*(RenderTargetIdentifier*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Target)) = renderTargetIdentifier;
		}
	}

	public unsafe RenderTargetIdentifier DepthTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DepthTarget);
			return *(RenderTargetIdentifier*)num;
		}
		set
		{
			*(RenderTargetIdentifier*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DepthTarget)) = renderTargetIdentifier;
		}
	}

	public unsafe CommandBuffer Buffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Buffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CommandBuffer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Buffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)commandBuffer));
		}
	}

	public unsafe DilateQuality DilateQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateQuality);
			return *(DilateQuality*)num;
		}
		set
		{
			*(DilateQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateQuality)) = dilateQuality;
		}
	}

	public unsafe int DilateIterations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateIterations);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateIterations)) = num;
		}
	}

	public unsafe int BlurIterations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlurIterations);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlurIterations)) = num;
		}
	}

	public unsafe Vector2 Scale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Scale);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Scale)) = vector;
		}
	}

	public unsafe Il2CppSystem.Nullable<Rect> CustomViewport
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomViewport);
			return new Il2CppSystem.Nullable<Rect>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<Rect>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomViewport), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<Rect>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe long OutlineLayerMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutlineLayerMask);
			return *(long*)num;
		}
		set
		{
			*(long*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutlineLayerMask)) = num;
		}
	}

	public unsafe int TargetWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetWidth);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetWidth)) = num;
		}
	}

	public unsafe int TargetHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetHeight);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetHeight)) = num;
		}
	}

	public unsafe float BlurShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlurShift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlurShift)) = num;
		}
	}

	public unsafe float DilateShift
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateShift);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DilateShift)) = num;
		}
	}

	public unsafe bool UseHDR
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseHDR);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseHDR)) = flag;
		}
	}

	public unsafe bool UseInfoBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseInfoBuffer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseInfoBuffer)) = flag;
		}
	}

	public unsafe bool IsEditorCamera
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsEditorCamera);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsEditorCamera)) = flag;
		}
	}

	public unsafe BufferSizeMode PrimaryBufferSizeMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryBufferSizeMode);
			return *(BufferSizeMode*)num;
		}
		set
		{
			*(BufferSizeMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryBufferSizeMode)) = bufferSizeMode;
		}
	}

	public unsafe int PrimaryBufferSizeReference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryBufferSizeReference);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryBufferSizeReference)) = num;
		}
	}

	public unsafe float PrimaryBufferScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryBufferScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryBufferScale)) = num;
		}
	}

	public unsafe StereoTargetEyeMask EyeMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyeMask);
			return *(StereoTargetEyeMask*)num;
		}
		set
		{
			*(StereoTargetEyeMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EyeMask)) = stereoTargetEyeMask;
		}
	}

	public unsafe int Antialiasing
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Antialiasing);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Antialiasing)) = num;
		}
	}

	public unsafe BlurType BlurType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlurType);
			return *(BlurType*)num;
		}
		set
		{
			*(BlurType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlurType)) = blurType;
		}
	}

	public unsafe LayerMask Mask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mask)) = layerMask;
		}
	}

	public unsafe Mesh BlitMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlitMesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BlitMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe List<Outlinable> OutlinablesToRender
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutlinablesToRender);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Outlinable>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutlinablesToRender)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool isInitialized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInitialized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInitialized)) = flag;
		}
	}

	static OutlineParameters()
	{
		Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "OutlineParameters");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr);
		NativeFieldInfoPtr_MeshPool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "MeshPool");
		NativeFieldInfoPtr_Camera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "Camera");
		NativeFieldInfoPtr_Target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "Target");
		NativeFieldInfoPtr_DepthTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "DepthTarget");
		NativeFieldInfoPtr_Buffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "Buffer");
		NativeFieldInfoPtr_DilateQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "DilateQuality");
		NativeFieldInfoPtr_DilateIterations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "DilateIterations");
		NativeFieldInfoPtr_BlurIterations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "BlurIterations");
		NativeFieldInfoPtr_Scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "Scale");
		NativeFieldInfoPtr_CustomViewport = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "CustomViewport");
		NativeFieldInfoPtr_OutlineLayerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "OutlineLayerMask");
		NativeFieldInfoPtr_TargetWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "TargetWidth");
		NativeFieldInfoPtr_TargetHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "TargetHeight");
		NativeFieldInfoPtr_BlurShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "BlurShift");
		NativeFieldInfoPtr_DilateShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "DilateShift");
		NativeFieldInfoPtr_UseHDR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "UseHDR");
		NativeFieldInfoPtr_UseInfoBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "UseInfoBuffer");
		NativeFieldInfoPtr_IsEditorCamera = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "IsEditorCamera");
		NativeFieldInfoPtr_PrimaryBufferSizeMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "PrimaryBufferSizeMode");
		NativeFieldInfoPtr_PrimaryBufferSizeReference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "PrimaryBufferSizeReference");
		NativeFieldInfoPtr_PrimaryBufferScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "PrimaryBufferScale");
		NativeFieldInfoPtr_EyeMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "EyeMask");
		NativeFieldInfoPtr_Antialiasing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "Antialiasing");
		NativeFieldInfoPtr_BlurType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "BlurType");
		NativeFieldInfoPtr_Mask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "Mask");
		NativeFieldInfoPtr_BlitMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "BlitMesh");
		NativeFieldInfoPtr_OutlinablesToRender = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "OutlinablesToRender");
		NativeFieldInfoPtr_isInitialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, "isInitialized");
		NativeMethodInfoPtr_MakeScaledVector_Public_Vector2Int_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, 100667374);
		NativeMethodInfoPtr_CheckInitialization_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, 100667375);
		NativeMethodInfoPtr_Prepare_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, 100667376);
		NativeMethodInfoPtr_CheckDiffers_Private_Static_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, 100667377);
		NativeMethodInfoPtr_CheckIfNonOne_Private_Static_Boolean_OutlineProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, 100667378);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr, 100667379);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63832, XrefRangeEnd = 63840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2Int MakeScaledVector(int x, int y)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&x);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeScaledVector_Public_Vector2Int_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2Int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 63845, RefRangeEnd = 63847, XrefRangeStart = 63840, XrefRangeEnd = 63845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckInitialization()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckInitialization_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 63887, RefRangeEnd = 63889, XrefRangeStart = 63847, XrefRangeEnd = 63887, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Prepare()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Prepare_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 63889, RefRangeEnd = 63890, XrefRangeStart = 63889, XrefRangeEnd = 63889, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool CheckDiffers(Outlinable outlinable)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outlinable);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckDiffers_Private_Static_Boolean_Outlinable_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool CheckIfNonOne(Outlinable.OutlineProperties parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckIfNonOne_Private_Static_Boolean_OutlineProperties_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 63920, RefRangeEnd = 63922, XrefRangeStart = 63890, XrefRangeEnd = 63920, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OutlineParameters()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutlineParameters>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public OutlineParameters(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
