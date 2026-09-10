using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne;

[System.Serializable]
public class NavigationOverride<T> : Il2CppSystem.Object
{
	[System.Serializable]
	public class OverrideElement<S> : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Element;

		private static readonly System.IntPtr NativeFieldInfoPtr_IsExplicit;

		private static readonly System.IntPtr NativeFieldInfoPtr_IsReciprocated;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe S Element
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Element);
				return IL2CPP.PointerToValueGeneric<S>((System.IntPtr)num, true, false);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Element);
				System.Type typeFromHandle = typeof(S);
				if (!typeFromHandle.IsValueType)
				{
					if (!string.Equals(typeFromHandle.FullName, "System.String"))
					{
						System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((((object)val) is Il2CppObjectBase) ? ((object)val) : null));
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

		public unsafe bool IsExplicit
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsExplicit);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsExplicit)) = flag;
			}
		}

		public unsafe bool IsReciprocated
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsReciprocated);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsReciprocated)) = flag;
			}
		}

		static OverrideElement()
		{
			Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, "OverrideElement`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[2]
			{
				Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)),
				Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<S>.NativeClassPtr))
			})).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr);
			NativeFieldInfoPtr_Element = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr, "Element");
			NativeFieldInfoPtr_IsExplicit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr, "IsExplicit");
			NativeFieldInfoPtr_IsReciprocated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr, "IsReciprocated");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr, 100665164);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OverrideElement()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OverrideElement<S>>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public OverrideElement(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Up;

	private static readonly System.IntPtr NativeFieldInfoPtr_Down;

	private static readonly System.IntPtr NativeFieldInfoPtr_Left;

	private static readonly System.IntPtr NativeFieldInfoPtr_Right;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasOverride_Public_Boolean_Vector2_byref_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe OverrideElement<T> Up
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Up);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OverrideElement<T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Up)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)overrideElement));
		}
	}

	public unsafe OverrideElement<T> Down
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Down);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OverrideElement<T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Down)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)overrideElement));
		}
	}

	public unsafe OverrideElement<T> Left
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Left);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OverrideElement<T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Left)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)overrideElement));
		}
	}

	public unsafe OverrideElement<T> Right
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Right);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OverrideElement<T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Right)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)overrideElement));
		}
	}

	static NavigationOverride()
	{
		Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "NavigationOverride`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr);
		NativeFieldInfoPtr_Up = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, "Up");
		NativeFieldInfoPtr_Down = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, "Down");
		NativeFieldInfoPtr_Left = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, "Left");
		NativeFieldInfoPtr_Right = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, "Right");
		NativeMethodInfoPtr_HasOverride_Public_Boolean_Vector2_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, 100665162);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr, 100665163);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 81986, XrefRangeEnd = 81991, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool HasOverride(Vector2 direction, out T component)
	{
		//IL_003b->IL003d: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&direction);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		Unsafe.SkipInit(out nint num2);
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			num2 = 0;
			reference = ref *(_003F*)(&num2);
		}
		else
		{
			reference = ref component;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasOverride_Public_Boolean_Vector2_byref_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		if (!typeof(T).IsValueType)
		{
			nint num3 = num2;
			Unsafe.As<T, object>(ref component) = ((num3 == 0) ? ((T)null) : IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num3, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NavigationOverride()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NavigationOverride<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NavigationOverride(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
