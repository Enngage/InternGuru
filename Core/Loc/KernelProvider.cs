using Ninject;

namespace Core.Loc
{
    public static class KernelProvider
    {
        /// <summary>
        /// Kernel with mapped classes
        /// </summary>
        public static IKernel Kernel { get; private set; }

        /// <summary>
        /// Sets static kernel with mapped classes. 
        /// Should be called on application start
        /// </summary>
        /// <param name="kernel">kernel to register</param>
        public static void SetKernel(IKernel kernel)
        {
            Kernel = kernel;
        }

        /// <summary>
        /// Gets an instance of the specified service
        /// </summary>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
