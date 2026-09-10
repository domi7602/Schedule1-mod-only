using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using Il2CppSystem.Text;

namespace Il2CppFluffyUnderware.DevTools;

public class DTObjectDump : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_INDENTSPACES;

	private static readonly System.IntPtr NativeFieldInfoPtr_mIndent;

	private static readonly System.IntPtr NativeFieldInfoPtr_mSB;

	private static readonly System.IntPtr NativeFieldInfoPtr_mObject;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AppendHeader_Private_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AppendMember_Private_Void_MemberInfo_0;

	public unsafe static int INDENTSPACES
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_INDENTSPACES, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_INDENTSPACES, (void*)(&num));
		}
	}

	public unsafe string mIndent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mIndent);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mIndent)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe StringBuilder mSB
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSB);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StringBuilder>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSB)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stringBuilder));
		}
	}

	public unsafe Il2CppSystem.Object mObject
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mObject);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mObject)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	static DTObjectDump()
	{
		Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTObjectDump");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr);
		NativeFieldInfoPtr_INDENTSPACES = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, "INDENTSPACES");
		NativeFieldInfoPtr_mIndent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, "mIndent");
		NativeFieldInfoPtr_mSB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, "mSB");
		NativeFieldInfoPtr_mObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, "mObject");
		NativeMethodInfoPtr__ctor_Public_Void_Object_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, 100663969);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, 100663970);
		NativeMethodInfoPtr_AppendHeader_Private_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, 100663971);
		NativeMethodInfoPtr_AppendMember_Private_Void_MemberInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr, 100663972);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 31035, RefRangeEnd = 31036, XrefRangeStart = 30998, XrefRangeEnd = 31035, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DTObjectDump(Il2CppSystem.Object o, int indent = 0)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DTObjectDump>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &indent;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31036, XrefRangeEnd = 31037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 31044, RefRangeEnd = 31045, XrefRangeStart = 31037, XrefRangeEnd = 31044, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AppendHeader(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AppendHeader_Private_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 31076, RefRangeEnd = 31078, XrefRangeStart = 31045, XrefRangeEnd = 31076, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AppendMember(MemberInfo info)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)info);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AppendMember_Private_Void_MemberInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DTObjectDump(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
