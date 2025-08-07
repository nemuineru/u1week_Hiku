using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI LifeUI;
    public TextMeshProUGUI LuminusUI;

    public TextMeshProUGUI BuildingText;
    public TextMeshProUGUI BuildingLvText;
    
    public GameObject BuildingLVComp;
    public GameObject OnGameUI;
    public GameObject OnGameOverUI;
    public GameObject GetReadyUI;
    public TextMeshProUGUI CurrentDepthUI;
    public TextMeshProUGUI FinalResultUI;

    
    public TextMeshProUGUI EnemyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameSystem.self.gameState != GameSystem.GameState.gameover)
        {
            if(GameSystem.self.gameState == GameSystem.GameState.gameready)
            {
                GetReadyUI.SetActive(true);
            }
            else
            {
                GetReadyUI.SetActive(false);
            }

            OnGameUI.SetActive(true);
            if(GameSystem.Player.buildHolded != null)
            {
                BuildingLVComp.SetActive(true);
                Building bd = GameSystem.Player.buildHolded;
                BuildingText.text = bd.BuildName + "(LV " + bd.Level + ")";
                int Cost = (int)(bd.LevelCost * Mathf.Pow(bd.Level, 2));
                if(bd.Level < bd.LevelMax)
                {
                    BuildingLvText.text = "UPGRADE\n" + string.Format("({0})",Cost);
                    if(Cost > GameSystem.self.CurrentLuminus)
                    {
                        BuildingLvText.color = Color.red;
                    }
                    else
                    {
                        BuildingLvText.color = Color.blue;
                    }
                }
                else
                {
                    BuildingLvText.text = "LEVEL MAX";
                    BuildingLvText.color = Color.gray;
                }
            }
            else
            {
                BuildingLVComp.SetActive(false);
            }
            LifeUI.text = GameSystem.self.PlayerLife.ToString();
            LuminusUI.text = GameSystem.self.CurrentLuminus.ToString();
            EnemyText.text = GameSystem.self.CurrentEnemyCount.ToString();
            CurrentDepthUI.text = "Depth " + GameSystem.self.Depth.ToString();
        }
        else
        {
            OnGameUI.SetActive(false);
            OnGameOverUI.SetActive(true);
            FinalResultUI.text = "Your Score : " + GameSystem.self.Score.ToString();
        }
    }
}
