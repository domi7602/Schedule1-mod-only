using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.EntityFramework;

public class IProceduralTileContainer : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_ProceduralTiles_Public_Abstract_Virtual_New_get_List_1_ProceduralTile_0;

	public unsafe virtual List<ProceduralTile> ProceduralTiles
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_ProceduralTiles_Public_Abstract_Virtual_New_get_List_1_ProceduralTile_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<ProceduralTile>>(intPtr) : null;
		}
	}

	static IProceduralTileContainer()
	{
		Il2CppClassPointerStore<IProceduralTileContainer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.EntityFramework", "IProceduralTileContainer");
		NativeMethodInfoPtr_get_ProceduralTiles_Public_Abstract_Virtual_New_get_List_1_ProceduralTile_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IProceduralTileContainer>.NativeClassPtr, 100673104);
	}

	public IProceduralTileContainer(IntPtr pointer)
		: base(pointer)
	{
	}
}
