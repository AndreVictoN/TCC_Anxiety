using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ezequiel : NPC
{
    public List<String> prototypeDialogue = new();
    private bool _inPrototype;

    void Awake()
    {
        BasicSettings();

        if(SceneManager.GetActiveScene().name == "PrototypeScene"){dialogue = prototypeDialogue; _inPrototype = true;}
    }

    protected override void CheckCharacter(int i)
    {
        GameObject playerImage = GameObject.FindGameObjectWithTag("Player_Image");
        GameObject npcImage = GameObject.FindGameObjectWithTag("NPC_Image");
        
        if(_inPrototype)
        {
            if(i == 2){
                dialogueText.alignment = TextAlignmentOptions.Right;
                playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 1);
                npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 0.75f);
            }else{
                dialogueText.alignment = TextAlignmentOptions.Left;
                playerImage.GetComponent<Image>().color = new Vector4(playerImage.GetComponent<Image>().color.r, playerImage.GetComponent<Image>().color.g, playerImage.GetComponent<Image>().color.b, 0.75f);
                npcImage.GetComponent<Image>().color = new Vector4(npcImage.GetComponent<Image>().color.r, npcImage.GetComponent<Image>().color.g, npcImage.GetComponent<Image>().color.b, 1);
            }
        }
    }

    protected override Vector3 getPosition()
    {
        return new Vector3(-5.3f, 2.5f, 0);
    }

    protected override Color CheckColorAspectByNPC()
    {
        Color colorToFade = spriteRenderer.color;

        colorToFade.b -= 0.7f;

        return colorToFade;
    }
}
