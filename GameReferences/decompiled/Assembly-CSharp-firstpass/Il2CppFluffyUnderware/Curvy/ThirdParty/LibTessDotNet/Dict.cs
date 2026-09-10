using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public class Dict<TValue> : Il2CppSystem.Object where TValue : class
{
	public class Node : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__key;

		private static readonly System.IntPtr NativeFieldInfoPtr__prev;

		private static readonly System.IntPtr NativeFieldInfoPtr__next;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Key_Public_get_TValue_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Prev_Public_get_Node_TValue_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Next_Public_get_Node_TValue_0;

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

		public unsafe Node _prev
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__prev);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__prev)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node));
			}
		}

		public unsafe Node _next
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__next)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node));
			}
		}

		public unsafe TValue Key
		{
			[CallerCount(12)]
			[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Key_Public_get_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.PointerToValueGeneric<TValue>(intPtr, false, true);
			}
		}

		public unsafe Node Prev
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Prev_Public_get_Node_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
			}
		}

		public unsafe Node Next
		{
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Next_Public_get_Node_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
			}
		}

		static Node()
		{
			Il2CppClassPointerStore<Node>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, "Node"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Node>.NativeClassPtr);
			NativeFieldInfoPtr__key = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Node>.NativeClassPtr, "_key");
			NativeFieldInfoPtr__prev = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Node>.NativeClassPtr, "_prev");
			NativeFieldInfoPtr__next = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Node>.NativeClassPtr, "_next");
			NativeMethodInfoPtr_get_Key_Public_get_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100667075);
			NativeMethodInfoPtr_get_Prev_Public_get_Node_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100667076);
			NativeMethodInfoPtr_get_Next_Public_get_Node_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100667077);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100667078);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Node()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Node>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Node(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class LessOrEqual : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_TValue_TValue_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TValue_TValue_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_IAsyncResult_0;

		static LessOrEqual()
		{
			Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, "LessOrEqual"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667079);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_TValue_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667080);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TValue_TValue_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667081);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LessOrEqual>.NativeClassPtr, 100667082);
		}

		[CallerCount(32)]
		[CachedScanResults(RefRangeStart = 59159, RefRangeEnd = 59191, XrefRangeStart = 59149, XrefRangeEnd = 59159, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59191, XrefRangeEnd = 59192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59192, XrefRangeEnd = 59194, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	private static readonly System.IntPtr NativeFieldInfoPtr__leq;

	private static readonly System.IntPtr NativeFieldInfoPtr__head;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_LessOrEqual_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Insert_Public_Node_TValue_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertBefore_Public_Node_TValue_Node_TValue_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Find_Public_Node_TValue_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Min_Public_Node_TValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_Node_TValue_0;

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

	public unsafe Node _head
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__head);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__head)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node));
		}
	}

	static Dict()
	{
		Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "Dict`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr);
		NativeFieldInfoPtr__leq = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, "_leq");
		NativeFieldInfoPtr__head = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, "_head");
		NativeMethodInfoPtr__ctor_Public_Void_LessOrEqual_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, 100667069);
		NativeMethodInfoPtr_Insert_Public_Node_TValue_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, 100667070);
		NativeMethodInfoPtr_InsertBefore_Public_Node_TValue_Node_TValue_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, 100667071);
		NativeMethodInfoPtr_Find_Public_Node_TValue_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, 100667072);
		NativeMethodInfoPtr_Min_Public_Node_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, 100667073);
		NativeMethodInfoPtr_Remove_Public_Void_Node_TValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr, 100667074);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 59203, RefRangeEnd = 59207, XrefRangeStart = 59194, XrefRangeEnd = 59203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dict(LessOrEqual leq)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Dict<TValue>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)leq);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_LessOrEqual_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 59215, RefRangeEnd = 59217, XrefRangeStart = 59207, XrefRangeEnd = 59215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Node Insert(TValue key)
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
			TValue val = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Insert_Public_Node_TValue_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr2) : null;
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 59225, RefRangeEnd = 59237, XrefRangeStart = 59217, XrefRangeEnd = 59225, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Node InsertBefore(Node node, TValue key)
	{
		//IL_0073->IL0076: Incompatible stack types: I vs Ref
		//IL_0052->IL0076: Incompatible stack types: I vs Ref
		//IL_005f->IL0076: Incompatible stack types: I vs Ref
		//IL_0066->IL0076: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		ref TValue reference;
		if (!typeof(TValue).IsValueType)
		{
			TValue val = key;
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
			reference = ref key;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertBefore_Public_Node_TValue_Node_TValue_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr2) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 59237, RefRangeEnd = 59241, XrefRangeStart = 59237, XrefRangeEnd = 59237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Node Find(TValue key)
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
			TValue val = key;
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
			reference = ref key;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Find_Public_Node_TValue_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr2) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 59241, RefRangeEnd = 59245, XrefRangeStart = 59241, XrefRangeEnd = 59241, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Node Min()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Min_Public_Node_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
	}

	[CallerCount(20)]
	[CachedScanResults(RefRangeStart = 59248, RefRangeEnd = 59268, XrefRangeStart = 59245, XrefRangeEnd = 59248, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Remove(Node node)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_Node_TValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Dict(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
