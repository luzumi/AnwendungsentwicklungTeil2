﻿using System;
using Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContainerTest
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void CreateableStandard()
        {
            // testet ob eine Liste mit Standardkonstruktor erstellt werden kann
            Assert.IsNotNull(new List());
        }

        [TestMethod]
        public void IsEmptyAfterNew()
        {
            // testet ob eine neue liste auch leer ist
            List testList = new();
            Assert.IsTrue(testList.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyAfterAddAndRemove()
        {
            // testet ob die Liste nach Einfügen und Entfernen von einem Wert wieder leer ist
            List testList = new();
            testList.Add(1);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 0);
        }

        [TestMethod]
        public void IsNotEmptyAfterAdd()
        {
            // testet ob die Liste nach einem Einfügen eine Anzahl von einem Element hat
            List testList = new();
            testList.Add(1);
            Assert.IsTrue(testList.Count == 1);
        }

        [TestMethod]
        public void IsNotEmptyAfterDoubleAddAndRemove()
        {
            // testet ob nach mehrfachen Einfügen und Einigen entfernen das der Füllstandszähler korrekt arbeitet
            List testList = new();
            testList.Add(1);
            testList.Add(1);
            testList.Add(1);
            testList.Remove(0);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 1);
        }

        [TestMethod]
        public void IsNotEmptyAfterMultiAddAndRemove()
        {
            // testet count ob es nach mehreren Add und Remove jederzeit korrekt arbeitet
            List testList = new();
            testList.Add(1);
            testList.Add(1);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 1);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 0);
            Assert.IsTrue(testList.IsEmpty());
            testList.Add(1);
            testList.Add(1);
            Assert.IsTrue(testList.Count == 2);
        }

        [TestMethod]
        public void EqualAfterAddAndRemove()
        {
            // testet ob das eingefügte Element auch wieder ausgelesen werden kann
            List testList = new();
            int testValue = 20;
            testList.Add(testValue);
            int returnValue = testList.At(0);
            Assert.IsTrue(testValue == returnValue);
        }

        [TestMethod]
        public void EqualAfterMultiAddAndRemove()
        {
            // testet das mehrfache Einfügen und Auslesen und überprüft die Werte
            List testList = new();
            int testValueA = 20;
            int testValueB = 5;
            int testValueC = 9000;
            testList.Add(testValueA);
            testList.Add(testValueB);
            testList.Add(testValueC);
            int returnValueA = testList.At(0);
            int returnValueB = testList.At(1);
            int returnValueC = testList.At(2);
            Assert.IsTrue(testValueA == returnValueA);
            Assert.IsTrue(testValueB == returnValueB);
            Assert.IsTrue(testValueC == returnValueC);
        }

        [TestMethod]
        public void EqualAfterLoopAddAndRemove()
        {
            // testet das vergrössern der Liste und das alle Elemente auch korrekt gespeichert wurden
            List testList = new();

            for (int i = 10; i <= 90000; i++)
            {
                testList.Add(i);
            }

            for (int counter = 10; counter <= 90000; counter++)
            {
                Assert.IsTrue(testList.At(counter - 10) == counter);
            }

            // Pos :  00 01 02 03 04 05 ..
            // Wert:  10 11 12 13 14 15 ..
        }

        [TestMethod]
        public void EqualAfterLoopAddAndAtWithParameter()
        {
            // testet ob der überladene construktor funktioniert
            List testList = new(90000);

            Assert.IsTrue(testList.Capacity == 90000);
            for (int i = 10; i <= 90000; i++)
            {
                testList.Add(i);
            }

            for (int i = 10; i <= 90000; i++)
            {
                Assert.IsTrue(testList[i - 10] == i);
            }
        }

        [TestMethod]
        public void RemoveOnEmpty()
        {
            // testet ob das Entfernen aus einer bereits leeren Liste einen Fehler erzeugt
            List testList = new();
            int testValue = 20;
            testList.Add(testValue);
            testList.Add(testValue);
            testList.Remove(0);
            testList.Remove(0);
            Assert.ThrowsException<IndexOutOfRangeException>(() => testList.Remove(0));
        }

        [TestMethod]
        public void ContinuousAddRemoveAndAtOnMaxCapacity()
        {
            // testet ob nach mehrfachen Einfügen und Löschen das vergrössern gestartet wird und die Werte trotzdem
            // korrekt hintereinander stehen
            List testList = new(4);
            testList.Add(1); // 1
            testList.Add(2); // 1 2
            testList.Add(3); // 1 2 3
            testList.Add(4); // 1 2 3 4
            testList.Remove(1); // 1 3 4
            testList.Remove(0); // 3 4
            Assert.IsTrue(testList.At(0) == 3); // 3 4
            Assert.IsTrue(testList.At(1) == 4); // 3 4
            testList.Add(5); // 3 4 5
            testList.Add(6); // 3 4 5 6
            Assert.IsTrue(testList.At(3) == 6); // 3 4 5 6
            testList.Add(7); // 3 4 5 6 7  Vergrösserung der Liste
            testList.Add(8); // 3 4 5 6 7 8
            Assert.IsTrue(testList.At(5) == 8);
        }

        [TestMethod]
        public void ContinuousAddAndRemoveTiming()
        {
            // testet die Geschwindigkeit der Grössenänderung des array
            List testList = new(5);
            DateTime testStart = DateTime.Now;
            for (int counter = 0; counter < 100000; counter++)
                testList.Add(counter);
            for (int counter = 99999; counter >= 0; counter--)
                testList.Remove(counter);
            DateTime testEnd = DateTime.Now;
            Assert.IsTrue((testEnd - testStart).TotalMilliseconds < 100);
        }

        [TestMethod]
        public void ResizeAutomated()
        {
            // testet die automatische Verkleinerung der Liste
            List testList = new(5);
            for (int counter = 0; counter < 20; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 10; counter++)
                testList.Remove(0);
            for (int counter = 20; counter < 40; counter++)
                testList.Add(counter);
            for (int counter = 10; counter < 30; counter++)
                testList.Remove(0);
            Assert.IsTrue(testList.Capacity <= 20);
        }

        [TestMethod]
        public void ResizeManualMiddle()
        {
            List testList = new(10);
            for (int counter = 0; counter < 8; counter++) // 0 1 2 3 4 5 6 7
                testList.Add(counter);
            for (int counter = 0; counter < 4; counter++) // 4 5 6 7
                testList.Remove(0);
            testList.Capacity = 5;
            for (int counter = 4; counter < 8; counter++)
                Assert.IsTrue(testList.At(counter - 4) == counter);
        }

        [TestMethod]
        public void ResizeTooSmall()
        {
            // testet ob ein zu starkes verkleinern eine fehlermeldung zeigt
            List testList = new(10);
            Assert.IsTrue(testList.Capacity == 10);
            for (int counter = 0; counter < 8; counter++)
                testList.Add(counter);
            Assert.ThrowsException<InsufficientMemoryException>(() => testList.Capacity = 5);
        }

        [TestMethod]
        public void CapacityOutOfRange()
        {
            // testet ob eine negative grösse der liste eine fehlermeldung erzeugt
            List testList = new();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testList.Capacity = -1);
        }

        [TestMethod]
        public void CapacityNeverBelowInitialization()
        {
            // testet ob die grösse der liste nie unter die initialisierungsgrösse fällt
            List testList = new(10);
            for (int counter = 0; counter < 30; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 29; counter++)
                testList.Remove(0);
            Assert.IsTrue(testList.Capacity == 10);
        }

        [TestMethod]
        public void CapacityMinimumOnUserDefinition()
        {
            // testet ob die kapazität der liste nimals unter den benutzerdefinierten wert fällt
            List testList = new(10);
            for (int counter = 0; counter < 30; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 29; counter++)
                testList.Remove(0);
            Assert.IsTrue(testList.Capacity == 10);
            testList.Capacity = 5;
            for (int counter = 0; counter < 29; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 29; counter++)
                testList.Remove(0);
            Assert.IsTrue(testList.Capacity == 5);
        }

        [TestMethod]
        public void DirectResize()
        {
            // Testet ob der benutzer die kapazität anpassen kann
            List testList = new(15);
            testList.Capacity = 7;
            Assert.IsTrue(testList.Capacity == 7);
        }

        [TestMethod]
        public void ForEachDelegate()
        {
            // testet ob ForEach alle elemente durchgeht und für jedes die übergebene methode (hier ein lambda) startet
            List testList = new(10);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);
            int Q = 0;
            testList.ForEach((i) => Q += i);
            Assert.IsTrue(Q == 10);
        }

        [TestMethod]
        public void EqualThisAndAtFunction()
        {
            List testList = new(10);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            Assert.IsTrue(testList.At(0) == testList[0]);
            Assert.IsTrue(testList.At(1) == testList[1]);
            Assert.IsTrue(testList.At(2) == testList[2]);
            Assert.IsTrue(testList.At(3) == testList[3]);
            Assert.IsFalse(testList.At(1) == testList[3]);
        }

        [TestMethod]
        public void AdditionOfLists()
        {
            List listA = new();
            listA.Add(1);
            listA.Add(2);
            listA.Add(3);
            List listB = new();
            listB.Add(4);
            listB.Add(5);
            listB.Add(6);

            List listC = listA + listB;

            for (int counter = 0; counter < listC.Count; counter++)
            {
                Assert.IsTrue(listC[counter] == counter + 1);
            }
        }

        [TestMethod]
        public void AdditionOfManyLists()
        {
            List listA = new();
            listA.Add(1);
            listA.Add(2);
            listA.Add(3);
            List listB = new();
            listB.Add(4);
            listB.Add(5);
            listB.Add(6);

            List resultA = List.Merge(listA, listB, listA);
            List resultB = List.Merge(listA, listB, listA, listB, listA);

            for (int counter = 0; counter < listA.Count; counter++)
            {
                Assert.IsTrue(resultA[counter] == counter + 1);
            }

            for (int counter = 0; counter < listB.Count; counter++)
            {
                Assert.IsTrue(resultB [counter] == counter + 1);
            }
        }
    }
}
