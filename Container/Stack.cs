using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Container
{
    public class Stack
    {
        private uint elementCounter = 0;
        private int[] elements;

        public Stack()
        {
            elements = new int[5];
        }
        
        public Stack(int Capacity)
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
                //int[] newArray = new int[elements.Length * 2];
                //for (int j = 0; j < elements.Length; j++)
                //{
                //    newArray[i] = elements[i];
                //}

                //elements = newArray;
                Array.Resize(ref elements, elements.Length*2);
            }
            elements[elementCounter++] = i;
        }

        public int Pop()
        {
            return elements[--elementCounter];
        }
    }
}
