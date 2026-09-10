using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSteamworks;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Runtime.CompilerServices;
using Il2CppSystem.Threading.Tasks;

namespace Il2CppScheduleOne.Platform;

public static class PlatformAuth : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EAuthState")]
	public enum EAuthState
	{
		NotRequested,
		Pending,
		Ready,
		Failed
	}

	[ObfuscatedName("ScheduleOne.Platform.PlatformAuth+<<PrepareAuth>g__Timeout|6_0>d")]
	public sealed class ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe Il2CppSystem.Object __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique()
		{
			Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "<<PrepareAuth>g__Timeout|6_0>d");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, "<>2__current");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, 100668680);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, 100668681);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, 100668682);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, 100668683);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, 100668684);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr, 100668685);
		}

		[CallerCount(83)]
		[CachedScanResults(RefRangeStart = 65267, RefRangeEnd = 65350, XrefRangeStart = 65267, XrefRangeEnd = 65350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique(int _003C_003E1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&_003C_003E1__state);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123692, XrefRangeEnd = 123700, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123700, XrefRangeEnd = 123705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObObObUnique(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.Platform.PlatformAuth+<InitSteamAppTicket>d__13")]
	public sealed class _InitSteamAppTicket_d__13 : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___t__builder;

		private static readonly System.IntPtr NativeFieldInfoPtr___u__1;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetStateMachine_Private_Virtual_Final_New_Void_IAsyncStateMachine_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe Il2CppSystem.Runtime.CompilerServices.AsyncTaskMethodBuilder __t__builder
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___t__builder);
				return new Il2CppSystem.Runtime.CompilerServices.AsyncTaskMethodBuilder(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Runtime.CompilerServices.AsyncTaskMethodBuilder>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___t__builder), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)asyncTaskMethodBuilder)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Runtime.CompilerServices.AsyncTaskMethodBuilder>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe Il2CppSystem.Runtime.CompilerServices.TaskAwaiter<string> __u__1
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___u__1);
				return new Il2CppSystem.Runtime.CompilerServices.TaskAwaiter<string>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Runtime.CompilerServices.TaskAwaiter<string>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___u__1), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)taskAwaiter)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Runtime.CompilerServices.TaskAwaiter<string>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		static _InitSteamAppTicket_d__13()
		{
			Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "<InitSteamAppTicket>d__13");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___t__builder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr, "<>t__builder");
			NativeFieldInfoPtr___u__1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr, "<>u__1");
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr, 100668686);
			NativeMethodInfoPtr_SetStateMachine_Private_Virtual_Final_New_Void_IAsyncStateMachine_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr, 100668687);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123705, XrefRangeEnd = 123779, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Void_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123779, XrefRangeEnd = 123783, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void SetStateMachine(Il2CppSystem.Runtime.CompilerServices.IAsyncStateMachine stateMachine)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stateMachine);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStateMachine_Private_Virtual_Final_New_Void_IAsyncStateMachine_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public _InitSteamAppTicket_d__13(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public _InitSteamAppTicket_d__13()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_InitSteamAppTicket_d__13>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_AuthRequestTimeoutSeconds;

	private static readonly System.IntPtr NativeFieldInfoPtr__authState;

	private static readonly System.IntPtr NativeFieldInfoPtr_AuthTicketBufferSize;

	private static readonly System.IntPtr NativeFieldInfoPtr__appTicket;

	private static readonly System.IntPtr NativeFieldInfoPtr_appTicketCallbackResponse;

	private static readonly System.IntPtr NativeFieldInfoPtr__tokenCompletion;

	private static readonly System.IntPtr NativeFieldInfoPtr__currentAuthTicket;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AuthState_Public_Static_get_EAuthState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AuthState_Private_Static_set_Void_EAuthState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareAuth_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAuthToken_Public_Static_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitSteamAppTicket_Private_Static_Task_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEncryptedAppTicketResponse_Private_Static_Void_EncryptedAppTicketResponse_t_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAppTicket_Private_Static_Task_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CleanTicket_Private_Static_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAuthSessionTicket_Public_Static_Boolean_CSteamID_byref_SteamSessionAuthTicket_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CancelCurrentAuthSessionTicket_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BeginAuthSession_Public_Static_EBeginAuthSessionResult_SteamSessionAuthTicket_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EndAuthSession_Public_Static_Void_CSteamID_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Internal_Static_IEnumerator_PDM_0;

	public unsafe static float AuthRequestTimeoutSeconds
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AuthRequestTimeoutSeconds, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AuthRequestTimeoutSeconds, (void*)(&num));
		}
	}

	public unsafe static EAuthState _authState
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out EAuthState result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__authState, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__authState, (void*)(&eAuthState));
		}
	}

	public unsafe static int AuthTicketBufferSize
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AuthTicketBufferSize, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AuthTicketBufferSize, (void*)(&num));
		}
	}

	public unsafe static string _appTicket
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__appTicket, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__appTicket, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static CallResult<EncryptedAppTicketResponse_t> appTicketCallbackResponse
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_appTicketCallbackResponse, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<CallResult<EncryptedAppTicketResponse_t>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_appTicketCallbackResponse, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callResult));
		}
	}

	public unsafe static TaskCompletionSource<string> _tokenCompletion
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__tokenCompletion, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<TaskCompletionSource<string>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__tokenCompletion, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)taskCompletionSource));
		}
	}

	public unsafe static HAuthTicket _currentAuthTicket
	{
		get
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out HAuthTicket result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__currentAuthTicket, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__currentAuthTicket, (void*)(&hAuthTicket));
		}
	}

	public unsafe static EAuthState AuthState
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123783, XrefRangeEnd = 123787, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AuthState_Public_Static_get_EAuthState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EAuthState*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 123804, RefRangeEnd = 123807, XrefRangeStart = 123787, XrefRangeEnd = 123804, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AuthState_Private_Static_set_Void_EAuthState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static PlatformAuth()
	{
		Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Platform", "PlatformAuth");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr);
		NativeFieldInfoPtr_AuthRequestTimeoutSeconds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "AuthRequestTimeoutSeconds");
		NativeFieldInfoPtr__authState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "_authState");
		NativeFieldInfoPtr_AuthTicketBufferSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "AuthTicketBufferSize");
		NativeFieldInfoPtr__appTicket = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "_appTicket");
		NativeFieldInfoPtr_appTicketCallbackResponse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "appTicketCallbackResponse");
		NativeFieldInfoPtr__tokenCompletion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "_tokenCompletion");
		NativeFieldInfoPtr__currentAuthTicket = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, "_currentAuthTicket");
		NativeMethodInfoPtr_get_AuthState_Public_Static_get_EAuthState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668666);
		NativeMethodInfoPtr_set_AuthState_Private_Static_set_Void_EAuthState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668667);
		NativeMethodInfoPtr_PrepareAuth_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668668);
		NativeMethodInfoPtr_GetAuthToken_Public_Static_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668669);
		NativeMethodInfoPtr_InitSteamAppTicket_Private_Static_Task_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668670);
		NativeMethodInfoPtr_OnEncryptedAppTicketResponse_Private_Static_Void_EncryptedAppTicketResponse_t_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668671);
		NativeMethodInfoPtr_GetAppTicket_Private_Static_Task_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668672);
		NativeMethodInfoPtr_CleanTicket_Private_Static_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668673);
		NativeMethodInfoPtr_GetAuthSessionTicket_Public_Static_Boolean_CSteamID_byref_SteamSessionAuthTicket_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668674);
		NativeMethodInfoPtr_CancelCurrentAuthSessionTicket_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668675);
		NativeMethodInfoPtr_BeginAuthSession_Public_Static_EBeginAuthSessionResult_SteamSessionAuthTicket_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668676);
		NativeMethodInfoPtr_EndAuthSession_Public_Static_Void_CSteamID_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668677);
		NativeMethodInfoPtr_Method_Internal_Static_IEnumerator_PDM_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformAuth>.NativeClassPtr, 100668679);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 123841, RefRangeEnd = 123846, XrefRangeStart = 123807, XrefRangeEnd = 123841, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void PrepareAuth()
	{
		System.IntPtr* ptr = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareAuth_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 123855, RefRangeEnd = 123857, XrefRangeStart = 123846, XrefRangeEnd = 123855, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetAuthToken()
	{
		System.IntPtr* ptr = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAuthToken_Public_Static_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123857, XrefRangeEnd = 123866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Task InitSteamAppTicket()
	{
		System.IntPtr* ptr = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitSteamAppTicket_Private_Static_Task_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Task>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123866, XrefRangeEnd = 123889, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OnEncryptedAppTicketResponse(EncryptedAppTicketResponse_t response, bool ioFailure)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&response);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &ioFailure;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEncryptedAppTicketResponse_Private_Static_Void_EncryptedAppTicketResponse_t_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123889, XrefRangeEnd = 123911, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Task<string> GetAppTicket()
	{
		System.IntPtr* ptr = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAppTicket_Private_Static_Task_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Task<string>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123911, XrefRangeEnd = 123917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string CleanTicket(string ticket)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(ticket);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CleanTicket_Private_Static_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123917, XrefRangeEnd = 123955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetAuthSessionTicket(CSteamID recipientSteamId, out SteamSessionAuthTicket result)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&recipientSteamId);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAuthSessionTicket_Public_Static_Boolean_CSteamID_byref_SteamSessionAuthTicket_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		result = ((num3 == 0) ? null : new SteamSessionAuthTicket(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 123976, RefRangeEnd = 123979, XrefRangeStart = 123955, XrefRangeEnd = 123976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CancelCurrentAuthSessionTicket()
	{
		System.IntPtr* ptr = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CancelCurrentAuthSessionTicket_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 123988, RefRangeEnd = 123989, XrefRangeStart = 123979, XrefRangeEnd = 123988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EBeginAuthSessionResult BeginAuthSession(SteamSessionAuthTicket ticket)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)ticket));
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginAuthSession_Public_Static_EBeginAuthSessionResult_SteamSessionAuthTicket_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(EBeginAuthSessionResult*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 123998, RefRangeEnd = 123999, XrefRangeStart = 123989, XrefRangeEnd = 123998, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void EndAuthSession(CSteamID steamID)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&steamID);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndAuthSession_Public_Static_Void_CSteamID_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 123999, XrefRangeEnd = 124003, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerator Method_Internal_Static_IEnumerator_PDM_0()
	{
		System.IntPtr* ptr = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Internal_Static_IEnumerator_PDM_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	public PlatformAuth(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
