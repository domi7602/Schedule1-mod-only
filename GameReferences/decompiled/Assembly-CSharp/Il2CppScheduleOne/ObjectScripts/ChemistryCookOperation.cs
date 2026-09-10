using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.StationFramework;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.ObjectScripts;

public class ChemistryCookOperation : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_recipe;

	private static readonly System.IntPtr NativeFieldInfoPtr_RecipeID;

	private static readonly System.IntPtr NativeFieldInfoPtr_ProductQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_StartLiquidColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_LiquidLevel;

	private static readonly System.IntPtr NativeFieldInfoPtr_CurrentTime;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Recipe_Public_get_StationRecipe_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_StationRecipe_EQuality_Color_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Color_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Progress_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsComplete_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__get_Recipe_b__2_0_Private_Boolean_StationRecipe_0;

	public unsafe StationRecipe recipe
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_recipe);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StationRecipe>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_recipe)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stationRecipe));
		}
	}

	public unsafe string RecipeID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RecipeID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RecipeID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EQuality ProductQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuality);
			return *(EQuality*)num;
		}
		set
		{
			*(EQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuality)) = eQuality;
		}
	}

	public unsafe Color StartLiquidColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartLiquidColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartLiquidColor)) = color;
		}
	}

	public unsafe float LiquidLevel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LiquidLevel);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LiquidLevel)) = num;
		}
	}

	public unsafe int CurrentTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CurrentTime)) = num;
		}
	}

	public unsafe StationRecipe Recipe
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 250310, RefRangeEnd = 250322, XrefRangeStart = 250292, XrefRangeEnd = 250310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Recipe_Public_get_StationRecipe_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StationRecipe>(intPtr) : null;
		}
	}

	static ChemistryCookOperation()
	{
		Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ObjectScripts", "ChemistryCookOperation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr);
		NativeFieldInfoPtr_recipe = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, "recipe");
		NativeFieldInfoPtr_RecipeID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, "RecipeID");
		NativeFieldInfoPtr_ProductQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, "ProductQuality");
		NativeFieldInfoPtr_StartLiquidColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, "StartLiquidColor");
		NativeFieldInfoPtr_LiquidLevel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, "LiquidLevel");
		NativeFieldInfoPtr_CurrentTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, "CurrentTime");
		NativeMethodInfoPtr_get_Recipe_Public_get_StationRecipe_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680453);
		NativeMethodInfoPtr__ctor_Public_Void_StationRecipe_EQuality_Color_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680454);
		NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Color_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680455);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680456);
		NativeMethodInfoPtr_Progress_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680457);
		NativeMethodInfoPtr_IsComplete_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680458);
		NativeMethodInfoPtr__get_Recipe_b__2_0_Private_Boolean_StationRecipe_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr, 100680459);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 250325, RefRangeEnd = 250327, XrefRangeStart = 250322, XrefRangeEnd = 250325, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ChemistryCookOperation(StationRecipe recipe, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime = 0)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)recipe);
		*(EQuality**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &productQuality;
		*(Color**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startLiquidColor;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &liquidLevel;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_StationRecipe_EQuality_Color_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 250329, RefRangeEnd = 250331, XrefRangeStart = 250327, XrefRangeEnd = 250329, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ChemistryCookOperation(string recipeID, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime = 0)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(recipeID);
		*(EQuality**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &productQuality;
		*(Color**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startLiquidColor;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &liquidLevel;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &currentTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EQuality_Color_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ChemistryCookOperation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ChemistryCookOperation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 250331, XrefRangeEnd = 250332, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Progress(int mins)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mins);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Progress_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 250332, XrefRangeEnd = 250333, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsComplete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsComplete_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 250333, XrefRangeEnd = 250336, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool _get_Recipe_b__2_0(StationRecipe r)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)r);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__get_Recipe_b__2_0_Private_Boolean_StationRecipe_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ChemistryCookOperation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
