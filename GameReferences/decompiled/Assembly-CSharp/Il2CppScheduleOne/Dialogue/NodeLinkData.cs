using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Dialogue;

[System.Serializable]
public class NodeLinkData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_BaseDialogueOrBranchNodeGuid;

	private static readonly System.IntPtr NativeFieldInfoPtr_BaseChoiceOrOptionGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_TargetNodeGuid;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string BaseDialogueOrBranchNodeGuid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseDialogueOrBranchNodeGuid);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseDialogueOrBranchNodeGuid)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string BaseChoiceOrOptionGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseChoiceOrOptionGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseChoiceOrOptionGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string TargetNodeGuid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetNodeGuid);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetNodeGuid)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static NodeLinkData()
	{
		Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Dialogue", "NodeLinkData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr);
		NativeFieldInfoPtr_BaseDialogueOrBranchNodeGuid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr, "BaseDialogueOrBranchNodeGuid");
		NativeFieldInfoPtr_BaseChoiceOrOptionGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr, "BaseChoiceOrOptionGUID");
		NativeFieldInfoPtr_TargetNodeGuid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr, "TargetNodeGuid");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr, 100674877);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NodeLinkData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NodeLinkData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NodeLinkData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
