﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //We do this explicitly here instead of in the enemy class, that allow to handle obstacle differently according to state
        // (e.g. look at the EnemyRunToTargetSMB that stop the pursuit if there is an obstacle) 
        float dist = m_MonoBehaviour.m_body.moveSpeed;
        if (m_MonoBehaviour.CheckForObstacle(dist))
        {
            //this will inverse the move vector, and UpdateFacing will then flip the sprite & forward vector as moveVector will be in the other direction
            m_MonoBehaviour.SetHorizontalSpeed(-dist);
            m_MonoBehaviour.UpdateFacing();
        }
        else
        {
            m_MonoBehaviour.SetHorizontalSpeed(dist);
        }

        m_MonoBehaviour.ScanForPlayer();
    }
}