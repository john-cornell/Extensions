
namespace Avance
{
    public static class GenericExtensions
    {

        /// <summary>
        /// Applies a function to the given value - best used with static methods. e.g. "3".Apply(System.Int32.Parse) (returns 3)
        /// Source: http://extensionmethod.net/csharp/generic/apply-a-function
        /// </summary>        
        public static B Apply<A, B>(this A a, System.Func<A, B> f)
        {
            return f(a);
        }

    }
}
