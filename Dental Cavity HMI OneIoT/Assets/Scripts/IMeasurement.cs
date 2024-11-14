using UnityEngine;

public interface IMeasurement
{
    enum CavityClass
    {
        Good,
        Bad,
        Worst
    }
    public string PatientName { get; set; }
    public float CavityDepth { get; set; }
    public CavityClass ClasifyCavityDepth(float depth);

}
