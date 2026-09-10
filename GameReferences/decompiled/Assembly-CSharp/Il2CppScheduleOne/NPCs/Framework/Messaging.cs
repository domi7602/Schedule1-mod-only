using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Messaging;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Messaging : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_IsKnownByDefault;

	private static readonly System.IntPtr NativeFieldInfoPtr_ConversationCanBeHidden;

	private static readonly System.IntPtr NativeFieldInfoPtr_ConversationCategories;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Messaging_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool IsKnownByDefault
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsKnownByDefault);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsKnownByDefault)) = flag;
		}
	}

	public unsafe bool ConversationCanBeHidden
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConversationCanBeHidden);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConversationCanBeHidden)) = flag;
		}
	}

	public unsafe Il2CppStructArray<EConversationCategory> ConversationCategories
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConversationCategories);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<EConversationCategory>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConversationCategories)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static Messaging()
	{
		Il2CppClassPointerStore<Messaging>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Messaging");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Messaging>.NativeClassPtr);
		NativeFieldInfoPtr_IsKnownByDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Messaging>.NativeClassPtr, "IsKnownByDefault");
		NativeFieldInfoPtr_ConversationCanBeHidden = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Messaging>.NativeClassPtr, "ConversationCanBeHidden");
		NativeFieldInfoPtr_ConversationCategories = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Messaging>.NativeClassPtr, "ConversationCategories");
		NativeMethodInfoPtr_GetCopy_Public_Messaging_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Messaging>.NativeClassPtr, 100682822);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Messaging>.NativeClassPtr, 100682823);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272281, XrefRangeEnd = 272289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Messaging GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Messaging_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Messaging>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272289, XrefRangeEnd = 272290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Messaging()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Messaging>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Messaging(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
