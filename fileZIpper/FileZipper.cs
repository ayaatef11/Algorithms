using System;
using System.Collections.Generic;
using System.IO;
namespace file_Zipper
{
    public class FileZipper
    {
        private PriorityQueue<Node> huffmanTree = new PriorityQueue<Node>();

        private string ReadFile(string path)
        {
            string text = File.ReadAllText(path);
            return text;
        }

        private Dictionary<char, long> GenerateFrequencies(string path)
        {
            Dictionary<char, long> frequencies = new Dictionary<char, long>();
            string text = ReadFile(path);
            foreach (char c in text)
            {
                if (frequencies.ContainsKey(c))
                {
                    frequencies[c]++;
                }
                else
                {
                    frequencies[c] = 1;
                }
            }
            return frequencies;
        }

        private Node CreateNode(char data, int frequency)
        {
            return new Node(data, frequency);
        }

        private void CreateMinHeap(string pathInput)
        {
            Dictionary<char, long> frequencies = GenerateFrequencies(pathInput);
            foreach (KeyValuePair<char, long> kvp in frequencies)
            {
                Node freqNode = CreateNode(kvp.Key, (int)kvp.Value);
                huffmanTree.Enqueue(freqNode);
            }
        }

        private void Copy(PriorityQueue<Node> copiedTree)
        {
            PriorityQueue<Node> arbitraryQueue = new PriorityQueue<Node>();
            while (huffmanTree.Count > 0)
            {
                arbitraryQueue.Enqueue(huffmanTree.Dequeue());
            }
            while (arbitraryQueue.Count > 0)
            {
                Node node = arbitraryQueue.Dequeue();
                huffmanTree.Enqueue(node);
                copiedTree.Enqueue(node);
            }
        }

        private void CreateTree()
        {
            PriorityQueue<Node> copiedTree = new PriorityQueue<Node>();
            Copy(copiedTree);
            while (copiedTree.Count > 1)
            {
                Node left = copiedTree.Dequeue();
                Node right = copiedTree.Dequeue();
                huffmanTree.Enqueue(CreateNode('\0', left.Frequency + right.Frequency));
            }
        }

        private void CreateCode(Node root, string code)
        {
            if (root == null) return;
            if (root.Left == null && root.Right == null) root.HuffmanCode = code;
            CreateCode(root.Left, code + "0");
            CreateCode(root.Right, code + "1");
        }

        private void SaveEncodedFile(string encodedFile, string outputFile)
        {
            File.WriteAllText(outputFile, encodedFile);
        }

        private Node GetTree(BinaryReader file)
        {
            int minHeapSize = file.ReadInt32();
            PriorityQueue<Node> minHeap = new PriorityQueue<Node>();
            for (int i = 0; i < minHeapSize; i++)
            {
                char data = file.ReadChar();
                int frequency = file.ReadInt32();
                minHeap.Enqueue(new Node(data, frequency));
            }
            while (minHeap.Count > 1)
            {
                Node left = minHeap.Dequeue();
                Node right = minHeap.Dequeue();
                Node internalNode = new Node('\0', left.Frequency + right.Frequency);
                minHeap.Enqueue(internalNode);
            }
            return minHeap.Count == 0 ? null : minHeap.Peek();
        }

        private void SaveDecodedFile(BinaryReader file, BinaryWriter outputFile, Node root)
        {
            Node current = root;
            while (file.BaseStream.Position < file.BaseStream.Length)
            {
                byte bit = file.ReadByte();
                if (bit == '0')
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
                if (current.Left == null && current.Right == null)
                {
                    outputFile.Write(current.Data);
                    current = root;
                }
            }
        }

        public string Compress(string inputFile)
        {
            CreateTree();
            CreateCode(huffmanTree.Peek(), "");
            string outputFile = Path.GetTempFileName();
            SaveEncodedFile(inputFile, outputFile);
            return outputFile;
        }

        public void Decompress(string outputFile, string inputFile)
        {
            using (BinaryReader br = new BinaryReader(new FileStream(outputFile, FileMode.Open)))
            using (BinaryWriter bw = new BinaryWriter(new FileStream(inputFile, FileMode.Create)))
            {
                Node root = GetTree(br);
                SaveDecodedFile(br, bw, root);
            }
        }
    }

    public class Node
    {
        public char Data { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public string HuffmanCode { get; set; }

        public Node(char data, int frequency)
        {
            Data = data;
            Frequency = frequency;
        }
    }

    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> data;

        public int Count { get { return data.Count; } }

        public PriorityQueue()
        {
            data = new List<T>();
        }

        public void Enqueue(T item)
        {
            data.Add(item);
            int ci = data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (data[ci].CompareTo(data[pi]) >= 0)
                    break;
                T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            int li = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[li];
            data.RemoveAt(li);

            --li;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > li)
                    break;
                int rc = ci + 1;
                if (rc <= li && data[rc].CompareTo(data[ci]) < 0)
                    ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0)
                    break;
                T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }
    }
}