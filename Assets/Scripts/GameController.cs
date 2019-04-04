﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject left_bar;
    public GameObject right_bar;
    public GameObject right_sub;
    public GameObject left_sub;
    public GameObject ready_text;
    public GameObject go_text;
    public Text WinText;
    public GameObject sByeBye;

    private HealthCounter health_right;
    private HealthCounter health_left;
    private bool _is_end = false;
    private bool _is_start = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health_right = right_sub.GetComponent<HealthCounter>();
        health_left = left_sub.GetComponent<HealthCounter>();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (_is_end || _is_start)
            return;
        UpdateHealthBar(left_bar, left_sub);
        UpdateHealthBar(right_bar, right_sub);

        if (health_right.health <= 0 || health_left.health <= 0)
        {
            GameEnd();
        }
    }

    void UpdateHealthBar(GameObject bar, GameObject sub)
    {
        HealthBar bar_script = bar.GetComponent<HealthBar>();
        HealthCounter health = sub.GetComponent<HealthCounter>();

        bar_script.SetSize((float)health.health / (float)health.maxHealth);

    }

    public void GameEnd()
    {
        _is_end = true;
        health_right = right_sub.GetComponent<HealthCounter>();
        health_left = left_sub.GetComponent<HealthCounter>();
        Global.instance.AllPlayersMovementEnable = false;

        if (health_right.health < health_left.health)
        {
            WinText.text = "Red Team Win!";
            StartCoroutine(DestroyLoser(right_sub, left_sub));
        }
        else
        {
            WinText.text = "Blue Team Win!";
            StartCoroutine(DestroyLoser(left_sub, right_sub));
        }
    }

    IEnumerator FixCamera(GameObject loser, GameObject winner)
    {
        Global.instance.GameEndCustomizeScreen = true;
        Vector3 init_pos = Camera.main.transform.position;
        Vector3 target_pos = loser.transform.position;
        target_pos.z = init_pos.z;
        float init_size = Camera.main.orthographicSize;
        float temp = 0f;
        while (temp < 1f)
        {
            temp += Time.deltaTime / 5f;
            Camera.main.transform.position = Vector3.Lerp(init_pos, target_pos, temp);
            Camera.main.orthographicSize = Mathf.Lerp(init_size, 6.4f, temp);
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        init_pos = Camera.main.transform.position;
        target_pos = winner.transform.position;
        target_pos.z = init_pos.z;
        temp = 0f;

        while (temp < 1f)
        {
            temp += Time.deltaTime / 3f;
            Camera.main.transform.position = Vector3.Lerp(init_pos, target_pos, temp);
            Camera.main.orthographicSize = Mathf.Lerp(6.4f, 7.5f, temp);
            yield return null;
        }
    }

    private IEnumerator DestroyLoser(GameObject loser, GameObject winner)
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(FixCamera(loser, winner));
        InputSystemManager.SetVibration(-1, 0.3f, 7f);
        yield return new WaitForSeconds(7f);
        InputSystemManager.SetVibration(-1, 0.9f, 2.7f);
        GameObject temp = Instantiate(sByeBye);
        temp.transform.position = loser.transform.position;
        yield return new WaitForSeconds(1.56f);
        Destroy(loser.transform.parent.gameObject);
        yield return new WaitForSeconds(1.44f);

        SoundManager.instance.PlaySound("win", winner.transform.position);
        StartCoroutine(ReloadScene());

    }
    
    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("Selection");
    }

    IEnumerator StartGame()
    {
        Global.instance.AllPlayersMovementEnable = false;
        _is_start = true;
        GameObject startText = Instantiate(ready_text, transform);
        startText.transform.localScale = new Vector3(10f, 10f);
        startText.transform.position = new Vector3(0f, 4.0f);

        while (startText.transform.localScale.x >= 1)
        {
            float old_value = startText.transform.localScale.x;
            startText.transform.localScale = new Vector3(old_value - 0.2f, old_value - 0.2f);
            yield return null;
            
        }
        Destroy(startText);

        GameObject goText = Instantiate(go_text, transform);
        goText.transform.localScale = new Vector3(10f, 10f);
        goText.transform.position = new Vector3(0f, 4.0f);

        while (goText.transform.localScale.x >= 1)
        {
            float old_value = goText.transform.localScale.x;
            goText.transform.localScale = new Vector3(old_value - 0.2f, old_value - 0.2f);
            yield return null;

        }

        Destroy(goText);
        Global.instance.AllPlayersMovementEnable = true;
        _is_start = false;


    }

}
