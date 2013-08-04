
namespace Grain.Serialization
{
    public static class BinarySerializer
    {
        /// <summary>
        /// Converts the object to a binary array by serializing.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The object to convert.</param>
        /// <returns>A serialized binary array of the object.</returns>
        public static byte[] ToBinary<T>(this T item)
        {
            return item.ToBson();
        }

        /// <summary>
        /// Reconstruct an object from a Binary JSON string (BSON)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bson"></param>
        /// <returns></returns>
        public static T FromBinary<T>(this byte[] bson)
        {
            return bson.FromBson<T>();
        }
    }
}
