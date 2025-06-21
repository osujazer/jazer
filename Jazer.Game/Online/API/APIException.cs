using System;

namespace Jazer.Game.Online.API;

public class APIException(string[] errors, Exception? innerException)
    : InvalidOperationException(string.Join(", ", errors), innerException);
