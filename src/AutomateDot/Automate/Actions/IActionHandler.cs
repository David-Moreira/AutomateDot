﻿using AutomateDot.Automate.Configurations;

namespace AutomateDot.Actions;

public interface IActionHandler<in T> where T : IActionConfiguration
{
    Task ExecuteAsync(T config, object? triggerPayload = null);
}
