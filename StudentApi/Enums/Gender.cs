using System.Runtime.Serialization;

namespace StudentApi.Enums;

public enum Gender
{
    [EnumMember(Value = "Male")]
    Male,
    [EnumMember(Value = "Female")]
    Female
}
