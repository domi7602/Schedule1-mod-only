using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Instancing;

public class InstancingBaker : MonoBehaviour
{
	public class InstanceObjectBakeData : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Position;

		private static readonly System.IntPtr NativeFieldInfoPtr_Rotation;

		private static readonly System.IntPtr NativeFieldInfoPtr_Scale;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Vector3 Position
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position)) = vector;
			}
		}

		public unsafe Vector4 Rotation
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation);
				return *(Vector4*)num;
			}
			set
			{
				*(Vector4*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation)) = vector;
			}
		}

		public unsafe float Scale
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Scale);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Scale)) = num;
			}
		}

		static InstanceObjectBakeData()
		{
			Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "InstanceObjectBakeData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr);
			NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr, "Position");
			NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr, "Rotation");
			NativeFieldInfoPtr_Scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr, "Scale");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr, 100672401);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe InstanceObjectBakeData()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InstanceObjectBakeData>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public InstanceObjectBakeData(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__objects;

	private static readonly System.IntPtr NativeFieldInfoPtr__textureResolution;

	private static readonly System.IntPtr NativeFieldInfoPtr__fileName;

	private static readonly System.IntPtr NativeFieldInfoPtr__mesh;

	private static readonly System.IntPtr NativeFieldInfoPtr__material;

	private static readonly System.IntPtr NativeFieldInfoPtr_SAVE_PATH;

	private static readonly System.IntPtr NativeMethodInfoPtr_BakeGameObjects_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Bake_Public_Void_List_1_InstanceObjectBakeData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<GameObject> _objects
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__objects);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<GameObject>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__objects)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe int _textureResolution
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureResolution);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureResolution)) = num;
		}
	}

	public unsafe string _fileName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fileName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fileName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Mesh _mesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe Material _material
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__material);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__material)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static string SAVE_PATH
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SAVE_PATH, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SAVE_PATH, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static InstancingBaker()
	{
		Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Instancing", "InstancingBaker");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr);
		NativeFieldInfoPtr__objects = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "_objects");
		NativeFieldInfoPtr__textureResolution = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "_textureResolution");
		NativeFieldInfoPtr__fileName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "_fileName");
		NativeFieldInfoPtr__mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "_mesh");
		NativeFieldInfoPtr__material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "_material");
		NativeFieldInfoPtr_SAVE_PATH = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, "SAVE_PATH");
		NativeMethodInfoPtr_BakeGameObjects_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, 100672398);
		NativeMethodInfoPtr_Bake_Public_Void_List_1_InstanceObjectBakeData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, 100672399);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr, 100672400);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166106, XrefRangeEnd = 166163, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BakeGameObjects()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BakeGameObjects_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166163, XrefRangeEnd = 166169, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Bake(List<InstanceObjectBakeData> bakingData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bakingData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Bake_Public_Void_List_1_InstanceObjectBakeData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166169, XrefRangeEnd = 166174, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InstancingBaker()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InstancingBaker>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InstancingBaker(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
