using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UI_BuildingBuy : MonoBehaviour
{
    public TextMeshProUGUI NewBuildingBuyUI;
    public GameObject ButtonUI;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Magnitude(transform.position - GameSystem.Player.transform.position) < 2.0f)
        {
            ButtonUI.SetActive(true);
            NewBuildingBuyUI.text = "Buy Building\n" + "(" + GameSystem.self.BuildingNextCost + ")";
            if(GameSystem.self.BuildingNextCost > GameSystem.self.CurrentLuminus)
            {
                NewBuildingBuyUI.color = Color.red;
            }
            else
            {
                NewBuildingBuyUI.color = Color.blue;
            }
        }
        else
        {
            ButtonUI.SetActive(false);
        }
    }
}
