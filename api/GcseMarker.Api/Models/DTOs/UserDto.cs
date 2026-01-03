namespace GcseMarker.Api.Models.DTOs;

public record UserDto(int Id, string Email, string? Name, bool IsAdmin);
