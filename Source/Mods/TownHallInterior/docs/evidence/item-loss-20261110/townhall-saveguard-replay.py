from pathlib import Path
import subprocess,json,hashlib,sys
repo=Path('C:/Users/pc/Schedule1-mod-only')
source=(repo/'Source/Mods/TownHallInterior/src/Mod.cs').read_text(encoding='utf-8-sig')
root=Path('C:/Users/pc/AppData/Local/hermes/cache/scratch/townhall-saveguard-replay');root.mkdir(parents=True,exist_ok=True)
start=source.index('    private static ',source.index('private static bool SameSlot'))
# Locate the save prefix independently of its return type.
start=source.rfind('    private static ',0,source.index('BeforeNativeSave(SaveManager'))
first=source.index('{',start);depth=0
for i in range(first,len(source)):
    if source[i]=='{':depth+=1
    elif source[i]=='}':
        depth-=1
        if depth==0:method=source[start:i+1];break
def extract(marker):
    start=source.index(marker); first=source.index('{',start); depth=0
    for i in range(first,len(source)):
        if source[i]=='{':depth+=1
        elif source[i]=='}':
            depth-=1
            if depth==0:return source[start:i+1]
    raise ValueError(marker)
callbacks=extract('    private static void AfterNativeSave(')+'\n'+extract('    private void OnNativeSaveStart()')
prefix='''using System;
class SaveManager { public bool IsSaving; public IntPtr Pointer=(IntPtr)1; }
static class PersistentSingleton<T> {public static T? Instance;}
class Session { public int PrepareCalls; public bool HasPendingSave; public string SlotPath="slot2"; public void Prepare(TownHallNativeValues v){PrepareCalls++;if(Replay.FailPrepare)throw new Exception("sidecar write failed");HasPendingSave=true;} }
record struct TownHallNativeValues(float Bank,float Cash,int Time,int Days);
class Clock { public int CurrentTime=1200,ElapsedDays=0; }
class Purchase { public bool IsMutating; }
class Laundering { public bool IsMutating; public void SyncStatsActiveOperations(object o){} }
static class NetworkGuard {public static bool IsAlive(object? o)=>o!=null;}
static class TownHallNativeMoney {public static bool TryReadWallet(out float bank,out float cash){bank=10;cash=10;return true;}}
static class MelonLogger { public static void Warning(string s){} }
class Cabinet { public void CaptureContents(){if(Replay.FailCapture)throw new Exception("unread cabinet");} }
static class HallGrid { public static void CaptureContents(){if(Replay.FailCapture)throw new Exception("unread hall");} }
class Replay { public static bool FailCapture,FailPrepare;
static Replay? Instance; Session? _saveSession=new(); bool _persistenceFailed;
IntPtr _requestedSaveManager,_acceptedSaveManager; Cabinet _cabinets=new();
void BlockForSaveFailure(Exception ex){_persistenceFailed=true;}
Clock? _clock=new();Purchase _purchaseManager=new();Laundering _launderingManager=new();object _businessData=new();void AdvanceLaunderingClock(){}
object GetBoundContext(bool allowSaving)=>new(); static bool SameSlot(string a,string b)=>a==b;
'''
invoke='BeforeNativeSave(new(), "slot2", out state); return true;' if 'static void BeforeNativeSave' in method else 'return BeforeNativeSave(new(), "slot2", out state);'
suffix='''
static bool Invoke(out bool state){INVOKE}
static void Main(){Instance=new();FailCapture=true;
bool run=Invoke(out bool state); if(run || state)throw new Exception("BUG: native save proceeds although storage capture failed");
Instance=new();FailCapture=false;run=Invoke(out state);
if(!run || !state)throw new Exception("BUG: readable bound storage cannot save");
Instance=new();FailPrepare=true;run=Invoke(out state);
if(run || state)throw new Exception("BUG: native save proceeds before durable pending preparation succeeds");
Instance=new();FailPrepare=false;run=Invoke(out state);
if(!run || !state || !Instance._saveSession!.HasPendingSave)throw new Exception("BUG: native writer admitted without durable pending checkpoint");
run=Invoke(out state);if(run || state)throw new Exception("BUG: second writer admitted while pending snapshot exists");
Instance=new();var manager=new SaveManager();run=BeforeNativeSave(manager,"slot2",out state);
manager.IsSaving=true;PersistentSingleton<SaveManager>.Instance=manager;Instance.OnNativeSaveStart();AfterNativeSave(manager,state);
if(!run || Instance._persistenceFailed || Instance._acceptedSaveManager!=manager.Pointer || Instance._saveSession!.PrepareCalls!=1)
throw new Exception("BUG: prepared save/start/finalizer handshake failed or prepared twice");
Instance=new();manager=new();run=BeforeNativeSave(manager,"slot2",out state);AfterNativeSave(manager,state);
if(!Instance._persistenceFailed || !Instance._saveSession!.HasPendingSave)throw new Exception("BUG: missing start callback did not protect pending state");
Console.WriteLine("PASS: capture/prepare failures block native writer; pending precedes writer; duplicate refused; start/finalizer handshake and missing callback protected");}
}
'''.replace('INVOKE',invoke)
(root/'Replay.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework><Nullable>enable</Nullable><NoWarn>CS0649</NoWarn></PropertyGroup></Project>',encoding='utf-8')
(root/'Program.cs').write_text(prefix+method+callbacks+suffix,encoding='utf-8')
r=subprocess.run(['dotnet','run','--project',str(root/'Replay.csproj'),'-c','Release','--nologo'],text=True,capture_output=True)
print('exit',r.returncode,r.stdout,r.stderr)
variant=sys.argv[1] if len(sys.argv)>1 else 'candidate'
(root/(variant+'-result.json')).write_text(json.dumps({'variant':variant,'source_sha256':hashlib.sha256(source.encode()).hexdigest(),'scope':'Exact production save prefix with substituted native/storage boundaries; not a native integration test.','exit':r.returncode,'stdout':r.stdout,'stderr':r.stderr},indent=2),encoding='utf-8')
if variant.endswith('pre-fix'):assert r.returncode!=0 and 'BUG: native save proceeds' in r.stderr
else:assert r.returncode==0
