using System.Collections.Generic;
using Cpp2ILInjected;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence;

[Token(Token = "0x2000382")]
public class SaveManager : PersistentSingleton<SaveManager>
{
	[Token(Token = "0x40012BC")]
	public const string MAIN_SCENE_NAME = "Main";

	[Token(Token = "0x40012BD")]
	public const string MENU_SCENE_NAME = "Menu";

	[Token(Token = "0x40012BE")]
	public const string TUTORIAL_SCENE_NAME = "Tutorial";

	[Token(Token = "0x40012BF")]
	public const int SAVES_PER_FRAME = 15;

	[Token(Token = "0x40012C0")]
	public const string SAVE_FILE_EXTENSION = ".json";

	[Token(Token = "0x40012C1")]
	public const int SAVE_SLOT_COUNT = 5;

	[Token(Token = "0x40012C2")]
	public const string SAVE_GAME_PREFIX = "SaveGame_";

	[Token(Token = "0x40012C3")]
	public const bool DEBUG = false;

	[Token(Token = "0x40012C4")]
	public const bool PRETTY_PRINT = true;

	[Token(Token = "0x40012C5")]
	public static bool SaveError;

	[Token(Token = "0x40012CC")]
	[FieldOffset(Offset = "0x48")]
	public List<ISaveable> Saveables;

	[Token(Token = "0x40012CD")]
	[FieldOffset(Offset = "0x50")]
	public List<IBaseSaveable> BaseSaveables;

	[HideInInspector]
	[Token(Token = "0x40012CE")]
	[FieldOffset(Offset = "0x58")]
	public List<string> ApprovedBaseLevelPaths;

	[Token(Token = "0x40012CF")]
	[FieldOffset(Offset = "0x60")]
	protected List<ISaveable> CompletedSaveables;

	[Token(Token = "0x40012D0")]
	[FieldOffset(Offset = "0x68")]
	protected List<SaveRequest> QueuedSaveRequests;

	[Header("References")]
	[Token(Token = "0x40012D1")]
	[FieldOffset(Offset = "0x70")]
	public RectTransform WriteIssueDisplay;

	[Header("Events")]
	[Token(Token = "0x40012D2")]
	[FieldOffset(Offset = "0x78")]
	public UnityEvent onSaveStart;

	[Token(Token = "0x40012D3")]
	[FieldOffset(Offset = "0x80")]
	public UnityEvent onSaveComplete;

	[Token(Token = "0x40012D4")]
	[FieldOffset(Offset = "0x88")]
	private bool saveFolderInitialized;

	[Token(Token = "0x1700047C")]
	[field: Token(Token = "0x40012C6")]
	[field: FieldOffset(Offset = "0x28")]
	public bool AccessPermissionIssueDetected
	{
		[Token(Token = "0x6001816")]
		[Address(RVA = "0x45FBC0", Offset = "0x45EDC0", Length = "0x5")]
		get;
		[Token(Token = "0x6001817")]
		[Address(RVA = "0x45FBD0", Offset = "0x45EDD0", Length = "0x4")]
		protected set;
	}

	[Token(Token = "0x1700047D")]
	[field: Token(Token = "0x40012C7")]
	[field: FieldOffset(Offset = "0x29")]
	public bool IsSaving
	{
		[Token(Token = "0x6001818")]
		[Address(RVA = "0x589FE0", Offset = "0x5891E0", Length = "0x5")]
		get;
		[Token(Token = "0x6001819")]
		[Address(RVA = "0x58A0F0", Offset = "0x5892F0", Length = "0x4")]
		protected set;
	}

	[Token(Token = "0x1700047E")]
	[field: Token(Token = "0x40012C8")]
	[field: FieldOffset(Offset = "0x2C")]
	public float SecondsSinceLastSave
	{
		[Token(Token = "0x600181A")]
		[Address(RVA = "0x463B00", Offset = "0x462D00", Length = "0x6")]
		get;
		[Token(Token = "0x600181B")]
		[Address(RVA = "0x58A110", Offset = "0x589310", Length = "0x6")]
		protected set;
	}

	[Token(Token = "0x1700047F")]
	[field: Token(Token = "0x40012C9")]
	[field: FieldOffset(Offset = "0x30")]
	public string PlayersSavePath
	{
		[Token(Token = "0x600181C")]
		[Address(RVA = "0x421DD0", Offset = "0x420FD0", Length = "0x5")]
		get;
		[Token(Token = "0x600181D")]
		[Address(RVA = "0x421E10", Offset = "0x421010", Length = "0xD")]
		protected set;
	}

	[Token(Token = "0x17000480")]
	[field: Token(Token = "0x40012CA")]
	[field: FieldOffset(Offset = "0x38")]
	public string IndividualSavesContainerPath
	{
		[Token(Token = "0x600181E")]
		[Address(RVA = "0x421DE0", Offset = "0x420FE0", Length = "0x5")]
		get;
		[Token(Token = "0x600181F")]
		[Address(RVA = "0x421E20", Offset = "0x421020", Length = "0xD")]
		protected set;
	}

	[Token(Token = "0x17000481")]
	public string BackupFolderPath
	{
		[Token(Token = "0x6001820")]
		[Address(RVA = "0x5AB0E0", Offset = "0x5AA2E0", Length = "0x61")]
		get
		{
			return null;
		}
	}

	[Token(Token = "0x17000482")]
	[field: Token(Token = "0x40012CB")]
	[field: FieldOffset(Offset = "0x40")]
	public string SaveName
	{
		[Token(Token = "0x6001821")]
		[Address(RVA = "0x4610C0", Offset = "0x4602C0", Length = "0x5")]
		get;
		[Token(Token = "0x6001822")]
		[Address(RVA = "0x49EF90", Offset = "0x49E190", Length = "0xD")]
		protected set;
	}

	[Token(Token = "0x6001815")]
	[Address(RVA = "0x5AA710", Offset = "0x5A9910", Length = "0x36")]
	public static void ReportSaveError()
	{
	}

	[Token(Token = "0x6001823")]
	[Address(RVA = "0x5A8F70", Offset = "0x5A8170", Length = "0x2BD")]
	protected override void Awake()
	{
	}

	[Token(Token = "0x6001824")]
	[Address(RVA = "0x5AACB0", Offset = "0x5A9EB0", Length = "0x102")]
	protected override void Start()
	{
	}

	[Token(Token = "0x6001825")]
	[Address(RVA = "0x5A9230", Offset = "0x5A8430", Length = "0x2AA")]
	public void CheckSaveFolderInitialized()
	{
	}

	[Token(Token = "0x6001826")]
	[Address(RVA = "0x5AA3A0", Offset = "0x5A95A0", Length = "0xB6")]
	public static bool HasWritePermissionOnDir(string path)
	{
		return false;
	}

	[Token(Token = "0x6001827")]
	[Address(RVA = "0x5AAE30", Offset = "0x5AA030", Length = "0xA9")]
	private void Update()
	{
	}

	[Token(Token = "0x6001828")]
	[Address(RVA = "0x5AA800", Offset = "0x5A9A00", Length = "0x85")]
	public void Save()
	{
	}

	[Token(Token = "0x6001829")]
	[Address(RVA = "0x5AA890", Offset = "0x5A9A90", Length = "0x414")]
	public void Save(string saveFolderPath)
	{
	}

	[Token(Token = "0x600182A")]
	[Address(RVA = "0x5A9580", Offset = "0x5A8780", Length = "0x4BF")]
	private void ClearBaseLevelOutdatedSaves(string saveFolderPath)
	{
	}

	[Token(Token = "0x600182B")]
	[Address(RVA = "0x5A9AA0", Offset = "0x5A8CA0", Length = "0xC3")]
	public void CompleteSaveable(ISaveable saveable)
	{
	}

	[Token(Token = "0x600182C")]
	[Address(RVA = "0x5A9A40", Offset = "0x5A8C40", Length = "0x53")]
	public void ClearCompletedSaveable(ISaveable saveable)
	{
	}

	[Token(Token = "0x600182D")]
	[Address(RVA = "0x5A9B70", Offset = "0x5A8D70", Length = "0x42C")]
	public void CreateSaveBackup(SaveInfo saveInfo)
	{
	}

	[Token(Token = "0x600182E")]
	[Address(RVA = "0x5AA5C0", Offset = "0x5A97C0", Length = "0x148")]
	public void RegisterSaveable(ISaveable saveable)
	{
	}

	[Token(Token = "0x600182F")]
	[Address(RVA = "0x5AA510", Offset = "0x5A9710", Length = "0xAA")]
	public void QueueSaveRequest(SaveRequest request)
	{
	}

	[Token(Token = "0x6001830")]
	[Address(RVA = "0x5A9FA0", Offset = "0x5A91A0", Length = "0x53")]
	public void DequeueSaveRequest(SaveRequest request)
	{
	}

	[Token(Token = "0x6001831")]
	[Address(RVA = "0x5AADC0", Offset = "0x5A9FC0", Length = "0x63")]
	public static string StripExtensions(string filePath)
	{
		return null;
	}

	[Token(Token = "0x6001832")]
	[Address(RVA = "0x5AA460", Offset = "0x5A9660", Length = "0xA5")]
	public static string MakeFileSafe(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6001833")]
	[Address(RVA = "0x5AA1E0", Offset = "0x5A93E0", Length = "0x1B9")]
	public static float GetVersionNumber(string version)
	{
		return 0f;
	}

	[Token(Token = "0x6001834")]
	[Address(RVA = "0x5A94E0", Offset = "0x5A86E0", Length = "0x95")]
	private void Clean()
	{
	}

	[Token(Token = "0x6001835")]
	[Address(RVA = "0x5AA000", Offset = "0x5A9200", Length = "0x1DA")]
	public void DisablePlayTutorial(SaveInfo info)
	{
	}

	[Token(Token = "0x6001836")]
	[Address(RVA = "0x5AA750", Offset = "0x5A9950", Length = "0xA5")]
	public static string SanitizeFileName(string fileName)
	{
		return null;
	}

	[Token(Token = "0x6001837")]
	[Address(RVA = "0x5AAEE0", Offset = "0x5AA0E0", Length = "0x1FB")]
	public SaveManager()
	{
	}
}
