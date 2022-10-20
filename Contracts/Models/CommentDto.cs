﻿using Contracts.Models.FootballMatch;
using Contracts.Models.User;

namespace Contracts.Models;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserDto User { get; set; }
    public FootballMatchDto FootballMatch { get; set; }
}