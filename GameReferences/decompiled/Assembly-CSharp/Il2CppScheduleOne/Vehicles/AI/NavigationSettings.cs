using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vehicles.AI;

public class NavigationSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_endAtRoad;

	private static readonly System.IntPtr NativeFieldInfoPtr_ensureProximityToGraph;

	private static readonly System.IntPtr NativeFieldInfoPtr_teleportToGraphIfCalculationFails;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool endAtRoad
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endAtRoad);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endAtRoad)) = flag;
		}
	}

	public unsafe bool ensureProximityToGraph
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ensureProximityToGraph);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ensureProximityToGraph)) = flag;
		}
	}

	public unsafe bool teleportToGraphIfCalculationFails
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_teleportToGraphIfCalculationFails);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_teleportToGraphIfCalculationFails)) = flag;
		}
	}

	static NavigationSettings()
	{
		Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "NavigationSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr);
		NativeFieldInfoPtr_endAtRoad = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "endAtRoad");
		NativeFieldInfoPtr_ensureProximityToGraph = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "ensureProximityToGraph");
		NativeFieldInfoPtr_teleportToGraphIfCalculationFails = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, "teleportToGraphIfCalculationFails");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr, 100666374);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95333, XrefRangeEnd = 95334, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NavigationSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NavigationSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NavigationSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
