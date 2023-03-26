using System.Collections.Generic;
using UnityEngine;

public class LoadAnimation : MonoBehaviour
{
    /// <Guide Rotations>
    /// 0 => 0,0,90 
    /// 1 => 0,0,180
    /// 2 => 0,0,-90
    /// 3 => 0,0,0
    /// </Guide Rotations>

    #region EXPOSED_CALLS
    [Header("Spawns")]
    [SerializeField] private List<RectTransform> spawnsList = new List<RectTransform>();
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        ChangeCharacterPosition();
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    public void ChangeCharacterPosition()
    {
        int num = Random.Range(0, spawnsList.Count);

        Vector3 newPosition = Vector3.zero;
        newPosition.x = Random.Range(-spawnsList[num].rect.width / 2, spawnsList[num].rect.width / 2);
        newPosition.y = Random.Range(-spawnsList[num].rect.height / 2, spawnsList[num].rect.height / 2);

        this.transform.parent = spawnsList[num];
        this.transform.localPosition = newPosition;

        switch(num)
        {
            case 0:
                this.transform.rotation = Quaternion.Euler(0,0,90);
                break;
            case 1:
                this.transform.rotation = Quaternion.Euler(0,0,180);
                break;
            case 2:
                this.transform.rotation = Quaternion.Euler(0,0,-90);
                break;
            case 3:
                this.transform.rotation = Quaternion.Euler(0,0,0);
                break;
        }
    }
    #endregion
}