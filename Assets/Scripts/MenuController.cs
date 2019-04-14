﻿using System.Collections;
using System.Collections.Generic;
using BeautifulTransitions.Scripts.Transitions.Components;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MenuUI;
    public static MenuController instance;


    private GameObject _box;
    private GameObject _option;
    private List<GameObject> _options;
    private int _curr_pos = 0;

    private int frame = 0;
    private bool isCD;
    // private bool isPopup = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        // DontDestroyOnLoad(this.gameObject);
        // DontDestroyOnLoad(MenuUI.transform.parent.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        _box = MenuUI.transform.Find("Box").gameObject;
        _option = MenuUI.transform.Find("Option").gameObject;
        _options = new List<GameObject>();

        foreach (Transform child in _option.transform)
        {
            _options.Add(child.gameObject);
        }

        _box.transform.position = _options[_curr_pos].transform.position;
        MenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isPopup)
        {
            
            if (MenuUI.GetComponent<RectTransform>().position.magnitude > 2f)
            {

                MenuUI.GetComponent<RectTransform>().position = Vector3.Lerp(Vector3.zero, new Vector3(530f, 285f, 0f), );
                //MenuUI.GetComponent<RectTransform>().localScale += (new Vector3(1f, 1f, 1f) - MenuUI.GetComponent<RectTransform>().localScale).normalized * 10f * Time.deltaTime;

            }
            else
            {
                isPopup = false;
                Time.timeScale = 0f;
            }
        }*/

        // Stop the scene
        if (InputSystemManager.GetMenuButton(0))
        {

            if (System.Math.Abs(Time.timeScale) > 0.95f)
            {
                MenuUI.SetActive(true);
                Time.timeScale = 0f;
                // MenuUI.GetComponent<RectTransform>().position = new Vector3(530f, 285f, 0f);
                // MenuUI.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0f);
                // isPopup = true;
            }
            else
            {
                MenuUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }


        if (InputSystemManager.GetAction2(0) && MenuUI.activeInHierarchy)
        {
            Time.timeScale = 1f;
            MenuUI.SetActive(false);
            if (_curr_pos == 0)
            {
                TransitionManager.Instance.TransitionOutAndLoadScene("Main");
            } else if (_curr_pos == 2)
            {
                Application.Quit();
            }
            else
            {
                _curr_pos = 0;
            }

        }

        if (!isCD)
        {
            if (InputSystemManager.GetLeftSVertical(0) < -0.5f)
            {

                if (_curr_pos < _options.Count - 1)
                {
                    _curr_pos++;
                    isCD = true;
                }
            }
            else if (InputSystemManager.GetLeftSVertical(0) > 0.5f)
            {
                isCD = true;
                if (_curr_pos > 0)
                {
                    _curr_pos--;
                }
            }
            _box.transform.position = _options[_curr_pos].transform.position;
        }
        else
        {
            frame++;
            if (frame == 25)
            {
                isCD = false;
                frame = 0;
            }
        }


    }

    private void OnApplicationQuit()
    {
        Time.timeScale = 1f;
    }


}
