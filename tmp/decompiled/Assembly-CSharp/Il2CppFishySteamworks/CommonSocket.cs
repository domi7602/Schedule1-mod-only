using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Transporting;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSteamworks;
using Il2CppSystem;
using Il2CppSystem.Collections.Concurrent;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFishySteamworks;

public class CommonSocket : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__connectionState;

	private static readonly System.IntPtr NativeFieldInfoPtr_PeerToPeer;

	private static readonly System.IntPtr NativeFieldInfoPtr_Transport;

	private static readonly System.IntPtr NativeFieldInfoPtr_MessagePointers;

	private static readonly System.IntPtr NativeFieldInfoPtr_InboundBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_MESSAGES;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalConnectionState_Internal_LocalConnectionState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLocalConnectionState_Protected_Virtual_New_Void_LocalConnectionState_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Internal_Virtual_New_Void_Transport_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetIPBytes_Protected_Il2CppStructArray_1_Byte_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Send_Protected_EResult_HSteamNetConnection_ArraySegment_1_Byte_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearQueue_Internal_Void_ConcurrentQueue_1_LocalPacket_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearQueue_Internal_Void_Queue_1_LocalPacket_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMessage_Protected_Void_IntPtr_Il2CppStructArray_1_Byte_byref_ArraySegment_1_Byte_byref_Byte_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe LocalConnectionState _connectionState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__connectionState);
			return *(LocalConnectionState*)num;
		}
		set
		{
			*(LocalConnectionState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__connectionState)) = localConnectionState;
		}
	}

	public unsafe bool PeerToPeer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PeerToPeer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PeerToPeer)) = flag;
		}
	}

	public unsafe Transport Transport
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Transport);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transport>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Transport)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transport));
		}
	}

	public unsafe Il2CppStructArray<System.IntPtr> MessagePointers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MessagePointers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<System.IntPtr>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MessagePointers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<byte> InboundBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InboundBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InboundBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe static int MAX_MESSAGES
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_MESSAGES, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_MESSAGES, (void*)(&num));
		}
	}

	static CommonSocket()
	{
		Il2CppClassPointerStore<CommonSocket>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishySteamworks", "CommonSocket");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr);
		NativeFieldInfoPtr__connectionState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, "_connectionState");
		NativeFieldInfoPtr_PeerToPeer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, "PeerToPeer");
		NativeFieldInfoPtr_Transport = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, "Transport");
		NativeFieldInfoPtr_MessagePointers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, "MessagePointers");
		NativeFieldInfoPtr_InboundBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, "InboundBuffer");
		NativeFieldInfoPtr_MAX_MESSAGES = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, "MAX_MESSAGES");
		NativeMethodInfoPtr_GetLocalConnectionState_Internal_LocalConnectionState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664843);
		NativeMethodInfoPtr_SetLocalConnectionState_Protected_Virtual_New_Void_LocalConnectionState_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664844);
		NativeMethodInfoPtr_Initialize_Internal_Virtual_New_Void_Transport_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664845);
		NativeMethodInfoPtr_GetIPBytes_Protected_Il2CppStructArray_1_Byte_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664846);
		NativeMethodInfoPtr_Send_Protected_EResult_HSteamNetConnection_ArraySegment_1_Byte_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664847);
		NativeMethodInfoPtr_ClearQueue_Internal_Void_ConcurrentQueue_1_LocalPacket_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664848);
		NativeMethodInfoPtr_ClearQueue_Internal_Void_Queue_1_LocalPacket_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664849);
		NativeMethodInfoPtr_GetMessage_Protected_Void_IntPtr_Il2CppStructArray_1_Byte_byref_ArraySegment_1_Byte_byref_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664850);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr, 100664851);
	}

	[CallerCount(0)]
	public unsafe LocalConnectionState GetLocalConnectionState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalConnectionState_Internal_LocalConnectionState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(LocalConnectionState*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 78351, RefRangeEnd = 78354, XrefRangeStart = 78350, XrefRangeEnd = 78351, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&connectionState);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &server;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetLocalConnectionState_Protected_Virtual_New_Void_LocalConnectionState_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78354, XrefRangeEnd = 78364, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Initialize(Transport t)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialize_Internal_Virtual_New_Void_Transport_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 78372, RefRangeEnd = 78374, XrefRangeStart = 78364, XrefRangeEnd = 78372, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<byte> GetIPBytes(string address)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(address);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIPBytes_Protected_Il2CppStructArray_1_Byte_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 78406, RefRangeEnd = 78409, XrefRangeStart = 78374, XrefRangeEnd = 78406, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EResult Send(HSteamNetConnection steamConnection, Il2CppSystem.ArraySegment<byte> segment, byte channelId)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&steamConnection);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)segment));
		*(byte**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &channelId;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Send_Protected_EResult_HSteamNetConnection_ArraySegment_1_Byte_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EResult*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78409, XrefRangeEnd = 78417, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearQueue(ConcurrentQueue<LocalPacket> queue)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)queue);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearQueue_Internal_Void_ConcurrentQueue_1_LocalPacket_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78417, XrefRangeEnd = 78426, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearQueue(Queue<LocalPacket> queue)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)queue);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearQueue_Internal_Void_Queue_1_LocalPacket_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 78438, RefRangeEnd = 78439, XrefRangeStart = 78426, XrefRangeEnd = 78438, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetMessage(System.IntPtr ptr, Il2CppStructArray<byte> buffer, out Il2CppSystem.ArraySegment<byte> segment, out byte channel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr2 = stackalloc System.IntPtr[4];
		*ptr2 = (nint)(&ptr);
		*(System.IntPtr*)((byte*)ptr2 + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)buffer);
		byte* num = (byte*)ptr2 + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		*(void**)((byte*)ptr2 + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref channel);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMessage_Protected_Void_IntPtr_Il2CppStructArray_1_Byte_byref_ArraySegment_1_Byte_byref_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr2, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		segment = ((num3 == 0) ? null : new Il2CppSystem.ArraySegment<byte>(num3));
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78439, XrefRangeEnd = 78444, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CommonSocket()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CommonSocket>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CommonSocket(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
