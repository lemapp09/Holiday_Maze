using System;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    private HolidayMazeInput input;

    private void OnEnable() {
        input = new HolidayMazeInput();
        input.QuitGame.Enable();
    }

    private void Update() {
        if (input.QuitGame.Quit.IsPressed()) {
            QuitGame();
        }
    }

    // Call this method to quit the game
    public void QuitGame()
    {
        // If we are running in a standalone build of the game
#if UNITY_STANDALONE
        // Quit the application
        Application.Quit();
#endif

        // If we are running in the editor
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    private void OnDisable()
    {
        input.QuitGame.Disable();
    }
}