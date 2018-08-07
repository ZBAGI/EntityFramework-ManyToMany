# EntityFramework Many-To-Many relationship

As of now EntityFramework doas not support **Many-To-Many** relationship. In order to achieve as us relation **join entity** is needed. That creates uncoforable sytuation in which you have to constantly deal with aditional 'middleman' entity that usually serves no other porpues then satisfy Entity Framework. This project aims to show how you can use  [`JointCollectionFacade.cs`](https://github.com/ZBAGI/EntityFramework-ManyToMany/blob/master/EntityFramework-ManyToMany/Relationship/IJointEntity.cs) that will manage **joints entities** for you. So dealing with many to many relationship is as easy as dealing with `ICollection`.

Example uses MySQL provider for Entity Framework Core named [Pomelo](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) with lazy loading. To run it just change [database](https://github.com/ZBAGI/EntityFramework-ManyToMany/blob/master/EntityFramework-ManyToMany/Database.cs) credentials.


**Please notice** that this idea was taken from some article that i cannot find anymore (feel free to send me message if you know where it is). I have modified it for easier usage and added support for lazy loading.
