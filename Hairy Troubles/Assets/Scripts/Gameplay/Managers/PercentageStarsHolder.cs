using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageStarsHolder : MonoBehaviour
{
    #region EXPOSED_FIELD
    [SerializeField] private Image[] percentageStars = new Image[3];
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(float first, float medium, float final)
    {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;

        percentageStars[0].rectTransform.localPosition = new Vector3(size.x * first, 0, 0);
        percentageStars[1].rectTransform.localPosition = new Vector3(size.x * medium, 0, 0);
        percentageStars[2].rectTransform.localPosition = new Vector3(size.x * final, 0, 0);
    }
    #endregion
}
