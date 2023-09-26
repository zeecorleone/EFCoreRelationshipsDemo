using System.Text.Json.Serialization;

namespace EFCoreRelationshipsDemo.EntityModels;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    [JsonIgnore]
    public List<Character> Character { get; set; }
}

