// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageTypes.cs022021

namespace ChatMessages
{
    public enum MessageTypes : byte
    {
        Login,
        Logout,
        LoginSuccessful,
        LoginFail,
        KickFromServer,
        Broadcast,
        DirectMessage,
        ServerShutdown,
        BanFromServer,
        JoinGroup,
        LeaveGroup,
        GroupMessage,
        RoomUserList,
        ClientAdd,
        ClientRemove,
        ViewRequest,
        ViewAllClients
    }


    public enum DataType : byte
    {
        Text,
        Image,
        File
    }

    public enum LoginStates : byte
    {
        LoginSuccessfully,
        LoginFail,

    }
}
