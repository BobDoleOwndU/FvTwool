using System.IO;

namespace FvTwool
{
    public static class ExtensionMethods
    {
        public static void WriteZeroes(this BinaryWriter writer, int count)
        {
            byte[] array = new byte[count];

            writer.Write(array);
        } //WriteZeroes
    } //class
} //namespace
