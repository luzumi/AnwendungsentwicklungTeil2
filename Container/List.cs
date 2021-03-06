﻿using System;

namespace Container
{
    public class List
    {
        int[] elements;
        int minSize;

        public List(int MinimumSize = 16)
        {
            elements = new int[MinimumSize];
            minSize = MinimumSize;
        }

        public int Capacity
        {
            get => elements.Length;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                minSize = value;
                resize(value);
            }
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public int Count
        {
            get { return count; }
        }

        private int count;

        public void Add(int v)
        {
            if (elements.Length == count)
                resize(elements.Length * 2);
            elements[count] = v;
            count++;
        }

        private void resize(int v)
        {
            if (v <= count || v < 0) throw new InsufficientMemoryException();
            Array.Resize(ref elements, v);
        }

        public void Remove(int v)
        {
            if (v >= count) throw new IndexOutOfRangeException();
            for (int counter = v; counter < count - 1; counter++)
            {
                elements[counter] = elements[counter + 1];
            }

            if (count * 3 < Capacity && Capacity > minSize) resize(Capacity / 2);
            count--;
        }

        public int At(int v)
        {
            return elements[v];
        }

        public void ForEach(Action<int> pAction)
        {
            for (int i = 0; i < count; i++)
            {
                pAction(elements[i]);
            }
        }

        /// <summary>
        /// stellt [] schreibweise zur verfügung
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int this[int index]
        {
            get => elements[index];
            set => elements[index] = value;
        }

        /// <summary>
        /// hängt zwei Listen aneinander
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns>hängt Liste2 an Liste1 und gibt diese neue Liste zurück</returns>
        public static List operator +(List firstList, List secondList)
        {
            if (firstList == null || secondList == null) throw new ArgumentNullException();
            List resultList = new List(firstList.Count + secondList.Count);
            firstList.ForEach(resultList.Add);
            secondList.ForEach(resultList.Add);
            return resultList;
        }

        public static List Merge(params List[] pLists)
        {
            int size = 0;

            foreach (var list in pLists)
            {
                size += list.Count;
            }

            List result = new List(size);

            foreach (var list in pLists)
            {
                list.ForEach(result.Add);
            }

            return result;
        }
    }
}
