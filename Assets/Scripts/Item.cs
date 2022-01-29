using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, REDCOLOUR, BLUECOLOUR}
    public InteractionType type;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 9;
    }

    public void Interact(PlayerController player)
    {
        switch (type){
            case InteractionType.REDCOLOUR:
                Debug.Log("REDCOLOUR");
                player.GetComponent<ColourSystem>().AddColour("REDCOLOUR");
                Destroy(gameObject);
                break;
            case InteractionType.BLUECOLOUR:
                Debug.Log("BLUECOLOUR");
                player.GetComponent<ColourSystem>().AddColour("BLUECOLOUR");
                Destroy(gameObject);
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }
    }
}
