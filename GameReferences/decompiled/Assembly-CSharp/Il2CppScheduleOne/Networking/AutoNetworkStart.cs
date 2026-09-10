using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Networking;

public class AutoNetworkStart : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EAutoStartType")]
	public enum EAutoStartType
	{
		Disabled,
		Host,
		Server,
		Client
	}

	private static readonly IntPtr NativeFieldInfoPtr__autoStartType;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EAutoStartType _autoStartType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__autoStartType);
			return *(EAutoStartType*)num;
		}
		set
		{
			*(EAutoStartType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__autoStartType)) = eAutoStartType;
		}
	}

	static AutoNetworkStart()
	{
		Il2CppClassPointerStore<AutoNetworkStart>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Networking", "AutoNetworkStart");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AutoNetworkStart>.NativeClassPtr);
		NativeFieldInfoPtr__autoStartType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AutoNetworkStart>.NativeClassPtr, "_autoStartType");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AutoNetworkStart>.NativeClassPtr, 100669541);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AutoNetworkStart()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AutoNetworkStart>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AutoNetworkStart(IntPtr pointer)
		: base(pointer)
	{
	}
}
