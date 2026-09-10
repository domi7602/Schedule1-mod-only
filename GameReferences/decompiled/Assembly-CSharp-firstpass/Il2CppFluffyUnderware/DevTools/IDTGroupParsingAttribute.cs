using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class IDTGroupParsingAttribute : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_Path_Public_Abstract_Virtual_New_get_String_0;

	public unsafe virtual string Path
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Path_Public_Abstract_Virtual_New_get_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static IDTGroupParsingAttribute()
	{
		Il2CppClassPointerStore<IDTGroupParsingAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "IDTGroupParsingAttribute");
		NativeMethodInfoPtr_get_Path_Public_Abstract_Virtual_New_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IDTGroupParsingAttribute>.NativeClassPtr, 100663866);
	}

	public IDTGroupParsingAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
