using CommunityToolkit.Mvvm.Messaging.Messages;
using VRCFaceTracking.Avalonia.Services;

namespace VRCFaceTracking.Avalonia.Messages;

public class LoginSuccessMessage(AuthenticationResult result) : ValueChangedMessage<AuthenticationResult>(result);
