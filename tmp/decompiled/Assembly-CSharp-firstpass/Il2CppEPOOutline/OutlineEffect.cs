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

public static class OutlineEffect : Il2CppSystem.Object
{
	public sealed class OutlineTargetGroup : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Outlinable;

		private static readonly System.IntPtr NativeFieldInfoPtr_Target;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Outlinable_OutlineTarget_0;

		public unsafe Outlinable Outlinable
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Outlinable);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Outlinable>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Outlinable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outlinable));
			}
		}

		public unsafe OutlineTarget Target
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Target);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OutlineTarget>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outlineTarget));
			}
		}

		static OutlineTargetGroup()
		{
			Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "OutlineTargetGroup");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr);
			NativeFieldInfoPtr_Outlinable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr, "Outlinable");
			NativeFieldInfoPtr_Target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr, "Target");
			NativeMethodInfoPtr__ctor_Public_Void_Outlinable_OutlineTarget_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr, 100667344);
		}

		[CallerCount(80)]
		[CachedScanResults(RefRangeStart = 62123, RefRangeEnd = 62203, XrefRangeStart = 62121, XrefRangeEnd = 62123, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OutlineTargetGroup(Outlinable outlinable, OutlineTarget target)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outlinable);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Outlinable_OutlineTarget_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public OutlineTargetGroup(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public OutlineTargetGroup()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutlineTargetGroup>.NativeClassPtr))
		{
		}
	}

	[System.Serializable]
	[ObfuscatedName("EPOOutline.OutlineEffect+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_2;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_12;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_13;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_14;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_15;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_16;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_17;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_18;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_19;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_20;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_21;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_22;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_23;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_3;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_4;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_5;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_6;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_7;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_8;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_10;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__55_11;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_0_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_1_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_2_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_12_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_13_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_14_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_15_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_16_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_17_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_18_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_19_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_20_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_21_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_22_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_23_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_3_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_4_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_5_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_6_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_7_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_8_Internal_Material_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_9_Internal_Boolean_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_10_Internal_Color_Outlinable_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetupOutline_b__55_11_Internal_Material_Outlinable_0;

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

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_2
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_2, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_2, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_12
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_12, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_12, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_13
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_13, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_13, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_14
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_14, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_14, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_15
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_15, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_15, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_16
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_16, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_16, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_17
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_17, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_17, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_18
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_18, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_18, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_19
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_19, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_19, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_20
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_20, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_20, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_21
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_21, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_21, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_22
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_22, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_22, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_23
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_23, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_23, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_3
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_3, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_3, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_4
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_4, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_4, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_5
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_5, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_5, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_6
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_6, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_6, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_7
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_7, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_7, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_8
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_8, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_8, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, bool> __9__55_9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Color> __9__55_10
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_10, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_10, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<Outlinable, Material> __9__55_11
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__55_11, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Outlinable, Material>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__55_11, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__55_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_0");
			NativeFieldInfoPtr___9__55_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_1");
			NativeFieldInfoPtr___9__55_2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_2");
			NativeFieldInfoPtr___9__55_12 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_12");
			NativeFieldInfoPtr___9__55_13 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_13");
			NativeFieldInfoPtr___9__55_14 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_14");
			NativeFieldInfoPtr___9__55_15 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_15");
			NativeFieldInfoPtr___9__55_16 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_16");
			NativeFieldInfoPtr___9__55_17 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_17");
			NativeFieldInfoPtr___9__55_18 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_18");
			NativeFieldInfoPtr___9__55_19 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_19");
			NativeFieldInfoPtr___9__55_20 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_20");
			NativeFieldInfoPtr___9__55_21 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_21");
			NativeFieldInfoPtr___9__55_22 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_22");
			NativeFieldInfoPtr___9__55_23 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_23");
			NativeFieldInfoPtr___9__55_3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_3");
			NativeFieldInfoPtr___9__55_4 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_4");
			NativeFieldInfoPtr___9__55_5 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_5");
			NativeFieldInfoPtr___9__55_6 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_6");
			NativeFieldInfoPtr___9__55_7 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_7");
			NativeFieldInfoPtr___9__55_8 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_8");
			NativeFieldInfoPtr___9__55_9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_9");
			NativeFieldInfoPtr___9__55_10 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_10");
			NativeFieldInfoPtr___9__55_11 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__55_11");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667346);
			NativeMethodInfoPtr__SetupOutline_b__55_0_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667347);
			NativeMethodInfoPtr__SetupOutline_b__55_1_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667348);
			NativeMethodInfoPtr__SetupOutline_b__55_2_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667349);
			NativeMethodInfoPtr__SetupOutline_b__55_12_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667350);
			NativeMethodInfoPtr__SetupOutline_b__55_13_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667351);
			NativeMethodInfoPtr__SetupOutline_b__55_14_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667352);
			NativeMethodInfoPtr__SetupOutline_b__55_15_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667353);
			NativeMethodInfoPtr__SetupOutline_b__55_16_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667354);
			NativeMethodInfoPtr__SetupOutline_b__55_17_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667355);
			NativeMethodInfoPtr__SetupOutline_b__55_18_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667356);
			NativeMethodInfoPtr__SetupOutline_b__55_19_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667357);
			NativeMethodInfoPtr__SetupOutline_b__55_20_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667358);
			NativeMethodInfoPtr__SetupOutline_b__55_21_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667359);
			NativeMethodInfoPtr__SetupOutline_b__55_22_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667360);
			NativeMethodInfoPtr__SetupOutline_b__55_23_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667361);
			NativeMethodInfoPtr__SetupOutline_b__55_3_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667362);
			NativeMethodInfoPtr__SetupOutline_b__55_4_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667363);
			NativeMethodInfoPtr__SetupOutline_b__55_5_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667364);
			NativeMethodInfoPtr__SetupOutline_b__55_6_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667365);
			NativeMethodInfoPtr__SetupOutline_b__55_7_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667366);
			NativeMethodInfoPtr__SetupOutline_b__55_8_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667367);
			NativeMethodInfoPtr__SetupOutline_b__55_9_Internal_Boolean_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667368);
			NativeMethodInfoPtr__SetupOutline_b__55_10_Internal_Color_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667369);
			NativeMethodInfoPtr__SetupOutline_b__55_11_Internal_Material_Outlinable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100667370);
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

		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _SetupOutline_b__55_0(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_0_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_1(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_1_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62203, XrefRangeEnd = 62207, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_2(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_2_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _SetupOutline_b__55_12(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_12_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_13(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_13_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62207, XrefRangeEnd = 62211, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_14(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_14_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _SetupOutline_b__55_15(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_15_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_16(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_16_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62211, XrefRangeEnd = 62215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_17(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_17_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _SetupOutline_b__55_18(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_18_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_19(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_19_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62215, XrefRangeEnd = 62219, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_20(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_20_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _SetupOutline_b__55_21(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_21_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62219, XrefRangeEnd = 62224, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Color _SetupOutline_b__55_22(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_22_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62224, XrefRangeEnd = 62228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_23(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_23_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _SetupOutline_b__55_3(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_3_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_4(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_4_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62228, XrefRangeEnd = 62232, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_5(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_5_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _SetupOutline_b__55_6(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_6_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_7(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_7_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62232, XrefRangeEnd = 62236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_8(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_8_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _SetupOutline_b__55_9(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_9_Internal_Boolean_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe Color _SetupOutline_b__55_10(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_10_Internal_Color_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62236, XrefRangeEnd = 62240, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Material _SetupOutline_b__55_11(Outlinable x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetupOutline_b__55_11_Internal_Material_Outlinable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_FillRefHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateShiftHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ColorMaskHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutlineRefHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_RefHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ZWriteHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_EffectSizeHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_CullHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ZTestHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ColorHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ScaleHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ShiftHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_InitialTexHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_InfoBufferHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ComparisonHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_ReadMaskHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_WriteMaskHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_OperationHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_CutoutThresholdHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_CutoutMaskHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_TextureIndexHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_CutoutTextureHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_CutoutTextureSTHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_SrcBlendHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_DstBlendHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_TargetHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_InfoTargetHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_PrimaryBufferHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_HelperBufferHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_PrimaryInfoBufferHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_HelperInfoBufferHash;

	private static readonly System.IntPtr NativeFieldInfoPtr_TransparentBlitMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_EmptyFillMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutlineMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_PartialBlitMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_ObstacleMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_FillMaskMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_ZPrepassMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutlineMaskMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_DilateMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_BlurMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_FinalBlitMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_BasicBlitMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_ClearStencilMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr_targets;

	private static readonly System.IntPtr NativeFieldInfoPtr_keywords;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadMaterial_Public_Static_Material_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitMaterials_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Postprocess_Private_Static_Void_OutlineParameters_Int32_Int32_Material_Int32_Boolean_Single_byref_Int32_Rect_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Blit_Private_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_RenderTargetIdentifier_Material_Single_CommandBuffer_Int32_Nullable_1_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBlurShift_Private_Static_Single_BlurType_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaskingValueForMode_Private_Static_Single_OutlinableDrawingMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeEffectShift_Private_Static_Single_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareTargets_Private_Static_Void_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupOutline_Public_Static_Void_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupDilateKeyword_Private_Static_Void_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupBlurKeyword_Private_Static_Void_OutlineParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DrawOutlineables_Private_Static_Int32_OutlineParameters_CompareFunction_Func_2_Outlinable_Boolean_Func_2_Outlinable_Color_Func_2_Outlinable_Material_RenderStyle_OutlinableDrawingMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DrawFill_Private_Static_Void_OutlineParameters_RenderTargetIdentifier_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupCutout_Private_Static_Void_OutlineParameters_OutlineTarget_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupCull_Private_Static_Void_OutlineParameters_OutlineTarget_0;

	public unsafe static int FillRefHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FillRefHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FillRefHash, (void*)(&num));
		}
	}

	public unsafe static int DilateShiftHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DilateShiftHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DilateShiftHash, (void*)(&num));
		}
	}

	public unsafe static int ColorMaskHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ColorMaskHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ColorMaskHash, (void*)(&num));
		}
	}

	public unsafe static int OutlineRefHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OutlineRefHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OutlineRefHash, (void*)(&num));
		}
	}

	public unsafe static int RefHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RefHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RefHash, (void*)(&num));
		}
	}

	public unsafe static int ZWriteHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ZWriteHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ZWriteHash, (void*)(&num));
		}
	}

	public unsafe static int EffectSizeHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EffectSizeHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EffectSizeHash, (void*)(&num));
		}
	}

	public unsafe static int CullHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CullHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CullHash, (void*)(&num));
		}
	}

	public unsafe static int ZTestHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ZTestHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ZTestHash, (void*)(&num));
		}
	}

	public unsafe static int ColorHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ColorHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ColorHash, (void*)(&num));
		}
	}

	public unsafe static int ScaleHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ScaleHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ScaleHash, (void*)(&num));
		}
	}

	public unsafe static int ShiftHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ShiftHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ShiftHash, (void*)(&num));
		}
	}

	public unsafe static int InitialTexHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_InitialTexHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_InitialTexHash, (void*)(&num));
		}
	}

	public unsafe static int InfoBufferHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_InfoBufferHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_InfoBufferHash, (void*)(&num));
		}
	}

	public unsafe static int ComparisonHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ComparisonHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ComparisonHash, (void*)(&num));
		}
	}

	public unsafe static int ReadMaskHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ReadMaskHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ReadMaskHash, (void*)(&num));
		}
	}

	public unsafe static int WriteMaskHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WriteMaskHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WriteMaskHash, (void*)(&num));
		}
	}

	public unsafe static int OperationHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OperationHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OperationHash, (void*)(&num));
		}
	}

	public unsafe static int CutoutThresholdHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CutoutThresholdHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CutoutThresholdHash, (void*)(&num));
		}
	}

	public unsafe static int CutoutMaskHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CutoutMaskHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CutoutMaskHash, (void*)(&num));
		}
	}

	public unsafe static int TextureIndexHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TextureIndexHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TextureIndexHash, (void*)(&num));
		}
	}

	public unsafe static int CutoutTextureHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CutoutTextureHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CutoutTextureHash, (void*)(&num));
		}
	}

	public unsafe static int CutoutTextureSTHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CutoutTextureSTHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CutoutTextureSTHash, (void*)(&num));
		}
	}

	public unsafe static int SrcBlendHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SrcBlendHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SrcBlendHash, (void*)(&num));
		}
	}

	public unsafe static int DstBlendHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DstBlendHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DstBlendHash, (void*)(&num));
		}
	}

	public unsafe static int TargetHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TargetHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TargetHash, (void*)(&num));
		}
	}

	public unsafe static int InfoTargetHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_InfoTargetHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_InfoTargetHash, (void*)(&num));
		}
	}

	public unsafe static int PrimaryBufferHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PrimaryBufferHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PrimaryBufferHash, (void*)(&num));
		}
	}

	public unsafe static int HelperBufferHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HelperBufferHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HelperBufferHash, (void*)(&num));
		}
	}

	public unsafe static int PrimaryInfoBufferHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PrimaryInfoBufferHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PrimaryInfoBufferHash, (void*)(&num));
		}
	}

	public unsafe static int HelperInfoBufferHash
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HelperInfoBufferHash, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HelperInfoBufferHash, (void*)(&num));
		}
	}

	public unsafe static Material TransparentBlitMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TransparentBlitMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TransparentBlitMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material EmptyFillMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EmptyFillMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EmptyFillMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material OutlineMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OutlineMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OutlineMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material PartialBlitMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PartialBlitMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PartialBlitMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material ObstacleMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ObstacleMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ObstacleMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material FillMaskMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FillMaskMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FillMaskMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material ZPrepassMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ZPrepassMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ZPrepassMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material OutlineMaskMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OutlineMaskMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OutlineMaskMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material DilateMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DilateMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DilateMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material BlurMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BlurMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BlurMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material FinalBlitMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FinalBlitMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FinalBlitMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material BasicBlitMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BasicBlitMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BasicBlitMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static Material ClearStencilMaterial
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ClearStencilMaterial, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ClearStencilMaterial, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static List<OutlineTargetGroup> targets
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_targets, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<OutlineTargetGroup>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_targets, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static List<string> keywords
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_keywords, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_keywords, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static OutlineEffect()
	{
		Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "OutlineEffect");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr);
		NativeFieldInfoPtr_FillRefHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "FillRefHash");
		NativeFieldInfoPtr_DilateShiftHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "DilateShiftHash");
		NativeFieldInfoPtr_ColorMaskHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ColorMaskHash");
		NativeFieldInfoPtr_OutlineRefHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "OutlineRefHash");
		NativeFieldInfoPtr_RefHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "RefHash");
		NativeFieldInfoPtr_ZWriteHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ZWriteHash");
		NativeFieldInfoPtr_EffectSizeHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "EffectSizeHash");
		NativeFieldInfoPtr_CullHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "CullHash");
		NativeFieldInfoPtr_ZTestHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ZTestHash");
		NativeFieldInfoPtr_ColorHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ColorHash");
		NativeFieldInfoPtr_ScaleHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ScaleHash");
		NativeFieldInfoPtr_ShiftHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ShiftHash");
		NativeFieldInfoPtr_InitialTexHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "InitialTexHash");
		NativeFieldInfoPtr_InfoBufferHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "InfoBufferHash");
		NativeFieldInfoPtr_ComparisonHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ComparisonHash");
		NativeFieldInfoPtr_ReadMaskHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ReadMaskHash");
		NativeFieldInfoPtr_WriteMaskHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "WriteMaskHash");
		NativeFieldInfoPtr_OperationHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "OperationHash");
		NativeFieldInfoPtr_CutoutThresholdHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "CutoutThresholdHash");
		NativeFieldInfoPtr_CutoutMaskHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "CutoutMaskHash");
		NativeFieldInfoPtr_TextureIndexHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "TextureIndexHash");
		NativeFieldInfoPtr_CutoutTextureHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "CutoutTextureHash");
		NativeFieldInfoPtr_CutoutTextureSTHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "CutoutTextureSTHash");
		NativeFieldInfoPtr_SrcBlendHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "SrcBlendHash");
		NativeFieldInfoPtr_DstBlendHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "DstBlendHash");
		NativeFieldInfoPtr_TargetHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "TargetHash");
		NativeFieldInfoPtr_InfoTargetHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "InfoTargetHash");
		NativeFieldInfoPtr_PrimaryBufferHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "PrimaryBufferHash");
		NativeFieldInfoPtr_HelperBufferHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "HelperBufferHash");
		NativeFieldInfoPtr_PrimaryInfoBufferHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "PrimaryInfoBufferHash");
		NativeFieldInfoPtr_HelperInfoBufferHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "HelperInfoBufferHash");
		NativeFieldInfoPtr_TransparentBlitMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "TransparentBlitMaterial");
		NativeFieldInfoPtr_EmptyFillMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "EmptyFillMaterial");
		NativeFieldInfoPtr_OutlineMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "OutlineMaterial");
		NativeFieldInfoPtr_PartialBlitMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "PartialBlitMaterial");
		NativeFieldInfoPtr_ObstacleMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ObstacleMaterial");
		NativeFieldInfoPtr_FillMaskMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "FillMaskMaterial");
		NativeFieldInfoPtr_ZPrepassMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ZPrepassMaterial");
		NativeFieldInfoPtr_OutlineMaskMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "OutlineMaskMaterial");
		NativeFieldInfoPtr_DilateMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "DilateMaterial");
		NativeFieldInfoPtr_BlurMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "BlurMaterial");
		NativeFieldInfoPtr_FinalBlitMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "FinalBlitMaterial");
		NativeFieldInfoPtr_BasicBlitMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "BasicBlitMaterial");
		NativeFieldInfoPtr_ClearStencilMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "ClearStencilMaterial");
		NativeFieldInfoPtr_targets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "targets");
		NativeFieldInfoPtr_keywords = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, "keywords");
		NativeMethodInfoPtr_LoadMaterial_Public_Static_Material_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667328);
		NativeMethodInfoPtr_InitMaterials_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667329);
		NativeMethodInfoPtr_Postprocess_Private_Static_Void_OutlineParameters_Int32_Int32_Material_Int32_Boolean_Single_byref_Int32_Rect_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667330);
		NativeMethodInfoPtr_Blit_Private_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_RenderTargetIdentifier_Material_Single_CommandBuffer_Int32_Nullable_1_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667331);
		NativeMethodInfoPtr_GetBlurShift_Private_Static_Single_BlurType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667332);
		NativeMethodInfoPtr_GetMaskingValueForMode_Private_Static_Single_OutlinableDrawingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667333);
		NativeMethodInfoPtr_ComputeEffectShift_Private_Static_Single_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667334);
		NativeMethodInfoPtr_PrepareTargets_Private_Static_Void_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667335);
		NativeMethodInfoPtr_SetupOutline_Public_Static_Void_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667336);
		NativeMethodInfoPtr_SetupDilateKeyword_Private_Static_Void_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667337);
		NativeMethodInfoPtr_SetupBlurKeyword_Private_Static_Void_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667338);
		NativeMethodInfoPtr_DrawOutlineables_Private_Static_Int32_OutlineParameters_CompareFunction_Func_2_Outlinable_Boolean_Func_2_Outlinable_Color_Func_2_Outlinable_Material_RenderStyle_OutlinableDrawingMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667339);
		NativeMethodInfoPtr_DrawFill_Private_Static_Void_OutlineParameters_RenderTargetIdentifier_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667340);
		NativeMethodInfoPtr_SetupCutout_Private_Static_Void_OutlineParameters_OutlineTarget_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667341);
		NativeMethodInfoPtr_SetupCull_Private_Static_Void_OutlineParameters_OutlineTarget_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutlineEffect>.NativeClassPtr, 100667342);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 62252, RefRangeEnd = 62265, XrefRangeStart = 62240, XrefRangeEnd = 62252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Material LoadMaterial(string shaderName)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(shaderName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadMaterial_Public_Static_Material_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 62449, RefRangeEnd = 62450, XrefRangeStart = 62265, XrefRangeEnd = 62449, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitMaterials()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitMaterials_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 62509, RefRangeEnd = 62512, XrefRangeStart = 62450, XrefRangeEnd = 62509, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Postprocess(OutlineParameters parameters, int first, int second, Material material, int iterations, bool additionalShift, float shiftValue, ref int stencil, Rect viewport, float scale)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[10];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &first;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &second;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &iterations;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &additionalShift;
		*(float**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &shiftValue;
		*(void**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref stencil);
		*(Rect**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &viewport;
		*(float**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Postprocess_Private_Static_Void_OutlineParameters_Int32_Int32_Material_Int32_Boolean_Single_byref_Int32_Rect_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 62521, RefRangeEnd = 62522, XrefRangeStart = 62512, XrefRangeEnd = 62521, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Blit(OutlineParameters parameters, RenderTargetIdentifier source, RenderTargetIdentifier destination, RenderTargetIdentifier destinationDepth, Material material, float effectSize, CommandBuffer buffer, int pass = -1, Il2CppSystem.Nullable<Rect> viewport = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[9];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &source;
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &destination;
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &destinationDepth;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &effectSize;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)buffer);
		*(int**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &pass;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)viewport));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Blit_Private_Static_Void_OutlineParameters_RenderTargetIdentifier_RenderTargetIdentifier_RenderTargetIdentifier_Material_Single_CommandBuffer_Int32_Nullable_1_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static float GetBlurShift(BlurType blurType, int iterrationsCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&blurType);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &iterrationsCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBlurShift_Private_Static_Single_BlurType_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float GetMaskingValueForMode(OutlinableDrawingMode mode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaskingValueForMode_Private_Static_Single_OutlinableDrawingMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 62522, XrefRangeEnd = 62528, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ComputeEffectShift(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeEffectShift_Private_Static_Single_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 62576, RefRangeEnd = 62577, XrefRangeStart = 62528, XrefRangeEnd = 62576, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void PrepareTargets(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareTargets_Private_Static_Void_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 63304, RefRangeEnd = 63308, XrefRangeStart = 62577, XrefRangeEnd = 63304, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupOutline(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupOutline_Public_Static_Void_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 63343, RefRangeEnd = 63344, XrefRangeStart = 63308, XrefRangeEnd = 63343, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupDilateKeyword(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupDilateKeyword_Private_Static_Void_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 63378, RefRangeEnd = 63379, XrefRangeStart = 63344, XrefRangeEnd = 63378, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupBlurKeyword(OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupBlurKeyword_Private_Static_Void_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 63430, RefRangeEnd = 63438, XrefRangeStart = 63379, XrefRangeEnd = 63430, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int DrawOutlineables(OutlineParameters parameters, CompareFunction function, Il2CppSystem.Func<Outlinable, bool> shouldRender, Il2CppSystem.Func<Outlinable, Color> colorProvider, Il2CppSystem.Func<Outlinable, Material> materialProvider, RenderStyle styleMask, OutlinableDrawingMode modeMask = OutlinableDrawingMode.Normal)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(CompareFunction**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &function;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shouldRender);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)colorProvider);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialProvider);
		*(RenderStyle**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &styleMask;
		*(OutlinableDrawingMode**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &modeMask;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawOutlineables_Private_Static_Int32_OutlineParameters_CompareFunction_Func_2_Outlinable_Boolean_Func_2_Outlinable_Color_Func_2_Outlinable_Material_RenderStyle_OutlinableDrawingMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 63681, RefRangeEnd = 63682, XrefRangeStart = 63438, XrefRangeEnd = 63681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DrawFill(OutlineParameters parameters, RenderTargetIdentifier targetSurface)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(RenderTargetIdentifier**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &targetSurface;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawFill_Private_Static_Void_OutlineParameters_RenderTargetIdentifier_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 63748, RefRangeEnd = 63755, XrefRangeStart = 63682, XrefRangeEnd = 63748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupCutout(OutlineParameters parameters, OutlineTarget target)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupCutout_Private_Static_Void_OutlineParameters_OutlineTarget_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 63755, XrefRangeEnd = 63761, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupCull(OutlineParameters parameters, OutlineTarget target)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupCull_Private_Static_Void_OutlineParameters_OutlineTarget_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public OutlineEffect(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
