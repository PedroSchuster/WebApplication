namespace SocialMedia.Application.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ProfilePicURL { get; set; }

        public string? Bio { get; set; }

        public string? Password { get; set; }

        public string? Token { get; set; }

        public IEnumerable<UserUpdateDto>? Followers { get; set; }

        public int FollowersCount { get; set; }

        public IEnumerable<UserUpdateDto>? Following { get; set; }

        public int FollowingCount { get; set; }


    }
}