using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MainColours { RED, BLUE, YELLOW }
public enum CombinationState { RED, BLUE, YELLOW, VIOLET, ORANGE, GREEN }

public class ColourSystem : MonoBehaviour
{

    public MainColours mainState;
    public MainColours secondaryState;
    public CombinationState combinationState;

    public Queue<MainColours> playerColourWheel = new Queue<MainColours>();
    public Queue<MainColours> weaponColourWheel = new Queue<MainColours>();
    // Start is called before the first frame update
    void Start()
    {
        mainState = MainColours.YELLOW;
        secondaryState = MainColours.YELLOW;
        combinationState = CombinationState.YELLOW;

        //Change to only add yellow, because this is a test with all colours.
        playerColourWheel.Enqueue(MainColours.RED);
        playerColourWheel.Enqueue(MainColours.BLUE);
        playerColourWheel.Enqueue(MainColours.YELLOW);
        weaponColourWheel.Enqueue(MainColours.YELLOW);
        weaponColourWheel.Enqueue(MainColours.RED);
        weaponColourWheel.Enqueue(MainColours.BLUE);
    }



    public MainColours ChangeMainColour()
    {
        mainState = playerColourWheel.Dequeue();
        playerColourWheel.Enqueue(mainState);
        ChangeColour();
        return mainState;
    }

    public MainColours ChangeSecondaryColour()
    {
        secondaryState = weaponColourWheel.Dequeue();
        weaponColourWheel.Enqueue(secondaryState);
        ChangeColour();
        return secondaryState;
    }

    void ChangeColour()
    {
        switch (mainState)
        {
            case MainColours.YELLOW:
                switch (secondaryState)
                {
                    case MainColours.YELLOW:
                        combinationState = CombinationState.YELLOW;
                        break;
                    case MainColours.RED:
                        combinationState = CombinationState.ORANGE;
                        break;
                    case MainColours.BLUE:
                        combinationState = CombinationState.GREEN;
                        break;
                    default:
                        Console.WriteLine("Invalid secondary colour");
                        break;
                }
                break;
            case MainColours.BLUE:
                switch (secondaryState)
                {
                    case MainColours.YELLOW:
                        combinationState = CombinationState.GREEN;
                        break;
                    case MainColours.RED:
                        combinationState = CombinationState.VIOLET;
                        break;
                    case MainColours.BLUE:
                        combinationState = CombinationState.BLUE;
                        break;
                    default:
                        Console.WriteLine("Invalid secondary colour");
                        break;
                }
                break;
            case MainColours.RED:
                switch (secondaryState)
                {
                    case MainColours.YELLOW:
                        combinationState = CombinationState.ORANGE;
                        break;
                    case MainColours.RED:
                        combinationState = CombinationState.RED;
                        break;
                    case MainColours.BLUE:
                        combinationState = CombinationState.VIOLET;
                        break;
                    default:
                        Console.WriteLine("Invalid secondary colour");
                        break;
                }
                break;
            default:
                Console.WriteLine("Invalid main colour");
                break;
        }
    }

}
