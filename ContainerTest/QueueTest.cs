using System;
using Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContainerTest
{
    [TestClass]
    public class QueueTest
    {
        [TestMethod]
        public void IsEmptyAfterNew()
        {
            Queue testQueue = new();

            Assert.IsTrue(testQueue.IsEmpty());
        }


        [TestMethod]
        public void IsEmptyAfterPushAndPop()
        {
            Queue testQueue = new();

            testQueue.Push(1);

            _ = testQueue.Pop();

            Assert.IsTrue(testQueue.IsEmpty());
        }


        [TestMethod]
        public void IsNotEmptyAfterPush()
        {
            Queue testQueue = new();

            testQueue.Push(1);

            Assert.IsFalse(testQueue.IsEmpty());
        }


        [TestMethod]
        public void IsNotEmptyAfterDoublePushAndPop()
        {
            Queue testQueue = new();

            testQueue.Push(1);
            testQueue.Push(1);

            testQueue.Pop();

            Assert.IsFalse(testQueue.IsEmpty());
        }


        [TestMethod]
        public void IsNotEmptyAfterMultiPushAndPop()
        {
            Queue testQueue = new();

            testQueue.Push(1);
            testQueue.Push(1);

            testQueue.Pop();

            Assert.IsFalse(testQueue.IsEmpty());

            testQueue.Pop();

            Assert.IsTrue(testQueue.IsEmpty());

            testQueue.Push(1);
            testQueue.Push(1);

            Assert.IsFalse(testQueue.IsEmpty());
        }


        [TestMethod]
        public void EqualAfterPushAndPop()
        {
            Queue testQueue = new();
            int testValue = 20;

            testQueue.Push(testValue);

            int returnValue = testQueue.Pop();

            Assert.IsTrue(testValue == returnValue);
        }


        [TestMethod]
        public void EqualAfterMultiPushAndPop()
        {
            Queue testQueue = new();

            int testValueA = 20;
            int testValueB = 5;
            int testValueC = 9_000;

            testQueue.Push(testValueA);
            testQueue.Push(testValueB);
            testQueue.Push(testValueC);

            int returnValueA = testQueue.Pop();
            int returnValueB = testQueue.Pop();
            int returnValueC = testQueue.Pop();

            Assert.IsTrue(testValueA == returnValueA);
            Assert.IsTrue(testValueB == returnValueB);
            Assert.IsTrue(testValueC == returnValueC);
        }


        [TestMethod]
        public void EqualAfterLoopPushAndPop()
        {
            Queue testQueue = new();

            for (int i = 10; i <= 90_000; i++)
            {
                testQueue.Push(i);
            }

            for (int i = 10; i <= 90_000; i++)
            {
                Assert.IsTrue(testQueue.Pop() == i);
            }
        }


        [TestMethod]
        public void EqualAfterLoopPushAndPopWithParameter()
        {
            Queue testQueue = new(90_000);

            for (int i = 10; i <= 90_000; i++)
            {
                testQueue.Push(i);
            }

            for (int i = 10; i <= 90_000; i++)
            {
                Assert.IsTrue(testQueue.Pop() == i);
            }
        }


        [TestMethod]
        public void MakeBiggerIfArraySizeEnd()
        {
            Queue testQueue = new();

            for (int i = 0; i <= 10; i++)
            {
                testQueue.Push(i);
            }

            Assert.IsTrue(testQueue.Elements.Length == 20);
        }

        [TestMethod]
        public void PopOnEmpty()
        {
            Queue testQueue = new();

            int testValue = 20;

            testQueue.Push(testValue);
            testQueue.Push(testValue);
            testQueue.Pop();
            testQueue.Pop();

            Assert.ThrowsException<IndexOutOfRangeException>(() => testQueue.Pop());
        }


        [TestMethod]
        public void ContinuousPushAndPop()
        {
            Queue testQueue = new();

            for (int testNumber = 0; testNumber < 50; testNumber++)
            {
                testQueue.Push(testNumber);

                Assert.IsTrue(testNumber == testQueue.Pop());
            }
        }


        [TestMethod]
        public void ContinuousPushAndPopOnMaxCapacity()
        {
            Queue testQueue = new(4);
            testQueue.Push(1);
            testQueue.Push(2);
            testQueue.Push(3);
            testQueue.Push(4);
            Assert.IsTrue(testQueue.Pop() == 1);
            Assert.IsTrue(testQueue.Pop() == 2);
            testQueue.Push(5);
            testQueue.Push(6);
            Assert.IsTrue(testQueue.Pop() == 3);
            testQueue.Push(7);
            testQueue.Push(8);
            Assert.IsTrue(testQueue.Pop() == 4);
        }

        [TestMethod]
        public void ContinuousPushTiming()
        {
            Queue testQueue = new(5);
            Assert.IsTrue(testQueue.Capacity == 5);

            DateTime testStart = DateTime.Now;
            Assert.IsTrue(testStart.GetType() == typeof(DateTime));

            for (int counter = 0; counter < 100_000; counter++)
                testQueue.Push(counter);
            for (int counter = 0; counter < 100_000; counter++)
                testQueue.Pop();
            Assert.IsTrue((DateTime.Now - testStart).TotalMilliseconds < 200);
        }

        [TestMethod]
        public void ZBonusResizeAutomated()
        {
            Queue testQueue = new(5);
            for (int counter = 0; counter < 20; counter++)
                testQueue.Push(counter);
            for (int counter = 0; counter < 10; counter++)
                Assert.IsTrue(testQueue.Pop() == counter);
            for (int counter = 20; counter < 40; counter++)
                testQueue.Push(counter);
            for (int counter = 10; counter < 30; counter++)
                Assert.IsTrue(testQueue.Pop() == counter);
            Assert.IsTrue(testQueue.Capacity <= 20);
        }

        [TestMethod]
        public void ZBonusResizeManualMiddle()
        {
            Queue testQueue = new(10);
            for (int counter = 0; counter < 8; counter++)
                testQueue.Push(counter);
            for (int counter = 0; counter < 4; counter++)
                Assert.IsTrue(testQueue.Pop() == counter);
            testQueue.Capacity = 5;
            for (int counter = 4; counter < 8; counter++)
                Assert.IsTrue(testQueue.Pop() == counter);
        }

        [TestMethod]
        public void ZBonusResizeManualEnd()
        {
            Queue testQueue = new(10);
            for (int counter = 0; counter < 10; counter++)
                testQueue.Push(counter);
            for (int counter = 0; counter < 8; counter++)
                Assert.IsTrue(testQueue.Pop() == counter);
            for (int counter = 10; counter < 12; counter++)
                testQueue.Push(counter);
            testQueue.Capacity = 5;
            for (int counter = 8; counter < 12; counter++)
                Assert.IsTrue(testQueue.Pop() == counter);
        }

        [TestMethod]
        public void ZBonusResizeTooSmall()
        {
            Queue testQueue = new(10);
            for (int counter = 0; counter < 8; counter++)
                testQueue.Push(counter);
            Assert.ThrowsException<InsufficientMemoryException>(() => testQueue.Capacity = 5);
        }

        [TestMethod]
        public void ZBonusDirectResize()
        {
            Queue tesQueue = new(15);
            tesQueue.Capacity = 7;
            Assert.IsTrue(tesQueue.Capacity == 7);
        }

        [TestMethod]
        public void ZBonusCapacityOutOfRange()
        {
            Queue testQueue = new();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testQueue.Capacity = -1);
        }

        [TestMethod]
        public void ZBonusCapacityNeverBelowInitialization()
        {
            Queue testQueue = new(10);

            for (int counter = 0; counter < 30; counter++)
                testQueue.Push(counter);

            for (int counter = 0; counter < 29; counter++)
                testQueue.Pop();


            Assert.IsTrue(testQueue.Capacity == 10);
        }

        [TestMethod]
        public void ZBonusCapacityMinumumOnUserDefenition()
        {
            Queue testQueue = new(10);

            for (int counter = 0; counter < 30; counter++)
                testQueue.Push(counter);

            for (int counter = 0; counter < 29; counter++)
                testQueue.Pop();

            testQueue.Capacity = 5;

            for (int counter = 0; counter < 30; counter++)
                testQueue.Push(counter);

            for (int counter = 0; counter < 29; counter++)
                testQueue.Pop();

            Assert.IsTrue(testQueue.Capacity == 10);
        }

        [TestMethod]
        public void ZBonusForEachDelegate()
        {
            Queue testQueue = new(10);
            testQueue.Push(1);
            testQueue.Push(2);
            testQueue.Push(3);
            testQueue.Push(4);
            int Q = 0;
            testQueue.ForEach((i) => Q += i);
            Assert.IsTrue(Q == 10);
        }
    }
}
