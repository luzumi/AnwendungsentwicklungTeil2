using System;

namespace Container
{
    public class Queue
    {
        int elementCount = 0;
        private int minimumSize = 0;

        public int[] Elements { get; set; }

        int pushIndex = 0;
        int popIndex = 0;

        public int Capacity
        {
            get => Elements.Length;
            set
            {
                minimumSize = value;
                resize(value);
            }
        }

        private void resize(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException();

            if (value < elementCount) throw new InsufficientMemoryException();

            if (value < minimumSize) minimumSize = value;

            int[] biggerArray = new int[value];

            int readIndex = popIndex;
            int writeIndex = 0;

            for (int counter = 0; counter < elementCount; counter++)
            {
                if (readIndex == Elements.Length)
                {
                    readIndex = 0;
                }

                biggerArray[writeIndex++] = Elements[readIndex++];
            }

            popIndex = 0;
            pushIndex = elementCount;
            Elements = biggerArray;
        }

        public Queue(int InitialCapacity = 20)
        {
            Elements = new int[InitialCapacity];
            minimumSize = InitialCapacity;
        }

        public bool IsEmpty()
        {
            return elementCount == 0;
        }

        public void Push(int v)
        {
            if (Elements.Length == elementCount)
                resize(Elements.Length * 2);
            elementCount++;
            if (pushIndex == Elements.Length) pushIndex = 0;
            Elements[pushIndex] = v;
            pushIndex++;
        }
        
        public int Pop()
        {
            if (IsEmpty()) throw new IndexOutOfRangeException();

            if (elementCount < Capacity / 3 && Capacity > minimumSize) resize(Capacity / 2);

            elementCount--;

            if (popIndex == Elements.Length) popIndex = 0;

            return Elements[popIndex++];
        }

        public void ForEach(Action<int> methodAction)
        {
            while (elementCount > 0)
            {
                methodAction(Pop());
            }
        }
    }
}
