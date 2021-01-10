using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    public class Queue
    {
        private int elementCounter = 0;
        private int firstIndex = 0;
        private int elementsCapacity = 0;
        public int[] elements;
        private int[] elementsCopy;
        private bool freePlace;


        public Queue(int Capacity = 5)
        {
            elements = new int[Capacity];
            elementsCopy = new int[Capacity];
            elementsCapacity = Capacity;
        }

        public bool IsEmpty()
        {
            return elementCounter == 0;
        }

        public void Push(int number)
        {
            if (elementCounter == elements.Length)
            {
                elementsCapacity *= 2;
                Array.Resize(ref elements, elementsCapacity);
            }

            elements[elementCounter++] = number;
        }

        public int Pop()
        {
            if (elementCounter == 0)
            {
                throw new IndexOutOfRangeException();
            }
            int result = elements[0];
            Array.Copy(elements, 1, elements, 0, elements.Length - 1);
            elementCounter--;
            return result;
        }
    }
}
