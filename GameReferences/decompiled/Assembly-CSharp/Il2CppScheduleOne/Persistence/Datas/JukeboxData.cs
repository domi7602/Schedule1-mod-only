using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

public class JukeboxData : GridItemData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_State;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_JukeboxState_0;

	public unsafe Jukebox.JukeboxState State
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_State);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Jukebox.JukeboxState>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_State)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jukeboxState));
		}
	}

	static JukeboxData()
	{
		Il2CppClassPointerStore<JukeboxData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "JukeboxData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<JukeboxData>.NativeClassPtr);
		NativeFieldInfoPtr_State = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JukeboxData>.NativeClassPtr, "State");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_JukeboxState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JukeboxData>.NativeClassPtr, 100669427);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 135060, RefRangeEnd = 135067, XrefRangeStart = 135060, XrefRangeEnd = 135067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe JukeboxData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, Jukebox.JukeboxState state)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<JukeboxData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &originCoordinate;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_JukeboxState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public JukeboxData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
