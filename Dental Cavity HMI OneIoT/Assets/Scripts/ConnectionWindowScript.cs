using System;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using ClickEvent = UnityEngine.UIElements.ClickEvent;
using Task = System.Threading.Tasks.Task;

public class ConnectionWindowScript : MonoBehaviour
{
    [SerializeField] private Connection connection;
    [SerializeField] private UIDocument uiDocument;
    
    private VisualElement _root;
    private VisualElement _button;
    private Label _reconnectingText;

    private Coroutine _connectionSchedulerCoroutine;
    private void OnEnable()
    {
        _root = uiDocument.rootVisualElement;
        _button = _root.Q(className:"reconnect-button");
        _reconnectingText = _root.Q<Label>(className: "reconnecting-text");
        
        _button.RegisterCallback<ClickEvent>(async (e) =>
        {
            _button.AddToClassList("hidden");
            _reconnectingText.RemoveFromClassList("hidden");
            
            Debug.Log("reconnect button");
            
            // Await RetryConnection to handle completion
            await connection.RetryConnection();

            // Update UI after the connection attempt
            if (connection.IsConnected())
            {
                _reconnectingText.AddToClassList("hidden");
            }
            else
            {
                _reconnectingText.AddToClassList("hidden");
                _button.RemoveFromClassList("hidden");
            }
            // _connectionSchedulerCoroutine = StartCoroutine(ConnectionScheduler());
        });
    }

    private void Start()
    {
        
    }
    

    private IEnumerator ConnectionScheduler()
    {
        while (true)
        {
            // if (connection.IsRetryingConnection())
            // {
            //     HandleOnRetryConnection();
            // }
            // else
            // {
            //     _reconnectingText.AddToClassList("hidden");
            //     _button.RemoveFromClassList("hidden");
            //
            //     if (_connectionSchedulerCoroutine != null)
            //     {
            //         StopCoroutine(_connectionSchedulerCoroutine);
            //     }
            //     
            //
            //     yield break;
            // }
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void HandleOnRetryConnection()
    {
        
    }

    private void HandleOnRetryDone()
    {
        
    }
    
}
