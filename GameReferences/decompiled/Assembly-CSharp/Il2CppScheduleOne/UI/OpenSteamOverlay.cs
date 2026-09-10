using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.UI;

public class OpenSteamOverlay : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EType")]
	public enum EType
	{
		Store,
		CustomLink
	}

	private static readonly IntPtr NativeFieldInfoPtr_APP_ID;

	private static readonly IntPtr NativeFieldInfoPtr_Type;

	private static readonly IntPtr NativeFieldInfoPtr_CustomLink;

	private static readonly IntPtr NativeMethodInfoPtr_OpenOverlay_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static uint APP_ID
	{
		get
		{
			Unsafe.SkipInit(out uint result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_APP_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_APP_ID, (void*)(&num));
		}
	}

	public unsafe EType Type
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Type);
			return *(EType*)num;
		}
		set
		{
			*(EType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Type)) = eType;
		}
	}

	public unsafe string CustomLink
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomLink);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomLink)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static OpenSteamOverlay()
	{
		Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "OpenSteamOverlay");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr);
		NativeFieldInfoPtr_APP_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr, "APP_ID");
		NativeFieldInfoPtr_Type = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr, "Type");
		NativeFieldInfoPtr_CustomLink = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr, "CustomLink");
		NativeMethodInfoPtr_OpenOverlay_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr, 100686284);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr, 100686285);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 298066, XrefRangeEnd = 298069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OpenOverlay()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenOverlay_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OpenSteamOverlay()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OpenSteamOverlay>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public OpenSteamOverlay(IntPtr pointer)
		: base(pointer)
	{
	}
}
