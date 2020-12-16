namespace TicTacToe
{
    public class Button
    {
        public FieldState FieldState;

        public Button(Point position)
        {
            Position = position;
        }

        private Point Position { get; }
    }
}