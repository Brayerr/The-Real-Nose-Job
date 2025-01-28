using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int branchAmountNeeded;
    [SerializeField] GameObject winPanel;

    bool CheckWinCondition(PlayerController controller)
    {
        if (controller.getBranchAmount() == branchAmountNeeded) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var roller))
            {
                if (CheckWinCondition(roller))
                {
                    //win condition
                    Debug.Log("you win!");
                    winPanel.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    //keep trying!
                    Debug.Log("you still need more branches!");
                }
            }
        }
    }

    public void MainMenuButton()
    {
        if(Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
