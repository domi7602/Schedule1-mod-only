using S1API.Entities;
using S1API.Entities.NPCs;

namespace HitmanPhone.Bounty;

/// <summary>
/// A non-physical S1API NPC that acts as the virtual caller for all hitman bounties.
/// Because IsPhysical is false, this NPC never spawns in the 3D world, but it gets
/// a full S1API wrapper which allows us to use SendTextMessage to its dedicated
/// phone thread ("Unknown Number").
/// </summary>
public sealed class HitmanCallerNPC : NPC
{
    public const string NPC_ID = "hitman_unknown_caller";

    public override bool IsPhysical => false;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity(id: NPC_ID, firstName: "Unknown", lastName: "Number");
    }
}
