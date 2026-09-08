using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public class PriorityHeap<TValue> : Il2CppSystem.Object where TValue : class
{
	public sealed class LessOrEqual : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_TValue_TValue_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TValue_TValue_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_IAsyncResult_0;

		static LessOrEqual()
		{
			Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "LessOrEqual"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667169);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_TValue_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667170);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TValue_TValue_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667171);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667172);
		}

		[CallerCount(32)]
		[CachedScanResults(RefRangeStart = 59159, RefRangeEnd = 59191, XrefRangeStart = 59159, XrefRangeEnd = 59191, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LessOrEqual(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual bool Invoke(TValue lhs, TValue rhs)
		{
			//IL_0061->IL0064: Incompatible stack types: I vs Ref
			//IL_0040->IL0064: Incompatible stack types: I vs Ref
			//IL_004d->IL0064: Incompatible stack types: I vs Ref
			//IL_00be->IL00c1: Incompatible stack types: I vs Ref
			//IL_009d->IL00c1: Incompatible stack types: I vs Ref
			//IL_0054->IL0064: Incompatible stack types: I vs Ref
			//IL_00aa->IL00c1: Incompatible stack types: I vs Ref
			//IL_00b1->IL00c1: Incompatible stack types: I vs Ref
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			ref TValue reference;
			if (!typeof(TValue).IsValueType)
			{
				TValue val = lhs;
				if (val is string)
				{
					reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
				}
				else
				{
					System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
					reference = ref *(_003F*)intPtr;
					if (intPtr != (System.IntPtr)0)
					{
						reference = ref *(_003F*)intPtr;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
						}
					}
				}
			}
			else
			{
				reference = ref lhs;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
			ref TValue reference2;
			if (!typeof(TValue).IsValueType)
			{
				TValue val2 = rhs;
				if (val2 is string)
				{
					reference2 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val2 as string);
				}
				else
				{
					System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val2 is Il2CppObjectBase) ? val2 : null));
					reference2 = ref *(_003F*)intPtr2;
					if (intPtr2 != (System.IntPtr)0)
					{
						reference2 = ref *(_003F*)intPtr2;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr2)))
						{
							reference2 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr2);
						}
					}
				}
			}
			else
			{
				reference2 = ref rhs;
			}
			*(void**)num = Unsafe.AsPointer(ref reference2);
			Unsafe.SkipInit(out System.IntPtr intPtr4);
			System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_TValue_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
			Il2CppException.RaiseExceptionIfNecessary(intPtr4);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr3);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(TValue lhs, TValue rhs, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			//IL_0061->IL0064: Incompatible stack types: I vs Ref
			//IL_0040->IL0064: Incompatible stack types: I vs Ref
			//IL_004d->IL0064: Incompatible stack types: I vs Ref
			//IL_00be->IL00c1: Incompatible stack types: I vs Ref
			//IL_009d->IL00c1: Incompatible stack types: I vs Ref
			//IL_0054->IL0064: Incompatible stack types: I vs Ref
			//IL_00aa->IL00c1: Incompatible stack types: I vs Ref
			//IL_00b1->IL00c1: Incompatible stack types: I vs Ref
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			ref TValue reference;
			if (!typeof(TValue).IsValueType)
			{
				TValue val = lhs;
				if (val is string)
				{
					reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
				}
				else
				{
					System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
					reference = ref *(_003F*)intPtr;
					if (intPtr != (System.IntPtr)0)
					{
						reference = ref *(_003F*)intPtr;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
						}
					}
				}
			}
			else
			{
				reference = ref lhs;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
			ref TValue reference2;
			if (!typeof(TValue).IsValueType)
			{
				TValue val2 = rhs;
				if (val2 is string)
				{
					reference2 = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val2 as string);
				}
				else
				{
					System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val2 is Il2CppObjectBase) ? val2 : null));
					reference2 = ref *(_003F*)intPtr2;
					if (intPtr2 != (System.IntPtr)0)
					{
						reference2 = ref *(_003F*)intPtr2;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr2)))
						{
							reference2 = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr2);
						}
					}
				}
			}
			else
			{
				reference2 = ref rhs;
			}
			*(void**)num = Unsafe.AsPointer(ref reference2);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr4);
			System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TValue_TValue_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr4);
			Il2CppException.RaiseExceptionIfNecessary(intPtr4);
			return (intPtr3 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr3) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public LessOrEqual(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator LessOrEqual(System.Func<TValue, TValue, bool> P_0)
		{
			return DelegateSupport.ConvertDelegate<LessOrEqual>((System.Delegate)P_0);
		}

		public static LessOrEqual operator +(LessOrEqual P_0, LessOrEqual P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<LessOrEqual>();
		}

		public static LessOrEqual operator -(LessOrEqual P_0, LessOrEqual P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<LessOrEqual>();
			}
			return (LessOrEqual)obj;
		}
	}

	public class HandleElem : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__key;

		private static readonly System.IntPtr NativeFieldInfoPtr__node;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe TValue _key
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__key);
				return IL2CPP.PointerToValueGeneric<TValue>((System.IntPtr)num, true, false);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__key);
				System.Type typeFromHandle = typeof(TValue);
				if (!typeFromHandle.IsValueType)
				{
					if (!string.Equals(typeFromHandle.FullName, "System.String"))
					{
						System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
						if (intPtr != (System.IntPtr)0 && IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							System.IntPtr intPtr2 = intPtr;
							// IL cpblk instruction
							Unsafe.CopyBlock(num2, IL2CPP.il2cpp_object_unbox(intPtr), IL2CPP.il2cpp_class_value_size(IL2CPP.il2cpp_object_get_class(intPtr2), ref *(uint*)null));
						}
						else
						{
							IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, intPtr);
						}
					}
					else
					{
						IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, IL2CPP.ManagedStringToIl2Cpp(val as string));
					}
				}
				else
				{
					Unsafe.Write((void*)num2, val);
				}
			}
		}

		public unsafe int _node
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__node);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__node)) = num;
			}
		}

		static HandleElem()
		{
			Il2CppClassPointerStore<HandleElem>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "HandleElem"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HandleElem>.NativeClassPtr);
			NativeFieldInfoPtr__key = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HandleElem>.NativeClassPtr, "_key");
			NativeFieldInfoPtr__node = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HandleElem>.NativeClassPtr, "_node");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HandleElem>.NativeClassPtr, 100667173);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe HandleElem()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HandleElem>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public HandleElem(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__leq;

	private static readonly System.IntPtr NativeFieldInfoPtr__nodes;

	private static readonly System.IntPtr NativeFieldInfoPtr__handles;

	private static readonly System.IntPtr NativeFieldInfoPtr__size;

	private static readonly System.IntPtr NativeFieldInfoPtr__max;

	private static readonly System.IntPtr NativeFieldInfoPtr__freeList;

	private static readonly System.IntPtr NativeFieldInfoPtr__initialized;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Empty_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_LessOrEqual_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FloatDown_Private_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FloatUp_Private_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Init_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Insert_Public_PQHandle_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExtractMin_Public_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Minimum_Public_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_PQHandle_0;

	public unsafe LessOrEqual _leq
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__leq);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<LessOrEqual>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__leq)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lessOrEqual));
		}
	}

	public unsafe Il2CppStructArray<int> _nodes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__nodes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__nodes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<HandleElem> _handles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__handles);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<HandleElem>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__handles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int _size
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__size);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__size)) = num;
		}
	}

	public unsafe int _max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__max);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__max)) = num;
		}
	}

	public unsafe int _freeList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__freeList);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__freeList)) = num;
		}
	}

	public unsafe bool _initialized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__initialized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__initialized)) = flag;
		}
	}

	public unsafe bool Empty
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 59844, RefRangeEnd = 59848, XrefRangeStart = 59844, XrefRangeEnd = 59844, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Empty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PriorityHeap()
	{
		Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "PriorityHeap`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr);
		NativeFieldInfoPtr__leq = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_leq");
		NativeFieldInfoPtr__nodes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_nodes");
		NativeFieldInfoPtr__handles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_handles");
		NativeFieldInfoPtr__size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_size");
		NativeFieldInfoPtr__max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_max");
		NativeFieldInfoPtr__freeList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_freeList");
		NativeFieldInfoPtr__initialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, "_initialized");
		NativeMethodInfoPtr_get_Empty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667160);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_LessOrEqual_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667161);
		NativeMethodInfoPtr_FloatDown_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667162);
		NativeMethodInfoPtr_FloatUp_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667163);
		NativeMethodInfoPtr_Init_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667164);
		NativeMethodInfoPtr_Insert_Public_PQHandle_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667165);
		NativeMethodInfoPtr_ExtractMin_Public_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667166);
		NativeMethodInfoPtr_Minimum_Public_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667167);
		NativeMethodInfoPtr_Remove_Public_Void_PQHandle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr, 100667168);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59866, RefRangeEnd = 59867, XrefRangeStart = 59848, XrefRangeEnd = 59866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PriorityHeap(int initialSize, LessOrEqual leq)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PriorityHeap<TValue>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&initialSize);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)leq);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_LessOrEqual_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 59868, RefRangeEnd = 59875, XrefRangeStart = 59867, XrefRangeEnd = 59868, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FloatDown(int curr)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&curr);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FloatDown_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59875, XrefRangeEnd = 59876, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FloatUp(int curr)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&curr);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FloatUp_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59877, RefRangeEnd = 59878, XrefRangeStart = 59876, XrefRangeEnd = 59877, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Init()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Init_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59878, XrefRangeEnd = 59891, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PQHandle Insert(TValue value)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref TValue reference;
		if (!typeof(TValue).IsValueType)
		{
			TValue val = value;
			if (val is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref value;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Insert_Public_PQHandle_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return *(PQHandle*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59891, XrefRangeEnd = 59892, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TValue ExtractMin()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ExtractMin_Public_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<TValue>(intPtr, false, true);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 59892, RefRangeEnd = 59894, XrefRangeStart = 59892, XrefRangeEnd = 59892, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TValue Minimum()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Minimum_Public_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<TValue>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59894, XrefRangeEnd = 59897, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Remove(PQHandle handle)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&handle);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_PQHandle_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PriorityHeap(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
