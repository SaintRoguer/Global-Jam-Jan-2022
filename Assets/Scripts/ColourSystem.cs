using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MainColours { RED, BLUE, YELLOW }
public enum CombinationState { RED, BLUE, YELLOW, VIOLET, GREEN, ORANGE}

public class ColourSystem : MonoBehaviour
{

    public MainColours mainState;
    public MainColours secondaryState;
    public CombinationState combinationState;
    // Start is called before the first frame update
    void Start()
    {
        mainState = MainColours.YELLOW;
        secondaryState = MainColours.YELLOW;
        combinationState = CombinationState.YELLOW;
    }

    void ChangeMainColour( MainColours colour)
    {
        mainState = colour;
        ChangeColour();
    }

    void ChangeSecondaryColour( MainColours colour)
    {
        secondaryState = colour;
        ChangeColour();
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
