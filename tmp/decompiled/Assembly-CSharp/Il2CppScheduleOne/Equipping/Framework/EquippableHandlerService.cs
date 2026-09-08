using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Configuration;
using Il2CppScheduleOne.Core.Equipping.Framework;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Equipping.Framework;

public static class EquippableHandlerService : Il2CppSystem.Object
{
	public class HandlerInfo : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_DataType;

		private static readonly System.IntPtr NativeFieldInfoPtr_HandlerType;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Type_Type_0;

		public unsafe Il2CppSystem.Type DataType
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataType);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataType)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type));
			}
		}

		public unsafe Il2CppSystem.Type HandlerType
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandlerType);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HandlerType)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type));
			}
		}

		static HandlerInfo()
		{
			Il2CppClassPointerStore<HandlerInfo>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, "HandlerInfo");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HandlerInfo>.NativeClassPtr);
			NativeFieldInfoPtr_DataType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HandlerInfo>.NativeClassPtr, "DataType");
			NativeFieldInfoPtr_HandlerType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HandlerInfo>.NativeClassPtr, "HandlerType");
			NativeMethodInfoPtr__ctor_Public_Void_Type_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HandlerInfo>.NativeClassPtr, 100679932);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 245106, RefRangeEnd = 245108, XrefRangeStart = 245074, XrefRangeEnd = 245106, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe HandlerInfo(Il2CppSystem.Type dataType, Il2CppSystem.Type handlerType)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HandlerInfo>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dataType);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)handlerType);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Type_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public HandlerInfo(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.Equipping.Framework.EquippableHandlerService+<>c__DisplayClass5_0")]
	public sealed class __c__DisplayClass5_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_equippedData;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetHandlerPrefab_b__0_Internal_Boolean_HandlerInfo_0;

		public unsafe EquippableData equippedData
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equippedData);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EquippableData>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_equippedData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equippableData));
			}
		}

		static __c__DisplayClass5_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass5_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, "<>c__DisplayClass5_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass5_0>.NativeClassPtr);
			NativeFieldInfoPtr_equippedData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass5_0>.NativeClassPtr, "equippedData");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass5_0>.NativeClassPtr, 100679933);
			NativeMethodInfoPtr__GetHandlerPrefab_b__0_Internal_Boolean_HandlerInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass5_0>.NativeClassPtr, 100679934);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass5_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass5_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245108, XrefRangeEnd = 245114, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetHandlerPrefab_b__0(HandlerInfo h)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)h);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetHandlerPrefab_b__0_Internal_Boolean_HandlerInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass5_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__configuration;

	private static readonly System.IntPtr NativeFieldInfoPtr__defaultHandlers;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetupHandlerKeys_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHandlerPrefab_Public_Static_IEquippedItemHandler_EquippableData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Internal_Static_Void_PDM_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Internal_Static_Void_BaseConfiguration_PDM_0;

	public unsafe static EquipConfiguration _configuration
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__configuration, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<EquipConfiguration>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__configuration, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equipConfiguration));
		}
	}

	public unsafe static List<HandlerInfo> _defaultHandlers
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__defaultHandlers, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<HandlerInfo>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__defaultHandlers, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static EquippableHandlerService()
	{
		Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "EquippableHandlerService");
		NativeFieldInfoPtr__configuration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, "_configuration");
		NativeFieldInfoPtr__defaultHandlers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, "_defaultHandlers");
		NativeMethodInfoPtr_SetupHandlerKeys_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, 100679928);
		NativeMethodInfoPtr_GetHandlerPrefab_Public_Static_IEquippedItemHandler_EquippableData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, 100679929);
		NativeMethodInfoPtr_Method_Internal_Static_Void_PDM_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, 100679930);
		NativeMethodInfoPtr_Method_Internal_Static_Void_BaseConfiguration_PDM_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableHandlerService>.NativeClassPtr, 100679931);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 245150, RefRangeEnd = 245151, XrefRangeStart = 245114, XrefRangeEnd = 245150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetupHandlerKeys()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupHandlerKeys_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 245230, RefRangeEnd = 245231, XrefRangeStart = 245151, XrefRangeEnd = 245230, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEquippedItemHandler GetHandlerPrefab(EquippableData equippedData)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equippedData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHandlerPrefab_Public_Static_IEquippedItemHandler_EquippableData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEquippedItemHandler>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245231, XrefRangeEnd = 245245, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Method_Internal_Static_Void_PDM_0()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Internal_Static_Void_PDM_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245245, XrefRangeEnd = 245297, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Method_Internal_Static_Void_BaseConfiguration_PDM_0(BaseConfiguration config)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)config);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Internal_Static_Void_BaseConfiguration_PDM_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EquippableHandlerService(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
