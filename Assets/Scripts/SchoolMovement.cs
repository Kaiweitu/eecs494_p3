﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolMovement : MonoBehaviour
{
   Vector3[] PresetPosition = { new Vector3(-14, 3, 0), new Vector3(12, 3, 0), 
                                        new Vector3(-14, -3, 0), new Vector3(12, -3, 0)};
    public int StartPos;
    public int DestPos;
    public float Speed = 2.0f;
    public float WaitTime;
    public GameObject[] fishes;

    int fishCount = 7;
    Animator[] fishAnimators;
    Rigidbody2D rb;
    bool traveling;
    bool inTravelRountine;
    bool inWaitRoutine;

    // Start is called before the first frame update
    void Start()
    {
        fishAnimators = new Animator[fishes.Length];
        for (int i = 0; i < transform.childCount; ++i)
        {
            fishAnimators[i] = transform.GetChild(i).GetComponent<Animator>();
        }
        rb = GetComponent<Rigidbody2D>();

        StartPos = 0;
        DestPos = 1;
        travel();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop when reach destination
        if (Mathf.Abs(transform.position.x - PresetPosition[DestPos].x) < 0.1f && !inWaitRoutine)
        {
            for (int i = 0; i < fishes.Length; ++i)
            {
                // if the fish is still active
                if (transform.GetChild(i))
                {
                    fishAnimators[i].speed = 0;
                    fishAnimators[i].SetBool("moving", false);
                }
               
            }
            rb.velocity = Vector2.zero;
            StartCoroutine(wait());
        }
        else if (!inWaitRoutine)
        {
            //rb.AddForce((PresetPosition[DestPos] - PresetPosition[StartPos]).normalized * Speed);
            rb.velocity = (PresetPosition[DestPos] - PresetPosition[StartPos]).normalized * Speed;
        }

    }

    void travel() {
        traveling = true;
        for (int i = 0; i < fishes.Length; ++i)
        {
            if (transform.GetChild(i))
            {
                fishAnimators[i].speed = 1;
                fishAnimators[i].SetBool("moving", true);
            }
        }
        //rb.AddForce((PresetPosition[DestPos] - PresetPosition[StartPos]).normalized * Speed);
        rb.velocity = (PresetPosition[DestPos] - PresetPosition[StartPos]).normalized * Speed;
    }

    // Adjust fish count and deactive the killed fish
    public void KillFish()
    {
        fishCount--;
        //fishes[fishCount].SetActive(false);
        if (fishCount == 0)
            StartCoroutine(wait());
    }


    // Wait for a period of time and reset school
    IEnumerator wait()
    {
        inWaitRoutine = true;
        yield return new WaitForSeconds(WaitTime);
        StartPos = new System.Random().Next(0, 4);
        fishCount = fishes.Length;
        for (int i = 0; i < fishes.Length; ++i)
        {
            fishes[i].SetActive(true);
        }
        // rotate the school to the correct direction
        if ((StartPos == 1 || StartPos == 3) && transform.localScale.x != -1.0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if ((StartPos == 0 || StartPos == 2) && transform.localScale.x != 1.0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        DestPos = getDestination(StartPos);
        transform.position = PresetPosition[StartPos];
        travel();
        inWaitRoutine = false;
    }

    // Given the start position of school, return the corresponding destination
    int getDestination (int start)
    {
        switch (start)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
            default:
                Debug.LogWarning("WARNING: No such school start position available");
                return 4;
        }
    }




    // ------ using transform -------
    //if ((transform.position - PausePosition).magnitude < 0.3f && !inStopRountine)
    //    StartCoroutine(pause());

    //// moving to target position
    //if (approaching)
    //{
    //    for (int i = 0; i < fishAnimators.Length; ++i)
    //    {
    //        fishAnimators[i].speed = 1;
    //        fishAnimators[i].SetBool("moving", true);
    //    }
    //    transform.position = Vector3.Lerp(transform.position, PausePosition, Time.deltaTime * speed);
    //}
    //// leaving
    //else
    //{
    //    for (int i = 0; i < fishAnimators.Length; ++i)
    //    {
    //        fishAnimators[i].speed = 1;
    //        fishAnimators[i].SetBool("moving", true);
    //    }
    //    transform.position = Vector3.Lerp(transform.position, LeavePosition, Time.deltaTime * speed);

    //}


}
