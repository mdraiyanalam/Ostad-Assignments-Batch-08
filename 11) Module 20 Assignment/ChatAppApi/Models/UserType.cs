using ChatAppApi.Models;
using HotChocolate.Types;
namespace ChatAppApi.Models
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(u => u.Id);
            descriptor.Field(u => u.Name);
            descriptor.Field(u => u.Address);
        }
    }
}
