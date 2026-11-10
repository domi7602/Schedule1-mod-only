from pathlib import Path
import subprocess,json,sys,hashlib
repo=Path('C:/Users/pc/Schedule1-mod-only'); source=(repo/'Source/Mods/TownHallInterior/src/TownHallCabinetStorage.cs').read_text(encoding='utf-8-sig')
root=Path('C:/Users/pc/AppData/Local/hermes/cache/scratch/townhall-cabinet-replay');root.mkdir(parents=True,exist_ok=True)
start=source.index('    public void CaptureContents()'); first=source.index('{',start);depth=0
for i in range(first,len(source)):
    if source[i]=='{':depth+=1
    elif source[i]=='}':
        depth-=1
        if depth==0:method=source[start:i+1];break
prefix='''using System; using System.Collections.Generic;
class GameObject {public string Value="new";}
class HallCabinetRecord {public string Name="",ItemSetJson="";}
static class MelonLogger{public static void Msg(string s){}}
static class NativeHallStorage{public static bool Fail;public static string Read(GameObject o){if(Fail)throw new Exception("read-failed");return o.Value;}}
class Replay{
List<HallCabinetRecord>? _records=new();bool _placed=true,_restoreFailed;
Dictionary<string,GameObject> _hosts=new();
'''
suffix='''
static void Main(){var r=new Replay();r._records!.Add(new(){Name="a",ItemSetJson="old-a"});r._records.Add(new(){Name="unknown",ItemSetJson="keep-me"});r._hosts["a"]=new();
r.CaptureContents();if(r._records.Count!=2 || r._records.Find(x=>x.Name=="unknown")?.ItemSetJson!="keep-me")throw new Exception("BUG: unmatched cabinet checkpoint payload was discarded");
NativeHallStorage.Fail=true;try{r.CaptureContents();throw new Exception("read failure not raised");}catch(Exception e)when(e.Message=="read-failed"){}
if(r._records.Count!=2)throw new Exception("BUG: failed capture changed stored contents");
Console.WriteLine("PASS: unknown cabinet contents retained; failed capture leaves snapshot unchanged");}}
'''
(root/'Replay.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework><Nullable>enable</Nullable><NoWarn>CS0649</NoWarn></PropertyGroup></Project>',encoding='utf-8')
(root/'Program.cs').write_text(prefix+method+suffix,encoding='utf-8')
r=subprocess.run(['dotnet','run','--project',str(root/'Replay.csproj'),'-c','Release','--nologo'],text=True,capture_output=True)
print('exit',r.returncode,r.stdout,r.stderr)
variant=sys.argv[1] if len(sys.argv)>1 else 'candidate'
(root/(variant+'-result.json')).write_text(json.dumps({'source_sha256':hashlib.sha256(source.encode()).hexdigest(),'scope':'Production cabinet CaptureContents method replay with substituted native read.','exit':r.returncode,'stdout':r.stdout,'stderr':r.stderr},indent=2),encoding='utf-8')
if variant=='pre-fix':assert r.returncode!=0 and 'BUG: unmatched cabinet' in r.stderr
else:assert r.returncode==0
