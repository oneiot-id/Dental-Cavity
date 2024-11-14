using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using Task = System.Threading.Tasks.Task;

public class Connection : MonoBehaviour
{
    [SerializeField] private GameObject connectionWindow;
    [SerializeField] private bool isConnected = false;
    private SerialPort _serialPort;
    
    private bool _isRetryConnection = false;

    private void Start()
    {
        // RetryConnection();
        // StartCoroutine(ConnectionScheduler());
        StartCoroutine(OnDisconnected());
    }
    
    public bool IsRetryingConnection() => _isRetryConnection;
    private IEnumerator ConnectionScheduler()
    {
        while (true)
        {
            string[] allPort = SerialPort.GetPortNames();
            bool isPortAvailable = false;
            bool portIsNotNull = false;

            if (isConnected)
            {
                portIsNotNull = true;
                
                if (_serialPort != null)
                {
                    try
                    {
                        if(!_serialPort.IsOpen)
                            _serialPort.Open();
                        if (_serialPort.IsOpen)
                        {
                            _serialPort.Write("*");
                        }

                    }
                    catch (Exception e)
                    {
                        portIsNotNull = false;
                        isConnected = false;
                        isPortAvailable = false;
                        
                        Debug.Log("test");
                    }
                }
                
                foreach (var port in allPort)
                {
                    if (_serialPort == null) break;
                    if (_serialPort.PortName == port)
                    {
                        isPortAvailable = true;
                        break;
                    }
                }
                
                isConnected = isPortAvailable && portIsNotNull;
            }
            

            // isConnected = isPortAvailable && portIsNotNull;
            
            // Debug.Log("test");
            yield return new WaitForSeconds(0.01f);   
        }
    }
    private IEnumerator OnDisconnected()
    {
        while (true)
        {
            connectionWindow.SetActive(!isConnected);
            
            yield return new WaitForSeconds(0.5f);
        }
    }
    public async Task<Task> RetryConnection()
    {
        string[] portAvailable = SerialPort.GetPortNames();
        
        _isRetryConnection = true;
        
        StopCoroutine(ConnectionScheduler());
        
        foreach(var port in portAvailable)
        {
            Debug.Log($"Try connecting to {port} ");
            
            SerialPort newPort = new SerialPort(port, 115200);

            /*
             * 1 -> open the port
             * 2 -> ping the port with '*'
             * 3 -> set timeout
             * 4 -> if it is responding, then get the port, don't forget to close the port first before assigning it to property
             */ 
            try
            {
                newPort.Open();
                newPort.ReadTimeout = 2000;

                if (newPort.IsOpen)
                {
                    newPort.Write("*");
                    
                    await Task.Delay(2000); // this is because if we open the serial i don't know why the arduino keep restarting bruh
                    
                    var data = newPort.ReadExisting(); // whyyyy

                    if (data.Contains("connect"))
                    {
                        isConnected = true;
                        
                        newPort.Close();
                        _serialPort = new SerialPort(port, 115200);
                        
                        StartCoroutine(ConnectionScheduler());
                        
                        Debug.Log($"Connection established with {port}");
                        break;
                    }   
                    
                    newPort.Close();
                }
                
                
            }
            catch (Exception e)
            {
                newPort.Close();
                Console.WriteLine(e);
                continue;
                // throw;
            }
        }
        
        _isRetryConnection = false;
        // Debug.Log(portAvailable);
        Debug.Log("Reconnecting");
        //let's do this later we want to check the UI first
        return Task.CompletedTask;
    }

    public bool IsConnected() => isConnected;
    
    //Be right back! bruh forgot the water, okay let's do this first
    
    
}
