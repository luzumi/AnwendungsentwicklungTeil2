using System;
using System.Collections.Generic;
using System.Linq;

public class ListFilterer
{
    public static void Main(string[] args)
    {
    }

    public class Number
    {
        public int DigitalRoot(long n)
        {
            int result = 0;
            string numberRow = n.ToString();

            while (numberRow.Length > 1)
            {
                result = 0;

                foreach (var item in numberRow)
                {
                    result += Int32.Parse(item.ToString());
                }

                numberRow = result.ToString();
            }
            return result;
        }
    }

    public static List<string> Anagrams(string word, List<string> words)
    {
        List<string> result = new List<string>();

        foreach (var itemWord in words)
        {
            List<char> charOfWord = new List<char>(word.ToList());
            List<char> checkItemWord = itemWord.ToList();
            charOfWord.Sort();
            checkItemWord.Sort();
            bool check = false;
            if (charOfWord.Count == checkItemWord.Count)
            {
                for (int i = 0; i < itemWord.Length; i++)
                {
                    if (charOfWord[i] == checkItemWord[i])
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        break;
                    }
                }

                if (check) result.Add(itemWord);
            }
        }

        return result;
    }


    public class Persist
    {
        public static int Persistence(long n)
        {
            string strNumbers = n.ToString();
            int count = 0;

            while (strNumbers.Length > 1)
            {
                long tmp = 1;
                count++;

                foreach (var number in strNumbers) tmp *= (number - 48);

                strNumbers = tmp.ToString();
            }

            return count;
        }
    }

    public class Kata
    {
        public static int Find(int[] integers)
        {
            int numb = 0;

            foreach (var i in integers) numb += i;

            numb = (numb % 2 == 0) ? 0 : 1;

            foreach (var i in integers)
                if (i % 2 == numb)
                    return i;

            return -1;
        }

        public static string PigIt(string pigLatinIsCool)
        {
            string[] inputStrings = pigLatinIsCool.Split(" ");
            string result = "";

            foreach (string s in inputStrings)
            {
                result += s[1..] + s[0] + "ay ";
            }

            return result.Trim();
        }

        public static int DuplicateCount(string empty)
        {
            int count = 0;
            empty = empty.ToLower();
            string check = "";
            for (int i = 0; i < empty.Length - 1; i++)
            {
                char tmp = empty[i];
                if (check.Contains(tmp)) continue;
                for (int j = i + 1; j < empty.Length; j++)
                {
                    if (tmp == empty[j])
                    {
                        count++;
                        check += tmp;
                        break;
                    }
                }
            }

            return count;
        }

        public static bool IsValidWalk(string[] pWayPoints)
        {
            //Sie wohnen in der Stadt Cartesia, wo alle Straßen in einem perfekten Raster angeordnet sind.
            //Sie kamen zehn Minuten zu früh zu einem Termin an und beschlossen, die Gelegenheit zu nutzen,
            //um einen kurzen Spaziergang zu machen. Die Stadt bietet ihren Bürgern eine App zum Generieren
            //von Spaziergängen auf ihren Handys. Jedes Mal, wenn Sie die Taste drücken, erhalten Sie eine
            //Reihe von Ein-Buchstaben-Zeichenfolgen, die die Laufrichtungen darstellen
            //(z. B. ['n', 's', 'w',). 'e']). Sie gehen immer nur einen Block für jeden Buchstaben (Richtung)
            //und Sie wissen, dass Sie eine Minute brauchen, um einen Stadtblock zu durchqueren.
            //Erstellen Sie also eine Funktion, die true zurückgibt, wenn der von der App angebotene
            //Spaziergang genau zehn Minuten dauert (Sie) Ich möchte nicht früh oder spät sein!) und werde
            //Sie natürlich zu Ihrem Ausgangspunkt zurückbringen. Andernfalls wird false zurückgegeben.
            int horizontal = 0;
            int vertical = 0;
            foreach (var direction in pWayPoints)
            {
                switch (direction)
                {
                    case "n":
                        vertical++;
                        break;
                    case "s":
                        vertical--;
                        break;
                    case "e":
                        horizontal++;
                        break;
                    case "w":
                        horizontal--;
                        break;
                    default:
                        break;
                }
            }

            return pWayPoints.Length == 10 && horizontal == 0 && vertical == 0;
        }

        public static bool ValidatePin(string pin)
        {
            switch (pin.Length)
            {
                case 4:
                case 6:
                {
                    foreach (var item in pin)
                    {
                        if (!Int32.TryParse(item.ToString(), out _))
                            return false;
                    }

                    return true;
                }
                default:
                    return false;
            }
        }
    }

    public static class Kata2
    {
        public static IEnumerable<string> FriendOrFoe(string[] names)
        {
            List<string> friends = new List<string>();
            foreach (var name in names)
            {
                if (name.Length == 4) friends.Add(name);
            }

            return (IEnumerable<string>)friends;
        }
    }

    public static IEnumerable<int> GetIntegersFromList(List<object> listOfItems)
    {
        List<int> numbers = new List<int>();
        foreach (var item in listOfItems)
        {
            if (item is string) continue;
            string s = item.ToString();
            if (Int32.TryParse(item.ToString(), out _))
                numbers.Add(Int32.Parse(s ?? throw new InvalidOperationException()));
        }

        return numbers;
    }


    /// <summary>
    /// Kata8
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    public static string SpinWords(string sentence)
    {
        string[] arr = sentence.Split(" ");
        string result = "";
        int count = 0;
        foreach (string s in arr)
        {
            if (s.Length >= 5)
            {
                for (int i = s.Length; i > 0; i--)
                {
                    result += s[i];
                }
            }
            else result += s;

            if (arr.Length > 1) result += " ";

            count++;
        }

        return result.Trim();
    }
}
