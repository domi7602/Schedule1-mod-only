using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGModuleSlot : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Module_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Info_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Origin_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__DropZone_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_mLinkedSlots;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Module_Public_get_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Module_Internal_set_Void_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Info_Public_get_SlotInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Info_Internal_set_Void_SlotInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Origin_Public_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Origin_Public_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DropZone_Public_get_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_DropZone_Public_set_Void_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsLinked_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsLinkedAndConfigured_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnRequestModule_Public_get_IOnRequestProcessing_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PathProvider_Public_get_IPathProvider_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ExternalInput_Public_get_IExternalInput_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LinkedSlots_Public_get_List_1_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Name_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasLinkTo_Public_Boolean_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLinkedModules_Public_List_1_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LinkTo_Public_Virtual_New_Void_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LinkInputAndOutput_Protected_Static_Void_CGModuleSlot_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnlinkFrom_Public_Virtual_New_Void_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnlinkAll_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadLinkedSlots_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetInfoFromField_Public_Void_FieldInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Implicit_Public_Static_Boolean_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CGModule _Module_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Module_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Module_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModule));
		}
	}

	public unsafe SlotInfo _Info_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Info_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SlotInfo>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Info_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotInfo));
		}
	}

	public unsafe Vector2 _Origin_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Origin_k__BackingField);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Origin_k__BackingField)) = vector;
		}
	}

	public unsafe Rect _DropZone_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DropZone_k__BackingField);
			return *(Rect*)num;
		}
		set
		{
			*(Rect*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DropZone_k__BackingField)) = rect;
		}
	}

	public unsafe List<CGModuleSlot> mLinkedSlots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mLinkedSlots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleSlot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mLinkedSlots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe CGModule Module
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Module_Public_get_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29107, RefRangeEnd = 29109, XrefRangeStart = 29107, XrefRangeEnd = 29109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Module_Internal_set_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SlotInfo Info
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Info_Public_get_SlotInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SlotInfo>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Info_Internal_set_Void_SlotInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Vector2 Origin
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Origin_Public_get_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Origin_Public_set_Void_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Rect DropZone
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DropZone_Public_get_Rect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 49913, RefRangeEnd = 49917, XrefRangeStart = 49913, XrefRangeEnd = 49913, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DropZone_Public_set_Void_Rect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsLinked
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 49919, RefRangeEnd = 49929, XrefRangeStart = 49917, XrefRangeEnd = 49919, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsLinked_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsLinkedAndConfigured
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49929, XrefRangeEnd = 49940, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsLinkedAndConfigured_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe IOnRequestProcessing OnRequestModule
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49940, XrefRangeEnd = 49943, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnRequestModule_Public_get_IOnRequestProcessing_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IOnRequestProcessing>(intPtr) : null;
		}
	}

	public unsafe IPathProvider PathProvider
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 49946, RefRangeEnd = 49953, XrefRangeStart = 49943, XrefRangeEnd = 49946, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PathProvider_Public_get_IPathProvider_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IPathProvider>(intPtr) : null;
		}
	}

	public unsafe IExternalInput ExternalInput
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 49956, RefRangeEnd = 49957, XrefRangeStart = 49953, XrefRangeEnd = 49956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ExternalInput_Public_get_IExternalInput_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IExternalInput>(intPtr) : null;
		}
	}

	public unsafe List<CGModuleSlot> LinkedSlots
	{
		[CallerCount(35)]
		[CachedScanResults(RefRangeStart = 49963, RefRangeEnd = 49998, XrefRangeStart = 49957, XrefRangeEnd = 49963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LinkedSlots_Public_get_List_1_CGModuleSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleSlot>>(intPtr) : null;
		}
	}

	public unsafe int Count
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 50000, RefRangeEnd = 50002, XrefRangeStart = 49998, XrefRangeEnd = 50000, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Count_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe string Name
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50002, XrefRangeEnd = 50003, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Name_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static CGModuleSlot()
	{
		Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGModuleSlot");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr);
		NativeFieldInfoPtr__Module_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, "<Module>k__BackingField");
		NativeFieldInfoPtr__Info_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, "<Info>k__BackingField");
		NativeFieldInfoPtr__Origin_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, "<Origin>k__BackingField");
		NativeFieldInfoPtr__DropZone_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, "<DropZone>k__BackingField");
		NativeFieldInfoPtr_mLinkedSlots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, "mLinkedSlots");
		NativeMethodInfoPtr_get_Module_Public_get_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666071);
		NativeMethodInfoPtr_set_Module_Internal_set_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666072);
		NativeMethodInfoPtr_get_Info_Public_get_SlotInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666073);
		NativeMethodInfoPtr_set_Info_Internal_set_Void_SlotInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666074);
		NativeMethodInfoPtr_get_Origin_Public_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666075);
		NativeMethodInfoPtr_set_Origin_Public_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666076);
		NativeMethodInfoPtr_get_DropZone_Public_get_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666077);
		NativeMethodInfoPtr_set_DropZone_Public_set_Void_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666078);
		NativeMethodInfoPtr_get_IsLinked_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666079);
		NativeMethodInfoPtr_get_IsLinkedAndConfigured_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666080);
		NativeMethodInfoPtr_get_OnRequestModule_Public_get_IOnRequestProcessing_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666081);
		NativeMethodInfoPtr_get_PathProvider_Public_get_IPathProvider_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666082);
		NativeMethodInfoPtr_get_ExternalInput_Public_get_IExternalInput_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666083);
		NativeMethodInfoPtr_get_LinkedSlots_Public_get_List_1_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666084);
		NativeMethodInfoPtr_get_Count_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666085);
		NativeMethodInfoPtr_get_Name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666086);
		NativeMethodInfoPtr_HasLinkTo_Public_Boolean_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666087);
		NativeMethodInfoPtr_GetLinkedModules_Public_List_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666088);
		NativeMethodInfoPtr_LinkTo_Public_Virtual_New_Void_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666089);
		NativeMethodInfoPtr_LinkInputAndOutput_Protected_Static_Void_CGModuleSlot_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666090);
		NativeMethodInfoPtr_UnlinkFrom_Public_Virtual_New_Void_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666091);
		NativeMethodInfoPtr_UnlinkAll_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666092);
		NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666093);
		NativeMethodInfoPtr_LoadLinkedSlots_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666094);
		NativeMethodInfoPtr_SetInfoFromField_Public_Void_FieldInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666095);
		NativeMethodInfoPtr_op_Implicit_Public_Static_Boolean_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666096);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666097);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr, 100666098);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 50011, RefRangeEnd = 50015, XrefRangeStart = 50003, XrefRangeEnd = 50011, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool HasLinkTo(CGModuleSlot other)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasLinkTo_Public_Boolean_CGModuleSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 50043, RefRangeEnd = 50047, XrefRangeStart = 50015, XrefRangeEnd = 50043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModule> GetLinkedModules()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLinkedModules_Public_List_1_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModule>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 50057, RefRangeEnd = 50059, XrefRangeStart = 50047, XrefRangeEnd = 50057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void LinkTo(CGModuleSlot other)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LinkTo_Public_Virtual_New_Void_CGModuleSlot_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 50092, RefRangeEnd = 50094, XrefRangeStart = 50059, XrefRangeEnd = 50092, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LinkInputAndOutput(CGModuleSlot inputSlot, CGModuleSlot outputSlot)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputSlot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LinkInputAndOutput_Protected_Static_Void_CGModuleSlot_CGModuleSlot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 50111, RefRangeEnd = 50113, XrefRangeStart = 50094, XrefRangeEnd = 50111, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void UnlinkFrom(CGModuleSlot other)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_UnlinkFrom_Public_Virtual_New_Void_CGModuleSlot_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50113, XrefRangeEnd = 50134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void UnlinkAll()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_UnlinkAll_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50134, XrefRangeEnd = 50135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReInitializeLinkedSlots()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void LoadLinkedSlots()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LoadLinkedSlots_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 50169, RefRangeEnd = 50171, XrefRangeStart = 50135, XrefRangeEnd = 50169, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInfoFromField(FieldInfo fieldInfo)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fieldInfo);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInfoFromField_Public_Void_FieldInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 46815, RefRangeEnd = 46828, XrefRangeStart = 46815, XrefRangeEnd = 46828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static implicit operator bool(CGModuleSlot a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Implicit_Public_Static_Boolean_CGModuleSlot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50171, XrefRangeEnd = 50183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleSlot()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGModuleSlot>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGModuleSlot(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
