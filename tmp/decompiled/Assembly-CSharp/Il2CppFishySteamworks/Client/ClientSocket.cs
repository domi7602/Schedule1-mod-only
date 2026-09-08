using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSteamworks;
using Il2CppSystem;
using Il2CppSystem.Threading;

namespace Il2CppFishySteamworks.Client;

public class ClientSocket : CommonSocket
{
	private static readonly System.IntPtr NativeFieldInfoPtr__onLocalConnectionStateCallback;

	private static readonly System.IntPtr NativeFieldInfoPtr__hostSteamID;

	private static readonly System.IntPtr NativeFieldInfoPtr__socket;

	private static readonly System.IntPtr NativeFieldInfoPtr__timeoutThread;

	private static readonly System.IntPtr NativeFieldInfoPtr__connectTimeout;

	private static readonly System.IntPtr NativeFieldInfoPtr_CONNECT_TIMEOUT_DURATION;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckTimeout_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartConnection_Internal_Boolean_String_UInt16_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnLocalConnectionState_Private_Void_SteamNetConnectionStatusChangedCallback_t_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopConnection_Internal_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IterateIncoming_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SendToServer_Internal_Void_Byte_ArraySegment_1_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IterateOutgoing_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Callback<SteamNetConnectionStatusChangedCallback_t> _onLocalConnectionStateCallback
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__onLocalConnectionStateCallback);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Callback<SteamNetConnectionStatusChangedCallback_t>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__onLocalConnectionStateCallback)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback));
		}
	}

	public unsafe CSteamID _hostSteamID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hostSteamID);
			return *(CSteamID*)num;
		}
		set
		{
			*(CSteamID*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hostSteamID)) = cSteamID;
		}
	}

	public unsafe HSteamNetConnection _socket
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__socket);
			return *(HSteamNetConnection*)num;
		}
		set
		{
			*(HSteamNetConnection*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__socket)) = hSteamNetConnection;
		}
	}

	public unsafe Thread _timeoutThread
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeoutThread);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Thread>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeoutThread)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)thread));
		}
	}

	public unsafe float _connectTimeout
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__connectTimeout);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__connectTimeout)) = num;
		}
	}

	public unsafe static float CONNECT_TIMEOUT_DURATION
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CONNECT_TIMEOUT_DURATION, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CONNECT_TIMEOUT_DURATION, (void*)(&num));
		}
	}

	static ClientSocket()
	{
		Il2CppClassPointerStore<ClientSocket>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishySteamworks.Client", "ClientSocket");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr);
		NativeFieldInfoPtr__onLocalConnectionStateCallback = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, "_onLocalConnectionStateCallback");
		NativeFieldInfoPtr__hostSteamID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, "_hostSteamID");
		NativeFieldInfoPtr__socket = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, "_socket");
		NativeFieldInfoPtr__timeoutThread = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, "_timeoutThread");
		NativeFieldInfoPtr__connectTimeout = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, "_connectTimeout");
		NativeFieldInfoPtr_CONNECT_TIMEOUT_DURATION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, "CONNECT_TIMEOUT_DURATION");
		NativeMethodInfoPtr_CheckTimeout_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664929);
		NativeMethodInfoPtr_StartConnection_Internal_Boolean_String_UInt16_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664930);
		NativeMethodInfoPtr_OnLocalConnectionState_Private_Void_SteamNetConnectionStatusChangedCallback_t_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664931);
		NativeMethodInfoPtr_StopConnection_Internal_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664932);
		NativeMethodInfoPtr_IterateIncoming_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664933);
		NativeMethodInfoPtr_SendToServer_Internal_Void_Byte_ArraySegment_1_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664934);
		NativeMethodInfoPtr_IterateOutgoing_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664935);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr, 100664936);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79137, XrefRangeEnd = 79168, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckTimeout()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckTimeout_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79168, XrefRangeEnd = 79205, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StartConnection(string address, ushort port, bool peerToPeer)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(address);
		*(ushort**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &port;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &peerToPeer;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartConnection_Internal_Boolean_String_UInt16_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79205, XrefRangeEnd = 79216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnLocalConnectionState(SteamNetConnectionStatusChangedCallback_t args)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)args));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnLocalConnectionState_Private_Void_SteamNetConnectionStatusChangedCallback_t_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 79235, RefRangeEnd = 79239, XrefRangeStart = 79216, XrefRangeEnd = 79235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool StopConnection()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopConnection_Internal_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79253, RefRangeEnd = 79254, XrefRangeStart = 79239, XrefRangeEnd = 79253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void IterateIncoming()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IterateIncoming_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79254, XrefRangeEnd = 79263, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SendToServer(byte channelId, Il2CppSystem.ArraySegment<byte> segment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&channelId);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)segment));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SendToServer_Internal_Void_Byte_ArraySegment_1_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79263, XrefRangeEnd = 79264, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void IterateOutgoing()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IterateOutgoing_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79264, XrefRangeEnd = 79273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ClientSocket()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ClientSocket>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ClientSocket(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
