namespace OlympicsAPI.Dtos
{
    //DTO - Data Transfer Object
    //This was made for inserts (is this DRY?)
    public partial class UserDto()
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool Active { get; set; }

    }
}