using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private TMP_Text giftsCollected;
    [SerializeField] private GameObject gameWon;
    [SerializeField] private GameObject[] levelIcons;
    
    private int numberGifts;

    public void FoundGift() {
        numberGifts++;
        giftsCollected.text = "GIFTS COLLECTED: " + numberGifts;
    }

    public void RemoveGifts() {
        numberGifts -= 10;
        if (numberGifts < 0) numberGifts = 0;
        giftsCollected.text = "GIFTS COLLECTED: " + numberGifts; }

    public void updateLevel(int level)
    {
        if (level <= levelIcons.Length) {
            for (int i = 0; i < levelIcons.Length; i++) {
                // Check if the index is less than or equal to the level
                if (i <= level - 1) {
                    levelIcons[i].SetActive(true);
                } else {
                    levelIcons[i].SetActive(false);
                }
            }
        }
    }

    public void GameWon() {
        gameWon.SetActive(true);
    }
}
