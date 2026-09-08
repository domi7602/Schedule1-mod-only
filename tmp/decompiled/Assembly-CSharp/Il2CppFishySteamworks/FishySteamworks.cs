using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Managing;
using Il2CppFishNet.Transporting;
using Il2CppFishySteamworks.Client;
using Il2CppFishySteamworks.Server;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFishySteamworks;

public class FishySteamworks : Transport
{
	private static readonly System.IntPtr NativeFieldInfoPtr_LocalUserSteamID;

	private static readonly System.IntPtr NativeFieldInfoPtr__serverBindAddress;

	private static readonly System.IntPtr NativeFieldInfoPtr__port;

	private static readonly System.IntPtr NativeFieldInfoPtr__maximumClients;

	private static readonly System.IntPtr NativeFieldInfoPtr__peerToPeer;

	private static readonly System.IntPtr NativeFieldInfoPtr__clientAddress;

	private static readonly System.IntPtr NativeFieldInfoPtr__mtus;

	private static readonly System.IntPtr NativeFieldInfoPtr__client;

	private static readonly System.IntPtr NativeFieldInfoPtr__clientHost;

	private static readonly System.IntPtr NativeFieldInfoPtr__server;

	private static readonly System.IntPtr NativeFieldInfoPtr__shutdownCalled;

	private static readonly System.IntPtr NativeFieldInfoPtr_CLIENT_HOST_ID;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnClientConnectionState;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnServerConnectionState;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnRemoteConnectionState;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnClientReceivedData;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnServerReceivedData;

	private static readonly System.IntPtr NativeMethodInfoPtr_Finalize_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Virtual_Void_NetworkManager_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CreateChannelData_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitializeRelayNetworkAccess_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsNetworkAccessAvailable_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConnectionAddress_Public_Virtual_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnClientConnectionState_Public_Virtual_add_Void_Action_1_ClientConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnClientConnectionState_Public_Virtual_rem_Void_Action_1_ClientConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnServerConnectionState_Public_Virtual_add_Void_Action_1_ServerConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnServerConnectionState_Public_Virtual_rem_Void_Action_1_ServerConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnRemoteConnectionState_Public_Virtual_add_Void_Action_1_RemoteConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnRemoteConnectionState_Public_Virtual_rem_Void_Action_1_RemoteConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConnectionState_Public_Virtual_LocalConnectionState_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConnectionState_Public_Virtual_RemoteConnectionState_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleClientConnectionState_Public_Virtual_Void_ClientConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleServerConnectionState_Public_Virtual_Void_ServerConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleRemoteConnectionState_Public_Virtual_Void_RemoteConnectionStateArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IterateIncoming_Public_Virtual_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IterateOutgoing_Public_Virtual_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnClientReceivedData_Public_Virtual_add_Void_Action_1_ClientReceivedDataArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnClientReceivedData_Public_Virtual_rem_Void_Action_1_ClientReceivedDataArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleClientReceivedDataArgs_Public_Virtual_Void_ClientReceivedDataArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnServerReceivedData_Public_Virtual_add_Void_Action_1_ServerReceivedDataArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnServerReceivedData_Public_Virtual_rem_Void_Action_1_ServerReceivedDataArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleServerReceivedDataArgs_Public_Virtual_Void_ServerReceivedDataArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SendToServer_Public_Virtual_Void_Byte_ArraySegment_1_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SendToClient_Public_Virtual_Void_Byte_ArraySegment_1_Byte_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaximumClients_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMaximumClients_Public_Virtual_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetClientAddress_Public_Virtual_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetServerBindAddress_Public_Virtual_Void_String_IPAddressType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPort_Public_Virtual_Void_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartConnection_Public_Virtual_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopConnection_Public_Virtual_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopConnection_Public_Virtual_Boolean_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Shutdown_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartServer_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopServer_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartClient_Private_Boolean_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopClient_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopClient_Private_Boolean_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMTU_Public_Virtual_Int32_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ulong LocalUserSteamID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LocalUserSteamID);
			return *(ulong*)num;
		}
		set
		{
			*(ulong*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LocalUserSteamID)) = num;
		}
	}

	public unsafe string _serverBindAddress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__serverBindAddress);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__serverBindAddress)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe ushort _port
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__port);
			return *(ushort*)num;
		}
		set
		{
			*(ushort*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__port)) = num;
		}
	}

	public unsafe ushort _maximumClients
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maximumClients);
			return *(ushort*)num;
		}
		set
		{
			*(ushort*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maximumClients)) = num;
		}
	}

	public unsafe bool _peerToPeer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__peerToPeer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__peerToPeer)) = flag;
		}
	}

	public unsafe string _clientAddress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientAddress);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientAddress)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppStructArray<int> _mtus
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mtus);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mtus)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe ClientSocket _client
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__client);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ClientSocket>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__client)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clientSocket));
		}
	}

	public unsafe ClientHostSocket _clientHost
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientHost);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ClientHostSocket>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientHost)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clientHostSocket));
		}
	}

	public unsafe ServerSocket _server
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__server);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ServerSocket>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__server)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)serverSocket));
		}
	}

	public unsafe bool _shutdownCalled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shutdownCalled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__shutdownCalled)) = flag;
		}
	}

	public unsafe static int CLIENT_HOST_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CLIENT_HOST_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CLIENT_HOST_ID, (void*)(&num));
		}
	}

	public unsafe Il2CppSystem.Action<ClientConnectionStateArgs> OnClientConnectionState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnClientConnectionState);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<ClientConnectionStateArgs>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnClientConnectionState)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe Il2CppSystem.Action<ServerConnectionStateArgs> OnServerConnectionState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnServerConnectionState);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<ServerConnectionStateArgs>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnServerConnectionState)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe Il2CppSystem.Action<RemoteConnectionStateArgs> OnRemoteConnectionState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnRemoteConnectionState);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<RemoteConnectionStateArgs>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnRemoteConnectionState)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe Il2CppSystem.Action<ClientReceivedDataArgs> OnClientReceivedData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnClientReceivedData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<ClientReceivedDataArgs>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnClientReceivedData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe Il2CppSystem.Action<ServerReceivedDataArgs> OnServerReceivedData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnServerReceivedData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<ServerReceivedDataArgs>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnServerReceivedData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	static FishySteamworks()
	{
		Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishySteamworks", "FishySteamworks");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr);
		NativeFieldInfoPtr_LocalUserSteamID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "LocalUserSteamID");
		NativeFieldInfoPtr__serverBindAddress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_serverBindAddress");
		NativeFieldInfoPtr__port = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_port");
		NativeFieldInfoPtr__maximumClients = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_maximumClients");
		NativeFieldInfoPtr__peerToPeer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_peerToPeer");
		NativeFieldInfoPtr__clientAddress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_clientAddress");
		NativeFieldInfoPtr__mtus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_mtus");
		NativeFieldInfoPtr__client = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_client");
		NativeFieldInfoPtr__clientHost = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_clientHost");
		NativeFieldInfoPtr__server = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_server");
		NativeFieldInfoPtr__shutdownCalled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "_shutdownCalled");
		NativeFieldInfoPtr_CLIENT_HOST_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "CLIENT_HOST_ID");
		NativeFieldInfoPtr_OnClientConnectionState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "OnClientConnectionState");
		NativeFieldInfoPtr_OnServerConnectionState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "OnServerConnectionState");
		NativeFieldInfoPtr_OnRemoteConnectionState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "OnRemoteConnectionState");
		NativeFieldInfoPtr_OnClientReceivedData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "OnClientReceivedData");
		NativeFieldInfoPtr_OnServerReceivedData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, "OnServerReceivedData");
		NativeMethodInfoPtr_Finalize_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664853);
		NativeMethodInfoPtr_Initialize_Public_Virtual_Void_NetworkManager_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664854);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664855);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664856);
		NativeMethodInfoPtr_CreateChannelData_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664857);
		NativeMethodInfoPtr_InitializeRelayNetworkAccess_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664858);
		NativeMethodInfoPtr_IsNetworkAccessAvailable_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664859);
		NativeMethodInfoPtr_GetConnectionAddress_Public_Virtual_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664860);
		NativeMethodInfoPtr_add_OnClientConnectionState_Public_Virtual_add_Void_Action_1_ClientConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664861);
		NativeMethodInfoPtr_remove_OnClientConnectionState_Public_Virtual_rem_Void_Action_1_ClientConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664862);
		NativeMethodInfoPtr_add_OnServerConnectionState_Public_Virtual_add_Void_Action_1_ServerConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664863);
		NativeMethodInfoPtr_remove_OnServerConnectionState_Public_Virtual_rem_Void_Action_1_ServerConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664864);
		NativeMethodInfoPtr_add_OnRemoteConnectionState_Public_Virtual_add_Void_Action_1_RemoteConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664865);
		NativeMethodInfoPtr_remove_OnRemoteConnectionState_Public_Virtual_rem_Void_Action_1_RemoteConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664866);
		NativeMethodInfoPtr_GetConnectionState_Public_Virtual_LocalConnectionState_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664867);
		NativeMethodInfoPtr_GetConnectionState_Public_Virtual_RemoteConnectionState_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664868);
		NativeMethodInfoPtr_HandleClientConnectionState_Public_Virtual_Void_ClientConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664869);
		NativeMethodInfoPtr_HandleServerConnectionState_Public_Virtual_Void_ServerConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664870);
		NativeMethodInfoPtr_HandleRemoteConnectionState_Public_Virtual_Void_RemoteConnectionStateArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664871);
		NativeMethodInfoPtr_IterateIncoming_Public_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664872);
		NativeMethodInfoPtr_IterateOutgoing_Public_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664873);
		NativeMethodInfoPtr_add_OnClientReceivedData_Public_Virtual_add_Void_Action_1_ClientReceivedDataArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664874);
		NativeMethodInfoPtr_remove_OnClientReceivedData_Public_Virtual_rem_Void_Action_1_ClientReceivedDataArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664875);
		NativeMethodInfoPtr_HandleClientReceivedDataArgs_Public_Virtual_Void_ClientReceivedDataArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664876);
		NativeMethodInfoPtr_add_OnServerReceivedData_Public_Virtual_add_Void_Action_1_ServerReceivedDataArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664877);
		NativeMethodInfoPtr_remove_OnServerReceivedData_Public_Virtual_rem_Void_Action_1_ServerReceivedDataArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664878);
		NativeMethodInfoPtr_HandleServerReceivedDataArgs_Public_Virtual_Void_ServerReceivedDataArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664879);
		NativeMethodInfoPtr_SendToServer_Public_Virtual_Void_Byte_ArraySegment_1_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664880);
		NativeMethodInfoPtr_SendToClient_Public_Virtual_Void_Byte_ArraySegment_1_Byte_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664881);
		NativeMethodInfoPtr_GetMaximumClients_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664882);
		NativeMethodInfoPtr_SetMaximumClients_Public_Virtual_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664883);
		NativeMethodInfoPtr_SetClientAddress_Public_Virtual_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664884);
		NativeMethodInfoPtr_SetServerBindAddress_Public_Virtual_Void_String_IPAddressType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664885);
		NativeMethodInfoPtr_SetPort_Public_Virtual_Void_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664886);
		NativeMethodInfoPtr_StartConnection_Public_Virtual_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664887);
		NativeMethodInfoPtr_StopConnection_Public_Virtual_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664888);
		NativeMethodInfoPtr_StopConnection_Public_Virtual_Boolean_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664889);
		NativeMethodInfoPtr_Shutdown_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664890);
		NativeMethodInfoPtr_StartServer_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664891);
		NativeMethodInfoPtr_StopServer_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664892);
		NativeMethodInfoPtr_StartClient_Private_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664893);
		NativeMethodInfoPtr_StopClient_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664894);
		NativeMethodInfoPtr_StopClient_Private_Boolean_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664895);
		NativeMethodInfoPtr_GetMTU_Public_Virtual_Int32_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664896);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr, 100664897);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78456, XrefRangeEnd = 78459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public new unsafe void Finalize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Finalize_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78459, XrefRangeEnd = 78539, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Initialize(NetworkManager networkManager, int transportIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)networkManager);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &transportIndex;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialize_Public_Virtual_Void_NetworkManager_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78539, XrefRangeEnd = 78545, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CreateChannelData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateChannelData_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78549, RefRangeEnd = 78550, XrefRangeStart = 78545, XrefRangeEnd = 78549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool InitializeRelayNetworkAccess()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeRelayNetworkAccess_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 78552, RefRangeEnd = 78554, XrefRangeStart = 78550, XrefRangeEnd = 78552, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsNetworkAccessAvailable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsNetworkAccessAvailable_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78554, XrefRangeEnd = 78567, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string GetConnectionAddress(int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionId);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetConnectionAddress_Public_Virtual_String_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78567, XrefRangeEnd = 78572, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void add_OnClientConnectionState(Il2CppSystem.Action<ClientConnectionStateArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_OnClientConnectionState_Public_Virtual_add_Void_Action_1_ClientConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78572, XrefRangeEnd = 78577, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void remove_OnClientConnectionState(Il2CppSystem.Action<ClientConnectionStateArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_OnClientConnectionState_Public_Virtual_rem_Void_Action_1_ClientConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78577, XrefRangeEnd = 78582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void add_OnServerConnectionState(Il2CppSystem.Action<ServerConnectionStateArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_OnServerConnectionState_Public_Virtual_add_Void_Action_1_ServerConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78582, XrefRangeEnd = 78587, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void remove_OnServerConnectionState(Il2CppSystem.Action<ServerConnectionStateArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_OnServerConnectionState_Public_Virtual_rem_Void_Action_1_ServerConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78587, XrefRangeEnd = 78592, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void add_OnRemoteConnectionState(Il2CppSystem.Action<RemoteConnectionStateArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_OnRemoteConnectionState_Public_Virtual_add_Void_Action_1_RemoteConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78592, XrefRangeEnd = 78597, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void remove_OnRemoteConnectionState(Il2CppSystem.Action<RemoteConnectionStateArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_OnRemoteConnectionState_Public_Virtual_rem_Void_Action_1_RemoteConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override LocalConnectionState GetConnectionState(bool server)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&server);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetConnectionState_Public_Virtual_LocalConnectionState_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(LocalConnectionState*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78597, XrefRangeEnd = 78601, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override RemoteConnectionState GetConnectionState(int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionId);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetConnectionState_Public_Virtual_RemoteConnectionState_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RemoteConnectionState*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe override void HandleClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionStateArgs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleClientConnectionState_Public_Virtual_Void_ClientConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void HandleServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionStateArgs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleServerConnectionState_Public_Virtual_Void_ServerConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionStateArgs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleRemoteConnectionState_Public_Virtual_Void_RemoteConnectionStateArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78601, XrefRangeEnd = 78619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void IterateIncoming(bool server)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&server);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_IterateIncoming_Public_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78619, XrefRangeEnd = 78620, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void IterateOutgoing(bool server)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&server);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_IterateOutgoing_Public_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78620, XrefRangeEnd = 78625, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void add_OnClientReceivedData(Il2CppSystem.Action<ClientReceivedDataArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_OnClientReceivedData_Public_Virtual_add_Void_Action_1_ClientReceivedDataArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78625, XrefRangeEnd = 78630, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void remove_OnClientReceivedData(Il2CppSystem.Action<ClientReceivedDataArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_OnClientReceivedData_Public_Virtual_rem_Void_Action_1_ClientReceivedDataArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)receivedDataArgs));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleClientReceivedDataArgs_Public_Virtual_Void_ClientReceivedDataArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78630, XrefRangeEnd = 78635, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void add_OnServerReceivedData(Il2CppSystem.Action<ServerReceivedDataArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_OnServerReceivedData_Public_Virtual_add_Void_Action_1_ServerReceivedDataArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78635, XrefRangeEnd = 78640, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void remove_OnServerReceivedData(Il2CppSystem.Action<ServerReceivedDataArgs> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_OnServerReceivedData_Public_Virtual_rem_Void_Action_1_ServerReceivedDataArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)receivedDataArgs));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_HandleServerReceivedDataArgs_Public_Virtual_Void_ServerReceivedDataArgs_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78640, XrefRangeEnd = 78668, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SendToServer(byte channelId, Il2CppSystem.ArraySegment<byte> segment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&channelId);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)segment));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SendToServer_Public_Virtual_Void_Byte_ArraySegment_1_Byte_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78668, XrefRangeEnd = 78669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SendToClient(byte channelId, Il2CppSystem.ArraySegment<byte> segment, int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&channelId);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)segment));
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &connectionId;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SendToClient_Public_Virtual_Void_Byte_ArraySegment_1_Byte_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override int GetMaximumClients()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetMaximumClients_Public_Virtual_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78669, XrefRangeEnd = 78673, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SetMaximumClients(int value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetMaximumClients_Public_Virtual_Void_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78673, XrefRangeEnd = 78674, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SetClientAddress(string address)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(address);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetClientAddress_Public_Virtual_Void_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SetServerBindAddress(string address, IPAddressType addressType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(address);
		*(IPAddressType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &addressType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetServerBindAddress_Public_Virtual_Void_String_IPAddressType_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe override void SetPort(ushort port)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&port);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetPort_Public_Virtual_Void_UInt16_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78674, XrefRangeEnd = 78676, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool StartConnection(bool server)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&server);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StartConnection_Public_Virtual_Boolean_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78676, XrefRangeEnd = 78678, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool StopConnection(bool server)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&server);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StopConnection_Public_Virtual_Boolean_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78678, XrefRangeEnd = 78689, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool StopConnection(int connectionId, bool immediately)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&connectionId);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &immediately;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StopConnection_Public_Virtual_Boolean_Int32_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe override void Shutdown()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Shutdown_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78702, RefRangeEnd = 78703, XrefRangeStart = 78689, XrefRangeEnd = 78702, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StartServer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartServer_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool StopServer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopServer_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78708, RefRangeEnd = 78709, XrefRangeStart = 78703, XrefRangeEnd = 78708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StartClient(string address)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(address);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartClient_Private_Boolean_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78709, XrefRangeEnd = 78711, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StopClient()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopClient_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78711, XrefRangeEnd = 78722, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StopClient(int connectionId, bool immediately)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&connectionId);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &immediately;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopClient_Private_Boolean_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78722, XrefRangeEnd = 78725, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetMTU(byte channel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&channel);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetMTU_Public_Virtual_Int32_Byte_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78725, XrefRangeEnd = 78731, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FishySteamworks()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FishySteamworks>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FishySteamworks(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
