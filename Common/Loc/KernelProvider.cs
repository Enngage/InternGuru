using Ninject;

namespace Common.Loc
{
    public static class KernelProvider
    {
        private static IKernel _kernel;

        /// <summary>
        /// Kernel with mapped classes
        /// </summary>
        public static IKernel Kernel
        {
            get
            {
                return _kernel;
            }
        }

        /// <summary>
        /// Sets static kernel with mapped classes. 
        /// Should be called on application start
        /// </summary>
        /// <param name="kernel">kernel to register</param>
        public static void SetKernel(IKernel kernel)
        {
            _kernel = kernel;
        }
    }
}
