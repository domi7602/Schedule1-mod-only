using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Product;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.Effects;

public static class EffectMixCalculator : Il2CppSystem.Object
{
	public class Reaction : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Existing;

		private static readonly System.IntPtr NativeFieldInfoPtr_Output;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Effect Existing
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Existing);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Effect>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Existing)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)effect));
			}
		}

		public unsafe Effect Output
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Output);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Effect>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Output)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)effect));
			}
		}

		static Reaction()
		{
			Il2CppClassPointerStore<Reaction>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr, "Reaction");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Reaction>.NativeClassPtr);
			NativeFieldInfoPtr_Existing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Reaction>.NativeClassPtr, "Existing");
			NativeFieldInfoPtr_Output = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Reaction>.NativeClassPtr, "Output");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Reaction>.NativeClassPtr, 100685535);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Reaction()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Reaction>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Reaction(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_Shuffle_Public_Static_Void_List_1_T_Int32_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Shuffle_Public_Static_Void_List_1_T_Int32_0, Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_PROPERTIES;

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_DELTA_DIFFERENCE;

	private static readonly System.IntPtr NativeMethodInfoPtr_MixProperties_Public_Static_List_1_Effect_List_1_Effect_Effect_EDrugType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Shuffle_Public_Static_Void_List_1_T_Int32_0;

	public unsafe static int MAX_PROPERTIES
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_PROPERTIES, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_PROPERTIES, (void*)(&num));
		}
	}

	public unsafe static float MAX_DELTA_DIFFERENCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_DELTA_DIFFERENCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_DELTA_DIFFERENCE, (void*)(&num));
		}
	}

	static EffectMixCalculator()
	{
		Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Effects", "EffectMixCalculator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr);
		NativeFieldInfoPtr_MAX_PROPERTIES = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr, "MAX_PROPERTIES");
		NativeFieldInfoPtr_MAX_DELTA_DIFFERENCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr, "MAX_DELTA_DIFFERENCE");
		NativeMethodInfoPtr_MixProperties_Public_Static_List_1_Effect_List_1_Effect_Effect_EDrugType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr, 100685533);
		NativeMethodInfoPtr_Shuffle_Public_Static_Void_List_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EffectMixCalculator>.NativeClassPtr, 100685534);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 290899, RefRangeEnd = 290903, XrefRangeStart = 290797, XrefRangeEnd = 290899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<Effect> MixProperties(List<Effect> existingProperties, Effect newProperty, EDrugType drugType)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)existingProperties);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newProperty);
		*(EDrugType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &drugType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MixProperties_Public_Static_List_1_Effect_List_1_Effect_Effect_EDrugType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Effect>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 290903, XrefRangeEnd = 290915, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Shuffle<T>(List<T> list, int seed)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &seed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_Shuffle_Public_Static_Void_List_1_T_Int32_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EffectMixCalculator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
