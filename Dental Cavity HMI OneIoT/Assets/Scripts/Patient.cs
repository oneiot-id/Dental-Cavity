using JetBrains.Annotations;
using UnityEngine;

public class Patient
{
    public string PatientName = "";
    public float CavityDepth;
    public int CavityClass;
    //
    public Patient()
    {
        
    }
        
    public Patient(string patientName, float cavityDepth, Cavity.CavityType cavityClass)
    {
        patientName = patientName;
        cavityDepth = cavityDepth;
        cavityClass = cavityClass;
    }

    
}
