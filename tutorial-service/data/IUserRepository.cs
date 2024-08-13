namespace OlympicsAPI.Data
{
    public interface IUserRepository
    {
        //By creating an instance of this interface, we don't need to create an instance of the UserRepository class itself.
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
    }

    //To add an interface, addScoped needs to be added to Program.cs
    //builder.Services.AddScoped<TutorialIUserRepository, TutorialUserRepository>();
}