using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PauseHelper : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    // Start is called before the first frame update
    private void OnEnable()
    {
        director.Pause();
    }

    private void OnDisable()
    {
        director.Resume();
    }
}
