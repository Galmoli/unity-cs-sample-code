using Domain.Entities;
using SharedTypes.User;

namespace Domain.DataAccess
{
    public interface IUserDataAccess
    {
        void EditUsername(string _username);
    }
}