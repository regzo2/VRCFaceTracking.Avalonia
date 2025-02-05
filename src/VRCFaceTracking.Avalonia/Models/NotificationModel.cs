using System;
using System.Collections.Generic;

namespace VRCFaceTracking.Models;

public class NotificationModel
{
    public string Title;
    public string Body;
    public string? BodyImagePath;
    public string? BodyAltText;
    public List<(string Title, string ActionId)?> ActionButtons;
    public DateTimeOffset? OptionalScheduledTime;
    public DateTimeOffset? OptionalExpirationTime;
}
