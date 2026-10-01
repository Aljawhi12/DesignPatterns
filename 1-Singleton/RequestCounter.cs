namespace _1_Singleton
{
    public class RequestCounter : IRequestCounter
    {
        private int _count;

        public int GetCount()
        {
            return Volatile.Read(ref _count);
        }

        // Singleton is shared by all concurrent requests, so the increment must be atomic.
        public int Increment()
        {
            return Interlocked.Increment(ref _count);
        }
    }
}
