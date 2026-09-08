using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppSFB;

public class WindowWrapper : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__hwnd;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Handle_Public_Virtual_Final_New_get_IntPtr_0;

	public unsafe System.IntPtr _hwnd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hwnd);
			return *(System.IntPtr*)num;
		}
		set
		{
			*(System.IntPtr*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hwnd)) = intPtr;
		}
	}

	public unsafe virtual System.IntPtr Handle
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Handle_Public_Virtual_Final_New_get_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(System.IntPtr*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static WindowWrapper()
	{
		Il2CppClassPointerStore<WindowWrapper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "SFB", "WindowWrapper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WindowWrapper>.NativeClassPtr);
		NativeFieldInfoPtr__hwnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WindowWrapper>.NativeClassPtr, "_hwnd");
		NativeMethodInfoPtr_get_Handle_Public_Virtual_Final_New_get_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WindowWrapper>.NativeClassPtr, 100664496);
	}

	public WindowWrapper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
