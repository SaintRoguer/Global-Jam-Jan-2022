using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, REDCOLOUR, BLUECOLOUR, SOUL}
    public InteractionType type;
    public int ID;
    public RespawnSystem respawnSystem;


    private void Start()
    {
        respawnSystem = FindObjectOfType<RespawnSystem>();

        if (!respawnSystem.itemChecker.Any(door => door.objectID == ID))
            respawnSystem.doorChecker.Add(new CheckerClass(ID));
        else if (respawnSystem.doorChecker.Find(door => door.objectID == ID).interacted == true)
            Destroy(gameObject);
    }

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
            case InteractionType.SOUL:
                player.GetComponent<WinController>().GetSoul();
                Destroy(gameObject);
                break;
        }
    }
}
