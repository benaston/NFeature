namespace NFeature
{
    public interface ITenancyContext<out TTenant>
    where TTenant : struct 
    { TTenant CurrentTenant { get; } }
}