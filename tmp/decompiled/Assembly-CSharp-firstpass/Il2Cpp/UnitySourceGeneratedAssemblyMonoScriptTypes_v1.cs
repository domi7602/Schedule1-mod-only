using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2Cpp;

public class UnitySourceGeneratedAssemblyMonoScriptTypes_v1 : Il2CppSystem.Object
{
	public sealed class MonoScriptData : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_FilePathsData;

		private static readonly System.IntPtr NativeFieldInfoPtr_TypesData;

		private static readonly System.IntPtr NativeFieldInfoPtr_TotalTypes;

		private static readonly System.IntPtr NativeFieldInfoPtr_TotalFiles;

		private static readonly System.IntPtr NativeFieldInfoPtr_IsEditorOnly;

		public unsafe Il2CppStructArray<byte> FilePathsData
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FilePathsData);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FilePathsData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppStructArray<byte> TypesData
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TypesData);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TypesData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe int TotalTypes
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TotalTypes);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TotalTypes)) = num;
			}
		}

		public unsafe int TotalFiles
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TotalFiles);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TotalFiles)) = num;
			}
		}

		public unsafe bool IsEditorOnly
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsEditorOnly);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsEditorOnly)) = flag;
			}
		}

		static MonoScriptData()
		{
			Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<UnitySourceGeneratedAssemblyMonoScriptTypes_v1>.NativeClassPtr, "MonoScriptData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr);
			NativeFieldInfoPtr_FilePathsData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr, "FilePathsData");
			NativeFieldInfoPtr_TypesData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr, "TypesData");
			NativeFieldInfoPtr_TotalTypes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr, "TotalTypes");
			NativeFieldInfoPtr_TotalFiles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr, "TotalFiles");
			NativeFieldInfoPtr_IsEditorOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr, "IsEditorOnly");
		}

		public MonoScriptData(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public MonoScriptData()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MonoScriptData>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_Get_Private_Static_MonoScriptData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static UnitySourceGeneratedAssemblyMonoScriptTypes_v1()
	{
		Il2CppClassPointerStore<UnitySourceGeneratedAssemblyMonoScriptTypes_v1>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "", "UnitySourceGeneratedAssemblyMonoScriptTypes_v1");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UnitySourceGeneratedAssemblyMonoScriptTypes_v1>.NativeClassPtr);
		NativeMethodInfoPtr_Get_Private_Static_MonoScriptData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnitySourceGeneratedAssemblyMonoScriptTypes_v1>.NativeClassPtr, 100663327);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnitySourceGeneratedAssemblyMonoScriptTypes_v1>.NativeClassPtr, 100663328);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19169, XrefRangeEnd = 19182, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MonoScriptData Get()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Get_Private_Static_MonoScriptData_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new MonoScriptData(pointer);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UnitySourceGeneratedAssemblyMonoScriptTypes_v1()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UnitySourceGeneratedAssemblyMonoScriptTypes_v1>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UnitySourceGeneratedAssemblyMonoScriptTypes_v1(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
