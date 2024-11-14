using UnityEngine;

public class Cavity
{
    public enum CavityType
    {
        Good,
        Bad, 
        Worst
    };
    
    public virtual CavityType ClasifyCavityDepth(float depth)
    {
        if(depth >= 0f && depth <= 1.5f) return CavityType.Good;
        else if (depth > 1.5f && depth <= 5f) return CavityType.Bad;
        else return CavityType.Worst;
    }
    
}
