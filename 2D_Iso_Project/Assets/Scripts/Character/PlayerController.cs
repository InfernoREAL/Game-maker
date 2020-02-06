﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMove))]
public class PlayerController : BaseController<PlayerMove>
{
    static public PlayerController PlayerInstance { get; private set; }

    //****************** script
    CameraShaker m_shaker;


    //****************** Shape
    [Header("Shape Setting")]
    public GameObject[] ShapeList;
    public float shapeDuration = 0.1f;
    public float shapeInterval = 2.0f;
    public bool[] CanMeleeAttack = { true, false };

    int shapeIndex = 0;
    int ShapeNumber;
    Ability shapeAB;
    public void DisShapable() => shapeAB.Disable();
    public void EnShapable() => shapeAB.Enable();


    //****************** meleeAttack
    [Header("meleeAttack Setting")]
    public float attackPressDelay = 0.5f;
    
    const int Attack_Sequence = 4;
    bool attack_pressDown = false;
    protected Coroutine Cor_DisPress = null;


    protected override void OnAwake()
    {
        base.OnAwake();

        PlayerInstance = this;
        DontDestroyOnLoad(gameObject);
        
        m_shaker = Camera.main.GetComponent<CameraShaker>();

        shapeAB = gameObject.AddComponent<Ability>() as Ability;
        shapeAB.InitSetParams(shapeDuration, shapeInterval);
        ShapeNumber = ShapeList.Length;
    }

    void Start()
    {
        ShapeInit();
    }


    protected override void OnFixedUpdate()
    {
        ProcessInput();
        base.OnFixedUpdate();
    }

    void ProcessInput()
    {
        float horizontal = PlayerInput.Instance.Horizontal.Value;
        float Vertical = PlayerInput.Instance.Vertical.Value;
        Vector2 movement = new Vector2(horizontal, Vertical);
        if (PlayerInput.Instance.Jump.Down)
        {
            m_body.Dash();
        }
        m_body.Move(movement);

        if (PlayerInput.Instance.MeleeAttack.Down)
        {
            attack_pressDown = true;
            if (Cor_DisPress != null)
                StopCoroutine(Cor_DisPress);
            Cor_DisPress = StartCoroutine(DisPressAfterDelay());
        }

        if (PlayerInput.Instance.Interact.Down)
        {
            ShapeChange();
        }
    }

    IEnumerator DisPressAfterDelay()
    {
        yield return new WaitForSeconds(attackPressDelay);
        attack_pressDown = false;
        attackIndex = 0;
    }

    public void CheckForMeleeAttack()
    {
        if (attack_pressDown && damager.IsCanDamage && CanMeleeAttack[shapeIndex])
        {
            attackIndex = attackIndex % Attack_Sequence + 1;
            MeleeAttackStart();
        }
    }


    public void MeleeAttack()
    {
        Vector2 dir = m_body.FaceDir;
        //if (PlayerInput.Instance.inputType == PlayerInput.InputType.MouseAndKeyboard)
        //{
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    dir = (mousePos - (Vector2)m_body.Position).normalized;
        //}    

        MeleeAttack(dir);
        m_shaker.Shake();
        //FindObjectOfType<CameraShaker>().Shake();
    }



    public void ShapeInit()
    {
        m_animator = ShapeList[shapeIndex].GetComponent<Animator>();
        m_sprite = ShapeList[shapeIndex].GetComponent<SpriteRenderer>();
        SceneLinkedSMB<PlayerController>.Initialise(m_animator, this);
    }

    public void ShapeChange()
    {
        if (!shapeAB.Usable)
            return;

        shapeAB.Use();

        ShapeList[shapeIndex].SetActive(false);
        shapeIndex = (shapeIndex + 1)% ShapeNumber;
        ShapeList[shapeIndex].SetActive(true);
        ShapeInit();

        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
    }



    public bool GetHealth(int healthAmount)
    {
        return true;
        //TODO:
        //if (damageable.NeedHealth)
        //{
        //    damageable.GainHealth(healthAmount);
        //    return true;
        //}
        //else
        //    return false;
    }














    //public void SetChekpoint(Checkpoint checkpoint)
    //{
    //    m_LastCheckpoint = checkpoint;
    //}

    //public void SetDirection(Vector2 direction)
    //{

    //    //use the Run states by default
    //    string[] directionArray = null;

    //    //measure the magnitude of the input.
    //    if (direction.magnitude < .01f)
    //    {
    //        //if we are basically standing still, we'll use the Static states
    //        //we won't be able to calculate a direction if the user isn't pressing one, anyway!
    //        directionArray = staticDirections;
    //    }
    //    else
    //    {
    //        //we can calculate which direction we are going in
    //        //use DirectionToIndex to get the index of the slice from the direction vector
    //        //save the answer to lastDirIndex
    //        directionArray = runDirections;
    //        lastDirIndex = DirectionToIndex(direction, 8);
    //    }

    //    //tell the animator to play the requested state
    //    animator.Play(directionArray[lastDirIndex]);
    //}

    ////helper functions

    ////this function converts a Vector2 direction to an index to a slice around a circle
    ////this goes in a counter-clockwise direction.
    //public static int DirectionToIndex(Vector2 dir, int sliceCount)
    //{
    //    //get the normalized direction
    //    Vector2 normDir = dir.normalized;
    //    //calculate how many degrees one slice is
    //    float step = 360f / sliceCount;
    //    //calculate how many degress half a slice is.
    //    //we need this to offset the pie, so that the North (UP) slice is aligned in the center
    //    float halfstep = step / 2;
    //    //get the angle from -180 to 180 of the direction vector relative to the Up vector.
    //    //this will return the angle between dir and North.
    //    float angle = Vector2.SignedAngle(Vector2.up, normDir);
    //    //add the halfslice offset
    //    angle += halfstep;
    //    //if angle is negative, then let's make it positive by adding 360 to wrap it around.
    //    if (angle < 0)
    //    {
    //        angle += 360;
    //    }
    //    //calculate the amount of steps required to reach this angle
    //    float stepCount = angle / step;
    //    //round it, and we have the answer!
    //    return Mathf.FloorToInt(stepCount);
    //}

}
