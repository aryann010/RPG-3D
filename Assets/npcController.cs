using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcController : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform transformToFollow;
   public playerController playerController;
    private Animator animator;
    private Animator[] npcAnimator;
    //NavMesh Agent variable
    NavMeshAgent agent;
    void Start()
    {
      
        animator = playerController.GetComponentInChildren<Animator>();
        npcAnimator = gameObject.GetComponentsInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {

        //Follow the player
        agent.destination = transformToFollow.position;
        
        run();
    }
    private void run()
    {
        if (animator.GetBool("isRun") == true)
        {
           // activateLayer("Base Layer");
            npcAnimator[0].SetBool("isRun", true);
            npcAnimator[1].SetBool("isRun", true);
            npcAnimator[2].SetBool("isRun", true);
        }
        else
        {
            npcAnimator[0].SetBool("isRun", false);
            npcAnimator[1].SetBool("isRun", false);
            npcAnimator[2].SetBool("isRun", false);
        }
    }
 

}
