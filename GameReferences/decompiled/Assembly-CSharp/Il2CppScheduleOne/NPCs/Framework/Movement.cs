using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Movement : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_WalkSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_SprintSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_CanOpenDoors;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Movement_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float WalkSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WalkSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WalkSpeed)) = num;
		}
	}

	public unsafe float SprintSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SprintSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SprintSpeed)) = num;
		}
	}

	public unsafe bool CanOpenDoors
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanOpenDoors);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanOpenDoors)) = flag;
		}
	}

	static Movement()
	{
		Il2CppClassPointerStore<Movement>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Movement");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Movement>.NativeClassPtr);
		NativeFieldInfoPtr_WalkSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Movement>.NativeClassPtr, "WalkSpeed");
		NativeFieldInfoPtr_SprintSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Movement>.NativeClassPtr, "SprintSpeed");
		NativeFieldInfoPtr_CanOpenDoors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Movement>.NativeClassPtr, "CanOpenDoors");
		NativeMethodInfoPtr_GetCopy_Public_Movement_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Movement>.NativeClassPtr, 100682826);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Movement>.NativeClassPtr, 100682827);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272293, XrefRangeEnd = 272297, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Movement GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Movement_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Movement>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272297, XrefRangeEnd = 272298, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Movement()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Movement>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Movement(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
