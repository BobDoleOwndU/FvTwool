namespace FvTwool
{
    public static class Utils
    {
        public static void ResizeArray<T>(ref T[] array, int newSize) where T : new()
        {
            if (array != null)
            {
                int oldSize = array.Length;

                T[] tempArray = new T[newSize];

                for (int i = 0; i < newSize; i++)
                {
                    if (i < oldSize)
                        tempArray[i] = array[i];
                    else
                        tempArray[i] = new T();
                } //for

                array = tempArray;
            } //if
            else
                array = new T[newSize];
        } //Resize

        public static void ResizeStringArray(ref string[] array, int newSize)
        {
            if (array != null)
            {
                int oldSize = array.Length;

                string[] tempArray = new string[newSize];

                for (int i = 0; i < newSize; i++)
                {
                    if (i < oldSize)
                        tempArray[i] = array[i];
                    else
                        tempArray[i] = "";
                } //for

                array = tempArray;
            } //if
            else
                array = new string[newSize];
        } //Resize
    } //class
} //namespace
