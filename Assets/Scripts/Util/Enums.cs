
public class Enums
{
    public enum Tags
    {
        Wall,
        WallInProgress,
        WallOutside,
        Ennemy
    }

    public static string GetTagsValue(Tags eventValue)
    {
        return eventValue switch
        {
            Tags.Wall => "Wall",
            Tags.WallInProgress => "WallInProgress",
            Tags.WallOutside => "WallOutside",
            Tags.Ennemy => "Ennemy",
            _ => "",
        };
    }
}
