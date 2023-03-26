[System.Serializable]
public class HairyTroublesData
{
    const int totalLevels = 3;
    public int _stars;
    public int[] _levelStars;
    public int[] _levelProgress;
    public bool[] _levelClear;
    public HairyTroublesData()
    {
        _stars = 0;
        _levelStars = new int[totalLevels];
        _levelProgress = new int[totalLevels];
        _levelClear = new bool[totalLevels];
    }
    public HairyTroublesData(int stars,int[] levelStars, int[] levelProgress,bool[] levelClear)
    {
        _stars = stars;
        _levelStars = levelStars;
        _levelProgress = levelProgress;
        _levelClear = levelClear;
    }

}
