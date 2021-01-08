using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    public class Queue
    {
        private uint elementCounter = 0;
        private uint firstIndex = 0;
        private int[] elements;

        public Queue()
        {
            elements = new int[5];
        }
        
        public Queue(int Capacity)
        {
            elements = new int[Capacity];
        } 
        
        public bool IsEmpty()
        {
            return elementCounter == 0;
        }
        
        public void Push(int i)
        {
            if (elementCounter == elements.Length)
            {
                Array.Resize(ref elements, elements.Length*2);
            }
            elements[elementCounter++] = i;
        }
        
        public int Pop()
        {
            int result = elements[firstIndex];
            
            elementCounter--;
            firstIndex++;

            return result;
        }
    }
}
