public interface IRandomPool<T>
{

    /// <summary>
    /// Gets a random item from the pool.
    /// </summary>
    T GetItem();
}