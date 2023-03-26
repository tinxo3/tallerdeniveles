using UnityEngine;

public class Control_Obj_Eliminations : MB_SingletonDestroy<Control_Obj_Eliminations>
{
    [Header("Furnitures")]
    [SerializeField] private float furnitureSeconds = 10f;
    [SerializeField] private float furnitureSpeed = 2f;

    [Header("Ornaments")]
    [SerializeField] private float ornamentSeconds = 10f;
    [SerializeField] private float ornamentSpeed = 2f;

    // ------------------------------

    public float GetSeconds(EliminateByTime.Type type)
    {
        switch (type)
        {
            case EliminateByTime.Type.Furniture:
                return furnitureSeconds;
            case EliminateByTime.Type.Ornaments:
                return ornamentSeconds;
        }

        return 0;
    }

    public float GetSpeed(EliminateByTime.Type type)
    {
        switch (type)
        {
            case EliminateByTime.Type.Furniture:
                return furnitureSpeed;
            case EliminateByTime.Type.Ornaments:
                return ornamentSpeed;
        }

        return 0;
    }
}
