using OlympicsAPI.Data;

//with repository flow, all entity framework logic is abstracted into here
public class UserRepository : IUserRepository
{
    //KEY: Repositories are instantiated via interfaces, in order to abstract away connections to the database. 

    DataContextEF _entityFramework;

    //constructor of a repository -> a generic type of 'model' that gets passed in as an argument to an endpoint, for DRY purposes 
    public UserRepository(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
    }

    //the repository's related base methods that are likely to be reused.
    //Save, Add and Delete are the only Dynamic methods that can be set up, beyond that things are more specific.
    public bool SaveChanges()
    {
        //Making a boolean of EF returning the no of rows, 0 = failure
        return _entityFramework.SaveChanges() > 0;
    }

    //generic type defined on call. Its important that T is written in both places, an example being AddEntity<int>(int number)
    public void AddEntity<T>(T entityToAdd)
    //it can be useful to return booleans on success failure of adding (not implemented, e.g. public bool, if entitytoadd return true/false)
    {
        if (entityToAdd != null)
        {
            _entityFramework.Add(entityToAdd);
        }
    }

    public void RemoveEntity<T>(T entityToRemove)
    {
        if (entityToRemove != null)
        {
            _entityFramework.Remove(entityToRemove);
        }
    }

        /*
        public IEnumerable<TutorialUser> GetUsers()
        {
            IEnumerable<TutorialUser> users = _entityFramework.TutorialUsers.ToList<TutorialUser>();
            return users;
        }
        */
    
}