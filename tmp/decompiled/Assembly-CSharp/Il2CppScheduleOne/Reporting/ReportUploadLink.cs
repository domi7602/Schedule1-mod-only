using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Reporting;

[System.Serializable]
public class ReportUploadLink : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_signedUrl;

	private static readonly System.IntPtr NativeFieldInfoPtr_token;

	private static readonly System.IntPtr NativeFieldInfoPtr_path;

	private static readonly System.IntPtr NativeFieldInfoPtr_contentType;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string signedUrl
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_signedUrl);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_signedUrl)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string token
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_token);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_token)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string path
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_path);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_path)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string contentType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_contentType);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_contentType)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ReportUploadLink()
	{
		Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Reporting", "ReportUploadLink");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr);
		NativeFieldInfoPtr_signedUrl = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr, "signedUrl");
		NativeFieldInfoPtr_token = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr, "token");
		NativeFieldInfoPtr_path = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr, "path");
		NativeFieldInfoPtr_contentType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr, "contentType");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr, 100667360);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 106810, XrefRangeEnd = 106820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ReportUploadLink()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ReportUploadLink>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ReportUploadLink(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
