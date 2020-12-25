# TODOs-API
A TODO API with a JSON Web Tokens Authentication and Authorization.

## Controllers.

There are total 2 Controllers (TasksController and UserController) which are responsible for Tasks routes and User route (which are Register and Login) respectively.

### User Controller
- **Register Route**
> Checks for any duplicate user, if exists sends back an error else creates a row in the database with **hashed password**
- **Login Route**
> Checks if a user exists with the provided username, if it does, the passwords are compared and a Json Web Token is sent back to the client which is then required by every route in **TasksController**.

### Tasks Controller
Includes all Task related Actions and **CRUD** operations that can be applied to them, there is also support for pagination when fetching all tasks

## JWT Details
JWT default bearer scheme is used to authentication a token in startup.cs
JWT when issued, gets expires after 2 hours.
