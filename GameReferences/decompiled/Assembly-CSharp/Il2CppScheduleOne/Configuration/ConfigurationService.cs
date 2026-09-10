using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.Configuration;

public class ConfigurationService : PersistentSingleton<ConfigurationService>
{
	[ObfuscatedName("ScheduleOne.Configuration.ConfigurationService+<>c__DisplayClass6_0")]
	public sealed class __c__DisplayClass6_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_configurationName;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__TryGetConfiguration_b__0_Internal_Boolean_BaseConfiguration_0;

		public unsafe string configurationName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_configurationName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_configurationName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static __c__DisplayClass6_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass6_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, "<>c__DisplayClass6_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass6_0>.NativeClassPtr);
			NativeFieldInfoPtr_configurationName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass6_0>.NativeClassPtr, "configurationName");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass6_0>.NativeClassPtr, 100675541);
			NativeMethodInfoPtr__TryGetConfiguration_b__0_Internal_Boolean_BaseConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass6_0>.NativeClassPtr, 100675542);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass6_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass6_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199979, XrefRangeEnd = 199982, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _TryGetConfiguration_b__0(BaseConfiguration config)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)config);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__TryGetConfiguration_b__0_Internal_Boolean_BaseConfiguration_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass6_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_TryGetConfiguration_Public_Boolean_byref_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_TryGetConfiguration_Public_Boolean_byref_T_0, Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetConfigurationAndListenForChanges_Public_Void_Action_1_BaseConfiguration_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetConfigurationAndListenForChanges_Public_Void_Action_1_BaseConfiguration_0, Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_UnsubscribeFromConfigurationChanges_Public_Void_Action_1_BaseConfiguration_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_UnsubscribeFromConfigurationChanges_Public_Void_Action_1_BaseConfiguration_0, Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__configurations;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Configurations_Public_get_Il2CppReferenceArray_1_BaseConfiguration_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetConfigurations_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetConfiguration_Public_Boolean_byref_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetConfiguration_Public_Boolean_String_byref_BaseConfiguration_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConfigurationAndListenForChanges_Public_Void_Action_1_BaseConfiguration_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnsubscribeFromConfigurationChanges_Public_Void_Action_1_BaseConfiguration_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<BaseConfiguration> _configurations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__configurations);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<BaseConfiguration>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__configurations)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<BaseConfiguration> Configurations
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Configurations_Public_get_Il2CppReferenceArray_1_BaseConfiguration_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<BaseConfiguration>>(intPtr) : null;
		}
	}

	static ConfigurationService()
	{
		Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Configuration", "ConfigurationService");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr);
		NativeFieldInfoPtr__configurations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, "_configurations");
		NativeMethodInfoPtr_get_Configurations_Public_get_Il2CppReferenceArray_1_BaseConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675533);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675534);
		NativeMethodInfoPtr_ResetConfigurations_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675535);
		NativeMethodInfoPtr_TryGetConfiguration_Public_Boolean_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675536);
		NativeMethodInfoPtr_TryGetConfiguration_Public_Boolean_String_byref_BaseConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675537);
		NativeMethodInfoPtr_GetConfigurationAndListenForChanges_Public_Void_Action_1_BaseConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675538);
		NativeMethodInfoPtr_UnsubscribeFromConfigurationChanges_Public_Void_Action_1_BaseConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675539);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr, 100675540);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199982, XrefRangeEnd = 199986, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199986, XrefRangeEnd = 199987, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetConfigurations()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetConfigurations_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 199994, RefRangeEnd = 200000, XrefRangeStart = 199987, XrefRangeEnd = 199994, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetConfiguration<T>(out T configuration) where T : BaseConfiguration
	{
		//IL_002d->IL002f: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		Unsafe.SkipInit(out nint num);
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			num = 0;
			reference = ref *(_003F*)(&num);
		}
		else
		{
			reference = ref configuration;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_TryGetConfiguration_Public_Boolean_byref_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		if (!typeof(T).IsValueType)
		{
			nint num2 = num;
			configuration = ((num2 == 0) ? null : IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num2, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 200000, XrefRangeEnd = 200020, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetConfiguration(string configurationName, out BaseConfiguration configuration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(configurationName);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetConfiguration_Public_Boolean_String_byref_BaseConfiguration_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		configuration = ((num3 == 0) ? null : new BaseConfiguration(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 200036, RefRangeEnd = 200039, XrefRangeStart = 200020, XrefRangeEnd = 200036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetConfigurationAndListenForChanges<T>(Il2CppSystem.Action<BaseConfiguration> onConfigChanged) where T : BaseConfiguration
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)onConfigChanged);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetConfigurationAndListenForChanges_Public_Void_Action_1_BaseConfiguration_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 200055, RefRangeEnd = 200056, XrefRangeStart = 200039, XrefRangeEnd = 200055, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnsubscribeFromConfigurationChanges<T>(Il2CppSystem.Action<BaseConfiguration> onConfigChanged) where T : BaseConfiguration
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)onConfigChanged);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_UnsubscribeFromConfigurationChanges_Public_Void_Action_1_BaseConfiguration_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 200056, XrefRangeEnd = 200059, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ConfigurationService()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ConfigurationService>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ConfigurationService(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
