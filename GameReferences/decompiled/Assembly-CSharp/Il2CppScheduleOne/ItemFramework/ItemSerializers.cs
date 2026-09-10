using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Serializing;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Product;
using Il2CppSystem;

namespace Il2CppScheduleOne.ItemFramework;

public static class ItemSerializers : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_NullItem;

	private static readonly System.IntPtr NativeMethodInfoPtr_WriteItemInstance_Public_Static_Void_Writer_ItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadItemInstance_Public_Static_ItemInstance_Reader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WriteProductItemInstance_Public_Static_Void_Writer_ProductItemInstance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadProductItemInstance_Public_Static_ProductItemInstance_Reader_0;

	public unsafe static string NullItem
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NullItem, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NullItem, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ItemSerializers()
	{
		Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ItemSerializers");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr);
		NativeFieldInfoPtr_NullItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr, "NullItem");
		NativeMethodInfoPtr_WriteItemInstance_Public_Static_Void_Writer_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr, 100672555);
		NativeMethodInfoPtr_ReadItemInstance_Public_Static_ItemInstance_Reader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr, 100672556);
		NativeMethodInfoPtr_WriteProductItemInstance_Public_Static_Void_Writer_ProductItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr, 100672557);
		NativeMethodInfoPtr_ReadProductItemInstance_Public_Static_ProductItemInstance_Reader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemSerializers>.NativeClassPtr, 100672558);
	}

	[CallerCount(92)]
	[CachedScanResults(RefRangeStart = 167681, RefRangeEnd = 167773, XrefRangeStart = 167677, XrefRangeEnd = 167681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void WriteItemInstance(this Writer writer, ItemInstance value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)writer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteItemInstance_Public_Static_Void_Writer_ItemInstance_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(46)]
	[CachedScanResults(RefRangeStart = 167776, RefRangeEnd = 167822, XrefRangeStart = 167773, XrefRangeEnd = 167776, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ItemInstance ReadItemInstance(this Reader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadItemInstance_Public_Static_ItemInstance_Reader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	[CallerCount(92)]
	[CachedScanResults(RefRangeStart = 167681, RefRangeEnd = 167773, XrefRangeStart = 167681, XrefRangeEnd = 167773, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void WriteProductItemInstance(this Writer writer, ProductItemInstance value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)writer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteProductItemInstance_Public_Static_Void_Writer_ProductItemInstance_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 167827, RefRangeEnd = 167832, XrefRangeStart = 167822, XrefRangeEnd = 167827, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ProductItemInstance ReadProductItemInstance(this Reader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadProductItemInstance_Public_Static_ProductItemInstance_Reader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ProductItemInstance>(intPtr) : null;
	}

	public ItemSerializers(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
