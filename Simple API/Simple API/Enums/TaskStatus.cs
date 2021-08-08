using System.Runtime.Serialization;

namespace Simple_API.Enums
{
    public enum TaskStatus
    {
        //EnumMember used for show value in json response instead of enum id
        //[EnumMember(Value = "Not Completed")]
        NotCompleted = 0,
        //EnumMember used for show value in json response instead of enum id
        //[EnumMember(Value = "Completed")]
        Completed = 1
    }
}
