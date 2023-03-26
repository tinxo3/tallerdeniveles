using UnityEngine;

public class SaveManager : MonoBehaviour
{
    const int levelIndexOffset = 2;
    public HairyTroublesData data;
    public static SaveManager singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
            data = SaveSystem.LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveProgress(int starsEarned, int progressMade, int levelIndex)
    {
        if(starsEarned != 0)
        {
            data._levelClear[levelIndex-levelIndexOffset] = true;
        }
        if (data._levelStars[levelIndex - levelIndexOffset] < starsEarned)
        {
            data._levelStars[levelIndex - levelIndexOffset] = starsEarned;
        }
        if (data._levelProgress[levelIndex - levelIndexOffset] < progressMade)
        {
            data._levelProgress[levelIndex - levelIndexOffset] = progressMade;
        }
        data._stars += starsEarned;
        SaveSystem.SaveGame(data);
    }
}
