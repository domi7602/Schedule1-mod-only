using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.AvatarFramework.Animation;

public static class AAId : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DIRECTION;

	private static readonly System.IntPtr NativeFieldInfoPtr_STRAFE;

	private static readonly System.IntPtr NativeFieldInfoPtr_TIME_AIRBORNE;

	private static readonly System.IntPtr NativeFieldInfoPtr_IS_CROUCHED;

	private static readonly System.IntPtr NativeFieldInfoPtr_IS_GROUNDED;

	private static readonly System.IntPtr NativeFieldInfoPtr_JUMP;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_FORWARD;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_BACKWARD;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_LEFT;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_RIGHT;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_HEAVY_FORWARD;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_HEAVY_BACKWARD;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_HEAVY_LEFT;

	private static readonly System.IntPtr NativeFieldInfoPtr_FLINCH_HEAVY_RIGHT;

	private static readonly System.IntPtr NativeFieldInfoPtr_STANDUP_BACK;

	private static readonly System.IntPtr NativeFieldInfoPtr_STANDUP_FRONT;

	private static readonly System.IntPtr NativeFieldInfoPtr_SITTING;

	private static readonly System.IntPtr NativeFieldInfoPtr_s_CustomHashes;

	private static readonly System.IntPtr NativeMethodInfoPtr_Init_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Public_Static_Int32_String_0;

	public unsafe static int DIRECTION
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DIRECTION, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DIRECTION, (void*)(&num));
		}
	}

	public unsafe static int STRAFE
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STRAFE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STRAFE, (void*)(&num));
		}
	}

	public unsafe static int TIME_AIRBORNE
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TIME_AIRBORNE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TIME_AIRBORNE, (void*)(&num));
		}
	}

	public unsafe static int IS_CROUCHED
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IS_CROUCHED, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IS_CROUCHED, (void*)(&num));
		}
	}

	public unsafe static int IS_GROUNDED
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IS_GROUNDED, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IS_GROUNDED, (void*)(&num));
		}
	}

	public unsafe static int JUMP
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JUMP, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JUMP, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_FORWARD
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_FORWARD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_FORWARD, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_BACKWARD
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_BACKWARD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_BACKWARD, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_LEFT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_LEFT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_LEFT, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_RIGHT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_RIGHT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_RIGHT, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_HEAVY_FORWARD
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_HEAVY_FORWARD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_HEAVY_FORWARD, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_HEAVY_BACKWARD
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_HEAVY_BACKWARD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_HEAVY_BACKWARD, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_HEAVY_LEFT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_HEAVY_LEFT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_HEAVY_LEFT, (void*)(&num));
		}
	}

	public unsafe static int FLINCH_HEAVY_RIGHT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLINCH_HEAVY_RIGHT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLINCH_HEAVY_RIGHT, (void*)(&num));
		}
	}

	public unsafe static int STANDUP_BACK
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STANDUP_BACK, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STANDUP_BACK, (void*)(&num));
		}
	}

	public unsafe static int STANDUP_FRONT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_STANDUP_FRONT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_STANDUP_FRONT, (void*)(&num));
		}
	}

	public unsafe static int SITTING
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SITTING, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SITTING, (void*)(&num));
		}
	}

	public unsafe static Dictionary<string, int> s_CustomHashes
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_s_CustomHashes, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, int>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_s_CustomHashes, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	static AAId()
	{
		Il2CppClassPointerStore<AAId>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Animation", "AAId");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AAId>.NativeClassPtr);
		NativeFieldInfoPtr_DIRECTION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "DIRECTION");
		NativeFieldInfoPtr_STRAFE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "STRAFE");
		NativeFieldInfoPtr_TIME_AIRBORNE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "TIME_AIRBORNE");
		NativeFieldInfoPtr_IS_CROUCHED = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "IS_CROUCHED");
		NativeFieldInfoPtr_IS_GROUNDED = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "IS_GROUNDED");
		NativeFieldInfoPtr_JUMP = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "JUMP");
		NativeFieldInfoPtr_FLINCH_FORWARD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_FORWARD");
		NativeFieldInfoPtr_FLINCH_BACKWARD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_BACKWARD");
		NativeFieldInfoPtr_FLINCH_LEFT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_LEFT");
		NativeFieldInfoPtr_FLINCH_RIGHT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_RIGHT");
		NativeFieldInfoPtr_FLINCH_HEAVY_FORWARD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_HEAVY_FORWARD");
		NativeFieldInfoPtr_FLINCH_HEAVY_BACKWARD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_HEAVY_BACKWARD");
		NativeFieldInfoPtr_FLINCH_HEAVY_LEFT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_HEAVY_LEFT");
		NativeFieldInfoPtr_FLINCH_HEAVY_RIGHT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "FLINCH_HEAVY_RIGHT");
		NativeFieldInfoPtr_STANDUP_BACK = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "STANDUP_BACK");
		NativeFieldInfoPtr_STANDUP_FRONT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "STANDUP_FRONT");
		NativeFieldInfoPtr_SITTING = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "SITTING");
		NativeFieldInfoPtr_s_CustomHashes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AAId>.NativeClassPtr, "s_CustomHashes");
		NativeMethodInfoPtr_Init_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AAId>.NativeClassPtr, 100677639);
		NativeMethodInfoPtr_Get_Public_Static_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AAId>.NativeClassPtr, 100677640);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 222637, XrefRangeEnd = 222718, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Init()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Init_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 222732, RefRangeEnd = 222736, XrefRangeStart = 222718, XrefRangeEnd = 222732, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int Get(string id)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(id);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Public_Static_Int32_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public AAId(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
