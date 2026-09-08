using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Serializing;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Equipping.Framework;

public static class INetworkedEquippableUserSerializer : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_WriteINetworkedEquippableUser_Public_Static_Void_Writer_INetworkedEquippableUser_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadINetworkedEquippableUser_Public_Static_INetworkedEquippableUser_Reader_0;

	static INetworkedEquippableUserSerializer()
	{
		Il2CppClassPointerStore<INetworkedEquippableUserSerializer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "INetworkedEquippableUserSerializer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<INetworkedEquippableUserSerializer>.NativeClassPtr);
		NativeMethodInfoPtr_WriteINetworkedEquippableUser_Public_Static_Void_Writer_INetworkedEquippableUser_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<INetworkedEquippableUserSerializer>.NativeClassPtr, 100679944);
		NativeMethodInfoPtr_ReadINetworkedEquippableUser_Public_Static_INetworkedEquippableUser_Reader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<INetworkedEquippableUserSerializer>.NativeClassPtr, 100679945);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245318, XrefRangeEnd = 245324, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void WriteINetworkedEquippableUser(this Writer writer, INetworkedEquippableUser value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)writer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteINetworkedEquippableUser_Public_Static_Void_Writer_INetworkedEquippableUser_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245324, XrefRangeEnd = 245346, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static INetworkedEquippableUser ReadINetworkedEquippableUser(this Reader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadINetworkedEquippableUser_Public_Static_INetworkedEquippableUser_Reader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<INetworkedEquippableUser>(intPtr) : null;
	}

	public INetworkedEquippableUserSerializer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
