using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class PerfectPowerTest
    {
        [Test]
        public void Test0()
        {
            Assert.IsNull(ListFilterer.PerfectPower.IsPerfectPower(0), "0 is not a perfect number");
        }

        [Test]
        public void Test1()
        {
            Assert.IsNull(ListFilterer.PerfectPower.IsPerfectPower(1), "1 is not a perfect number");
        }

        [Test]
        public void Test2()
        {
            Assert.IsNull(ListFilterer.PerfectPower.IsPerfectPower(2), "2 is not a perfect number");
        }

        [Test]
        public void Test3()
        {
            Assert.IsNull(ListFilterer.PerfectPower.IsPerfectPower(3), "3 is not a perfect number");
        }

        [Test]
        public void Test4()
        {
            Assert.AreEqual((2, 2), ListFilterer.PerfectPower.IsPerfectPower(4), "4 = 2^2");
        }

        [Test]
        public void Test5()
        {
            Assert.IsNull(ListFilterer.PerfectPower.IsPerfectPower(5), "5 is not a perfect power");
        }

        [Test]
        public void Test8()
        {
            Assert.AreEqual((2, 3), ListFilterer.PerfectPower.IsPerfectPower(8), "8 = 2^3");
        }

        [Test]
        public void Test9()
        {
            Assert.AreEqual((3, 2), ListFilterer.PerfectPower.IsPerfectPower(9), "9 = 3^2");
        }

        [Test]
        public void TestUpTo500()
        {
            var pp = new int[]
            {
                4, 8, 9, 16, 25, 27, 32, 36, 49, 64, 81, 100, 121, 125, 128, 144, 169, 196, 216, 225, 243, 256, 289,
                324, 343, 361, 400, 441, 484
            };
            foreach (var i in pp)
                Assert.IsNotNull(ListFilterer.PerfectPower.IsPerfectPower(i), $"{i} is a perfect power");
        }
    }

    [TestFixture]
    public static class ScrambliesTests
    {
        private static void testing(bool actual, bool expected)
        {
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public static void test1()
        {
            testing(ListFilterer.Scramblies.Scramble("rkqodlw", "world"), true);
            testing(ListFilterer.Scramblies.Scramble("cedewaraaossoqqyt", "codewars"), true);
            testing(ListFilterer.Scramblies.Scramble("katas", "steak"), false);
            testing(ListFilterer.Scramblies.Scramble("scriptjavx", "javascript"), false);
            testing(ListFilterer.Scramblies.Scramble("scriptingjava", "javascript"), true);
            testing(ListFilterer.Scramblies.Scramble("scriptsjava", "javascripts"), true);
            testing(ListFilterer.Scramblies.Scramble("javscripts", "javascript"), false);
            testing(ListFilterer.Scramblies.Scramble("aabbcamaomsccdd", "commas"), true);
            testing(ListFilterer.Scramblies.Scramble("commas", "commas"), true);
            testing(ListFilterer.Scramblies.Scramble("sammoc", "commas"), true);
        }
    }

    [TestFixture]
    public class SolutionTest
    {
        [Test]
        public void GetIntegersFromList_MixedValues_ShouldPass_1()
        {
            var list = new List<object>() {1, 2, "a", "b"};
            var expected = new List<int>() {1, 2};
            var actual = ListFilterer.GetIntegersFromList(list);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public void GetIntegersFromList_MixedValues_ShouldPass_2()
        {
            var list = new List<object>()
            {
                1,
                "a",
                "b",
                0,
                15
            };
            var expected = new List<int>() {1, 0, 15};
            var actual = ListFilterer.GetIntegersFromList(list);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public void GetIntegersFromList_MixedValues_ShouldPass_3()
        {
            var list = new List<object>()
            {
                1,
                2,
                "aasf",
                "1",
                "123",
                123
            };
            var expected = new List<int>() {1, 2, 123};
            var actual = ListFilterer.GetIntegersFromList(list);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public static void Test1()
        {
            int[] exampleTest1 = {2, 6, 8, -10, 3};
            Assert.IsTrue(3 == ListFilterer.Kata.Find(exampleTest1));
        }

        [Test]
        public static void Test2()
        {
            int[] exampleTest2 = {206847684, 1056521, 7, 17, 1901, 21104421, 7, 1, 35521, 1, 7781};
            Assert.IsTrue(206847684 == ListFilterer.Kata.Find(exampleTest2));
        }

        [Test]
        public static void Test3()
        {
            int[] exampleTest3 = {int.MaxValue, 0, 1};
            Assert.IsTrue(0 == ListFilterer.Kata.Find(exampleTest3));
        }

        [Test]
        public void TestNames()
        {
            string[] expected = {"Ryan", "Mark"};
            string[] names = {"Ryan", "Kieran", "Mark", "Jimmy"};
            CollectionAssert.AreEqual(expected, ListFilterer.Kata2.FriendOrFoe(names));
        }

        [Test]
        public void KataTests()
        {
            Assert.AreEqual("igPay atinlay siay oolcay", ListFilterer.Kata.PigIt("Pig latin is cool"));
            Assert.AreEqual("hisTay siay ymay tringsay", ListFilterer.Kata.PigIt("This is my string"));
        }

        [Test]
        public void KataTests2()
        {
            Assert.AreEqual(0, ListFilterer.Kata.DuplicateCount(""));
            Assert.AreEqual(0, ListFilterer.Kata.DuplicateCount("abcde"));
            Assert.AreEqual(2, ListFilterer.Kata.DuplicateCount("aabbcde"));
            Assert.AreEqual(2, ListFilterer.Kata.DuplicateCount("aabBcde"), "should ignore case");
            Assert.AreEqual(1, ListFilterer.Kata.DuplicateCount("Indivisibility"));
            Assert.AreEqual(2, ListFilterer.Kata.DuplicateCount("Indivisibilities"), "characters may not be adjacent");
        }

        [Test]
        public void SampleTest()
        {
            Assert.AreEqual(true,
                ListFilterer.Kata.IsValidWalk(new string[] {"n", "s", "n", "s", "n", "s", "n", "s", "n", "s"}),
                "should return true");
            Assert.AreEqual(false,
                ListFilterer.Kata.IsValidWalk(new string[]
                {
                    "w", "e", "w", "e", "w", "e", "w", "e", "w", "e", "w", "e"
                }), "should return false");
            Assert.AreEqual(false, ListFilterer.Kata.IsValidWalk(new string[] {"w"}), "should return false");
            Assert.AreEqual(false,
                ListFilterer.Kata.IsValidWalk(new string[] {"n", "n", "n", "s", "n", "s", "n", "s", "n", "s"}),
                "should return false");
        }

        [Test]
        public void Test10()
        {
            Console.WriteLine("****** Basic Tests");
            Assert.AreEqual(3, ListFilterer.Persist.Persistence(39));
            Assert.AreEqual(0, ListFilterer.Persist.Persistence(4));
            Assert.AreEqual(2, ListFilterer.Persist.Persistence(25));
            Assert.AreEqual(4, ListFilterer.Persist.Persistence(999));
        }

        [Test, Description("ValidatePin should return false for pins with length other than 4 or 6")]
        public void LengthTest()
        {
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("1"), "Wrong output for \"1\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("12"), "Wrong output for \"12\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("123"), "Wrong output for \"123\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("12345"), "Wrong output for \"12345\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("1234567"), "Wrong output for \"1234567\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("-1234"), "Wrong output for \"-1234\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("1.234"), "Wrong output for \"1.234\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("-1.234"), "Wrong output for \"-1.234\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("00000000"), "Wrong output for \"00000000\"");
        }

        [Test, Description("ValidatePin should return false for pins which contain characters other than digits")]
        public void NonDigitTest()
        {
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin("a234"), "Wrong output for \"a234\"");
            Assert.AreEqual(false, ListFilterer.Kata.ValidatePin(".234"), "Wrong output for \".234\"");
        }

        [Test, Description("ValidatePin should return true for valid pins")]
        public void ValidTest()
        {
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("1234"), "Wrong output for \"1234\"");
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("0000"), "Wrong output for \"0000\"");
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("1111"), "Wrong output for \"1111\"");
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("123456"), "Wrong output for \"123456\"");
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("098765"), "Wrong output for \"098765\"");
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("000000"), "Wrong output for \"000000\"");
            Assert.AreEqual(true, ListFilterer.Kata.ValidatePin("090909"), "Wrong output for \"090909\"");
        }

        [Test]
        public void SampleTest1()
        {
            //Assert.AreEqual(new List<string> { "a" }, ListFilterer.Anagrams("a", new List<string> { "a", "b", "c", "d" }));
            Assert.AreEqual(
                new List<string> {"carer", "arcre", "carre"},
                ListFilterer.Anagrams(
                    "racer",
                    new List<string>
                    {
                        "carer",
                        "arcre",
                        "carre",
                        "racrs",
                        "racers",
                        "arceer",
                        "raccer",
                        "carrer",
                        "cerarr"
                    }));
        }


        public class NumberTest
        {
            private ListFilterer.Number num;

            [SetUp]
            public void SetUp()
            {
                num = new ListFilterer.Number();
            }

            [TearDown]
            public void TearDown()
            {
                num = null;
            }

            [Test]
            public void Tests()
            {
                Assert.AreEqual(7, num.DigitalRoot(16));
                Assert.AreEqual(6, num.DigitalRoot(456));
            }
        }
    }

    [TestFixture]
    public class DecomposeTests
    {

        [Test]
        public void Test1()
        {
            ListFilterer.Decompose d = new ListFilterer.Decompose();
            long n = 11;
            Assert.AreEqual("1 2 4 10", d.decompose(n));
            //TODO: Test löppt nicht
            n = 12;
            Assert.AreEqual("1 3 7 9", d.decompose(n));
        }
    }
}
