using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppFishNet.Transporting;
using Il2CppFishySteamworks.Client;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSteamworks;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFishySteamworks.Server;

public class ServerSocket : CommonSocket
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ConnectionChange
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ConnectionId;

		private static readonly System.IntPtr NativeFieldInfoPtr_SteamConnection;

		private static readonly System.IntPtr NativeFieldInfoPtr_SteamId;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsConnect_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_HSteamNetConnection_CSteamID_0;

		[FieldOffset(0)]
		public int ConnectionId;

		[FieldOffset(4)]
		public HSteamNetConnection SteamConnection;

		[FieldOffset(8)]
		public CSteamID SteamId;

		public unsafe bool IsConnect
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78731, XrefRangeEnd = 78735, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsConnect_Public_get_Boolean_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static ConnectionChange()
		{
			Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "ConnectionChange");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr);
			NativeFieldInfoPtr_ConnectionId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, "ConnectionId");
			NativeFieldInfoPtr_SteamConnection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, "SteamConnection");
			NativeFieldInfoPtr_SteamId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, "SteamId");
			NativeMethodInfoPtr_get_IsConnect_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, 100664918);
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, 100664919);
			NativeMethodInfoPtr__ctor_Public_Void_Int32_HSteamNetConnection_CSteamID_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, 100664920);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78735, XrefRangeEnd = 78739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ConnectionChange(int id)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&id);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe ConnectionChange(int id, HSteamNetConnection steamConnection, CSteamID steamId)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&id);
			*(HSteamNetConnection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &steamConnection;
			*(CSteamID**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &steamId;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_HSteamNetConnection_CSteamID_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ConnectionChange>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__steamConnections;

	private static readonly System.IntPtr NativeFieldInfoPtr__steamIds;

	private static readonly System.IntPtr NativeFieldInfoPtr__maximumClients;

	private static readonly System.IntPtr NativeFieldInfoPtr__nextConnectionId;

	private static readonly System.IntPtr NativeFieldInfoPtr__socket;

	private static readonly System.IntPtr NativeFieldInfoPtr__clientHostIncoming;

	private static readonly System.IntPtr NativeFieldInfoPtr__clientHostStarted;

	private static readonly System.IntPtr NativeFieldInfoPtr__onRemoteConnectionStateCallback;

	private static readonly System.IntPtr NativeFieldInfoPtr__cachedConnectionIds;

	private static readonly System.IntPtr NativeFieldInfoPtr__clientHost;

	private static readonly System.IntPtr NativeFieldInfoPtr__iteratingConnections;

	private static readonly System.IntPtr NativeFieldInfoPtr__pendingConnectionChanges;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConnectionState_Internal_RemoteConnectionState_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetInvalidSocket_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartConnection_Internal_Boolean_String_UInt16_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopConnection_Internal_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopConnection_Internal_Boolean_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopConnection_Private_Boolean_Int32_HSteamNetConnection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnRemoteConnectionState_Private_Void_SteamNetConnectionStatusChangedCallback_t_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddConnection_Private_Void_Int32_HSteamNetConnection_CSteamID_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveConnection_Private_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IterateOutgoing_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IterateIncoming_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ProcessPendingConnectionChanges_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SendToClient_Internal_Void_Byte_ArraySegment_1_Byte_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConnectionAddress_Internal_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMaximumClients_Internal_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMaximumClients_Internal_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetClientHostSocket_Internal_Void_ClientHostSocket_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnClientHostState_Internal_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReceivedFromClientHost_Internal_Void_LocalPacket_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe BidirectionalDictionary<HSteamNetConnection, int> _steamConnections
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__steamConnections);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BidirectionalDictionary<HSteamNetConnection, int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__steamConnections)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bidirectionalDictionary));
		}
	}

	public unsafe BidirectionalDictionary<CSteamID, int> _steamIds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__steamIds);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BidirectionalDictionary<CSteamID, int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__steamIds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bidirectionalDictionary));
		}
	}

	public unsafe int _maximumClients
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maximumClients);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maximumClients)) = num;
		}
	}

	public unsafe int _nextConnectionId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__nextConnectionId);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__nextConnectionId)) = num;
		}
	}

	public unsafe HSteamListenSocket _socket
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__socket);
			return *(HSteamListenSocket*)num;
		}
		set
		{
			*(HSteamListenSocket*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__socket)) = hSteamListenSocket;
		}
	}

	public unsafe Queue<LocalPacket> _clientHostIncoming
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientHostIncoming);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Queue<LocalPacket>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientHostIncoming)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)queue));
		}
	}

	public unsafe bool _clientHostStarted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientHostStarted);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__clientHostStarted)) = flag;
		}
	}

	public unsafe Callback<SteamNetConnectionStatusChangedCallback_t> _onRemoteConnectionStateCallback
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__onRemoteConnectionStateCallback);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Callback<SteamNetConnectionStatusChangedCallback_t>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__onRemoteConnectionStateCallback)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback));
		}
	}

	public unsafe Queue<int> _cachedConnectionIds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cachedConnectionIds);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Queue<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cachedConnectionIds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)queue));
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

	public unsafe bool _iteratingConnections
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__iteratingConnections);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__iteratingConnections)) = flag;
		}
	}

	public unsafe List<ConnectionChange> _pendingConnectionChanges
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pendingConnectionChanges);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ConnectionChange>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pendingConnectionChanges)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static ServerSocket()
	{
		Il2CppClassPointerStore<ServerSocket>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishySteamworks.Server", "ServerSocket");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr);
		NativeFieldInfoPtr__steamConnections = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_steamConnections");
		NativeFieldInfoPtr__steamIds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_steamIds");
		NativeFieldInfoPtr__maximumClients = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_maximumClients");
		NativeFieldInfoPtr__nextConnectionId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_nextConnectionId");
		NativeFieldInfoPtr__socket = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_socket");
		NativeFieldInfoPtr__clientHostIncoming = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_clientHostIncoming");
		NativeFieldInfoPtr__clientHostStarted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_clientHostStarted");
		NativeFieldInfoPtr__onRemoteConnectionStateCallback = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_onRemoteConnectionStateCallback");
		NativeFieldInfoPtr__cachedConnectionIds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_cachedConnectionIds");
		NativeFieldInfoPtr__clientHost = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_clientHost");
		NativeFieldInfoPtr__iteratingConnections = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_iteratingConnections");
		NativeFieldInfoPtr__pendingConnectionChanges = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, "_pendingConnectionChanges");
		NativeMethodInfoPtr_GetConnectionState_Internal_RemoteConnectionState_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664898);
		NativeMethodInfoPtr_ResetInvalidSocket_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664899);
		NativeMethodInfoPtr_StartConnection_Internal_Boolean_String_UInt16_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664900);
		NativeMethodInfoPtr_StopConnection_Internal_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664901);
		NativeMethodInfoPtr_StopConnection_Internal_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664902);
		NativeMethodInfoPtr_StopConnection_Private_Boolean_Int32_HSteamNetConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664903);
		NativeMethodInfoPtr_OnRemoteConnectionState_Private_Void_SteamNetConnectionStatusChangedCallback_t_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664904);
		NativeMethodInfoPtr_AddConnection_Private_Void_Int32_HSteamNetConnection_CSteamID_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664905);
		NativeMethodInfoPtr_RemoveConnection_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664906);
		NativeMethodInfoPtr_IterateOutgoing_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664907);
		NativeMethodInfoPtr_IterateIncoming_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664908);
		NativeMethodInfoPtr_ProcessPendingConnectionChanges_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664909);
		NativeMethodInfoPtr_SendToClient_Internal_Void_Byte_ArraySegment_1_Byte_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664910);
		NativeMethodInfoPtr_GetConnectionAddress_Internal_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664911);
		NativeMethodInfoPtr_SetMaximumClients_Internal_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664912);
		NativeMethodInfoPtr_GetMaximumClients_Internal_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664913);
		NativeMethodInfoPtr_SetClientHostSocket_Internal_Void_ClientHostSocket_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664914);
		NativeMethodInfoPtr_OnClientHostState_Internal_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664915);
		NativeMethodInfoPtr_ReceivedFromClientHost_Internal_Void_LocalPacket_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664916);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr, 100664917);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78739, XrefRangeEnd = 78743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RemoteConnectionState GetConnectionState(int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionId);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConnectionState_Internal_RemoteConnectionState_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(RemoteConnectionState*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78743, XrefRangeEnd = 78749, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetInvalidSocket()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetInvalidSocket_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78783, RefRangeEnd = 78784, XrefRangeStart = 78749, XrefRangeEnd = 78783, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StartConnection(string address, ushort port, int maximumClients, bool peerToPeer)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(address);
		*(ushort**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &port;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maximumClients;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &peerToPeer;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartConnection_Internal_Boolean_String_UInt16_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78784, XrefRangeEnd = 78800, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StopConnection()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopConnection_Internal_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78800, XrefRangeEnd = 78811, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StopConnection(int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionId);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopConnection_Internal_Boolean_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78811, XrefRangeEnd = 78821, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StopConnection(int connectionId, HSteamNetConnection socket)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&connectionId);
		*(HSteamNetConnection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &socket;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopConnection_Private_Boolean_Int32_HSteamNetConnection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78821, XrefRangeEnd = 78842, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnRemoteConnectionState(SteamNetConnectionStatusChangedCallback_t args)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)args));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnRemoteConnectionState_Private_Void_SteamNetConnectionStatusChangedCallback_t_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78859, RefRangeEnd = 78860, XrefRangeStart = 78842, XrefRangeEnd = 78859, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddConnection(int connectionId, HSteamNetConnection steamConnection, CSteamID steamId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&connectionId);
		*(HSteamNetConnection**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &steamConnection;
		*(CSteamID**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &steamId;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddConnection_Private_Void_Int32_HSteamNetConnection_CSteamID_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78877, RefRangeEnd = 78878, XrefRangeStart = 78860, XrefRangeEnd = 78877, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveConnection(int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionId);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveConnection_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78878, XrefRangeEnd = 78900, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void IterateOutgoing()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IterateOutgoing_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78933, RefRangeEnd = 78934, XrefRangeStart = 78900, XrefRangeEnd = 78933, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void IterateIncoming()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IterateIncoming_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 78956, RefRangeEnd = 78958, XrefRangeStart = 78934, XrefRangeEnd = 78956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ProcessPendingConnectionChanges()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessPendingConnectionChanges_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78977, RefRangeEnd = 78978, XrefRangeStart = 78958, XrefRangeEnd = 78977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SendToClient(byte channelId, Il2CppSystem.ArraySegment<byte> segment, int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&channelId);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)segment));
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &connectionId;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SendToClient_Internal_Void_Byte_ArraySegment_1_Byte_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78978, XrefRangeEnd = 78991, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetConnectionAddress(int connectionId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&connectionId);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConnectionAddress_Internal_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78991, XrefRangeEnd = 78995, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMaximumClients(int value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMaximumClients_Internal_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe int GetMaximumClients()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMaximumClients_Internal_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78995, XrefRangeEnd = 78996, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetClientHostSocket(ClientHostSocket socket)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)socket);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetClientHostSocket_Internal_Void_ClientHostSocket_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 79019, RefRangeEnd = 79021, XrefRangeStart = 78996, XrefRangeEnd = 79019, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnClientHostState(bool started)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&started);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnClientHostState_Internal_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79021, XrefRangeEnd = 79024, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReceivedFromClientHost(LocalPacket packet)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)packet));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReceivedFromClientHost_Internal_Void_LocalPacket_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79024, XrefRangeEnd = 79073, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ServerSocket()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ServerSocket>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ServerSocket(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
