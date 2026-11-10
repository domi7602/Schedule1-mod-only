from pathlib import Path
import subprocess,json,hashlib
repo=Path('C:/Users/pc/Schedule1-mod-only'); base=repo/'Source/Mods/TownHallInterior/src'
source=(base/'TownHallCabinetStorage.cs').read_text(encoding='utf-8-sig');domain=(base/'BusinessPersistenceDomain.cs').read_text(encoding='utf-8-sig')
root=Path('C:/Users/pc/AppData/Local/hermes/cache/scratch/townhall-storage-replay');root.mkdir(parents=True,exist_ok=True)
def extract(text,marker):
    start=text.index(marker);first=text.index('{',start);depth=0
    for i in range(first,len(text)):
        if text[i]=='{':depth+=1
        elif text[i]=='}':
            depth-=1
            if depth==0:return text[start:i+1]
    raise ValueError(marker)
production=extract(domain,'public static class HallStorageSnapshot')+'\n'+extract(source,'internal static class NativeHallStorage')
stubs=r'''using System; using System.IO; using System.Text.Json; using System.Collections.Generic; using System.Linq;
class Definition { public string ID="largestoragerack"; }
class ItemInstance { public Definition Definition=new(); public int Quantity=1; public string Json=""; }
class ItemSlot { public ItemInstance? ItemInstance; public ItemInstance? _ItemInstance_k__BackingField=>ItemInstance; public int Quantity=>ItemInstance?.Quantity??0; }
class StorageEntity { public List<ItemSlot> ItemSlots=new(){new(),new()}; public int SlotCount=2; }
class GameObject { public StorageEntity Storage=new(); public T? GetComponentInChildren<T>(bool b) where T:class =>Storage as T; }
class ItemSet {
public static bool FalseEmpty,DropItem,NoEffect;
public const string Empty="{\"DataType\":\"ItemData\",\"ID\":\"\",\"Quantity\":0}";
List<ItemSlot> slots;public ItemSet(List<ItemSlot> s){slots=s;}
public string GetJSON()=>JsonSerializer.Serialize(new { Items=slots.Select(s=>FalseEmpty?Empty:s.ItemInstance?.Json??Empty).ToArray() });
public static bool TryDeserialize(string json,out DeserializedItemSet contents){using var d=JsonDocument.Parse(json);var list=new List<ItemInstance?>();foreach(var e in d.RootElement.GetProperty("Items").EnumerateArray()){
string? s=e.ValueKind==JsonValueKind.Null?null:e.GetString();if(HallStorageSnapshot.IsEmptyItem(s)||DropItem){list.Add(null);continue;}
using var item=JsonDocument.Parse(s!);list.Add(new(){Definition=new(){ID=item.RootElement.GetProperty("ID").GetString()!},Quantity=item.RootElement.GetProperty("Quantity").GetInt32(),Json=s!});}
contents=new(){Items=list.ToArray()};return true;}
}
class DeserializedItemSet { public ItemInstance?[] Items=Array.Empty<ItemInstance?>();public void LoadTo(List<ItemSlot> s){if(ItemSet.NoEffect)return;for(int i=0;i<Items.Length;i++)s[i].ItemInstance=Items[i];} }
'''
tests=r'''
class Replay {
static void Refused(Action a){try{a();}catch(InvalidDataException){return;}throw new Exception("Unsafe native result was accepted");}
static void Main(){
var source=new GameObject();source.Storage.ItemSlots[1].ItemInstance=new(){Json="{\"DataType\":\"QualityItemData\",\"ID\":\"largestoragerack\",\"Quantity\":1,\"Quality\":4}"};
string saved=NativeHallStorage.Read(source);var target=new GameObject();NativeHallStorage.Restore(target,saved);
if(target.Storage.ItemSlots[1].ItemInstance?.Json!=source.Storage.ItemSlots[1].ItemInstance?.Json)throw new Exception("Slot or item metadata lost");
ItemSet.FalseEmpty=true;Refused(()=>NativeHallStorage.Read(source));ItemSet.FalseEmpty=false;
ItemSet.DropItem=true;Refused(()=>NativeHallStorage.Restore(new(),saved));ItemSet.DropItem=false;
ItemSet.NoEffect=true;Refused(()=>NativeHallStorage.Restore(new(),saved));ItemSet.NoEffect=false;
Console.WriteLine("PASS: full metadata and slot preserved; false-empty capture, missing item, and ineffective restore refused (4 scenarios)");
}}
'''
(root/'Replay.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework><Nullable>enable</Nullable></PropertyGroup></Project>',encoding='utf-8')
(root/'Program.cs').write_text(stubs+production+tests,encoding='utf-8')
r=subprocess.run(['dotnet','run','--project',str(root/'Replay.csproj'),'-c','Release','--nologo'],text=True,capture_output=True)
print('exit',r.returncode,r.stdout,r.stderr)
(root/'result.json').write_text(json.dumps({'source_sha256':hashlib.sha256(source.encode()).hexdigest(),'scope':'Exact production NativeHallStorage and HallStorageSnapshot classes; substituted Unity and native ItemSet boundaries. Does not verify IL2CPP codec/runtime.','exit':r.returncode,'stdout':r.stdout,'stderr':r.stderr},indent=2),encoding='utf-8')
assert r.returncode==0
