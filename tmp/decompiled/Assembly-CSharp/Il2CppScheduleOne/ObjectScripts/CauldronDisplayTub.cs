using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.ObjectScripts;

public class CauldronDisplayTub : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EContents")]
	public enum EContents
	{
		None,
		CocaLeaf
	}

	private static readonly IntPtr NativeFieldInfoPtr_CocaLeafContainer;

	private static readonly IntPtr NativeFieldInfoPtr_Container_Min;

	private static readonly IntPtr NativeFieldInfoPtr_Container_Max;

	private static readonly IntPtr NativeMethodInfoPtr_Configure_Public_Void_EContents_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Transform CocaLeafContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CocaLeafContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CocaLeafContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform Container_Min
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Container_Min);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Container_Min)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform Container_Max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Container_Max);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Container_Max)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	static CauldronDisplayTub()
	{
		Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ObjectScripts", "CauldronDisplayTub");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr);
		NativeFieldInfoPtr_CocaLeafContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr, "CocaLeafContainer");
		NativeFieldInfoPtr_Container_Min = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr, "Container_Min");
		NativeFieldInfoPtr_Container_Max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr, "Container_Max");
		NativeMethodInfoPtr_Configure_Public_Void_EContents_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr, 100680430);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr, 100680431);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 250141, RefRangeEnd = 250143, XrefRangeStart = 250128, XrefRangeEnd = 250141, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Configure(EContents contentsType, float fillLevel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&contentsType);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &fillLevel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Configure_Public_Void_EContents_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CauldronDisplayTub()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CauldronDisplayTub>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CauldronDisplayTub(IntPtr pointer)
		: base(pointer)
	{
	}
}
