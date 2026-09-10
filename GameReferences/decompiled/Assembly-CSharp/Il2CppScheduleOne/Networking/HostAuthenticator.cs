using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Authenticating;
using Il2CppFishNet.Connection;
using Il2CppFishNet.Managing;
using Il2CppFishNet.Transporting;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Networking;

public class HostAuthenticator : Authenticator
{
	private static readonly IntPtr NativeFieldInfoPtr__hostHash;

	private static readonly IntPtr NativeFieldInfoPtr__allowHostAuthentication;

	private static readonly IntPtr NativeMethodInfoPtr_InitializeOnce_Public_Virtual_Void_NetworkManager_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ServerManager_OnServerConnectionState_Private_Void_ServerConnectionStateArgs_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnHostPasswordBroadcast_Private_Void_NetworkConnection_HostPasswordBroadcast_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnHostAuthenticationResult_Protected_Abstract_Virtual_New_Void_NetworkConnection_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetHostHash_Private_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_AuthenticateAsHost_Protected_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe static string _hostHash
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__hostHash, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__hostHash, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool _allowHostAuthentication
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allowHostAuthentication);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allowHostAuthentication)) = flag;
		}
	}

	static HostAuthenticator()
	{
		Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Networking", "HostAuthenticator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr);
		NativeFieldInfoPtr__hostHash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, "_hostHash");
		NativeFieldInfoPtr__allowHostAuthentication = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, "_allowHostAuthentication");
		NativeMethodInfoPtr_InitializeOnce_Public_Virtual_Void_NetworkManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669571);
		NativeMethodInfoPtr_OnDestroy_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669572);
		NativeMethodInfoPtr_ServerManager_OnServerConnectionState_Private_Void_ServerConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669573);
		NativeMethodInfoPtr_OnHostPasswordBroadcast_Private_Void_NetworkConnection_HostPasswordBroadcast_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669574);
		NativeMethodInfoPtr_OnHostAuthenticationResult_Protected_Abstract_Virtual_New_Void_NetworkConnection_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669575);
		NativeMethodInfoPtr_SetHostHash_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669576);
		NativeMethodInfoPtr_AuthenticateAsHost_Protected_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669577);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr, 100669578);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 136572, XrefRangeEnd = 136590, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void InitializeOnce(NetworkManager networkManager)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)networkManager);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_InitializeOnce_Public_Virtual_Void_NetworkManager_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 136590, XrefRangeEnd = 136610, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDestroy_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 136610, XrefRangeEnd = 136611, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&obj);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ServerManager_OnServerConnectionState_Private_Void_ServerConnectionStateArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 136611, XrefRangeEnd = 136618, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnHostPasswordBroadcast(NetworkConnection conn, HostPasswordBroadcast hpb)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)hpb));
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnHostPasswordBroadcast_Private_Void_NetworkConnection_HostPasswordBroadcast_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void OnHostAuthenticationResult(NetworkConnection conn, bool authenticated)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &authenticated;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnHostAuthenticationResult_Protected_Abstract_Virtual_New_Void_NetworkConnection_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 136653, RefRangeEnd = 136654, XrefRangeStart = 136618, XrefRangeEnd = 136653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetHostHash(int length)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&length);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetHostHash_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 136654, XrefRangeEnd = 136668, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool AuthenticateAsHost()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AuthenticateAsHost_Protected_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 136668, XrefRangeEnd = 136669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HostAuthenticator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HostAuthenticator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public HostAuthenticator(IntPtr pointer)
		: base(pointer)
	{
	}
}
