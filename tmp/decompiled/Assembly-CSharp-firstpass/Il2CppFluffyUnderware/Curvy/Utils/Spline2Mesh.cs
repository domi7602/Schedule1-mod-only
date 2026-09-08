using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Utils;

public class Spline2Mesh : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Lines;

	private static readonly System.IntPtr NativeFieldInfoPtr_Winding;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVTiling;

	private static readonly System.IntPtr NativeFieldInfoPtr_UVOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_SuppressUVMapping;

	private static readonly System.IntPtr NativeFieldInfoPtr_UV2;

	private static readonly System.IntPtr NativeFieldInfoPtr_MeshName;

	private static readonly System.IntPtr NativeFieldInfoPtr_VertexLineOnly;

	private static readonly System.IntPtr NativeFieldInfoPtr__Error_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_mTess;

	private static readonly System.IntPtr NativeFieldInfoPtr_mMesh;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Error_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Error_Private_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Boolean_byref_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_triangulate_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_polyLineIsValid_Private_Static_Boolean_SplinePolyLine_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<SplinePolyLine> Lines
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Lines);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SplinePolyLine>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Lines)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe WindingRule Winding
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Winding);
			return *(WindingRule*)num;
		}
		set
		{
			*(WindingRule*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Winding)) = windingRule;
		}
	}

	public unsafe Vector2 UVTiling
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVTiling);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVTiling)) = vector;
		}
	}

	public unsafe Vector2 UVOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVOffset);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UVOffset)) = vector;
		}
	}

	public unsafe bool SuppressUVMapping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SuppressUVMapping);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SuppressUVMapping)) = flag;
		}
	}

	public unsafe bool UV2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UV2);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UV2)) = flag;
		}
	}

	public unsafe string MeshName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MeshName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MeshName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool VertexLineOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VertexLineOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VertexLineOnly)) = flag;
		}
	}

	public unsafe string _Error_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Error_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Error_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Tess mTess
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mTess);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Tess>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mTess)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tess));
		}
	}

	public unsafe Mesh mMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mMesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
		}
	}

	public unsafe string Error
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30474, RefRangeEnd = 30475, XrefRangeStart = 30474, XrefRangeEnd = 30475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Error_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43857, XrefRangeEnd = 43858, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Error_Private_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Spline2Mesh()
	{
		Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Utils", "Spline2Mesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr);
		NativeFieldInfoPtr_Lines = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "Lines");
		NativeFieldInfoPtr_Winding = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "Winding");
		NativeFieldInfoPtr_UVTiling = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "UVTiling");
		NativeFieldInfoPtr_UVOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "UVOffset");
		NativeFieldInfoPtr_SuppressUVMapping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "SuppressUVMapping");
		NativeFieldInfoPtr_UV2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "UV2");
		NativeFieldInfoPtr_MeshName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "MeshName");
		NativeFieldInfoPtr_VertexLineOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "VertexLineOnly");
		NativeFieldInfoPtr__Error_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "<Error>k__BackingField");
		NativeFieldInfoPtr_mTess = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "mTess");
		NativeFieldInfoPtr_mMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, "mMesh");
		NativeMethodInfoPtr_get_Error_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, 100665319);
		NativeMethodInfoPtr_set_Error_Private_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, 100665320);
		NativeMethodInfoPtr_Apply_Public_Boolean_byref_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, 100665321);
		NativeMethodInfoPtr_triangulate_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, 100665322);
		NativeMethodInfoPtr_polyLineIsValid_Private_Static_Boolean_SplinePolyLine_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, 100665323);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr, 100665324);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43934, RefRangeEnd = 43935, XrefRangeStart = 43858, XrefRangeEnd = 43934, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Apply(out Mesh result)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		nint num = 0;
		*ptr = (nint)(&num);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Apply_Public_Boolean_byref_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num2 = num;
		result = ((num2 == 0) ? null : new Mesh(num2));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 43986, RefRangeEnd = 43987, XrefRangeStart = 43935, XrefRangeEnd = 43986, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool triangulate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_triangulate_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool polyLineIsValid(SplinePolyLine pl)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pl);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_polyLineIsValid_Private_Static_Boolean_SplinePolyLine_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43987, XrefRangeEnd = 44002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Spline2Mesh()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Spline2Mesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Spline2Mesh(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
