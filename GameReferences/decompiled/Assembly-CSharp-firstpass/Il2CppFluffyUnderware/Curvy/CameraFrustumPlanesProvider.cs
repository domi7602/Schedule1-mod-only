using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CameraFrustumPlanesProvider : Il2CppSystem.Object
{
	[System.Serializable]
	[ObfuscatedName("FluffyUnderware.Curvy.CameraFrustumPlanesProvider+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr___cctor_b__13_0_Internal_CameraFrustumPlanesProvider_0;

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

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664482);
			NativeMethodInfoPtr___cctor_b__13_0_Internal_CameraFrustumPlanesProvider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664483);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34783, XrefRangeEnd = 34791, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CameraFrustumPlanesProvider __cctor_b__13_0()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___cctor_b__13_0_Internal_CameraFrustumPlanesProvider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CameraFrustumPlanesProvider>(intPtr) : null;
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_instance;

	private static readonly System.IntPtr NativeFieldInfoPtr_lockObject;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedPlanes;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedForward;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedFov;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedPixelWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedPixelHeight;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Instance_Public_Static_get_CameraFrustumPlanesProvider_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFrustumPlanes_Public_Il2CppStructArray_1_Plane_Camera_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsCacheOutdated_Private_Boolean_Vector3_Vector3_Int32_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Il2CppSystem.Lazy<CameraFrustumPlanesProvider> instance
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_instance, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Lazy<CameraFrustumPlanesProvider>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_instance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lazy));
		}
	}

	public unsafe static Il2CppSystem.Object lockObject
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lockObject, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lockObject, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe Il2CppStructArray<Plane> cachedPlanes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPlanes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Plane>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPlanes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Vector3 cachedPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPosition)) = vector;
		}
	}

	public unsafe Vector3 cachedForward
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedForward);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedForward)) = vector;
		}
	}

	public unsafe float cachedFov
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedFov);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedFov)) = num;
		}
	}

	public unsafe int cachedPixelWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPixelWidth);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPixelWidth)) = num;
		}
	}

	public unsafe int cachedPixelHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPixelHeight);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedPixelHeight)) = num;
		}
	}

	public unsafe static CameraFrustumPlanesProvider Instance
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34791, XrefRangeEnd = 34799, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Instance_Public_Static_get_CameraFrustumPlanesProvider_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CameraFrustumPlanesProvider>(intPtr) : null;
		}
	}

	static CameraFrustumPlanesProvider()
	{
		Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CameraFrustumPlanesProvider");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr);
		NativeFieldInfoPtr_instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "instance");
		NativeFieldInfoPtr_lockObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "lockObject");
		NativeFieldInfoPtr_cachedPlanes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "cachedPlanes");
		NativeFieldInfoPtr_cachedPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "cachedPosition");
		NativeFieldInfoPtr_cachedForward = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "cachedForward");
		NativeFieldInfoPtr_cachedFov = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "cachedFov");
		NativeFieldInfoPtr_cachedPixelWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "cachedPixelWidth");
		NativeFieldInfoPtr_cachedPixelHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, "cachedPixelHeight");
		NativeMethodInfoPtr_get_Instance_Public_Static_get_CameraFrustumPlanesProvider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, 100664476);
		NativeMethodInfoPtr_GetFrustumPlanes_Public_Il2CppStructArray_1_Plane_Camera_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, 100664477);
		NativeMethodInfoPtr_IsCacheOutdated_Private_Boolean_Vector3_Vector3_Int32_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, 100664478);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr, 100664479);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34799, XrefRangeEnd = 34807, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<Plane> GetFrustumPlanes(Camera camera)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFrustumPlanes_Public_Il2CppStructArray_1_Plane_Camera_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Plane>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34807, XrefRangeEnd = 34808, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsCacheOutdated(Vector3 cameraPosition, Vector3 cameraZDirection, int cameraPixelWidth, int cameraPixelHeight, float cameraFieldOfView)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&cameraPosition);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraZDirection;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraPixelWidth;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraPixelHeight;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &cameraFieldOfView;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsCacheOutdated_Private_Boolean_Vector3_Vector3_Int32_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34808, XrefRangeEnd = 34813, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CameraFrustumPlanesProvider()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CameraFrustumPlanesProvider>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CameraFrustumPlanesProvider(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
