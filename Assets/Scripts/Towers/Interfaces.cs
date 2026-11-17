public interface IPathProgress { 
    float Progress { get; } // mayor = más adelantado
}          

public interface ISlowable { 
    void ApplySlow(float factor, float duration); 
}