using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    PlayableDirector director;
    public string Scene1;
        public string Scene2;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        director = GetComponent<PlayableDirector>();
    }

    public void StartTimeline() {
        director.Play();
    }

    public void PauseTimeline() {
        director.Pause();
    }

    public void ResumeTimeline() {
        if (director.state == PlayState.Paused) {
            director.Resume();
        }
    }

    public void loadScene1() {
        SceneManager.LoadScene(Scene1);
    }

    public void loadScene2() {
        // SceneManager.LoadScene(Scene2);
        gameManager.SetGameState(GameState.MainMenu, Scene2);
    }
}
