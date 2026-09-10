using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Effects;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Product;

[Serializable]
public class PropertyItemDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr_Properties;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Virtual_New_Void_List_1_Effect_0;

	private static readonly IntPtr NativeMethodInfoPtr_HasProperty_Public_Boolean_Effect_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<Effect> Properties
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Properties);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Effect>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Properties)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static PropertyItemDefinition()
	{
		Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "PropertyItemDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_Properties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr, "Properties");
		NativeMethodInfoPtr_Initialize_Public_Virtual_New_Void_List_1_Effect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr, 100679572);
		NativeMethodInfoPtr_HasProperty_Public_Boolean_Effect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr, 100679573);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr, 100679574);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 241622, RefRangeEnd = 241623, XrefRangeStart = 241618, XrefRangeEnd = 241622, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Initialize(List<Effect> properties)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)properties);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialize_Public_Virtual_New_Void_List_1_Effect_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 241627, RefRangeEnd = 241629, XrefRangeStart = 241623, XrefRangeEnd = 241627, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool HasProperty(Effect property)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)property);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasProperty_Public_Boolean_Effect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 241637, RefRangeEnd = 241638, XrefRangeStart = 241629, XrefRangeEnd = 241637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PropertyItemDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PropertyItemDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PropertyItemDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
