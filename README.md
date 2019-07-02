# EntityFramework Many-To-Many relationship

Currently EntityFramework doas not support **Many-To-Many** relationship. In order to achieve such us relation **joint entity** is needed. That creates uncoforable sytuation in which we have to constantly deal with aditional 'middleman' entity that usually serves no other porpues then satisfy Entity Framework. This project aims to show how to use  [`JointCollectionFacade.cs`](https://github.com/ZBAGI/EntityFramework-ManyToMany/blob/master/EntityFramework-ManyToMany/Relationship/JointCollectionFacade.cs) that will manage **joints entities** automatically. So dealing with many-to-many relationship is as easy as using `ICollection`.

Example uses MySQL provider for [EntityFrameworkCore](https://github.com/aspnet/EntityFrameworkCore) named [Pomelo](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) with [lazy loading proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/). To run it, just change [database](https://github.com/ZBAGI/EntityFramework-ManyToMany/blob/master/EntityFramework-ManyToMany/Database.cs) credentials.

### Lazy-loading
- Please notice that in order to populate Facade, joint entity with it's properties has to be included. In example [lazy-load proxies](https://github.com/ZBAGI/EntityFramework-ManyToMany/blob/master/EntityFramework-ManyToMany/Database.cs#L51) loading it automatically. Without proxies inclusion have to be done manually otherwise property `Roles` will be empty.
```csharp
DbSet<User> Users = DatabaseContext.Accounts;
var usersWithRoles = Users.Include(u => u.UserRoleJoints).ThenInclude(j => j.Role).ToList();
```

 - Without lazy-loading [`virtual` reference on Joint entity properties](https://github.com/ZBAGI/EntityFramework-ManyToMany/blob/master/EntityFramework-ManyToMany/Models/UserRoleJoint.cs) is not needed. Its `virtual` to allow proxies to do their magic.

### OData implementation
In order to correctly `$expand` collection facade i recommend to use [DTO's](https://en.wikipedia.org/wiki/Data_transfer_object) with [AutoMapper](https://github.com/AutoMapper/AutoMapper) which will help with loading facade without exposing joint entity in EDM model.

Here is how to [create `EdmModel`](http://odata.github.io/WebApi/#02-04-convention-model-builder) for `DTO`:
```csharp
var builder = new ODataConventionModelBuilder();
var userType= builder.AddEntityType(UserDTO);
userType.Name = nameof(User); // I like to hide the fact it is DTO, at the end every entity in EDM should be DTO so adding sufix into every entry is pointless.
builder.AddEntitySet("Users", userType);
```
Here is example of [AutoMapper config](https://automapper.readthedocs.io/en/latest/Getting-started.html#where-do-i-configure-automapper):
```csharp
new MapperConfiguration(cfg =>
{
    cfg.CreateMap<User, UserDto>()
        .ForMember(dto => dto.Roles,
            opt => opt.MapFrom(a => a.UserRoleJoints.Select(j => j.Role).AsQueryable()));
});
```
And finally the controller:
```csharp
[EnableQuery]
public IEnumerable<UserDto> Get()
{
    DbSet<User> users = Database.Users;
    return Mapper.ProjectTo<UserDto>(users);
} 
```
