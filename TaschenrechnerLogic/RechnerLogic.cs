using System;
using System.ComponentModel;

namespace TaschenrechnerLogic
{
    public class RechnerLogic : INotifyPropertyChanged // das INotifyPropertyChanged-Interface wird benötigt damit wir dem GUI sagen können wann sich werte ändern
    {
        private int opA;
        private int opB;
        private Operations op;
        public event PropertyChangedEventHandler PropertyChanged;


        public int OperatorA
        {
            get { return opA; }
            set
            {
                if (opA != value)
                {
                    opA = value;
                    // da sich durch das verändern von einem Operator auch das ergebnis ändert geben wir dem GUI
                    // die info das auch all Bindungen auf das Property Result neu gelesen werden sollen
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperatorA)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }


        public int OperatorB
        {
            get { return opB; }
            set
            {

                if (opB != value)
                {
                    opB = value;
                    // da sich durch das verändern von einem Operator auch das ergebnis ändert geben wir dem GUI
                    // die info das auch all Bindungen auf das Property Result neu gelesen werden sollen
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperatorB)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }


        public int Result
        {
            // beim lesen des Ergebnisses wird erst berechnet. dadurch brauchen wir keinen setter und auch keine private variable
            get
            {
                switch (op)
                {
                    case Operations.Addition: return opA + opB;
                    case Operations.Subtraction: return opA - opB;
                    case Operations.Multiplication: return opA * opB;
                    case Operations.Division: return opB == 0 ? 0 : opA / opB;
                    case Operations.Modulo: return opB == 0 ? 0 : opA % opB;
                    default: return 0;
                }
            }
        }


        public Operations Operator
        {
            get { return op; }
            set
            {
                if (op != value)
                {
                    op = value;
                    // da sich durch das verändern von der Mathematischen Operation auch das ergebnis ändert geben wir
                    // dem GUI die info das auch all Bindungen auf das Property Result neu gelesen werden sollen
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Operator)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

    }
}
