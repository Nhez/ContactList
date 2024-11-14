﻿namespace CL.Shared.Abstractions.Modules;

public interface IModuleSerializer
{
    byte[] Serialize<T>(T value);
    T Deserialize<T>(byte[] value);
    object Deserialize(byte[] value, Type type);
}
