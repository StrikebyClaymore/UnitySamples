using UISample.Features;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "MapGeneratorConfig", menuName = "UISample/MapGeneratorConfig")]
    public class MapGeneratorConfig : ScriptableObject
    {
        [field: SerializeField] public TileBase Ground { get; set; }
        [field: SerializeField] public TileBase Trunk { get; set; }
        [field: SerializeField] public TileBase Branch { get; set; }
        [field: SerializeField] public TileBase Leaves { get; set; }
        [field: SerializeField] public TileBase TrunkHollow { get; set; }
        [field: SerializeField] public RuleTile Crown { get; set; }
        
        [field: SerializeField] public Vector2Int BranchesRange { get; set; } = new(2, 3);
        [field: SerializeField] public int HollowSpawnNumber { get; set; }  = 10;
        [field: SerializeField] public Acorn AcornPrefab { get; set; }
    }
}