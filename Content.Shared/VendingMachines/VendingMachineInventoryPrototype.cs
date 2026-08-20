using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.VendingMachines
{
    // ST:OW begin
    [DataDefinition]
    public sealed partial class VendingInventoryCategoryEntry
    {
        [DataField("id", required: true)]
        public string ID = default!;

        [DataField("amount", required: true)]
        public uint Amount;

        [DataField("category")]
        public string Category = "misc";
    }
    // ST:OW end
    [Prototype]
    public sealed partial class VendingMachineInventoryPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("startingInventory", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<uint, EntityPrototype>))]
        public Dictionary<string, uint> StartingInventory { get; private set; } = new();

        [DataField("emaggedInventory", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<uint, EntityPrototype>))]
        public Dictionary<string, uint>? EmaggedInventory { get; private set; }

        [DataField("contrabandInventory", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<uint, EntityPrototype>))]
        public Dictionary<string, uint>? ContrabandInventory { get; private set; }
        
        // ST:OW begin
        [DataField("categoryGroups")]
        public List<VendingCategoryGroup>? CategoryGroups { get; private set; }
        
        [DataDefinition]
        public sealed partial class VendingCategoryGroup
        {
            [DataField("category", required: true)]
            public string Category = "misc";

            [DataField("entries", required: true, customTypeSerializer: typeof(PrototypeIdDictionarySerializer<uint, EntityPrototype>))]
            public Dictionary<string, uint> Entries = new();
        }
        // ST:OW end
    }
}
