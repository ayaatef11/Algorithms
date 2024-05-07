using System;
namespace file_Zipper
{
    public class Node
    {
        public char Data { get; set; }
        public long Frequency { get; set; }
        public string HuffmanCode { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node()
        {
            Left = null;
            Right = null;
            Frequency = 0;
            Data = '\0';
        }

        public Node(char data, long frequency)
        {
            Data = data;
            Frequency = frequency;
            Left = null;
            Right = null;
        }
    }
}
