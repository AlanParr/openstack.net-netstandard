using OpenStack.Serialization;

namespace OpenStack.Compute.v2_1.Serialization
{
    /// <summary />
    public class AddressType<T> : StringEnumeration
        where T : AddressType<T>, new()
    {
        /// <summary />
        public static readonly T Fixed = new T {DisplayName = "fixed"};

        /// <summary />
        public static readonly T Floating = new T {DisplayName = "floating"};
    }
}