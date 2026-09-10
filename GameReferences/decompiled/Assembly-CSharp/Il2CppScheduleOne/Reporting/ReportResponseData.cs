using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Reporting;

[System.Serializable]
public class ReportResponseData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_reportUid;

	private static readonly System.IntPtr NativeFieldInfoPtr_uploadLinks;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string reportUid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reportUid);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reportUid)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Dictionary<string, ReportUploadLink> uploadLinks
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uploadLinks);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, ReportUploadLink>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uploadLinks)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	static ReportResponseData()
	{
		Il2CppClassPointerStore<ReportResponseData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Reporting", "ReportResponseData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ReportResponseData>.NativeClassPtr);
		NativeFieldInfoPtr_reportUid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ReportResponseData>.NativeClassPtr, "reportUid");
		NativeFieldInfoPtr_uploadLinks = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ReportResponseData>.NativeClassPtr, "uploadLinks");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ReportResponseData>.NativeClassPtr, 100667356);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 106797, XrefRangeEnd = 106805, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ReportResponseData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ReportResponseData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ReportResponseData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
