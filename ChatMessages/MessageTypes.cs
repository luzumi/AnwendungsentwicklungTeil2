// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageTypes.cs022021

namespace ChatMessages
{
    /// <summary>
    /// NachrichtenArt
    /// </summary>
    public enum MessageTypes : byte
    {
        /// <summary>
        /// LoginAnfrage
        /// </summary>
        Login,

        /// <summary>
        /// LogoutAnfrage
        /// </summary>
        Logout,

        /// <summary>
        /// fehlerhafter Login
        /// </summary>
        LoginFail,

        /// <summary>
        /// erfolgreicher Login
        /// </summary>
        LoginSuccessfully,

        /// <summary>
        /// Client wird vom Server getrennt
        /// </summary>
        KickFromServer,

        /// <summary>
        /// Nachricht an alle Clients
        /// </summary>
        Broadcast,

        /// <summary>
        /// Nachricht an einen (gewünschten) Client
        /// </summary>
        DirectMessage,

        /// <summary>
        /// Server wird heruntergefahren
        /// </summary>
        ServerShutdown,

        /// <summary>
        /// Client wird permanent von Server getrennt
        /// </summary>
        BanFromServer,

        /// <summary>
        /// Client tritt GruppenChat bei
        /// </summary>
        JoinGroup,

        /// <summary>
        /// Client verlässt Gruppenchat
        /// </summary>
        LeaveGroup,

        /// <summary>
        /// Nachricht an alle Clients in einer Gruppe
        /// </summary>
        GroupMessage,

        /// <summary>
        /// Anfrage einer Userliste einer Chatgruppe
        /// </summary>
        RoomUserList,

        /// <summary>
        /// fügt einen Neuen Client hinzu
        /// </summary>
        ClientAdd,

        /// <summary>
        /// entfernt einen Client
        /// </summary>
        ClientRemove,

        /// <summary>
        /// Anfrage einer speziellen ClientList
        /// </summary>
        ViewRequest,

        /// <summary>
        /// Anfrage Clientlist aller Clients
        /// </summary>
        ViewAllClients
    }

    /// <summary>
    /// DatenTyp der Nachricht
    /// </summary>
    public enum DataType : byte
    {
        /// <summary>
        /// eine Textnachricht wird verschickt
        /// </summary>
        Text,

        /// <summary>
        /// ein Bild wird verschickt
        /// </summary>
        Image,

        /// <summary>
        /// eine Datei wird verschickt
        /// </summary>
        File
    }
}
