from pathlib import Path
import subprocess, os, json, hashlib
repo=Path('C:/Users/pc/Schedule1-mod-only')
source=(repo/'Source/Mods/TownHallInterior/src/HallGrid.cs').read_text(encoding='utf-8-sig')
root=Path('C:/Users/pc/AppData/Local/hermes/cache/scratch/townhall-lifecycle-replay')
root.mkdir(parents=True,exist_ok=True)
def method(marker):
    start=source.index(marker)
    first=source.index('{',start)
    depth=0
    for i in range(first,len(source)):
        if source[i]=='{': depth+=1
        elif source[i]=='}':
            depth-=1
            if depth==0:return source[start:i+1]
    raise ValueError(marker)
methods='\n'.join(method(m) for m in ['internal static void Dispose()', 'internal static void Bind(', 'internal static void TryRestore()'])
prefix='''using System; using System.Collections.Generic;
class GameObject {} class HallPlacementRecord { public string Id=""; public bool IsPlausible()=>true; }
static class MelonLogger { public static void Msg(string s){} }
namespace UnityEngine { static class Object { public static void Destroy(global::GameObject o){} } }
class Replay {
static GameObject? _root; static List<HallPlacementRecord>? _records;
static Dictionary<int,string> PlacedIds=new(), RecordIds=new(); static HashSet<int> HallOwned=new();
static HashSet<string> SpawnedRecords=new(), FailedContents=new();
static Dictionary<string,GameObject> Live=new(); static int restored;
static bool RestoreOne(HallPlacementRecord r){restored++;Live[r.Id]=new();return true;}
'''
suffix='''
static void Main(){
_root=new(); Bind(new(){new(){Id="old-world"}}); Dispose();
_root=new(); TryRestore();
if(restored!=0) throw new Exception("BUG: old checkpoint restored before new-world binding");
Bind(new(){new(){Id="new-world"}}); TryRestore(); TryRestore();
if(restored!=1 || Live.ContainsKey("old-world"))throw new Exception("BUG: missing or duplicate new-world restore");
Dispose(); _root=new(); Bind(new()); TryRestore();
if(restored!=1)throw new Exception("BUG: old placement leaked into an empty checkpoint");
Console.WriteLine("PASS: no restore before bind; one new-world restore; no duplicate; empty world stays empty");
}}
'''
(root/'Replay.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework><Nullable>enable</Nullable></PropertyGroup></Project>',encoding='utf-8')
results=[]
for variant,body in [('pre-fix',methods.replace('        _records = null;\n','')),('candidate',methods)]:
    (root/'Program.cs').write_text(prefix+body+suffix,encoding='utf-8')
    result=subprocess.run(['dotnet','run','--project',str(root/'Replay.csproj'),'-c','Release','--nologo'],text=True,capture_output=True)
    print(variant,'exit',result.returncode,result.stdout,result.stderr)
    results.append({'variant':variant,'exit':result.returncode,'stdout':result.stdout,'stderr':result.stderr})
(root/'results.json').write_text(json.dumps({'source_sha256':hashlib.sha256(source.encode()).hexdigest(),'scope':'Extracted production Dispose/Bind/TryRestore; Unity/spawn boundaries substituted. Pre-fix reset omission reproduced from observed original source. Not a native game test.','results':results},indent=2),encoding='utf-8')
assert results[0]['exit']!=0 and results[1]['exit']==0
print('RESULTS',root/'results.json')
