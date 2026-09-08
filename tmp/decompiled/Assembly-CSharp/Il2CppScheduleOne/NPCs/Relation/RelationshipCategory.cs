using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.NPCs.Relation;

public class RelationshipCategory : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Hostile_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Unfriendly_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Neutral_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Friendly_Color;

	private static readonly System.IntPtr NativeFieldInfoPtr_Loyal_Color;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCategory_Public_Static_ERelationshipCategory_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetColor_Public_Static_Color32_ERelationshipCategory_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Color32 Hostile_Color
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Hostile_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Hostile_Color, (void*)(&color));
		}
	}

	public unsafe static Color32 Unfriendly_Color
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Unfriendly_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Unfriendly_Color, (void*)(&color));
		}
	}

	public unsafe static Color32 Neutral_Color
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Neutral_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Neutral_Color, (void*)(&color));
		}
	}

	public unsafe static Color32 Friendly_Color
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Friendly_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Friendly_Color, (void*)(&color));
		}
	}

	public unsafe static Color32 Loyal_Color
	{
		get
		{
			Unsafe.SkipInit(out Color32 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Loyal_Color, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Loyal_Color, (void*)(&color));
		}
	}

	static RelationshipCategory()
	{
		Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Relation", "RelationshipCategory");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr);
		NativeFieldInfoPtr_Hostile_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, "Hostile_Color");
		NativeFieldInfoPtr_Unfriendly_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, "Unfriendly_Color");
		NativeFieldInfoPtr_Neutral_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, "Neutral_Color");
		NativeFieldInfoPtr_Friendly_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, "Friendly_Color");
		NativeFieldInfoPtr_Loyal_Color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, "Loyal_Color");
		NativeMethodInfoPtr_GetCategory_Public_Static_ERelationshipCategory_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, 100682709);
		NativeMethodInfoPtr_GetColor_Public_Static_Color32_ERelationshipCategory_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, 100682710);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr, 100682711);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 271761, RefRangeEnd = 271763, XrefRangeStart = 271761, XrefRangeEnd = 271761, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ERelationshipCategory GetCategory(float delta)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&delta);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCategory_Public_Static_ERelationshipCategory_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ERelationshipCategory*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 271771, RefRangeEnd = 271772, XrefRangeStart = 271763, XrefRangeEnd = 271771, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color32 GetColor(ERelationshipCategory category)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&category);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColor_Public_Static_Color32_ERelationshipCategory_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color32*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RelationshipCategory()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RelationshipCategory>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RelationshipCategory(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
