using System.Runtime.Serialization;

namespace StudentApi.Enums;

public enum MaritalStatus
{
    [EnumMember(Value = "Single")]
    Single,
    [EnumMember(Value = "Married")]
    Married
}
