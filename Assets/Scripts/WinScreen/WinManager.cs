using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the win condition of the game and displays the win screen.
/// </summary>
public class WinManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //iterate through LaneManger, check if Power Towers are alive
        //if all are dead, display win scene
        if(LaneManager.Instance.BoardInitialized)
        {
            CheckWinCondition();
        }
    }

    /// <summary>
    /// Checks the win condition by iterating through each lane and checking if the power towers are alive.
    /// If a player's power tower is destroyed, the win screen is displayed.
    /// </summary>
    private void CheckWinCondition()
    {
        bool p1HasPowerTower = false;
        bool p2HasPowerTower = false;
        for(int i = 0; i < LaneManager.Instance.laneCount; i++)
        {
            GameObject firstBuilding = LaneManager.Instance.GetTile(i, 0).GetComponent<TileScript>().GetBuilding();
            GameObject lastBuilding = LaneManager.Instance.GetTile(i, LaneManager.Instance.laneLength - 1).GetComponent<TileScript>().GetBuilding();
            if(firstBuilding != null && firstBuilding.tag == "Power Tower")
            {
                p1HasPowerTower = true;
            }
            if(lastBuilding != null && lastBuilding.tag == "Power Tower")
            {
                p2HasPowerTower = true;
            }
        }
        if(!p1HasPowerTower)
        {
            Debug.Log("Player 2 Wins");
            //display win screen
            SceneController.Instance.LoadScene("WinScene");
            WinState.Winner = "Player 2";
        }
        if(!p2HasPowerTower)
        {
            Debug.Log("Player 1 Wins");
            //display win screen
            SceneController.Instance.LoadScene("WinScene");
            WinState.Winner = "Player 1";
        }
    }
}
