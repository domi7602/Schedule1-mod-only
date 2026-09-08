using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public static class MeshUtils : Il2CppSystem.Object
{
	public class Pooled<T> : Il2CppSystem.Object where T : Pooled<T>, new()
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__stack;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Abstract_Virtual_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_OnFree_Public_Virtual_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Static_T_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Free_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

		public unsafe static Stack<T> _stack
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__stack, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stack<T>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__stack, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stack));
			}
		}

		static Pooled()
		{
			Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, "Pooled`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr);
			NativeFieldInfoPtr__stack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr, "_stack");
			NativeMethodInfoPtr_Reset_Public_Abstract_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr, 100667128);
			NativeMethodInfoPtr_OnFree_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr, 100667129);
			NativeMethodInfoPtr_Create_Public_Static_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr, 100667130);
			NativeMethodInfoPtr_Free_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr, 100667131);
			NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr, 100667132);
		}

		[CallerCount(0)]
		public unsafe virtual void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Abstract_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void OnFree()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnFree_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(22)]
		[CachedScanResults(RefRangeStart = 59589, RefRangeEnd = 59611, XrefRangeStart = 59577, XrefRangeEnd = 59589, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static T Create()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Create_Public_Static_T_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
		}

		[CallerCount(20)]
		[CachedScanResults(RefRangeStart = 59660, RefRangeEnd = 59680, XrefRangeStart = 59611, XrefRangeEnd = 59660, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Free()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Free_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Pooled()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Pooled<T>>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Pooled(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class Vertex : Pooled<Vertex>
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__prev;

		private static readonly System.IntPtr NativeFieldInfoPtr__next;

		private static readonly System.IntPtr NativeFieldInfoPtr__anEdge;

		private static readonly System.IntPtr NativeFieldInfoPtr__coords;

		private static readonly System.IntPtr NativeFieldInfoPtr__s;

		private static readonly System.IntPtr NativeFieldInfoPtr__t;

		private static readonly System.IntPtr NativeFieldInfoPtr__pqHandle;

		private static readonly System.IntPtr NativeFieldInfoPtr__n;

		private static readonly System.IntPtr NativeFieldInfoPtr__data;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Vertex _prev
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__prev);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Vertex>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__prev)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertex));
			}
		}

		public unsafe Vertex _next
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Vertex>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertex));
			}
		}

		public unsafe Edge _anEdge
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__anEdge);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__anEdge)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Vec3 _coords
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__coords);
				return *(Vec3*)num;
			}
			set
			{
				*(Vec3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__coords)) = vec;
			}
		}

		public unsafe float _s
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__s);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__s)) = num;
			}
		}

		public unsafe float _t
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__t);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__t)) = num;
			}
		}

		public unsafe PQHandle _pqHandle
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pqHandle);
				return *(PQHandle*)num;
			}
			set
			{
				*(PQHandle*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pqHandle)) = pQHandle;
			}
		}

		public unsafe int _n
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__n);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__n)) = num;
			}
		}

		public unsafe Il2CppSystem.Object _data
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		static Vertex()
		{
			Il2CppClassPointerStore<Vertex>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, "Vertex");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Vertex>.NativeClassPtr);
			NativeFieldInfoPtr__prev = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_prev");
			NativeFieldInfoPtr__next = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_next");
			NativeFieldInfoPtr__anEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_anEdge");
			NativeFieldInfoPtr__coords = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_coords");
			NativeFieldInfoPtr__s = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_s");
			NativeFieldInfoPtr__t = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_t");
			NativeFieldInfoPtr__pqHandle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_pqHandle");
			NativeFieldInfoPtr__n = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_n");
			NativeFieldInfoPtr__data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertex>.NativeClassPtr, "_data");
			NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vertex>.NativeClassPtr, 100667133);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vertex>.NativeClassPtr, 100667134);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59680, XrefRangeEnd = 59688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59688, XrefRangeEnd = 59691, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Vertex()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Vertex>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Vertex(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class Face : Pooled<Face>
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__prev;

		private static readonly System.IntPtr NativeFieldInfoPtr__next;

		private static readonly System.IntPtr NativeFieldInfoPtr__anEdge;

		private static readonly System.IntPtr NativeFieldInfoPtr__trail;

		private static readonly System.IntPtr NativeFieldInfoPtr__n;

		private static readonly System.IntPtr NativeFieldInfoPtr__marked;

		private static readonly System.IntPtr NativeFieldInfoPtr__inside;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_VertsCount_Internal_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Face _prev
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__prev);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Face>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__prev)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)face));
			}
		}

		public unsafe Face _next
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Face>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)face));
			}
		}

		public unsafe Edge _anEdge
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__anEdge);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__anEdge)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Face _trail
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__trail);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Face>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__trail)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)face));
			}
		}

		public unsafe int _n
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__n);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__n)) = num;
			}
		}

		public unsafe bool _marked
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__marked);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__marked)) = flag;
			}
		}

		public unsafe bool _inside
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inside);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inside)) = flag;
			}
		}

		public unsafe int VertsCount
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VertsCount_Internal_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static Face()
		{
			Il2CppClassPointerStore<Face>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, "Face");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Face>.NativeClassPtr);
			NativeFieldInfoPtr__prev = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_prev");
			NativeFieldInfoPtr__next = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_next");
			NativeFieldInfoPtr__anEdge = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_anEdge");
			NativeFieldInfoPtr__trail = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_trail");
			NativeFieldInfoPtr__n = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_n");
			NativeFieldInfoPtr__marked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_marked");
			NativeFieldInfoPtr__inside = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Face>.NativeClassPtr, "_inside");
			NativeMethodInfoPtr_get_VertsCount_Internal_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Face>.NativeClassPtr, 100667135);
			NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Face>.NativeClassPtr, 100667136);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Face>.NativeClassPtr, 100667137);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59691, XrefRangeEnd = 59695, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59695, XrefRangeEnd = 59698, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Face()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Face>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Face(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class EdgePair : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__e;

		private static readonly System.IntPtr NativeFieldInfoPtr__eSym;

		private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Static_EdgePair_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

		public unsafe Edge _e
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__e);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__e)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Edge _eSym
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eSym);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__eSym)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		static EdgePair()
		{
			Il2CppClassPointerStore<EdgePair>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, "EdgePair");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EdgePair>.NativeClassPtr);
			NativeFieldInfoPtr__e = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EdgePair>.NativeClassPtr, "_e");
			NativeFieldInfoPtr__eSym = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EdgePair>.NativeClassPtr, "_eSym");
			NativeMethodInfoPtr_Create_Public_Static_EdgePair_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EdgePair>.NativeClassPtr, 100667138);
			NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EdgePair>.NativeClassPtr, 100667139);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59698, XrefRangeEnd = 59707, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static EdgePair Create()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Create_Public_Static_EdgePair_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new EdgePair(pointer);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59707, XrefRangeEnd = 59709, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public EdgePair(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public EdgePair()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EdgePair>.NativeClassPtr))
		{
		}
	}

	public class Edge : Pooled<Edge>
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__pair;

		private static readonly System.IntPtr NativeFieldInfoPtr__next;

		private static readonly System.IntPtr NativeFieldInfoPtr__Sym;

		private static readonly System.IntPtr NativeFieldInfoPtr__Onext;

		private static readonly System.IntPtr NativeFieldInfoPtr__Lnext;

		private static readonly System.IntPtr NativeFieldInfoPtr__Org;

		private static readonly System.IntPtr NativeFieldInfoPtr__Lface;

		private static readonly System.IntPtr NativeFieldInfoPtr__activeRegion;

		private static readonly System.IntPtr NativeFieldInfoPtr__winding;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Rface_Internal_get_Face_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Rface_Internal_set_Void_Face_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Dst_Internal_get_Vertex_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Dst_Internal_set_Void_Vertex_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Oprev_Internal_get_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Oprev_Internal_set_Void_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Lprev_Internal_get_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Lprev_Internal_set_Void_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Dprev_Internal_get_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Dprev_Internal_set_Void_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Rprev_Internal_get_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Rprev_Internal_set_Void_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Dnext_Internal_get_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Dnext_Internal_set_Void_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get__Rnext_Internal_get_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set__Rnext_Internal_set_Void_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EnsureFirst_Internal_Static_Void_byref_Edge_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe EdgePair _pair
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pair);
				return new EdgePair(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<EdgePair>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pair), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edgePair)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<EdgePair>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe Edge _next
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Edge _Sym
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Sym);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Sym)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Edge _Onext
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Onext);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Onext)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Edge _Lnext
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Lnext);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Lnext)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)edge));
			}
		}

		public unsafe Vertex _Org
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Org);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Vertex>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Org)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertex));
			}
		}

		public unsafe Face _Lface
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Lface);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Face>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Lface)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)face));
			}
		}

		public unsafe Tess.ActiveRegion _activeRegion
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__activeRegion);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Tess.ActiveRegion>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__activeRegion)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeRegion));
			}
		}

		public unsafe int _winding
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__winding);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__winding)) = num;
			}
		}

		public unsafe Face _Rface
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Rface_Internal_get_Face_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Face>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59709, XrefRangeEnd = 59711, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Rface_Internal_set_Void_Face_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Vertex _Dst
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Dst_Internal_get_Vertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Vertex>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59711, XrefRangeEnd = 59713, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Dst_Internal_set_Void_Vertex_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Edge _Oprev
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Oprev_Internal_get_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59713, XrefRangeEnd = 59715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Oprev_Internal_set_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Edge _Lprev
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Lprev_Internal_get_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59715, XrefRangeEnd = 59717, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Lprev_Internal_set_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Edge _Dprev
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Dprev_Internal_get_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59717, XrefRangeEnd = 59719, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Dprev_Internal_set_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Edge _Rprev
		{
			[CallerCount(2)]
			[CachedScanResults(RefRangeStart = 59719, RefRangeEnd = 59721, XrefRangeStart = 59719, XrefRangeEnd = 59719, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Rprev_Internal_get_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59721, XrefRangeEnd = 59723, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Rprev_Internal_set_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Edge _Dnext
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Dnext_Internal_get_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59723, XrefRangeEnd = 59725, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Dnext_Internal_set_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe Edge _Rnext
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__Rnext_Internal_get_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59725, XrefRangeEnd = 59727, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set__Rnext_Internal_set_Void_Edge_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		static Edge()
		{
			Il2CppClassPointerStore<Edge>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, "Edge");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Edge>.NativeClassPtr);
			NativeFieldInfoPtr__pair = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_pair");
			NativeFieldInfoPtr__next = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_next");
			NativeFieldInfoPtr__Sym = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_Sym");
			NativeFieldInfoPtr__Onext = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_Onext");
			NativeFieldInfoPtr__Lnext = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_Lnext");
			NativeFieldInfoPtr__Org = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_Org");
			NativeFieldInfoPtr__Lface = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_Lface");
			NativeFieldInfoPtr__activeRegion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_activeRegion");
			NativeFieldInfoPtr__winding = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Edge>.NativeClassPtr, "_winding");
			NativeMethodInfoPtr_get__Rface_Internal_get_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667140);
			NativeMethodInfoPtr_set__Rface_Internal_set_Void_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667141);
			NativeMethodInfoPtr_get__Dst_Internal_get_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667142);
			NativeMethodInfoPtr_set__Dst_Internal_set_Void_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667143);
			NativeMethodInfoPtr_get__Oprev_Internal_get_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667144);
			NativeMethodInfoPtr_set__Oprev_Internal_set_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667145);
			NativeMethodInfoPtr_get__Lprev_Internal_get_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667146);
			NativeMethodInfoPtr_set__Lprev_Internal_set_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667147);
			NativeMethodInfoPtr_get__Dprev_Internal_get_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667148);
			NativeMethodInfoPtr_set__Dprev_Internal_set_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667149);
			NativeMethodInfoPtr_get__Rprev_Internal_get_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667150);
			NativeMethodInfoPtr_set__Rprev_Internal_set_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667151);
			NativeMethodInfoPtr_get__Dnext_Internal_get_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667152);
			NativeMethodInfoPtr_set__Dnext_Internal_set_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667153);
			NativeMethodInfoPtr_get__Rnext_Internal_get_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667154);
			NativeMethodInfoPtr_set__Rnext_Internal_set_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667155);
			NativeMethodInfoPtr_EnsureFirst_Internal_Static_Void_byref_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667156);
			NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667157);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Edge>.NativeClassPtr, 100667158);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59727, XrefRangeEnd = 59728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void EnsureFirst(ref Edge e)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
			*ptr = (nint)(&intPtr);
			Unsafe.SkipInit(out System.IntPtr intPtr3);
			System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureFirst_Internal_Static_Void_byref_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
			Il2CppException.RaiseExceptionIfNecessary(intPtr3);
			System.IntPtr intPtr4 = intPtr;
			e = ((intPtr4 == (System.IntPtr)0) ? null : new Edge(intPtr4));
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59728, XrefRangeEnd = 59737, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59737, XrefRangeEnd = 59740, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Edge()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Edge>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Edge(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Undef;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeEdge_Public_Static_Edge_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Splice_Public_Static_Void_Edge_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeVertex_Public_Static_Void_Edge_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeFace_Public_Static_Void_Edge_Face_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_KillEdge_Public_Static_Void_Edge_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_KillVertex_Public_Static_Void_Vertex_Vertex_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_KillFace_Public_Static_Void_Face_Face_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FaceArea_Public_Static_Single_Face_0;

	public unsafe static int Undef
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Undef, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Undef, (void*)(&num));
		}
	}

	static MeshUtils()
	{
		Il2CppClassPointerStore<MeshUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "MeshUtils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr);
		NativeFieldInfoPtr_Undef = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, "Undef");
		NativeMethodInfoPtr_MakeEdge_Public_Static_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667120);
		NativeMethodInfoPtr_Splice_Public_Static_Void_Edge_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667121);
		NativeMethodInfoPtr_MakeVertex_Public_Static_Void_Edge_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667122);
		NativeMethodInfoPtr_MakeFace_Public_Static_Void_Edge_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667123);
		NativeMethodInfoPtr_KillEdge_Public_Static_Void_Edge_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667124);
		NativeMethodInfoPtr_KillVertex_Public_Static_Void_Vertex_Vertex_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667125);
		NativeMethodInfoPtr_KillFace_Public_Static_Void_Face_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667126);
		NativeMethodInfoPtr_FaceArea_Public_Static_Single_Face_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshUtils>.NativeClassPtr, 100667127);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 59766, RefRangeEnd = 59771, XrefRangeStart = 59740, XrefRangeEnd = 59766, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Edge MakeEdge(Edge eNext)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eNext);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeEdge_Public_Static_Edge_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Edge>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59771, XrefRangeEnd = 59776, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Splice(Edge a, Edge b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Splice_Public_Static_Void_Edge_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 59785, RefRangeEnd = 59792, XrefRangeStart = 59776, XrefRangeEnd = 59785, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeVertex(Edge eOrig, Vertex vNext)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eOrig);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vNext);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeVertex_Public_Static_Void_Edge_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 59802, RefRangeEnd = 59807, XrefRangeStart = 59792, XrefRangeEnd = 59802, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeFace(Edge eOrig, Face fNext)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eOrig);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fNext);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeFace_Public_Static_Void_Edge_Face_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 59813, RefRangeEnd = 59815, XrefRangeStart = 59807, XrefRangeEnd = 59813, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void KillEdge(Edge eDel)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eDel);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_KillEdge_Public_Static_Void_Edge_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 59822, RefRangeEnd = 59825, XrefRangeStart = 59815, XrefRangeEnd = 59822, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void KillVertex(Vertex vDel, Vertex newOrg)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vDel);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newOrg);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_KillVertex_Public_Static_Void_Vertex_Vertex_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 59832, RefRangeEnd = 59836, XrefRangeStart = 59825, XrefRangeEnd = 59832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void KillFace(Face fDel, Face newLFace)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fDel);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newLFace);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_KillFace_Public_Static_Void_Face_Face_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 59836, RefRangeEnd = 59844, XrefRangeStart = 59836, XrefRangeEnd = 59836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float FaceArea(Face f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)f);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FaceArea_Public_Static_Single_Face_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public MeshUtils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
