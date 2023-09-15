namespace Utils
{
    public interface IStepsController<out T>
    {
        /// <summary>
        /// The time between this step and another (either the next or the previous
        /// depending on which method was called last)
        /// </summary>
        float DeltaTime { get; }
        
        
        /// <summary>
        /// Returns the next step of the simulation
        /// </summary>
        T GetNext();
        
        /// <summary>
        /// Returns the previous step of the simulation
        /// </summary>
        T GetPrev();
    }
}