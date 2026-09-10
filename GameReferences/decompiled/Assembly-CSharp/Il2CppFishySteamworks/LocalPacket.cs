using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFishySteamworks;

public sealed class LocalPacket : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Data;

	private static readonly System.IntPtr NativeFieldInfoPtr_Length;

	private static readonly System.IntPtr NativeFieldInfoPtr_Channel;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_ArraySegment_1_Byte_Byte_0;

	public unsafe Il2CppStructArray<byte> Data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int Length
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Length);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Length)) = num;
		}
	}

	public unsafe byte Channel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Channel);
			return *(byte*)num;
		}
		set
		{
			*(byte*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Channel)) = b;
		}
	}

	static LocalPacket()
	{
		Il2CppClassPointerStore<LocalPacket>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "FishySteamworks", "LocalPacket");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr);
		NativeFieldInfoPtr_Data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr, "Data");
		NativeFieldInfoPtr_Length = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr, "Length");
		NativeFieldInfoPtr_Channel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr, "Channel");
		NativeMethodInfoPtr__ctor_Public_Void_ArraySegment_1_Byte_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr, 100664852);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 78444, XrefRangeEnd = 78456, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LocalPacket(Il2CppSystem.ArraySegment<byte> data, byte channel)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)data));
		*(byte**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &channel;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ArraySegment_1_Byte_Byte_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LocalPacket(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public LocalPacket()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LocalPacket>.NativeClassPtr))
	{
	}
}
