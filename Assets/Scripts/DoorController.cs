using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public CombinationState doorColour = CombinationState.VIOLET;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool Opposite( CombinationState bulletColour)
    {
        return bulletColour switch
        {
            CombinationState.YELLOW => doorColour == CombinationState.VIOLET,
            CombinationState.BLUE => doorColour == CombinationState.ORANGE,
            CombinationState.RED => doorColour == CombinationState.GREEN,
            CombinationState.VIOLET => doorColour == CombinationState.YELLOW,
            CombinationState.ORANGE => doorColour == CombinationState.BLUE,
            CombinationState.GREEN => doorColour == CombinationState.RED,
            _ => false,
        };
    }

    public void Open()
    {
        gameObject.GetComponent<LevelLoader>().isOpen = true;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

}
