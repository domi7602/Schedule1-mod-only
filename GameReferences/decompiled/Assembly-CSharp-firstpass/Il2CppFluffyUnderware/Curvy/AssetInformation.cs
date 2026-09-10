using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy;

public static class AssetInformation : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Version;

	private static readonly System.IntPtr NativeFieldInfoPtr_ApiVersion;

	private static readonly System.IntPtr NativeFieldInfoPtr_Website;

	private static readonly System.IntPtr NativeFieldInfoPtr_DocsRedirectionBaseUrl;

	public unsafe static string Version
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Version, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Version, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string ApiVersion
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ApiVersion, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ApiVersion, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string Website
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Website, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Website, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string DocsRedirectionBaseUrl
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DocsRedirectionBaseUrl, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DocsRedirectionBaseUrl, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static AssetInformation()
	{
		Il2CppClassPointerStore<AssetInformation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "AssetInformation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AssetInformation>.NativeClassPtr);
		NativeFieldInfoPtr_Version = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AssetInformation>.NativeClassPtr, "Version");
		NativeFieldInfoPtr_ApiVersion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AssetInformation>.NativeClassPtr, "ApiVersion");
		NativeFieldInfoPtr_Website = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AssetInformation>.NativeClassPtr, "Website");
		NativeFieldInfoPtr_DocsRedirectionBaseUrl = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AssetInformation>.NativeClassPtr, "DocsRedirectionBaseUrl");
	}

	public AssetInformation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
