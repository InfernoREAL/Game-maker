﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSMB : SceneLinkedSMB<PlayerController>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.ResetAttack();
        m_MonoBehaviour.DisMovable();
    }
    
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.MeleeAttack();
        m_MonoBehaviour.AttackMove();
        //m_MonoBehaviour.SetHorizontalMovement(m_MonoBehaviour.meleeAttackDashSpeed * m_MonoBehaviour.GetFacing());
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.GroundedHorizontalMovement(false);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_MonoBehaviour.DisableMeleeAttack();
        m_MonoBehaviour.EnMovable();
    }
}