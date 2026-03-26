namespace PeopleVille;

public class HumanAppearance
{
    public int HairColor { get; set; } = RandomRGB();
    
    public string HairColorFormatted => HairColor.ToString("X8");
    
    public int SkinColor { get; set; } = RandomRGB();
    
    public string SkinColorFormatted => SkinColor.ToString("X8");

    private static int RandomRGB()
    {
        return (int)(Random.Shared.Next() | 0xFF000000);
    }
}