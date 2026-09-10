using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

public class GenericNPCDataObject<T> : BaseNPCDataObject where T : NPCData
{
	private static readonly System.IntPtr NativeFieldInfoPtr__data;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRuntimeData_Public_Virtual_NPCData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOriginalData_Public_Virtual_NPCData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe T _data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data);
			return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data);
			System.Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsValueType)
			{
				if (!string.Equals(typeFromHandle.FullName, "System.String"))
				{
					System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
					if (intPtr != (System.IntPtr)0 && IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						System.IntPtr intPtr2 = intPtr;
						// IL cpblk instruction
						Unsafe.CopyBlock(num2, IL2CPP.il2cpp_object_unbox(intPtr), IL2CPP.il2cpp_class_value_size(IL2CPP.il2cpp_object_get_class(intPtr2), ref *(uint*)null));
					}
					else
					{
						IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, intPtr);
					}
				}
				else
				{
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, IL2CPP.ManagedStringToIl2Cpp(val as string));
				}
			}
			else
			{
				Unsafe.Write((void*)num2, val);
			}
		}
	}

	static GenericNPCDataObject()
	{
		Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "GenericNPCDataObject`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr);
		NativeFieldInfoPtr__data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr, "_data");
		NativeMethodInfoPtr_GetRuntimeData_Public_Virtual_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr, 100682847);
		NativeMethodInfoPtr_GetOriginalData_Public_Virtual_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr, 100682848);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr, 100682849);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272361, XrefRangeEnd = 272362, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override NPCData GetRuntimeData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetRuntimeData_Public_Virtual_NPCData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NPCData>(intPtr) : null;
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override NPCData GetOriginalData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetOriginalData_Public_Virtual_NPCData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NPCData>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 199973, RefRangeEnd = 199979, XrefRangeStart = 199973, XrefRangeEnd = 199979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GenericNPCDataObject()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GenericNPCDataObject<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GenericNPCDataObject(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
