﻿namespace BulkyBook.Models.ViewModels;
public class UserVM
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Company { get; set; }
    public string Role { get; set; }
    public bool IsLocked { get; set; }
}
