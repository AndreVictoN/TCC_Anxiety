using TMPro;
using UnityEngine;

public class Estella : NPC
{
    protected override void BattleSettings()
    {
        if(sanity == null) sanity = GameObject.FindGameObjectWithTag("EsSanity").GetComponent<TextMeshProUGUI>();
        if(maxSanity == null) maxSanity = GameObject.FindGameObjectWithTag("EsMaxSanity").GetComponent<TextMeshProUGUI>();
        if(anxiety == null) anxiety = GameObject.FindGameObjectWithTag("EsAnxiety").GetComponent<TextMeshProUGUI>();
        if(maxAnxiety == null) maxAnxiety = GameObject.FindGameObjectWithTag("EsMaxAnxiety").GetComponent<TextMeshProUGUI>();

        base.BattleSettings();
    }
}
