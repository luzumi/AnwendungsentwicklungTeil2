using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;

namespace TicTacToeTest
{
    [TestClass]
    public class TicTacToe
    {
        [TestMethod]
        public void GameCreation()
        {
            Spielfeld s = new Spielfeld();
            Assert.IsNotNull(s.GetPlayerID());
        }

        [TestMethod]
        public void GameFieldCount()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            Assert.IsTrue(s.GetBoard().Length == 9);
        }

        [TestMethod]
        public void TurnValid()
        {
            Spielfeld s = new Spielfeld();
            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 3; j++)
                {
                    s = new Spielfeld();
                    Assert.IsTrue(s.Turn(new Point(i, j)) == TurnResult.Valid);
                }
            }
        }


        [TestMethod]
        public void TurnInvalid()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 0));
            Assert.IsTrue(s.Turn(new Point(0, 0)) == TurnResult.Invalid);
        }
        
        
        [TestMethod]
        public void TurnTie()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 0));
            s.Turn(new Point(1, 0));
            s.Turn(new Point(2, 0));
            
            s.Turn(new Point(1, 1));
            s.Turn(new Point(0, 1));
            s.Turn(new Point(0, 2));
            
            s.Turn(new Point(2, 1));
            s.Turn(new Point(2, 2));
            Assert.IsTrue(s.Turn(new Point(1, 2)) == TurnResult.Tie);
        }
        
        
        [TestMethod]
        public void TurnToWinHorizontalRow0()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 0));
            s.Turn(new Point(1, 0));
            s.Turn(new Point(0, 1));
            
            s.Turn(new Point(1, 1));
            Assert.IsTrue(s.Turn(new Point(0, 2)) == TurnResult.Win);
        }
        
        
        [TestMethod]
        public void TurnToWinHorizontalRow1()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(1, 0));
            s.Turn(new Point(0, 0));
            s.Turn(new Point(1, 1));
            
            s.Turn(new Point(0, 1));
            Assert.IsTrue(s.Turn(new Point(1, 2)) == TurnResult.Win);
        }
        
        
        [TestMethod]
        public void TurnToWinHorizontalRow2()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(2, 0));
            s.Turn(new Point(0, 0));
            s.Turn(new Point(2, 1));
            
            s.Turn(new Point(0, 1));
            Assert.IsTrue(s.Turn(new Point(2, 2)) == TurnResult.Win);
        }

        
        [TestMethod]
        public void TurnToWinVerticalCol0()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 0));
            s.Turn(new Point(0, 1));
            s.Turn(new Point(1, 0));
            
            s.Turn(new Point(1, 1));
            Assert.IsTrue(s.Turn(new Point(2, 0)) == TurnResult.Win);
        }
        
        
        [TestMethod]
        public void TurnToWinVerticalCol1()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 1));
            s.Turn(new Point(0, 0));
            s.Turn(new Point(1, 1));
            
            s.Turn(new Point(1, 0));
            Assert.IsTrue(s.Turn(new Point(2, 1)) == TurnResult.Win);
        }
        
        
        [TestMethod]
        public void TurnToWinVerticalCol2()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 2));
            s.Turn(new Point(0, 1));
            s.Turn(new Point(1, 2));
            
            s.Turn(new Point(1, 1));
            Assert.IsTrue(s.Turn(new Point(2, 2)) == TurnResult.Win);
        }
        
        
        [TestMethod]
        public void TurnToWinDiagonal0()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 0));
            s.Turn(new Point(1, 0));
            s.Turn(new Point(1, 1));
            
            s.Turn(new Point(0, 1));
            Assert.IsTrue(s.Turn(new Point(2, 2)) == TurnResult.Win);
        }
        
        
        [TestMethod]
        public void TurnToWinDiagonal1()
        {
            Spielfeld s = new Spielfeld();
            s.GetBoard();
            s.Turn(new Point(0, 2));
            s.Turn(new Point(0, 1));
            s.Turn(new Point(1, 1));
            
            s.Turn(new Point(2, 1));
            Assert.IsTrue(s.Turn(new Point(2, 0)) == TurnResult.Win);
        }
        
        
        
        /*
         * anzahl der felder muss 9 sein
         * 
         * zug auf belegtes feld soll invalid sein
         * 
         * gültiger spielerzug wird auf spielfeld eingetragen
         * 
         * spielerwechsel nach zug
         * 
         * sieg über horizontale
         * sieg über diagonale
         * sieg über vertikale
         * 
         * unentschieden wenn alle felder belegt und kein zug mehr möglich
         */
    }
}
