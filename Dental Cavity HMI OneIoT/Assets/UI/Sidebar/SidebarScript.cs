using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class SidebarScript : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] private Connection connection;
    [SerializeField] private List<GameObject> display;
    
    private VisualElement _root;
    private bool _lastConnectionStatus = false;
    private List<VisualElement> _sidebarItem = new List<VisualElement>();
    private int _selectedIndex;
    
    [CanBeNull] private const string SidebarActive = "sidebar-active";
    [CanBeNull] private const string SidebarDisconnected = "sidebar-disconnect";

    private void Start()
    {
        _lastConnectionStatus = !connection.IsConnected();
        StartCoroutine(CheckConnection());
    }

    private IEnumerator CheckConnection()
    {
        while (true)
        {
            bool isConnected = connection.IsConnected();
            
            if (isConnected)
                OnConnectionConnected();
            else
                OnConnectionDisconnected();
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnConnectionDisconnected()
    {
        _sidebarItem[_selectedIndex].RemoveFromClassList(SidebarActive);
        _sidebarItem[_selectedIndex].AddToClassList(SidebarDisconnected);
    }
    private void OnConnectionConnected()
    {
        // if(_sidebarItem[selectedIndex].has)
        _sidebarItem[_selectedIndex].RemoveFromClassList(SidebarDisconnected);
        _sidebarItem[_selectedIndex].AddToClassList(SidebarActive);
    }
    private void OnEnable()
    {
        _root = uiDoc.rootVisualElement;
        _sidebarItem = _root.Query<VisualElement>(className: "sidebar-item").ToList();

        for (var i = 0; i < _sidebarItem.Count; i++)
        {
            var selector = i;
            _sidebarItem[i].RegisterCallback<ClickEvent>((e ) => OnButtonClicked(e, selector));
        }
    }

    private void OnButtonClicked(ClickEvent e, int selectIndex)
    {
        //check if the connection is not established, if so just return preventing user to change the screen
        if (!connection.IsConnected()) return;
        
        _selectedIndex = selectIndex;
        
        for (var i = 0; i < _sidebarItem.Count; i++)
        {
            display[i].SetActive(i == selectIndex);
            
            if (i != selectIndex)
            {
                _sidebarItem[i].RemoveFromClassList(SidebarActive);
            }
            else
            {
                _sidebarItem[i].AddToClassList(SidebarActive);
            }
            
        }
    }
}
