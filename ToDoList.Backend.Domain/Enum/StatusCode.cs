namespace ToDoList.Backend.Domain.Enum
{
    public enum StatusCode
    {
        TaskIsHasAlready = 01,
        TaskIsNotFound = 02,

        Ok = 200,
        InternalServerError = 500,
    }
}
