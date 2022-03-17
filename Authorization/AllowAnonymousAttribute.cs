using System;

namespace DevConsulting.RegistrationLoginApi.Client.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }
}