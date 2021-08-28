using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition02_B : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //Disable every collider that deals damage to enemys
       PlayerController.instance.gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
       PlayerController.instance.gameObject.GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
       PlayerController.instance.gameObject.GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);

       PlayerController.instance.canReceiveInput = true;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if ( PlayerController.instance.inputReceived )
       {
            animator.SetTrigger("Attack03");
            PlayerController.instance.InputManager();
            PlayerController.instance.inputReceived = false;
            PlayerController.instance.canReceiveInput = false;

            PlayerController.instance.gameObject.GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
