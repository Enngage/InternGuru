using Ninject;

namespace Common.Loc
{
    public static class KernelProvider
    {
        private static IKernel _kernel;

        public static IKernel Kernel
        {
            get
            {
                return _kernel;
            }
        }

        public static void SetKernel(IKernel kernel)
        {
            _kernel = kernel;
        }
    }
}
