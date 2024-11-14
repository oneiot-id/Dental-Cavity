using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CalibrationScript : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private GameObject configuationManager;
    
    private VisualElement _rootElement;

    private VisualElement _stepsContainer;
    private VisualElement _visual;
    private VisualElement _connectionStatus;
    private VisualElement _nextButton;
    private VisualElement _nav4;
    private VisualElement _retryButton;
    private VisualElement _saveButton;

    private const string StepKey = "step";
    private const string VisualKey = "visual";
    private const string CalibrationNavigation = "calibration-navigation";
    
    private int _calibrationIndex = 1;
    

    private void OnEnable()
    {
        _rootElement = uiDocument.rootVisualElement;    
        _stepsContainer = _rootElement.Q(className: "calibration-step");
        _visual = _rootElement.Q(className: "visual");
        _connectionStatus = _rootElement.Q(className: "connection");
        _nav4 = _rootElement.Q(className: "calibration-navigation-4"); 
        _nextButton = _rootElement.Q(className: "calibration-navigation");
        _retryButton = _rootElement.Q(className: "retry-button");
        _saveButton = _rootElement.Q(className: "save-button");
        
        _nextButton.RegisterCallback<ClickEvent>((e) => StepChanged(e, _calibrationIndex + 1));
        _retryButton.RegisterCallback<ClickEvent>((e) => RetryCalibration());
        _saveButton.RegisterCallback<ClickEvent>((e) => SaveToConfigFile());

    }
    private void OnDisable()
    {
        _rootElement.visible = false;
    }
    private void ResetIndex() => _calibrationIndex = 1;

    private void RetryCalibration()
    {
        _stepsContainer.RemoveFromClassList(GetFromKey(StepKey, 4));
        _visual.RemoveFromClassList(GetFromKey(VisualKey, 4));
        
        _calibrationIndex = 1;
        _stepsContainer.AddToClassList(GetFromKey(StepKey, _calibrationIndex));
        _visual.AddToClassList(GetFromKey(VisualKey,  _calibrationIndex));
        
        _nextButton.AddToClassList("visible");
        _nextButton.RemoveFromClassList("hidden");
        
        _nav4.visible = false;
    }
    
    private void StepChanged(ClickEvent e, int selectedIndex)
    {
        if (selectedIndex == 4)
        {
            _nextButton.AddToClassList("hidden");
            _nav4.visible = true;
        }
        
        Debug.Log(selectedIndex);
        
        switch (_calibrationIndex)
        {
            case 0: break;
            case 1: break;
            case 2: break;
            case 3: break;
        }
     
        DisplayNewVisual(selectedIndex);
        
        _calibrationIndex = selectedIndex;
    }

    private string GetFromKey(string key, int calIndex) => $"{key}-{calIndex}";
    
    private void DisplayNewVisual(int index)
    {
        //Delete the old visual
        _stepsContainer.RemoveFromClassList(GetFromKey("step", _calibrationIndex));
        _visual.RemoveFromClassList(GetFromKey("visual", _calibrationIndex));
        
        //Get the new visual
        _stepsContainer.AddToClassList(GetFromKey("step", index));
        _visual.AddToClassList(GetFromKey("visual", index));
    }

    private void SaveToConfigFile()
    {
        
    }
}
