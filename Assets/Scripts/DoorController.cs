using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private CombinationState doorColour = CombinationState.VIOLET;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        switch (doorColour)
        {
            case CombinationState.BLUE:
                animator.SetFloat("BlueDoor", 1f);
                break;
            case CombinationState.RED:
                animator.SetFloat("RedDoor", 1f);
                break;
            case CombinationState.YELLOW:
                animator.SetFloat("YellowDoor", 1f);
                break;
            case CombinationState.GREEN:
                animator.SetFloat("GreenDoor", 1f);
                break;
            case CombinationState.ORANGE:
                animator.SetFloat("OrangeDoor", 1f);
                break;
            case CombinationState.VIOLET:
                animator.SetFloat("VioletDoor", 1f);
                break;
        }    
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
        animator.SetBool("Open", true);
        gameObject.GetComponent<LevelLoader>().isOpen = true;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

}
