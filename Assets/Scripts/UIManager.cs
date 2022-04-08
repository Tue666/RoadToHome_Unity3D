using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class View
{
    public string name;
    public GameObject viewObject;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public View[] views;
    Dictionary<string, GameObject> viewsDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitDictionary();
    }

    void InitDictionary()
    {
        foreach (View view in views)
        {
            if (view != null)
                viewsDictionary.Add(view.name, view.viewObject);
        }
    }

    public GameObject GetView(string viewName)
    {
        if (Helpers.ContainsKeyButValueNotNull(viewsDictionary, viewName))
            return viewsDictionary[viewName];
        return null;
    }

    public bool isShowing(string viewName)
    {
        if (Helpers.ContainsKeyButValueNotNull(viewsDictionary, viewName))
        {
            GameObject view = viewsDictionary[viewName];
            return view.activeSelf;
        }
        return false;
    }

    public void ShowView(string viewName)
    {
        if (Helpers.ContainsKeyButValueNotNull(viewsDictionary, viewName))
        {
            GameObject view = viewsDictionary[viewName];
            view.SetActive(!view.activeSelf);
        }
    }

    public void HideView(string viewName)
    {
        if (Helpers.ContainsKeyButValueNotNull(viewsDictionary, viewName))
        {
            GameObject view = viewsDictionary[viewName];
            view.SetActive(false);
        }
    }
}
