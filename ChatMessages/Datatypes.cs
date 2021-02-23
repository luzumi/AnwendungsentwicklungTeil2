// Daniel Neubieser:WPFFIrstStepsChatMessagesDatatypes.cs022021

namespace ChatMessages
{
    /// <summary>
    /// Datentyp der gesendet/empfangen wird
    /// </summary>
    public enum Datatypes : byte
    {
        /// <summary>
        /// Eine Textnachricht wird gesendet/empfangen
        /// </summary>
        Text,
        /// <summary>
        /// Eine Bildnachricht wird gesendet/empfangen
        /// </summary>
        Picture,
        /// <summary>
        /// Eine Datei wird gesendet/empfangen
        /// </summary>
        File,
        /// <summary>
        /// nicht spezifizierter Nachrichtentyp wird gesendet/empfangen
        /// </summary>
        Other
    }
}
