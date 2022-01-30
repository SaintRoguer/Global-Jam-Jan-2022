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
        playerColourWheel.Enqueue(MainColours.YELLOW);
        weaponColourWheel.Enqueue(MainColours.YELLOW);

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

    public void AddColour(string colour)
    {
        switch (colour)
        {
            case "REDCOLOUR":
                playerColourWheel.Enqueue(MainColours.RED);
                weaponColourWheel.Enqueue(MainColours.RED);
                break;
            case "BLUECOLOUR":
                playerColourWheel.Enqueue(MainColours.BLUE);
                weaponColourWheel.Enqueue(MainColours.BLUE);
                break;
        }
        FixColourChange();
    }

    private void FixColourChange()
    {
        MainColours mainAux = mainState;
        MainColours secondaryAux = secondaryState;
        ChangeMainColour();
        ChangeSecondaryColour();
        while (mainState != mainAux)
        {
            ChangeMainColour();
        }
        while (secondaryAux != secondaryState)
        {
            ChangeSecondaryColour();
        }
    }

    void ChangeColour()
    {
        switch (mainState)
        {
            case MainColours.YELLOW:
                ChangeMainYellow();
                break;
            case MainColours.BLUE:
                ChangeMainBlue();
               
                break;
            case MainColours.RED:
                ChangeMainRed();
                break;
        }
    }

    private void ChangeMainYellow()
    {
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
        }
    }

    private void ChangeMainBlue()
    {
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
        }
    }

    private void ChangeMainRed()
    {
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
        }
    }

    public void ForceChangeToYellow()
    {
        while(mainState != MainColours.YELLOW)
        {
            ChangeMainColour();
        }
        while (secondaryState != MainColours.YELLOW)
        {
            ChangeSecondaryColour();
        }
    }

}
