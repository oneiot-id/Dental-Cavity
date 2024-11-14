using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Codice.Client.Common.EventTracking;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;
// using 


public class MeasurementScript : MonoBehaviour
{
    [SerializeField] private Connection connection;
    [SerializeField] private UIDocument uiDocs;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject sceneManager;
    
    private static bool _isLoadedBefore = false;
    
    private static Patient _patient;

    private VisualElement _visualElement;
    private VisualElement _saveData;
    private TextField _patientName;
    private Label _cavityDepth;
    private Label _cariesClass;

    private VisualElement _connectionLogo;
    private Label _connectionText;

    private void OnDisable()
    {
        _visualElement.visible = false;
    }
    
    
    private void OnEnable()
    {
        _visualElement = uiDocs.rootVisualElement;
        _patientName = _visualElement.Q<TextField>(className: "id-profile-name");
        _cavityDepth = _visualElement.Q<Label>(className: "depth-value");  
        _cariesClass = _visualElement.Q<Label>(className: "caries-value");
        _saveData = _visualElement.Q<VisualElement>(className: "save-button");
        
        _connectionLogo = _visualElement.Q<VisualElement>(className: "connection-logo");
        _connectionText = _visualElement.Q<Label>(className: "connection-status");

        _patientName.RegisterValueChangedCallback((e) => _patient.PatientName = _patientName.value);
        _saveData.RegisterCallback<ClickEvent>((e) => SaveData());

        if (_isLoadedBefore)
        {
            _patientName.value = _patient.PatientName;
            Debug.Log(_patient.PatientName);

        }
        else
        {
            _patient = new Patient();
        }
        
        _isLoadedBefore = true;
        
    }
    private void Start()
    {
        StartCoroutine(CheckConnection());
    }
    private IEnumerator CheckConnection()
    {
        while (true)
        {
            if(connection.IsConnected())
                OnConnected();
            else
                OnDisconnected();

            // Debug.Log(connection.IsConnected());
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnConnected()
    {
        _connectionLogo.AddToClassList("connected");
        _connectionLogo.RemoveFromClassList("disconnected");
        _connectionText.AddToClassList("connected");
        _connectionText.RemoveFromClassList("disconnected");
        
        _connectionText.text = "Connected";
    }

    private void OnDisconnected()
    {
        _connectionLogo.RemoveFromClassList("connected");
        _connectionLogo.AddToClassList("disconnected");
        _connectionText.RemoveFromClassList("connected");
        _connectionText.AddToClassList("disconnected");
        
        _connectionText.text = "Not Connected";
    }
    
    private void SaveData()
    {
        var path = Application.dataPath + $"/Data Log/{_patient.PatientName}.json";
        var data = JsonConvert.SerializeObject(_patient);

        using var sw = File.CreateText(path);
        {
            sw.Write(data);
        }

    }

    private void LoadConfigurationFile()
    {
        
    }
    
    
}
