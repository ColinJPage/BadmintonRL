using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionEffect : MonoBehaviour
{
    [SerializeField] private float outDuration = 0f; // How long after begining the Out animation that the scene may start loading
    public float OutDuration => outDuration;
    
    //[SerializeField] private bool startLoadingNextSceneImmediately = false;

    //public Event OutFinishedEvent = new Event();

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }
    public void Out()
    {
        
    }

    public void In()
    {
        animator.SetTrigger("inReady");
        Destroy(gameObject, 5f);
    }

    //public void OnOutFinished()
    //{
    //    OutFinishedEvent.Trigger();
    //}
}
