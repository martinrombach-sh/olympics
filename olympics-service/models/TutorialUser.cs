namespace OlympicsAPI
{
    //the model is partial because it is good practice to have the ability to add to them on the fly from inside another file.

    //it's important to use singular naming
    //this isnt the users model, its the user model
    public partial class TutorialUser()
    {
        public int UserId { get; set; }

        //nullable values can be handled two ways:
        //public string? UserName { get; set;} - set to nullable
        //or create a constructor and set values in case of null, like below
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool Active { get; set; }
        //invalid column name 'true'

    }
}