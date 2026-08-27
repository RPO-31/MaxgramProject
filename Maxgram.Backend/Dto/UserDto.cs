namespace Maxgram.Backend.Dto;

public record UserDto(int Id, string Username, string DisplayName, string Email);
public record UserSearchDto(int Id, string Username, string DisplayName);
