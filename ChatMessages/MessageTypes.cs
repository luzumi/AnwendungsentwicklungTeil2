// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageTypes.cs022021

namespace ChatMessages
{
    public enum MessageTypes : byte
    {
        Login,
        Logout,
        Broadcast,
        DirectMessage,

        JoinGroup,
        LeaveGroup,
        GroupMessage,
        
        

        ClientAdd,
        ClientRemove,
        ViewRequest,
        ViewAllClients
    }

    public enum LoginStates : byte
    {
        LoginSuccessfully,
        LoginFail,

    }
}
