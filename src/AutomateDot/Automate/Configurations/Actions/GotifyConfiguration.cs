﻿using AutomateDot.Automate.Configurations;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class GotifyConfiguration : IActionConfiguration
{
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public int Priority { get; set; }
}